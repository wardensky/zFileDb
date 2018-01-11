
namespace wardensky.xmldb
{
    public class FileBllFactory
    {
        public static IFileDbBll<T> GetXmlBll<T>() where T : new()
        {
            return new BaseXmlBll<T>();
        }

        public static IFileDbBll<T> GetBinaryBll<T>() where T : new()
        {
            return new BaseBinaryBll<T>();
        }
    }
}
