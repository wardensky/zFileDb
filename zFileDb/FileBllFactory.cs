
using Unity;
using Unity.Injection;
using Unity.RegistrationByConvention;
namespace wardensky.xmldb
{
    public class FileBllFactory<T> where T : new()
    {
        private static IUnityContainer container = new UnityContainer();
        private const string XML = "xml";
        private const string BINARY = "binary";
        static FileBllFactory()
        {
            container.RegisterType<ISerialUtil<T>, BinarySerialUtil<T>>(BINARY).RegisterType<ISerialUtil<T>, XmlSerialUtil<T>>(XML);
        }
        public static IFileDbBll<T> GetXmlBll()
        {
            container.RegisterType<MultiFilesDbBll<T>>("xmlbll", new InjectionProperty("SerialUtil", new ResolvedParameter<ISerialUtil<T>>(XML))
                , new InjectionConstructor(ENUM_SAVE_FILE_NUM.SINGLE_FILE, ENUM_SAVE_FILE_TYPE.XML));
            return container.Resolve<MultiFilesDbBll<T>>("xmlbll");
        }

        public static IFileDbBll<T> GetBinaryBll()
        {
            container.RegisterType<MultiFilesDbBll<T>>("binbll",new InjectionProperty("SerialUtil", new ResolvedParameter<ISerialUtil<T>>(BINARY))
                , new InjectionConstructor(ENUM_SAVE_FILE_NUM.SINGLE_FILE, ENUM_SAVE_FILE_TYPE.BINARY));
            return container.Resolve<MultiFilesDbBll<T>>("binbll");
        }

        public static IFileDbBll<T> GetXmlBll(ENUM_SAVE_FILE_NUM num)
        {
            container.RegisterType<MultiFilesDbBll<T>>("xmlbll", new InjectionProperty("SerialUtil", new ResolvedParameter<ISerialUtil<T>>(XML))
                , new InjectionConstructor(num, ENUM_SAVE_FILE_TYPE.XML));
            return container.Resolve<MultiFilesDbBll<T>>("xmlbll");
        }

        public static IFileDbBll<T> GetBinaryBll(ENUM_SAVE_FILE_NUM num)
        {
            container.RegisterType<MultiFilesDbBll<T>>("binbll", new InjectionProperty("SerialUtil", new ResolvedParameter<ISerialUtil<T>>(BINARY))
                , new InjectionConstructor(num, ENUM_SAVE_FILE_TYPE.BINARY));
            return container.Resolve<MultiFilesDbBll<T>>("binbll");
        }
    }
}
