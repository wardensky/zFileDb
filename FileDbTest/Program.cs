using System;
using System.Collections.Generic;
using System.Diagnostics;
using wardensky.xmldb;

namespace FileDbTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SaveBinary();
            Console.WriteLine("完成");
        }


        static void SaveXml()
        {
            IFileDbBll<TestModel> bll = FileBllFactory<TestModel>.GetXmlBll(ENUM_SAVE_FILE_NUM.FIVE_FILES);
            bll.SelectAll();
            for (int i = 0; i < 30; i++)
            {
                TestModel model = TestModel.GetModel();
                bll.Insert(model);
            }
        }

        

        static void SaveBinary()
        {
            IFileDbBll<TestModel> bll = FileBllFactory<TestModel>.GetBinaryBll(ENUM_SAVE_FILE_NUM.HUNDRED_FILE);
            List<TestModel> list = new List<TestModel>(1000000);
            for (int i = 0; i < 100000; i++)
            {
                TestModel model = TestModel.GetModel();
                list.Add(model);
            }
            bll.Insert(list);
        }
    }
}
