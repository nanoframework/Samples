// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.

using nanoFramework.Azure.Devices.Client;
using nanoFramework.Azure.Devices.Shared;
using System.Diagnostics;
using System.Threading;

namespace CountMeasurement
{
    /// <summary>
    /// The main OTA class that will be loaded dynamically
    /// </summary>
    public static class OtaRunner
    {
        private const string UpdateTime = "UpdateTime";

        // Any of the private fields must be static as the class is NOT created
        private static CancellationTokenSource _cancellationTokenSource;
        private static DeviceClient _azureIot;
        private static int _updateTime = 60000;
        private static Thread _runer;
        private static long _count = 0;

        public static void Start(DeviceClient azureIot)
        {
            Debug.WriteLine("Start called");
            _cancellationTokenSource = new();
            _azureIot = azureIot;
            // Get the twins
            Debug.WriteLine("Getting twins");
            var twins = _azureIot.GetTwin(new CancellationTokenSource(10000).Token);
            Debug.WriteLine("Having twins");
            if ((twins != null) && (twins.Properties.Desired.Contains(UpdateTime)))
            {
                _updateTime = (int)twins.Properties.Desired[UpdateTime];
                Debug.WriteLine($"Update time: {_updateTime}");
            }

            Debug.WriteLine("Running...");
            _azureIot.AddMethodCallback(GetCount);
            _azureIot.SendMessage($"{{\"state\":\"Code {nameof(CountMeasurement)} verion: 4 running...\"}}");
            _runer = new Thread(() => Runner());
            _runer.Start();
        }

        private static void Runner()
        {
            Debug.WriteLine("In the thread");
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                //var mem = nanoFramework.Runtime.Native.GC.Run(false);
                _count++;
                Debug.WriteLine($"Sending telemetry... Counts: {_count}");
                _azureIot.SendMessage($"{{\"Counts\":{_count}}}");
                _cancellationTokenSource.Token.WaitHandle.WaitOne(_updateTime, true);
            }
        }

        public static void Stop()
        {
            Debug.WriteLine("Stop!!!!");
            _azureIot.SendMessage($"{{\"state\":\"Stopping {nameof(CountMeasurement)}\"}}");
            _cancellationTokenSource.Cancel();
        }

        public static void TwinUpdated(TwinCollection twins)
        {
            Debug.WriteLine("Twin update");
            if (twins.Contains(UpdateTime))
            {
                _updateTime = (int)twins[UpdateTime];
                Debug.WriteLine($"Update time: {_updateTime}");
            }
        }

        private static string GetCount(int rid, string payload)
        {
            Debug.WriteLine("Get Memory called");
            // Ignore the payload, we will just return the count available
            return $"{{\"Counts\":{_count}}}";
        }
    }
}
