namespace NeoPixel
{
	public class Program
	{
        private const byte GpioPin = 5;
        private const ushort Size = 300;

		public static void Main()
		{
			var chain = new NeopixelChain(GpioPin, Size);
			
			while (true)
			{
                chain.MovePixel();
			}
		}
	}
}
