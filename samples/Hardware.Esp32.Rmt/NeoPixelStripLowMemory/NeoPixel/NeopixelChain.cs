using nanoFramework.Hardware.Esp32.Rmt;

namespace NeoPixel
{
	public class NeopixelChain
	{
		// 80MHz / 4 => min pulse 0.00us
        protected const byte ClockDivider = 2;
        protected const float MinPulse = 1000000.0f / (80000000 / ClockDivider);
        readonly byte[] _color = { 0, 255, 0 };  // GRB value
        ushort _ledIndex = 0;
        readonly ushort _nrOfLeds;
        readonly byte[] _ledData;
        private readonly byte _gpioPin;
        private readonly TransmitterChannel _transmitterChannel;
        private readonly byte[] _onePulse;
        private readonly byte[] _zeroPulse;
        private readonly byte[] _resPulse;

        public NeopixelChain(byte gpioPin, ushort size)
        {
            _gpioPin = gpioPin;
            _transmitterChannel = new TransmitterChannel(_gpioPin);
            ConfigureTransmitter(_transmitterChannel);

            _nrOfLeds = size;
            var nrOfAllBits = 24 * _nrOfLeds;
            _ledData = new byte[(nrOfAllBits + 1) * 4];
            _onePulse = new byte[] { (byte)(0.7 / MinPulse), 128, (byte)(0.6 / MinPulse), 0 };
            _zeroPulse = new byte[] { (byte)(0.35 / MinPulse), 128, (byte)(0.8 / MinPulse), 0 };
            _resPulse = getResPulse(MinPulse);
        }

        
        public void MovePixel()
        {
            ushort led;
            int i = 0;
            for (led = 0; led < _nrOfLeds; led++)
            {
                byte col;
                for (col = 0; col < 3; col++)
                {
                    byte bit;
                    for (bit = 0; bit < 8; bit++)
                    {
                        if ((_color[col] & (1 << bit)) != 0 && (led == _ledIndex))
                        {
                            _ledData[0 + i] = _onePulse[0];
                            _ledData[1 + i] = _onePulse[1];
                            _ledData[2 + i] = _onePulse[2];
                            _ledData[3 + i] = _onePulse[3];
                        }
                        else
                        {
                            _ledData[0 + i] = _zeroPulse[0];
                            _ledData[1 + i] = _zeroPulse[1];
                            _ledData[2 + i] = _zeroPulse[2];
                            _ledData[3 + i] = _zeroPulse[3];
                        }
                        i = i + 4;
                    }
                }
            }

            //RES
            _ledData[0 + i] = _resPulse[0];
            _ledData[1 + i] = _resPulse[1];
            _ledData[2 + i] = _resPulse[2];
            _ledData[3 + i] = _resPulse[3];
            
            if ((++_ledIndex) >= _nrOfLeds)
            {
                _ledIndex = 0;
            }

            _transmitterChannel.SendData(_ledData, false);
        }
        
		protected void ConfigureTransmitter(TransmitterChannel commandList)
		{
			commandList.CarrierEnabled = false;
			commandList.ClockDivider = ClockDivider;
			commandList.SourceClock = SourceClock.APB;
			commandList.IdleLevel = false;
			commandList.IsChannelIdle = true;
		}

        private byte[] getResPulse(float min_pulse)
        {
            var result = new byte[4];
            var duration0 = (ushort)(25 / min_pulse);
            var duration1 = (ushort)(26 / min_pulse);

            var remaining = duration0 % 256;
            result[0] = (byte)remaining;
            result[1] = (byte)((duration0 - remaining) / 256);

            remaining = duration1 % 256;
            result[2] = (byte)remaining;
            result[3] = (byte)((duration1 - remaining) / 256);
            return result;
        }

    }
}
