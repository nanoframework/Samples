using nanoFramework.Presentation.Media;
using nanoFramework.UI;

namespace Primitives.SimplePrimitives
{
    public class DisplayMetrics
    {
        public DisplayMetrics(Bitmap fullScreenBitmap, int width, int height, int bpp)
        {
            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();
            Font fontHeading = Resource.GetFont(Resource.FontResources.SegoeUIRegular12);
            Font fontDetail = Resource.GetFont(Resource.FontResources.CourierRegular10);
            int yCoord = 0;
            fullScreenBitmap.DrawText("LCD/TFT screen metrics", fontHeading, Color.AliceBlue, 0, yCoord);
            yCoord += fontDetail.Height + 2;
            fullScreenBitmap.DrawText($"Width {width}.", fontDetail, Color.Beige, 0, 25);
            yCoord += fontDetail.Height + 2;
            fullScreenBitmap.DrawText($"Height {height}.", fontDetail, Color.Chocolate, 0, 50);
            yCoord += fontDetail.Height + 2;
            fullScreenBitmap.DrawText($"Bits per pixel  {bpp}.", fontDetail, Color.NavajoWhite, 0, 40);
            fullScreenBitmap.Flush();
        }


    }
}
