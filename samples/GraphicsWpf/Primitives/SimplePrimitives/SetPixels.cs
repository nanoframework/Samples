using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;


namespace Primitives.SimplePrimitives
{
    class SetPixels
    {
        public  SetPixels(Bitmap fullScreenBitmap, int width, int height, Font DisplayFont)
        {
            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();
            for (int i = 0; i < fullScreenBitmap.Height; ++i)
            {
                fullScreenBitmap.SetPixel(i, i, (Color)((255 - i) << 16));
            }
            for (int i = 0; i < fullScreenBitmap.Height; i += 2)
            {
                fullScreenBitmap.SetPixel(fullScreenBitmap.Height - i, i, (Color)(i << 8));
            }
            fullScreenBitmap.Flush();

        }

    }
}
