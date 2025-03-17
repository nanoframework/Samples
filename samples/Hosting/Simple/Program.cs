//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using System.Device.Gpio;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            host.StartAsync();
            Thread.Sleep(5000);
            host.StopAsync();
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

        public override void StartAsync(CancellationToken cancellationToken)
        {
            Debug.WriteLine("LED Hosted Service running.");

            base.StartAsync(cancellationToken);
        }

        protected override void ExecuteAsync(CancellationToken cancellationToken)
        {
            var ledPin = 16; 

            GpioPin led = _hardware.GpioController.OpenPin(ledPin, PinMode.Output);

            while (!cancellationToken.IsCancellationRequested)
            {
                led.Toggle();
                Thread.Sleep(100);
            }
        }

        public override void StopAsync(CancellationToken cancellationToken)
        {
            Debug.WriteLine("LED Hosted Service stopped.");

            base.StopAsync(cancellationToken);
        }
    }
}
