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
        StreamReader itsReader;
        string itsCurrentLine;
        int itsLineNo;

        /// <summary>
        /// Constructor to initialise a new reader
        /// </summary>
        /// <param name="strFileName">The file name</param>
        public CSVFileReader(string strFileName)
        {
            itsLineNo = 0;
            itsReader = new StreamReader(strFileName);
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
                itsCurrentLine = itsReader.ReadLine();
                if (itsCurrentLine == null) itsCurrentLine = "";
                itsLineNo++;
            } while (itsCurrentLine.Trim() == "" && itsReader.Peek() != -1);

            return (itsCurrentLine.Trim().Length != 0);
            //return (itsReader.Peek() != -1) ;
        }

        /// <summary>
        /// Loads the values and returns them in a list
        /// </summary>
        /// <param name="numValues">The number of values to load</param>
        /// <returns>Returns a list of values</returns>
        public IList GetValues(int numValues)
        {
            CoreStringBuilder stringBuilder = new CoreStringBuilder(itsCurrentLine.Replace(",\"\",", ",,"));
            stringBuilder.SetQuotes(new string[] {"\""});
            stringBuilder.RemoveQuotedSections();
            if (stringBuilder.IndexOf("\"") > -1)
            {
                string nextLine = itsReader.ReadLine();
                if (nextLine == null)
                {
                    nextLine = "";
                    throw new UserException("Unclosed quote in CSV file, line " + itsLineNo);
                }
                itsCurrentLine = itsCurrentLine + nextLine;

                itsLineNo++;
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
            itsReader.Close();
        }
    }
}