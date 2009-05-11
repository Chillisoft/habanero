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
using Habanero.BO.Comparer;
using Habanero.BO.Loaders;
using Habanero.Util;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Defines the business object class <see cref="IBusinessObject"/>, 
    ///   its properties <see cref="PropDefcol"/>,
    ///   their related property rules <see cref="IPropRule"/>,
    ///   its Primary and Alternate keys <see cref="IPrimaryKey"/> <see cref="IBOKey"/>, 
    ///   its relationships <see cref="IRelationship"/>,
    ///   any inheritance relationships
    ///   and the mappings of the <see cref="IBusinessObject"/> to the user inteface.
    /// <br/>
    /// The Definition includes the mapping of the <see cref="IBusinessObject"/> 
    ///   its properties and its relationships to the Database tables and fields.
    /// <br/>
    /// The Class Definition (ClassDef) is loaded from the ClassDef.xml file at application startup.
    /// <br/>
    /// The Class Definition class along with the ClassDef.xml implements the pattern
    ///   MetaData Mapper (Fowler - (306) 'Patterns of Enterprise Application Architecture').
    /// <br/>
    /// Required data is:
    /// <ul>
    /// <li>The primary key, which is the object identifier that uniquely
    ///     identifies the object in the database and object manager.
    ///     _Note that under Object-Oriented development philosophy, this
    ///     key is universally unique and should be indepedendent of the
    ///     values of the object, which differs somewhat from relational
    ///     database design.  However, the architecture has been extended
    ///     to support traditional composite keys in order to accomodate
    ///     some clients' requirements.</li>
    /// <li>All the properties and property types of the object that must
    ///     be recovered or persisted to the database.</li>
    /// <li>All the relationships of the object which it must manage.</li>
    /// </ul>
    /// </summary>
    /// <remarks>
    /// Design Notes:
    /// <ol>
    /// <li>To improve maintenance of the solution, a class definition
    ///    was chosen to hold the definitions of a class, rather than
    ///    having each class implement a ClassDefinition interface.
    ///    The ClassDef and BusinessObject work together to implement
    ///    functionality of a Business object.</li>
    /// </ol>
    /// </remarks>
    /// <futureEnhancements>
    /// TODO_Future:
    /// <ul>
    /// <li>Add abiltiy to do transaction log definition in class defs.</li>
    /// <li>Check for potential duplicates e.g. duplicate email addresses,
    ///  which should warn but not prevent.</li>
    /// </ul>
    /// </futureEnhancements>
    public class ClassDef : IClassDef
    {
        /// <summary>
        /// The collection of classDefs used for the the Singleton.
        /// </summary>
        protected static ClassDefCol _classDefCol;
        private string _assemblyName;
        private string _className;
        private string _classNameFull;
        private Type _classType;
        private string _tableName = "";
        private string _displayName = "";

        private PrimaryKeyDef _primaryKeyDef;
        private IPropDefCol _propDefCol;
        private KeyDefCol _keysCol;
        private RelationshipDefCol _relationshipDefCol;

        private SuperClassDef _superClassDef;
        private UIDefCol _uiDefCol;
        private string _singleTableInheritanceTableName;

        #region Constructors

        /// <summary>
        /// Constructor to create a new class definition object
        /// </summary>
        /// <param name="classType">The type of the class definition</param>
        /// <param name="primaryKeyDef">The primary key definition</param>
        /// <param name="tableName">The name of the table</param>
        /// <param name="propDefCol">The collection of property definitions</param>
        /// <param name="keyDefCol">The collection of key definitions</param>
        /// <param name="relationshipDefCol">The collection of relationship definitions</param>
        internal ClassDef(Type classType,
                          PrimaryKeyDef primaryKeyDef,
                          string tableName,
                          IPropDefCol propDefCol,
                          KeyDefCol keyDefCol,
                          RelationshipDefCol relationshipDefCol)
            : this(classType, primaryKeyDef, tableName, propDefCol, keyDefCol, relationshipDefCol, null)
        {
        }

        /// <summary>
        /// Constructor to create a new class definition object
        /// </summary>
        /// <param name="classType">The type of the class definition</param>
        /// <param name="primaryKeyDef">The primary key definition</param>
        /// <param name="tableName">The name of the table</param>
        /// <param name="propDefCol">The collection of property definitions</param>
        /// <param name="keyDefCol">The collection of key definitions</param>
        /// <param name="relationshipDefCol">The collection of relationship definitions</param>
        /// <param name="uiDefCol">The collection of user interface definitions</param>
        internal ClassDef(Type classType,
                          PrimaryKeyDef primaryKeyDef,
                          string tableName,
                          IPropDefCol propDefCol,
                          KeyDefCol keyDefCol,
                          RelationshipDefCol relationshipDefCol,
                          UIDefCol uiDefCol)
            : this(
                classType, null, null, tableName, null, primaryKeyDef, propDefCol, keyDefCol, relationshipDefCol,
                uiDefCol)
        {
        }


        /// <summary>
        /// As before, but excludes the table name
        /// </summary>
        public ClassDef(Type classType,
                        PrimaryKeyDef primaryKeyDef,
                        IPropDefCol propDefCol,
                        KeyDefCol keyDefCol,
                        RelationshipDefCol relationshipDefCol,
                        UIDefCol uiDefCol)
            :
                this(
                classType, null, null, null, null, primaryKeyDef, propDefCol, keyDefCol, relationshipDefCol,
                uiDefCol)
        {
        }

        /// <summary>
        /// As before, but excludes the table name and UI def collection
        /// </summary>
        public ClassDef(Type classType,
                        PrimaryKeyDef primaryKeyDef,
                        IPropDefCol propDefCol,
                        KeyDefCol keyDefCol,
                        IRelationshipDefCol relationshipDefCol)
            :
                this(
                classType, null, null, null, null, primaryKeyDef, propDefCol, keyDefCol, relationshipDefCol,
                null)
        {
        }

        /// <summary>
        /// As before, but excludes the table name
        /// </summary>
        public ClassDef(string assemblyName, string className, PrimaryKeyDef primaryKeyDef, IPropDefCol propDefCol, KeyDefCol keyDefCol,
                        IRelationshipDefCol relationshipDefCol, UIDefCol uiDefCol)
            : this(assemblyName, className, null, primaryKeyDef, propDefCol, keyDefCol, relationshipDefCol, uiDefCol)
        {
        }

        /// <summary>
        /// As before, but excludes the table name
        /// </summary>
        public ClassDef(string assemblyName, string className, string displayName, PrimaryKeyDef primaryKeyDef, IPropDefCol propDefCol,
                        KeyDefCol keyDefCol, IRelationshipDefCol relationshipDefCol, UIDefCol uiDefCol)
            : this(
                null, assemblyName, className, null, displayName, primaryKeyDef, propDefCol, keyDefCol,
                relationshipDefCol, uiDefCol)
        {
        }

        private ClassDef(Type classType, string assemblyName, string className, string tableName, string displayName,
                         PrimaryKeyDef primaryKeyDef, IPropDefCol propDefCol, KeyDefCol keyDefCol, IRelationshipDefCol relationshipDefCol,
                         UIDefCol uiDefCol)
        {
            if (classType != null)
                MyClassType = classType;
            else
            {
                _assemblyName = assemblyName;
                _classNameFull = className;
                _className = ClassDefCol.StripOutNameSpace(_classNameFull);
                _classType = null;
            }
            _displayName = displayName ?? "";
            _tableName = string.IsNullOrEmpty(tableName) ? _className : tableName;
            _primaryKeyDef = primaryKeyDef;
            _propDefCol = propDefCol;
            if (_propDefCol != null)
            {
                foreach (PropDef def in _propDefCol)
                {
                    def.ClassDef = this;
                }
            }
            _keysCol = keyDefCol;
            _relationshipDefCol = (RelationshipDefCol) relationshipDefCol;
            _uiDefCol = uiDefCol ?? new UIDefCol();
            _uiDefCol.ClassDef = this;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The name of the assembly for the class definition
        /// </summary>
        public string AssemblyName
        {
            get { return _assemblyName; }
            set
            {
                if (_assemblyName != value)
                {
                    _classType = null;
                    _classNameFull = null;
                    _className = null;
                }
                _assemblyName = value;
            }
        }

        /// <summary>
        /// The name of the class type for the class definition
        /// </summary>
        public string ClassName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.TypeParameter))
                {
                    return _className + "_" + this.TypeParameter;
                }
                return _className;
            }
            set
            {
                if (_className != value)
                    _classType = null;
                _classNameFull = value;
                _className = ClassDefCol.StripOutNameSpace(_classNameFull);
            }
        }

        /// <summary>
        /// The name of the class type for the class definition (excluding the namespace and the type parameter).
        /// </summary>
        public string ClassNameExcludingTypeParameter
        {
            get { return _className; }

        }

        /// <summary>
        /// The possibly full name of the class type for the class definition
        /// </summary>
        public string ClassNameFull
        {
            get { return _classNameFull; }
            set
            {
                if (_classNameFull != value)
                    _classType = null;
                _classNameFull = value;
                _className = ClassDefCol.StripOutNameSpace(_classNameFull);
            }
        }

        /// <summary>
        /// The type of the class definition
        /// </summary>
        public Type ClassType
        {
            get { return MyClassType; }
            set { MyClassType = value; }
        }

        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName
        {
            get
            {
                return _tableName;
            }
            set { _tableName = value; }
        }

        ///<summary>
        /// The display name for the class
        ///</summary>
        public string DisplayName
        {
            get
            {
                return String.IsNullOrEmpty(_displayName) 
                    ? StringUtilities.DelimitPascalCase(ClassName, " ") 
                    : _displayName;
            }
            set { _displayName = value; }
        }

        /// <summary>
        /// The name of the table where single table inheritance
        /// is being used in the table name is being inherited from
        /// some parent
        /// </summary>
        public string InheritedTableName
        {
            get
            {
                ClassDef currentClassDef = this;
                while (currentClassDef.SuperClassDef != null &&
                       currentClassDef.SuperClassDef.ORMapping == ORMapping.SingleTableInheritance)
                {
                    currentClassDef = currentClassDef.SuperClassClassDef;
                }
                if ((currentClassDef.SuperClassDef != null) &&
                    (currentClassDef.SuperClassDef.ORMapping == ORMapping.SingleTableInheritance))
                {
                    return currentClassDef.SuperClassClassDef.TableName;
                }
                return currentClassDef.TableName;
            }
        }


        /// <summary>
        /// The collection of property definitions
        /// </summary>
        public IPropDefCol PropDefcol
        {
            get { return _propDefCol; }
            set { _propDefCol = value; }
        }

        /// <summary>
        /// The collection of property definitions for this
        /// class and any properties inherited from parent classes
        /// </summary>
        public IPropDefCol PropDefColIncludingInheritance
        {
            get
            {
                PropDefCol col = new PropDefCol();
                col.Add(PropDefcol);

                ClassDef currentClassDef = this;
                while (currentClassDef.SuperClassClassDef != null)
                {
                    currentClassDef = currentClassDef.SuperClassClassDef;
                    col.Add(currentClassDef.PropDefcol);
                }
                return col;
            }
        }

        /// <summary>
        /// The collection of key definitions
        /// </summary>
        public KeyDefCol KeysCol
        {
            get { return _keysCol; }
            protected set { _keysCol = value; }
        }

        /// <summary>
        /// The primary key definition for this class definition.
        /// This could be null if the primary key is inherited from the super class.
        /// To retrieve the primary key that is used for this class use the GetPrimaryKeyDef() method.
        /// </summary>
        public PrimaryKeyDef PrimaryKeyDef
        {
            get { return _primaryKeyDef; }
            protected set { _primaryKeyDef = value; }
        }

        /// <summary>
        /// Indicates if the primary key of this class is an ObjectID, that is,
        /// the primary key is a single discrete property that serves as the ID
        /// </summary>
        public bool HasObjectID
        {
            get
            {
                if (_primaryKeyDef == null) return true;
                return _primaryKeyDef.IsGuidObjectID;
            }
        }

        /// <summary>
        /// The collection of relationship definitions
        /// </summary>
        public IRelationshipDefCol RelationshipDefCol
        {
            get { return _relationshipDefCol; }
            set { _relationshipDefCol = (RelationshipDefCol) value; }
        }

        /// <summary>
        /// The collection of user interface definitions
        /// </summary>
        public UIDefCol UIDefCol
        {
            get { return _uiDefCol; }
            protected set { _uiDefCol = value; }
        }

        ///// <summary>
        ///// Indicates whether synchronising is supported
        ///// </summary>
        //public bool SupportsSynchronising
        //{
        //    get { return _supportsSynchronisation; }
        //    set { _supportsSynchronisation = value; }
        //}

        #endregion Properties

        #region FactoryMethods

        /// <summary>
        /// Returns the collection of class definitions of which this is
        /// a part
        /// </summary>
        /// <returns>Returns a ClassDefCol object</returns>
        public static ClassDefCol ClassDefs
        {
            get { return ClassDefCol.GetColClassDef(); }
        }

        /// <summary>
        /// Indicates whether the specified class type is amongst those
        /// contained in the class definition collection
        /// </summary>
        /// <param name="classType">The class type in question</param>
        /// <returns>Returns true if found, false if not</returns>
        public static bool IsDefined(Type classType)
        {
            return ClassDefs.Contains(classType);
        }

        #endregion FactoryMethods

        #region Creating BOs

        /// <summary>
        /// Creates a collection of BO properties
        /// </summary>
        /// <param name="newObject">Whether the BOProp object being created 
        /// is a new object or is being 
        /// loaded from the DB. If it is new, then the property is
        /// initialised with the default value.</param>
        /// <returns>Returns the collection of BOProps</returns>
        public IBOPropCol CreateBOPropertyCol(bool newObject)
        {
            IBOPropCol propCol = _propDefCol.CreateBOPropertyCol(newObject);
            if (this.SuperClassDef != null)
            {
                ClassDef superClassDef = (ClassDef) SuperClassDef.SuperClassClassDef;
                propCol.Add(superClassDef.CreateBOPropertyCol(newObject));
                switch (this.SuperClassDef.ORMapping)
                {
                    case ORMapping.ConcreteTableInheritance:
                        if (superClassDef.PrimaryKeyDef != null)
                        {
                            foreach (PropDef def in superClassDef.PrimaryKeyDef)
                            {
                                propCol.Remove(def.PropertyName);
                            }
                        }
                        break;
                    case ORMapping.SingleTableInheritance:
                        if (this.PrimaryKeyDef != null)
                        {
                            foreach (PropDef def in this.PrimaryKeyDef)
                            {
                                propCol.Remove(def.PropertyName);
                            }
                        }
                        break;
                }
            }
            return propCol;
        }

        /// <summary>
        /// Creates a new business object
        /// </summary>
        /// <returns>Returns a new business object</returns>
        private BusinessObject InstantiateBusinessObject(bool instantiateWithClassDef)
            // This was internal, but it's been made private because you should rather use CreateNewBusinessObject
        {
            if (instantiateWithClassDef)
            {
                try
                {
                    return (BusinessObject)Activator.CreateInstance(MyClassType, new object[] { this });
                }
                catch (MissingMethodException ex)
                {
                    throw new MissingMethodException("Unable to instantiate a " + MyClassType.Name + " with a ClassDef.  Please inherit the constructor that takes a ClassDef as parameter. Or alternately call CreateNewBusinessObject with no parameter (or false) to use the default classdef for the type.", ex);
                }
            }
            else
            {

                try
                {

                    return (BusinessObject) Activator.CreateInstance(MyClassType, true);
                }
                catch (MissingMethodException ex)
                {
                    throw new MissingMethodException("Each class that implements " +
                                                     "BusinessObject needs to have a parameterless constructor.", ex);
                }
            }
        }

        /// <summary>
        /// Creates a new collection of relationships.
        /// </summary>
        /// <param name="propCol">The collection of properties</param>
        /// <param name="bo">The business object that owns this collection</param>
        /// <returns>Returns the new collection or null if no relationship 
        /// definition collection exists</returns>
        public RelationshipCol CreateRelationshipCol(IBOPropCol propCol, IBusinessObject bo)
        {
            if (_relationshipDefCol == null)
            {
                return null;
            }
            RelationshipCol relCol = _relationshipDefCol.CreateRelationshipCol(propCol, bo);
            if (this.SuperClassClassDef != null)
            {
                ClassDef superClassClassDef = (ClassDef) this.SuperClassDef.SuperClassClassDef;
                relCol.Add(superClassClassDef.CreateRelationshipCol(propCol, bo));
            }
            return relCol;
        }

        /// <summary>
        /// Creates a new collection of BO keys using the properties provided
        /// </summary>
        /// <param name="col">The collection of properties</param>
        /// <returns>Returns the new key collection</returns>
        public BOKeyCol createBOKeyCol(IBOPropCol col)
        {
            BOKeyCol keyCol = _keysCol.CreateBOKeyCol(col);
            if (this.SuperClassClassDef != null)
            {
                ClassDef superClassClassDef = (ClassDef) this.SuperClassDef.SuperClassClassDef;

                keyCol.Add(superClassClassDef.createBOKeyCol(col));
            }
            return keyCol;
        }

        /// <summary>
        /// Creates a new business object using the default class definition for the type linked to this <see cref="ClassDef"/>
        /// </summary>
        /// <returns>Returns the new object</returns>
        public IBusinessObject CreateNewBusinessObject()
        {
            return InstantiateBusinessObject(false);
        }


        /// <summary>
        /// Creates a new business object either using the default class definition for the type linked to this <see cref="ClassDef"/>
        /// or using this particular class definition (in the case where you might have more than one class definition for one C#
        /// type, useful for user defined types)
        /// Note_ that this means the business object being created must have a constructor that takes a <see cref="ClassDef"/>,
        /// passing this through to the base class as follows:
        /// <code>
        /// public class Entity
        /// {
        ///    public Entity() {}
        ///    public Entity(ClassDef def): base(def) { }
        /// }
        /// </code>
        /// </summary>
        /// <param name="instantiateWithClassDef">Whether to use the constructor that takes a classdef (in case you have multiple
        /// classdefs for a .NET type)</param>
        /// <returns>Returns the new object</returns>
        public IBusinessObject CreateNewBusinessObject(bool instantiateWithClassDef)
        {
            return InstantiateBusinessObject(instantiateWithClassDef);
        }


        #endregion //Creating BOs

        #region Type Initialisation

        private Type MyClassType
        {
            get
            {
                TypeLoader.LoadClassType(ref _classType, _assemblyName, _classNameFull,
                                         "class", "class definition");
                return _classType;
            }
            set
            {
                _classType = value;
                TypeLoader.ClassTypeInfo(_classType, out _assemblyName, out _classNameFull);
                _className = ClassDefCol.StripOutNameSpace(_classNameFull);
            }
        }

        #endregion Type Initialisation

        #region Superclasses & Inheritance

        /// <summary>
        /// Gets and sets the super-class of this class definition
        /// </summary>
        public SuperClassDef SuperClassDef
        {
            get { return _superClassDef; }
            set { _superClassDef = value; }
        }

        /// <summary>
        /// Returns the class definition of the super-class, or null
        /// if there is no super-class
        /// </summary>
        public ClassDef SuperClassClassDef
        {
            get
            {
                if (this.SuperClassDef != null)
                {
                    return (ClassDef) this.SuperClassDef.SuperClassClassDef;
                }
                return null;
            }
        }

        /// <summary>
        /// Indicates whether ClassTableInheritance is being used. See
        /// the ORMapping enumeration for more detail.
        /// </summary>
        /// <returns>Returns true if so, or false if there is no
        /// super class or another type of inheritance is being used</returns>
        public bool IsUsingClassTableInheritance()
        {
            return (this.SuperClassDef != null) && (this.SuperClassDef.ORMapping == ORMapping.ClassTableInheritance);
        }

        /// <summary>
        /// Indicates whether ConcreteTableInheritance is being used. See
        /// the ORMapping enumeration for more detail.
        /// </summary>
        /// <returns>Returns true if so, or false if there is no
        /// super class or another type of inheritance is being used</returns>
        public bool IsUsingConcreteTableInheritance()
        {
            return
                (this.SuperClassDef != null) && (this.SuperClassDef.ORMapping == ORMapping.ConcreteTableInheritance);
        }

        /// <summary>
        /// Indicates whether SingleTableInheritance is being used. See
        /// the ORMapping enumeration for more detail.
        /// </summary>
        /// <returns>Returns true if so, or false if there is no
        /// super class or another type of inheritance is being used</returns>
        public bool IsUsingSingleTableInheritance()
        {
            return (this.SuperClassDef != null) && (this.SuperClassDef.ORMapping == ORMapping.SingleTableInheritance);
        }

        /// <summary>
        /// Returns a collection of all the class defs that indicate a direct
        /// inheritance from this one
        /// </summary>
        /// <returns>Returns a ClassDefCol collection</returns>
        public ClassDefCol ImmediateChildren
        {
            get
            {
                ClassDefCol children = new ClassDefCol();
                foreach (ClassDef def in ClassDefs)
                {
                    if (def._superClassDef != null && def._superClassDef.SuperClassClassDef == this)
                    {
                        children.Add(def);
                    }
                }
                return children;
            }
        }

        /// <summary>
        /// Returns all children of this class based on the loaded inheritance hierachies
        /// </summary>
        public ClassDefCol AllChildren
        {
            get
            {
                ClassDefCol children = new ClassDefCol();
                ClassDefCol immediateChildren = ImmediateChildren;
                if (immediateChildren.Count == 0) return children;

                foreach (ClassDef def in immediateChildren)
                {
                    children.Add(def);
                    foreach (ClassDef child in def.AllChildren)
                    {
                        children.Add(child);
                    }
                }
                return children;
            }
        }

        /// <summary>
        /// This parameter can be used to allow multiple classdefs for one .NET type, as long as the
        /// type parameter between the classdefs are different.
        /// </summary>
        public string TypeParameter { get; set; }

        /// <summary>
        /// Creates a list of classdefs in this inheritance hierarchy.  The first item in the list is this classdef,
        /// followed by the one higher up the hierarchy (ie, this classdef's base or super class), followed by that
        /// one's base/super class and so on.
        /// </summary>
        /// <returns>The list of classdefs in order from lowest to highest in the hierarchy.</returns>
        public IList<ClassDef> GetAllClassDefsInHierarchy()
        {
            IList<ClassDef> classDefs = new List<ClassDef>();
            ClassDef tempClassDef = this;
            while (tempClassDef != null)
            {
                classDefs.Add(tempClassDef);
                tempClassDef = tempClassDef.SuperClassClassDef;
            }
            return classDefs;
        }

        #endregion //Superclasses&inheritance

        #region Returning Defs

        /// <summary>
        /// Takes the class definitions loaded into the IClassDefsLoader
        /// (eg. XmlClassDefsLoader)
        /// object and copies them across to the static ClassDefCol object.
        /// Checks to see that the definitions have not been loaded already,
        /// and displays a warning message to the console if so.
        /// </summary>
        /// <param name="loader">An object of type IClassDefsLoader
        /// or of its descendants</param>
        public static void LoadClassDefs(IClassDefsLoader loader)
        {
            ClassDefCol classDefCol = loader.LoadClassDefs();
            ClassDefCol.LoadColClassDef(classDefCol);
        }

        /// <summary>
        /// Searches the property definition collection and returns 
        /// the lookup-list found under the property with the
        /// name specified.  Also checks the super-class.
        /// </summary>
        /// <param name="propertyName">The property name in question</param>
        /// <returns>Returns the lookup-list if the property is
        /// found, or a NullLookupList object if not</returns>
        public ILookupList GetLookupList(string propertyName)
        {
            if (this.PropDefcol.Contains(propertyName))
            {
                return this.PropDefcol[propertyName].LookupList;
            }
            return this.SuperClassDef == null 
                ? new NullLookupList() 
                : this.SuperClassClassDef.GetLookupList(propertyName);
        }

        /// <summary>
        /// Searches the relationship definition collection and returns 
        /// the relationship definition found under the
        /// relationship with the name specified.
        /// </summary>
        /// <param name="relationshipName">The relationship name in question</param>
        /// <returns>Returns the relationship definition if found, 
        /// or null if not found</returns>
        public IRelationshipDef GetRelationship(string relationshipName)
        {
            ClassDef currentClassDef = this;
            while (currentClassDef != null)
            {
                if (currentClassDef.RelationshipDefCol.Contains(relationshipName))
                {
                    return currentClassDef.RelationshipDefCol[relationshipName];
                }
                currentClassDef = currentClassDef.SuperClassClassDef;
            }
            return null;
            //            throw new InvalidRelationshipAccessException(String.Format(
            //                "A relationship definition with the name of '{0}' was not found.",
            //                relationshipName));
        }

        /// <summary>
        /// Searches the UI definition collection and returns 
        /// the UI definition found under the UI with the name specified.
        /// </summary>
        /// <param name="uiDefName">The UI name in question</param>
        /// <returns>Returns the UI definition if found, or null if not found</returns>
        public UIDef GetUIDef(string uiDefName)
        {
            ClassDef currentClassDef = this;
            while (currentClassDef != null)
            {
                if (currentClassDef.UIDefCol.Contains(uiDefName))
                {
                    return currentClassDef.UIDefCol[uiDefName];
                }
                currentClassDef = currentClassDef.SuperClassClassDef;
            }
            return null;
        }

        /// <summary>
        /// Searches the property definition collection and returns the 
        /// property definition for the property with the name provided.
        /// </summary>
        /// <param name="propertyName">The property name in question</param>
        /// <returns>Returns the property definition if found, or
        /// throws an error if not</returns>
        /// <exception cref="InvalidPropertyNameException">
        /// This exception is thrown if the property is not found</exception>
        public IPropDef GetPropDef(string propertyName)
        {
            return GetPropDef(propertyName, true);
        }

        ///<summary>
        /// Returns a particular property definition for a class definition.
        ///</summary>
        ///<param name="source"></param>
        ///<param name="propertyName"></param>
        ///<param name="throwError"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentException"></exception>
        public IPropDef GetPropDef(Source source, string propertyName, bool throwError)
        {
            if (source == null) return GetPropDef(propertyName, throwError);
            if (!this.RelationshipDefCol.Contains(source.Name))
            {
                if (throwError)
                {
                    throw new ArgumentException("The ClassDef for " + this.ClassName + " does not contain a relationship with the name " +
                                                source.Name + " and thus cannot retrieve the PropDef through this relationship");
                }
                return null;
            }

            IClassDef relatedClassDef = this.RelationshipDefCol[source.Name].RelatedObjectClassDef;
            if (source.Joins.Count > 0)
            {
                return relatedClassDef.GetPropDef(source.Joins[0].ToSource, propertyName, throwError);
            }
            return relatedClassDef.GetPropDef(propertyName, throwError);
        }

        ///<summary>
        /// Retrieves the primary key definition for this class, traversing 
        /// the SuperClass structure to get the primary key definition if necessary
        ///</summary>
        ///<returns>The primary key for this class</returns>
        public PrimaryKeyDef GetPrimaryKeyDef()
        {
            PrimaryKeyDef primaryKeyDef = ClassDefHelper.GetPrimaryKeyDef(this, ClassDef.ClassDefs);
            return primaryKeyDef;
        }

        /// <summary>
        /// Searches the property definition collection and returns the 
        /// property definition for the property with the name provided.
        /// </summary>
        /// <param name="propertyName">The property name in question</param>
        /// <param name="throwError">Should an error be thrown if the property is not found</param>
        /// <returns>Returns the property definition if found, or
        /// throw an error if <paramref name="throwError"/> is true,
        /// otherwise return null</returns>
        /// <exception cref="InvalidPropertyNameException">
        /// This exception is thrown if the property is not found and 
        /// <paramref name="throwError"/> is true</exception>
        public IPropDef GetPropDef(string propertyName, bool throwError)
        {
            IPropDef foundPropDef = null;
            ClassDef currentClassDef = this;
            while (currentClassDef != null)
            {
                if (currentClassDef.PropDefcol.Contains(propertyName))
                {
                    foundPropDef = currentClassDef.PropDefcol[propertyName];
                    break;
                }
                currentClassDef = currentClassDef.SuperClassClassDef;
            }
            if (foundPropDef != null)
            {
                return foundPropDef;
            }
            if (throwError)
            {
                throw new InvalidPropertyNameException(String.Format(
                                                           "The property definition for the property '{0}' could not be " +
                                                           "found on a ClassDef of type '{1}'", propertyName, this.ClassNameFull));
            }
            return null;
        }

        #endregion //Returning defs

        #region Equals

        ///<summary>
        ///Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        ///</returns>
        ///
        ///<param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. </param>
        ///<exception cref="T:System.NullReferenceException">The <paramref name="obj" /> parameter is null.</exception><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ClassDef)) return false;
            return Equals((ClassDef) obj);
        }
        /// <summary>
        /// Checks for equality between two classdefs. This checks that the properties
        /// are the same between the two.
        /// </summary>
        /// <param name="otherClsDef">The Class Def to be compared to</param>
        /// <returns></returns>
        public bool Equals(ClassDef otherClsDef)
        {
            if (otherClsDef.TypeParameter != this.TypeParameter) return false;
            //This is a rough and ready equals test later need to improve
            if (PropDefcol == null) return false;
            if (PropDefcol.Count != otherClsDef.PropDefcol.Count)
            {
                return false;
            }
            foreach (PropDef def in this.PropDefcol)
            {
                if (!otherClsDef.PropDefcol.Contains(def.PropertyName))
                {
                    return false;
                }
            }
            return true;
//            return Equals(obj._className, _className) && Equals(obj._classType, _classType) && Equals(obj._primaryKeyDef, _primaryKeyDef) && Equals(obj._propDefCol, _propDefCol) && Equals(obj.TypeParameter, TypeParameter);
        }

        ///<summary>
        ///Serves as a hash function for a particular type. 
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the current <see cref="T:System.Object" />.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = (_className != null ? _className.GetHashCode() : 0);
                result = (result * 397) ^ (_classType != null ? _classType.GetHashCode() : 0);
                result = (result * 397) ^ (_primaryKeyDef != null ? _primaryKeyDef.GetHashCode() : 0);
                result = (result * 397) ^ (_propDefCol != null ? _propDefCol.GetHashCode() : 0);
                result = (result * 397) ^ (TypeParameter != null ? TypeParameter.GetHashCode() : 0);
                return result;
            }
        }


        #endregion

        ///<summary>
        /// Does a deep clone of the classdef and return the clone.
        /// NNB: The new propdefcol has the same propdefs in it (i.e. the propdefs are not copied)
        ///</summary>
        ///<returns></returns>
        public ClassDef Clone()
        {
            return Clone(false);
        }

        ///<summary>
        /// Does a deep clone of the classdef and return the clone.
        /// NNB: The new propdefcol has the same propdefs in it (i.e. the propdefs are not copied)
        ///</summary>
        ///<returns></returns>
        public ClassDef Clone(bool clonePropDefs)
        {
            IPropDefCol propDefClone = this.PropDefcol != null ? this.PropDefcol.Clone(clonePropDefs) : null;
            UIDefCol uiDefClone = this.UIDefCol != null ? this.UIDefCol.Clone() : null;
            ClassDef newClassDef = new ClassDef(this.AssemblyName, this.ClassName, this.PrimaryKeyDef,
                                                propDefClone, this.KeysCol,
                                                this.RelationshipDefCol, uiDefClone);
            newClassDef.TableName = this.TableName;
            newClassDef.DisplayName = this.DisplayName;

            return newClassDef;
        }


        /// <summary>
        /// Returns whether this class has an autoincrementing field or not. Checks the propdefs for whether
        /// one of them is autoincrementing, returning true if this is the case.
        /// </summary>
        public bool HasAutoIncrementingField
        {
            get
            {
                foreach (PropDef def in _propDefCol)
                {
                    if (def.AutoIncrementing) return true;
                }
                return false;
            }
        }


        ///<summary>
        /// traverses the inheritance hierachy to find the base class of this type in the case 
        /// of single table inheritance.
        ///</summary>
        ///<returns></returns>
        public ClassDef GetBaseClassOfSingleTableHierarchy()
        {
            ClassDef currentClassDef = this;
            while (currentClassDef.IsUsingSingleTableInheritance())
            {
                currentClassDef = currentClassDef.SuperClassClassDef;
            }
            return currentClassDef;
        }

        ///<summary>
        /// Gets the type of the specified property for this classDef.
        /// The specified property can also have a format like the custom properties for a UiGridColumn or UiFormField def.
        /// eg: MyRelatedBo.MyFurtherRelatedBo|MyAlternateRelatedBo.Name
        ///</summary>
        ///<param name="propertyName">The property to get the type for.</param>
        ///<returns>The type of the specified property</returns>
        public Type GetPropertyType(string propertyName)
        {
            //This could be Generalised this for properties that do not have PropDefs

            if (propertyName.IndexOf(".") != -1)
            {
                //Get the first property name
                string relationshipName = propertyName.Substring(0, propertyName.IndexOf("."));
                propertyName = propertyName.Remove(0, propertyName.IndexOf(".") + 1);
                //If there are some alternative relationships to traverse through then
                //  go through each alternative and check if there is a related object and return the first one
                // else get the related object

                string[] parts = relationshipName.Split(new char[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
                List<string> relNames = new List<string>(parts);
                List<Type> relatedPropertyTypes = new List<Type>();
                relNames.ForEach(delegate(string relationship)
                {
                    if (RelationshipDefCol.Contains(relationship))
                    {
                        IRelationshipDef relationshipDef = RelationshipDefCol[relationship];
                        IClassDef relatedObjectClassDef = relationshipDef.RelatedObjectClassDef;
                        if (relatedObjectClassDef != null)
                        {
                            Type propertyType = relatedObjectClassDef.GetPropertyType(propertyName);
                            relatedPropertyTypes.Add(propertyType);
                        }
                    }
                });
                Type currentPropertyType = null;
                relatedPropertyTypes.ForEach(delegate(Type propertyType)
                {
                    if (currentPropertyType == null)
                    {
                        currentPropertyType = propertyType;
                    }
                    else if (currentPropertyType != propertyType)
                    {
                        currentPropertyType = typeof (object);
                    }
                });

                return currentPropertyType ?? typeof (object);
            }
            if (propertyName.IndexOf("-") != -1)
            {
                return typeof (object);
            }
            IPropDef propDef = this.GetPropDef(propertyName, false);
            if (propDef != null && propDef.LookupList is NullLookupList)
            {
                return propDef.PropertyType;
            }
            return typeof (object);
        }

        ///<summary>
        /// Creates a property comparer for the given property
        /// The specified property can also have a format like the custom properties for a UiGridColumn or UiFormField def.
        /// eg: MyRelatedBo.MyFurtherRelatedBo|MyAlternateRelatedBo.Name
        ///</summary>
        ///<param name="propertyName">The property to get the type for.</param>
        ///<returns>The type of the specified property</returns>
        public IPropertyComparer<T> CreatePropertyComparer<T>(string propertyName) where T : IBusinessObject
        {
            Type comparerType = typeof (PropertyComparer<,>);
            Type propertyType = GetPropertyType(propertyName);
            comparerType = comparerType.MakeGenericType(typeof (T), propertyType);
            IPropertyComparer<T> comparer = (IPropertyComparer<T>) Activator.CreateInstance(comparerType, propertyName);
            return comparer;
        }

        /// <summary>
        /// Returns the table name for this class
        /// </summary>
        /// <returns>Returns the table name of first real table for this class.</returns>
        public string GetTableName()
        {
            if (!string.IsNullOrEmpty(_singleTableInheritanceTableName))
            {
                return _singleTableInheritanceTableName;
            }
            if (IsUsingSingleTableInheritance())
            {
                ClassDef superClassClassDef = SuperClassClassDef;
                if (superClassClassDef != null)
                {
                    _singleTableInheritanceTableName = superClassClassDef.GetTableName();
                    return _singleTableInheritanceTableName;
                }
            }
            return TableName;
        }

      

        /// <summary>
        /// Returns the table name of the specified property
        /// </summary>
        /// <param name="propDef">The property to fine the table name for</param>
        /// <returns>Returns the table name of the table that the specified property belongs to</returns>
        public string GetTableName(IPropDef propDef)
        {
            if (IsUsingConcreteTableInheritance())
            {
                return TableName;
            }
            return propDef.ClassDef.GetTableName();
//            //This is coded to throw the exception instead of doing a Contains for performance Reasons
//            try
//            {
//                IPropDef defcol = PropDefcol[propDef.PropertyName];
//                return GetTableName();
//            }
//            catch (ArgumentException)
//            {
//                ClassDef superClassClassDef = SuperClassClassDef;
//                return superClassClassDef != null ? superClassClassDef.GetTableName(propDef) : "";
//            }

        }


        ///<summary>
        /// Returns the Class Defition for a <see cref="IBusinessObject"/> of type T.
        ///</summary>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        public static ClassDef Get<T>() where T : class, IBusinessObject
        {
            return ClassDefs[typeof(T)];
        }
    }
}