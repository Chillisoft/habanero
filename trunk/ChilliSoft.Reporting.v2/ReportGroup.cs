using System.Collections;

namespace Chillisoft.Reporting.v2
{
    /// <summary>
    /// Summary description for ReportGroup.
    /// </summary>
    public class ReportGroup
    {
        IList itsRows;
        private string itsHeader;

        public ReportGroup()
        {
            itsRows = new ArrayList();
        }

        public IList Rows
        {
            get { return itsRows; }
        }

        public void Add(ReportRow row)
        {
            itsRows.Add(row);
        }

        public string Header
        {
            get { return itsHeader; }
            set { itsHeader = value; }
        }
    }
}