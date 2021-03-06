using PaintContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Paint
{
    public class ShapeBuilder : Builder
    {
        protected StrokeType Stroke;
        protected double Thickness;
        protected Color OutlineColor;
        protected Color InsideColor;
        public Dictionary<string, IShape> _prototypes { get; set; }

        private ShapeBuilder()
        {
            _prototypes = new Dictionary<string, IShape>();
            Reset();
        }

        private static ShapeBuilder _instance;
        
        public static ShapeBuilder GetInstance()
        {
            if (_instance == null)
                _instance = new ShapeBuilder();
            return _instance;
        }
        public void BuildColor(Color color)
        {
            OutlineColor = color;
        }

        public void BuildColorFill(Color color)
        {
            InsideColor = color;
        }

        public IShape BuildShape(string shapeType)
        {
            if (shapeType == "None") return null;
            IShape shape = (IShape)_prototypes[shapeType].Clone();
            shape.OutlineColor = OutlineColor;
            shape.Stroke = Stroke;
            shape.Thickness = Thickness;
            if (shapeType != "Line")
            {
                shape.InsideColor = InsideColor;
            }
            return shape;
        }

        public void BuildStroke(StrokeType stt)
        {
            Stroke = stt;
        }

        public void BuildThickness(double t)
        {
            Thickness = t;
        }

        public void Reset()
        {
            Stroke = StrokeType.SOLID;
            Thickness = 1;
            OutlineColor = Colors.Black;
        }
    }
}
