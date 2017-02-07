#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
namespace Habanero.Base.Logging
{
    /// <summary>
    /// The Category of Log file that you want to log for.
    /// This is typically used by your <see cref="IHabaneroLogger"/> implementation
    /// to provide control for what is logged. Since logging is a time consuming procedure it is
    /// critical that the log is created only where required for released applicatione etc
    /// </summary>
    public enum LogCategory
    {
        /// <summary>
        /// Are you logging information specifically to be used when debugging an application
        /// </summary>
        Debug, 
        /// <summary>
        /// Are you logging an exception i.e. an Exception has been thrown and you are logging it.
        /// </summary>
        Exception, 
        /// <summary>
        /// Are you logging general info e.g. Resolved ...
        /// Bootstrapped application ...
        /// Starting ....
        /// </summary>
        Info, 
        /// <summary>
        /// Has an error occured whereby you do not want to specifically throw an exception and prevent the 
        /// user from continuing but you do want it logged so that your support team can look into it.
        /// </summary>
        Warn,
        /// <summary>
        /// There is a Fatal Exception that has occured. 
        ///   You want to log this. Probably want to log and close the app down or something like that.
        /// </summary>
        Fatal,
        /// <summary>
        /// There is an Error Exception that has occured.
        /// </summary>
        Error
    }
}