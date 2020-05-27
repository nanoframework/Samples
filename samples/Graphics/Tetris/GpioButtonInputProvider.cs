using nanoFramework.Presentation;
using nanoFramework.UI;
using nanoFramework.UI.Input;
using nanoFramework.UI.Threading;
using System;
using Windows.Devices.Gpio;

namespace Tetris
{
    // This class dispatches input events from emulated GPIO pins (0-4) to Input.Button 
    // events. It is specific to the SDK's sample emulator; if you use this code,
    // please update this class to reflect the design of your hardware.
    public sealed class GpioButtonInputProvider
    {
        public readonly Dispatcher Dispatcher;

        private ButtonPad[] buttons;
        private ReportInputCallback callback;
        private InputProviderSite site;
        private PresentationSource source;
        private readonly GpioController Gpio = GpioController.GetDefault();
        private ButtonDevice buttonDevice = null;


        private delegate bool ReportInputCallback(InputReport inputReport);

        // This class maps GPIOs to Buttons processable by nanoFramework.UI.Presentation
        public GpioButtonInputProvider(PresentationSource source)
        {
            // Set the input source.
            this.source = source;
            // Register our object as an input source with the input manager and get back an
            // InputProviderSite object which forwards the input report to the input manager,
            // which then places the input in the staging area.
            site = InputManager.CurrentInputManager.RegisterInputProvider(this);
            // Create a delegate that refers to the InputProviderSite object's ReportInput method
            callback = new ReportInputCallback(site.ReportInput);
            Dispatcher = Dispatcher.CurrentDispatcher;


            GpioPin pinLeft = Gpio.OpenPin(1);
            GpioPin pinRight = Gpio.OpenPin(2);
            GpioPin pinUp = Gpio.OpenPin(3);
            GpioPin pinSelect = Gpio.OpenPin(4);
            GpioPin pinDown = Gpio.OpenPin(5);


            // Allocate button pads and assign the (emulated) hardware pins as input 
            // from specific buttons.
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

        // The emulated device provides a button pad containing five buttons 
        // for user input. This class represents the button pad.
        internal class ButtonPad
        {
            private Button button;
            private GpioButtonInputProvider sink;

            // Construct the object. Set this class to handle the emulated 
            // hardware's button interrupts.
            public ButtonPad(GpioButtonInputProvider sink, Button button, GpioPin pin)
            {
                this.sink = sink;
                this.button = button;
                pin.SetDriveMode(GpioPinDriveMode.Input);
                pin.ValueChanged += Pin_ValueChanged;
            }

            private void Pin_ValueChanged(object sender, GpioPinValueChangedEventArgs e)
            {
                RawButtonActions action = (e.Edge == GpioPinEdge.FallingEdge) ? RawButtonActions.ButtonUp : RawButtonActions.ButtonDown;

                DateTime time = DateTime.UtcNow;
                RawButtonInputReport report = new RawButtonInputReport(sink.source, time, button, action);

                // Queue the button press to the input provider site.
                sink.Dispatcher.BeginInvoke(sink.callback, new InputReportArgs(buttonDevice, report));
            }
        }
    }
}


