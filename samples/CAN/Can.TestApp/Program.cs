using nanoFramework.Devices.Can;
using System;
using System.Threading;

namespace Can.TestApp
{
    public class Program
    {
        static CanController CanController1;
        static CanController CanController2;
        public static void Main()
        {
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
                    Console.WriteLine("*** No more message available!!!");
                    break;
                }

                // new message available, output message
                if (msg.Message != null)
                {
                    Console.Write($"Message on {canCtl.ControllerId}: ");
                    for (int i = 0; i < msg.Message.Length; i++)
                    {
                        Console.Write(msg.Message[i].ToString("X2"));
                    }
                }
                Console.WriteLine("");
            }
        }
    }
}
