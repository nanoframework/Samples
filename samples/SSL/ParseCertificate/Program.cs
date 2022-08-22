//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace ParseCertificate
{
    public class Program
    {
        public static void Main()
        {
            //////////////////////////////////////////////////////////////////////////////////
            // certificate in PEM format (as a string in the app)
            X509Certificate cert = new X509Certificate(x509RsaPem2048bytesCertificate);

            Debug.WriteLine("Certificate Details:");

            Debug.WriteLine($"Issuer: {cert.Issuer}");
            Debug.WriteLine($"Subject: {cert.Subject}");
            Debug.WriteLine($"Effective Date: {cert.GetEffectiveDate()}");
            Debug.WriteLine($"Expiry Date:: {cert.GetExpirationDate()}");

            // check raw data against buffer
            if (cert.GetRawCertData().GetHashCode() == Encoding.UTF8.GetBytes(x509RsaPem2048bytesCertificate).GetHashCode())
            {
                Debug.WriteLine("Raw data checks");
            }
            else
            {
                Debug.WriteLine("******************************");
                Debug.WriteLine("ERROR: Raw data is different!!");
                Debug.WriteLine("******************************");
            }

            /////////////////////////////////////////////////////////////////////////////////////
            // add certificate in CER format (as a managed resource)
            cert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.DigiCertGlobalRootCA));

            Debug.WriteLine("DigiCert Certificate Details:");

            Debug.WriteLine($"Issuer: {cert.Issuer}");
            Debug.WriteLine($"Subject: {cert.Subject}");
            Debug.WriteLine($"Effective Date: {cert.GetEffectiveDate()}");
            Debug.WriteLine($"Expiry Date:: {cert.GetExpirationDate()}");

            // check raw data against buffer
            if (cert.GetRawCertData().GetHashCode() ==  Resources.GetBytes(Resources.BinaryResources.DigiCertGlobalRootCA).GetHashCode())
            {
                Debug.WriteLine("Raw data checks");
            }
            else
            {
                Debug.WriteLine("******************************");
                Debug.WriteLine("ERROR: Raw data is different!!");
                Debug.WriteLine("******************************");
            }


            /////////////////////////////////////////////////////////////////////////////////////
            // add certificate in CER format (as a string in the app)
            cert = new X509Certificate2(clientRsaSha256Crt, clientRsaKey, "");

            Debug.WriteLine("PolarSSL Client Certificate Details:");

            Debug.WriteLine($"Issuer: {cert.Issuer}");
            Debug.WriteLine($"Subject: {cert.Subject}");
            Debug.WriteLine($"Effective Date: {cert.GetEffectiveDate()}");
            Debug.WriteLine($"Expiry Date:: {cert.GetExpirationDate()}");

            // check raw data against buffer
            if (cert.GetRawCertData().GetHashCode() == Encoding.UTF8.GetBytes(x509RsaPem2048bytesCertificate).GetHashCode())
            {
                Debug.WriteLine("Raw data checks");
            }
            else
            {
                Debug.WriteLine("******************************");
                Debug.WriteLine("ERROR: Raw data is different!!");
                Debug.WriteLine("******************************");
            }

            Thread.Sleep(Timeout.Infinite);
        }

        // X509 RSA key PEM format 512 bytes
        // static string x509RsaPem512bytesCertificate = 
//@"-----BEGIN CERTIFICATE-----
//MIICEjCCAXsCAg36MA0GCSqGSIb3DQEBBQUAMIGbMQswCQYDVQQGEwJKUDEOMAwG
//A1UECBMFVG9reW8xEDAOBgNVBAcTB0NodW8ta3UxETAPBgNVBAoTCEZyYW5rNERE
//MRgwFgYDVQQLEw9XZWJDZXJ0IFN1cHBvcnQxGDAWBgNVBAMTD0ZyYW5rNEREIFdl
//YiBDQTEjMCEGCSqGSIb3DQEJARYUc3VwcG9ydEBmcmFuazRkZC5jb20wHhcNMTIw
//ODIyMDUyNjU0WhcNMTcwODIxMDUyNjU0WjBKMQswCQYDVQQGEwJKUDEOMAwGA1UE
//CAwFVG9reW8xETAPBgNVBAoMCEZyYW5rNEREMRgwFgYDVQQDDA93d3cuZXhhbXBs
//ZS5jb20wXDANBgkqhkiG9w0BAQEFAANLADBIAkEAm/xmkHmEQrurE/0re/jeFRLl
//8ZPjBop7uLHhnia7lQG/5zDtZIUC3RVpqDSwBuw/NTweGyuP+o8AG98HxqxTBwID
//AQABMA0GCSqGSIb3DQEBBQUAA4GBABS2TLuBeTPmcaTaUW/LCB2NYOy8GMdzR1mx
//8iBIu2H6/E2tiY3RIevV2OW61qY2/XRQg7YPxx3ffeUugX9F4J/iPnnu1zAxxyBy
//2VguKv4SWjRFoRkIfIlHX0qVviMhSlNy2ioFLy7JcPZb+v3ftDGywUqcBiVDoea0
//Hn+GmxZA
//-----END CERTIFICATE-----";


        // X509 RSA key PEM format 1024 bytes
        // static string x509RsaPem1024bytesCertificate =
        //@"-----BEGIN CERTIFICATE-----
        //MIICVjCCAb8CAg37MA0GCSqGSIb3DQEBBQUAMIGbMQswCQYDVQQGEwJKUDEOMAwG
        //A1UECBMFVG9reW8xEDAOBgNVBAcTB0NodW8ta3UxETAPBgNVBAoTCEZyYW5rNERE
        //MRgwFgYDVQQLEw9XZWJDZXJ0IFN1cHBvcnQxGDAWBgNVBAMTD0ZyYW5rNEREIFdl
        //YiBDQTEjMCEGCSqGSIb3DQEJARYUc3VwcG9ydEBmcmFuazRkZC5jb20wHhcNMTIw
        //ODIyMDUyNzIzWhcNMTcwODIxMDUyNzIzWjBKMQswCQYDVQQGEwJKUDEOMAwGA1UE
        //CAwFVG9reW8xETAPBgNVBAoMCEZyYW5rNEREMRgwFgYDVQQDDA93d3cuZXhhbXBs
        //ZS5jb20wgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBAMYBBrx5PlP0WNI/ZdzD
        //+6Pktmurn+F2kQYbtc7XQh8/LTBvCo+P6iZoLEmUA9e7EXLRxgU1CVqeAi7QcAn9
        //MwBlc8ksFJHB0rtf9pmf8Oza9E0Bynlq/4/Kb1x+d+AyhL7oK9tQwB24uHOueHi1
        //C/iVv8CSWKiYe6hzN1txYe8rAgMBAAEwDQYJKoZIhvcNAQEFBQADgYEAASPdjigJ
        //kXCqKWpnZ/Oc75EUcMi6HztaW8abUMlYXPIgkV2F7YanHOB7K4f7OOLjiz8DTPFf
        //jC9UeuErhaA/zzWi8ewMTFZW/WshOrm3fNvcMrMLKtH534JKvcdMg6qIdjTFINIr
        //evnAhf0cwULaebn+lMs8Pdl7y37+sfluVok=
        //-----END CERTIFICATE-----";

        static string x509RsaPem2048bytesCertificate =
@"-----BEGIN CERTIFICATE-----
MIIC2jCCAkMCAg38MA0GCSqGSIb3DQEBBQUAMIGbMQswCQYDVQQGEwJKUDEOMAwG
A1UECBMFVG9reW8xEDAOBgNVBAcTB0NodW8ta3UxETAPBgNVBAoTCEZyYW5rNERE
MRgwFgYDVQQLEw9XZWJDZXJ0IFN1cHBvcnQxGDAWBgNVBAMTD0ZyYW5rNEREIFdl
YiBDQTEjMCEGCSqGSIb3DQEJARYUc3VwcG9ydEBmcmFuazRkZC5jb20wHhcNMTIw
ODIyMDUyNzQxWhcNMTcwODIxMDUyNzQxWjBKMQswCQYDVQQGEwJKUDEOMAwGA1UE
CAwFVG9reW8xETAPBgNVBAoMCEZyYW5rNEREMRgwFgYDVQQDDA93d3cuZXhhbXBs
ZS5jb20wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQC0z9FeMynsC8+u
dvX+LciZxnh5uRj4C9S6tNeeAlIGCfQYk0zUcNFCoCkTknNQd/YEiawDLNbxBqut
bMDZ1aarys1a0lYmUeVLCIqvzBkPJTSQsCopQQ9V8WuT252zzNzs68dVGNdCJd5J
NRQykpwexmnjPPv0mvj7i8XgG379TyW6P+WWV5okeUkXJ9eJS2ouDYdR2SM9BoVW
+FgxDu6BmXhozW5EfsnajFp7HL8kQClI0QOc79yuKl3492rH6bzFsFn2lfwWy9ic
7cP8EpCTeFp1tFaD+vxBhPZkeTQ1HKx6hQ5zeHIB5ySJJZ7af2W8r4eTGYzbdRW2
4DDHCPhZAgMBAAEwDQYJKoZIhvcNAQEFBQADgYEAQMv+BFvGdMVzkQaQ3/+2noVz
/uAKbzpEL8xTcxYyP3lkOeh4FoxiSWqy5pGFALdPONoDuYFpLhjJSZaEwuvjI/Tr
rGhLV1pRG9frwDFshqD2Vaj4ENBCBh6UpeBop5+285zQ4SI7q4U9oSebUDJiuOx6
+tZ9KynmrbJpTSi0+BM=
-----END CERTIFICATE-----";

        static string pfxTestCertificate =
@"-----BEGIN CERTIFICATE-----
MIIGIzCCBQugAwIBAgIQJ8rSog2vgvpqFcUPeqrUXjANBgkqhkiG9w0BAQsFADCB
jzELMAkGA1UEBhMCR0IxGzAZBgNVBAgTEkdyZWF0ZXIgTWFuY2hlc3RlcjEQMA4G
A1UEBxMHU2FsZm9yZDEYMBYGA1UEChMPU2VjdGlnbyBMaW1pdGVkMTcwNQYDVQQD
Ey5TZWN0aWdvIFJTQSBEb21haW4gVmFsaWRhdGlvbiBTZWN1cmUgU2VydmVyIENB
MB4XDTE5MDYyNDAwMDAwMFoXDTIwMDYyMzIzNTk1OVowYzEhMB8GA1UECxMYRG9t
YWluIENvbnRyb2wgVmFsaWRhdGVkMSEwHwYDVQQLExhQb3NpdGl2ZVNTTCBNdWx0
aS1Eb21haW4xGzAZBgNVBAMTEnd3dy50ZXJtb21ldHJvcy5wdDCCASIwDQYJKoZI
hvcNAQEBBQADggEPADCCAQoCggEBALEniG1m1e/FX77YLuYtDx4bf1gjoRKq2u6o
kIF1yuTUP7M5VB67V7bTHks7DLRErEubCyw6WBcQ3dUy6ICnpAEJibjwTpqIqMQT
9n3Flx2HU778hrGQCmWqhb5DnbHL6sSXCN/J0GmRkHysbQtqmQ+8UsM+AF8zos8W
LQrpNFTRU9eV+TAczVMqIC9t5dXZ0i9VO7oxgnnILjjW5KLj+krb+0vB1KsdrMTq
zdfAt0u5m1Ft4df+6NDjVvSvvtber3vakhnl8wFV4GFrbo54rKng+XVyQeyoEREo
jFp0cRRDA3vcDhX7pV6f7Q9OmQAtEncD7HEA2aq/38olMHrB5akCAwEAAaOCAqQw
ggKgMB8GA1UdIwQYMBaAFI2MXsRUrYrhd+mb+ZsF4bgBjWHhMB0GA1UdDgQWBBS2
T8H5CD9ipxhgNsHri1oZKloXKTAOBgNVHQ8BAf8EBAMCBaAwDAYDVR0TAQH/BAIw
ADAdBgNVHSUEFjAUBggrBgEFBQcDAQYIKwYBBQUHAwIwSQYDVR0gBEIwQDA0Bgsr
BgEEAbIxAQICBzAlMCMGCCsGAQUFBwIBFhdodHRwczovL3NlY3RpZ28uY29tL0NQ
UzAIBgZngQwBAgEwgYQGCCsGAQUFBwEBBHgwdjBPBggrBgEFBQcwAoZDaHR0cDov
L2NydC5zZWN0aWdvLmNvbS9TZWN0aWdvUlNBRG9tYWluVmFsaWRhdGlvblNlY3Vy
ZVNlcnZlckNBLmNydDAjBggrBgEFBQcwAYYXaHR0cDovL29jc3Auc2VjdGlnby5j
b20wRQYDVR0RBD4wPIISd3d3LnRlcm1vbWV0cm9zLnB0ghF3d3cuaWJ1dHRvbi5z
dG9yZYITd3d3LnJlZ2lzdGFkb3Jlcy5wdDCCAQYGCisGAQQB1nkCBAIEgfcEgfQA
8gB3ALvZ37wfinG1k5Qjl6qSe0c4V5UKq1LoGpCWZDaOHtGFAAABa4isdUIAAAQD
AEgwRgIhAKeCjMBWYxX+oKPQUqc/q9LOY4F4uhRcXnQX1DLaP3DEAiEAnG1P9uIK
JAc2FdieA5TYvyYhzcEufODfHg2GBpSg1/kAdwBep3P531bA57U2SH3QSeAyepGa
DIShEhKEGHWWgXFFWAAAAWuIrHOSAAAEAwBIMEYCIQC++yj6dECCzHUVIxyp4hc/
JXjbxeo9g79CaWnnZR5CJwIhAPQ20iVSj/aetrYXY0pmRDqgEzLMQVvPW76lOplb
r9XkMA0GCSqGSIb3DQEBCwUAA4IBAQDHa0lajW6XYoN2ij1BVKprjuLD6oqm9WyD
EoHg78fq4c2nrddr0ne4EtkPwqpH2HqWQtJ/79Q5DMYLpCmczFRdk9zwkV1pdnYT
ok2x7yLTY187/zHuKUvivfUF3GJpnSl8xdSzDnslS4Eph2p9FKXmvPUOmP20ftmk
0tmLSMThNu/E/OABqpuPZEajYxgNT/hh0tb7WxoS6yuOhaVLfoBepKKTxuenehBp
5HEnnYu6Jq80n7P62mGukQBc9WxZE2hjehJ8dCU126wPZBd514IcJ6hX6Y7SV07u
Twn2s38+Qtp6sfh8jSwNbts4wUauGV6Q8n15SxqF75jWvpYcO6ER
-----END CERTIFICATE-----";

        static string clientRsaSha256Crt =
@"-----BEGIN CERTIFICATE-----
MIIDPzCCAiegAwIBAgIBBDANBgkqhkiG9w0BAQsFADA7MQswCQYDVQQGEwJOTDER
MA8GA1UECgwIUG9sYXJTU0wxGTAXBgNVBAMMEFBvbGFyU1NMIFRlc3QgQ0EwHhcN
MTEwMjEyMTQ0NDA2WhcNMjEwMjEyMTQ0NDA2WjA8MQswCQYDVQQGEwJOTDERMA8G
A1UECgwIUG9sYXJTU0wxGjAYBgNVBAMMEVBvbGFyU1NMIENsaWVudCAyMIIBIjAN
BgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAyHTEzLn5tXnpRdkUYLB9u5Pyax6f
M60Nj4o8VmXl3ETZzGaFB9X4J7BKNdBjngpuG7fa8H6r7gwQk4ZJGDTzqCrSV/Uu
1C93KYRhTYJQj6eVSHD1bk2y1RPD0hrt5kPqQhTrdOrA7R/UV06p86jt0uDBMHEw
MjDV0/YI0FZPRo7yX/k9Z5GIMC5Cst99++UMd//sMcB4j7/Cf8qtbCHWjdmLao5v
4Jv4EFbMs44TFeY0BGbH7vk2DmqV9gmaBmf0ZXH4yqSxJeD+PIs1BGe64E92hfx/
/DZrtenNLQNiTrM9AM+vdqBpVoNq0qjU51Bx5rU2BXcFbXvI5MT9TNUhXwIDAQAB
o00wSzAJBgNVHRMEAjAAMB0GA1UdDgQWBBRxoQBzckAvVHZeM/xSj7zx3WtGITAf
BgNVHSMEGDAWgBS0WuSls97SUva51aaVD+s+vMf9/zANBgkqhkiG9w0BAQsFAAOC
AQEAlHabem2Tu69VUN7EipwnQn1dIHdgvT5i+iQHpSxY1crPnBbAeSdAXwsVEqLQ
gOOIAQD5VIITNuoGgo4i+4OpNh9u7ZkpRHla+/swsfrFWRRbBNP5Bcu74AGLstwU
zM8gIkBiyfM1Q1qDQISV9trlCG6O8vh8dp/rbI3rfzo99BOHXgFCrzXjCuW4vDsF
r+Dao26bX3sJ6UnEWg1H3o2x6PpUcvQ36h71/bz4TEbbUUEpe02V4QWuL+wrhHJL
U7o3SVE3Og7jPF8sat0a50YUWhwEFI256m02KAXLg89ueUyYKEr6rNwhcvXJpvU9
giIVvd0Sbjjnn7NC4VDbcXV8vw==
-----END CERTIFICATE-----";

        static string clientRsaKey =
@"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEAyHTEzLn5tXnpRdkUYLB9u5Pyax6fM60Nj4o8VmXl3ETZzGaF
B9X4J7BKNdBjngpuG7fa8H6r7gwQk4ZJGDTzqCrSV/Uu1C93KYRhTYJQj6eVSHD1
bk2y1RPD0hrt5kPqQhTrdOrA7R/UV06p86jt0uDBMHEwMjDV0/YI0FZPRo7yX/k9
Z5GIMC5Cst99++UMd//sMcB4j7/Cf8qtbCHWjdmLao5v4Jv4EFbMs44TFeY0BGbH
7vk2DmqV9gmaBmf0ZXH4yqSxJeD+PIs1BGe64E92hfx//DZrtenNLQNiTrM9AM+v
dqBpVoNq0qjU51Bx5rU2BXcFbXvI5MT9TNUhXwIDAQABAoIBAGdNtfYDiap6bzst
yhCiI8m9TtrhZw4MisaEaN/ll3XSjaOG2dvV6xMZCMV+5TeXDHOAZnY18Yi18vzz
4Ut2TnNFzizCECYNaA2fST3WgInnxUkV3YXAyP6CNxJaCmv2aA0yFr2kFVSeaKGt
ymvljNp2NVkvm7Th8fBQBO7I7AXhz43k0mR7XmPgewe8ApZOG3hstkOaMvbWAvWA
zCZupdDjZYjOJqlA4eEA4H8/w7F83r5CugeBE8LgEREjLPiyejrU5H1fubEY+h0d
l5HZBJ68ybTXfQ5U9o/QKA3dd0toBEhhdRUDGzWtjvwkEQfqF1reGWj/tod/gCpf
DFi6X0ECgYEA4wOv/pjSC3ty6TuOvKX2rOUiBrLXXv2JSxZnMoMiWI5ipLQt+RYT
VPafL/m7Dn6MbwjayOkcZhBwk5CNz5A6Q4lJ64Mq/lqHznRCQQ2Mc1G8eyDF/fYL
Ze2pLvwP9VD5jTc2miDfw+MnvJhywRRLcemDFP8k4hQVtm8PMp3ZmNECgYEA4gz7
wzObR4gn8ibe617uQPZjWzUj9dUHYd+in1gwBCIrtNnaRn9I9U/Q6tegRYpii4ys
c176NmU+umy6XmuSKV5qD9bSpZWG2nLFnslrN15Lm3fhZxoeMNhBaEDTnLT26yoi
33gp0mSSWy94ZEqipms+ULF6sY1ZtFW6tpGFoy8CgYAQHhnnvJflIs2ky4q10B60
ZcxFp3rtDpkp0JxhFLhiizFrujMtZSjYNm5U7KkgPVHhLELEUvCmOnKTt4ap/vZ0
BxJNe1GZH3pW6SAvGDQpl9sG7uu/vTFP+lCxukmzxB0DrrDcvorEkKMom7ZCCRvW
KZsZ6YeH2Z81BauRj218kQKBgQCUV/DgKP2985xDTT79N08jUo3hTP5MVYCCuj/+
UeEw1TvZcx3LJby7P6Xad6a1/BqveaGyFKIfEFIaBUBItk801sDDpDaYc4gL00Xc
7lFuBHOZkxJYlss5QrGpuOEl9ZwUt5IrFLBdYaKqNHzNVC1pCPfb/JyH6Dr2HUxq
gxUwAQKBgQCcU6G2L8AG9d9c0UpOyL1tMvFe5Ttw0KjlQVdsh1MP6yigYo9DYuwu
bHFVW2r0dBTqegP2/KTOxKzaHfC1qf0RGDsUoJCNJrd1cwoCLG8P2EF4w3OBrKqv
8u4ytY0F+Vlanj5lm3TaoHSVF1+NWPyOTiwevIECGKwSxvlki4fDAA==
-----END RSA PRIVATE KEY-----";
    }
}
