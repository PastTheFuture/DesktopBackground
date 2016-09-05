using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DesktopBackground
{
    public class Vector
    {
        public PointD PointA, PointB;
        //           pB
        //         /  |
        //       /    |
        //     /      |
        //  pA________|

        private double width
        { get { return (PointB.X - PointA.X); } }
        private double height
        { get { return (PointB.Y - PointA.Y); } }

        public Vector() { }
        public Vector(PointD pA, PointD pB)
        {
            PointA = pA;
            PointB = pB;
        }
        public Vector(PointD pA, double distance, double angle)
        {
            PointA = pA;
            SetVector(distance, angle);
        }
        //public Vector(PointD pA, PointD vel)
        //{
        //    PointA = pA;
        //    PointB = new PointD(pA.X + vel.X, pA.Y + vel.Y);
        //}

        //Vector
        public double Distance { get { return D; } }
        public double D
        {
            get { return (Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2))); }
        }

        public double Angle { get { return A; } }
        public double A
        {
            get
            {
                double ret = MathV.RadianToDegrees(Math.Atan2(width, height));
                if (ret < 0) ret += 360;
                return ret;
            }
        }

        public void SetVector(double dist, double angle)
        {
            double newHeight = (MathV.SinD(angle) * dist);
            double newWidth = (MathV.CosD(angle) * dist);
            PointB = new PointD(PointA.X + newHeight, PointA.Y + newWidth);
        }

        public double Radians { get { return R; } }
        public double R
        { get { return Math.Atan2(height, width); } }

        public PointD Velocity { get { return V; } }
        public PointD V { get { return new PointD(width, height); } }
    }

    public class PointD
    {
        private double px;
        private double py;

        public double X
        {
            get { return px; }
            set { px = value; }
        }
        public double Y
        {
            get { return py; }
            set { py = value; }
        }

        public PointD() { }
        public PointD(double x, double y)
        { px = x; py = y; }

        public Point AsPoint
        { get { return new Point((int)px, (int)py); } }

        public static PointD operator +(PointD pa, PointD pb)
        {
            PointD pc = new PointD(pa.X + pb.X, pa.Y + pb.Y);
            return pc;
        }
        public static PointD operator -(PointD pa, PointD pb)
        {
            PointD pc = new PointD(pa.X - pb.X, pa.Y - pb.Y);
            return pc;
        }
        public static PointD operator *(PointD pa, int m)
        {
            PointD pc = new PointD(pa.X * m, pa.Y * m);
            return pc;
        }
        public static PointD operator *(PointD pa, double m)
        {
            PointD pc = new PointD(pa.X * m, pa.Y * m);
            return pc;
        }
        public static PointD operator /(PointD pa, int m)
        {
            PointD pc = new PointD(pa.X / m, pa.Y / m);
            return pc;
        }
        public static PointD operator /(PointD pa, double m)
        {
            PointD pc = new PointD(pa.X / m, pa.Y / m);
            return pc;
        }
    }

    public static class MathV
    {
        public static double RadianToDegrees(double r)
        { return ((r * 180) / Math.PI); }
        public static double DegreesToRadians(double d)
        { return ((d * Math.PI) / 180); }

        public static double CosD(double a)
        { return Math.Cos(DegreesToRadians(a)); }
        public static double SinD(double a)
        { return Math.Sin(DegreesToRadians(a)); }
        public static double TanD(double a)
        { return Math.Tan(DegreesToRadians(a)); }

        public static double DistanceBetweenPoints(PointD pa, PointD pb)
        {
            return DBP(pa, pb);
        }
        public static double DBP(PointD pa, PointD pb)
        {
            double width = (pb.X - pa.X);
            double height = (pb.Y - pa.Y);

            return (Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2)));
        }

        public static double AngleBetweenPoints(PointD pa, PointD pb)
        { return ABP(pa, pb); }
        public static double ABP(PointD pa, PointD pb)
        {
            double width = (pb.X - pa.X);
            double height = (pb.Y - pa.Y);
            return Math.Atan(width / height);
        }

        public static PointD VelocityToPoint(PointD pa, PointD vel)
        {
            return VTP(pa, vel);
        }
        public static PointD VTP(PointD pa, PointD vel)
        {
            return new PointD(pa.X + vel.X, pa.Y + vel.Y);
        }

        public static PointD PointToVelocity(PointD pa, PointD pb)
        { return PTV(pa, pb); }
        public static PointD PTV(PointD pa, PointD pb)
        {
            return new PointD(pb.X - pa.X, pb.Y - pa.Y);
        }

        public static bool ContainsPoint(this Rectangle rect, Point p)
        {
            if ((p.X <= rect.Right) && (p.X >= rect.Left) && (p.Y <= rect.Bottom) && (p.Y >= rect.Top))
            { return true; }
            else return false;
        }
    }
}
