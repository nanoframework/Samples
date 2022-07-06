//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using System.Diagnostics;

using nanoFramework.Hosting;

namespace Hosting
{
    internal class DisplayService : BackgroundService
    {
        private readonly BackgroundQueue _queue;

        public DisplayService(BackgroundQueue queue)
        {
            _queue = queue;
        }

        protected override void ExecuteAsync(CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Service '{nameof(DisplayService)}' is now running in the background.");
            cancellationToken.Register(() => Debug.WriteLine($"Service '{nameof(DisplayService)}' is stopping."));

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Thread.Sleep(50);

                    var workItem = _queue.Dequeue();
                    if (workItem == null)
                    {
                        continue;
                    }

                    FakeDisplay(workItem.ToString());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"An error occurred when dequeueing work item. Exception: {ex}");
                }
            }
        }

        void FakeDisplay(string text)
        {
            //Debug.WriteLine(text);
        }
    }
}
