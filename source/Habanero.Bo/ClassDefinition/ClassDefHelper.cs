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

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Provides helper utilities for class definitions
    /// </summary>
    internal class ClassDefHelper
    {
        /// <summary>
        /// Finds the property definition with the given name for the specified
        /// class definition.  This method will search through an inheritance
        /// structure or relationship if needed.
        /// </summary>
        /// <param name="classDef">The class definition containing either the property
        /// or containing inheritance or relationship structures that might hold
        /// the property</param>
        /// <param name="propertyName">The name of the property.  A related property can
        /// be described by "RelationshipName.PropertyName".</param>
        /// <returns></returns>
        public static IPropDef GetPropDefByPropName(ClassDef classDef, string propertyName)
        {
            if (classDef == null || propertyName.IndexOf("-") != -1)
            {
                return null;
            }
            if (propertyName.IndexOf(".") != -1)
            {
                string relationshipName = propertyName.Substring(0, propertyName.IndexOf("."));
                propertyName = propertyName.Remove(0, propertyName.IndexOf(".") + 1);
                List<string> relNames = new List<string>();
                relNames.AddRange(relationshipName.Split(new string[]{"|"}, StringSplitOptions.RemoveEmptyEntries));
                RelationshipDefCol relationshipDefCol = classDef.RelationshipDefCol;
                IPropDef propDef = null;
                foreach (string relName in relNames)
                {
                    if (relationshipDefCol.Contains(relName))
                    {
                        RelationshipDef relationshipDef = relationshipDefCol[relName];
                        ClassDef relatedClassDef = relationshipDef.RelatedObjectClassDef;
                        propDef = GetPropDefByPropName(relatedClassDef, propertyName);
                    }
                    if (propDef != null)
                    {
                        return propDef;
                    }
                }
                return null;
            }
            else
            {
                IPropDefCol propDefCol = classDef.PropDefColIncludingInheritance;
                if (propDefCol.Contains(propertyName))
                {
                    return propDefCol[propertyName];
                } else
                {
                    return null;
                }
            }
        }
    }
}
