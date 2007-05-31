using System.Windows.Forms;
using Chillisoft.Reporting.ExcelRenderer.v2;
using Chillisoft.Reporting.v2;
using Chillisoft.UI.Misc.v2;

namespace Chillisoft.Reporting.Renderer.v2
{
    /// <summary>
    /// Summary description for ReportRenderer.
    /// </summary>
    public class ReportRenderer
    {
        public static void RenderReport(Report report)
        {
            InputBoxRadioButton inputBox = new InputBoxRadioButton();
            if (inputBox.ShowDialog(new string[] {"PDF", "Excel"}) == DialogResult.OK)
            {
                IReportRenderer renderer = null;
                switch (inputBox.SelectedOption)
                {
                    case "PDF":
                        renderer = new PDFRenderer.v2.PDFRenderer();
                        break;
                    case "Excel":
                        renderer = new ExcelReportRenderer();
                        break;
                }
                renderer.ShowReport(report);
            }
        }
    }
}