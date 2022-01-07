using PaintContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintLibrary
{
    public class Ellipse2D : IShape
    {
        public static int numberOfInstance = 0;
        public Point2D _topLeft;
        public Point2D _botRight;
        public StrokeType Stroke { get; set; }
        public double Thickness { get; set; }
        public Color OutlineColor { get; set; }

        public string Name => "Ellipse";

        public Point2D Offset { get; set; }
        public bool IsShow { get; set; }
        public Ellipse2D()
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
            var width = Math.Abs(_botRight.X - _topLeft.X);
            var height = Math.Abs(_botRight.Y - _topLeft.Y);
            var ellipse = new Ellipse()
            {
                Width = width,
                Height = height,
                StrokeThickness = Thickness
            };

            switch (Stroke)
            {
                case StrokeType.SOLID:
                    ellipse.Stroke = new SolidColorBrush(OutlineColor);
                    break;
                case StrokeType.DASH:
                    ellipse.Stroke = new SolidColorBrush(OutlineColor);
                    ellipse.StrokeDashArray = new DoubleCollection(new double[] { 6, 1 });
                    break;
                case StrokeType.DASHDOT:
                    ellipse.Stroke = new SolidColorBrush(OutlineColor);
                    ellipse.StrokeDashArray = new DoubleCollection(new double[] { 5, 5, 1, 5 });
                    break;
                default:
                    break;
            }

            if (_topLeft.X < _botRight.X)
            {
                Canvas.SetLeft(ellipse, _topLeft.X);
            }
            else
            {
                Canvas.SetLeft(ellipse, _botRight.X);
            }

            if (_topLeft.Y < _botRight.Y)
            {
                Canvas.SetTop(ellipse, _topLeft.Y);
            }
            else
            {
                Canvas.SetTop(ellipse, _botRight.Y);
            }
            return ellipse;
        }

        public string GetUniqueName()
        {
            return Name + ++numberOfInstance;
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
    }
}
