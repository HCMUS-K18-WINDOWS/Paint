using Paint;
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
        public Point2D _topLeft = new Point2D();
        public Point2D _botRight = new Point2D();
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
            var NewRec = (Rectangle2D)this.MemberwiseClone();
            NewRec._botRight = (Point2D)_botRight.Clone();
            NewRec._topLeft = (Point2D)_topLeft.Clone();
            return NewRec;
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
                case StrokeType.DOTDOT:
                    rect.Stroke = new SolidColorBrush(OutlineColor);
                    rect.StrokeDashArray = new DoubleCollection(new double[] { 1, 1 });
                    break;
                default:
                    break;
            }

            if (_topLeft.X < _botRight.X)
            {
                Canvas.SetLeft(rect, _topLeft.X + Offset.X);
            }
            else
            {
                Canvas.SetLeft(rect, _botRight.X + Offset.X);
            }

            if (_topLeft.Y < _botRight.Y)
            {
                Canvas.SetTop(rect, _topLeft.Y + Offset.Y);
            }
            else
            {
                Canvas.SetTop(rect, _botRight.Y + Offset.Y);
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
            if(position.Y == top && position.X == left )
            {
                return CursorState.CornerTopLeft;
            }
            if (position.Y == top && position.X == right)
            {
                return CursorState.CornerTopRight;
            }
            if (position.Y == bottom && position.X == left)
            {
                return CursorState.CornerBottomLeft;
            }
            if (position.Y == bottom && position.X == right)
            {
                return CursorState.CornerBottomRight;
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
                Width = width + 2,
                Height = height + 2,
                StrokeDashArray = new DoubleCollection(new double[] { 5, 5, 1, 5 }),
                Stroke = new SolidColorBrush(Colors.Black)
            };
            Canvas.SetLeft(rectangle, left-1);
            Canvas.SetTop(rectangle, top-1);

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

        public void handleResize(CursorState direction, double deltaX, double deltaY)
        {
            switch(direction)
            {
                case CursorState.Left:
                    if (_topLeft.X < _botRight.X)
                        _topLeft.X += deltaX;
                    else
                        _botRight.X += deltaX;
                    break;
                case CursorState.Right:
                    if (_topLeft.X < _botRight.X)
                        _botRight.X += deltaX;
                    else
                        _topLeft.X += deltaX;
                    break;
                case CursorState.Top:
                    if (_topLeft.Y < _botRight.Y)
                        _topLeft.Y += deltaY;
                    else
                        _botRight.Y += deltaY;
                    break;
                case CursorState.Bottom:
                    if (_topLeft.Y < _botRight.Y)
                        _botRight.Y += deltaY;
                    else
                        _topLeft.Y += deltaY;
                    break;
                case CursorState.CornerTopLeft:
                    //start is topleft
                    if(_topLeft.X < _botRight.X && _topLeft.Y < _botRight.Y)
                    {
                        _topLeft.X += deltaX;
                        _topLeft.Y += deltaY;
                    } else if(_topLeft.X > _botRight.X && _topLeft.Y > _botRight.Y)
                    {
                        // end is topleft
                        _botRight.X += deltaX;
                        _botRight.Y += deltaY;
                    } else if(_topLeft.X > _botRight.X && _topLeft.Y < _botRight.Y)
                    {
                        _topLeft.Y += deltaY;
                        _botRight.X += deltaX;
                    } else if(_topLeft.X < _botRight.X && _topLeft.Y > _botRight.Y)
                    {
                        _topLeft.X += deltaX;
                        _botRight.Y += deltaY;
                    }
                    break;
                case CursorState.CornerTopRight:
                    if (_topLeft.X < _botRight.X && _topLeft.Y < _botRight.Y)
                    {
                        _topLeft.Y += deltaY;
                        _botRight.X += deltaX;
                    }
                    else if (_topLeft.X > _botRight.X && _topLeft.Y > _botRight.Y)
                    {
                        _topLeft.X += deltaX;
                        _botRight.Y += deltaY;
                    }
                    else if (_topLeft.X > _botRight.X && _topLeft.Y < _botRight.Y)
                    {
                        _topLeft.X += deltaX;
                        _topLeft.Y += deltaY;
                    }
                    else if (_topLeft.X < _botRight.X && _topLeft.Y > _botRight.Y)
                    {
                        _botRight.X += deltaX;
                        _botRight.Y += deltaY;
                    }
                    break;
                case CursorState.CornerBottomLeft:
                    if (_topLeft.X < _botRight.X && _topLeft.Y < _botRight.Y)
                    {
                        _topLeft.X += deltaX;
                        _botRight.Y += deltaY;
                    }
                    else if (_topLeft.X > _botRight.X && _topLeft.Y > _botRight.Y)
                    {
                        _topLeft.Y += deltaY;
                        _botRight.X += deltaX;
                    }
                    else if (_topLeft.X > _botRight.X && _topLeft.Y < _botRight.Y)
                    {
                        _botRight.X += deltaX;
                        _botRight.Y += deltaY;
                    }
                    else if (_topLeft.X < _botRight.X && _topLeft.Y > _botRight.Y)
                    {
                        _topLeft.X += deltaX;
                        _topLeft.Y += deltaY;
                    }
                    break;
                case CursorState.CornerBottomRight:
                    if (_topLeft.X < _botRight.X && _topLeft.Y < _botRight.Y)
                    {
                        _botRight.X += deltaX;
                        _botRight.Y += deltaY;
                    }
                    else if (_topLeft.X > _botRight.X && _topLeft.Y > _botRight.Y)
                    {
                        _topLeft.X += deltaX;
                        _topLeft.Y += deltaY;
                    }
                    else if (_topLeft.X > _botRight.X && _topLeft.Y < _botRight.Y)
                    {
                        _topLeft.X += deltaX;
                        _botRight.Y += deltaY;
                    }
                    else if (_topLeft.X < _botRight.X && _topLeft.Y > _botRight.Y)
                    {
                        _botRight.X += deltaX;
                        _topLeft.Y += deltaY;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
