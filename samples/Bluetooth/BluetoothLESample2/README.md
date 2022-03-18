# Bluetooth Low energy: read/write with encryption a value

This sample adds security to the Characteristic access. This will force the Server/Client to bond/pair which is 
used to generate key pairs for communications. All access is now encrypted. 

| Custom service | Behavior |
| --- | --- |
| Read and Write (requires encryption) a value | Read/Write Int32 |
| Read (requires encryption) and Write a value | Read/Write Int32 (same value) |

The 1st Characteristic allows the read but the write requires it to be paired.
The 2nd Characteristic allows writes but requires to be paired for the read.

See [main Bluetooth sample's readme](../README.md) for more information.