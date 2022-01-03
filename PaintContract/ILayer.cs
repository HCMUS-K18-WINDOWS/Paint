using System;
using System.Windows;

namespace PaintContract
{
    public interface ILayer : ICloneable
    {
        Point2D Offset { get; set; }
        UIElement Draw();
    }
}
