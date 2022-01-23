using System;
using System.Runtime.Serialization;
using System.Windows;

namespace PaintContract
{
    public interface ILayer : ICloneable
    {
        string Name { get; }
        Point2D Offset { get; set; }
        public bool IsShow { get; set; }
        public string GetUniqueName();
        int checkPosition(Point2D position);  //  0 out,  1 in, 2 above x, 3 above y, 4 corner

        void handleMove(Point2D p);

        void handleMoveDone();
        UIElement Draw();
    }
}
