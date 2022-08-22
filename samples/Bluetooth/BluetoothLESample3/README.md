# Bluetooth Low energy: adding, replacing services to the main service

This show cases the use of adding extra services to main service or replacing an existing service 
like the default "Device Information Service". 

This sample also includes some standard Bluetooth services as separate classes which may be useful 
for any Bluetooth LE project.

The temperature values for the Environmental Service are incremented up and down to simulate temperature 
changes from a real sensor. These are used by the Central2 data collection sample.

## Device Information Service 

Provides device information like Manufacturer, model, software version etc.

## Battery Level Service

Publishes the current battery level as a percentage.

## Current Time Service

Publishes the current date/time of device and optionally allows the date/time to be set on device.

## Environmental Sensor service

This allows multiple environmental sensors to be published such as Temperature, Humidity, Pressure, Rainfall.
This sample class includes these 4 but other types can easily added to class. Multiple sensor of same type can be added.
The sample shows 3 Temperatures (Instantaneous, Maximum, Minimum) added to service and a humidity sensor.

See [main Bluetooth sample's readme](../README.md) for more information.
