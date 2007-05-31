using System.Data;

namespace Chillisoft.Reporting.Export.v2
{
    /// <summary>
    /// An interface to model a report exporter
    /// </summary>
    public interface ReportExporter
    {
        /// <summary>
        /// Adds a data table
        /// </summary>
        /// <param name="table">The data table to add</param>
        void AddDataTable(DataTable table);

        /// <summary>
        /// Shows the report
        /// </summary>
        void ShowReport();

        /// <summary>
        /// Adds a blank row
        /// </summary>
        void AddBlankRow();

        /// <summary>
        /// Specifies that the columns automatically fit to their contents
        /// </summary>
        void AutoFitColumns();

        /// <summary>
        /// Formats the given column number using information in the Format
        /// object provided
        /// </summary>
        /// <param name="columnNumber">The column number to format</param>
        /// <param name="format">The Format object</param>
        void FormatColumn(int columnNumber, Format format);

        /// <summary>
        /// Formats a range of cells using information in the Format
        /// object provided
        /// </summary>
        /// <param name="startRow">The starting row number</param>
        /// <param name="startCol">The starting column number</param>
        /// <param name="endRow">The ending row number</param>
        /// <param name="endCol">The ending column number</param>
        /// <param name="format">The Format object</param>
        void FormatRange(int startRow, int startCol, int endRow, int endCol, Format format);
    }
}