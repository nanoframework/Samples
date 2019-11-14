using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;

namespace Primitives.SimplePrimitives
{
    public class RandomEllipses
    {
        public RandomEllipses(Bitmap fullScreenBitmap, int width, int height, Font fontNinaB)
        {
            Random random = new Random();
            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();
            for (int i = 0; i < 100; i++)
            {
                int radius = random.Next(100);
                fullScreenBitmap.DrawEllipse((nanoFramework.Presentation.Media.Color)random.Next(0xFFFFFF), 1,
                                random.Next(fullScreenBitmap.Width), random.Next(fullScreenBitmap.Height - 20), radius, radius, 0, 0, 0, 0, 0, 0, 0);
                fullScreenBitmap.DrawText($"Circle Number {i}", fontNinaB, Color.Black, 0, fullScreenBitmap.Height - 20);
                fullScreenBitmap.Flush();
            }
        }
    }
}
