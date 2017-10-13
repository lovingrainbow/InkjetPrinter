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
            point1.X = point.X * Math.Cos(-1*angle) + point.Y * Math.Sin(-1*angle) + pointConverter.dX;
            point1.Y = -1 * point.X * Math.Sin(-1*angle) + point.Y * Math.Cos(-1*angle) + pointConverter.dY;
            point1.Z = point.Z;
            return point1;
        }

        public static double ZInterpolationByX(double x)
        {
            //Z軸內插數值
            //當X軸移動後，Z軸噴灑高度需要調整距離
            double z0 = 0;
            double z1 = 200;

            double x0 = 0;
            double x1 = 300000;

            return ((x - x0) / (x1 - x0)) * (z1 - z0);
            
        }
    }
}
