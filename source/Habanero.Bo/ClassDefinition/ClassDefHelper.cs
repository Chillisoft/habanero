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

using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;

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
        public static IPropDef GetPropDefByPropName(IClassDef classDef, string propertyName)
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
                IRelationshipDefCol relationshipDefCol = classDef.RelationshipDefCol;
                IPropDef propDef = null;
                foreach (string relName in relNames)
                {
                    if (relationshipDefCol.Contains(relName))
                    {
                        IRelationshipDef relationshipDef = relationshipDefCol[relName];
                        IClassDef relatedClassDef = relationshipDef.RelatedObjectClassDef;
                        propDef = GetPropDefByPropName((ClassDef) relatedClassDef, propertyName);
                    }
                    if (propDef != null)
                    {
                        return propDef;
                    }
                }
                return null;
            }
            IPropDefCol propDefCol = classDef.PropDefColIncludingInheritance;
            return propDefCol.Contains(propertyName) ? propDefCol[propertyName] : null;
        }

        ///<summary>
        /// Returns the <see cref="ClassDef.PrimaryKeyDef"/> for the specified <see cref="ClassDef"/>.
        /// This may be an inherited <see cref="ClassDef.PrimaryKeyDef"/> from a super class.
        ///</summary>
        ///<param name="classDef">The <see cref="ClassDef"/> for which the <see cref="ClassDef.PrimaryKeyDef"/> needs to be found.</param>
        ///<param name="classDefCol">The <see cref="ClassDefCol"/> to use when searching the super classes for the <see cref="ClassDef.PrimaryKeyDef"/>.</param>
        ///<returns>Returns the <see cref="ClassDef.PrimaryKeyDef"/> for the specified <see cref="ClassDef"/>.</returns>
        ///<exception cref="InvalidXmlDefinitionException"></exception>
        public static IPrimaryKeyDef GetPrimaryKeyDef(IClassDef classDef, ClassDefCol classDefCol)
        {
            IPrimaryKeyDef primaryKeyDef = classDef.PrimaryKeyDef;
            if (primaryKeyDef == null)
            {
                ClassDef superClassClassDef = GetSuperClassClassDef(classDef.SuperClassDef, classDefCol);
                if (superClassClassDef != null)
                {
                    primaryKeyDef = superClassClassDef.PrimaryKeyDef;
                }
            }
            return primaryKeyDef;
        }

        ///<summary>
        /// Returns the <see cref="ClassDef"/> for the super class defined in the specified <see cref="SuperClassDef"/>.
        ///</summary>
        ///<param name="superClassDef">The <see cref="SuperClassDef"/> for which to find its Super class <see cref="ClassDef"/>.</param>
        ///<param name="classDefCol">The <see cref="ClassDefCol"/> to use to search for the super class <see cref="ClassDef"/>.</param>
        ///<returns>Returns the <see cref="ClassDef"/> for the super class defined in the specified <see cref="SuperClassDef"/>.</returns>
        ///<exception cref="InvalidXmlDefinitionException"></exception>
        public static ClassDef GetSuperClassClassDef(ISuperClassDef superClassDef, ClassDefCol classDefCol)
        {
            ClassDef superClassClassDef = null;
            string assemblyName = superClassDef.AssemblyName;
            string className = superClassDef.ClassName;
            if (assemblyName != null && className != null)
            {
                if (classDefCol.Contains(assemblyName, className))
                {
                    superClassClassDef = (ClassDef) classDefCol[assemblyName, className];
                }
                if (superClassClassDef == null)
                {
                    throw new InvalidXmlDefinitionException(String.Format(
                                                                "The class definition for the super class with the type " +
                                                                "'{0}' was not found. Check that the class definition " +
                                                                "exists or that spelling and capitalisation are correct. " +
                                                                "There are {1} class definitions currently loaded."
                                                                , assemblyName + "." + className, classDefCol.Count));
                }
            }
            return superClassClassDef;
        }
    }
}
