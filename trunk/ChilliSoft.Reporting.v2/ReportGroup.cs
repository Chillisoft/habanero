using System.Collections;

namespace Chillisoft.Reporting.v2
{
    /// <summary>
    /// Summary description for ReportGroup.
    /// </summary>
    public class ReportGroup
    {
        IList _rows;
        private string _header;

        public ReportGroup()
        {
            _rows = new ArrayList();
        }

        public IList Rows
        {
            get { return _rows; }
        }

        public void Add(ReportRow row)
        {
            _rows.Add(row);
        }

        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
    }
}