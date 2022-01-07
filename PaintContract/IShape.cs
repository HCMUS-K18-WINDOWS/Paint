using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintContract
{
    public interface IShape : ILayer
    {
        void HandleStart(Point2D point);
        void HandleEnd(Point2D point);
    }
}
