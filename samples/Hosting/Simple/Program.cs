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
                    services.AddHostedService(typeof(TimedHostedService));
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

        protected override void ExecuteAsync(CancellationToken cancellationToken)
        {
            Debug.WriteLine("LED Hosted Service running.");

            cancellationToken.Register(() => Debug.WriteLine("LED Hosted Service is stopping."));

            var ledPin = 16; //LD1;

            GpioPin led = _hardware.GpioController.OpenPin(ledPin, PinMode.Output);

            while (!cancellationToken.IsCancellationRequested)
            {
                led.Toggle();
                Thread.Sleep(100);
            }
        }
    }
    
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private Timer _timer = null;
        private int executionCount = 0;

        public void StartAsync()
        {
            Debug.WriteLine("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            Debug.WriteLine($"Timed Hosted Service is working. Count: {count}");
        }

        public void StopAsync()
        {
            Debug.WriteLine("Timed Hosted Service is stopping.");

            _timer.Change(Timeout.Infinite, 0);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}