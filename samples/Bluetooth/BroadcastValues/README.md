# 🌶️🌶️ - Bluetooth Low energy: Broadcast current values in a Bluetooth advertisement

This shows how to create a Bluetooth advertisement publisher with custom values.

Putting values in an advertisement saves having to connect to device and read the value.
This sample puts a 1 byte count followed by a 32bit integer into a Manufacturer Data Section.

To collect data from this sample create a simple Bluetooth Watcher and save data coming from 
device with local name "MyValues".

You will be able see advertisements from device with ManufacterData value changing every second.
Suitable Phone applications: "LightBlue" or "nRF Connect"

See [main Bluetooth sample's readme](../README.md) for more information.
