using System;
using System.IO;
using System.Reflection;

namespace wardensky.xmldb
{
    public sealed class CommonUtil
    {
        public static int GetKey<T>(T entity, ENUM_SAVE_FILE_NUM num) where T : new()
        {
            return entity.GetHashCode() % (int)num;
        }

        public static string GetFileName(int index, ENUM_SAVE_FILE_TYPE fileType, Type type)
        {
            string suffix = fileType == ENUM_SAVE_FILE_TYPE.XML ? ".xml" : ".dat";
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            if (Directory.Exists(folder) == false)
            {
                Directory.CreateDirectory(folder);
            }
            return Path.Combine(folder, type.Name + "_" + index + suffix);
        }


        public static String SetIdGuid<T>(T t) where T : new()
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

        public static String GetIdValue<T>(T t) where T : new()
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

    }
}
