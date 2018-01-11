using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace wardensky.xmldb
{
    public interface IFileDbBll<T> where T : new()
    {
        bool Delete(string id);

        bool Delete(Expression<Func<T, bool>> express);

        T Insert(T entity);

        T InsertOrUpdate(T entity);

        List<T> Insert(List<T> list);

        System.Collections.Generic.List<T> SelectAll();

        T Update(string oldId, T entity);

        T Select(string id);

        System.Collections.Generic.List<T> SelectBy(string name, object value);

        bool DeleteAll();

    }
}
