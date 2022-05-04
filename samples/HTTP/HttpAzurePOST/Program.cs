﻿//
// Copyright (c) 2017 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Json;
using nanoFramework.Networking;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Device.Gpio;
using System.Net.Http;

#if BUILD_FOR_ESP32
using System.Device.Wifi;
#endif

namespace HttpSamples.HttpAzurePOST
{
    public class Program
    {
#if BUILD_FOR_ESP32
        private static string MySsid = "ssid";
        private static string MyPassword = "password";      
#endif

        private static AutoResetEvent sendMessage = new AutoResetEvent(false);
        private static int temperature;
        private static GpioPin _userButton;
        private static HttpClient _httpClient;

        private static Random _random = new Random();
        private static Temp _temp = new Temp();

        public static void Main()
        {
            Debug.WriteLine("Waiting for network up and IP address...");
            bool success;
            CancellationTokenSource cs = new(60000);
#if BUILD_FOR_ESP32
            success = WifiNetworkHelper.ConnectDhcp(MySsid, MyPassword, requiresDateTime: true, token: cs.Token);
#else
            success = NetworkHelper.SetupAndConnectNetwork(cs.Token, true);
#endif
            if (!success)
            {
                Debug.WriteLine($"Can't get a proper IP address and DateTime, error: {WifiNetworkHelper.Status}.");
                if (WifiNetworkHelper.HelperException != null)
                {
                    Debug.WriteLine($"Exception: {WifiNetworkHelper.HelperException}");
                }
                return;
            }

            // setup user button
            _userButton = new GpioController().OpenPin(0, PinMode.Input);
            _userButton.ValueChanged += UserButton_ValueChanged;

            // crate HTTP Client
            _httpClient = new HttpClient();
            _httpClient.SslProtocols = System.Net.Security.SslProtocols.Tls12;


            /////////////////////////////////////////////////////////////////////////////////////
            ////add certificate in PEM format(as a string in the app). This was taken from Azure's sample at https://github.com/Azure/azure-iot-sdk-c/blob/master/certs/certs.c
            /// chis cert should be used when connecting to Azure IoT on the Azure Cloud available globally. Additional certs can be found in the link above
            _httpClient.HttpsAuthentCert = new X509Certificate(azurePEMCertBaltimore);
            /////////////////////////////////////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////////////////////////////////
            /// enter your Shared Access Signature. You can create this using the Azure IOT Explorer under "Device Identity" format is
            /// "SharedAccessSignature sr=<iotHubName>.azure-devices.net%2Fdevices%2F<deviceName>&sig=<signature>"
            /// See "Readme" for more details
            string sas = "<Enter SAS Token Here See Read Me for example>";
            ///////////////////////////////////////////////////////////////////////////////////
            
            // add an Authorization header of our SAS
            _httpClient.DefaultRequestHeaders.Add("Authorization", sas);

            //strip the device iotHubName from the SAS token
            var from = sas.IndexOf('=') + 1;
            var to = sas.IndexOf(".");
            string iotHubName = sas.Substring(from, to - from);
            // strip the deviceName from the SAS
            to = sas.IndexOf("&");
            from = sas.IndexOf("%2Fdevices%2F") + 13;
            string deviceName = sas.Substring(from, to - from);

            //Create the url for the azure iot hub

            //Create the url for the azure iot hub
            _httpClient.BaseAddress = new Uri($"https://{iotHubName}.azure-devices.net/devices/{deviceName}/messages/events?api-version=2020-03-13");

            Debug.WriteLine($"Your IOT Hub Name: {iotHubName} and your Device Name: {deviceName}");
            Debug.WriteLine($"Sending data to: {_httpClient.BaseAddress.AbsoluteUri}");

            //Set the Device Id from our temp class
            _temp.DeviceID = deviceName;

            WorkerThread();

            // A custom class that we will convert to Json and pass as the body of the post
            Thread.Sleep(Timeout.Infinite);
        }

        private static void WorkerThread()
        {
            while (true)
            {
                //wait for button press to do this just ground the pin set above (Pin 0 in this sample)
                sendMessage.WaitOne();

                Debug.WriteLine("Button Clicked");

                //we have device name now set the new temperature
                _temp.Temperature = temperature.ToString();

                // convert the custom class into a serialized Json Object then convert to byte array.
                // you must pass in the content length of the body
                string output = JsonConvert.SerializeObject(_temp);

                // perform the request via HTTP Client
                StringContent content = new StringContent(output);

                var response = _httpClient.Post("", content);
                response.EnsureSuccessStatusCode();

                Debug.WriteLine($"The following data was POST to Azure IOT: {output}");
            }
        }

        private static void UserButton_ValueChanged(object sender, PinValueChangedEventArgs e)
        {
            if (e.ChangeType == PinEventTypes.Falling)
            {
                // user button pressed, generate a random temperature value
                temperature = _random.Next(10);

                // signal event
                sendMessage.Set();
            }
        }

        // X509 RSA key PEM format
        private const string azurePEMCertBaltimore =
@"-----BEGIN CERTIFICATE-----
MIIDdzCCAl+gAwIBAgIEAgAAuTANBgkqhkiG9w0BAQUFADBaMQswCQYDVQQGEwJJ
RTESMBAGA1UEChMJQmFsdGltb3JlMRMwEQYDVQQLEwpDeWJlclRydXN0MSIwIAYD
VQQDExlCYWx0aW1vcmUgQ3liZXJUcnVzdCBSb290MB4XDTAwMDUxMjE4NDYwMFoX
DTI1MDUxMjIzNTkwMFowWjELMAkGA1UEBhMCSUUxEjAQBgNVBAoTCUJhbHRpbW9y
ZTETMBEGA1UECxMKQ3liZXJUcnVzdDEiMCAGA1UEAxMZQmFsdGltb3JlIEN5YmVy
VHJ1c3QgUm9vdDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAKMEuyKr
mD1X6CZymrV51Cni4eiVgLGw41uOKymaZN+hXe2wCQVt2yguzmKiYv60iNoS6zjr
IZ3AQSsBUnuId9Mcj8e6uYi1agnnc+gRQKfRzMpijS3ljwumUNKoUMMo6vWrJYeK
mpYcqWe4PwzV9/lSEy/CG9VwcPCPwBLKBsua4dnKM3p31vjsufFoREJIE9LAwqSu
XmD+tqYF/LTdB1kC1FkYmGP1pWPgkAx9XbIGevOF6uvUA65ehD5f/xXtabz5OTZy
dc93Uk3zyZAsuT3lySNTPx8kmCFcB5kpvcY67Oduhjprl3RjM71oGDHweI12v/ye
jl0qhqdNkNwnGjkCAwEAAaNFMEMwHQYDVR0OBBYEFOWdWTCCR1jMrPoIVDaGezq1
BE3wMBIGA1UdEwEB/wQIMAYBAf8CAQMwDgYDVR0PAQH/BAQDAgEGMA0GCSqGSIb3
DQEBBQUAA4IBAQCFDF2O5G9RaEIFoN27TyclhAO992T9Ldcw46QQF+vaKSm2eT92
9hkTI7gQCvlYpNRhcL0EYWoSihfVCr3FvDB81ukMJY2GQE/szKN+OMY3EU/t3Wgx
jkzSswF07r51XgdIGn9w/xZchMB5hbgF/X++ZRGjD8ACtPhSNzkE1akxehi/oCr0
Epn3o0WC4zxe9Z2etciefC7IpJ5OCBRLbf1wbWsaY71k5h+3zvDyny67G7fyUIhz
ksLi4xaNmjICq44Y3ekQEe5+NauQrz4wlHrQMz2nZQ/1/I6eYs9HRCwBXbsdtTLS
R9I4LtD+gdwyah617jzV/OeBHRnDJELqYzmp
-----END CERTIFICATE-----";
    }
}
