//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Hosting
{
    internal class SensorService : BackgroundService
    {
        private readonly Random _random;
        private readonly BackgroundQueue _queue;

        public SensorService(BackgroundQueue queue)
        {
            _queue = queue;
            _random = new Random();
        }

        protected override void ExecuteAsync(CancellationToken stoppingToken)
        {
            Debug.WriteLine($"Service '{nameof(SensorService)}' is now running in the background.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Thread.Sleep(1);
                    var workItem = FakeSensor();
                    _queue.Enqueue(workItem);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"An error occurred when enqueueing work item. Exception: {ex}");
                }
            }
        }

        private int FakeSensor()
        {
            Thread.Sleep(_random.Next(10));
            return _random.Next(1000);
        }
    }
}
