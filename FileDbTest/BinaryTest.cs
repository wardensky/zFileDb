using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wardensky.xmldb;

namespace FileDbTest
{
    class BinaryTest
    {
        static TestModel GetModel()
        {
            TestModel model = new TestModel();
            model.Age = 10;
            model.Id = Guid.NewGuid().ToString();
            model.Name = "keke";
            return model;
        }
        public static void SaveSingle() { }

        public static void SaveMulti(int count)
        {
            IFileDbBll<TestModel> bll = FileBllFactory<TestModel>.GetXmlBll();
            List<TestModel> list = new List<TestModel>(1000000);
            for (int i = 0; i < count; i++)
            {
                // bll.Insert(GetModel());
                list.Add(GetModel());
            }
            bll.Insert(list);
        }

        public static void SaveMulti1(int count)
        {
           
            //bll.Insert(list);
        }
    }
}
