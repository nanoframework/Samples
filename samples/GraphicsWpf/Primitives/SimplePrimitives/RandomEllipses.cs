using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;

namespace Primitives.SimplePrimitives
{
    public class RandomEllipses
    {
        public RandomEllipses(Bitmap fullScreenBitmap, Font DisplayFont)
        {
            Random random = new Random();
            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();
            for (int i = 0; i < 100; i++)
            {
                int radiusX = random.Next(200);
                int radiusY = random.Next(200);

                fullScreenBitmap.DrawEllipse((nanoFramework.Presentation.Media.Color)random.Next(0xFFFFFF), 1,
                                random.Next(fullScreenBitmap.Width), random.Next(fullScreenBitmap.Height - 20), radiusX, radiusY, 0, 0, 0, 0, 0, 0, 0);
                InformationBar.DrawInformationBar(fullScreenBitmap, DisplayFont, InfoBarPosition.bottom, $"Ellipse Number {i}");
                fullScreenBitmap.Flush();
            }
        }
    }
}
