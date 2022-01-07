using System;
using System.Windows;

namespace PaintContract
{
    public interface ILayer : ICloneable
    {
        string Name { get; }
        Point2D Offset { get; set; }
        public bool IsShow { get; set; }
        public string GetUniqueName();
        UIElement Draw();
    }
}
