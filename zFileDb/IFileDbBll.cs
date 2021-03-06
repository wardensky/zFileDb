﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace wardensky.xmldb
{
    public interface IFileDbBll<T> where T : new()
    {
        bool Delete(string id);
        bool Delete(T t);

        bool Delete(Expression<Func<T, bool>> express);

        T Insert(T entity);

        T InsertOrUpdate(T entity);

        IList<T> Insert(IList<T> list);

        IList<T> SelectAll();

        T Update(string oldId, T entity);

        T Select(string id);

        IList<T> Select(Expression<Func<T, bool>> express);

        IList<T> SelectBy(string name, object value);

        bool DeleteAll();

    }
}
