# Improv Wifi provisioning

This sample shows the use of the *Improv class* to provision the Wifi credentials for an ESP32 device via Bluetooth LE. 

The device will advertise with the name "Improv sample" and support the *Improv Service* allowing the Wifi Credentials to be setup directly from the Improv test page.
or any other web page.

This is an initial working version of the *Improv* class showing what can be done. 
Probably later this class can be moved to a separate repo, so it has its own nuGet package to make it easy to consume by applications.

*Improv* is a free open standard which allows the device to be provisioned directly from a web page.
This works for Chrome and Edge browsers.

For more information on the *Improv* standard and a test page, see: <https://www.improv-wifi.com/>
See <https://www.improv-wifi.com/code/> for details on including it into other web pages.


