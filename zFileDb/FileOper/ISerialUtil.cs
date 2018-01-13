using System;
using System.Collections.Generic;

namespace wardensky.xmldb
{
    public interface ISerialUtil<T> where T : new()
    {
        void SaveFile(IList<T> data, String file,Object lockObject);

        IList<T> ReadFile(String file, Object lockObject);
    }
}
