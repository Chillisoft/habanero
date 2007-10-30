//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Text.RegularExpressions;

namespace Habanero.Util
{
	/// <summary>
	/// Provides a collection of utilities for strings
	/// </summary>
	public class StringUtilities
	{
		/// <summary>
		/// Replaces single quotes with double quotes in the given string
		/// </summary>
		/// <param name="value">The string to amend</param>
		/// <returns>Returns the reformatted string</returns>
		public static string ReplaceSingleQuotesWithTwo(string value)
		{
			return value.Replace("'", "''");
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
				if (str == str.ToUpper() && counter > 0
					&& str != " "
					&& inputString.Substring(counter - 1, 1) != " ")
				{
					formatted += delimiter + str;
				} else
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

			Regex format = new Regex(
				"^[A-Fa-f0-9]{32}$|" +
				"^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
				"^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
			Match match = format.Match(s);

			if (match.Success)
			{
				result = new Guid(s);
				return true;
			} else
			{
				result = Guid.Empty;
				return false;
			}
		}
	}
}