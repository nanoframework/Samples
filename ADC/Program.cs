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

            // uncomment this if ADC2 available or to test exception on not available
            //AdcController adc2 = AdcController.FromID("ADC2");

            int max1 = adc1.MaxValue;
            int min1 = adc1.MinValue;
            //int max2 = adc2.MaxValue;
            //int min2 = adc2.MinValue;

            Console.WriteLine("min1=" + min1.ToString() + " max1=" + max1.ToString());
            //Console.WriteLine("min2=" + min2.ToString() + " max2=" + max2.ToString());

            AdcChannel ac0 = adc1.OpenChannel(0);
            AdcChannel vref = adc1.OpenChannel(3);
            AdcChannel vbat = adc1.OpenChannel(4);

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

                //int value3 = ac3.ReadValue();

                //double percent3 = ac3.ReadRatio();

                //Console.WriteLine("value3=" + value3.ToString() + " ratio=" + percent3.ToString());

                Thread.Sleep(1000);
            }
        }
    }
}
