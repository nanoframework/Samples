using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;

namespace Primitives.SimplePrimitives
{
    public class RandomRectangles
    {
        public RandomRectangles(Bitmap fullScreenBitmap, int width, int height, Font DisplayFont)
        {
            Random random = new Random();
            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();
            for (int i = 0; i < 100; i++)
            {
                nanoFramework.Presentation.Media.Color fillColor = (nanoFramework.Presentation.Media.Color)random.Next(0xFFFFFF);
                fullScreenBitmap.DrawRectangle((nanoFramework.Presentation.Media.Color)random.Next(0xFFFFFF), random.Next(1),
                    random.Next(fullScreenBitmap.Width), random.Next(fullScreenBitmap.Height - 20), random.Next(fullScreenBitmap.Width), random.Next(fullScreenBitmap.Height), 0, 0, fillColor, 0, 0, fillColor, 0, 0, (ushort)random.Next(256));
                fullScreenBitmap.DrawText($"Rectangle Number {i}", DisplayFont, Color.Black, 0, fullScreenBitmap.Height - 20);
                fullScreenBitmap.Flush();
            }

        }

    }
}
