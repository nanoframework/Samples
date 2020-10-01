//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using System.Diagnostics;

using nanoFramework.device.sensor;


//
//	Sample program to show using the RMT device to read the distance using a HC-SR04 
//  
namespace Ultrasonic
{
	public class Program
	{
		public static void Main()
		{
			// Change to suit you hardware set-up
			const int GPIO_TRIGGER_PIN = 27;
			const int GPIO_ECHO_PIN = 26;

			try
			{
				Sr04 device = new Sr04(GPIO_TRIGGER_PIN, GPIO_ECHO_PIN);

				while (true)
				{
					float distance = device.GetDistance();

					if (distance == -1)
						Debug.WriteLine($"Out of range");
					else
						Debug.WriteLine($"Distance {distance:F3} meters");

					Thread.Sleep(1000);
				}
			}
			catch(Exception)
			{ }

			Thread.Sleep(Timeout.Infinite);

		}
	}
}
