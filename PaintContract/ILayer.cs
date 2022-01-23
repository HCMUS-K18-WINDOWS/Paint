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
        bool checkPosition(Point2D position);

        void handleMove(Point2D p);

        void handleMoveDone();
        UIElement Draw();
    }
}
