using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DesktopBackground
{
    public class BackgroundWallpaper
    {
        private MainForm Master;
        private int UpdateInt;
        private string WallpaperPath;
        private Bitmap Wallpaper;
        public bool DarkSoulsGif;

        public BackgroundWallpaper(MainForm master)
        {
            Master = master;
            string wp = Master.GetPathOfWallpaper();
            if (wp != WallpaperPath)
            {
                WallpaperPath = wp;
                Wallpaper = (Bitmap)Image.FromFile(wp);
                Master.Invalidate();
            }
        }

        public void Update()
        {
            if (UpdateInt <= 0)
            {
                if (DarkSoulsGif)
                {
                    Master.DarkSoulsFire.Visible = true;
                    Master.DarkSoulsFire.Location = new Point(0, 0);
                    Master.DarkSoulsFire.Size = Master.ClientSize;
                }
                else
                {
                    string wp = Master.GetPathOfWallpaper();
                    if (wp != WallpaperPath)
                    {
                        WallpaperPath = wp;
                        Wallpaper = (Bitmap)Image.FromFile(wp);
                        Master.Invalidate();
                    }
                    UpdateInt = 1000;
                }
            }
            if (UpdateInt > 0) UpdateInt--;
        }

        public void Draw(Graphics g)
        {
            if (!DarkSoulsGif) g.DrawImage(Wallpaper, new Point(0, 0));
        }
    }
}
