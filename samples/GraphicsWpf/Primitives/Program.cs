using System;
using System.Threading;
using nanoFramework.UI;
using Primitives.SimplePrimitives;

namespace Primitives
{
    public class Program
    {
        public static void Main()
        {
            DisplayControl dc = new DisplayControl();
            int height = dc.ShorterSide;
            int width = dc.LongerSide;
            int bpp = dc.BitsPerPixel;
            int delayBetween = 3000;

            Bitmap fullScreenBitmap = new Bitmap(width, height);                            // Use this bitmap as our output to the screen.
            DisplayOrientation orientation = (DisplayOrientation)dc.Orientation;
            Font DisplayFont = Resource.GetFont(Resource.FontResources.SegoeUIRegular12);

            while (true)
            {
                StretchImage si = new StretchImage(fullScreenBitmap, width, height, DisplayFont);
                Thread.Sleep(delayBetween);

                RandomDrawLine rdlt = new RandomDrawLine(fullScreenBitmap, width, height, DisplayFont);
                Thread.Sleep(delayBetween);

                TileImage ti = new TileImage(fullScreenBitmap, width, height, DisplayFont);
                Thread.Sleep(delayBetween);

                RandomEllipses re = new RandomEllipses(fullScreenBitmap, width, height, DisplayFont);
                Thread.Sleep(delayBetween);

                DisplayMetrics dm = new DisplayMetrics(fullScreenBitmap, width, height, bpp);
                Thread.Sleep(delayBetween);

                FontExamples fe = new FontExamples(fullScreenBitmap, width, height);
                Thread.Sleep(delayBetween);

                RandomRectangles rr = new RandomRectangles(fullScreenBitmap, width, height, DisplayFont);
                Thread.Sleep(delayBetween);

                SliceScaling9 ss = new SliceScaling9(fullScreenBitmap, width, height, DisplayFont);
                Thread.Sleep(delayBetween);

                RotateImage ri = new RotateImage(fullScreenBitmap, width, height, DisplayFont);
                Thread.Sleep(delayBetween);

                //MatrixRain mr = new MatrixRain(fullScreenBitmap, width, height, DisplayFont);
                //Thread.Sleep(delayBetween);

                Thread.Sleep(delayBetween);

            }
        }
    }
}
