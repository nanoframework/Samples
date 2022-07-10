//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;

using nanoFramework.Hosting;

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

        public override void Start()
        {
            Debug.WriteLine($"Service '{nameof(MonitorService)}' is now running in the background.");

            base.Start();
        }

        protected override void ExecuteAsync(object state)
        {
            Debug.WriteLine($"Queue Depth: {_queue.QueueCount}");
        }

        public override void Stop()
        {
            Debug.WriteLine($"Service '{nameof(MonitorService)}' is stopping.");
            
            base.Stop();
        }
    }
}
