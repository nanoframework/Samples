//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using Amqp;
using Amqp.Framing;
using nanoFramework.Networking;
using System;
using System.Text;
using System.Threading;
using Windows.Devices.Gpio;
using AmqpTrace = Amqp.Trace;

namespace AmqpSamples.AzureSB.Sender
{
    public class Program
    {
        private static string hubNamespace = "<replace-with-servicehub-name>";
        private static string keyName = "<replace-with-servicehub-key-name>";
        private static string keyValue = "<replace-with-servicehub-key-value>";
        private static string entity = "<replace-with-servicehub-entity-name>";

        private static int temperature;
        private static AutoResetEvent sendMessage = new AutoResetEvent(false);

        private static GpioPin _userButton;

        private static Random _random = new Random();

        public static void Main()
        {
            // setup and connect network
            NetworkHelpers.SetupAndConnectNetwork(true);

            // wait for network and valid system date time
            NetworkHelpers.IpAddressAvailable.WaitOne();
            NetworkHelpers.DateTimeAvailable.WaitOne();

            // setup user button
            // F769I-DISCO -> USER_BUTTON is @ PA0 -> (0 * 16) + 0 = 0
            _userButton = GpioController.GetDefault().OpenPin(0);
            _userButton.SetDriveMode(GpioPinDriveMode.Input);
            _userButton.ValueChanged += UserButton_ValueChanged;

            // setup AMQP
            // set trace level 
            AmqpTrace.TraceLevel = TraceLevel.Frame | TraceLevel.Verbose;
            
            // enable trace
            AmqpTrace.TraceListener = WriteTrace;
            
            // disable server certificate validation
            Connection.DisableServerCertValidation = true;

            // launch worker thread
            new Thread(WorkerThread).Start();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void WorkerThread()
        {
            // need to wait for network connection...
            NetworkHelpers.IpAddressAvailable.WaitOne();

            // ... and valid date time
            NetworkHelpers.DateTimeAvailable.WaitOne();

            // establish connection
            Connection connection = new Connection(new Address(hubNamespace, 5672, keyName, keyValue));

            // create session
            Session session = new Session(connection);

            // create a sender link
            SenderLink sender = new SenderLink(session, "sender-nanoframework", entity);

            while (true)
            {
                // wait for button press
                sendMessage.WaitOne();

                // compose message 
                // json format like: { temp: 23.67 }
                Message message = new Message()
                {
                    BodySection = new Data() { Binary = Encoding.UTF8.GetBytes($"{{ temp: {temperature.ToString("D2")} }}") }
                };

                message.Properties = new Properties() { GroupId = "F769I-DISCO" };

                // send message
                sender.Send(message);
            }

            // the loop won't reach here, just kept here for the ceremony
            sender.Close();
            session.Close();
            connection.Close();
        }

        private static void UserButton_ValueChanged(object sender, GpioPinValueChangedEventArgs e)
        {
            if (e.Edge == GpioPinEdge.FallingEdge)
            {
                // user button pressed, generate a random temperature value
                temperature = _random.Next(50);

                // signal event
                sendMessage.Set();
            }
        }

        static void WriteTrace(TraceLevel level, string format, params object[] args)
        {
            Console.WriteLine(Fx.Format(format, args));
        }
    }
}
