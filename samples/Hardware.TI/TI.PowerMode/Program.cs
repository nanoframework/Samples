//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;
using System.Threading;
using Windows.Devices.Gpio;
using TIPower = nanoFramework.Hardware.TI.Power;

namespace Hardware.TI.PowerMode
{
    public class Program
    {
        public static void Main()
        {
            // TI CC13x2 Launchpad: DIO_07 it's the green LED
            GpioPin led = GpioController.GetDefault().OpenPin(7);

            led.SetDriveMode(GpioPinDriveMode.Output);
            led.Write(GpioPinValue.High);

            // query target about wake-up reason
            switch (nanoFramework.Hardware.TI.Power.SourceOfReset)
            {
                case TIPower.ResetSource.ResetPin:
                    Debug.WriteLine("[INFO] Boot following reset pin hit.");
                    break;

                case TIPower.ResetSource.WarmReset:
                    Debug.WriteLine("[INFO] Boot following warm reset.");
                    break;

                case TIPower.ResetSource.SoftwareReset:
                    Debug.WriteLine("[INFO] Boot following software reset.");
                    break;

                case TIPower.ResetSource.WakeupFromShutdown:
                    Debug.WriteLine("[INFO] Boot following wake-up from shutdown.");
                    break;

                case TIPower.ResetSource.PowerOn:
                    Debug.WriteLine("[INFO] Boot following regular power on.");
                    break;
            }

            // enable wake-up from BTN1 GPIO pin (DIO15)
            // need to enable internal pull-up
            // sensitive to transition to negative
            TIPower.ConfigureWakeupFromGpioPin(
                new TIPower.PinWakeupConfig[] { 
                    new TIPower.PinWakeupConfig(
                        15,
                        TIPower.PinWakeupEdge.NegativeEdge,
                        TIPower.PinPullUpDown.PullUp)
                });

            // start a thread blinking the LED to check that something is happening 
            new Thread( () => {
                while (true)
                {
                    Thread.Sleep(125);
                    led.Toggle();
                }
            }).Start();

            // sleep here for 5 seconds to allow the LED to blink after wakeup
            Thread.Sleep(5000);

            Debug.WriteLine($"Going to shutdown mode now...");

            // this call never returns
            // after this the target will enter "TI shutdown" mode and will be waked by a push on the BTN1 switch
            TIPower.EnterShutdownMode();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
