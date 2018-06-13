//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) 2016 STMicroelectronics.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;

namespace nanoFramework.Drivers
{
    public class L3GD20
    {

        /// <summary>
        /// most significant bit of address set to 1 for read operations
        /// </summary>
        private const byte READ_BIT = 0x80;

        /// <summary>
        /// Multiple address R/W operation on which the address is automatically incremented
        /// </summary>
        private const byte MULTIPLE_OPERATION_BIT = 0x40;

        /// <summary>
        /// device identification register
        /// </summary>
        private const byte WHO_AM_I = 0x0F;

        /// <summary>
        ///  Control register 1
        /// </summary>
        private const byte ControlRegister1 = 0x20;

        /// <summary>
        /// Control register 2
        /// </summary>
        private const byte ControlRegister2 = 0x21;

        /// <summary>
        /// Control register 3
        /// </summary>
        private const byte ControlRegister3 = 0x22;

        /// <summary>
        /// Control register 4
        /// </summary>
        private const byte ControlRegister4 = 0x23;

        /// <summary>
        /// Control register 5
        /// </summary>
        private const byte ControlRegister5 = 0x24;

        /// <summary>
        /// Reference register
        /// </summary>
        private const byte REFERENCE_REG = 0x25;

        /// <summary>
        ///  Out temp register 
        /// </summary>
        private const byte OUT_TEMP = 0x26;

        /// <summary>
        /// Status register
        /// </summary>
        private const byte STATUS_REG = 0x27;

        /// <summary>
        /// Output Register X
        /// </summary>
        private const byte OUT_X_L = 0x28;

        /// <summary>
        /// Output Register X
        /// </summary>
        private const byte OUT_X_H = 0x29;

        /// <summary>
        /// Output Register Y
        /// </summary>
        private const byte OUT_Y_L = 0x2A;

        /// <summary>
        /// Output Register Y
        /// </summary>
        private const byte OUT_Y_H = 0x2B;

        /// <summary>
        /// Output Register Z
        /// </summary>
        private const byte OUT_Z_L = 0x2C;

        /// <summary>
        /// Output Register Z
        /// </summary>
        private const byte OUT_Z_H = 0x2D;

        /// <summary>
        /// FIFO control Register
        /// </summary>
        private const byte FIFO_ControlRegister = 0x2E;

        /// <summary>
        /// FIFO src Register
        /// </summary>
        private const byte FIFO_SRC_REG = 0x2F;

        /// <summary>
        /// Interrupt 1 configuration Register
        /// </summary>
        private const byte INT1_CFG = 0x30;

        /// <summary>
        /// Interrupt 1 source Register
        /// </summary>
        private const byte INT1_SRC = 0x31;

        /// <summary>
        /// Interrupt 1 Threshold X register
        /// </summary>
        private const byte INT1_TSH_XH = 0x32;

        /// <summary>
        /// Interrupt 1 Threshold X register
        /// </summary>
        private const byte INT1_TSH_XL = 0x33;

        /// <summary>
        /// Interrupt 1 Threshold Y register
        /// </summary>
        private const byte INT1_TSH_YH = 0x34;

        /// <summary>
        /// Interrupt 1 Threshold Y register
        /// </summary>
        private const byte INT1_TSH_YL = 0x35;

        /// <summary>
        /// Interrupt 1 Threshold Z register
        /// </summary>
        private const byte INT1_TSH_ZH = 0x36;

        /// <summary>
        /// Interrupt 1 Threshold Z register
        /// </summary>
        private const byte INT1_TSH_ZL = 0x37;

        /// <summary>
        /// Interrupt 1 DURATION register
        /// </summary>
        private const byte INT1_DURATION = 0x38;

        /// <summary>
        /// This is the ID reading from the <see cref="WHO_AM_I"/> register.
        /// </summary>
        private const byte I_AM_L3GD20 = 0xD4;


        private byte _chipId;

        private readonly SpiDevice _gyroscope;
        private readonly GpioPin _chipSelectLine;

        public L3GD20(string spiBus, GpioPin chipSelectLine)
        {
            _chipSelectLine = chipSelectLine;
            _chipSelectLine.SetDriveMode(GpioPinDriveMode.Output);

            var connectionSettings = new SpiConnectionSettings(chipSelectLine.PinNumber);
            connectionSettings.DataBitLength = 8;
            connectionSettings.ClockFrequency = 10000000;

            // create SPI device for gyroscope
            _gyroscope = SpiDevice.FromId(spiBus, connectionSettings);
        }

        public void Initialize(DataRate ouputDataRate = DataRate._95Hz, 
            AxisSelection axisSelection = AxisSelection.All, 
            PowerMode powerMode = PowerMode.Active, 
            HighPassFilterMode hpMode = HighPassFilterMode.Normal,
            HighPassConfiguration hpConfiguration = HighPassConfiguration.HPConf0, 
            Scale scale = Scale._0250, 
            LowPass2Mode lpMode = LowPass2Mode.Bypassed)
        {
            // we are setting the 5 control registers in a single SPI write operation 
            // by taking advantage on the consecutive write capability
            byte[] configBuffer = new byte[5];

            // control register 1
            configBuffer[0] = (byte)axisSelection;
            configBuffer[0] |= (byte)powerMode;
            configBuffer[0] |= (byte)ouputDataRate;

            // control register 2
            if (hpMode != HighPassFilterMode.Bypassed)
            {
                configBuffer[1] = (byte)hpConfiguration;
            }

            // control register 3 skipped

            // control register 4
            configBuffer[3] = (byte)scale;
            // TDB 
            // block auto-update 
            // endianess

            // control register 5
            if (hpMode != HighPassFilterMode.Bypassed)
            {
                // high pass filter enabled
                configBuffer[4] = 0x10;

                if(lpMode != LowPass2Mode.Bypassed)
                {
                    configBuffer[4] |= 0x08 | 0x02;
                }
                else
                {
                    configBuffer[4] |= 0x04 | 0x01;
                }
            }

            WriteOperation(ControlRegister1, configBuffer);
        }

        /// <summary>
        /// Device identification.
        /// </summary>
        public byte ChipID
        {
            get
            {
                // do we have this already?
                if (_chipId == 0)
                {
                    // no, need to get it from the device
                    byte[] buffer = new byte[1];

                    ReadOperation(WHO_AM_I, buffer);
                    _chipId = buffer[0];

                    // sanity check
                    if (_chipId != I_AM_L3GD20)
                    {
                        throw new ApplicationException();
                    }

                    return buffer[0];
                }

                return _chipId;
            }
        }

        public int[] GetXYZ()
        {
            byte[] buffer = new byte[2 * 3];
            int[] readings = new int[3];

            // read raw data from gyroscope registers 
            // by taking advantage on the consecutive read capability
            ReadOperation(OUT_X_L, buffer);

            readings[0] = buffer[2 * 0] + (buffer[2 * 0 + 1] << 8);
            readings[1] = buffer[2 * 1] + (buffer[2 * 1 + 1] << 8);
            readings[2] = buffer[2 * 2] + (buffer[2 * 2 + 1] << 8);

            return readings;
        }

        //void LowPower(uint16_t InitStruct);

        //void RebootCmd();

        ///* Interrupt Configuration Functions
        //void INT1InterruptConfig(uint16_t Int1Config);
        //void EnableIT(uint8_t IntSel);
        //void DisableIT(uint8_t IntSel);

        ///* High Pass Filter Configuration Functions
        //void FilterConfig(uint8_t FilterStruct);
        //void FilterCmd(uint8_t HighPassFilterState);
        //void ReadXYZAngRate(float* pfData);
        //uint8_t GetDataStatus();

        ///* Gyroscope IO functions
        //void GYRO_IO_Init();
        //void GYRO_IO_DeInit();
        //void GYRO_IO_Write(uint8_t* pBuffer, uint8_t WriteAddr, uint16_t NumByteToWrite);
        //void GYRO_IO_Read(uint8_t* pBuffer, uint8_t ReadAddr, uint16_t NumByteToRead);

        //
        // for SPI operations with L3GD20 the address has to be tweaked by setting the R/W bit and the MS bit for operations with multiple read/writes
        private void ReadOperation(byte address, byte[] buffer)
        {
            // read operations have to set R/W bite
            address |= READ_BIT;

            // multiple address access has to set MS bit to have the read address auto-increment
            address |= buffer.Length > 1 ? MULTIPLE_OPERATION_BIT : (byte)0x00;

            // set CS line low to select the device
            //_chipSelectLine.Write(GpioPinValue.Low);

            _gyroscope.TransferSequential(new byte[] { address }, buffer);

            // set CS line high to unselect the device
            //_chipSelectLine.Write(GpioPinValue.High);
        }

        private void WriteOperation(byte address, byte[] buffer)
        {
            // write operations have to reset MSb
            // don't do anything about this as all address constants already have that

            // if this is a multiple address write set MS bit
            address |= buffer.Length > 1 ? MULTIPLE_OPERATION_BIT : (byte)0x00;

            // need to create a write buffer with the address as the first element, followed by the data to be written
            byte[] writeBuffer = new byte[1 + buffer.Length];
            writeBuffer[0] = address;
            // copy address to start of write buffer
            Array.Copy(buffer, 0, writeBuffer, 1, buffer.Length);

            // set CS line low to select the device
            //_chipSelectLine.Write(GpioPinValue.Low);

            _gyroscope.Write(writeBuffer);

            // set CS line high to unselect the device
            //_chipSelectLine.Write(GpioPinValue.High);
        }

        #region Enums

        public enum PowerMode : byte
        {
            PowerDown = 0x00,
            Active = 0x08
        }

        public enum DataRate : byte
        {
            /// <summary>
            /// Output data rate 95 Hz.
            /// </summary>
            _95Hz = 0x00,

            /// <summary>
            /// Output data rate 190 Hz.
            /// </summary>
            _190Hz = 0x40,

            /// <summary>
            /// Output data rate 380 Hz.
            /// </summary>
            _380Hz = 0x80,

            /// <summary>
            /// Output data rate 760 Hz.
            /// </summary>
            _760Hz = 0xC0,
        }

        [Flags]
        public enum AxisSelection : byte
        {
            None = 0,

            Y = 0x01,
            X = 0x02,
            Z = 0x04,

            All = X | Y | Z,
        }

        public enum Scale : byte
        {
            _0250 = 0x00,
            _0500 = 0x10,
            _2000 = 0x20,
        }
        public enum Sensivity
        {
            _0250dps,
            _0500dps,
            _2000dps,
        }

        /// <summary>
        /// low pass filter 1 bandwidth, depends on Output data rate ODR.
        /// </summary>
        public enum LPFilter1Bandwith : byte
        {
            BW0 = 0x00,
            BW1 = 0x40,
            BW2 = 0x80,
            BW3 = 0xC0,
        }

        public enum HighPassFilterMode : byte
        {
            Normal = 0x00,
            ReferenceSignal = 0x10,
            AutoReset = 0x30,
            Bypassed = 0xFF,
        }

        /// <summary>
        /// High-Pass filter configuration. depends on Output data rate ODR. See table 26 on datasheet for details)
        /// </summary>
        public enum HighPassConfiguration : byte
        {
            HPConf0 = 0x00,
            HPConf1 = 0x01,
            HPConf2 = 0x02,
            HPConf3 = 0x03,
            HPConf4 = 0x04,
            HPConf5 = 0x05,
            HPConf6 = 0x06,
            HPConf7 = 0x07,
            HPConf8 = 0x08,
            HPConf9 = 0x09,
        }

        public enum LowPass2Mode
        {
            On,
            Bypassed,
        }

        public enum Endianess : byte
        {
            LittleEndian = 0x00,
            BigEndian = 040,
        }

        #endregion
    }
}
