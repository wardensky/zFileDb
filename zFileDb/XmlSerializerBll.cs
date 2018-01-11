using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
namespace wardensky.xmldb
{
    public class XmlSerializerBll<T> : AbstractSerializeBll<T>
    {
        private static XmlSerializerBll<T> instance;
        private XmlSerializer ks = new XmlSerializer(typeof(List<T>));

        private XmlSerializerBll()
        {
            this.SetDbFile();
            this.ReadDb();
        }
        protected override void SetDbFile()
        {
            this.SetDbFile(".xml");
        }
        public static XmlSerializerBll<T> GetInstance()
        {
            if (instance == null)
            {
                instance = new XmlSerializerBll<T>();
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

                using (Stream writer = new FileStream(this.Dbfile, mode, FileAccess.ReadWrite))
                {
                    ks.Serialize(writer, this.entityList);
                }
            }
        }

        protected override void ReadDb()
        {
            if (File.Exists(this.Dbfile))
            {
                lock (this.lockObject)
                {
                    using (Stream reader = new FileStream(this.Dbfile, FileMode.Open, FileAccess.ReadWrite))
                    {
                        this.entityList = ks.Deserialize(reader) as List<T>;
                    }
                }
            }
            else
            {
                this.entityList = new List<T>();
            }
        }
    }
}