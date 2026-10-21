# 🌶️ - Managed resources sample

Shows how to use the [System.Resources.ResourceManager](http://docs.nanoframework.net/api/System.Resources.ResourceManager.html) API to access the application managed resources.

This sample illustrates how to access from the code the application managed resources.

> [!Important]
>
> To access the application managed resources the project has to have to reference `nanoFramework.Runtime.Native` and make sure you have the nanoFramework Resources nuget installed.

The sample is [located here](./Program.cs).

The solution also includes [ManagedResources.Graphics](./ManagedResources.Graphics/Program.cs), which shows how to use images (BMP, JPG and GIF) and fonts (.tinyfnt) as managed resources. These require a reference to `nanoFramework.Graphics`.

## Resource types covered

Depending on the Visual Studio version, the resource editor declares the same file with different types in the .resx. Both projects include each variant, so the solution can be used to check managed resource generation.

| Project | Resource | File | Declared type | Generated as |
|---|---|---|---|---|
| ManagedResources | `String1`, `String2` | (inline) | string | `StringResources` |
| ManagedResources | `textFile` | sample.txt | `System.String` | `StringResources` |
| ManagedResources | `nano-Framework-logo-32-border-trans` | .png | `System.Drawing.Bitmap` | `BinaryResources` |
| ManagedResources | `legacyPng` | .png | `System.Byte[]` | `BinaryResources` |
| ManagedResources | `favicon` | .ico | `System.Drawing.Icon` | `BinaryResources` |
| ManagedResources | `beep` | .wav | `System.IO.MemoryStream` | `BinaryResources` |
| ManagedResources.Graphics | `SoccerBall`, `Waterfall`, `Checker` | .gif, .jpg, .bmp | `System.Drawing.Bitmap` | `BitmapResources` |
| ManagedResources.Graphics | `WaterfallLegacy` | .jpg | `System.Byte[]` | `BitmapResources` |
| ManagedResources.Graphics | `Small` | .tinyfnt | `System.Byte[]` | `FontResources` |
| ManagedResources.Graphics | `SmallNew` | .tinyfnt | `System.IO.MemoryStream` | `FontResources` |

Resource names that aren't valid C# identifiers (like `nano-Framework-logo-32-border-trans`) are generated with `_` in place of the invalid characters.

## Hardware requirements

Any hardware device running a nanoFramework image.

ManagedResources.Graphics requires a nanoFramework image with graphics support.

## Related topics

### Reference

- [System.Resources.ResourceManager](http://docs.nanoframework.net/api/System.Resources.ResourceManager.html)

## Build the sample

1. Start Microsoft Visual Studio 2022 or Visual Studio 2019 (Visual Studio 2017 should be OK too) and select `File > Open > Project/Solution`.
1. Starting in the folder where you unzipped the samples/cloned the repository, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
1. Press `Ctrl+Shift+B`, or select `Build > Build Solution`.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select `Build > Deploy Solution`.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select `Debug > Start Debugging`.

> [!NOTE]
>
> **Important**: Before deploying or running the sample, please make sure your device is visible in the Device Explorer.
>
> **Tip**: To display the Device Explorer, go to Visual Studio menus: `View > Other Windows > Device Explorer`.
