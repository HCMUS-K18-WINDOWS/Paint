using Newtonsoft.Json;
using PaintContract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    public class IOManager
    {
        private static IOManager? _instance;
        private IOManager()
        {

        }
        public static IOManager GetInstance()
        {
            if (_instance == null)
                _instance = new IOManager();
            return _instance;
        }
        public void SaveToBinaryFile(LayerSaveDto dic, string file)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented };
            var jsonString = JsonConvert.SerializeObject(dic, settings);
            File.WriteAllText(file, jsonString);
        }
        public LayerSaveDto? LoadFromBinaryFile(string file)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented };
            var jsonString = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<LayerSaveDto>(jsonString, settings);
        }
        public string ExportToPNG(Canvas canvas, string filename)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
            double dpi = 96d;

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(canvas);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                pngEncoder.Save(ms);
                ms.Close();

                System.IO.File.WriteAllBytes(filename, ms.ToArray());
                return "";
            }
            catch (Exception err)
            {
                return err.ToString();
            }
        }
    }
}
