using PaintContract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public class LayerSaveDto
    {
        public ObservableCollection<KeyValuePair<string, ILayer>> Layers { get; set; }
        public Dictionary<string, ILayer> PenLines { get; set; }
    }
}
