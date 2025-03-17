//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.Hosting;

namespace Hosting
{
    internal class MonitorService : SchedulerService
    {
        private readonly BackgroundQueue _queue;

        public MonitorService(BackgroundQueue queue)
            : base(TimeSpan.FromSeconds(1))
        {
            _queue = queue;
        }

        public virtual void StartAsync(CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Service '{nameof(MonitorService)}' is now running in the background.");

            base.StartAsync(cancellationToken);
        }

        protected override void ExecuteAsync(CancellationToken stoppingToken)
        {
            Debug.WriteLine($"Queue Depth: {_queue.QueueCount}");
        }

        public virtual void StopAsync(CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Service '{nameof(MonitorService)}' is stopping.");
            
            base.StopAsync(cancellationToken);
        }
    }
}
