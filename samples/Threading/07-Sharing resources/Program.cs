//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Runtime.Native;
using System;
using System.Threading;

namespace Sharing_resources
{
    public class Program
    {
        public static void Main()
        {
            var account = new Account(1000);

            var threads = new Thread[100];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                    {
                        Update(account);
                    });

                threads[i].Start();
            }

            // wait for all threads to complete
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Debug.WriteLine($"Account's balance is {account.GetBalance()}");

            // Output should be:
            // Account's balance is 2000

            Thread.Sleep(Timeout.Infinite);
        }

        static void Update(Account account)
        {
            float[] amounts = { 0, 2, -3, 6, -2, -1, 8, -5, 11, -6 };

            foreach (var amount in amounts)
            {
                if (amount >= 0)
                {
                    account.Credit(amount);
                }
                else
                {
                    account.Debit(Math.Abs(amount));
                }
            }
        }
    }

    public class Account
    {
        private readonly object balanceLock = new object();
        private float balance;

        public Account(float initialBalance) => balance = initialBalance;

        public float Debit(float amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "The debit amount cannot be negative.");
            }

            float appliedAmount = 0;
            lock (balanceLock)
            {
                if (balance >= amount)
                {
                    balance -= amount;
                    appliedAmount = amount;
                }
            }
            return appliedAmount;
        }

        public void Credit(float amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "The credit amount cannot be negative.");
            }

            lock (balanceLock)
            {
                balance += amount;
            }
        }

        public float GetBalance()
        {
            lock (balanceLock)
            {
                return balance;
            }
        }
    }
}
