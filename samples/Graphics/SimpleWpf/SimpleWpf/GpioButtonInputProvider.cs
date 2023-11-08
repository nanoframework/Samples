// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Device.Gpio;
using nanoFramework.UI.Input;
using nanoFramework.UI.Threading;
using nanoFramework.Presentation;


namespace SimpleWPF
{
    /// <summary>
    /// Uses the hardware provider to get the pins for handling button input.
    /// </summary>
    public sealed class GPIOButtonInputProvider
    {
        public readonly Dispatcher Dispatcher;

        //private ButtonPad[] buttons;
        private ArrayList buttons;
        private DispatcherOperationCallback callback;
        private InputProviderSite site;
        private PresentationSource source;
        private readonly GpioController Gpio = new GpioController();

        /// <summary>
        /// Maps GPIOs to Buttons that can be processed by 
        /// nanoFramework.Presentation.
        /// </summary>
        /// <param name="source"></param>
        public GPIOButtonInputProvider(PresentationSource source)
        {
            // Set the input source.
            this.source = source;

            // Register our object as an input source with the input manager and 
            // get back an InputProviderSite object which forwards the input 
            // report to the input manager, which then places the input in the 
            // staging area.
            site = InputManager.CurrentInputManager.RegisterInputProvider(this);

            // Create a delegate that refers to the InputProviderSite object's 
            // ReportInput method.
            callback = new DispatcherOperationCallback(delegate (object report)
                {
                    InputReportArgs args = (InputReportArgs)report;
                    return site.ReportInput(args.Device, args.Report);
                });

            Dispatcher = Dispatcher.CurrentDispatcher;

            this.buttons = new ArrayList();
        }

        /// <summary>
        /// Add a GPIO pin as a specific Button
        /// </summary>
        /// <param name="gpioPinNumber">GPIO pin number</param>
        /// <param name="button">Button that this pin represents</param>
        /// <param name="internalPullup">If true will enable the internal pull up on pin ( SetDriveMode = InputPullUp )
        public void AddButton(int gpioPinNumber, Button button, bool internalPullup)
        {
            GpioPin pin = Gpio.OpenPin(gpioPinNumber);


            pin.SetPinMode(internalPullup ? PinMode.InputPullUp : PinMode.Input);
            pin.DebounceTimeout = new TimeSpan(0, 0, 0, 0, 50);

            this.buttons.Add(new ButtonPad(this, button, pin));
        }

        /// <summary>
        /// Represents a button pad on the emulated device, containing five 
        /// buttons for user input. 
        /// </summary>
        internal class ButtonPad : IDisposable
        {
            private readonly Button button;
            private readonly GPIOButtonInputProvider sink;
            private readonly ButtonDevice buttonDevice = InputManager.CurrentInputManager.ButtonDevice;

            /// <summary>
            /// Constructs a ButtonPad object that handles the  
            /// hardware's button interrupts.
            /// </summary>
            /// <param name="sink"></param>
            /// <param name="button"></param>
            /// <param name="pin"></param>
            public ButtonPad(GPIOButtonInputProvider sink, Button button, GpioPin pin)
            {
                this.sink = sink;
                this.button = button;
                pin.ValueChanged += Pin_ValueChanged;
            }

            private void Pin_ValueChanged(object sender, PinValueChangedEventArgs e)
            {
                RawButtonActions action = (e.ChangeType == PinEventTypes.Falling) ? RawButtonActions.ButtonUp : RawButtonActions.ButtonDown;

                // Create a time, should be from the pin_ValueChanged event.
                DateTime time = DateTime.UtcNow;

                RawButtonInputReport report = new RawButtonInputReport(sink.source, time, button, action);

                // Queue the button press to the input provider site.
                sink.Dispatcher.BeginInvoke(sink.callback, new InputReportArgs(buttonDevice, report));
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

        }
    }
}
