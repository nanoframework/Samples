# Bluetooth Low energy: read static and dynamic values, notification, read/write value

This shows how to create a custom service which shows the use of:

| Custom service | Behavior |
| --- | --- |
| Read static value (value that doesn't change) | Value text |
| Read a dynamic value using an event handler. | value 3 bytes (Hour/Minute/Seconds) |
| Notifying clients of a changed value | Notify time every 60 seconds or when date updated. |
| Read and Write a value | Read/Write 3 bytes RGB |

You will be able to connect to the service and read values or subscribe to be Notified ever 60 seconds.
Suitable Phone apps: "LightBlue" or "nRF Connect"

See [main Bluetooth sample's readme](../README.md) for more information.