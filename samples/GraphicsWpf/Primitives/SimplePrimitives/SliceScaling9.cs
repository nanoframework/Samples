using nanoFramework.Presentation.Media;
using nanoFramework.UI;

namespace Primitives.SimplePrimitives
{
    public class SliceScaling9
    {
        public SliceScaling9(Bitmap fullScreenBitmap,  Font DisplayFont)
        {
            Font fontHeading = Resource.GetFont(Resource.FontResources.ComicSansMS16);

            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();

            fullScreenBitmap.DrawText($"Scale image", fontHeading, Color.Beige, 0, 0);
            fullScreenBitmap.DrawRectangle(Color.White, 0, 0, 0, fullScreenBitmap.Width, fullScreenBitmap.Height, 0, 0, Color.White, 0, 0, 0, 0, 0, 256);

            Bitmap bmpScaleWaterFall = new Bitmap(WaterFallJpg.WaterFall, Bitmap.BitmapImageType.Jpeg);
            bmpScaleWaterFall.MakeTransparent(ColorUtility.ColorFromRGB(255, 0, 255));
            fullScreenBitmap.Scale9Image(30, 30, fullScreenBitmap.Width / 3, fullScreenBitmap.Height / 3, bmpScaleWaterFall, 6, 6, 6, 6, 256);
            fullScreenBitmap.Scale9Image(fullScreenBitmap.Width / 2, fullScreenBitmap.Height / 2, fullScreenBitmap.Width / 3, 30, bmpScaleWaterFall, 6, 6, 6, 6, 256);
            fullScreenBitmap.Scale9Image(30, fullScreenBitmap.Height / 2, 30, fullScreenBitmap.Height / 3, bmpScaleWaterFall, 6, 6, 6, 6, 256);
            fullScreenBitmap.Scale9Image(fullScreenBitmap.Width / 2, 30, 30, 30, bmpScaleWaterFall, 6, 6, 6, 6, 256);
            bmpScaleWaterFall.Dispose();

            InformationBar.DrawInformationBar(fullScreenBitmap, DisplayFont, InfoBarPosition.bottom, "9 Slice Scaling ( Scale 9 grid) ");
            fullScreenBitmap.Flush();
        }
        public void Scale9ImageManaged(Bitmap bmpDest, int xDst, int yDst, int widthDst, int heightDst, Bitmap bitmap, int leftBorder, int topBorder, int rightBorder, int bottomBorder, ushort opacity)
        {
            if (widthDst < leftBorder || heightDst < topBorder)
                return;
            int widthSrc = bitmap.Width;
            int heightSrc = bitmap.Height;
            int centerWidthSrc = widthSrc - (leftBorder + rightBorder);
            int centerHeightSrc = heightSrc - (topBorder + bottomBorder);
            int centerWidthDst = widthDst - (leftBorder + rightBorder);
            int centerHeightDst = heightDst - (topBorder + bottomBorder);

            //top-left
            if (widthDst >= leftBorder && heightDst >= topBorder)
                bmpDest.StretchImage(xDst, yDst, leftBorder, topBorder, bitmap, 0, 0, leftBorder, topBorder, opacity);

            //top-right
            if (widthDst > leftBorder /*&& heightDst >= topBorder*/)
                bmpDest.StretchImage(xDst + widthDst - rightBorder, yDst, rightBorder, topBorder, bitmap, widthSrc - rightBorder, 0, rightBorder, topBorder, opacity);
            //bottom-left
            if (/*widthDst >= leftBorder && */heightDst > topBorder)
                bmpDest.StretchImage(xDst, yDst + heightDst - bottomBorder, leftBorder, bottomBorder, bitmap, 0, heightSrc - bottomBorder, leftBorder, bottomBorder, opacity);
            //bottom-right
            if (widthDst > leftBorder && heightDst > topBorder)
                bmpDest.StretchImage(xDst + widthDst - rightBorder, yDst + heightDst - bottomBorder, rightBorder, bottomBorder, bitmap, widthSrc - rightBorder, heightSrc - bottomBorder, rightBorder, bottomBorder, opacity);

            //left
            if (/*widthDst >= leftBorder &&*/ centerHeightDst > 0)
                bmpDest.StretchImage(xDst, yDst + topBorder, leftBorder, centerHeightDst, bitmap, 0, topBorder, leftBorder, centerHeightSrc, opacity);

            //top
            if (centerWidthDst > 0 /*&& heightDst >= topBorder*/)
                bmpDest.StretchImage(xDst + leftBorder, yDst, centerWidthDst, topBorder, bitmap, leftBorder, 0, centerWidthSrc, topBorder, opacity);

            //right
            if (widthDst > leftBorder && centerHeightDst > 0)
                bmpDest.StretchImage(xDst + widthDst - rightBorder, yDst + topBorder, rightBorder, centerHeightDst, bitmap, widthSrc - rightBorder, topBorder, rightBorder, centerHeightSrc, opacity);

            //bottom
            if (centerWidthDst > 0 && heightDst > topBorder)
                bmpDest.StretchImage(xDst + leftBorder, yDst + heightDst - bottomBorder, centerWidthDst, bottomBorder, bitmap, leftBorder, heightSrc - bottomBorder, centerWidthSrc, bottomBorder, opacity);

            //center
            if (centerWidthDst > 0 && centerHeightDst > 0)
                bmpDest.StretchImage(xDst + leftBorder, yDst + topBorder, centerWidthDst, centerHeightDst, bitmap, leftBorder, topBorder, centerWidthSrc, centerHeightSrc, opacity);
        }
    }
}
