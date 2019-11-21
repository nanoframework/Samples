using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;

namespace Primitives.SimplePrimitives
{
    public class RandomClipping
    {
        internal struct Rect
        {
            public int x;
            public int y;
            public int width;
            public int height;
        }
        public RandomClipping(Bitmap fullScreenBitmap,  Font DisplayFont)
        {
            Random random = new Random();
            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();

            Rect r0 = new Rect { x = 0, y = 0, width = fullScreenBitmap.Width, height = fullScreenBitmap.Height-20 };
            Rect r1 = new Rect { x = 20, y = 20, width = 100, height = 100 };
            Rect r2 = new Rect { x = 180, y = 80, width = 100, height = 100 };
            Rect r3 = new Rect { x = 20, y = 170, width = 100, height = 30 };

            string additionalInfo = "No Clipping";
            for (int i = 0; i < 400; i++)
            {
                if (i == 100)
                {
                    additionalInfo = "Clipping [20,20,120,120]";
                    DullExistingLines(fullScreenBitmap, r0);
                    fullScreenBitmap.SetClippingRectangle(r1.x, r1.y, r1.width, r1.height);
                    DrawClippingRectangular(fullScreenBitmap,r1);
                    fullScreenBitmap.Flush();
                }
                if (i == 200)
                {
                    additionalInfo = "Clipping [180,80,280,180]";
                    DullExistingLines(fullScreenBitmap, r0);
                    fullScreenBitmap.SetClippingRectangle(r2.x, r2.y, r2.width, r2.height);
                    DrawClippingRectangular(fullScreenBitmap, r2);
                    fullScreenBitmap.Flush();
                }
                if (i == 300)
                {
                    additionalInfo = "Clipping [20,170,120,200]";
                    DullExistingLines(fullScreenBitmap, r0);
                    fullScreenBitmap.SetClippingRectangle(r3.x, r3.y, r3.width, r3.height);
                    DrawClippingRectangular(fullScreenBitmap, r3);
                    fullScreenBitmap.Flush();
                }
                int radiusX = random.Next(100);
                int radiusY = random.Next(100);
                fullScreenBitmap.DrawEllipse((nanoFramework.Presentation.Media.Color)random.Next(0xFFFFFF), 0,
                                random.Next(fullScreenBitmap.Width), random.Next(fullScreenBitmap.Height - 20), radiusX, radiusY, 0, 0, 0, 0, 0, 0, (ushort)random.Next(256));
                DrawInformationBar(fullScreenBitmap, DisplayFont, additionalInfo);
                fullScreenBitmap.Flush();
            }
            fullScreenBitmap.SetClippingRectangle(0, 0, fullScreenBitmap.Width, fullScreenBitmap.Height);
        }

        private void DrawClippingRectangular(Bitmap fullScreenBitmap,Rect r1)
        {
            fullScreenBitmap.DrawRectangle(colorOutline: Color.Cyan, thicknessOutline: 2,
                                                x: r1.x-1, y: r1.y-1,
                                                width: r1.width+2, height: r1.height+2,
                                                xCornerRadius: 0, yCornerRadius: 0,
                                                colorGradientStart: 0,
                                                xGradientStart: 0, yGradientStart: 0,
                                                colorGradientEnd: 0,
                                                xGradientEnd: 0, yGradientEnd: 0,
                                                opacity: 100);
        }

        private void DullExistingLines(Bitmap fullScreenBitmap, Rect rectangle)
        {
            fullScreenBitmap.DrawRectangle(colorOutline: Color.Black, thicknessOutline: 0,
                                                x: rectangle.x, y: rectangle.y,
                                                width: rectangle.width, height: rectangle.height,
                                                xCornerRadius: 0, yCornerRadius: 0,
                                                colorGradientStart: Color.Black,
                                                xGradientStart: 0, yGradientStart: 0,
                                                colorGradientEnd: Color.Black,
                                                xGradientEnd: fullScreenBitmap.Width, yGradientEnd: fullScreenBitmap.Height,
                                                opacity: 200);
        }
        private void DrawInformationBar(Bitmap fullScreenBitmap, Font DisplayFont, string value)
        {
            fullScreenBitmap.DrawRectangle(Color.White, 0, 0, fullScreenBitmap.Height - 20, 320, 22, 0, 0, Color.White,
                0, fullScreenBitmap.Height - 20, Color.White, 0, fullScreenBitmap.Height, Bitmap.OpacityOpaque);
            fullScreenBitmap.DrawText("Clipping " + value.ToString(), DisplayFont, Color.Black, 0, fullScreenBitmap.Height - 20);
        }
    }
}
