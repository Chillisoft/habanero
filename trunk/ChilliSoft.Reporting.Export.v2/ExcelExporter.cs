using System;
using System.Data;
using Excel;
using DataTable=System.Data.DataTable;

namespace Chillisoft.Reporting.Export.v2
{
    /// <summary>
    /// Exports reports to the Microsoft Excel format
    /// </summary>
    public class ExcelExporter : ReportExporter
    {
        private Worksheet itsWorksheet;
        private Application itsExcelApp;
        private int itsCurrentRow;
        private int itsCurrentCol;

        /// <summary>
        /// Constructor to initialise a new exporter
        /// </summary>
        public ExcelExporter()
        {
            itsExcelApp = new Application();
            Workbook workbook = itsExcelApp.Workbooks.Add(Type.Missing);
            itsWorksheet = (Worksheet) workbook.ActiveSheet;
            itsCurrentRow = 1;
            itsCurrentCol = 1;
        }

        /// <summary>
        /// Adds a data table
        /// </summary>
        /// <param name="table">The data table to add</param>
        public void AddDataTable(DataTable table)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                itsWorksheet.Cells[itsCurrentRow, itsCurrentCol + i] = table.Columns[i].ColumnName;
            }

            itsCurrentRow++;

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    itsWorksheet.Cells[itsCurrentRow, itsCurrentCol + i] = row[i];
                }
                itsCurrentRow++;
            }
        }

        /// <summary>
        /// Shows the report
        /// </summary>
        public void ShowReport()
        {
            itsExcelApp.Visible = true;
        }

        /// <summary>
        /// Adds a blank row
        /// </summary>
        public void AddBlankRow()
        {
            itsCurrentRow++;
            itsCurrentRow++;
        }

        /// <summary>
        /// Specifies that the columns automatically fit to their contents
        /// </summary>
        public void AutoFitColumns()
        {
            ((Excel.Range) itsWorksheet.Columns[1, Type.Missing]).AutoFit();
        }

        /// <summary>
        /// Formats the given column to that specified in the Format enumeration
        /// </summary>
        /// <param name="columnNumber">The column number to format</param>
        /// <param name="format">The specified Format</param>
        public void FormatColumn(int columnNumber, Format format)
        {
            FormatRange((Range) itsWorksheet.Columns[columnNumber, Type.Missing], format);
        }

        /// <summary>
        /// Formats a range of cells to that specified in the Format enumeration
        /// </summary>
        /// <param name="startRow">The starting row number</param>
        /// <param name="startCol">The starting column number</param>
        /// <param name="endRow">The ending row number</param>
        /// <param name="endCol">The ending column number</param>
        /// <param name="format">The specified Format</param>
        public void FormatRange(int startRow, int startCol, int endRow, int endCol, Format format)
        {
            for (int i = startRow; i <= endRow; i++)
            {
                for (int j = startCol; j <= endCol; j++)
                {
                    FormatRange((Range) itsWorksheet.Cells[i, j], format);
                }
            }
        }

        /// <summary>
        /// Formats a range of cells to that specified in the Format enumeration
        /// </summary>
        /// <param name="cell">The range of cells</param>
        /// <param name="format">The specified Format</param>
        private void FormatRange(Range cell, Format format)
        {
            if (format == Format.currency)
            {
                cell.NumberFormat = "R #,##0.00;R -#,##0.00";
            }
            else if (format == Format.percentage)
            {
                cell.NumberFormat = "0.00%";
            }
        }
    }
}