# SSL sample pack

Shows how to use various APIs related with SSL.

### Test certificates

The test certificates used in the code are available in this folder.

- [X509 Certificate RSA PEM format 512 bytes](512b-rsa-example-cert.pem)
- [X509 Certificate RSA DER format 512 bytes](512b-rsa-example-cert.der)
- [X509 Certificate RSA PEM format 1024 bytes](512b-rsa-example-cert.pem)
- [X509 Certificate RSA DER format 1024 bytes](512b-rsa-example-cert.der)
- [X509 Certificate RSA PEM format 2048 bytes](512b-rsa-example-cert.pem)
- [X509 Certificate RSA DER format 2048 bytes](512b-rsa-example-cert.der)

> NOTE: when working with ESP32 edit the nfproj files and add `BUIID_FOR_ESP32` to the DefineConstants, like this:
```text
<DefineConstants>$(DefineConstants);BUIID_FOR_ESP32;</DefineConstants>
```

> **Note:** This sample is part of a large collection of nanoFramework feature samples.
> If you are unfamiliar with Git and GitHub, you can download the entire collection as a
> [ZIP file](https://github.com/nanoframework/Samples/archive/main.zip), but be
> sure to unzip everything to access any shared dependencies.
<!-- For more info on working with the ZIP file, 
> the samples collection, and GitHub, see [Get the UWP samples from GitHub](https://aka.ms/ovu2uq). 
> For more samples, see the [Samples portal](https://aka.ms/winsamples) on the Windows Dev Center.  -->

## Hardware requirements

An hardware device with networking capabilities running a nanoFramework image.

## Related topics

### Reference

- [System.Security.Cryptography.X509Certificates](http://docs.nanoframework.net/api/System.Security.Cryptography.X509Certificates.html)
- [System.Net.Sockets](http://docs.nanoframework.net/api/System.Net.Sockets.html)

## Build the sample

1. If you download the samples ZIP, be sure to unzip the entire archive, not just the folder with the sample you want to build. 
2. Start Microsoft Visual Studio 2019 (VS 2017 should be OK too) and select **File** \> **Open** \> **Project/Solution**.
3. Starting in the folder where you unzipped the samples, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
4. Press Ctrl+Shift+B, or select **Build** \> **Build Solution**.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select Build > Deploy Solution.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select Debug >  Start Debugging.
