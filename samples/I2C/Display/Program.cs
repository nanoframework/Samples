//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using Driver;
using System;
using System.Threading;
using Windows.Devices.Gpio;
using static Driver.STMPE811;

namespace I2CDemoApp
{
    public class Program
    {
        static STMPE811 _touchController;
        public static GpioPin _touchInterrupt;
        public static ManualResetEvent _touchEvent;

        public static void Main()
        {
            // create the event NOT signalled
            _touchEvent = new ManualResetEvent(false);

            // STMPE811 touchscreen controller in STM32F429 DISCOVERY board has default I2C address 1000001 = 0x41 ...
            // ... and it's connected to I2C3 bus
            _touchController = new STMPE811(0x41, "I2C3");

            // ... INT signal is connected to PA15 with a pull-up, setup the GPIO and an event handler to handle the device interrupt requests
            var gpioController = GpioController.GetDefault();
            _touchInterrupt = gpioController.OpenPin(PinNumber('A', 15));
            _touchInterrupt.SetDriveMode(GpioPinDriveMode.Input);
            _touchInterrupt.ValueChanged += TouchScreenInterruptRequest;

            // initialize STMPE811
            if (_touchController.Initialize())
            {
                // start the touch screen controller
                _touchController.Start();

                // in the F429I DISCOVERY the INT signal has a pull-up so we need to configure the interrupt polarity to active low
                _touchController.SetInterruptPolarity(InterruptPolarity.Low);
                // as for the type better use level to improve detection
                _touchController.SetInterruptType(InterruptType.Level);

                // enable interrupts from the FIFO and the touch controller
                _touchController.EnableInterruptSource(InterruptSource.FifoAboveThreshold | InterruptSource.Touch);
                _touchController.EnableGlobalInterrupt();

                // output the ID and revision of the device
                Console.WriteLine("ChipID " + _touchController.ChipID.ToString("X4"));
                Console.WriteLine("Rev " + _touchController.RevisionNumber.ToString());

                // launch touch tracking thread
                new Thread(new ThreadStart(TouchTracking)).Start();
            }
            else
            {
                // failed to init the device
                Console.WriteLine("***** FATAL ERROR: failed to initialise the touch screen controller *****");
            }

            // infinite loop to keep main thread active
            for (; ; )
            {
                Thread.Sleep(1000);
            }
        }

        public static void TouchScreenInterruptRequest(object sender, GpioPinValueChangedEventArgs e)
        {
            // get interrupt status from touch controller
            InterruptSource interruptStatus = _touchController.ReadGlobalInterruptStatus();

            if ((interruptStatus & InterruptSource.Touch) == InterruptSource.Touch)
            {
                // set the touch event
                _touchEvent.Set();
            }

            // clear interrupt flags
            _touchController.ClearGlobalInterrupt(interruptStatus);
        }

        public static void TouchTracking()
        {
            Reading touchReading;

            while (true)
            {
                // wait for the touch event
                _touchEvent.WaitOne();

                // disable all FIFO interrupts
                _touchController.DisableInterruptSource(InterruptSource.AllFIFOs);

                // get touch readings from FIFO until its empty
                while (_touchController.FifoSize() > 0)
                {
                    touchReading = _touchController.ReadTouch();

                    if (touchReading.IsValid)
                    {
                        Console.WriteLine("Touchscreen pressed @ (" + touchReading.X + "," + touchReading.Y + ")");
                    }
                }

                // enable back FIFO threshold interrupt
                _touchController.EnableInterruptSource(InterruptSource.FifoAboveThreshold);

                Console.WriteLine("Touchscreen released");

                // clear event
                _touchEvent.Reset();
            }
        }

        static int PinNumber(char port, byte pin)
        {
            if (port < 'A' || port > 'J')
                throw new ArgumentException();

            return ((port - 'A') * 16) + pin;
        }
    }
}
