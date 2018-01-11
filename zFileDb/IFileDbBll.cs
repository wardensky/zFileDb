using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace wardensky.xmldb
{
    public interface IFileDbBll<T> where T : new()
    {
        void Delete(string id);

        void Delete(Expression<Func<T, bool>> express);

        void Insert(T entity);

        void Insert(List<T> list);

        System.Collections.Generic.List<T> SelectAll();

        void Update(string oldId, T entity);

        T Select(string id);

        System.Collections.Generic.List<T> SelectBy(string name, object value);

        void DeleteAll();

    }
}
