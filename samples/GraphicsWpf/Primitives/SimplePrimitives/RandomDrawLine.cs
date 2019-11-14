using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;

namespace Primitives.SimplePrimitives
{
    public class RandomDrawLine
    {
        public RandomDrawLine(Bitmap fullScreenBitmap, int width, int height, Font DisplayFont)
        {
            Random random = new Random();
            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();
            fullScreenBitmap.DrawText("Random Line Drawing", DisplayFont, Color.AliceBlue, 0, 0);

            for (int i = 100; i > 0; i--)
            {
                fullScreenBitmap.DrawLine((Color)random.Next(0xFFFFFF),
                                           1,
                                           random.Next(width),
                                           random.Next(height - 22),
                                           random.Next(width),
                                           random.Next(height));
                fullScreenBitmap.DrawRectangle(Color.White, 0, 0, fullScreenBitmap.Height - 20, 320, 22, 0, 0, Color.White,
                    0, fullScreenBitmap.Height - 20, Color.White, 0, fullScreenBitmap.Height, Bitmap.OpacityOpaque);
                fullScreenBitmap.DrawText($"Line Number {i}", DisplayFont, Color.Black, 0, fullScreenBitmap.Height - 20);
                fullScreenBitmap.Flush();
            }
        }
    }
}
