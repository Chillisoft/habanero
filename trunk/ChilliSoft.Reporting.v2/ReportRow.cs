using System.Collections;

namespace Chillisoft.Reporting.v2
{
    /// <summary>
    /// Summary description for ReportRow.
    /// </summary>
    public class ReportRow
    {
        IList _contents;

        public ReportRow()
        {
            _contents = new ArrayList();
        }

        public void Add(object o)
        {
            _contents.Add(o);
        }

        public object this[int index]
        {
            get { return _contents[index]; }
        }

        public int Count
        {
            get { return _contents.Count; }
        }
    }
}