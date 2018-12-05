//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using Windows.Devices.Adc;

namespace TestAdc
{
    public class Program
    {
        public static void Main()
        {
            string devs = AdcController.GetDeviceSelector();

            Console.WriteLine("devs=" + devs);

            AdcController adc1 = AdcController.GetDefault();

            int max1 = adc1.MaxValue;
            int min1 = adc1.MinValue;

            Console.WriteLine("min1=" + min1.ToString() + " max1=" + max1.ToString());

            AdcChannel ac0 = adc1.OpenChannel(0);

            // the following indexes are valid for STM32F769I-DISCO board
            AdcChannel vref = adc1.OpenChannel(0);
            AdcChannel vbat = adc1.OpenChannel(8);

            // VP 
            //AdcChannel ac3 = adc1.OpenChannel(3);
            // VN 
            while (true)
            {
                int value = ac0.ReadValue();
                int valueVref = vref.ReadValue();
                int valueVbat = vbat.ReadValue();

                double percent = ac0.ReadRatio();

                Console.WriteLine("value0=" + value.ToString() + " ratio=" + percent.ToString());
                Console.WriteLine("verf" + valueVref.ToString() + " ratio=" + percent.ToString());
                Console.WriteLine("vbat" + valueVbat.ToString() + " ratio=" + percent.ToString());

                Thread.Sleep(1000);
            }
        }
    }
}
