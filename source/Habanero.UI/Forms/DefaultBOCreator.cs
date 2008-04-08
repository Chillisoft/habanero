//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.Base;
using Habanero.UI.Base;
using BusinessObject=Habanero.BO.BusinessObject;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Creates business objects.  The default creator is used by facilities
    /// like ReadOnlyGridWithButtons to create new business objects.  Inherit
    /// from this class if you need to carry out additional steps at the time
    /// of creating a new business object.
    /// </summary>
    public class DefaultBOCreator : IObjectCreator
    {
        private readonly ClassDef _classDef;

        /// <summary>
        /// Constructor to initialise a new object creator
        /// </summary> 
        /// 
        /// <param name="classDef">The class definition</param>
        public DefaultBOCreator(ClassDef classDef)
        {
            _classDef = classDef;
        }

        /// <summary>
        /// Creates a business object
        /// </summary>
        /// <param name="editor">An object editor</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returns the business object created</returns>
        public Object CreateObject(IObjectEditor editor, string uiDefName)
        {
            return this.CreateObject(editor, null, uiDefName);
        }

        /// <summary>
        /// Creates a business object
        /// </summary>
        /// <param name="editor">An object editor</param>
        /// <param name="initialiser">An object initialiser</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returns the business object created</returns>
        public Object CreateObject(IObjectEditor editor, IObjectInitialiser initialiser, string uiDefName)
        {
            BusinessObject newBo = _classDef.CreateNewBusinessObject();
            if (initialiser != null)
            {
                initialiser.InitialiseObject(newBo);
            }
            if (editor.EditObject(newBo, uiDefName))
            {
                return newBo;
            }
            else
            {
                return null;
            }
        }
    }
}