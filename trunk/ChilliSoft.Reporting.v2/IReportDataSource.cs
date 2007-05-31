using System.Data;

namespace Chillisoft.Reporting.v2
{
    /// <summary>
    /// Summary description for IReportDataSource.
    /// </summary>
    public interface IReportDataSource
    {
        DataTable GetDataTable();
    }
}