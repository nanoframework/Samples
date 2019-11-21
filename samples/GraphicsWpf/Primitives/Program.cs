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
            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();
            DisplayOrientation orientation = (DisplayOrientation)dc.Orientation;
            Font DisplayFont = Resource.GetFont(Resource.FontResources.SegoeUIRegular12);

            while (true)
            {
                SetPixels sp = new SetPixels(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                FontExamples fe = new FontExamples(fullScreenBitmap);
                Thread.Sleep(delayBetween);

                StretchImage si = new StretchImage(fullScreenBitmap,  DisplayFont);
                Thread.Sleep(delayBetween);

                RandomDrawLine rdlt = new RandomDrawLine(fullScreenBitmap,  DisplayFont);
                Thread.Sleep(delayBetween);

                TileImage ti = new TileImage(fullScreenBitmap,  DisplayFont);
                Thread.Sleep(delayBetween);

                RandomEllipses re = new RandomEllipses(fullScreenBitmap,  DisplayFont);
                Thread.Sleep(delayBetween);

                RandomRectangles rr = new RandomRectangles(fullScreenBitmap,  DisplayFont);
                Thread.Sleep(delayBetween);

                SliceScaling9 ss = new SliceScaling9(fullScreenBitmap,  DisplayFont);
                Thread.Sleep(delayBetween);
                
                RotateImage ri = new RotateImage(fullScreenBitmap,  DisplayFont);
                Thread.Sleep(delayBetween);

                RandomClipping rc = new RandomClipping(fullScreenBitmap,  DisplayFont);
                Thread.Sleep(delayBetween);

                MatrixRain mr = new MatrixRain(fullScreenBitmap,  DisplayFont);
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
