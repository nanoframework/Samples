# 🌶️🌶️ - Using an existing generic graphic driver

This sample shows how you can use and adjust an existing generic driver. This usually requires quite some experience. You have the source of all the existing drivers in the [nanoFramework.Graphic libraries](https://github.com/nanoframework/nanoFramework.Graphics/tree/main/ManagedDrivers).

> **Important**: you **MUST** use a .NET nanoFramework image that is built with the Generic Graphic Driver. Using an image that is not will either throw because you don't have the graphic library at all either have absolutely no effect if you have one of the built in driver in the image.

The M5Stack devices have all an existing built in driver. So except if you are not using the standard image on them but one containing a generic image, you can't use a generic graphic driver on them. This won't have any effect at all.

The sample is [located here](./).
