using nanoFramework.Presentation.Media;
using nanoFramework.UI;

namespace Primitives.SimplePrimitives
{
    public class StretchImage
    {
        public StretchImage(Bitmap fullScreenBitmap, int width, int height, Font DisplayFont)
        {
            Bitmap bmWaterfall = new Bitmap(WaterFallJpg.WaterFall, Bitmap.BitmapImageType.Jpeg);

            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();

            fullScreenBitmap.DrawImage(0, 0, bmWaterfall, 0, 0, bmWaterfall.Width, bmWaterfall.Height);
            fullScreenBitmap.Flush();

            for(int i=0;i < 5;i++)
            {
                fullScreenBitmap.StretchImage(0, 0, bmWaterfall, bmWaterfall.Width + (i + 1) * 15, bmWaterfall.Height + (i + 1) * 15, 255);
                fullScreenBitmap.Flush();
                System.Threading.Thread.Sleep(500);
            }






            //fullScreenBitmap.StretchImage(20, 20, 30, 150, bmWaterfall, bmWaterfall.Width / 2, 0, bmWaterfall.Width / 2, bmWaterfall.Height / 2, 256);
            //fullScreenBitmap.Flush();

            //fullScreenBitmap.StretchImage(40, 40, 60, 250, bmWaterfall, bmWaterfall.Width / 2, 0, bmWaterfall.Width / 2, bmWaterfall.Height / 2, 256);
            //fullScreenBitmap.Flush();


            bmWaterfall.Dispose();
        }

    }
}
