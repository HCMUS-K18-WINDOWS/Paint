using PaintContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Paint
{
    public class Image2D : IEditableLayer
    {
        public string FilePath { get; set; }
        public static int numberOfInstances = 0;
        public string Name => "Image";

        private Point2D _topLeft;
        private Point2D _botRight;

        public Point2D Offset { get; set; }


        public bool IsShow { get; set; }

        public Image2D()
        {
            _topLeft = new Point2D(0, 0);
            Offset = new Point2D(0, 0);
        }

        public Image2D(string filePath)
        {
            FilePath = filePath;
            _topLeft = new Point2D(0, 0);
            Offset = new Point2D(0, 0);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public UIElement Draw()
        {
            BitmapImage bitmap = new BitmapImage(new Uri(FilePath));
            var width = (_botRight != null) ? Math.Abs(_botRight.X - _topLeft.X) : bitmap.PixelWidth ;
            var height = (_botRight != null) ? Math.Abs(_botRight.Y - _topLeft.Y) : bitmap.PixelHeight ;
            Image img = new Image();
            img.Source = bitmap;
            img.Width = width;
            img.Height = height;
            img.Stretch = Stretch.Fill;
            if(_botRight == null)
            {
                _botRight = new Point2D(_topLeft.X + width, _topLeft.Y + bitmap.PixelHeight );
            }
            RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);


            Canvas.SetLeft(img, _topLeft.X + Offset.X);
            Canvas.SetTop(img, _topLeft.Y + Offset.Y);
            return img;
        }

        public string GetUniqueName()
        {
            return Name + ++numberOfInstances;
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
            if (position.Y == top && position.X == left)
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
            switch (direction)
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
                    if (_topLeft.X < _botRight.X && _topLeft.Y < _botRight.Y)
                    {
                        _topLeft.X += deltaX;
                        _topLeft.Y += deltaY;
                    }
                    else if (_topLeft.X > _botRight.X && _topLeft.Y > _botRight.Y)
                    {
                        // end is topleft
                        _botRight.X += deltaX;
                        _botRight.Y += deltaY;
                    }
                    else if (_topLeft.X > _botRight.X && _topLeft.Y < _botRight.Y)
                    {
                        _topLeft.Y += deltaY;
                        _botRight.X += deltaX;
                    }
                    else if (_topLeft.X < _botRight.X && _topLeft.Y > _botRight.Y)
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

        public void HandleStart(double _x, double _y)
        {
            return;
        }

        public void HandleEnd(double _x, double _y)
        {
            return;
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
            Canvas.SetLeft(rectangle, left - 1);
            Canvas.SetTop(rectangle, top - 1);

            return rectangle;
        }
    }
}
