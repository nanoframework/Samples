using nanoFramework.Hardware.Esp32.Rmt;

namespace NeoPixel
{
	public class NeopixelChain
	{
		// 80MHz / 4 => min pulse 0.00us
		protected const byte ClockDivider = 4;
		// one pulse duration in us
		protected const float MinPulse = 1000000.0f / (80000000 / ClockDivider);

		// default datasheet values
		protected readonly RmtCommand OnePulse =
			new RmtCommand((ushort)(0.7 / MinPulse), true, (ushort)(0.6 / MinPulse), false);

		protected readonly RmtCommand ZeroPulse =
			new RmtCommand((ushort)(0.35 / MinPulse), true, (ushort)(0.8 / MinPulse), false);

		protected readonly RmtCommand ResCommand =
			new RmtCommand((ushort)(25 / MinPulse), false, (ushort)(26 / MinPulse), false);

		protected Color[] Pixels;
		private readonly int _gpioPin;

		public NeopixelChain(int gpioPin, uint size)
		{
			_gpioPin = gpioPin;

			Pixels = new Color[size];
			for (uint i = 0; i < size; ++i)
			{
				Pixels[i] = new Color();
			}
		}

		public void Update()
		{
			var transmitterChannelSettings = new TransmitChannelSettings(pinNumber: _gpioPin)
			{
				EnableCarrierWave = false,
				ClockDivider = ClockDivider,
				IdleLevel = false,
			};

			using (var commandList = new TransmitterChannel(transmitterChannelSettings))
			{
				for (uint pixel = 0; pixel < Pixels.Length; ++pixel)
				{
					SerializeColor(Pixels[pixel].G, commandList);
					SerializeColor(Pixels[pixel].R, commandList);
					SerializeColor(Pixels[pixel].B, commandList);
				}
				commandList.AddCommand(ResCommand); // RET
				commandList.Send(true);
			}
		}

		private void SerializeColor(byte b, TransmitterChannel commandList)
		{
			for (var i = 0; i < 8; ++i)
			{
				commandList.AddCommand(((b & (1u << 7)) != 0) ? OnePulse : ZeroPulse);
				b <<= 1;
			}
		}

		public Color this[uint i]
		{
			get => Pixels[i];
			set => Pixels[i] = value;
		}
	}
}
