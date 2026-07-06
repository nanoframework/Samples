//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Device.Adc;
using System.Threading;
using Iot.Device.Button;

namespace Esp32S3BoxLite
{
    /// <summary>
    /// Exposes the three navigation buttons of the ESP32-S3-BOX-Lite (Previous,
    /// Enter, Next) as <see cref="ButtonBase"/> instances from the
    /// nanoFramework.Iot.Device.Button NuGet.
    ///
    /// IMPORTANT: on the ESP32-S3-BOX-Lite these three buttons are NOT wired to
    /// individual GPIOs. They form a resistor ladder connected to a single analog
    /// input: ADC1 channel 0, which is GPIO1 (from the esp-box-lite BSP). Each
    /// button pulls that pin to a distinct voltage, so they are read with the ADC
    /// and distinguished by voltage windows. That's why a per-pin GpioButton can't
    /// be used here; instead a small poller drives the button state machine.
    /// </summary>
    public class BoxLiteButtons : IDisposable
    {
        // ADC1 channel 0 == GPIO1 on the ESP32-S3-BOX-Lite (esp-box-lite BSP).
        private const int AdcChannelNumber = 0;

        // Approximate ADC full-scale voltage. The ESP32 ADC defaults to ~11 dB
        // attenuation (~0-3.3 V full scale). Adjust if your readings are off.
        private const int ReferenceMillivolts = 3300;

        // How often the ADC is sampled to detect presses.
        private const int PollIntervalMs = 20;

        // Number of raw ADC samples taken per poll. The median of these is used,
        // which rejects the occasional spike from the noisy resistor ladder.
        private const int SamplesPerRead = 8;

        // A detected state must be seen this many polls in a row before it is
        // committed. This debounces transitions (e.g. voltages briefly passing
        // through another button's window while a key is pressed or released).
        private const int StableCountToCommit = 2;

        private readonly AdcController _adcController;
        private readonly AdcChannel _channel;
        private readonly int _maxValue;
        private readonly AdcButton[] _buttons;
        private readonly Thread _pollThread;

        // Debounce state: index of the matched button (-1 == none).
        private int _committedIndex = -1;
        private int _candidateIndex = -1;
        private int _candidateCount;

        private bool _running;
        private bool _disposed;

        /// <summary>
        /// Gets the "Previous" navigation button (~2410 mV).
        /// </summary>
        public AdcButton Previous { get; }

        /// <summary>
        /// Gets the "Enter" navigation button (~1980 mV).
        /// </summary>
        public AdcButton Enter { get; }

        /// <summary>
        /// Gets the "Next" navigation button (~820 mV).
        /// </summary>
        public AdcButton Next { get; }

        /// <summary>
        /// Initializes the ADC and starts polling the button ladder.
        /// </summary>
        public BoxLiteButtons()
        {
            // Voltage windows (min/max in millivolts) from the esp-box-lite BSP.
            Previous = new AdcButton(2310, 2510); // middle ~2410 mV
            Enter = new AdcButton(1880, 2080);    // middle ~1980 mV
            Next = new AdcButton(720, 920);       // middle ~820 mV
            _buttons = new AdcButton[] { Previous, Enter, Next };

            _adcController = new AdcController();
            _channel = _adcController.OpenChannel(AdcChannelNumber);
            _maxValue = _adcController.MaxValue;

            _running = true;
            _pollThread = new Thread(PollLoop);
            _pollThread.Start();
        }

        private void PollLoop()
        {
            while (_running)
            {
                int millivolts = ReadMillivolts();
                int matched = MatchIndex(millivolts);

                // Count how long the same reading persists.
                if (matched == _candidateIndex)
                {
                    if (_candidateCount < StableCountToCommit)
                    {
                        _candidateCount++;
                    }
                }
                else
                {
                    _candidateIndex = matched;
                    _candidateCount = 1;
                }

                // Only change the button state once the reading is stable and it
                // actually differs from what is already committed.
                if (_candidateCount >= StableCountToCommit && matched != _committedIndex)
                {
                    // The buttons share one analog pin, so you can never move
                    // directly from one button to another: a real press always
                    // starts from the idle (no-match) state, and a release always
                    // returns to it. While a key is released the voltage sweeps
                    // through neighbouring windows; those readings must be ignored.
                    bool isRelease = matched == -1;
                    bool pressFromIdle = matched >= 0 && _committedIndex == -1;

                    if (isRelease || pressFromIdle)
                    {
                        _committedIndex = matched;
                        for (int i = 0; i < _buttons.Length; i++)
                        {
                            _buttons[i].Update(i == matched);
                        }
                    }
                }

                Thread.Sleep(PollIntervalMs);
            }
        }

        /// <summary>
        /// Reads the ADC several times and returns the median value converted to
        /// millivolts. The median filters out transient spikes.
        /// </summary>
        private int ReadMillivolts()
        {
            int[] samples = new int[SamplesPerRead];
            for (int i = 0; i < SamplesPerRead; i++)
            {
                samples[i] = _channel.ReadValue();
            }

            // Insertion sort (small array) then take the middle element.
            for (int i = 1; i < samples.Length; i++)
            {
                int key = samples[i];
                int j = i - 1;
                while (j >= 0 && samples[j] > key)
                {
                    samples[j + 1] = samples[j];
                    j--;
                }

                samples[j + 1] = key;
            }

            int rawMedian = samples[samples.Length / 2];
            return _maxValue > 0 ? (rawMedian * ReferenceMillivolts / _maxValue) : 0;
        }

        /// <summary>
        /// Returns the index of the button whose voltage window contains the
        /// measured value, or -1 if none match.
        /// </summary>
        private int MatchIndex(int millivolts)
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (_buttons[i].Matches(millivolts))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _running = false;
            _pollThread?.Join();

            _channel?.Dispose();

            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// A <see cref="ButtonBase"/> whose pressed state is driven by an ADC voltage
    /// window rather than a GPIO pin. Subscribe to the inherited events
    /// (<see cref="ButtonBase.Press"/>, <see cref="ButtonBase.DoublePress"/>,
    /// <see cref="ButtonBase.Holding"/>, etc.).
    /// </summary>
    public class AdcButton : ButtonBase
    {
        private readonly int _minMillivolts;
        private readonly int _maxMillivolts;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdcButton"/> class.
        /// </summary>
        /// <param name="minMillivolts">Lower bound of the voltage window that means "pressed".</param>
        /// <param name="maxMillivolts">Upper bound of the voltage window that means "pressed".</param>
        public AdcButton(int minMillivolts, int maxMillivolts)
        {
            _minMillivolts = minMillivolts;
            _maxMillivolts = maxMillivolts;
        }

        /// <summary>
        /// Returns true when the measured voltage falls in this button's window.
        /// </summary>
        /// <param name="millivolts">The measured voltage in millivolts.</param>
        public bool Matches(int millivolts)
        {
            return millivolts >= _minMillivolts && millivolts <= _maxMillivolts;
        }

        /// <summary>
        /// Feeds the current pressed state into the button state machine, raising
        /// the button events on transitions.
        /// </summary>
        /// <param name="pressed">True if the button is currently pressed.</param>
        public void Update(bool pressed)
        {
            if (pressed && !IsPressed)
            {
                HandleButtonPressed();
            }
            else if (!pressed && IsPressed)
            {
                HandleButtonReleased();
            }
        }
    }
}
