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
namespace Habanero.BO
{
    /// <summary>
    /// A factory to create objects
    /// </summary>
    public class ObjectFactory
    {
        private static ObjectFactory _objectFactory;

        /// <summary>
        /// Constructor to initialise a new factory
        /// </summary>
        private ObjectFactory()
        {
        }

        /// <summary>
        /// Returns the object factory stored in this instance
        /// </summary>
        /// <returns>Returns an ObjectFactory object</returns>
        public static ObjectFactory GetObjectFactory()
        {
            if (_objectFactory == null)
            {
                _objectFactory = new ObjectFactory();
            }
            return _objectFactory;
        }
    }
}