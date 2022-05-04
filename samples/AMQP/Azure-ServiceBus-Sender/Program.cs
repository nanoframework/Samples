//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using Amqp;
using Amqp.Framing;
using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Text;
using System.Threading;
using AmqpTrace = Amqp.Trace;

#if HAS_WIFI
using System.Device.Wifi;
#endif

namespace AmqpSamples.AzureSB.Sender
{
    public class Program
    {
        private static string hubNamespace = "<replace-with-servicehub-name>";
        private static string keyName = "<replace-with-servicehub-key-name>";
        private static string keyValue = "<replace-with-servicehub-key-value>";
        private static string entity = "<replace-with-servicehub-entity-name>";

#if HAS_WIFI
        private static string MySsid = "ssid";
        private static string MyPassword = "password";
#endif

        private static int temperature;
        private static AutoResetEvent sendMessage = new AutoResetEvent(false);

        private static GpioPin _userButton;

        private static Random _random = new Random();

        public static void Main()
        {
            Debug.WriteLine("Waiting for network up and IP address...");
            bool success;
            CancellationTokenSource cs = new(60000);
#if HAS_WIFI
            success = WifiNetworkHelper.ConnectDhcp(MySsid, MyPassword, requiresDateTime: true, token: cs.Token);
#else
            success = NetworkHelper.SetupAndConnectNetwork(cs.Token, true);
#endif
            if (!success)
            {
#if HAS_WIFI
                Debug.WriteLine($"Can't get a proper IP address and DateTime, error: {WifiNetworkHelper.Status}.");
                if (WifiNetworkHelper.HelperException != null)
                {
                    Debug.WriteLine($"Exception: {WifiNetworkHelper.HelperException}");
                }
#else
                Debug.WriteLine($"Can't get a proper IP address and DateTime, error: {NetworkHelper.Status}.");
                if (NetworkHelper.HelperException != null)
                {
                    Debug.WriteLine($"Exception: {NetworkHelper.HelperException}");
                }
#endif

                return;
            }

            // setup user button
            // F769I-DISCO -> USER_BUTTON is @ PA0 -> (0 * 16) + 0 = 0
            var _gpioController = new GpioController();
            _userButton = _gpioController.OpenPin(0, PinMode.Input);
            _userButton.ValueChanged += _userButton_ValueChanged;

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

        private static void _userButton_ValueChanged(object sender, PinValueChangedEventArgs e)
        {
            if (e.ChangeType == PinEventTypes.Falling)
            {
                // user button pressed, generate a random temperature value
                temperature = _random.Next(50);

                // signal event
                sendMessage.Set();
            }
        }

        static void WriteTrace(TraceLevel level, string format, params object[] args)
        {
            Debug.WriteLine(Fx.Format(format, args));
        }
    }
}
