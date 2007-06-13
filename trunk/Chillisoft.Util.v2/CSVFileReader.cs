using System.Collections;
using System.IO;
using Chillisoft.Generic.v2;

namespace Chillisoft.Util.v2
{
    /// <summary>
    /// Provides a reader for a CSV (Comma-separated value) file
    /// </summary>
    public class CSVFileReader
    {
        StreamReader _reader;
        string _currentLine;
        int _lineNo;

        /// <summary>
        /// Constructor to initialise a new reader
        /// </summary>
        /// <param name="strFileName">The file name</param>
        public CSVFileReader(string strFileName)
        {
            _lineNo = 0;
            _reader = new StreamReader(strFileName);
        }

        /// <summary>
        /// Moves the reader to the next line
        /// </summary>
        /// <returns>Returns true if done successfully, false if at the
        /// end of the file</returns>
        public bool MoveToNextLine()
        {
            do
            {
                _currentLine = _reader.ReadLine();
                if (_currentLine == null) _currentLine = "";
                _lineNo++;
            } while (_currentLine.Trim() == "" && _reader.Peek() != -1);

            return (_currentLine.Trim().Length != 0);
            //return (_reader.Peek() != -1) ;
        }

        /// <summary>
        /// Loads the values and returns them in a list
        /// </summary>
        /// <param name="numValues">The number of values to load</param>
        /// <returns>Returns a list of values</returns>
        public IList GetValues(int numValues)
        {
            CoreStringBuilder stringBuilder = new CoreStringBuilder(_currentLine.Replace(",\"\",", ",,"));
            stringBuilder.SetQuotes(new string[] {"\""});
            stringBuilder.RemoveQuotedSections();
            if (stringBuilder.IndexOf("\"") > -1)
            {
                string nextLine = _reader.ReadLine();
                if (nextLine == null)
                {
                    nextLine = "";
                    throw new UserException("Unclosed quote in CSV file, line " + _lineNo);
                }
                _currentLine = _currentLine + nextLine;

                _lineNo++;
                return GetValues(numValues);
            }
            IList values = new ArrayList();


            int commaPos = 0;
            int pos = 0;
            int endPos = 0;
            do
            {
                commaPos = stringBuilder.IndexOf(",", pos);
                if (commaPos == -1)
                {
                    endPos = stringBuilder.ToString().Length;
                }
                else
                {
                    endPos = commaPos;
                }
                string value = stringBuilder.Substring(pos, endPos - pos).PutBackQuotedSections().ToString().Trim();
                if ((value.StartsWith("\"") && value.EndsWith("\"")) || (value.StartsWith("'") && value.EndsWith("'")))
                {
                    value = value.Substring(1, value.Length - 2);
                }
                values.Add(value);
                pos = commaPos + 1;
            } while (commaPos != -1);

            for (int i = values.Count; i < numValues; i++)
                values.Add("");

            return values;
        }

        /// <summary>
        /// Closes the reader
        /// </summary>
        public void Close()
        {
            _reader.Close();
        }
    }
}