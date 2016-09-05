using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopBackground
{
    public class BackgroundStarfield
    {
        private MainForm Master;

        public List<Vector> Starfield = new List<Vector>();
        private Random rand = new Random();
        private Point CenterScreen = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2);
        int createDelay;

        public BackgroundStarfield(MainForm master)
        {
            Master = master;
        }

        public void Update()
        {
            for (int num = 0; num < Starfield.Count; num++)
            {
                Starfield[num] = new Vector(Starfield[num].PointB, Starfield[num].D * 1.04, Starfield[num].A);
                if ((Starfield[num].PointA.X > Screen.PrimaryScreen.WorkingArea.Right) || (Starfield[num].PointA.Y > Screen.PrimaryScreen.WorkingArea.Bottom) || (Starfield[num].PointA.X < 0) || (Starfield[num].PointA.Y < 0))
                {
                    Starfield.RemoveAt(num);
                }
            }
            if ((Starfield.Count < 200) && (createDelay <= 0))
            {
                Starfield.Add(new Vector(new PointD(CenterScreen.X, CenterScreen.Y), ((double)rand.Next(10, 20) / 100), rand.Next(0, 360)));
                createDelay = 1;
            }
            if (createDelay > 0) createDelay--;
            Master.Invalidate();
        }

        public void Draw(Graphics g)
        {
            foreach (Vector Star in Starfield)
            {
                if (Star.D < 1)
                {
                    g.FillRectangle(Brushes.White, (int)Star.PointA.X, (int)Star.PointA.Y, 1, 1);
                }
                else
                {
                    Vector Temp = new Vector(Star.PointA, Star.D * 2, Star.A);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.DrawLine(Pens.White, Temp.PointA.AsPoint, Temp.PointB.AsPoint);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                }
            }
            g.DrawString(Starfield.Count.ToString(), SystemFonts.DefaultFont, Brushes.White, new Point(10, 10));
        }
    }
}
