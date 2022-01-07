using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public class IOManager
    {
        private IOManager? _instance;
        private IOManager()
        {

        }
        public IOManager GetInstance()
        {
            if (_instance == null)
                _instance = new IOManager();
            return _instance;
        }
        public void SaveToBinaryFile<T>(List<T> list, string file)
        {
            using (var stream = File.OpenWrite(file))
            {
                var formater = new BinaryFormatter();
                formater.Serialize(stream, list);
            }
        }
        public List<T>? LoadFromBinaryFile<T>(string file)
        {
            var list = new List<T>();
            using (var stream = File.OpenRead(file))
            {
                var formatter = new BinaryFormatter();
                list = formatter.Deserialize(stream) as List<T>;
            }
            return list;
        }
    }
}
