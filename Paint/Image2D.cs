using PaintContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    public class Image2D : ILayer
    {
        public string FilePath { get; set; }
        public static int numberOfInstances = 0;
        public string Name => "Image";

        public Point2D Offset { get; set; }
        public bool IsShow { get; set; }

        public Image2D()
        {

        }

        public Image2D(string filePath)
        {
            FilePath = filePath;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public UIElement Draw()
        {
            
            BitmapImage bitmap = new BitmapImage(new Uri(FilePath));
            Image img = new Image();
            img.Source = bitmap;
            img.Width = 200;
            RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
            return img;
        }

        public string GetUniqueName()
        {
            return Name + ++numberOfInstances;
        }
    }
}
