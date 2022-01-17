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
    public class Rectangle2D : IShape
    {
        public static int _numberOfInstances = 0;
        public Point2D _topLeft;
        public Point2D _botRight;
        public string Name => "Rectangle";

        public Point2D Offset { get; set ; }
        public StrokeType Stroke { get; set; }
        public double Thickness { get ; set ; }
        public Color OutlineColor { get; set ; }
        public bool IsShow { get; set; }
        public Color InsideColor { get; set; }

        public Rectangle2D()
        {
            Offset = new Point2D() { X = 0, Y = 0 };
            Stroke = StrokeType.SOLID;
            Thickness = 1;
            OutlineColor = Colors.Black;
            InsideColor = Colors.White;
            IsShow = true;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public UIElement Draw()
        {
            if (!IsShow) return null;
            var width = Math.Abs(_botRight.X - _topLeft.X);
            var height = Math.Abs(_botRight.Y - _topLeft.Y);
            var rect = new Rectangle()
            {
                Width = width,
                Height = height,
                StrokeThickness = Thickness
            };

            switch(Stroke)
            {
                case StrokeType.SOLID:
                    rect.Stroke = new SolidColorBrush(OutlineColor);
                    rect.Fill = new SolidColorBrush(InsideColor);
                    break;
                case StrokeType.DASH:
                    rect.Stroke = new SolidColorBrush(OutlineColor);
                    rect.StrokeDashArray = new DoubleCollection(new double[] { 6, 1 });
                    break;
                case StrokeType.DASHDOT:
                    rect.Stroke = new SolidColorBrush(OutlineColor);
                    rect.StrokeDashArray = new DoubleCollection(new double[] { 5, 5, 1, 5 });
                    break;
                default:
                    break;
            }

            if (_topLeft.X < _botRight.X)
            {
                Canvas.SetLeft(rect, _topLeft.X);
            }
            else
            {
                Canvas.SetLeft(rect, _botRight.X);
            }

            if (_topLeft.Y < _botRight.Y)
            {
                Canvas.SetTop(rect, _topLeft.Y);
            }
            else
            {
                Canvas.SetTop(rect, _botRight.Y);
            }
            return rect;
        }

        public void HandleEnd(double _x, double _y)
        {
            _botRight = new Point2D()
            {
                X = _x,
                Y = _y
            };
        }

        public void HandleStart(double _x, double _y)
        {
            _topLeft = new Point2D()
            {
                X = _x,
                Y = _y
            };
        }

        public string GetUniqueName()
        {
            return Name + ++_numberOfInstances;
        }
    }
}
