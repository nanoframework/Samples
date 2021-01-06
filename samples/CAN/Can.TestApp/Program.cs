using nanoFramework.Devices.Can;
using System;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.Gpio;

namespace Can.TestApp
{
    public class Program
    {
        static GpioPin _led;

        static CanController CanController1;
        static CanController CanController2;
        public static void Main()
        {
            // PJ5 is LD2 in STM32F769I_DISCO
            //_led = GpioController.GetDefault().OpenPin(PinNumber('J', 5));
            // PG14 is LEDLD4 in F429I_DISCO
            //_led = GpioController.GetDefault().OpenPin(PinNumber('G', 14));
            // PD13 is LED3 in DISCOVERY4
            _led = GpioController.GetDefault().OpenPin(PinNumber('D', 13));
            _led.SetDriveMode(GpioPinDriveMode.Output);

            // set settings for CAN controller
            CanSettings canSettings = new CanSettings(6, 8, 1, 0);

            // get controller for CAN1
            CanController1 = CanController.FromId("CAN1", canSettings);
            // get controller for CAN2
            CanController2 = CanController.FromId("CAN2", canSettings);

            //CanController1.MessageReceived += CanController_DataReceived;
            CanController2.MessageReceived += CanController_DataReceived;

            while (true)
            {
                CanController1.WriteMessage(new CanMessage(0x01234567, CanMessageIdType.EID, CanMessageFrameType.Data, new byte[] { 0xCA, 0xFE }));
                //CanController2.WriteMessage(new CanMessage(0x01234567, false, true, new byte[] { 0xFE, 0xCA }));
                Thread.Sleep(2000);
            }
        }

        private static void CanController_DataReceived(object sender, CanMessageReceivedEventArgs e)
        {
            CanController canCtl = (CanController)sender;

            while (true)
            {
                // try get the next message
                var msg = canCtl.GetMessage();
 
                if(msg == null)
                {
                    Debug.WriteLine("*** No more message available!!!");
                    break;
                }

                // new message available, output message
                if (msg.Message != null)
                {
                    Debug.Write($"Message on {canCtl.ControllerId}: ");
                    for (int i = 0; i < msg.Message.Length; i++)
                    {
                        Debug.Write(msg.Message[i].ToString("X2"));
                    }

                    new Thread(BlinkLED).Start();
                }
                Debug.WriteLine("");
            }
        }

        static void BlinkLED()
        {
            // blink led for each message received
            _led.Write(GpioPinValue.High);
            Thread.Sleep(500);
            _led.Toggle();
        }

        static int PinNumber(char port, byte pin)
        {
            if (port < 'A' || port > 'J')
                throw new ArgumentException();

            return ((port - 'A') * 16) + pin;
        }
    }
}
