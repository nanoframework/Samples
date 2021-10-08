using Iot.Device.Axp192;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;
using System.Device.I2c;
using System.Diagnostics;
using System.Threading;
using UnitsNet;

namespace m5stick.screen
{
    public class Program
    {
        private static Axp192 power = null;

        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");
            InitiM5Stick();
            // For M5Stick, values from 8 to 12 are working fine
            power!.SetLDO2Output(8);

            int backLightPin = -1; // Not managed thru ESP32 but thru AXP192
            int chipSelect = 5;
            int dataCommand = 23;
            int reset = 18;
            Configuration.SetPinFunction(4, DeviceFunction.SPI1_MISO); // 4 is unused but necessary
            Configuration.SetPinFunction(15, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(13, DeviceFunction.SPI1_CLOCK);
            DisplayControl.Initialize(new SpiConfiguration(1, chipSelect, dataCommand, reset, backLightPin), new ScreenConfiguration(26, 1, 80, 160), 10 * 1024);
            Debug.WriteLine($"DisplayControl.MaximumBufferSize:{DisplayControl.MaximumBufferSize}");

            ushort[] toSend = new ushort[100];
            var blue = ColorUtility.To16Bpp(Color.Blue);
            var red = ColorUtility.To16Bpp(Color.Red);
            var green = ColorUtility.To16Bpp(Color.Green);
            var white = ColorUtility.To16Bpp(Color.White);

            for (int i = 0; i < toSend.Length; i++)
            {
                toSend[i] = blue;
            }

            DisplayControl.Write(0, 0, 10, 10, toSend);

            for (int i = 0; i < toSend.Length; i++)
            {
                toSend[i] = red;
            }

            DisplayControl.Write(69, 0, 10, 10, toSend);

            for (int i = 0; i < toSend.Length; i++)
            {
                toSend[i] = green;
            }

            DisplayControl.Write(0, 149, 10, 10, toSend);

            for (int i = 0; i < toSend.Length; i++)
            {
                toSend[i] = white;
            }

            DisplayControl.Write(69, 149, 10, 10, toSend);

            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }

        public static void InitiM5Stick()
        {
            Debug.WriteLine("This is the sequence to power on the Axp192 for M5 Stick");

            Configuration.SetPinFunction(22, DeviceFunction.I2C1_CLOCK);
            Configuration.SetPinFunction(21, DeviceFunction.I2C1_DATA);

            I2cDevice i2cAxp192 = new(new I2cConnectionSettings(1, Axp192.I2cDefaultAddress));
            power = new Axp192(i2cAxp192);

            // NOTE: the comments include code which was originally used
            // to setup the AXP192 and can be found in the M5Stick repository
            // This allows to understand the selection dome.
            // Set LDO2 & LDO3(TFT_LED & TFT) 3.0V
            // I2cWrite(Register.VoltageSettingLdo2_3, 0xcc);
            power.SetLDO2Output(0xC);
            power.SetLDO3Output(0xC);
            // Set ADC sample rate to 200hz
            // I2cWrite(Register.AdcFrequency, 0xF2);
            power.AdcFrequency = AdcFrequency.Frequency200Hz;
            power.AdcPinCurrent = AdcPinCurrent.MicroAmperes80;
            power.BatteryTemperatureMonitoring = true;
            power.AdcPinCurrentSetting = AdcPinCurrentSetting.AlwaysOn;
            // Set ADC to All Enable
            // I2cWrite(Register.AdcPin1, 0xff);
            power.AdcPinEnabled = AdcPinEnabled.All;
            // Bat charge voltage to 4.2, Current 100MA
            // I2cWrite(Register.ChargeControl1, 0xc0);
            power.SetChargingFunctions(true, ChargingVoltage.V4_2, ChargingCurrent.Current100mA, ChargingStopThreshold.Percent10);
            // Depending on configuration enable LDO2, LDO3, DCDC1, DCDC3.
            // byte data = I2cRead(Register.SwitchControleDcDC1_3LDO2_3);
            // data = (byte)((data & 0xEF) | 0x4D);
            // I2cWrite(Register.SwitchControleDcDC1_3LDO2_3, data);
            power.LdoDcPinsEnabled = LdoDcPinsEnabled.All;
            // 128ms power on, 4s power off
            // I2cWrite(Register.ParameterSetting, 0x0C);
            power.SetButtonBehavior(LongPressTiming.S1, ShortPressTiming.Ms128, true, SignalDelayAfterPowerUp.Ms64, ShutdownTiming.S10);
            // Set RTC voltage to 3.3V
            // I2cWrite(Register.VoltageOutputSettingGpio0Ldo, 0xF0);
            power.PinOutputVoltage = PinOutputVoltage.V3_3;
            // Set GPIO0 to LDO
            // I2cWrite(Register.ControlGpio0, 0x02);
            power.SetGPIO0(Gpio0Behavior.LowNoiseLDO, 0);
            // Disable vbus hold limit
            // I2cWrite(Register.PathSettingVbus, 0x80);
            power.SetVbusSettings(true, false, VholdVoltage.V4_0, false, VbusCurrentLimit.MilliAmper500);
            // Set temperature protection
            // I2cWrite(Register.HigTemperatureAlarm, 0xfc);
            power.SetBatteryHighTemperatureThreshold(ElectricPotential.FromVolts(3.2256));
            // Enable RTC BAT charge 
            // I2cWrite(Register.BackupBatteryChargingControl, 0xa2);
            power.SetBackupBatteryChargingControl(true, BackupBatteryCharingVoltage.V3_0, BackupBatteryChargingCurrent.MicroAmperes200);
            // Enable bat detection
            // I2cWrite(Register.ShutdownBatteryDetectionControl, 0x46);
            // Note 0x46 is not a possible value, most likely 0x4A
            power.SetShutdownBatteryDetectionControl(false, true, ShutdownBatteryPinFunction.HighResistance, true, ShutdownBatteryTiming.S2);
            // Set Power off voltage 3.0v            
            // data = I2cRead(Register.VoltageSettingOff);
            // data = (byte)((data & 0xF8) | (1 << 2));
            // I2cWrite(Register.VoltageSettingOff, data);
            power.VoffVoltage = VoffVoltage.V3_0;

        }
    }
}
