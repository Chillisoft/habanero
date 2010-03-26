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
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Creates business objects.  The default creator is used by facilities
    /// like ReadOnlyGridControl to create new business objects.  Inherit
    /// from this class if you need to carry out additional steps at the time
    /// of creating a new business object.
    /// </summary>
    public class DefaultBOCreator : IBusinessObjectCreator
    {
        private readonly IBusinessObjectCollection _boCol;
        private readonly ClassDef _classDef;

        /// <summary>
        /// Constructor to initialise a new object creator
        /// </summary> 
        /// <param name="classDef">The class definition</param>
        public DefaultBOCreator(ClassDef classDef)
        {
            _classDef = classDef;
        }

        /// <summary>
        /// Constructor to initialise a new object creator
        /// </summary> 
        /// <param name="boCol">The collection this BO will be created as part of 
        /// (it will be added to the collection proper when it is saved)</param>
        public DefaultBOCreator(IBusinessObjectCollection boCol)
        {
            _boCol = boCol;
        }

        /// <summary>
        /// Creates the object, without editing or saving it.
        /// </summary>
        /// <returns></returns>
        public IBusinessObject CreateBusinessObject()
        {
            return _boCol != null ? _boCol.CreateBusinessObject() : _classDef.CreateNewBusinessObject();
        }
    }
}