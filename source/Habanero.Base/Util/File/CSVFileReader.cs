//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using System.Collections.Generic;
using System.IO;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.Util
{
    /// <summary>
    /// Provides a reader for a CSV (Comma-separated value) file
    /// </summary>
    public class CSVFileReader
    {
        TextReader _reader;
        string _currentLine;
        int _lineNo;

        /// <summary>
        /// Constructor to initialise a new reader
        /// </summary>
        /// <param name="fileName">The file name</param>
        public CSVFileReader(string fileName)
        {
            _lineNo = 0;
            _reader = new StreamReader(fileName);
        }

        /// <summary>
        /// Constructor to initialise a new reader
        /// </summary>
        /// <param name="textReader">The text reader to read the information from</param>
        public CSVFileReader(TextReader textReader)
        {
            _lineNo = 0;
            _reader = textReader;
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
        /// Loads all the values for the current line and returns them in a list
        /// </summary>
        /// <returns>Returns a list of values</returns>
        public List<string> GetValues()
        {
            HabaneroStringBuilder stringBuilder = new HabaneroStringBuilder(_currentLine.Replace(",\"\",", ",,"));
            stringBuilder.SetQuotes(new string[] { "\"" });
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
                return GetValues();
            }
            List<string> values = new List<string>();
            
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
            return values;
        }

        /// <summary>
        /// Loads the values and returns them in a list
        /// </summary>
        /// <param name="numValues">The minimum number of values to load. 
        /// If there are more values than this number then all the values are loaded, 
        /// otherwise the extra required values are made up with empty strings.</param>
        /// <returns>Returns a list of values</returns>
        public List<string> GetValues(int numValues)
        {
            List<string> values = GetValues();
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