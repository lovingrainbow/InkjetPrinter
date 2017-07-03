using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkjetPrinter
{
    public class Point
    {
        public double X;
        public double Y;
        public double Z;

        public Point GetPoint()
        {
            Point _Point = new Point();
            _Point.X = X;
            _Point.Y = Y;
            _Point.Z = Z;
            return _Point;
        }
    }
    public class PointConverter
    {
        public double dX;
        public double dY;
        public double dZ;
        public double dR;
    }
}
