using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;

namespace Primitives.SimplePrimitives
{
    class RotateImage
    {

        public RotateImage(Bitmap fullScreenBitmap, int width, int height, Font DisplayFont)
        {
            Bitmap bmpSrc = null;
            Bitmap bmpDst = null;
            try
            {
                bmpDst = new Bitmap(width, height);
                bmpSrc = new Bitmap(WaterFallJpg.WaterFall, Bitmap.BitmapImageType.Jpeg);
                bmpDst.StretchImage(0, 0, bmpSrc, bmpSrc.Width, bmpSrc.Height, 0x0100);
                bmpDst.Flush();

                System.Threading.Thread.Sleep(200);

                for (int i = 1; i < 4; ++i)
                {
                    bmpDst.Clear();
                    bmpDst.RotateImage(90 * i, 0, 0, bmpSrc, 0, 0, bmpSrc.Width, bmpSrc.Height, 0xFFFF);
                    bmpDst.Flush();
                    System.Threading.Thread.Sleep(400);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Caught exception: " + e.Message);
            }
            finally
            {
                if (bmpSrc != null)
                {
                    bmpSrc.Dispose();
                }
                if (bmpDst != null)
                {
                    bmpDst.Dispose();
                }
                System.Threading.Thread.Sleep(500);
            }
        }

    }
}
