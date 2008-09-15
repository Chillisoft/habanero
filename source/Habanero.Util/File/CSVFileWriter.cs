//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Data;
using System.IO;

namespace Habanero.Util
{
    /// <summary>
    /// Provides a writer for a CSV (Comma-separated value) file
    /// </summary>
    public class CSVFileWriter
    {
        private TextWriter _writer;

        /// <summary>
        /// Constructor to initialise the writer
        /// </summary>
        /// <param name="fileName">The file name</param>
        public CSVFileWriter(string fileName)
        {
            _writer = new StreamWriter(fileName, false);
        }

        /// <summary>
        /// Constructor to initialise the writer
        /// </summary>
        /// <param name="textWriter">The text writer to use</param>
        public CSVFileWriter(TextWriter textWriter)
        {
            _writer = textWriter;
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
                _writer.Write(PrepareForCsv(column.Caption));
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
                    string val = row[i].ToString();
                    val = PrepareForCsv(val);
                    _writer.Write(val);
                }
                _writer.WriteLine();
            }
        }

        private static string PrepareForCsv(string val)
        {
            //val = StringUtilities.ReplaceSingleQuotesWithTwo(val);
            val = StringUtilities.ReplaceDoubleQuotesWithTwo(val);
            if (val.IndexOf(",") != -1 || val.IndexOf("\"") != -1)
            {
                val = "\"" + val + "\"";
            }
            return val;
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