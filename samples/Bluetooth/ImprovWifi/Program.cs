using System;
using System.Threading;
using System.Net;

/// <summary>
/// Improv provisioning sample
/// For details about Improv protocol and to try provisioning Wifi credentials from a web page 
/// See: https://www.improv-wifi.com/
/// Also has instructions on how to embed into your web page.
/// </summary>

namespace ImprovWifi
{
    public class Program
    {
        static Improv _imp;

        public static void Main()
        {
            Console.WriteLine("Example of using IMPROV bluetooth LE for Wifi provisioning");

            // Construct Improv class
            _imp = new Improv();

            // This optional event will be fired if asked to identify device
            // You can flash an LED or some other method.
            _imp.OnIdentify += Imp_OnIdentify;

            // This optional event is called when the provisioning is completed and Wifi is connected but before
            // improv has informed Improv client of result. This allows user to set the provision URL redirect with correct IP address 
            // See event handler
            _imp.OnProvisioningComplete += Imp_OnProvisioningComplete;

            // This optional event will be called to do the Wifi provisioning in user program.
            // if not set then improv class will automatically try to connect to Wifi 
            // For this sample we will let iprov do it, uncomment next line to try user event. See event handler
            // imp.OnProvisioned += Imp_OnProvisioned;

            // Start IMPROV service to start advertising using provided device name.
            _imp.Start("Improv sample");

            // You may need a physical button to be pressed to authorise the provisioning (security)
            // Wait for button press and call Authorise method
            // For out test we will just Authorise
            _imp.Authorise(true);

            Console.WriteLine("Waiting for device to be provisioned");

            // Now wait for Device to be Provisioned
            // we could also just use the OnProvisioningComplete event
            while (_imp.CurrentState != Improv.ImprovState.provisioned)
            {
                Thread.Sleep(500);
            }

            Console.WriteLine("Device has been provisioned");

            // We are now provisioned and connected to Wifi, so stop bluetooth service to release resources.
            _imp.Stop();
            _imp = null;
            ;

            // Start our very simple web page server to pick up the redirect we gave
            Console.WriteLine("Starting simple web server");
            SimpleWebListener();

            Thread.Sleep(Timeout.Infinite);
        }

        /// <summary>
        /// Event handler for OnProvisioningComplete event
        /// </summary>
        /// <param name="sender">Improv instance</param>
        /// <param name="e">Not used</param>
        private static void Imp_OnProvisioningComplete(object sender, EventArgs e)
        {
            SetProvisioningURL();
        }

        /// <summary>
        /// Set URL with current IP address
        /// The Improv client will redirect to this URL if set.
        /// </summary>
        private static void SetProvisioningURL()
        {
            // All good, wifi connected, set up URL for access
            _imp.RedirectUrl = "http://" + _imp.GetCurrentIPAddress() + "/start.htm";
        }

        private static void Imp_OnProvisioned(object sender, ProvisionedEventArgs e)
        {
            string ssid = e.Ssid;
            string password = e.Password;

            Console.WriteLine("Provisioning device");

            Console.WriteLine("Connecting to Wifi...");

            // Try to connect to Wifi AP
            // use improv internal method
            if (_imp.ConnectWiFi(ssid, password))
            {
                Console.WriteLine("Connected to Wifi");

                SetProvisioningURL();
            }
            else
            {
                Console.WriteLine("Failed to Connect to Wifi!");

                // if not successful set error and return
                _imp.ErrorState = Improv.ImprovError.unableConnect;
            }
        }

        private static void Imp_OnIdentify(object sender, EventArgs e)
        {
            // Flash LED to Identify device or do nothing
            Console.WriteLine("Flashing LED...");
        }

        private static void SimpleWebListener()
        {
            // set-up our HTTP response
            string responseString =
                "<HTML><BODY>" +
                "<h2>Hello from nanoFramework</h2>" +
                "<p>We are a newly provisioned device using <b>Improv</b> over Bluetooth.</p>" +
                "<p>See <a href='https://www.improv-wifi.com'>Improv web site</a> for details" +
                "</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            // Create a listener.
            HttpListener listener = new("http", 80);

            listener.Start();

            while (true)
            {
                try
                {
                    // Now wait on context for a connection
                    HttpListenerContext context = listener.GetContext();

                    Console.WriteLine("Web request received");

                    // Get the response stream
                    HttpListenerResponse response = context.Response;

                    // Write reply
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);

                    // output stream must be closed
                    context.Response.Close();

                    Console.WriteLine("Web response sent");

                    // context must be closed
                    context.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("* Error getting context: " + ex.Message + "\r\nSack = " + ex.StackTrace);
                }
            }
        }
    }
}

