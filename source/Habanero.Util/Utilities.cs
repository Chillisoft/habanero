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
using Habanero.Base.Exceptions;

namespace Habanero.Util
{
    ///<summary>
    /// General Utilities
    ///</summary>
    public static class Utilities
    {
        ///<summary>
        /// This method tests the reference passed in to see if it is null or not.
        /// It inspects the actual memory location of the object's pointer to see if it is null or not.
        /// This is useful in the case where you need to test for null without using the == operator.
        ///</summary>
        ///<param name="obj">The object to be tested it it is null or not.</param>
        ///<returns>True if the object is null, or false if not.</returns>
        public static bool IsNull(object obj)
        {
            WeakReference testNull = new WeakReference(obj);
            return !testNull.IsAlive;
        }


        public static void CheckTypeCanBeCreated(Type type)
        {
            //Check that the type can be created and raise appropriate error 
            try
            {
                Activator.CreateInstance(type, true);
            }
            catch (Exception ex)
            {
                throw new UnknownTypeNameException
                    (String.Format
                         ("An error occurred while attempting to load a related "
                          + "business object collection, with the type given as '{0}'. "
                          + "Check that the given type exists and has been correctly "
                          + "defined in the relationship and class definitions for the classes " + "involved.", type),
                     ex);
            }
        }
    }
}
