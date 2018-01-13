using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.Attributes;
using System.Linq.Expressions;

namespace wardensky.xmldb
{
    public class MultiFilesDbBll<T> : IFileDbBll<T> where T : new()
    {
        private Object lockObject = new object();
        [Dependency]
        public ISerialUtil<T> SerialUtil { get; set; }

        protected Dictionary<int, IList<T>> DIC_DATA = new Dictionary<int, IList<T>>();

        protected Dictionary<int, string> DIC_DBFILE = new Dictionary<int, string>();

        private ENUM_SAVE_FILE_NUM num = ENUM_SAVE_FILE_NUM.SINGLE_FILE;
        private ENUM_SAVE_FILE_TYPE type = ENUM_SAVE_FILE_TYPE.BINARY;
        public MultiFilesDbBll(ENUM_SAVE_FILE_NUM num, ENUM_SAVE_FILE_TYPE type)
        {
            this.num = num;
            this.type = type;
            this.InitDb();
        }


        private void InitDb()
        {
            for (int key = 0; key < (int)this.num; key++)
            {
                this.DIC_DATA.Add(key, new List<T>());
                this.DIC_DBFILE.Add(key, CommonUtil.GetFileName(key, this.type, typeof(T)));
            }
        }

        public T Insert(T entity)
        {
            this.TryInit();
            int key = CommonUtil.GetKey(entity, this.num);
            if (!this.DIC_DATA.ContainsKey(key))
            {
                this.DIC_DATA.Add(key, new List<T>());
            }
            this.DIC_DATA[key].Add(entity);
            this.SerialUtil.SaveFile(this.DIC_DATA[key], this.DIC_DBFILE[key], this.lockObject);
            return entity;
        }
        public IList<T> Insert(IList<T> list)
        {
            return this.InsertRange(list);

        }

        public IList<T> InsertRange(IList<T> list)
        {
            this.TryInit();
            ISet<int> keySet = new HashSet<int>();
            foreach (T t in list)
            {
                int key = CommonUtil.GetKey(t, this.num);
                keySet.Add(key);
                if (!this.DIC_DATA.ContainsKey(key))
                {
                    this.DIC_DATA.Add(key, new List<T>());
                    this.DIC_DBFILE.Add(key, CommonUtil.GetFileName(key, this.type, typeof(T)));
                }
                this.DIC_DATA[key].Add(t);
            }
            foreach (int key in keySet)
            {
                SerialUtil.SaveFile(this.DIC_DATA[key], this.DIC_DBFILE[key], this.lockObject);
            }
            return list;
        }
        public IList<T> SelectBy(string name, Object value)
        {
            this.TryInit();
            List<T> list = new List<T>();
            if (value == null)
            {
                return list;
            }
            Type t = typeof(T);
            foreach (var entityList in this.DIC_DATA.Values)
            {
                foreach (var inst in entityList)
                {
                    foreach (PropertyInfo pro in t.GetProperties())
                    {
                        if (pro.Name.ToLower() == name.ToLower())
                        {
                            if (value.ToString() == (pro.GetValue(inst, null) ?? string.Empty).ToString())
                            {
                                list.Add(inst);
                                break;
                            }
                        }
                    }
                }
            }
            return list;
        }

        public T Select(string id)
        {
            T entity = default(T);
            if (id == null)
            {
                return entity;
            }
            this.FindKey(id, out entity);
            return entity;
        }
        private Type t = typeof(T);
        private int FindKey(string id, out T entity)
        {
            this.TryInit();
            entity = default(T);
            foreach (int key in this.DIC_DATA.Keys)
            {
                foreach (var inst in this.DIC_DATA[key])
                {
                    foreach (PropertyInfo pro in t.GetProperties())
                    {
                        if (pro.Name.ToLower() == "id")
                        {
                            if (id == (pro.GetValue(inst, null) ?? string.Empty).ToString())
                            {
                                entity = inst;
                                return key;
                            }
                        }
                    }
                }
            }
            return -1;
        }


        public T InsertOrUpdate(T entity)
        {
            string id = CommonUtil.GetIdValue<T>(entity);
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("entity does not have id value");
            }
            T ret = default(T);
            int key = this.FindKey(id, out ret);
            if (key > 0)
            {
                this.Delete(id);
            }
            this.Insert(entity);
            return entity;
        }
        public T Update(string oldId, T entity)
        {
            this.Delete(oldId);
            this.Insert(entity);
            return entity;
        }


        public bool Delete(string id)
        {
            T entity = default(T);
            int key = this.FindKey(id, out entity);
            if (key < 0)
            {
                return false;
            }
            this.DIC_DATA[key].Remove(entity);
            this.SerialUtil.SaveFile(this.DIC_DATA[key], this.DIC_DBFILE[key], this.lockObject);
            return true;
        }
        public bool Delete(T t)
        {
            return this.Delete(CommonUtil.GetIdValue<T>(t));
        }

        public IList<T> Select(Expression<Func<T, bool>> express)
        {
            return this.SelectAll().Where(express.Compile()).ToList();
        }

        public bool Delete(Expression<Func<T, bool>> express)
        {
            bool ret = true;
            var data = this.Select(express);
            foreach (T t in data)
            {
                ret = ret && this.Delete(t);
            }
            return ret;
        }

        private void TryInit()
        {
            bool empty = true;
            foreach (var data in this.DIC_DATA.Values)
            {
                if (data.Count > 0)
                {
                    empty = false;
                    break;
                }
            }
            if (empty)
            {
                foreach (var key in this.DIC_DBFILE.Keys)
                {
                    string file = this.DIC_DBFILE[key];
                    this.DIC_DATA[key] = this.SerialUtil.ReadFile(file, this.lockObject);
                }
            }
        }

        public IList<T> SelectAll()
        {
            List<T> t = new List<T>();
            this.TryInit();
            foreach (var list in this.DIC_DATA.Values)
            {
                t.AddRange(list);
            }
            return t;
        }
        public bool DeleteAll()
        {
            foreach (var list in this.DIC_DATA.Values)
            {
                list.Clear();
            }
            foreach (var key in this.DIC_DBFILE.Keys)
            {
                this.SerialUtil.SaveFile(this.DIC_DATA[key], this.DIC_DBFILE[key], this.lockObject);
            }
            return true;
        }
    }
}
