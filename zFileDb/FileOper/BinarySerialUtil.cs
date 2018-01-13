using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace wardensky.xmldb
{
    public class BinarySerialUtil<T> : ISerialUtil<T> where T : new()
    {
        BinaryFormatter bf = new BinaryFormatter();
        public void SaveFile(IList<T> data, String file, Object lockObject)
        {
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
                    using (FileStream fs = new FileStream(file, mode, FileAccess.ReadWrite))
                    {
                        bf.Serialize(fs, data);
                    }
                }
            }
            else
            {
                using (FileStream fs = new FileStream(file, mode, FileAccess.ReadWrite))
                {
                    bf.Serialize(fs, data);
                }
            }
        }

        public IList<T> ReadFile(String file, Object lockObject)
        {
            if (File.Exists(file))
            {
                lock (lockObject)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Length > 0)
                    {
                        using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
                        {
                            return bf.Deserialize(fs) as List<T>;
                        }
                    }
                }
            }
            return new List<T>();
        }
    }
}