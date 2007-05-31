using System.Collections;

namespace Chillisoft.Reporting.v2
{
    /// <summary>
    /// Summary description for ReportRow.
    /// </summary>
    public class ReportRow
    {
        IList itsContents;

        public ReportRow()
        {
            itsContents = new ArrayList();
        }

        public void Add(object o)
        {
            itsContents.Add(o);
        }

        public object this[int index]
        {
            get { return itsContents[index]; }
        }

        public int Count
        {
            get { return itsContents.Count; }
        }
    }
}