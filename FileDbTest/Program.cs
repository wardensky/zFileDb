using System;
using System.Collections.Generic;
using wardensky.xmldb;

namespace FileDbTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SaveBinary();

            IFileDbBll<TestModel> bll = FileBllFactory.GetBinaryBll<TestModel>();
            List<TestModel> data = bll.SelectAll();
            Console.WriteLine(data.Count);
            //      Console.Read();
        }


        static void SaveXml()
        {
            IFileDbBll<TestModel> bll = FileBllFactory.GetXmlBll<TestModel>();
            bll.DeleteAll();
            TestModel model = GetModel();
            bll.Insert(model);
        }

        static TestModel GetModel()
        {
            TestModel model = new TestModel();
            model.Age = 10;
            model.Id = Guid.NewGuid().ToString();
            model.Name = "keke";
            return model;
        }

        static void SaveBinary()
        {
            IFileDbBll<TestModel> bll = FileBllFactory.GetBinaryBll<TestModel>();
            TestModel model = GetModel();
            bll.Insert(model);
        }
    }
}
