using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintContract
{
    public class Point2D: ICloneable
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point2D()
        {

        }

        public Point2D(double _x, double _y)
        {
            X = _x;
            Y = _y;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
