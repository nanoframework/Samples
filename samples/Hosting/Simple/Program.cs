using System;
using System.Threading;
using System.Device.Gpio;
using System.Diagnostics;

using nanoFramework.Hosting;
using nanoFramework.DependencyInjection;

namespace Hosting
{
    public class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(typeof(HardwareService));
                    services.AddHostedService(typeof(LedHostedService));
                });
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

        protected override void ExecuteAsync()
        {
            Debug.WriteLine("LED Hosted Service running.");

            var ledPin = 16; 

            GpioPin led = _hardware.GpioController.OpenPin(ledPin, PinMode.Output);

            while (!CancellationRequested)
            {
                led.Toggle();
                Thread.Sleep(100);
            }
        }
    }
}
