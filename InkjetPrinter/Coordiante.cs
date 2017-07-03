using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InkjetPrinter
{
    public static class Coordiante
    {


        public static Point CoordinateConveter(Point point, PointConverter pointConverter)
        {
            double angle = Math.PI * pointConverter.dR / 180;
            Point point1 = new Point();
            point1.X = (point.X - pointConverter.dX) * Math.Cos(angle) - (point.Y - pointConverter.dY) * Math.Sin(angle);
            point1.Y = (point.X - pointConverter.dX) * Math.Sin(angle) + (point.Y - pointConverter.dY) * Math.Cos(angle);
            point1.Z = point.Z;
            return point1;
        }

        public static Point ReverseCoordinateConveter(Point point, PointConverter pointConverter)
        {
            double angle = Math.PI * pointConverter.dR / 180;
            Point point1 = new Point();
            point1.X = point.X * Math.Cos(angle) + point.Y * Math.Sin(angle) + pointConverter.dX;
            point1.Y = -1 * point.X * Math.Sin(angle) + point.Y * Math.Cos(angle) + pointConverter.dY;
            point1.Z = point.Z;
            return point1;
        }
    }
}
