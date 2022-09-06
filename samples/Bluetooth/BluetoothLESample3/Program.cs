//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Threading;

using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;
using nanoFramework.Device.Bluetooth.Services;
using nanoFramework.Runtime.Native;

/// <summary>
/// Bluetooth Sample 3 is a application which shows the use of:
/// 
/// Standard Services:
/// - Device Information
/// - Battery Level
/// - Current Time
/// - Environmental Sensor
///  
/// Suitable Phone apps for testing: "nRF Connect" or "LightBlue" 
/// </summary>
namespace BluetoothLESample3
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello from Bluetooth Sample 3");

            // Define used custom Uuid 
            Guid serviceUuid = new("A7EEDF2C-DA87-4CB5-A9C5-5151C78B0057");
            Guid readStaticCharUuid = new("A7EEDF2C-DA89-4CB5-A9C5-5151C78B0057");

            //The GattServiceProvider is used to create and advertise the primary service definition.
            //An extra device information service will be automatically created.
            GattServiceProviderResult result = GattServiceProvider.Create(serviceUuid);
            if (result.Error != BluetoothError.Success)
            {
                return;
            }

            GattServiceProvider serviceProvider = result.ServiceProvider;

            // Get created Primary service from provider
            GattLocalService service = serviceProvider.Service;

            #region Static read characteristic
            // Now we add an characteristic to service
            // If the read value is not going to change then you can just use a Static value
            DataWriter sw = new();
            sw.WriteString("This is Bluetooth sample 3");

            GattLocalCharacteristicResult characteristicResult = service.CreateCharacteristic(readStaticCharUuid,
                 new GattLocalCharacteristicParameters()
                 {
                     CharacteristicProperties = GattCharacteristicProperties.Read,
                     UserDescription = "My Static Characteristic",
                     StaticValue = sw.DetachBuffer()
                 });
            ;

            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return;
            }
            #endregion

            // Add standard Bluetooth Sig services to the provider. These are an example of standard services
            // that can be reused/updated for other applications. Based on standards but simplified. 

            // === Device Information Service ===
            // https://www.bluetooth.com/specifications/specs/device-information-service-1-1/
            // The Device Information Service is created automatically when you create the initial primary service.
            // The default version just has a Manufacturer name of "nanoFramework and model or "Esp32"
            // You can add your own service which will replace the default one.
            // To make it easy we have included some standard services classes to this sample
            DeviceInformationServiceService DifService = new(
                    serviceProvider,
                    "MyGreatCompany",
                    "Model-1",
                    null, // no serial number
                    "v1.0",
                    SystemInfo.Version.ToString(),
                    "");

            // === Battery Service ===
            // https://www.bluetooth.com/specifications/specs/battery-service-1-0/
            // Battery service exposes the current battery level percentage
            BatteryService BatService = new(serviceProvider);

            // Update the Battery service the current battery level regularly. In this case 94%
            BatService.BatteryLevel = 94;

            // === Current Time Service ===
            // https://www.bluetooth.com/specifications/specs/current-time-service-1-1/
            // The Current Time Service exposes the device current date/time and also 
            // optionally allows the date time to be updated. You can call the Notify method to inform
            // any connected devices of changed in date/time.  Any subscribed clients will be automatically 
            // be notified every 60 seconds.
            CurrentTimeService CtService = new(serviceProvider, true);

            // === Environmental Sensor Service ===
            // https://www.bluetooth.com/specifications/specs/environmental-sensing-service-1-0/
            // This service exposes measurement data from an environmental sensors.
            EnvironmentalSensorService EnvService = new(serviceProvider);

            // Add sensors to service, return index so sensor can be updated later.
            int iTempOut = EnvService.AddSensor(EnvironmentalSensorService.SensorType.Temperature, "Outside Temp");
            int iTempOutMax = EnvService.AddSensor(EnvironmentalSensorService.SensorType.Temperature, "Max Outside Temp", EnvironmentalSensorService.Sampling.Maximum);
            int iTempOutMin = EnvService.AddSensor(EnvironmentalSensorService.SensorType.Temperature, "Min Outside Temp", EnvironmentalSensorService.Sampling.Minimum);
            int iHumidity = EnvService.AddSensor(EnvironmentalSensorService.SensorType.Humidity, "OUtside Humidty");

            // Update sensor values, these would need to be updated every time sensors are read. 
            EnvService.UpdateValue(iTempOut, 23.4F);
            EnvService.UpdateValue(iTempOutMax, 28.1F);
            EnvService.UpdateValue(iTempOutMin, 7.5F);
            EnvService.UpdateValue(iHumidity, 63.3F);


            #region Start Advertising
            // Once all the Characteristics/Services have been created you need to advertise so 
            // other devices can see it. Here we also say the device can be connected too and other
            // devices can see it with a specific device name.
            serviceProvider.StartAdvertising(new GattServiceProviderAdvertisingParameters()
            {
                DeviceName = "Sample3",
                IsConnectable = true,
                IsDiscoverable = true
            });
            #endregion 

            Thread.Sleep(60000);

            // Update values after 1 min. to simulate real sensors
            while (true)
            {
                float t1 = 23.4F;
                float t3 = 7.5F;

                // Move temperatures up
                while (t1 < 120)
                {
                    t1 += 1.3F;
                    t3 += 2.1F;

                    EnvService.UpdateValue(iTempOut, t1);
                    EnvService.UpdateValue(iTempOutMin, t3);
                    Thread.Sleep(5000);
                }

                // Move temperatures down
                while (t1 > -50F)
                {
                    t1 -= 1.3F;
                    t3 -= 2.1F;

                    EnvService.UpdateValue(iTempOut, t1);
                    EnvService.UpdateValue(iTempOutMin, t3);
                    Thread.Sleep(5000);
                }

            }
        }
    }
}
