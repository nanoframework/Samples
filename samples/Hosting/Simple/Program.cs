//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using System.Device.Gpio;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using nanoFramework.Hosting;

namespace Hosting
{
    public class Program
    {
        public static void Main()
        {
            IHostBuilder builder = new HostBuilder();
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(typeof(HardwareService));
                services.AddHostedService(typeof(LedHostedService));
            });

            IHost host = builder.Build();

            // blink for 5 seconds and then stop and dispose of host 
            host.Start();
            Thread.Sleep(5000);
            host.Stop();
            host.Dispose();
        }
    }

    internal class HardwareService : IDisposable
    {
        public GpioController GpioController { get; private set; }

        public HardwareService()
        {
            GpioController = new GpioController();
        }

        public void Dispose()
        {
            GpioController.Dispose();
        }
    }

    internal class LedHostedService : BackgroundService
    {
        private readonly HardwareService _hardware;

        public LedHostedService(HardwareService hardware)
        {
            _hardware = hardware;
        }

        public override void Start()
        {
            Debug.WriteLine("LED Hosted Service running.");

            base.Start();
        }

        protected override void ExecuteAsync()
        {
            var ledPin = 16; 

            GpioPin led = _hardware.GpioController.OpenPin(ledPin, PinMode.Output);

            while (!CancellationRequested)
            {
                led.Toggle();
                Thread.Sleep(100);
            }
        }

        public override void Stop()
        {
            Debug.WriteLine("LED Hosted Service stopped.");

            base.Stop();
        }
    }
}
