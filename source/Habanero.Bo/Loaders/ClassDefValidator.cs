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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    ///<summary>
    /// This class validates the Class Definitions against a set of standard criteria
    ///  If the class definitions fail these criteria then it is unlikely that Habanero will function correctly.
    ///</summary>
    public class ClassDefValidator
    {
        private readonly XmlClassDefsLoader _loader;
        public ClassDefValidator(XmlClassDefsLoader loader) { _loader = loader; }

        ///<summary>
        /// Validates a collection of Class Definitions.
        ///</summary>
        ///<param name="col">The class definitions being validated</param>
        ///<param name="errorMessage">A string message of all the failures detected or a null string if no failures are detected.</param>
        ///<returns>true if all the class Defs are valid false if any class def is invalid</returns>
        public bool AreClassDefsValid(ClassDefCol col, out string errorMessage)
        {
            errorMessage = "";
            try
            {
//                    UpdateKeyDefinitionsWithBoProp(col);
                CheckRelationships(col);
            }
            catch (InvalidXmlDefinitionException ex)
            {
                errorMessage = ex.Message;
            }
            return string.IsNullOrEmpty(errorMessage);
        }

        private void CheckRelationships(ClassDefCol classDefs)
        {
            Dictionary<IClassDef, IPropDefCol> loadedFullPropertyLists = new Dictionary<IClassDef, IPropDefCol>();
            foreach (IClassDef classDef in classDefs)
            {
                CheckRelationshipsForAClassDef(loadedFullPropertyLists, classDef, classDefs);
            }
        }

        private void CheckRelationshipsForAClassDef(IDictionary<IClassDef, IPropDefCol> loadedFullPropertyLists, IClassDef classDef, ClassDefCol classDefs)
        {
            if (classDef == null) return;

            IPropDefCol allPropsForClassDef = _loader.GetAllClassDefProps(loadedFullPropertyLists, classDef, classDefs);
            foreach (RelationshipDef relationshipDef in classDef.RelationshipDefCol)
            {
                // Check Relationship Properties
                foreach (RelPropDef relPropDef in relationshipDef.RelKeyDef)
                {
                    string ownerPropertyName = relPropDef.OwnerPropertyName;
                    if (!allPropsForClassDef.Contains(ownerPropertyName))
                    {
                        throw new InvalidXmlDefinitionException(String.Format(
                            "In a 'relatedProperty' element for the '{0}' relationship of " +
                            "the '{1}' class, the property '{2}' given in the " +
                            "'property' attribute does not exist for the class or for any of it's superclasses. " +
                            "Either add the property definition or check the spelling and " +
                            "capitalisation of the specified property. Check in the ClassDefs.xml file or fix in Firestarter",
                            relationshipDef.RelationshipName, classDef.ClassName, ownerPropertyName));
                    }
                }
            }
        }
    }
}