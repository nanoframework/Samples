using nanoFramework.Presentation.Media;
using nanoFramework.Presentation.Shapes;
using nanoFramework.UI;
using System;
using System.Diagnostics;

namespace Primitives.SimplePrimitives
{
    class RotateImage
    {
        public RotateImage(Bitmap fullScreenBitmap, Font DisplayFont)
        {
            Bitmap waterFall = new Bitmap(WaterFallJpg.WaterFall, Bitmap.BitmapImageType.Jpeg);
            try
            {
                int shorterSide = Math.Min(fullScreenBitmap.Width, fullScreenBitmap.Height);
                // Calculate an image size for the waterFall bitmap that will fit inside the bounds of the destination bitmap
                // The diaganol of the image is the longest size
                Single diagonalLength = Math.Sqrt(waterFall.Width * waterFall.Width + waterFall.Height * waterFall.Height);
                Single scaleFactor = shorterSide / diagonalLength;

                // Stretch the watefall image into the large size
                int scaledImageWidth = (int)(waterFall.Width * scaleFactor);
                int scaledImageHeight = (int)(waterFall.Height * scaleFactor);
                Bitmap scaledImage = new Bitmap(scaledImageWidth, scaledImageHeight);
                scaledImage.StretchImage(0, 0, waterFall, scaledImageWidth, scaledImageHeight, 0xFFFF);

                // Create a rectangle in the middle where the image will be displayed
                // The same size as the stretched waterfall
                Rect outputRectangle = new Rect { x = (fullScreenBitmap.Width - scaledImageWidth) / 2, y = (fullScreenBitmap.Height - scaledImageHeight) / 2, width = scaledImageWidth, height = scaledImageHeight };

                int scaledImageXDst = 0;
                int scaledImageYDst = 0;

                int xDstImageToRotate = (fullScreenBitmap.Width - scaledImageXDst) / 2;
                int yDstImageToRotate = (fullScreenBitmap.Height - scaledImageYDst) / 2;

                int numberOfRotations = 100;
                int degreesIncrement = 1;
                int dynamicIncrease = 0;
                for (int iCounter = 0; iCounter < numberOfRotations; iCounter++)
                {
                    fullScreenBitmap.Clear();
                    // Rotate and stretch
                    fullScreenBitmap.RotateImage(degreesIncrement, outputRectangle.x, outputRectangle.y, scaledImage, scaledImageXDst, scaledImageYDst, outputRectangle.width, outputRectangle.height, 0xFFFF);

                    InformationBar.DrawInformationBar(fullScreenBitmap, DisplayFont, InfoBarPosition.bottom, $"Rotate Image degrees {degreesIncrement:D3}");
                    fullScreenBitmap.Flush();

                    if (iCounter < 50)
                    {
                        degreesIncrement += dynamicIncrease;
                    }
                    else if (iCounter == 50)
                    {
                        degreesIncrement = 0;
                        dynamicIncrease = 1;
                    }
                    else
                    {
                        degreesIncrement -= dynamicIncrease;
                    }
                    dynamicIncrease += 1;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Caught exception: " + e.Message);
            }
            finally
            {
                if (waterFall != null)
                {
                    waterFall.Dispose();
                }


                System.Threading.Thread.Sleep(500);
            }
        }
        private class Rect
        {
            public int x;
            public int y;
            public int width;
            public int height;
        }
    }
}
