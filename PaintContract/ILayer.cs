using System;
using System.Windows;

namespace PaintContract
{
    public interface ILayer : ICloneable
    {
        string Name { get; }
        Point2D Offset { get; set; }
        UIElement Draw();
    }
}
