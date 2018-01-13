using System;

namespace FileDbTest
{
    [Serializable]
    public class TestModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public static TestModel GetModel()
        {
            TestModel model = new TestModel();
            model.Age = 10;
            model.Id = Guid.NewGuid().ToString();
            model.Name = "keke";
            return model;
        }

    }
}
