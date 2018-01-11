
namespace wardensky.xmldb
{
    public class BaseBinaryBll<T> : AbstractFileDbBll<T> where T : new()
    {
        public BaseBinaryBll()
        {
            this.bll = BinarySerializerBll<T>.GetInstance();
        }

        public override string DbFile
        {
            get { return this.bll.Dbfile; }
            set { bll.Dbfile = value; }
        }

    }
}