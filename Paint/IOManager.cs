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
    }
}
