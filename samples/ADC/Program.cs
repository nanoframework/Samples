//
// Copyright (c) 2021 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;
using System.Threading;
using Windows.Devices.Adc;

namespace TestAdc
{
    public class Program
    {
        public static void Main()
        {
            // nanoFramework provides a single "virtual" ADC controller, even if the target has multiple ones.
            AdcController adc = AdcController.GetDefault();

            // Minimum value is usually 0, but maximum depends on the board and the actual physical analog-to-digital converter.
            // For example, STM32F769 has 12bit ADC, so the maximum value is 4095.
            int maximum = adc.MaxValue;
            int minimum = adc.MinValue;
            Debug.WriteLine($"ADC minimum value is {minimum}, maximum - {maximum}");

            // Analog channels in nanoFramework are defined sequentially: 0, 1, 2, etc. However, these number are mapped inside firmware,
            // and it is not obvious of how manufacturers datasheet corresponds to the nanoFramework channel numbers.
            // It is different for every target board. For example, Channel 0 for STM32F769DISCOVERY board corresponds to ADC1_IN6,
            // which is mapped to PA6 MCU pin, which is then routed to A0 pin on Arduino headers.
            // Try looking for such information in reference target documentation pages: https://docs.nanoframework.net/content/reference-targets/index.html

            // For this sample, let's just open Channel 0. There's a good chance it is present on most boards.
            AdcChannel channel0 = adc.OpenChannel(0);

            while (true)
            {
                // Reading absolute value. 
                int value = channel0.ReadValue();

                // Reading ratio is may also be useful. You don't have to deal with ADC bits then.
                double ratio = channel0.ReadRatio();

                // If left onconnected, analog pin will "float", so we'll read some random values.
                // If you touch it with your finger, value will change.
                // You may use a Dupoint wire to short it to GND or 3V3 pins,
                // this will read values very close to 0 (if tied to GND) or maximum (if tied to 3V3).
                Debug.WriteLine($"Value: {value}, ratio: {ratio.ToString("F2")}");

                Thread.Sleep(1000);
            }
        }
    }
}
