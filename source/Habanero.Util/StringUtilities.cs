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

using System;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using Habanero.Base.Exceptions;

namespace Habanero.Util
{
    /// <summary>
    /// Provides a collection of utilities for strings
    /// </summary>
    public class StringUtilities
    {
        static readonly Regex _guidFormat = new Regex(
            "^[A-Fa-f0-9]{32}$|" + 
            "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
            "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");

        /// <summary>
        /// Replaces single quotes with two single quotes in the given string
        /// </summary>
        /// <param name="value">The string to amend</param>
        /// <returns>Returns the reformatted string</returns>
        public static string ReplaceSingleQuotesWithTwo(string value)
        {
            return value.Replace("'", "''");
        }

        /// <summary>
        /// Replaces double quotes with two double quotes in the given string
        /// </summary>
        /// <param name="value">The string to amend</param>
        /// <returns>Returns the reformatted string</returns>
        public static string ReplaceDoubleQuotesWithTwo(string value)
        {
            return value.Replace("\"", "\"\"");
        }

        /// <summary>
        /// Breaks up a Pascal-cased string into sections that are divided
        /// by the given delimiter.  For instance, an input string of
        /// "PascalCase" and a delimiter of " " will give "Pascal Case"
        /// </summary>
        /// <param name="inputString">The string to delimit</param>
        /// <param name="delimiter">The delimiter to insert between
        /// sections, such as a space or comma</param>
        /// <returns>Returns the delimited string</returns>
        public static string DelimitPascalCase(string inputString, string delimiter)
        {
            string formatted = "";
            int counter = 0;
            if (inputString == null) return null;
            foreach (char c in inputString)
            {
                string str = c.ToString();
                string prevStr = "";
                if (counter > 0)
                {
                    prevStr = inputString.Substring(counter - 1, 1);
                }
                int temp;

                // The rules are, add space if:
                //   - the letter is upper case
                //   - this is not the first letter
                //   - this is not a space
                //   - there is already a space before this
                //   - this is a number and there is no number just before
                if (str == str.ToUpper() && counter > 0 && str != " " && prevStr != " "
                    && !(Int32.TryParse(str, out temp) && Int32.TryParse(prevStr, out temp)))
                {
                    formatted += delimiter + str;
                }
                else
                {
                    formatted += str;
                }
                counter++;
            }
            return formatted;
        }

        /// <summary>
        /// Converts the string representation of a Guid to its Guid
        /// equivalent. A return value indicates whether the operation
        /// succeeded.
        /// </summary>
        /// <param name="s">A string containing a Guid to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the Guid value equivalent to
        /// the Guid contained in <paramref name="s"/>, if the conversion
        /// succeeded, or <see cref="Guid.Empty"/> if the conversion failed.
        /// The conversion fails if the <paramref name="s"/> parameter is a
        /// <see langword="null" /> reference (<see langword="Nothing" /> in
        /// Visual Basic), or is not of the correct format.
        /// </param>
        /// <value>
        /// <see langword="true" /> if <paramref name="s"/> was converted
        /// successfully; otherwise, <see langword="false" />.
        /// </value>
        /// <exception cref="ArgumentNullException">
        ///        Thrown if <pararef name="s"/> is <see langword="null"/>.
        /// </exception>
        public static bool GuidTryParse(string s, out Guid result)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            
            Match match = _guidFormat.Match(s);

            if (match.Success)
            {
                result = new Guid(s);
                return true;
            }
            result = Guid.Empty;
            return false;
        }

        ///<summary>
        /// Converts a string representing a boolean in one of many formats.
        ///</summary>
        ///<param name="valueToParse">string to be parsed</param>
        ///<param name="result">the resultant parsed value if parsing was a success</param>
        ///<returns>true if string was parsed false otherwise</returns>
        ///<exception cref="ArgumentNullException">
        ///   Thrown if <pararef name="s"/> is <see langword="null"/>.
        /// </exception>
        public static bool BoolTryParse(object valueToParse, out bool result)
        {
            try
            {
                if (valueToParse is string)
                {
                    string stringValue = valueToParse as string;
                    switch (stringValue.ToUpper())
                    {
                        case "TRUE":
                        case "T":
                        case "YES":
                        case "Y":
                        case "1":
                        case "-1":
                        case "ON":
                            result = true;
                            return true;
                        case "FALSE":
                        case "F":
                        case "NO":
                        case "N":
                        case "0":
                        case "OFF":
                            result = false;
                            return true;
                        default:
                            result = Convert.ToBoolean(stringValue);
                            return true;
                    }
                }
                result = Convert.ToBoolean(valueToParse);
            }
            catch (Exception ex)
            {
                throw new HabaneroDeveloperException("There was an error parsing " + valueToParse + " to a boolean. Please contact your system administrator", "There was an error parsing " + valueToParse + " to a boolean",ex  );
            }
            return true;
        }
        /// <summary>
        /// Indicates the number of times a given string appears in a larger string
        /// </summary>
        /// <param name="fullText">The string to search within</param>
        /// <param name="searchText">The section to search for</param>
        /// <returns>Returns the number of occurrences</returns>
        public static int CountOccurrences(string fullText, string searchText)
        {
            return CountOccurrences(fullText, searchText, 0, fullText.Length);
        }

        /// <summary>
        /// Indicates the number of times a given string appears in a larger string
        /// </summary>
        /// <param name="fullText">The string to search within</param>
        /// <param name="searchText">The section to search for</param>
        /// <param name="startIndex">The index of the position to start counting occurences from.</param>
        /// <param name="length">The length of text to count occurences from.</param>
        /// <returns>Returns the number of occurrences</returns>
        public static int CountOccurrences(string fullText, string searchText, int startIndex, int length)
        {
            string text = fullText.Substring(startIndex, length);
            string[] parts = text.Split(new string[] {searchText}, StringSplitOptions.None);
            return parts.Length - 1;
        }

        /// <summary>
        /// Indicates the number of times a given token appears in a string
        /// </summary>
        /// <param name="fullText">The string to search within</param>
        /// <param name="token">The token to search for</param>
        /// <returns>Returns the number of occurrences of the token</returns>
        public static int CountOccurrences(string fullText, char token)
        {
            return CountOccurrences(fullText, token, 0, fullText.Length);
        }

        /// <summary>
        /// Indicates the number of times a given token appears in a string
        /// </summary>
        /// <param name="fullText">The string to search within</param>
        /// <param name="token">The token to search for</param>
        /// <param name="startIndex">The index of the position to start counting occurences from.</param>
        /// <param name="length">The length of text to count occurences from.</param>
        /// <returns>Returns the number of occurrences of the token</returns>
        public static int CountOccurrences(string fullText, char token, int startIndex, int length)
        {
            int occurred = 0;
            for (int i = startIndex; i < length; i++)
            {
                if (token == fullText[i])
                {
                    occurred++;
                }
            }
            return occurred;
        }

        /// <summary>
        /// Returns the portion of the string that is left of the given
        /// search text
        /// </summary>
        /// <param name="fullText">The string to search within</param>
        /// <param name="searchText">The section to the right of the desired
        /// text</param>
        /// <returns>Returns the abbreviated string portion</returns>
        public static string GetLeftSection(string fullText, string searchText)
        {
            if (fullText.Contains(searchText))
            {
                return fullText.Substring(0, fullText.IndexOf(searchText));
            }
            return "";
        }

        /// <summary>
        /// Returns the portion of the string that is right of the given
        /// search text
        /// </summary>
        /// <param name="fullText">The string to search within</param>
        /// <param name="searchText">The section to the left of the desired
        /// text</param>
        /// <returns>Returns the abbreviated string portion</returns>
        public static string GetRightSection(string fullText, string searchText)
        {
            if (fullText.Contains(searchText))
            {
                int startPos = fullText.IndexOf(searchText) + searchText.Length;
                return fullText.Substring(startPos, fullText.Length - startPos);
            }
            return "";
        }

        /// <summary>
        /// Appends a given message to an existing message, inserting
        /// a new line (carriage return) between the messages
        /// </summary>
        /// <param name="origMessage">The existing message (left part)</param>
        /// <param name="messageToAppend">The message to add on (right part)</param>
        /// <returns>Returns the combined string</returns>
        public static string AppendMessage(string origMessage, string messageToAppend)
        {
            if (!String.IsNullOrEmpty(origMessage)) origMessage += Environment.NewLine;
            origMessage += messageToAppend;
            return origMessage;
        }

        /// <summary>
        /// Appends a given message to an existing message, using the given separator
        /// betweeen the two parts
        /// </summary>
        /// <param name="origMessage">The existing message (left part)</param>
        /// <param name="messageToAppend">The message to add on (right part)</param>
        /// <param name="separator">The separator to insert between the two
        /// parts</param>
        /// <returns>Returns the combined message</returns>
        public static string AppendMessage(string origMessage, string messageToAppend, string separator)
        {
            if (!String.IsNullOrEmpty(origMessage)) origMessage += separator;
            return origMessage + messageToAppend;
        }

        /// <summary>
        /// Appends a given message to an existing message contained in a string builder
        /// </summary>
        /// <param name="origStringBuilder">The string builder that contains the original
        /// message</param>
        /// <param name="appendedString">The message to add on (right part)</param>
        /// <param name="separator">The separator to insert between the two parts</param>
        /// <returns>Returns the combined message</returns>
        public static StringBuilder AppendMessage
            (StringBuilder origStringBuilder, string appendedString, string separator)
        {
            if (origStringBuilder.Length != 0)
                origStringBuilder.Append(separator);
            origStringBuilder.Append(appendedString);
            return origStringBuilder;
        }

        /// <summary>
        /// For a given name value pair e.g. a query string or cookie string that is formatted
        /// as name=value&name2=value2&name3=value3 etc this will return the value for a specified
        /// name e.g. for nameValuePairString = "name=value&name2=value2&name3=value3" and name = "name2"
        /// GetValueString will return value2.
        /// </summary>
        /// <param name="nameValuePairString">The name value pair to parse</param>
        /// <param name="name">The name of the name value pair for which you want the value</param>
        /// <returns></returns>
        public static string GetValueString(string nameValuePairString, string name)
        {
            NameValueCollection nameValueCollection = GetNameValueCollection(nameValuePairString);
            return nameValueCollection[name];
        }

        /// <summary>
        /// returns a NameValueCollection of nameValue Pairs for the nameValuePairString.
        /// e.g. nameValuePairString = "name=value&name2=value2&name3=value3" will return a 
        /// NameValueCollection with 3 items for name, name2 and name3.
        /// </summary>
        /// <param name="nameValuePairString">The name value pair to split.</param>
        /// <returns>The new collection containing the name value pair items.</returns>
        public static NameValueCollection GetNameValueCollection(string nameValuePairString)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            if (!string.IsNullOrEmpty(nameValuePairString))
            {
                string[] pairs = nameValuePairString.Split(new char[] {'&'});
                foreach (string pair in pairs)
                {
                    string[] values = pair.Split(new char[] {'='});
                    nameValueCollection[values[0]] = values[1];
                }
            }
            return nameValueCollection;
        }

        ///<summary>
        /// Returns the guid as a string with a standard format of "B" and with all the characters
        ///   changed to upper.
        ///</summary>
        ///<param name="guid"></param>
        ///<returns></returns>
        public static string GuidToUpper(Guid guid)
        {
            return guid.ToString("B").ToUpperInvariant();
        }
    }
}