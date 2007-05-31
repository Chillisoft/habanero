using Chillisoft.Reporting.Export.v2;
using Chillisoft.Reporting.v2;

namespace Chillisoft.Reporting.ExcelRenderer.v2
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class ExcelReportRenderer : IReportRenderer
    {
        public ExcelReportRenderer()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void ShowReport(Report report)
        {
            ExcelExporter exporter = new ExcelExporter();
            exporter.AddDataTable(report.DataTable);
            exporter.ShowReport();
        }
    }
}