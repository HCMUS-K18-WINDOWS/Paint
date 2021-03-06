using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PaintContract
{
    public enum StrokeType {
        SOLID,
        DASH,
        DASHDOT,
        DOTDOT
    }

    public interface IShape : IEditableLayer
    {
        public StrokeType Stroke { get; set; }
        public double Thickness { get; set; }
        public Color OutlineColor { get; set; }
        public Color InsideColor { get; set; }
    }
}
