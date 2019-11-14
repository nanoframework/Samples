////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Microsoft Corporation.  All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using nanoFramework.UI;
using nanoFramework.UI.Input;
using nanoFramework.UI.Threading;
using nanoFramework.Presentation;

using System.Threading;
using nanoFramework.Runtime;
using nanoFramework.Runtime.Events;

using Windows.Devices.Gpio;


namespace SimpleWPF
{
    /// <summary>
    /// Uses the hardware provider to get the pins for handling button input.
    /// </summary>
    public sealed class GPIOButtonInputProvider
    {
        public readonly Dispatcher Dispatcher;

        private ButtonPad[] buttons;
        private DispatcherOperationCallback callback;
        private InputProviderSite site;
        private PresentationSource source;
        private readonly GpioController Gpio = GpioController.GetDefault();

        /// <summary>
        /// Maps GPIOs to Buttons that can be processed by 
        /// Microsoft.SPOT.Presentation.
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

            //--------------
            // Create a hardware provider.
            // HardwareProvider hwProvider = new HardwareProvider();


            //--------------

            // Create the pins that are needed for the buttons.  Default their 
            // values for the emulator.
            GpioPin pinLeft = Gpio.OpenPin(1);
            GpioPin pinRight = Gpio.OpenPin(2);
            GpioPin pinUp = Gpio.OpenPin(3);
            GpioPin pinSelect = Gpio.OpenPin(4);
            GpioPin pinDown = Gpio.OpenPin(5);


            // Allocate button pads and assign the (emulated) hardware pins as 
            // input from specific buttons.
            ButtonPad[] buttons = new ButtonPad[]
            {
                // Associate the buttons to the pins as discovered or set above
                new ButtonPad(this, Button.VK_LEFT  , pinLeft),
                new ButtonPad(this, Button.VK_RIGHT , pinRight),
                new ButtonPad(this, Button.VK_UP    , pinUp),
                new ButtonPad(this, Button.VK_SELECT, pinSelect),
                new ButtonPad(this, Button.VK_DOWN  , pinDown),
            };

            this.buttons = buttons;
        }

        /// <summary>
        /// Represents a button pad on the emulated device, containing five 
        /// buttons for user input. 
        /// </summary>
        internal class ButtonPad : IDisposable
        {
            private Button button;
            private GPIOButtonInputProvider sink;
            private ButtonDevice buttonDevice = null;

            /// <summary>
            /// Constructs a ButtonPad object that handles the emulated 
            /// hardware's button interrupts.
            /// </summary>
            /// <param name="sink"></param>
            /// <param name="button"></param>
            /// <param name="pin"></param>
            public ButtonPad(GPIOButtonInputProvider sink, Button button, GpioPin pin)
            {
                this.sink = sink;
                this.button = button;
                /// Do not set an InterruptPort with GPIO_NONE.
                // When this GPIO pin is true, call the Interrupt method.
                pin.SetDriveMode(GpioPinDriveMode.Input);
                pin.ValueChanged += Pin_ValueChanged;
            }
            private void Pin_ValueChanged(object sender, GpioPinValueChangedEventArgs e)
            {
                RawButtonActions action = (e.Edge == GpioPinEdge.FallingEdge) ? RawButtonActions.ButtonUp : RawButtonActions.ButtonDown;

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
                    //port.Dispose();
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
