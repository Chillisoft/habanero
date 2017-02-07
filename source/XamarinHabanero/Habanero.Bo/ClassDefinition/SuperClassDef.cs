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
using System;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    

    /// <summary>
    /// Manages a super-class in the case where inheritance is being used.
    /// </summary>
    public class SuperClassDef : ISuperClassDef
    {
        private ORMapping _orMapping;
        private IClassDef _superClassClassDef;
        private string _className;
        private string _assemblyName;
        private string _id;
        private string _discriminator;
        private string _typeParameter;

        #region Constructors

        /// <summary>
        /// Constructor to create a new super-class
        /// </summary>
        /// <param name="superClassDef">The class definition</param>
        /// <param name="mapping">The type of OR-Mapping to use. See
        /// the ORMapping enumeration for more detail.</param>
        public SuperClassDef(IClassDef superClassDef, ORMapping mapping)
        {
            _orMapping = mapping;
            MySuperClassDef = superClassDef;
        }

        /// <summary>
        /// Constructor to create a new super-class
        /// </summary>
        /// <param name="assemblyName">The assembly name of the superClass</param>
        /// <param name="className">The class name of the superClass</param>
        /// <param name="mapping">The type of OR-Mapping to use. See
        /// the ORMapping enumeration for more detail.</param>
        /// <param name="id">In ClassTableInheritance, this determines which property
        /// in the child has a copy of the parent ID.  Provide an empty string if in
        /// the code the child simply inherits the parent's ID.</param>
        /// <param name="discriminator">In SingleTableInheritance, this determines
        /// the name of the database column that stores the type name of the class
        /// being stored in that particular row.</param>
        public SuperClassDef(string assemblyName, string className, ORMapping mapping, string id, string discriminator)
        {
            _orMapping = mapping;
            _superClassClassDef = null;
            _assemblyName = assemblyName;
            _className = className;
            ID = id;
            Discriminator = discriminator;
        }

        #endregion Constructors

        #region properties

        /// <summary>
        /// Returns the type of ORMapping used.  See the ORMapping
        /// enumeration for more detail.
        /// </summary>
        public ORMapping ORMapping
        {
            get { return _orMapping; }
            set { _orMapping = value; }
        }


        ///<summary>
        /// The assembly name of the SuperClass
        ///</summary>
        public string AssemblyName
        {
            get { return _assemblyName; }
            set
            {
                if (_assemblyName != value)
                {
                    _superClassClassDef = null;
                    _className = null;
                }
                _assemblyName = value;
            }
        }

        ///<summary>
        /// The class name of the SuperClass
        ///</summary>
        public string ClassName
        {
            get { return _className; }
            set
            {
                if (_className != value)
                    _superClassClassDef = null;
                _className = value;
            }
        }

        /// <summary>
        /// The type parameter of this SuperClassDef
        /// </summary>
        public string TypeParameter
        {
            get { return _typeParameter; }
            set
            {
                if (_typeParameter != value)
                {
                    _superClassClassDef = null;
                }
                _typeParameter = value;
            }
        }

        /// <summary>
        /// Returns the name of the property that identifies which field
        /// in the child class (containing the super class definition)
        /// contains a copy of the parent's ID.  An empty string implies
        /// that the parent's ID is simply inherited and is used as the
        /// child's ID.  This property applies only to ClassTableInheritance.
        /// </summary>
        public string ID
        {
            get { return _id; }
            set
            {
                if (value != null && _orMapping != ORMapping.ClassTableInheritance)
                {
                    throw new ArgumentException
                        ("An 'ID' property has been specified "
                         + "for a super-class definition where the OR-mapping type is other than "
                         + "ClassTableInheritance.");
                }
                _id = value;
            }
        }

        /// <summary>
        /// Returns the name of the discriminator column used to determine which class is being
        /// referred to in a row of the database table.
        /// This property applies only to SingleTableInheritance.
        /// </summary>
        public string Discriminator
        {
            get { return _discriminator; }
            set
            {
                if (!String.IsNullOrEmpty(value) && _orMapping == ORMapping.ConcreteTableInheritance)
                {
                    throw new ArgumentException
                        ("A 'Discriminator' property has been specified "
                         + "for a super-class definition where the OR-mapping type is ConcreteTableInheritance");
                }
                _discriminator = value;
            }
        }

        /// <summary>
        /// Returns the class definition for this super-class
        /// </summary>
        public IClassDef SuperClassClassDef
        {
            get { return MySuperClassDef; }
            set { MySuperClassDef = value; }
        }

        #endregion properties

        #region SuperClassDef Methods

        /// <summary>
        /// The actual classdef associated with this superclassdef.
        /// </summary>
        public IClassDef MySuperClassDef
        {
            get
            {
                if (_superClassClassDef == null)
                {
                    //TODO Mark 2009-01-15: Look at how we can change this ClassDef.ClassDefs to the actual parent collection of this SuperClassDef's Class (Low priority)
                    _superClassClassDef = ClassDefHelper.GetSuperClassClassDef(this, ClassDef.ClassDefs);
                }
                return _superClassClassDef;
            }
            set
            {
                _superClassClassDef = value;
                if (_superClassClassDef != null)
                {
                    _assemblyName = _superClassClassDef.AssemblyName;
                    _className = _superClassClassDef.ClassName;
                }
                else
                {
                    _assemblyName = null;
                    _className = null;
                }
            }
        }

        #endregion SuperClassDef Methods
    }
}