using Paint;
using PaintContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        public Color InsideColor { get; set; }

        public string Name => "Ellipse";

        public Point2D Offset { get; set; }
        public bool IsShow { get; set; }
        public Ellipse2D()
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
                    ellipse.Fill = new SolidColorBrush(InsideColor);
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
                Canvas.SetLeft(ellipse, _topLeft.X + Offset.X);
            }
            else
            {
                Canvas.SetLeft(ellipse, _botRight.X + Offset.X);
            }

            if (_topLeft.Y < _botRight.Y)
            {
                Canvas.SetTop(ellipse, _topLeft.Y + Offset.Y);
            }
            else
            {
                Canvas.SetTop(ellipse, _botRight.Y + Offset.Y);
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

        public CursorState checkPosition(Point2D position)
        {
            var element = this.Draw();
            var left = Canvas.GetLeft(element);
            var top = Canvas.GetTop(element);
            var width = Math.Abs(_botRight.X - _topLeft.X);
            var height = Math.Abs(_botRight.Y - _topLeft.Y);
            var right = left + width;
            var bottom = top + height;
            if (position.X < right && position.X > left && position.Y > top && position.Y < bottom)
                return CursorState.In;
            if (position.Y < bottom && position.Y > top)
            {
                if (position.X == left)
                    return CursorState.Left;
                if (position.X == right)
                    return CursorState.Right;
            }
            if (position.X < right && position.X > left)
            {
                if (position.Y == top)
                    return CursorState.Top;
                if (position.Y == bottom)
                    return CursorState.Bottom;
            }
            if ((position.Y == top || position.Y == bottom) && (position.X == left || position.X == right))
            {
                return CursorState.Corner;
            }
            return CursorState.Out;
        }

        public UIElement DrawBorder()
        {
            var element = this.Draw();
            var left = Canvas.GetLeft(element);
            var top = Canvas.GetTop(element);
            var width = Math.Abs(_botRight.X - _topLeft.X);
            var height = Math.Abs(_botRight.Y - _topLeft.Y);
            //var right = left + width;
            //var bottom = top + height;
            var rectangle = new Rectangle()
            {
                Width = width,
                Height = height,
                StrokeDashArray = new DoubleCollection(new double[] { 5, 5, 1, 5 }),
                Stroke = new SolidColorBrush(Colors.Black)
            };

            Canvas.SetLeft(rectangle, left);
            Canvas.SetTop(rectangle, top);
            
            return rectangle;
        }

        public void handleMove(Point2D p)
        {
            Offset = p;
        }

        public void handleMoveDone()
        {
            _topLeft.X += Offset.X;
            _topLeft.Y += Offset.Y;
            _botRight.X += Offset.X;
            _botRight.Y += Offset.Y;
            Offset = new Point2D() { X = 0, Y = 0 };
        }

        public void handleResize(CursorState direction, double delta)
        {
            switch (direction)
            {
                case CursorState.Left:
                    if (_topLeft.X < _botRight.X)
                        _topLeft.X += delta;
                    else
                        _botRight.X += delta;
                    break;
                case CursorState.Right:
                    if (_topLeft.X < _botRight.X)
                        _botRight.X += delta;
                    else
                        _topLeft.X += delta;
                    break;
                case CursorState.Top:
                    if (_topLeft.Y < _botRight.Y)
                        _topLeft.Y += delta;
                    else
                        _botRight.Y += delta;
                    break;
                case CursorState.Bottom:
                    if (_topLeft.Y < _botRight.Y)
                        _botRight.Y += delta;
                    else
                        _topLeft.Y += delta;
                    break;
                default:
                    break;
            }
        }
    }
}
