
namespace wardensky.xmldb
{
    public class BaseXmlBll<T> : AbstractFileDbBll<T> where T : new()
    {
        public BaseXmlBll()
        {
            this.bll = XmlSerializerBll<T>.GetInstance();
        }

        public override string DbFile
        {
            get { return this.bll.Dbfile; }
            set { bll.Dbfile = value; }
        }

    }
}