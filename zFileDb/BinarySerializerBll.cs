using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace wardensky.xmldb
{
    public class BinarySerializerBll<T> : AbstractSerializeBll<T>
    {
        private static BinarySerializerBll<T> instance;
        private BinaryFormatter bf = new BinaryFormatter();
        private BinarySerializerBll()
        {
            this.SetDbFile();
            this.ReadDb();
        }
        protected override void SetDbFile()
        {
            this.SetDbFile(".dat");
        }
        public static BinarySerializerBll<T> GetInstance()
        {
            if (instance == null)
            {
                instance = new BinarySerializerBll<T>();
            }
            return instance;
        }

        protected override void WriteDb()
        {
            lock (this.lockObject)
            {
                FileInfo fi = new FileInfo(this.Dbfile);
                var dir = fi.Directory;
                if (!dir.Exists)
                {
                    dir.Create();
                }
                FileMode mode = fi.Exists ? FileMode.Open : FileMode.Create;
                using (FileStream fs = new FileStream(this.Dbfile, mode, FileAccess.ReadWrite))
                {
                    bf.Serialize(fs, this.entityList);
                }
            }
        }
       
        protected override void ReadDb()
        {
            if (File.Exists(this.Dbfile))
            {
                lock (this.lockObject)
                {
                    FileInfo fi = new FileInfo(this.Dbfile);
                    if (fi.Length > 0)
                    {
                        using (FileStream fs = new FileStream(this.Dbfile, FileMode.Open, FileAccess.ReadWrite))
                        {
                            this.entityList = bf.Deserialize(fs) as List<T>;
                            return;
                        }
                    }
                }
            }

            this.entityList = new List<T>();

        }
    }
}