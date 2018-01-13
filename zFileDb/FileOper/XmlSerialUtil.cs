using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
namespace wardensky.xmldb
{
    public class XmlSerialUtil<T> : ISerialUtil<T> where T : new()
    {
        public void SaveFile(IList<T> data, String file,Object lockObject)
        {
            XmlSerializer ks = new XmlSerializer(typeof(List<T>));
            FileInfo fi = new FileInfo(file);
            var dir = fi.Directory;
            if (!dir.Exists)
            {
                dir.Create();
            }
            FileMode mode = fi.Exists ? FileMode.Open : FileMode.Create;
            if (lockObject != null)
            {
                lock (lockObject)
                {
                    using (Stream writer = new FileStream(file, mode, FileAccess.ReadWrite))
                    {
                        ks.Serialize(writer, data);
                    }
                }
            }
            else
            {
                using (Stream writer = new FileStream(file, mode, FileAccess.ReadWrite))
                {
                    ks.Serialize(writer, data);
                }
            }
        }

        public IList<T> ReadFile(String file, Object lockObject)
        {
            IList<T> ret = new List<T>();
            XmlSerializer ks = new XmlSerializer(typeof(List<T>));
            if (File.Exists(file))
            {
                using (Stream reader = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
                {
                    ret = ks.Deserialize(reader) as IList<T>;
                }
            }
            return ret;
        }


    }
}