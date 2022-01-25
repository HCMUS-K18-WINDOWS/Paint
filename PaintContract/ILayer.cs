using Paint;
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
        CursorState checkPosition(Point2D position);  //  0 out,  1 in, 2 above top, 3 above left, 4 above right, 5 above bot

        void handleMove(Point2D p);

        void handleMoveDone();

        void handleResize(CursorState direction, double deltaX, double deltaY);

        void HandleStart(double _x, double _y);
        void HandleEnd(double _x, double _y);
        UIElement Draw();
    }
}
