using System.Threading;
using nanoFramework.UI;
using Primitives.SimplePrimitives;


namespace Primitives
{
    public class Program
    {
        public static void Main()
        {
            int delayBetween = 1100;


            // Get full screen bitmap from displayControl to draw on.
            Bitmap fullScreenBitmap = DisplayControl.FullScreen;  

            fullScreenBitmap.Clear();

            Font DisplayFont = Resource.GetFont(Resource.FontResources.SegoeUIRegular12);

            while (true)
            {
                RandomDrawLine rdlt = new RandomDrawLine(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                RotateImage ri = new RotateImage(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                ColourGradient colourGradient = new ColourGradient(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                Colours ColourExample = new Colours(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                PagedText pt = new PagedText(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                BouncingBalls bb = new BouncingBalls(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                TileImage ti = new TileImage(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                StretchImage si = new StretchImage(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                SetPixels sp = new SetPixels(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                FontExamples fe = new FontExamples(fullScreenBitmap);
                Thread.Sleep(delayBetween);

                RandomRectangles rr = new RandomRectangles(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                SliceScaling9 ss = new SliceScaling9(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                RandomClipping rc = new RandomClipping(fullScreenBitmap, DisplayFont);
                Thread.Sleep(delayBetween);

                MatrixRain mr = new MatrixRain(fullScreenBitmap);
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
