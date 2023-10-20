//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;
using System.Threading;
using AudioPlayer;
using nanoFramework.Hardware.Esp32;
using nanoFramework.System.IO.FileSystem;

// This Sample lets you play a WAV file from an connected SD card (Check SDCard samples for other connection types)
// Tasks:
// - download the following linked audio file: https://www.videvo.net/royalty-free-music-track/variation/232917/
// - convert it to a wav file (16 bits, 16 kHz, Mono)
// - OR use the one WAV file from this git repository.
//
// Copy the WAV file to a microSD card on the root (or adapt the audioFile variable correspondingly)
// Tested with 16 bits, 16 kHz, Mono with a MAX98357A breakout board.


// Beware: if you stop debugging while the file is open,
// re-plugin the USB cable to the MCU will re-start the application
// therefore might re-open the wav file and therefore also "lock" the file it again.
// This is why we explicitly wait until Debugger is attached, so that we can "reset"
// the MCU without automatically re-locking the file.
while (!Debugger.IsAttached)
{
    Thread.Sleep(500);
}

// SD Card:
const uint cs = 5;
Configuration.SetPinFunction(23, DeviceFunction.SPI1_MOSI);
Configuration.SetPinFunction(18, DeviceFunction.SPI1_CLOCK);
Configuration.SetPinFunction(19, DeviceFunction.SPI1_MISO);

var sdCard = new SDCard(new SDCard.SDCardSpiParameters { spiBus = 1, chipSelectPin = cs });
sdCard.Mount();

// NOTE: If the audio has low quality and lots of static you may need to update
// I2sWavPlayer to add the I2sMode.Pdm flag when configuring I2sConnectionSettings

const string audioFile = "D:\\Variation-CLJ013901.wav";
var player = new I2sWavPlayer(I2sWavPlayer.Bus.One, audioFile);
player.Play();
player.Dispose();

sdCard.Unmount();
sdCard.Dispose();
