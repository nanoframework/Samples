namespace NeoPixel
{
	public class Program
	{
        private const int GpioPin = 5;
        private const int Size = 50;
        private static readonly Color RedColor = new Color { R = 255 };
        private static readonly Color BlackColor = new Color();

		public static void Main()
		{
			var chain = new NeopixelChain(GpioPin, Size);
			
			while (true)
			{
				for (uint i = 0; i < Size; i++)
				{
					chain[i] = RedColor;
					chain.Update();
					chain[i] = BlackColor;
				}
			}
		}
	}
}
