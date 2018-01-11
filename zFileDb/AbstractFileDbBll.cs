using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
namespace wardensky.xmldb
{
    public abstract class AbstractFileDbBll<T> : IFileDbBll<T> where T : new()
    {
        public virtual string DbFile
        {
            get { return this.bll.Dbfile; }
            set { bll.Dbfile = value; }
        }
        protected AbstractSerializeBll<T> bll;
        public bool Delete(string id)
        {
            var entity = this.Select(id);
            bll.DeleteById(id);
            return true;
        }

        public bool Delete(Expression<Func<T, bool>> express)
        {
            T data = this.SelectAll().Where(express.Compile()).First();
            if (data != null)
            {
                this.Delete(this.GetIdValue(data));
            }
            return true;
        }

        protected String SetIdGuid(T t)
        {
            string id = Guid.NewGuid().ToString();
            Type type = typeof(T);
            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.Name.ToLower().Equals("id"))
                {
                    pi.SetValue(t, id);
                }
            }
            return id;
        }

        protected String GetIdValue(T t)
        {
            Type type = typeof(T);
            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.Name.ToLower().Equals("id"))
                {
                    object o = pi.GetValue(t, null);
                    return o == null ? "" : o.ToString();
                }
            }
            return "";
        }

        public T Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("object is null");
            }
            if (String.IsNullOrEmpty(this.GetIdValue(entity)))
            {
                throw new ArgumentException("object does not have valid id value");
            }
            this.bll.Insert(entity);
            return entity;
        }
        public T InsertOrUpdate(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("object is null");
            }
            string id = this.GetIdValue(entity);
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentException("object does not have valid id value");
            }
            T old = this.Select(id);
            if (old != null)
            {
                this.bll.UpdateById(entity);
                return entity;
            }

            bll.Insert(entity);
            return entity;
        }

        public List<T> Insert(List<T> list)
        {
            bll.InsertRange(list);
            return list;
        }
        public System.Collections.Generic.List<T> SelectAll()
        {
            return bll.SelectAll();
        }
        public T Update(string oldId, T entity)
        {
            bll.UpdateById(entity);
            return entity;
        }
        public T Select(string id)
        {
            return bll.SelectById(id);
        }
        public System.Collections.Generic.List<T> SelectBy(string name, object value)
        {
            return bll.SelectBy(name, value);
        }
        public bool DeleteAll()
        {
            bll.DeleteAll();
            return true;
        }
    }
}
