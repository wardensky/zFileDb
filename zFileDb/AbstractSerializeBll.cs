using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace wardensky.xmldb
{
    public abstract class AbstractSerializeBll<T>
    {
        protected Object lockObject = new object();
        private string dbFile;
        public string Dbfile
        {
            get
            {
                return dbFile;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !value.Equals(dbFile))
                {
                    this.entityList.Clear();
                }
                dbFile = value;
                this.ReadDb();
            }
        }
        protected List<T> entityList = new List<T>();

        protected abstract void SetDbFile();

        protected void SetDbFile(String suffix)
        {
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            if (Directory.Exists(folder) == false)
            {
                Directory.CreateDirectory(folder);
            }
            Type type = typeof(T);
            if (string.IsNullOrEmpty(this.Dbfile))
            {
                this.Dbfile = Path.Combine(folder, type.Name + suffix);
            }

        }

        public void Insert(T entity)
        {
            this.entityList.Add(entity);
            this.WriteDb();
        }
        public void InsertRange(IList<T> list)
        {
            this.entityList.AddRange(list);
            this.WriteDb();
        }
        public System.Collections.Generic.List<T> SelectBy(string name, Object value)
        {
            System.Collections.Generic.List<T> list = new List<T>();
            if (value == null)
            {
                return list;
            }
            Type t = typeof(T);
            foreach (var inst in this.entityList)
            {
                foreach (PropertyInfo pro in t.GetProperties())
                {
                    if (pro.Name.ToLower() == name.ToLower())
                    {
                        if (value.ToString() == (pro.GetValue(inst, null) ?? string.Empty).ToString())
                        {
                            list.Add(inst);
                        }
                    }
                }
            }
            return list;
        }
        public T SelectById(string id)
        {
            Type t = typeof(T);
            foreach (var inst in this.entityList)
            {
                foreach (PropertyInfo pro in t.GetProperties())
                {
                    if (pro.Name.ToLower() == "id")
                    {
                        if (id == (pro.GetValue(inst, null) ?? string.Empty).ToString())
                        {
                            return inst;
                        }
                    }
                }
            }
            return default(T);
        }
        public void UpdateById(T entity)
        {
            Type t = typeof(T);
            string id = string.Empty;
            foreach (PropertyInfo pro in t.GetProperties())
            {
                if (pro.Name.ToLower() == "id")
                {
                    id = (pro.GetValue(entity, null) ?? string.Empty).ToString();
                    break;
                }
            }
            this.DeleteById(id);
            this.Insert(entity);
        }
        public void DeleteById(string id)
        {
            Type t = typeof(T);
            T entity = default(T);
            foreach (var inst in this.entityList)
            {
                foreach (PropertyInfo pro in t.GetProperties())
                {
                    if (pro.Name.ToLower() == "id")
                    {
                        if ((pro.GetValue(inst, null) ?? string.Empty).ToString() == id)
                        {
                            entity = inst;
                            goto FinishLoop;
                        }
                    }
                }
            }
        FinishLoop:
            this.entityList.Remove(entity);
            this.WriteDb();
        }
        public List<T> SelectAll()
        {
            return this.entityList;
        }
        public void DeleteAll()
        {
            this.entityList.Clear();
            this.WriteDb();
        }
        protected abstract void WriteDb();

        protected abstract void ReadDb();
    }
}
