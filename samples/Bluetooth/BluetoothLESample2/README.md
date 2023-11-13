# 🌶️🌶️ - Bluetooth Low energy: read/write with encryption a value

This sample adds pairing to the Characteristic access. This will force the Server/Client to pair which is 
used to generate key pairs for secure communications and a passkey for Authentication. 

All access is now encrypted. 

You can connect to this Server without pairing but you will on be able to read 1st Characteristic.

| Custom service | Behavior |
| --- | --- |
| Read and Write (requires encryption) a value | Read/Write Int32 |
| Read (requires encryption) and Write a value | Read/Write Int32 (same value) |
| Read (requires encryption and authentication) and Write a value | Read/Write Int32 (same value) |

The 1st Characteristic allows the read but the write requires it to be paired.
The 2nd Characteristic allows writes but requires to be paired for the read.
The 3rd Characteristic allows writes but requires to be paired with authentication for the read.

See [main Bluetooth sample's readme](../README.md) for more information.

The sample is [located here](./Program.cs).
