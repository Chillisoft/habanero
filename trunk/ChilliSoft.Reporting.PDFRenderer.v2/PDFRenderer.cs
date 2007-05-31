using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using Chillisoft.Reporting.v2;
using com.lowagie.text;
using com.lowagie.text.pdf;

namespace Chillisoft.Reporting.PDFRenderer.v2
{
    /// <summary>
    /// Summary description for PDFRenderer.
    /// </summary>
    public class PDFRenderer : IReportRenderer
    {
        private Report itsReport;


        public PDFRenderer()
        {
        }

        public void ShowReport(Report report)
        {
            itsReport = report;

            Rectangle pageDimension;

            if (itsReport.ReportDef.Orientation == Orientation.Landscape)
            {
                pageDimension = PageSize.A4.rotate();
            }
            else
            {
                pageDimension = PageSize.A4;
            }
            Layout defaultLayout = itsReport.ReportDef.DefaultLayout;
            Document document =
                new Document(pageDimension, defaultLayout.MarginLeft, defaultLayout.MarginRight, defaultLayout.MarginTop,
                             defaultLayout.MarginBottom);
            string fileName = report.ReportDef.Name + DateTime.Now.ToFileTime() + ".pdf";
            PdfWriter writer = PdfWriter.getInstance(document, new FileStream(fileName, FileMode.Create));
            writer.setPageEvent(new FooterHelper());
            document.open();

            IList relativeColumnWidthsList = new ArrayList();


            foreach (Column column in itsReport.ReportDef.Columns)
            {
                if (!column.GroupBy)
                    relativeColumnWidthsList.Add(column.Width);
            }
            int[] relativeColumnWidths = new int[relativeColumnWidthsList.Count];
            for (int i = 0; i < relativeColumnWidthsList.Count; i++)
            {
                relativeColumnWidths[i] = (int) relativeColumnWidthsList[i];
            }

            document.add(
                new Phrase(report.ReportDef.Caption, FontCreator.CreateFontForLayout(itsReport.ReportDef.HeaderLayout)));

            bool firstGroup = true;
            foreach (ReportGroup reportGroup in itsReport.ReportGroups)
            {
                if (!firstGroup)
                {
                    document.add(new Phrase("\n"));
                }
                PdfPTable table = new PdfPTable(relativeColumnWidths.Length);

                Font cellFont = FontCreator.CreateFontForLayout(report.ReportDef.DetailLayout);
                Font columnHeaderFont = FontCreator.CreateFontForLayout(report.ReportDef.ColumnHeaderLayout);

                table.getDefaultCell().setPadding(3);
                table.setWidths(relativeColumnWidths);
                table.setWidthPercentage(100);

                int headerRows = 0;
                if (reportGroup.Header != "")
                {
                    PdfPCell groupHeaderCell =
                        new PdfPCell(
                            new Phrase(reportGroup.Header,
                                       FontCreator.CreateFontForLayout(report.ReportDef.GroupHeaderLayout)));
                    groupHeaderCell.setColspan(relativeColumnWidths.Length);
                    groupHeaderCell.setBackgroundColor(report.ReportDef.GroupHeaderLayout.BackColor);
                    groupHeaderCell.setBorder(report.ReportDef.GroupHeaderLayout.Border ? 1 : 0);
                    table.addCell(groupHeaderCell);
                    headerRows++;
                }

                foreach (Column column in itsReport.ReportDef.Columns)
                {
                    if (!column.GroupBy)
                    {
                        PdfPCell columnHeaderCell = new PdfPCell(new Phrase(column.Caption, columnHeaderFont));
                        columnHeaderCell.setBackgroundColor(report.ReportDef.ColumnHeaderLayout.BackColor);
                        columnHeaderCell.setBorder(report.ReportDef.ColumnHeaderLayout.Border
                                                       ? Rectangle.BOX
                                                       : Rectangle.NO_BORDER);
                        columnHeaderCell.setHorizontalAlignment(ElementConst.ALIGN_CENTER);
                        table.addCell(columnHeaderCell);
                    }
                }
                headerRows++;

                table.setHeaderRows(headerRows);

                int rowCount = 0;
                foreach (ReportRow reportRow in reportGroup.Rows)
                {
                    for (int i = 0; i < reportRow.Count; i++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(reportRow[i].ToString(), cellFont));
                        if (rowCount%2 == 1)
                            cell.setGrayFill(0.9f);
                        cell.setBorder(report.ReportDef.DetailLayout.Border ? 1 : 0);
                        cell.setHorizontalAlignment(ElementConst.ALIGN_LEFT);
                        table.addCell(cell);
                    }
                    rowCount++;
                }
                document.add(table);
            }
            document.close();
            Process.Start(fileName);
        }
    }
}