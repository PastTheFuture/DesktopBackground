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
        private string WallpaperPath;
        private Bitmap Wallpaper;
        public Image Gif;
        public bool GifState;
        public string currentGif;

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

        public void Reset()
        {
            Update();
            if(GifState)
            {
            }
            else
            {
                string wp = Master.GetPathOfWallpaper();
                if(wp != WallpaperPath)
                {
                    WallpaperPath = wp;
                    Wallpaper = (Bitmap)Image.FromFile(wp);
                    Master.Invalidate();
                }
            }
        }

        public void Update()
        {
            if (GifState) ImageAnimator.UpdateFrames();
        }

        public void ChangeGif(Image newGif)
        {
            if (Gif != null) ImageAnimator.StopAnimate(Gif, OnFrameChanged);
            Gif = newGif;
            ImageAnimator.Animate(Gif, OnFrameChanged);
        }

        public void OnFrameChanged(object o, EventArgs e)
        {
            Master.Invalidate();
        }


        public void Draw(Graphics g)
        {
            if(GifState)
            {
                if(currentGif != "Earth")
                    g.DrawImage(Gif, Master.ScreenSelection.Bounds);
                else
                    g.DrawImage(Gif, new Rectangle(Master.ClientSize.Width / 2 - 360, Master.ClientSize.Height / 2 - 360, 720, 720));
            }
            if (!GifState) g.DrawImage(Wallpaper, Master.ClientRectangle);
            else g.DrawRectangle(Pens.Black, Master.ClientRectangle);
        }
    }
}
