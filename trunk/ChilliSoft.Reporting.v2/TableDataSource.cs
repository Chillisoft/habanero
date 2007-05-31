using System.Data;

namespace Chillisoft.Reporting.v2
{
    /// <summary>
    /// Summary description for TableDataSource.
    /// </summary>
    public class TableDataSource : IReportDataSource
    {
        private readonly DataTable itsTable;
        private readonly string itsGroupByColumn;

        public TableDataSource(DataTable table) : this(table, "")
        {
        }

        public TableDataSource(DataTable table, string groupByColumn)
        {
            itsTable = table;
            itsGroupByColumn = groupByColumn;
        }

        private DataTable DataTable
        {
            get { return itsTable; }
        }

        public DataTable GetDataTable()
        {
            return DataTable;
        }
    }
}