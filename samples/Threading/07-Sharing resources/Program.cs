//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;
using System;
using System.Threading;

namespace Sharing_resources
{
    public class Program
    {
        public static void Main()
        {
            var bus = new SharedBus(1000);

            var threads = new Thread[100];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                    {
                        ExecuteComm(bus);
                    });

                threads[i].Start();
            }

            // wait for all threads to complete
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Debug.WriteLine($"Account's balance is {bus.GetOperationValue()}");

            // Output should be:
            // Account's balance is 2000

            Thread.Sleep(Timeout.Infinite);
        }

        static void ExecuteComm(SharedBus bus)
        {
            float[] operations = { 0, 2, -3, 6, -2, -1, 8, -5, 11, -6 };

            foreach (var ops in operations)
            {
                if (ops >= 0)
                {
                    bus.Transmit(ops);
                }
                else
                {
                    bus.Receive(Math.Abs(ops));
                }
            }
        }
    }

    public class SharedBus
    {
        private readonly object _accessLock = new object();
        private float _operation;

        public SharedBus(float initialValue) => _operation = initialValue;

        public float Receive(float operationValue)
        {
            if (operationValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(operationValue), "The operation value cannot be negative.");
            }

            float appliedAmount = 0;

            lock (_accessLock)
            {
                if (_operation >= operationValue)
                {
                    _operation -= operationValue;
                    appliedAmount = operationValue;
                }
            }
            return appliedAmount;
        }

        public void Transmit(float operationValue)
        {
            if (operationValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(operationValue), "The operation value cannot be negative.");
            }

            lock (_accessLock)
            {
                _operation += operationValue;
            }
        }

        public float GetOperationValue()
        {
            lock (_accessLock)
            {
                return _operation;
            }
        }
    }
}
