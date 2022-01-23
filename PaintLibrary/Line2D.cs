using PaintContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        public int checkPosition(Point2D position)
        {
            Line line = (Line)this.Draw();
            // ax + by + c = 0
            // a*start.x + b.start.y + c = 0
            // a*end.x + b.end.y + c = 0
            // y = ax + b;
            // y1 = ax1 + b;
            // y2 = ax2 + b;
            // y1 - y2 = a(x1 - x2);
            // a = (y1-y2)/(x1-x2);
            // b = y1 - ax1;

            if((_start.X <= _end.X) && (position.X > _end.X || position.X < _start.X))
            {
                return 0;
            }

            if((_start.X > _end.X) && (position.X < _end.X || position.X > _start.X))
            {
                return 0;
            }

            var a = (_start.Y - _end.Y)/(_start.X - _end.X);
            var b = -1;
            var c = _start.Y - a * _start.X;

            // distance
            // y = 3x + 2 => 3x - y +2 = 0;
            var distance = Math.Abs(a*position.X + b*position.Y + c)/Math.Sqrt((a*a + b*b));
            if(distance <= this.Thickness /2)
            {
                return 1;
            }
            return 0;
        }

        public UIElement DrawBorder()
        {
            Line line = (Line)this.Draw();
            line.StrokeDashArray = new DoubleCollection(new double[] { 2, 4 });
            line.Stroke = new SolidColorBrush(Colors.Blue);
            return line;
        }

        public void handleMove(Point2D p)
        {
            Offset = p;
        }

        public void handleMoveDone()
        {
            _start.X += Offset.X;
            _start.Y += Offset.Y;
            _end.X += Offset.X;
            _end.Y += Offset.Y;
            Offset = new Point2D() { X = 0, Y = 0 };
        }
    }
}
