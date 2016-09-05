using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopBackground
{
    public class BackgroundPong
    {
        private MainForm Master;

        private Rectangle LeftPaddle;
        private Rectangle RightPaddle;
        private Random rand = new Random();
        private Size Ball;
        private Vector BallVector;
        private List<Rectangle> BallTrail = new List<Rectangle>();
        private Rectangle BallRect
        { get { return new Rectangle(BallVector.PointA.AsPoint, Ball); } }
        private int SpeedInterval;
        
        private Point CenterScreen = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2);

        public BackgroundPong(MainForm master)
        {
            Master = master;

            LeftPaddle = new Rectangle(Screen.PrimaryScreen.WorkingArea.Left + 30, Screen.PrimaryScreen.WorkingArea.Top + 30, 30, 100);
            RightPaddle = new Rectangle(Screen.PrimaryScreen.WorkingArea.Right - 60, Screen.PrimaryScreen.WorkingArea.Top + 30, 30, 100);
            Ball = new Size(30, 30);
            BallVector = new Vector(new PointD(CenterScreen.X - 15, CenterScreen.Y - 15), 5, 45);
        }

        public void Update()
        {
            #region Move Ball
            if (BallVector.D < 35)
            {
                if (SpeedInterval <= 0)
                {
                    BallVector.SetVector(BallVector.D + 1, BallVector.A);
                    SpeedInterval = rand.Next(150, 251);
                }
                SpeedInterval--;
            }
            BallVector = new Vector(BallVector.PointB, BallVector.D, BallVector.A);
            BallTrail.Add(new Rectangle(BallVector.PointA.AsPoint, Ball));

            if (LeftPaddle.IntersectsWith(BallRect))
            {
                double refAngle = (360 - BallVector.A + rand.Next(-5, 6));
                BallVector.SetVector(BallVector.D, refAngle);
            }
            if (RightPaddle.IntersectsWith(BallRect))
            {
                double refAngle = (360 - BallVector.A + rand.Next(-5, 6));
                BallVector.SetVector(BallVector.D, refAngle);
            }
            #endregion

            #region Move Paddles
            if (LeftPaddle.Y > BallVector.PointA.AsPoint.Y - 20) LeftPaddle.Y -= 10;
            if (LeftPaddle.Y < BallVector.PointA.AsPoint.Y - 40) LeftPaddle.Y += 10;
            if (RightPaddle.Y > BallVector.PointA.AsPoint.Y - 20) RightPaddle.Y -= 10;
            if (RightPaddle.Y < BallVector.PointA.AsPoint.Y - 40) RightPaddle.Y += 10;
            #endregion

            #region Collision
            if (!Screen.PrimaryScreen.WorkingArea.Contains(new Rectangle(BallVector.PointA.AsPoint, Ball)))
            {
                //Bottom Wall
                if (BallVector.PointA.Y + Ball.Height > Screen.PrimaryScreen.WorkingArea.Bottom)
                {
                    double refAngle = (180 - BallVector.A);
                    BallVector.SetVector(BallVector.D, refAngle);
                }
                //Top Wall
                if (BallVector.PointA.Y < Screen.PrimaryScreen.WorkingArea.Top)
                {
                    if (BallVector.A < 180) //Down and to the left
                    {
                        double refAngle = (90 - BallVector.A);
                        BallVector.SetVector(BallVector.D, 90 + refAngle);
                    }
                    else
                    {
                        double refAngle = (BallVector.A - 270);
                        BallVector.SetVector(BallVector.D, 270 - refAngle);
                    }
                }
                //Left Wall
                if (BallVector.PointA.X < Screen.PrimaryScreen.WorkingArea.Left)
                {
                    Ball = new Size(30, 30);
                    BallVector = new Vector(new PointD(CenterScreen.X - 15, CenterScreen.Y - 15), 5, 45);
                }
                //Right Wall
                if (BallVector.PointA.X + Ball.Width > Screen.PrimaryScreen.WorkingArea.Right)
                {
                    Ball = new Size(30, 30);
                    BallVector = new Vector(new PointD(CenterScreen.X - 15, CenterScreen.Y - 15), 5, 225);
                }
            }
            #endregion

            #region BallTrail
            if (BallTrail.Count > 0)
            {
                for (int x = 0; x < BallTrail.Count; x++)
                {
                    if ((BallTrail[x].Width >= 4) && (BallTrail[x].Height >= 4))
                    {
                        BallTrail[x] = new Rectangle(BallTrail[x].X + 1, BallTrail[x].Y + 1, BallTrail[x].Width - 2, BallTrail[x].Height - 2);
                    }
                    else BallTrail.RemoveAt(x);
                }
            }
            #endregion
            Master.Invalidate();
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.White, LeftPaddle);
            g.FillRectangle(Brushes.White, RightPaddle);
            foreach (Rectangle trail in BallTrail)
            {
                g.FillRectangle(Brushes.LightGray, trail);
            }
            g.FillRectangle(Brushes.White, new Rectangle(BallVector.PointA.AsPoint, Ball));
            g.DrawString(BallTrail.Count.ToString(), SystemFonts.DefaultFont, Brushes.White, new Point(10, 10));
        }
    }
}
