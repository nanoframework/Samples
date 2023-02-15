using nanoFramework.Hardware.Esp32;
using nanoFramework.Hardware.Esp32.Touch;
using System;
using System.Diagnostics;
using System.Threading;

namespace TestTouchApp
{
    public class Program
    {
        public static void Main()
        {
            // This is a test application for the touch pad on ESP32 and S2/S3

            // Uncomment the one you want to use. This is about sleep
            // using the touch pad as a wake up source
            // DeepSleepExampleS2S3();
            // DeepSleepExampleEsp32With2Pins();
            // DeepSleepExampleEsp32();

            // Adjust the touch pad you want to use
            // S2/S3 goes from 1 to 13.
            // ESP32 from 0 to 9
            const int TouchPadNumber0 = 5;
            const int TouchPadNumber1 = 12;
            Debug.WriteLine("Hello touch pad on ESP32 and S2/S3!");

            var pinNum = TouchPad.GetGpioNumberFromTouchNumber(TouchPadNumber0);
            Console.WriteLine($"Pad {TouchPadNumber0} is GPIO{pinNum}");
            pinNum = TouchPad.GetGpioNumberFromTouchNumber(TouchPadNumber1);
            Console.WriteLine($"Pad {TouchPadNumber1} is GPIO{pinNum}");

            //TestVoltageChange();
            //TestTriggerMode();
            //TestWakeupMode();
            TestDenoise();

            TouchPad touchpad0 = new(TouchPadNumber0);
            Debug.WriteLine("Initialized!");

            //TestChargeSpeed(touchpad0);
            //TestThreshold(touchpad0);
            //TestCMeasurementTime();

            // Always sets the voltage before calibrating
            TouchPad.SetVoltage(TouchHighVoltage.Volt2V7, TouchLowVoltage.Volt0V5, TouchHighVoltageAttenuation.Volt1V0);

            // This function calibrate
            Calibrate(touchpad0);

            // On ESP32: Setup a threshold, usually 2/3 or 80% is a good value.
            // touchpad0.Threshold = (uint)(touchpad0.CalibrationData * 2 / 3);
            // On S2/S3, the actual read vallues will be higher, so let's use 20% more
            TouchPad.TouchTriggerMode = TouchTriggerMode.AboveThreshold;
            touchpad0.Threshold = (uint)(touchpad0.CalibrationData * 1.3);
            // Optional, you can setup a filter for ESP32
            // TestFilterEsp();
            // TestFilterS2();


            Console.WriteLine("Testing timer mode for 20 seconds, touch the pad to generate events");
            TouchPad.MeasurementMode = MeasurementMode.Timer;

            touchpad0.ValueChanged += TouchpadValueChanged;
            Thread.Sleep(20_000);

            Console.WriteLine("Testing manual mode for 20 seconds");
            TouchPad.MeasurementMode = MeasurementMode.Software;
            int cnt = 0;
            uint val;
            while (cnt++ < 100)
            {
                val = touchpad0.Read();
                Console.WriteLine($"Val: {val}");
                Thread.Sleep(200);
            }

            // Open a second one
            Console.WriteLine($"Opening a second touchpad {TouchPadNumber1}");
            TouchPad touchpad1 = new(TouchPadNumber1);
            touchpad1.ValueChanged += TouchpadValueChanged;
            touchpad1.GetCalibrationData();
            Console.WriteLine($"Calibration touchpad {touchpad1.TouchPadNumber}: {touchpad1.CalibrationData}");
            Console.WriteLine("Testing timer mode for 20 seconds, touch the pads to generate events");
            touchpad1.Threshold = (uint)(touchpad1.CalibrationData * 1.3);
            TouchPad.MeasurementMode = MeasurementMode.Timer;

            Thread.Sleep(20_000);

            Console.WriteLine("Manually reading both pads for 20 seconds");
            uint val1;
            cnt = 0;
            while (cnt++ < 100)
            {
                val = touchpad0.Read();
                val1 = touchpad1.Read();
                Console.WriteLine($"Val: {val}, val1: {val1}");
                Thread.Sleep(200);
            }

            Console.WriteLine("End of tests!");
            Thread.Sleep(Timeout.Infinite);
        }

        private static void TouchpadValueChanged(object sender, TouchPadEventArgs e)
        {
            Console.WriteLine($"Touchpad {e.PadNumber} is {(e.Touched ? "touched" : "not touched")}");
        }

        private static void TestVoltageChange()
        {
            //// Changing voltage
            Console.WriteLine($"Initial Voltage: {TouchPad.TouchHighVoltage} {TouchPad.TouchLowVoltage} {TouchPad.TouchHighVoltageAttenuation}");
            Console.WriteLine($"Setting Voltage: {TouchHighVoltage.Volt2V7} {TouchLowVoltage.Volt0V6} {TouchHighVoltageAttenuation.Volt1V0}");
            TouchPad.SetVoltage(TouchHighVoltage.Volt2V7, TouchLowVoltage.Volt0V6, TouchHighVoltageAttenuation.Volt1V0);
            Console.WriteLine($"New set Voltage: {TouchPad.TouchHighVoltage} {TouchPad.TouchLowVoltage} {TouchPad.TouchHighVoltageAttenuation}");
        }

        private static void TestTriggerMode()
        {
            try
            {
                // static trigger mode, this is ESP32 only
                Console.WriteLine($"Initial trigger mode: {TouchPad.TouchTriggerMode}");
                TouchPad.TouchTriggerMode = TouchTriggerMode.AboveThreshold;
                Console.WriteLine($"New trigger mode: {TouchPad.TouchTriggerMode}");
                Console.WriteLine($"Setting new triggermode: {TouchTriggerMode.BellowThreshold}");
                TouchPad.TouchTriggerMode = TouchTriggerMode.BellowThreshold;
                Console.WriteLine($"New trigger mode: {TouchPad.TouchTriggerMode}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Only supported in ESP32, not on S2/S3. Exception rasied while changing trigger mode.");
                Console.WriteLine(ex.ToString());
            }
        }

        private static void TestWakeupMode()
        {
            try
            {
                // Static wakeup source
                Console.WriteLine($"Initial wakeup source: {TouchPad.WakeUpSource}");
                TouchPad.WakeUpSource = WakeUpSource.OnlySet1;
                Console.WriteLine($"New wakeup source: {TouchPad.WakeUpSource}");
                Console.WriteLine($"Setting new wakeup source: {WakeUpSource.BothSet1AndSet2}");
                TouchPad.WakeUpSource = WakeUpSource.BothSet1AndSet2;
                Console.WriteLine($"New wakeup source: {TouchPad.WakeUpSource}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Only supported in ESP32, not on S2/S3. Exception rasied while changing wakeup mode.");
                Console.WriteLine(ex.ToString());
            }
        }

        private static void TestChargeSpeed(TouchPad touchpad)
        {
            try
            {
                // Charge speed
                var chargeSpeed = touchpad.GetChargeSpeed();
                TouchChargeSpeed touchChargeSpeed = chargeSpeed;
                Console.WriteLine($"Initial speed: {chargeSpeed.Speed} charge {chargeSpeed.Charge}");
                chargeSpeed.Speed = TouchChargeSpeed.ChargeSpeed.Speed3;
                chargeSpeed.Charge = TouchChargeSpeed.InitialCharge.High;
                Console.WriteLine($"Set speed: {chargeSpeed.Speed} charge {chargeSpeed.Charge}");
                touchpad.SetChargeSpeed(chargeSpeed);
                chargeSpeed = touchpad.GetChargeSpeed();
                Console.WriteLine($"New speed: {chargeSpeed.Speed} charge {chargeSpeed.Charge}");
                chargeSpeed.Speed = touchChargeSpeed.Speed;
                chargeSpeed.Charge = touchChargeSpeed.Charge;
                touchpad.SetChargeSpeed(chargeSpeed);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in changing charge speed");
                Console.WriteLine(ex.ToString());
            }
        }

        private static void TestThreshold(TouchPad touchpad)
        {
            try
            {
                // Changing threashold
                Console.WriteLine($"Initial Threshold: {touchpad.Threshold}");
                uint oldThreshold = touchpad.Threshold;
                Console.WriteLine("Setting new Threshold: 42");
                touchpad.Threshold = 42;
                Console.WriteLine($"New Threshold is: {touchpad.Threshold}");
                Console.WriteLine($"Setting new Threshold: {oldThreshold}");
                touchpad.Threshold = oldThreshold;
                Console.WriteLine($"New Threshold is: {touchpad.Threshold}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in changing charge speed");
                Console.WriteLine(ex.ToString());
            }
        }

        private static void Calibrate(TouchPad touchpad)
        {
            try
            {
                Console.WriteLine($"Calibrating touch pad {touchpad.TouchPadNumber}, DO NOT TOUCH it during the process.");
                var calib = touchpad.GetCalibrationData();
                Console.WriteLine($"calib: {calib} vs Calibration {touchpad.CalibrationData}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception raised while calibrating.");
                Console.WriteLine(ex.ToString());
            }
        }

        private static void DeepSleepExampleS2S3()
        {
            const int PadForSleep = 6;
            var wakeup = Sleep.GetWakeupCause();
            var padNum = Sleep.GetWakeupTouchpad();
            Console.WriteLine($"Woke up, cause: {wakeup}, PadNum: {padNum}");
            Console.WriteLine("Waiting 10 seconds");
            WriteDot(10);
            Console.WriteLine($"Setting up sleep mode and calibrating, DO NOT TOUCH the pad, using pad {PadForSleep}");
            Sleep.EnableWakeupByTouchPad(PadForSleep, thresholdCoefficient: 90);
            Console.WriteLine("Sleeping");
            Sleep.StartDeepSleep();
        }

        private static void DeepSleepExampleEsp32()
        {
            var wakeup = Sleep.GetWakeupCause();
            var padNum = Sleep.GetWakeupTouchpad();
            Console.WriteLine($"Woke up, cause: {wakeup}, PadNum: {padNum}");
            WriteDot(10);
            Console.WriteLine("Setting up sleep mode and calibrating, DO NOT TOUCH the pad, using pad 0.");
            Sleep.EnableWakeupByTouchPad(0);
            Sleep.StartDeepSleep();
        }

        private static void DeepSleepExampleEsp32With2Pins()
        {
            var wakeup = Sleep.GetWakeupCause();
            var padNum = Sleep.GetWakeupTouchpad();
            Console.WriteLine($"Woke up, cause: {wakeup}, PadNum: {padNum}");
            WriteDot(10);
            Console.WriteLine("Setting up sleep mode and calibrating, DO NOT TOUCH the pads, using pad 0 and 9.");
            Sleep.EnableWakeupByTouchPad(0, 9);
            Sleep.StartDeepSleep();
        }

        private static void WriteDot(int iterations)
        {
            int cnt = 0;
            while (cnt++ < iterations)
            {
                Console.WriteLine($"{iterations - cnt}");
                Thread.Sleep(1000);
            }
        }

        private static void TestCMeasurementTime()
        {
            var meas = TouchPad.GetMeasurementTime();
            Console.WriteLine($"Cycles speed: {meas.SleepCycles} meas {meas.MeasurementCycles.TotalMilliseconds} ms");

        }

        private static void TestDenoise()
        {
            {
                var desnoise = TouchPad.GetDenoise();
                Console.WriteLine($"Desnoise: {desnoise.DenoiseRange} {desnoise.DenoiseCapacitance}");
                desnoise.DenoiseRange = DenoiseRange.Bit4;
                desnoise.DenoiseCapacitance = DenoiseCapacitance.Cap10pf6;
                TouchPad.SetDenoise(desnoise);
            }
        }

        private static void TestFilterS2()
        {
            TouchPad.StartFilter(new S2S3FilterSetting()
            {
                PeriodeSettingMode = FilterSettingMode.Iir16,
                FilterSettingDebounce = FilterSettingDebounce.One,
                FilterSettingNoiseThreshold = FilterSettingNoiseThreshold.Low,
                JitterSize = 4,
                FilterSettingSmoothMode = FilterSettingSmoothMode.Iir2
            });
        }

        private static void TestFilterEsp()
        {
            TouchPad.StartFilter(new Esp32FilterSetting() { Period = 10 });
        }
    }
}
