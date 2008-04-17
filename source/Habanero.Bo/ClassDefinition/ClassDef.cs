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
using Habanero.Base;
using Habanero.BO.Loaders;
using Habanero.Util;
using Habanero.Util.File;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Defines the properties of a class that need to be managed at a 
    /// Business Object (BO) level.<br/><br/>
    /// Required properties are:
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
    /// <li>All concurrency control rules</li>
    /// <li>All transactional log rules</li>
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
    /// <li>Add abiltiy to do transaction logs.</li>
    /// <li>Check for duplicates.</li>
    /// <li>Check for potential duplicates e.g. duplicate email addresses,
    ///  which should warn but not prevent.</li>
    /// <li>Check for absolute duplicates e.g. not allow duplicate first
    ///  name surname.</li>
    /// </ul>
    /// </futureEnhancements>
    public class ClassDef
    {
        protected static ClassDefCol _classDefCol;
        private string _assemblyName;
        private string _className;
        private string _classNameFull;
        private Type _classType;
        //private string _databaseName = "Default";
        //private string _SelectSql = "";
        private string _tableName = "";
        private string _displayName = "";
        private bool _hasObjectID = true;
        private PrimaryKeyDef _primaryKeyDef;
        private PropDefCol _propDefCol;
        private KeyDefCol _keysCol;
        private RelationshipDefCol _relationshipDefCol;

        private SuperClassDef _superClassDef;
        private UIDefCol _uiDefCol;
        private string _typeParameter;
        //private bool _supportsSynchronisation;

        //private static PropDef _versionNumberAtLastSyncPropDef =
        //    new PropDef("SyncVersionNumberAtLastSync", typeof (int), PropReadWriteRule.ReadWrite, 0);

        //private static PropDef _versionNumberPropDef =
        //    new PropDef("SyncVersionNumber", typeof (int), PropReadWriteRule.ReadWrite, 0);

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
                          PropDefCol propDefCol,
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
                          PropDefCol propDefCol,
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
                        PropDefCol propDefCol,
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
                        PropDefCol propDefCol,
                        KeyDefCol keyDefCol,
                        RelationshipDefCol relationshipDefCol)
            :
                            this(
                            classType, null, null, null, null, primaryKeyDef, propDefCol, keyDefCol, relationshipDefCol,
                            null)
        {
        }

        ///// <summary>
        ///// As before, but allows a database name to be specified
        ///// </summary>
        //public ClassDef(Type classType, PrimaryKeyDef primaryKeyDef, string tableName, PropDefCol propDefCol, KeyDefCol keyDefCol, RelationshipDefCol relationshipDefCol)
        //    : this(classType, null, null, tableName, null, primaryKeyDef, propDefCol, keyDefCol, relationshipDefCol, null)
        //{
        //}

        ///// <summary>
        ///// As before, but allows a database name and a user interface 
        ///// definition collection to be specified
        ///// </summary>
        //public ClassDef(Type classType, PrimaryKeyDef primaryKeyDef, string tableName, PropDefCol propDefCol, KeyDefCol keyDefCol, RelationshipDefCol relationshipDefCol, UIDefCol uiDefCol)
        //    :this(classType,null,null,tableName, null,primaryKeyDef, propDefCol, keyDefCol, relationshipDefCol,uiDefCol)
        //{}

        /// <summary>
        /// As before, but excludes the table name
        /// </summary>
        public ClassDef(string assemblyName,
                        string className,
                        PrimaryKeyDef primaryKeyDef,
                        PropDefCol propDefCol,
                        KeyDefCol keyDefCol,
                        RelationshipDefCol relationshipDefCol,
                        UIDefCol uiDefCol)
            : this(assemblyName, className, null, primaryKeyDef, propDefCol, keyDefCol, relationshipDefCol, uiDefCol)
        {
        }

        /// <summary>
        /// As before, but excludes the table name
        /// </summary>
        public ClassDef(string assemblyName,
                        string className,
                        string displayName,
                        PrimaryKeyDef primaryKeyDef,
                        PropDefCol propDefCol,
                        KeyDefCol keyDefCol,
                        RelationshipDefCol relationshipDefCol,
                        UIDefCol uiDefCol)
            : this(
                null, assemblyName, className, null, displayName, primaryKeyDef, propDefCol, keyDefCol,
                relationshipDefCol, uiDefCol)
        {
        }

        private ClassDef(Type classType, string assemblyName, string className, string tableName, string displayName,
                         PrimaryKeyDef primaryKeyDef, PropDefCol propDefCol, KeyDefCol keyDefCol,
                         RelationshipDefCol relationshipDefCol, UIDefCol uiDefCol)
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
            //_databaseName = databaseName;
            _displayName = displayName ?? "";
            if (tableName == null || tableName.Length == 0)
                _tableName = _className;
            else
                _tableName = tableName;
            _primaryKeyDef = primaryKeyDef;
            _propDefCol = propDefCol;
            _keysCol = keyDefCol;
            _relationshipDefCol = relationshipDefCol;
            if (uiDefCol != null)
                _uiDefCol = uiDefCol;
            else
                _uiDefCol = new UIDefCol();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The name of the assembly for the class definition
        /// </summary>
        public string AssemblyName
        {
            get { return _assemblyName; }
            protected set
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
                else
                {
                    return _className;
                }
            }
            protected set
            {
                if (_className != value)
                    _classType = null;
                _classNameFull = value;
                _className = ClassDefCol.StripOutNameSpace(_classNameFull);
            }
        }

        /// <summary>
        /// The possibly full name of the class type for the class definition
        /// </summary>
        internal string ClassNameFull
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
            protected set { MyClassType = value; }
        }

        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName
        {
            get
            {
                //log.Debug(_tableName.ToLower());
                return _tableName; //.ToLower() ;
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
                if (!String.IsNullOrEmpty(_displayName))
                {
                    return _displayName;
                }
                else
                {
                    return StringUtilities.DelimitPascalCase(ClassName, " ");
                }
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
                else
                {
                    return currentClassDef.TableName;
                }
            }
        }


        /// <summary>
        /// The collection of property definitions
        /// </summary>
        public PropDefCol PropDefcol
        {
            get { return _propDefCol; }
            protected set { _propDefCol = value; }
        }

        /// <summary>
        /// The collection of property definitions for this
        /// class and any properties inherited from parent classes
        /// </summary>
        public PropDefCol PropDefColIncludingInheritance
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
            get { return _hasObjectID; }
            set { _hasObjectID = value; }
        }

        /// <summary>
        /// The collection of relationship definitions
        /// </summary>
        public RelationshipDefCol RelationshipDefCol
        {
            get { return _relationshipDefCol; }
            set { _relationshipDefCol = value; }
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
        public BOPropCol createBOPropertyCol(bool newObject)
        {
            BOPropCol propCol = _propDefCol.CreateBOPropertyCol(newObject);
            if (this.SuperClassDef != null)
            {
                ClassDef superClassDef = SuperClassDef.SuperClassClassDef;
                propCol.Add(superClassDef.createBOPropertyCol(newObject));
                if (this.SuperClassDef.ORMapping == ORMapping.ConcreteTableInheritance)
                {
                    if (superClassDef.PrimaryKeyDef != null)
                    {
                        foreach (PropDef def in superClassDef.PrimaryKeyDef)
                        {
                            propCol.Remove(def.PropertyName);
                        }
                    }
                }
                else if (this.SuperClassDef.ORMapping == ORMapping.SingleTableInheritance)
                {
                    if (this.PrimaryKeyDef != null)
                    {
                        foreach (PropDef def in this.PrimaryKeyDef)
                        {
                            propCol.Remove(def.PropertyName);
                        }
                    }
                }
            }
            //if (this.SupportsSynchronising)
            //{
            //    propCol.Add(_versionNumberPropDef.CreateBOProp(newObject));
            //    propCol.Add(_versionNumberAtLastSyncPropDef.CreateBOProp(newObject));
            //}
            return propCol;
        }

        /// <summary>
        /// Creates a new business object
        /// </summary>
        /// <returns>Returns a new business object</returns>
        private BusinessObject InstantiateBusinessObject()
        // This was internal, but it's been made private because you should rather use CreateNewBusinessObject
        {
            try
            {
                return (BusinessObject)Activator.CreateInstance(MyClassType, true);
            }
            catch (MissingMethodException ex)
            {
                throw new MissingMethodException("Each class that implements " +
                                                 "BusinessObject needs to have a parameterless constructor.", ex);
            }
        }

        ///// <summary>
        ///// Creates a new business object using this class definition
        ///// </summary>
        ///// <returns>Returns a new business object</returns>
        //private BusinessObject InstantiateBusinessObjectWithClassDef()
        //// This was internal, but it's been made private because you should rather use CreateNewBusinessObject
        //{
        //    try
        //    {
        //        return (BusinessObject)Activator.CreateInstance(MyClassType, new object[] { });
        //    }
        //    catch (MissingMethodException ex)
        //    {
        //        throw new MissingMethodException("Each class that implements " +
        //             "BusinessObject needs to have a constructor with an argument " +
        //             "to accept a ClassDef object and pass it to the base class " +
        //             "(eg. public _className(ClassDef classDef) : base(classDef) {} )", ex);
        //    }
        //}

        /// <summary>
        /// Creates a new collection of relationships.
        /// </summary>
        /// <param name="propCol">The collection of properties</param>
        /// <param name="bo">The business object that owns this collection</param>
        /// <returns>Returns the new collection or null if no relationship 
        /// definition collection exists</returns>
        public RelationshipCol CreateRelationshipCol(BOPropCol propCol, BusinessObject bo)
        {
            if (_relationshipDefCol == null)
            {
                return null;
            }
            RelationshipCol relCol;
            relCol = _relationshipDefCol.CreateRelationshipCol(propCol, bo);
            if (this.SuperClassClassDef != null)
            {
                relCol.Add(this.SuperClassDef.SuperClassClassDef.CreateRelationshipCol(propCol, bo));
            }
            return relCol;
        }

        /// <summary>
        /// Creates a new collection of BO keys using the properties provided
        /// </summary>
        /// <param name="col">The collection of properties</param>
        /// <returns>Returns the new key collection</returns>
        public BOKeyCol createBOKeyCol(BOPropCol col)
        {
            BOKeyCol keyCol = _keysCol.CreateBOKeyCol(col);
            if (this.SuperClassClassDef != null)
            {
                keyCol.Add(SuperClassDef.SuperClassClassDef.createBOKeyCol(col));
            }
            return keyCol;
        }

        /// <summary>
        /// Creates a new business object using this class definition
        /// </summary>
        /// <returns>Returns the new object</returns>
        public BusinessObject CreateNewBusinessObject()
        {
            return InstantiateBusinessObject(); //this.InstantiateBusinessObjectWithClassDef();
        }

        /// <summary>
        /// Creates a new business object using this class definition
        /// and a provided database connection
        /// </summary>
        /// <param name="conn">A database connection</param>
        /// <returns>Returns the new object</returns>
        public BusinessObject CreateNewBusinessObject(IDatabaseConnection conn)
        {
            BusinessObject newObj = InstantiateBusinessObject(); //this.InstantiateBusinessObjectWithClassDef();
            newObj.SetDatabaseConnection(conn);
            return newObj;
        }

        #endregion //Creating BOs

        #region Type Initialisation

        private Type MyClassType
        {
            get
            {
                TypeLoader.LoadClassType(ref _classType, _assemblyName, _classNameFull,
                                         "class", "class definition");
                //if (_classType == null && _assemblyName != null && _className != null)
                //{
                //    try
                //    {
                //        _classType = TypeLoader.LoadType(_assemblyName, _className);
                //    }
                //    catch (UnknownTypeNameException ex)
                //    {
                //        throw new UnknownTypeNameException("Unable to load the class type while " +
                //            "attempting to load a type from a class definition, given the 'assembly' as: '" +
                //            _assemblyName + "', and the 'class' as: '" + _className +
                //            "'. Check that the class exists in the given assembly name and " +
                //            "that spelling and capitalisation are correct.", ex);
                //    }
                //}
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
        internal ClassDef SuperClassClassDef
        {
            get
            {
                if (this.SuperClassDef != null)
                {
                    return this.SuperClassDef.SuperClassClassDef;
                }
                else
                {
                    return null;
                }
            }
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

        /// <summary>
        /// Indicates whether ClassTableInheritance is being used. See
        /// the ORMapping enumeration for more detail.
        /// </summary>
        /// <returns>Returns true if so, or false if there is no
        /// super class or another type of inheritance is being used</returns>
        protected internal bool IsUsingClassTableInheritance()
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
        /// This parameter can be used to allow multiple classdefs for one .NET type, as long as the
        /// type parameter between the classdefs are different.
        /// </summary>
        public string TypeParameter
        {
            get { return _typeParameter; }
            set { _typeParameter = value; }
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
            else
            {
                if (this.SuperClassDef != null)
                {
                    return this.SuperClassClassDef.GetLookupList(propertyName);
                }
                else
                {
                    return new NullLookupList();
                }
            }
        }

        /// <summary>
        /// Searches the relationship definition collection and returns 
        /// the relationship definition found under the
        /// relationship with the name specified.
        /// </summary>
        /// <param name="relationshipName">The relationship name in question</param>
        /// <returns>Returns the relationship definition if found, 
        /// or null if not found</returns>
        public RelationshipDef GetRelationship(string relationshipName)
        {
            ClassDef currentClassDef = this;
            while (currentClassDef != null)
            {
                if (currentClassDef.RelationshipDefCol.Contains(relationshipName))
                {
                    return currentClassDef.RelationshipDefCol[relationshipName];
                }
                else
                {
                    currentClassDef = currentClassDef.SuperClassClassDef;
                }
            }
            return null;
            //            throw new InvalidRelationshipAccessException(String.Format(
            //                "A relationship definition with the name of '{0}' was not found.",
            //                relationshipName));
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
        public PropDef GetPropDef(string propertyName)
        {
            return GetPropDef(propertyName, true);
        }

        ///<summary>
        /// Retrieves the primary key definition for this class, traversing 
        /// the SuperClass structure to get the primary key definition if necessary
        ///</summary>
        ///<returns>The primary key for this class</returns>
        public PrimaryKeyDef GetPrimaryKeyDef()
        {
            PrimaryKeyDef primaryKeyDef = PrimaryKeyDef;
            if (primaryKeyDef == null)
            {
                ClassDef superClassClassDef = SuperClassClassDef;
                if (superClassClassDef != null)
                {
                    primaryKeyDef = superClassClassDef.GetPrimaryKeyDef();
                }
            }
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
        public PropDef GetPropDef(string propertyName, bool throwError)
        {
            PropDef foundPropDef = null;
            ClassDef currentClassDef = this;
            while (currentClassDef != null)
            {
                if (currentClassDef.PropDefcol.Contains(propertyName))
                {
                    foundPropDef = currentClassDef.PropDefcol[propertyName];
                    break;
                }
                else
                {
                    currentClassDef = currentClassDef.SuperClassClassDef;
                }
            }
            if (foundPropDef != null)
            {
                return foundPropDef;
            }
            else if (throwError)
            {
                throw new InvalidPropertyNameException(String.Format(
                                                           "The property definition for the property '{0}' could not be " +
                                                           "found.", propertyName));
            }
            else
            {
                return null;
            }
        }

        #endregion //Returning defs

        #region Equals

        /// <summary>
        /// Checks for equality between two classdefs. This checks that the properties
        /// are the same between the two.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(ClassDef)) return false;
            ClassDef otherClsDef = (ClassDef)obj;
            //TODO this is a rough and ready equals test later need to improve
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
        }

        #endregion

        ///<summary>
        /// Does a deep clone of the classdef and return the clone
        ///</summary>
        ///<returns></returns>
        public ClassDef Clone()
        {
            ClassDef newClassDef = new ClassDef(this.AssemblyName, this.ClassName, this.PrimaryKeyDef,
                                                this.PropDefcol != null ? this.PropDefcol.Clone() : null, this.KeysCol,
                                                this.RelationshipDefCol, this.UIDefCol);
            newClassDef.TableName = this.TableName;
            newClassDef.DisplayName = this.DisplayName;
           
            return newClassDef;
        }
    }
}