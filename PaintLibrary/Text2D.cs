using PaintContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Paint
{
    public class Text2D : IEditableLayer
    {
        public static int numberOfInstances = 0;
        public string Name => "Text";

        public Point2D Offset { get ; set; }
        public bool IsShow { get; set ; }

        public string Text { get; set; }

        private Point2D _start = new Point2D();
        private Point2D _end = new Point2D();

        public Text2D()
        {
            Offset = new Point2D(0, 0);
            Text = "Demo";
        }

        public CursorState checkPosition(Point2D position)
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            var newText = (Text2D)this.MemberwiseClone();
            newText._end = (Point2D)_end.Clone();
            newText._start = (Point2D)_start.Clone();
            return newText;
        }

        public UIElement Draw()
        {
            var width = Math.Abs(_end.X - _start.X);
            var height = Math.Abs(_end.Y - _start.Y);

            var text = new TextBox()
            {
                Text = Text,
                Width = 300,
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.Black),
            };
            if (_start.X < _end.X)
            {
                Canvas.SetLeft(text, _start.X + Offset.X);
            }
            else
            {
                Canvas.SetLeft(text, _end.X + Offset.X);
            }

            if (_start.Y < _end.Y)
            {
                Canvas.SetTop(text, _start.Y + Offset.Y);
            }
            else
            {
                Canvas.SetTop(text, _end.Y + Offset.Y);
            }
            return text;
        }

        public UIElement DrawBorder()
        {
            throw new NotImplementedException();
        }

        public string GetUniqueName()
        {
            return Name + ++numberOfInstances;
        }

        public void HandleStart(double _x, double _y)
        {
            _start = new Point2D()
            {
                X = _x,
                Y = _y
            };
        }

        public void HandleEnd(double _x, double _y)
        {
            _end = new Point2D()
            {
                X = _x,
                Y = _y
            };
            
        }

        public void handleMove(Point2D p)
        {
            throw new NotImplementedException();
        }

        public void handleMoveDone()
        {
            throw new NotImplementedException();
        }

        public void handleResize(CursorState direction, double deltaX, double deltaY)
        {
            throw new NotImplementedException();
        }
    }
}
