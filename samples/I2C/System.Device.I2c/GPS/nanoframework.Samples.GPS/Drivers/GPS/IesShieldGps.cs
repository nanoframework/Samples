using System;
using System.Text;
using System.Device.I2c;

namespace nanoframework.Drivers.GPS
{
    public class IesShieldGps
    {
        private readonly int _address;
        private readonly I2cDevice _gpsController;
        public const byte GPAM_DEFAULT_ADDDRESS = 0x68; // GPM I2C Register

        /// <summary>
        /// I2C address of the IES-SHIELD-GPS device.
        /// </summary>
        public int Address => _address;

        /// <summary>
        /// Creates a driver for the IES-SHIELD-GPS.
        /// </summary>
        /// <param name="i2cBus">The I2C bus where the device is connected to.</param>
        /// <param name="address">The I2C address of the device.</param>
        public IesShieldGps(int i2cBus, int address = GPAM_DEFAULT_ADDDRESS)
        {
            // store I2C address
            _address = address;

            // instantiate I2C controller
            _gpsController = I2cDevice.Create(new I2cConnectionSettings(i2cBus, address, I2cBusSpeed.FastMode));
        }

        /// <summary>
        /// Gets the current pitch of the device
        /// </summary>
        /// <returns>the pitch</returns>
        public int GetPitch()
        {
            int pitch = 0;
            byte data = (byte)GetSingleRegister(63); // Read Pitch registers from GPM
            if ((data & 0x80) != 0)
            {                                        // If negative :

                data &= 0x7F;
                pitch = (int)data;
                pitch *= -1;                        // Convert to minus value
            }
            else
            {
                pitch = (int)data;
            }

            return pitch;
        }

        /// <summary>
        /// Gets the current roll of the device
        /// </summary>
        /// <returns>the roll</returns>
        public int GetRoll()
        {
            int roll = 0;
            byte data = (byte)GetSingleRegister(64);    // Read Roll registers from GPM
            if ((data & 0x80) != 0)
            {                                           // If negative :
                data &= 0x7F;
                roll = (int)data;
                roll *= -1;                             // convert to minus value
            }
            else
            {
                roll = (int)data;
            }

            return roll;
        }

        /// <summary>
        /// Gets the current heading of the device
        /// </summary>
        /// <returns>the heading in degrees</returns>
        public float GetHeading()
        {
            var heading = (float)GetSingleRegister(44) * 100;
            heading += (float)GetSingleRegister(45) * 10;
            heading += (float)GetSingleRegister(46);
            heading += (float)GetSingleRegister(47) / 10;
            return heading;
        }

        /// <summary>
        /// Gets the current speed from the device
        /// </summary>
        /// <returns>the speed in metres per second</returns>
        public float GetSpeed()
        {
            var speed = (float)GetSingleRegister(52) * 100;
            speed += (float)GetSingleRegister(53) * 10;
            speed += (float)GetSingleRegister(54);
            speed += (float)GetSingleRegister(55) / 10;
            return speed;
        }

        /// <summary>
        /// Gets the current longitude from the device
        /// </summary>
        /// <returns>the longitude in degrees</returns>
        public float GetLongitude()
        {
            var longitude_degrees = GetSingleRegister(23) * 100;
            longitude_degrees += GetDoubleRegister(24);
            float longitude_minutes = GetDoubleRegister(26);
            float longitude_seconds = (float)GetSingleRegister(28) / 10;
            longitude_seconds += (float)GetSingleRegister(29) / 100;
            longitude_seconds += (float)GetSingleRegister(30) / 1000;
            longitude_seconds += (float)GetSingleRegister(31) / 10000;
            var longitude_direction = (char)GetSingleRegister(32);

            return ConvertDegreeAngleToFloat(longitude_degrees, longitude_minutes, longitude_seconds, longitude_direction);
        }

        /// <summary>
        /// Gets the current latitude from the device
        /// </summary>
        /// <returns>the latitude in degrees</returns>
        public float GetLatitude()
        {

            var latitude_degrees = GetDoubleRegister(14);
            float latitude_minutes = GetDoubleRegister(16);
            float latitude_seconds = (float)GetSingleRegister(18) / 10;
            latitude_seconds += (float)GetSingleRegister(19) / 100;
            latitude_seconds += (float)GetSingleRegister(20) / 1000;
            latitude_seconds += (float)GetSingleRegister(21) / 10000;
            var latitude_direction = (char)GetSingleRegister(22);

            return ConvertDegreeAngleToFloat(latitude_degrees, latitude_minutes, latitude_seconds, latitude_direction);
        }

        /// <summary>
        /// Gets the current Date and Time from the device
        /// </summary>
        /// <returns>Current DateTime</returns>
        public DateTime GetDateTime()
        {
            var y1 = GetDoubleRegister(10);                     //Read Year 1 registers from GPM and print to PC
            var y2 = GetDoubleRegister(12);                     //Read Year 2 registers from GPM and print to PC
            var mo = GetDoubleRegister(8);                     //Read Month registers from GPM and print to PC
            var d = GetDoubleRegister(6);                 //Read Day registers from GPM and print to PC

            var hh = GetDoubleRegister(0);                     //Read Hours registers from GPM and print to PC
            var mm = GetDoubleRegister(2);                     //Read Minutes registers from GPM and print to PC
            var ss = GetDoubleRegister(4);                     //Read Seconds registers from GPM and print to PC

            var yy = (y1 * 100) + y2;

            if (yy != 0 && mo != 0 && d != 0 && hh <= 24 && mm <= 59 && ss <= 59) //sanity check, otherwise the parse could fail
            {
                return new DateTime(yy, mo, d, hh, mm, ss);
            }
            else
            {
                return new DateTime();
            }
        }

        /// <summary>
        /// Converts DMS to DD
        /// </summary>
        /// <param name="degrees"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private float ConvertDegreeAngleToFloat(float degrees, float minutes, float seconds, char direction)
        {
            //Example: 17.21.18S

            var multiplier = ((direction == 'S') || (direction == 'W')) ? -1 : 1; //handle south and west

            return (degrees + (minutes / 60) + (seconds / 3600)) * multiplier;
        }

        /// <summary>
        /// Get double register value from GPAM
        /// </summary>
        /// <param name="register">The register to get</param>
        /// <returns></returns>
        private int GetDoubleRegister(byte register)
        {

            byte[] highByte = new byte[1];
            byte[] lowByte = new byte[1];

            _gpsController.Write(new byte[] { register });
            _gpsController.Read(highByte);

            _gpsController.Write(new byte[] { register += 1 });
            _gpsController.Read(lowByte);

            int value = (highByte[0] * 10) + lowByte[0];
            return (value);
        }

        /// <summary>
        /// Get single register value from GPAM
        /// </summary>
        /// <param name="register">The register to get</param>
        /// <returns></returns>
        private int GetSingleRegister(byte register)
        {

            byte[] buffer = new byte[1];
            _gpsController.Write(new byte[] { register });
            _gpsController.Read(buffer);
            return (buffer[0]);
        }
    }
}
