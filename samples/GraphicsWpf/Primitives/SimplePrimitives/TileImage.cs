using nanoFramework.Presentation.Media;
using nanoFramework.UI;

namespace Primitives.SimplePrimitives
{
    public class TileImage
    {
        public TileImage(Bitmap fullScreenBitmap, int width, int height, Font DisplayFont)
        {
            //Bitmap btn = new Bitmap(WaterFallJpg.WaterFall, Bitmap.BitmapImageType.Jpeg);
            Bitmap btn = new Bitmap(PandaGif.Panda, Bitmap.BitmapImageType.Gif);

            fullScreenBitmap.Clear();
            fullScreenBitmap.Flush();

            fullScreenBitmap.DrawRectangle(Color.White, 0, 0, 0, width, height, 0, 0, Color.White, 0, 0, 0, 0, 0, 256);
            Font fntComicSansMS16 = Resource.GetFont(Resource.FontResources.ComicSansMS16);
            fullScreenBitmap.DrawText("Tile Image Example", fntComicSansMS16, Color.Black, 10, 0);
            fullScreenBitmap.TileImage(20, 20, btn, width - 50, height - 50, 256);
            fullScreenBitmap.Flush();
            btn.Dispose();
        }
    }
}
