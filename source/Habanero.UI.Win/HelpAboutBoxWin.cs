// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a form that displays information about the application
    /// </summary>
    public class HelpAboutBoxWin : FormWin
    {
        /// <summary>
        /// Constructor to initialise a new About form with the given
        /// information
        /// </summary>
        /// <param name="programName">The program name</param>
        /// <param name="producedForName">Who the program is produced for</param>
        /// <param name="producedByName">Who produced the program</param>
        /// <param name="versionNumber">The version number</param>
        public HelpAboutBoxWin(string programName, string producedForName, string producedByName, string versionNumber)
        {
            new HelpAboutBoxManager(new ControlFactoryWin(), this, programName,producedForName,producedByName,versionNumber);

        }
    }
}
