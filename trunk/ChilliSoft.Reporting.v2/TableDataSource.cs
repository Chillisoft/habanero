using System.Data;

namespace Chillisoft.Reporting.v2
{
    /// <summary>
    /// Summary description for TableDataSource.
    /// </summary>
    public class TableDataSource : IReportDataSource
    {
        private readonly DataTable _table;
        private readonly string _groupByColumn;

        public TableDataSource(DataTable table) : this(table, "")
        {
        }

        public TableDataSource(DataTable table, string groupByColumn)
        {
            _table = table;
            _groupByColumn = groupByColumn;
        }

        private DataTable DataTable
        {
            get { return _table; }
        }

        public DataTable GetDataTable()
        {
            return DataTable;
        }
    }
}