//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

using System.IO;
using Habanero.Base;

namespace Habanero.Util
{
    /// <summary>
    /// Provides a StreamReader to load a text file
    /// </summary>
    public class TextFileLoader : ITextFileLoader
    {
        /// <summary>
        /// Returns a StreamReader object to load the specified file name
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <returns>Returns a StreamReader object</returns>
        public TextReader LoadTextFile(string fileName)
        {
            return new StreamReader(fileName);
        }
    }
}