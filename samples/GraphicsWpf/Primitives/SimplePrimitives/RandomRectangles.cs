using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;

namespace Primitives.SimplePrimitives
{
    public class RandomRectangles
    {
        public RandomRectangles(Bitmap fullScreenBitmap,  Font DisplayFont)
        {
            Random random = new Random();
            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();
            for (int i = 0; i < 100; i++)
            {
                Color fillColor = (nanoFramework.Presentation.Media.Color)random.Next(0xFFFFFF);
                fullScreenBitmap.DrawRectangle((nanoFramework.Presentation.Media.Color)random.Next(0xFFFFFF), random.Next(1),
                    random.Next(fullScreenBitmap.Width), random.Next(fullScreenBitmap.Height - 20), random.Next(fullScreenBitmap.Width), random.Next(fullScreenBitmap.Height-20), 0, 0, fillColor, 0, 0, fillColor, 0, 0, (ushort)random.Next(256));
                InformationBar.DrawInformationBar(fullScreenBitmap, DisplayFont, InfoBarPosition.bottom, $"Rectangle Number {i}");
                fullScreenBitmap.Flush();
            }
        }
    }
}

