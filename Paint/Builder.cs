using PaintContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Paint
{
    public interface Builder
    {
        void Reset();
        void BuildColor(Color color);
        void BuildThickness(double t);
        void BuildStroke(StrokeType stt);
        IShape BuildShape(string shapeType);
    }
}
