using System.Data;
using System.IO;

namespace Chillisoft.Util.v2
{
    /// <summary>
    /// Provides a writer for a CSV (Comma-separated value) file
    /// </summary>
    public class CSVFileWriter
    {
        private StreamWriter _writer;

        /// <summary>
        /// Constructor to initialise the writer
        /// </summary>
        /// <param name="strFileName">The file name</param>
        public CSVFileWriter(string strFileName)
        {
            _writer = new StreamWriter(strFileName, false);
        }

        /// <summary>
        /// Writes to the file from the data table provided
        /// </summary>
        /// <param name="table">The data table</param>
        public void WriteFromDataTable(DataTable table)
        {
            bool firstCol = true;
            foreach (DataColumn column in table.Columns)
            {
                if (!firstCol)
                {
                    _writer.Write(",");
                }
                _writer.Write(column.Caption);
                firstCol = false;
            }

            _writer.WriteLine();

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (i > 0)
                    {
                        _writer.Write(",");
                    }
                    string val = Chillisoft.Util.v2.StringUtilities.ReplaceSingleQuotesWithTwo(row[i].ToString());
                    if (val.IndexOf(",") != -1)
                    {
                        val = "\"" + val + "\"";
                    }
                    _writer.Write(val);
                }
                _writer.WriteLine();
            }
        }

        /// <summary>
        /// Closes the writer
        /// </summary>
        public void Close()
        {
            _writer.Close();
        }
    }
}