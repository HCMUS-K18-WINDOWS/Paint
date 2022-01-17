using PaintContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintLibrary
{
    public class Line2D : IShape
    {
        public static int numberOfInstances = 0;
        public Point2D _start = new Point2D();
        public Point2D _end = new Point2D();

        public string Name => "Line";

        public Point2D Offset { get ; set; }
        public StrokeType Stroke { get; set; }
        public double Thickness { get; set; }
        public Color OutlineColor { get; set; }
        public bool IsShow { get; set; }
        public Color InsideColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Line2D()
        {
            Offset = new Point2D() { X = 0, Y = 0 };
            Stroke = StrokeType.SOLID;
            Thickness = 1;
            OutlineColor = Colors.Black;
            IsShow = true;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public UIElement Draw()
        {
            if (!IsShow) return null;
            var line = new Line()
            {
                X1 = _start.X + Offset.X,
                Y1 = _start.Y + Offset.Y,
                X2 = _end.X + Offset.X,
                Y2 = _end.Y + Offset.Y,
                StrokeThickness = Thickness,
            };

            switch (Stroke)
            {
                case StrokeType.SOLID:
                    line.Stroke = new SolidColorBrush(OutlineColor);
                    break;
                case StrokeType.DASH:
                    line.Stroke = new SolidColorBrush(OutlineColor);
                    line.StrokeDashArray = new DoubleCollection(new double[] { 6, 1 });
                    break;
                case StrokeType.DASHDOT:
                    line.Stroke = new SolidColorBrush(OutlineColor);
                    line.StrokeDashArray = new DoubleCollection(new double[] { 5, 5, 1, 5 });
                    break;
                default:
                    break;
            }
            

            return line;
        }

        public void HandleEnd(double _x, double _y)
        {
            _start = new Point2D() { X = _x, Y = _y };
        }

        public void HandleStart(double _x, double _y)
        {
            _end = new Point2D() { X = _x, Y = _y };
        }

        public string GetUniqueName()
        {
            return Name + ++numberOfInstances;
        }
    }
}
