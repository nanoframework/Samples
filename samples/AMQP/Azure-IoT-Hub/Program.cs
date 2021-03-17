//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using Amqp;
using nanoFramework.Networking;
using System;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.Gpio;
using AmqpTrace = Amqp.Trace;

namespace AmqpSamples.AzureIoTHub
{
    public class Program
    {
        /////////////////////////////////////////////////////////////////////
        // Azure IoT Hub settings

        // To make your life easier use a SaS token generated from Azure Device Explorer like the one bellow
        // HostName=contoso.azure-devices.net;DeviceId=COFEEMACHINE001;SharedAccessSignature=SharedAccessSignature sr=contoso.azure-devices.net%2Fdevices%2FCOFEEMACHINE001&sig=tGeAGJeRgFUqIKEs%2BtYNLmLAGWGHiHT%2F2TIIsu8oQ%2F0%3D&se=1234656789

        const string _hubName = "contoso";
        const string _deviceId = "COFEEMACHINE001";
        const string _sasToken = "SharedAccessSignature sr=contoso.azure-devices.net%2Fdevices%2FCOFEEMACHINE001&sig=tGeAGJeRgFUqIKEs%2BtYNLmLAGWGHiHT%2F2TIIsu8oQ%2F0%3D&se=1234656789";

        /////////////////////////////////////////////////////////////////////

        private static AutoResetEvent sendMessage = new AutoResetEvent(false);

        private static int temperature;

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
            _userButton = GpioController.GetDefault().OpenPin(3*16+7);
            _userButton.SetDriveMode(GpioPinDriveMode.Input);
            _userButton.ValueChanged += UserButton_ValueChanged;

            // setup AMQP
            // set trace level 
            AmqpTrace.TraceLevel = TraceLevel.Frame | TraceLevel.Information;

            // enable trace
            AmqpTrace.TraceListener = WriteTrace;

            /////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////
            // *** CHECK README for details on this configuration **** //
            Connection.DisableServerCertValidation = false;
            /////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////
            
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

            // parse Azure IoT Hub Map settings to AMQP protocol settings
            string hostName = _hubName + ".azure-devices.net";
            string userName = _deviceId + "@sas." + _hubName;
            string senderAddress = "devices/" + _deviceId + "/messages/events";
            string receiverAddress = "devices/" + _deviceId + "/messages/deviceBound";

            Connection connection = new Connection(new Address(hostName, 5671, userName, _sasToken));
            Session session = new Session(connection);
            SenderLink sender = new SenderLink(session, "send-link", senderAddress);
            ReceiverLink receiver = new ReceiverLink(session, "receive-link", receiverAddress);
            receiver.Start(100, OnMessage);

            while (true)
            {
                // wait for button press
                sendMessage.WaitOne();

                // compose message
                Message message = new Message();
                message.ApplicationProperties = new Amqp.Framing.ApplicationProperties();
                message.ApplicationProperties["temperature"] = temperature;

                // send message with temperature
                sender.Send(message, null, null);
            }

            // the loop won't reach here, just kept here for the ceremony
            sender.Close();
            session.Close();
            connection.Close();
        }

        private static void OnMessage(IReceiverLink receiver, Message message)
        {
            // command received 
            int setTemperature = (int)message.ApplicationProperties["settemp"];
            OnAction(setTemperature);
        }

        private static void OnAction(int setTemperature)
        {
            Debug.WriteLine($"received new temperature setting: {setTemperature}");
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
            Debug.WriteLine(Fx.Format(format, args));
        }
    }
}
