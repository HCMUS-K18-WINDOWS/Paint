using PaintContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PaintLibrary
{
    public class Rectangle2D : IShape
    {
        public string Name => "Rectangle";

        public Point2D Offset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public UIElement Draw()
        {
            throw new NotImplementedException();
        }

        public void HandleEnd(Point2D point)
        {
            throw new NotImplementedException();
        }

        public void HandleStart(Point2D point)
        {
            throw new NotImplementedException();
        }
    }
}
