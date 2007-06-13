using System;
using System.IO;

namespace Chillisoft.Db.Utils.v2
{
    /// <summary>
    /// Processes a MySQL script
    /// </summary>
    public class MySqlScriptProcessor
    {
        private string _toFile;
        private string _fromFile;
        private bool _constraintsOnly;
        private StreamWriter _sw;
        private int _constraintCount;
        private int _tableCount;
        private string _toConstraint;
        private string _currentTableName;

        /// <summary>
        /// Constructor to initialise a processor
        /// </summary>
        /// <param name="toFile">The file to process to</param>
        /// <param name="fromFile">the file to process from</param>
        /// <param name="constraintsOnly">Whether to process constraints only</param>
        public MySqlScriptProcessor(string toFile, string fromFile, bool constraintsOnly)
        {
            _toFile = toFile;
            _fromFile = fromFile;
            _constraintsOnly = constraintsOnly;
        }

        /// <summary>
        /// Carries out a replication
        /// </summary>
        /// <param name="errMsg">A string error message to alter if there are
        /// any errors</param>
        /// <returns>Returns true if done successfully</returns>
        public bool Replicate(ref string errMsg)
        {
            if (!File.Exists(_fromFile))
            {
                errMsg = "File: " + _fromFile + " does not exist. Please reenter.";
                return false;
            }
            StreamReader sr = File.OpenText(_fromFile);
            string strFrom = sr.ReadLine();
            _sw = new StreamWriter(_toFile);

            bool inCreateTable = false;
            bool firstLineInCreateTable = false;
            while (strFrom != null)
            {
                int intIndexCreateTable = strFrom.IndexOf("CREATE TABLE");
                //'Check if line has a "Create Table" in it
                if (intIndexCreateTable != -1)
                {
                    int intFirstApostrophe = strFrom.IndexOf("`");
                    int intLastApostrophe = strFrom.IndexOf("`", intFirstApostrophe + 1);
                    _currentTableName =
                        strFrom.Substring(intFirstApostrophe + 1, intLastApostrophe - intFirstApostrophe - 1);
                    // 'As this is a new table, set constraint count to 0, so that only
                    // 'those tables which actually have constraints will be added to the second
                    // 'part of the document, when a constraint is added this count is incremented.
                    // 'When this count is incremented to 1, ie the first constraint is added for this
                    // 'table, then the alter table tableName will be added to the second part
                    // 'of the document.
                    _constraintCount = 0;
                    //'add string to table creation part of document.
                    ConcatStrToCreateTable(strFrom);
                    inCreateTable = true;
                    firstLineInCreateTable = true;
                }
                else
                {
                    if (!inCreateTable)
                    {
                        ConcatStrToCreateTable(strFrom);
                    }
                    else
                    {
                        //'Line adds a constraint.
                        if (strFrom.IndexOf("CONSTRAINT") != -1)
                        {
                            ConcatStrToConstraint(strFrom);
                            //'Line contains the type which is included at the end of the createtable or alter table.
                        }
                        else if (strFrom.IndexOf(") TYPE=") != -1)
                        {
                            //strCurrentTableType = strFrom;
                            ConcatStrToCreateTable("");
                            ConcatStrToCreateTable(strFrom);
                            inCreateTable = false;
                        }
                        else
                        {
                            //'line does not add a constraint or create a table.
                            if (inCreateTable)
                            {
                                if (strFrom.Length > 0)
                                {
                                    if (!firstLineInCreateTable)
                                    {
                                        ConcatStrToCreateTable(",");
                                    }
                                    if (strFrom.Substring(strFrom.Length - 1, 1) == ",")
                                    {
                                        ConcatStrToCreateTableNoNewLine(strFrom.Substring(0, strFrom.Length - 1));
                                    }
                                    else
                                    {
                                        ConcatStrToCreateTableNoNewLine(strFrom);
                                    }
                                }
                                if (firstLineInCreateTable)
                                {
                                    firstLineInCreateTable = false;
                                }
                            }
                            else
                            {
                                ConcatStrToCreateTable(strFrom);
                            }
                        }
                    }
                }
                strFrom = sr.ReadLine();
            }
            _sw.Write(_toConstraint);
            _sw.Close();
            sr.Close();
            //'SaveReplicatedScript()
            return true;
        }

        /// <summary>
        /// Concatenates a given string to the table, followed by a new line.
        /// Will not be written if constraints-only is set to true.
        /// </summary>
        /// <param name="nextLine">The string to concatenate</param>
        private void ConcatStrToCreateTable(string nextLine)
        {
            //Create 'Top part' of the document ie the part that creates and sets the data.
            //the 'Bottom part' is the section which adds all the constraints. 
            if (!_constraintsOnly)
            {
                _sw.WriteLine(nextLine);
            }
        }

        /// <summary>
        /// Concatenates a given string to the table, followed by a new line.
        /// Will not be written if constraints-only is set to true.
        /// </summary>
        /// <param name="substring">The string to concatenate</param>
        private void ConcatStrToCreateTableNoNewLine(string substring)
        {
            //'Create 'Top part' of the document ie the part that creates and sets the data.
            //'the 'Bottom part' is the section which adds all the constraints. 
            if (!_constraintsOnly)
            {
                _sw.Write(substring);
            }
        }

        /// <summary>
        /// Concatenates a given string to the constraint list
        /// </summary>
        /// <param name="nextLine">The constraint to add</param>
        private void ConcatStrToConstraint(string nextLine)
        {
            _constraintCount = _constraintCount + 1;
            if (_constraintCount == 1)
            {
                _tableCount = _tableCount + 1;
                if (_tableCount > 1)
                {
                    _toConstraint = _toConstraint + ";" + Environment.NewLine + Environment.NewLine;
                }
                _toConstraint = _toConstraint + "Alter Table `" + _currentTableName + "`" + Environment.NewLine;
                _toConstraint = _toConstraint + "ADD" + nextLine;
            }
            else
            {
                _toConstraint = _toConstraint + Environment.NewLine + "ADD" + nextLine;
            }
        }
    }
}