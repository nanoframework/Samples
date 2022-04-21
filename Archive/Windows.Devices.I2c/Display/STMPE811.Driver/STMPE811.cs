//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) 2016 STMicroelectronics.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using Windows.Devices.I2c;

namespace Driver
{
    public class STMPE811
    {
        private const UInt16 STMPE811_CHIPID = 0x0811;

        // Identification registers & System Control
        private const byte REGISTER_CHIP_ID  = 0x00;
        private const byte REGISTER_ID_VER = 0x02;

        // Global interrupt Enable bit
        private const byte GIT_EN = 0x01;

        // IO expander functionalities
        private const byte ADC_FCT = 0x01;
        private const byte TSC_FCT = 0x02;
        private const byte IO_FCT = 0x04;
        private const byte TEMPSENS_FCT = 0x08;

        // General Control Registers
        private const byte REGISTER_SYS_CTRL1 = 0x03;
        private const byte REGISTER_SYS_CTRL2 = 0x04;
        private const byte REGISTER_SPI_CFG = 0x08;

        // Interrupt system Registers
        private const byte REGISTER_INT_CTRL = 0x09;
        private const byte REGISTER_INT_EN = 0x0A;
        private const byte REGISTER_INT_STA = 0x0B;
        private const byte REGISTER_IO_INT_EN = 0x0C;
        private const byte REGISTER_IO_INT_STA = 0x0D;

        // IO Registers
        private const byte REGISTER_IO_SET_PIN = 0x10;
        private const byte REGISTER_IO_CLR_PIN = 0x11;
        private const byte REGISTER_IO_MP_STA = 0x12;
        private const byte REGISTER_IO_DIR = 0x13;
        private const byte REGISTER_IO_ED = 0x14;
        private const byte REGISTER_IO_RE = 0x15;
        private const byte REGISTER_IO_FE = 0x16;
        private const byte REGISTER_IO_AF = 0x17;

        // ADC Registers
        private const byte REGISTER_ADC_INT_EN = 0x0E;
        private const byte REGISTER_ADC_INT_STA = 0x0F;
        private const byte REGISTER_ADC_CTRL1 = 0x20;
        private const byte REGISTER_ADC_CTRL2 = 0x21;
        private const byte REGISTER_ADC_CAPT = 0x22;
        private const byte REGISTER_ADC_DATA_CH0 = 0x30;
        private const byte REGISTER_ADC_DATA_CH1 = 0x32;
        private const byte REGISTER_ADC_DATA_CH2 = 0x34;
        private const byte REGISTER_ADC_DATA_CH3 = 0x36;
        private const byte REGISTER_ADC_DATA_CH4 = 0x38;
        private const byte REGISTER_ADC_DATA_CH5 = 0x3A;
        private const byte REGISTER_ADC_DATA_CH6 = 0x3B;
        private const byte REGISTER_ADC_DATA_CH7 = 0x3C;

        // Touch Screen Registers
        private const byte REGISTER_TSC_CTRL = 0x40;
        private const byte REGISTER_TSC_CFG = 0x41;
        private const byte REGISTER_WDM_TR_X = 0x42;
        private const byte REGISTER_WDM_TR_Y = 0x44;
        private const byte REGISTER_WDM_BL_X = 0x46;
        private const byte REGISTER_WDM_BL_Y = 0x48;
        private const byte REGISTER_FIFO_TH = 0x4A;
        private const byte REGISTER_FIFO_STA = 0x4B;
        private const byte REGISTER_FIFO_SIZE = 0x4C;
        private const byte REGISTER_TSC_DATA_X = 0x4D;
        private const byte REGISTER_TSC_DATA_Y = 0x4F;
        private const byte REGISTER_TSC_DATA_Z = 0x51;
        private const byte REGISTER_TSC_DATA_XYZ = 0x52;
        private const byte REGISTER_TSC_FRACT_XYZ = 0x56;
        private const byte REGISTER_TSC_DATA_INC = 0x57;
        private const byte REGISTER_TSC_DATA_NON_INC = 0xD7;
        private const byte REGISTER_TSC_I_DRIVE = 0x58;
        private const byte REGISTER_TSC_SHIELD = 0x59;

        // TS registers masks
        private const byte TS_CTRL_ENABLE = 0x01;
        private const byte TS_CTRL_STATUS = 0x80;

        private readonly int _address;
        private I2cDevice _touchController;
        private UInt16 _chipId;
        private byte _revisionNumber;

        /// <summary>
        /// I2C address of the STMPE811 device.
        /// </summary>
        public int Address => _address;

        /// <summary>
        /// Creates a driver for the STMPE811.
        /// </summary>
        /// <param name="address">The I2C address of the device.</param>
        /// <param name="i2cBus">The I2C bus where the device is connected to.</param>
        public STMPE811(int address, string i2cBus)
        {
            // store I2C address
            _address = address;

            // instantiate I2C controller
            _touchController = I2cDevice.FromId(i2cBus, new I2cConnectionSettings(address));
        }

        /// <summary>
        /// Device identification.
        /// </summary>
        public UInt16 ChipID
        {
            get
            {
                // do we have this already?
                if (_chipId == 0)
                {
                    // no, need to get it from the device
                    byte[] buffer = new byte[2];

                    var readResult = _touchController.WriteReadPartial(new byte[] { REGISTER_CHIP_ID }, buffer);
                    _chipId = (UInt16)((buffer[0] << 8) + buffer[1]);
                }

                return _chipId;
            }
        }

        /// <summary>
        /// Revision number.
        /// </summary>
        public byte RevisionNumber
        {
            get
            {
                // do we have this already?
                if (_revisionNumber == 0)
                {
                    // no, need to get it from the device
                    byte[] buffer = new byte[1];

                    var readResult = _touchController.WriteReadPartial(new byte[] { REGISTER_ID_VER }, buffer);
                    _revisionNumber = buffer[0];
                }

                return _revisionNumber;
            }
        }

        /// <summary>
        /// Performs a software reset of the STMPE811 chip.
        /// </summary>
        public void Reset()
        {
            // the recommend times between these calls are being respected because of the time to execute each of these calls
            _touchController.Write(new byte[] { REGISTER_SYS_CTRL1, 0x02 });
            Thread.Sleep(10);
            _touchController.Write(new byte[] { REGISTER_SYS_CTRL1, 0x00 });
        }

        /// <summary>
        /// Perform initialization of the device.
        /// </summary>
        public bool Initialize()
        {
            // reset chip
            Reset();

            // check reset by reading the chip ID
            if (ChipID != STMPE811_CHIPID)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Enable the Global interrupt.
        /// </summary>
        public void EnableGlobalInterrupt()
        {
            byte[] buffer = new byte[1];

            // Read the Interrupt Control register 
            _touchController.WriteReadPartial(new byte[] { REGISTER_INT_CTRL }, buffer);

            // Set the global interrupts to be Enabled
            buffer[0] |= GIT_EN;

            // Write Back the Interrupt Control register
            _touchController.WritePartial(new byte[] { REGISTER_INT_CTRL, buffer[0] });
        }

        /// <summary>
        /// Disable the Global interrupt.
        /// </summary>
        public void DisableGlobalInterrupt()
        {
            byte[] buffer = new byte[1];

            // Read the Interrupt Control register 
            _touchController.WriteReadPartial(new byte[] { REGISTER_INT_CTRL }, buffer);

            // Set the global interrupts to be disabled
            buffer[0] &= 0xFE;

            // Write Back the Interrupt Control register
            _touchController.WritePartial(new byte[] { REGISTER_INT_CTRL, buffer[0] });
        }

        /// <summary>
        /// Enable the interrupt mode for the selected IT source.
        /// </summary>
        /// <param name="source">The interrupt source to be configured</param>
        public void EnableInterruptSource(InterruptSource source)
        {
            byte[] buffer = new byte[1];

            // Get the current value of the INT_EN register
            _touchController.WriteReadPartial(new byte[] { REGISTER_INT_EN }, buffer);

            // Set the interrupts to be Enabled
            buffer[0] |= (byte)source;

            // Write Back the Interrupt Control register
            _touchController.WritePartial(new byte[] { REGISTER_INT_EN, buffer[0] });
        }

        /// <summary>
        /// Disable the interrupt mode for the selected IT source.
        /// </summary>
        /// <param name="source">The interrupt source to be configured</param>
        public void DisableInterruptSource(InterruptSource source)
        {
            byte[] buffer = new byte[1];

            // Get the current value of the INT_EN register
            _touchController.WriteReadPartial(new byte[] { REGISTER_INT_EN }, buffer);

            // Set the interrupts to be Enabled
            buffer[0] &= (byte)(~(byte)source);

            // Write Back the Interrupt Control register
            _touchController.WritePartial(new byte[] { REGISTER_INT_EN, buffer[0] });
        }

        /// <summary>
        /// Set the global interrupt Polarity.
        /// </summary>
        /// <param name="polarity">The interrupt mode polarity</param>
        public void SetInterruptPolarity(InterruptPolarity polarity)
        {
            byte[] buffer = new byte[1];

            // Get the current register value
            _touchController.WriteReadPartial(new byte[] { REGISTER_INT_CTRL }, buffer);

            // Mask the polarity bits
            buffer[0] &= 0xFB; // equivalent to ~0x04

            // Modify the Interrupt Output line configuration
            buffer[0] |= (byte)polarity;

            // Write Back the Interrupt Control register
            _touchController.WritePartial(new byte[] { REGISTER_INT_CTRL, buffer[0] });
        }

        /// <summary>
        /// Set the global interrupt Type.
        /// </summary>
        /// <param name="polarity">Interrupt line activity type.</param>
        public void SetInterruptType(InterruptType type)
        {
            byte[] buffer = new byte[1];

            // Get the current register value
            _touchController.WriteReadPartial(new byte[] { REGISTER_INT_CTRL }, buffer);

            // Mask the type bits
            buffer[0] &= 0xFD; // equivalent to ~0x02

            // Modify the Interrupt Output line configuration
            buffer[0] |= (byte)type;

            // Write Back the Interrupt Control register
            _touchController.WritePartial(new byte[] { REGISTER_INT_CTRL, buffer[0] });
        }

        /// <summary>
        ///  Check the selected Global interrupt source pending bit
        /// </summary>
        /// <param name="source">The Global interrupt source(s) to be checked.</param>
        /// <returns>The checked Global interrupt source status.</returns>
        public bool GlobalInterruptStatus(InterruptSource source)
        {
            byte[] buffer = new byte[1];

            // Get the current register value
            _touchController.WriteReadPartial(new byte[] { REGISTER_INT_STA }, buffer);

            return (((InterruptSource)buffer[0] & source) == source);
        }

        /// <summary>
        /// Return the Global interrupts status
        /// </summary>
        /// <param name="source">The Global interrupt source(s) to be checked</param>
        /// <returns>The checked Global interrupt source status.</returns>
        public InterruptSource ReadGlobalInterruptStatus(InterruptSource source = InterruptSource.All)
        {
            byte[] buffer = new byte[1];

            // Get the current register value
            _touchController.WriteReadPartial(new byte[] { REGISTER_INT_STA }, buffer);

            return (InterruptSource)(buffer[0] & (byte)source);
        }

        /// <summary>
        /// Clear the selected Global interrupt pending bit(s).
        /// </summary>
        /// <param name="source">The Global interrupt source(s) to be cleared</param>
        public void ClearGlobalInterrupt(InterruptSource source)
        {
            // Write 1 to the bits that have to be cleared
            _touchController.WritePartial(new byte[] { REGISTER_INT_STA, (byte)source });
        }

        /// <summary>
        /// Start the IO functionality use and disable the AF for selected IO pin.
        /// </summary>
        /// <param name="pin">The IO pin(s) to put in AF.</param>
        public void IOStart(IOPins pin)
        {
            byte[] buffer = new byte[1];

            // Get the current register value
            _touchController.WriteReadPartial(new byte[] { REGISTER_SYS_CTRL2 }, buffer);

            // Set the Functionalities to be Disabled
            buffer[0] &= 0xBE; // equivalent to ~(IO_FCT | ADC_FCT)

            // Write the new register value
            _touchController.WritePartial(new byte[] { REGISTER_SYS_CTRL2, buffer[0] });

            // Disable AF for the selected IO pin(s)
            DisableAF(pin);
        }

        /// <summary>
        /// Configures the IO pin according to IO mode value.
        /// </summary>
        /// <param name="pin">The output pin to be set or reset.</param>
        /// <param name="mode">The IO pin mode to configure.</param>
        public void IOConfig(IOPins pin, IOMode mode)
        {
            switch(mode)
            {
                case IOMode.Input:
                    InitializePin(pin, IODirection.In);
                    break;

                case IOMode.Output:
                    InitializePin(pin, IODirection.Out);
                    break;

                case IOMode.InterruptRisingEdge:
                    EnableGlobalInterrupt();
                    //EnablePinInterrupt(pin);
                    InitializePin(pin, IODirection.In);
                    //IOSetEdgeMode(pin, Edge.Rising);
                    break;

                case IOMode.InterruptFallingEdge:
                    EnableGlobalInterrupt();
                    //EnablePinInterrupt(pin);
                    InitializePin(pin, IODirection.In);
                    //IOSetEdgeMode(pin, Edge.Falling);
                    break;

                case IOMode.InterruptLowLevel:
                    EnableGlobalInterrupt();
                    //EnablePinInterrupt(pin);
                    InitializePin(pin, IODirection.In);
                    SetInterruptType(InterruptType.Level);
                    SetInterruptPolarity(InterruptPolarity.Low);
                    break;

                case IOMode.InterruptHighLevel:
                    EnableGlobalInterrupt();
                    //EnablePinInterrupt(pin);
                    InitializePin(pin, IODirection.In);
                    SetInterruptType(InterruptType.Level);
                    SetInterruptPolarity(InterruptPolarity.High);
                    break;

                default:
                    throw new ArgumentException(); 
            }
        }

        /// <summary>
        /// Initialize the selected IO pin direction.
        /// </summary>
        /// <param name="pin">The IO pin to be configured.</param>
        /// <param name="direction">The IO pin direction.</param>
        public void InitializePin(IOPins pin, IODirection direction)
        {
            byte[] buffer = new byte[1];

            // Get all the Pins direction
            _touchController.WriteReadPartial(new byte[] { REGISTER_IO_DIR }, buffer);

            // Set the selected pin direction
            if (direction != IODirection.In)
            {
                buffer[0] |= (byte)pin;
            }
            else
            {
                buffer[0] &= (byte)(~((byte)pin));
            }

            // Write the register with the new value
            _touchController.WritePartial(new byte[] { REGISTER_IO_DIR, buffer[0] });
        }

        /// <summary>
        /// Disable the AF for the selected IO pin.
        /// </summary>
        /// <param name="pin">The IO pin to be configured.</param>
        public void DisableAF(IOPins pin)
        {
            byte[] buffer = new byte[1];

            // Get the current state of the IO_AF register
            _touchController.WriteReadPartial(new byte[] { REGISTER_IO_AF }, buffer);

            // Disable the selected pins alternate function
            buffer[0] |= (byte)pin;

            // Write back the new value in IO AF register 
            _touchController.WritePartial(new byte[] { REGISTER_IO_AF, buffer[0] });
        }

        /// <summary>
        /// Enable the AF for the selected IO pin(s).
        /// </summary>
        /// <param name="pins">The IO pin(s) to be configured.</param>
        public void EnableAF(IOPins pins)
        {
            byte[] buffer = new byte[1];

            // Get the current state of the IO_AF register
            _touchController.WriteReadPartial(new byte[] { REGISTER_IO_AF }, buffer);

            // Enable the selected pins alternate function
            buffer[0] &= (byte)(~((byte)pins));

            // Write back the new value in IO AF register 
            _touchController.WritePartial(new byte[] { REGISTER_IO_AF, buffer[0] });
        }

        // TODO
        //public void IOSetEdgeMode(IOPins pin, Edge edge)

        // TODO
        //public void IOWritePin(IOPins pin, PinState state)

        // TODO
        //public PinState IOReadPin(IOPins pin)

        /// <summary>
        /// Enable the global IO interrupt source.
        /// </summary>
        public void EnableInterrupt()
        {
            // Enable global IO IT source
            EnableInterruptSource(InterruptSource.IO);

            // Enable global interrupt
            EnableGlobalInterrupt();
        }

        /// <summary>
        /// Disable the global IO interrupt source.
        /// </summary>
        public void DisableInterrupt()
        {
            // Disable the global interrupt
            DisableGlobalInterrupt();

            // Disable global IO IT source
            DisableInterruptSource(InterruptSource.IO);
        }

        /// <summary>
        /// Enable interrupt mode for the selected IO pin(s).
        /// </summary>
        /// <param name="pins">The IO interrupt to be enabled.</param>
        public void EnableInterrupt(IOPins pins)
        {
            byte[] buffer = new byte[1];

            // Get the IO interrupt state 
            _touchController.WriteReadPartial(new byte[] { REGISTER_IO_INT_EN }, buffer);

            // Set the interrupts to be enabled
            buffer[0] |= (byte)pins;

            // Write back the new value in the register 
            _touchController.WritePartial(new byte[] { REGISTER_IO_INT_EN, buffer[0] });
        }

        /// <summary>
        /// Disable interrupt mode for the selected IO pin(s).
        /// </summary>
        /// <param name="pins">The IO interrupt to be enabled.</param>
        public void DisableInterrupt(IOPins pins)
        {
            byte[] buffer = new byte[1];

            // Get the IO interrupt state 
            _touchController.WriteReadPartial(new byte[] { REGISTER_IO_INT_EN }, buffer);

            // Set the interrupts to be Disabled
            buffer[0] &= (byte)(~(byte)pins);

            // Write back the new value in the register 
            _touchController.WritePartial(new byte[] { REGISTER_IO_INT_EN, buffer[0] });
        }

        // TODO
        // public void IOInterruptStatus(IOPins pins)

        // TODO
        // public void IOClearInterrupt(IOPins pins)

        /// <summary>
        /// Configures the touch Screen Controller (Single point detection)
        /// </summary>
        public void Start()
        {
            byte[] buffer = new byte[1];

            // Get the current register value
            _touchController.WriteReadPartial(new byte[] { REGISTER_SYS_CTRL2 }, buffer);
            // enable clock of ADC and touchscreen controller
            buffer[0] &= 0xFC; // equivalent to ~(TSC_FCT | ADC_FCT);

            // Set the Functionalities to be Enabled
            buffer[0] &= 0xFB; // equivalent to ~IO_FCT;

            // Write back the new value in the register 
            _touchController.WritePartial(new byte[] { REGISTER_SYS_CTRL2, buffer[0] });

            // Select TSC pins in TSC alternate mode
            EnableAF(IOPins.TouchPins);

            // Select Sample Time, bit number and ADC Reference 
            // sample time is 80 clk periods
            // resolution is 12 bits
            // using external reference
            _touchController.WritePartial(new byte[] { REGISTER_ADC_CTRL1, 0x49 });
            
            // ADC clock speed is 3.25MHz
            _touchController.WritePartial(new byte[] { REGISTER_ADC_CTRL2, 0x01 });

            // Select 2 nF filter capacitor
            // configure touch screen controller
            // 1 samples
            // Touch detect delay is 500us
            // Panel driver settling time is 500us
            _touchController.WritePartial(new byte[] { REGISTER_TSC_CFG, 0x1B });

            // Configure the Touch FIFO threshold: single point reading
            _touchController.WritePartial(new byte[] { REGISTER_FIFO_TH, 0x01 });

            // Set the range and accuracy of the pressure measurement (Z)
            // factional part is 7
            // whole part is 1
            _touchController.WritePartial(new byte[] { REGISTER_TSC_FRACT_XYZ, 0x01 });

            // Set the driving capability (limit) of the device for TSC pins: 50mA
            _touchController.WritePartial(new byte[] { REGISTER_TSC_I_DRIVE, 0x01 });

            // XY acquisition (location)
            // no window tracking
            _touchController.WritePartial(new byte[] { REGISTER_TSC_CTRL,  0x00 });

            // read register
            _touchController.WriteReadPartial(new byte[] { REGISTER_TSC_CTRL }, buffer);
            buffer[0] |= TS_CTRL_ENABLE;
            // Write back the new value in the register 
            _touchController.WritePartial(new byte[] { REGISTER_TSC_CTRL, buffer[0] });

            // clear any interrupt pending by reading the register
            ClearGlobalInterrupt(InterruptSource.All);
        }

        /// <summary>
        /// Read current FIFO size.
        /// </summary>
        /// <returns>The number of samples waiting in the FIFO to be read.</returns>
        public int FifoSize()
        {
            byte[] buffer = new byte[1];

            // Get the current register value
            _touchController.WriteReadPartial(new byte[] { REGISTER_FIFO_SIZE }, buffer);

            return buffer[0];
        }

        public bool IsTouchDetected()
        {
            byte[] buffer = new byte[1];

            _touchController.WriteReadPartial(new byte[] { REGISTER_TSC_CTRL }, buffer);

            return ((buffer[0] & TS_CTRL_STATUS) == 0);
        }

        /// <summary>
        /// Get a reading from the touch screen.
        /// </summary>
        /// <returns>A <see cref="Reading"/> with data about the touch status and coordinates</returns>
        public Reading ReadTouch()
        {
            uint valueX;
            uint valueY;
            uint valueZ;
            UInt32 rawValue;
            Reading reading = new Reading();

            byte[] buffer = new byte[1];

            //Debug.WriteLine("Current FIFO " + FifoSize());

            if(FifoSize() > 0)
            { 
                // read X, Y & Z
                buffer = new byte[4];
                if (_touchController.WriteReadPartial(new byte[] { REGISTER_TSC_DATA_XYZ }, buffer).Status == I2cTransferStatus.FullTransfer)
                {
                    rawValue = (((UInt32)buffer[0] << 24)) | ((UInt32)(buffer[1] << 16)) | ((UInt32)(buffer[2] << 8)) | ((UInt32)(buffer[3] << 0));

                    valueX = (rawValue >> 20) & 0x00000FFF;
                    valueY = (rawValue >> 8) & 0x00000FFF;
                    valueZ = rawValue & 0x000000FF;

                    reading = new Reading(valueX, valueY, valueZ);
                }
            }

            //Debug.WriteLine("Current FIFO " + FifoSize());

            // reset FIFO
            //ResetFIFO();

            //Debug.WriteLine("Current FIFO " + FifoSize());

            return reading;
        }

        /// <summary>
        /// Reset the FIFO by clearing its contents.
        /// </summary>
        public void ResetFIFO()
        {
            _touchController.Write(new byte[] { REGISTER_FIFO_STA, 0x01 });
            _touchController.Write(new byte[] { REGISTER_FIFO_STA, 0 });
        }

        #region enums

        /// <summary>
        /// The polarity of the global interrupt signal.
        /// </summary>
        public enum InterruptPolarity : byte
        {
            /// <summary>
            /// Interrupt line is active Low/Falling edge  
            /// </summary>
            Low = 0,
            /// <summary>
            /// Interrupt line is active High/Rising edge
            /// </summary>
            High = 4
        }

        /// <summary>
        /// The type of global interrupt signal.
        /// </summary>
        public enum InterruptType : byte
        {
            /// <summary>
            /// Interrupt line is active in level model 
            /// </summary>
            Level = 0,
            /// <summary>
            /// Interrupt line is active in edge model
            /// </summary>
            Edge = 2
        }

        /// <summary>
        /// Global interrupt sources
        /// </summary>
        [Flags]
        public enum InterruptSource : byte
        {
            /// <summary>
            /// Touch is detected 
            /// </summary>
            Touch = 0x01,
            /// <summary>
            /// FIFO above threshold
            /// </summary>
            FifoAboveThreshold = 0x02,
            /// <summary>
            /// FIFO overflowed 
            /// </summary>
            FifoOverflowed = 0x04,
            /// <summary>
            /// FIFO full
            /// </summary>
            FifoFull = 0x08,
            /// <summary>
            /// FIFO empty
            /// </summary>
            FifoEmpty = 0x10,
            /// <summary>
            /// ADC interrupt
            /// </summary>
            ADC = 0x40,
            /// <summary>
            /// IO interrupt 
            /// </summary>
            IO = 0x80,

            /// <summary>
            /// All interrupt sources
            /// </summary>
            All = 0xFF,

            /// <summary>
            /// All FIFO related interrupts
            /// </summary>
            AllFIFOs = FifoAboveThreshold | FifoEmpty | FifoFull | FifoOverflowed,
        }

        /// <summary>
        /// IO Pins definition
        /// </summary>
        [Flags]
        public enum IOPins : byte
        {
            Pin0 = 0x01,
            Pin1 = 0x02,
            Pin2 = 0x04,
            Pin3 = 0x08,
            Pin4 = 0x10,
            Pin5 = 0x20,
            Pin6 = 0x40,
            Pin7 = 0x80,
            /// <summary>
            /// TSC pins for touch mode
            /// </summary>
            TouchPins = Pin4 | Pin5 | Pin6 | Pin7,
            All = Pin0 | Pin1 | Pin2 | Pin3 | Pin4 | Pin5 | Pin6 | Pin7,
        }

        public enum IOMode : byte
        {
            /// <summary>
            /// Input floating
            /// </summary>
            Input = 0,
            /// <summary>
            /// Output Push Pull 
            /// </summary>
            Output,
            /// <summary>
            /// Float input - irq detect on rising edge
            /// </summary>
            InterruptRisingEdge,
            /// <summary>
            /// Float input - irq detect on falling edge
            /// </summary>
            InterruptFallingEdge,
            /// <summary>
            /// Float input - irq detect on low level
            /// </summary>
            InterruptLowLevel,
            /// <summary>
            /// Float input - irq detect on high level
            /// </summary>
            InterruptHighLevel,
        }

        public enum IODirection : byte
        {
            /// <summary>
            /// In
            /// </summary>
            In = 0,
            /// <summary>
            /// Out
            /// </summary>
            Out = 1
        }

        public enum IOPinEdge : byte
        {
            Falling = 1,
            Rising = 2
        }

        #endregion

        public class Reading
        {
            private uint _x;
            private uint _y;
            private uint _z;
            private bool _valid;

            public uint X { get => _x; }
            public uint Y { get => _y; }
            public uint Z { get => _z;  }
            public bool IsValid { get => _valid;  }

            public Reading()
            {
                _valid = false;
            }

            public Reading(uint x, uint y, uint z)
            {
                _x = x;
                _y = y;
                _z = z;

                // reset valid flag;
                _valid = true;
            }
        }
    }
}
