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
namespace Habanero.Base
{
    public interface ISuperClassDef
    {
        /// <summary>
        /// Returns the type of ORMapping used.  See the ORMapping
        /// enumeration for more detail.
        /// </summary>
        ORMapping ORMapping { get; set; }

        ///<summary>
        /// The assembly name of the SuperClass
        ///</summary>
        string AssemblyName { get; set; }

        ///<summary>
        /// The class name of the SuperClass
        ///</summary>
        string ClassName { get; set; }

        /// <summary>
        /// The type parameter of the SuperClass. See <see cref="IClassDef.TypeParameter"/>.
        /// </summary>
        string TypeParameter { get; set; }

        /// <summary>
        /// Returns the name of the discriminator column used to determine which class is being
        /// referred to in a row of the database table.
        /// This property applies only to SingleTableInheritance.
        /// </summary>
        string Discriminator { get; set; }

        /// <summary>
        /// Returns the class definition for this super-class
        /// </summary>
        IClassDef SuperClassClassDef { get; set; }

        /// <summary>
        /// Returns the name of the property that identifies which field
        /// in the child class (containing the super class definition)
        /// contains a copy of the parent's ID.  An empty string implies
        /// that the parent's ID is simply inherited and is used as the
        /// child's ID.  This property applies only to ClassTableInheritance.
        /// </summary>
        string ID { get; set; }

     
    }
}