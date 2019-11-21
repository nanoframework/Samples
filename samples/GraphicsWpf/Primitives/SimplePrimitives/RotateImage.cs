using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;

namespace Primitives.SimplePrimitives
{
    class RotateImage
    {
        public RotateImage(Bitmap fullScreenBitmap,  Font DisplayFont)
        {
            Bitmap bmpSrc = null;
            Bitmap bmpDst = null;
            try
            {
                bmpSrc = new Bitmap(WaterFallJpg.WaterFall, Bitmap.BitmapImageType.Jpeg);
                fullScreenBitmap.StretchImage(0, 0, bmpSrc, bmpSrc.Width, bmpSrc.Height, 0x0100);
                fullScreenBitmap.Flush();

                int xDst = 0;
                int yDst = 0;

                int rotation = 0;
                for (int i = 0; i < 66; ++i)
                {
                    switch (rotation)
                    {
                        case 0:
                            xDst = (fullScreenBitmap.Width - bmpSrc.Width) / 2;
                            yDst = (fullScreenBitmap.Height - bmpSrc.Height) / 2;
                            break;
                        case 90:
                            xDst = (fullScreenBitmap.Height - bmpSrc.Height) / 2;
                            yDst = (fullScreenBitmap.Width - bmpSrc.Width) / 2;
                            break;
                        case 180:
                            xDst = (fullScreenBitmap.Width - bmpSrc.Width) / 2;
                            yDst = (fullScreenBitmap.Height - bmpSrc.Height) / 2;
                            break;
                        case 270:
                            xDst = (fullScreenBitmap.Width - bmpSrc.Width) / 2;
                            yDst = (fullScreenBitmap.Height - bmpSrc.Height) / 2;
                            break;
                    }

                    fullScreenBitmap.Clear();
                    fullScreenBitmap.RotateImage(rotation, xDst, yDst, bmpSrc, 0, 0, bmpSrc.Width, bmpSrc.Height, 0xFFFF);

                    InformationBar.DrawInformationBar(fullScreenBitmap, DisplayFont, InfoBarPosition.bottom, $"Rotate Image degrees {i:D3}");
                    fullScreenBitmap.Flush();

                    rotation += 90;
                    if (rotation == 360)
                    {
                        rotation = 0;
                    }
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
