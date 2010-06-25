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
using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.Comparer;
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
//        /// <summary>
//        /// The collection of classDefs used for the the Singleton.
//        /// </summary>
//        protected static ClassDefCol _classDefCol;

        private string _assemblyName;
        private string _cachedTableName;
        private string _className;
        private string _classNameFull;
        private Type _classType;
        private string _displayName = "";
        private KeyDefCol _keysCol;

        private IPrimaryKeyDef _primaryKeyDef;
        private IPropDefCol _propDefCol;
        private IPropDefCol _propDefColIncludingInheritance;
        private IRelationshipDefCol _relationshipDefCol;

        private string _tableName = "";
        private UIDefCol _uiDefCol;

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
                          IPrimaryKeyDef primaryKeyDef,
                          string tableName,
                          IPropDefCol propDefCol,
                          KeyDefCol keyDefCol,
                          IRelationshipDefCol relationshipDefCol)
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
                          IPrimaryKeyDef primaryKeyDef,
                          string tableName,
                          IPropDefCol propDefCol,
                          KeyDefCol keyDefCol,
                          IRelationshipDefCol relationshipDefCol,
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
                        IPrimaryKeyDef primaryKeyDef,
                        IPropDefCol propDefCol,
                        KeyDefCol keyDefCol,
                        IRelationshipDefCol relationshipDefCol,
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
                        IPrimaryKeyDef primaryKeyDef,
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
        public ClassDef(string assemblyName, string className, IPrimaryKeyDef primaryKeyDef, IPropDefCol propDefCol,
                        KeyDefCol keyDefCol,
                        IRelationshipDefCol relationshipDefCol, UIDefCol uiDefCol)
            : this(assemblyName, className, null, primaryKeyDef, propDefCol, keyDefCol, relationshipDefCol, uiDefCol)
        {
        }

        /// <summary>
        /// As before, but excludes the table name
        /// </summary>
        public ClassDef(string assemblyName, string className, string displayName, IPrimaryKeyDef primaryKeyDef,
                        IPropDefCol propDefCol,
                        KeyDefCol keyDefCol, IRelationshipDefCol relationshipDefCol, UIDefCol uiDefCol)
            : this(
                null, assemblyName, className, null, displayName, primaryKeyDef, propDefCol, keyDefCol,
                relationshipDefCol, uiDefCol)
        {
        }

        private ClassDef(Type classType, string assemblyName, string className, string tableName, string displayName,
                         IPrimaryKeyDef primaryKeyDef, IPropDefCol propDefCol, KeyDefCol keyDefCol,
                         IRelationshipDefCol relationshipDefCol,
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
            this.PropDefcol = propDefCol;
            this.KeysCol = keyDefCol;
            this.RelationshipDefCol = relationshipDefCol;
            SetClassDefOnChildClasses();
            this.UIDefCol = uiDefCol ?? new UIDefCol();
        }

        private void SetClassDefOnChildClasses()
        {
            if (_propDefCol != null)
            {
                foreach (PropDef def in _propDefCol)
                {
                    def.ClassDef = this;
                }
            }
            if(_relationshipDefCol != null)
            {
                foreach (RelationshipDef relationshipDef in _relationshipDefCol)
                {
                    relationshipDef.OwningClassDef = this;
                }
            }
            if(this.UIDefCol != null)
            {
                foreach (UIDef uiDef in UIDefCol)
                {
                    uiDef.ClassDef = this;
                }
            }
        }

        #endregion Constructors
        #region Properties

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
                IClassDef currentClassDef = this;
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
        /// The collection of key definitions
        /// </summary>
        public KeyDefCol KeysCol
        {
            get { return _keysCol; }
            set
            {
                _keysCol = value;
                if(_keysCol != null) _keysCol.ClassDef = this;
            }
        }

        /// <summary>
        /// The primary key definition for this class definition.
        /// Retrieves the primary key definition for this class, traversing 
        /// the SuperClass structure to get the primary key definition if necessary
        ///  </summary>
        public IPrimaryKeyDef PrimaryKeyDef
        {
            get
            {
                return _primaryKeyDef;
            }
            set { _primaryKeyDef = value; }
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
        /// The collection of user interface definitions
        /// </summary>
        public UIDefCol UIDefCol
        {
            get { return _uiDefCol; }
            set { _uiDefCol = value;
                if(_uiDefCol != null) _uiDefCol.ClassDef = this;
            }
        }

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
                if (!string.IsNullOrEmpty(TypeParameter))
                {
                    return _className + "_" + TypeParameter;
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
            get { return _tableName; }
            set { _tableName = value; }
        }


        /// <summary>
        /// The collection of property definitions
        /// </summary>
        public IPropDefCol PropDefcol
        {
            get { return _propDefCol; }
            set
            {
                _propDefCol = value;
                if(_propDefCol != null) _propDefCol.ClassDef = this;
            }
        }

        /// <summary>
        /// The collection of property definitions for this
        /// class and any properties inherited from parent classes
        /// </summary>
        public IPropDefCol PropDefColIncludingInheritance
        {
            get
            {

                if (_propDefColIncludingInheritance == null ||
                    _propDefColIncludingInheritance.Count != this.TotalNoOfPropsIncludingInheritance)
                {                   
                    _propDefColIncludingInheritance = new PropDefCol {ClassDef = this};
                    _propDefColIncludingInheritance.Add(this.PropDefcol);

                    IClassDef currentClassDef = this;
                    while (currentClassDef.SuperClassClassDef != null)
                    {
                        currentClassDef = currentClassDef.SuperClassClassDef;
                        _propDefColIncludingInheritance.ClassDef = currentClassDef;
                        _propDefColIncludingInheritance.Add(currentClassDef.PropDefcol);
                    }
                }
                _propDefColIncludingInheritance.ClassDef = this;

                return _propDefColIncludingInheritance;
            }
        }
        /// <summary>
        /// This is a bit of a Hack but you need to be able to deal with a situation
        /// where you call <see cref="PropDefColIncludingInheritance"/> then add a prop
        /// to this class or any of its superclasses and then call
        /// <see cref="PropDefColIncludingInheritance"/> again.
        /// You would expect to see the new PropDef added at the same time you
        /// do not want to do the recursive algorithm of building the PropDef list repeatedly
        /// since this is used by the GetPropDef method which is used intensively.
        /// This recursive method with adding all the properties is less intense.
        /// We also do not want to add events to the PropDefCol that can be fired every
        /// time a PropDef is added or removed since this would
        ///  </summary>
        private int TotalNoOfPropsIncludingInheritance
        {
            get
            {
                int noProps = this.PropDefcol.Count;
                ClassDef currentClassDef = this;
                while (currentClassDef.SuperClassClassDef != null)
                {
                    currentClassDef = (ClassDef) currentClassDef.SuperClassClassDef;
                    noProps += currentClassDef.TotalNoOfPropsIncludingInheritance;
                }
                return noProps;
            }
        }
        /// <summary>
        /// The collection of relationship definitions
        /// </summary>
        public IRelationshipDefCol RelationshipDefCol
        {
            get { return _relationshipDefCol; }
            set { _relationshipDefCol = value;
                if(_relationshipDefCol != null) _relationshipDefCol.ClassDef = this;
            }
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
        /// Creates a new business object using the default class definition for the type linked to this <see cref="IClassDef"/>
        /// or using this particular class definition (in the case where you might have more than one class definition for one C#
        /// type, useful for user defined types).  If this <see cref="IClassDef"/> has a <see cref="IClassDef.TypeParameter"/> then
        /// the instantiation will happen with the <see cref="IClassDef"/> passed in as a parameter to the <see cref="IBusinessObject"/>
        /// constructor.
        /// Note_ that this means the business object being created must have a constructor that takes a <see cref="IClassDef"/>,
        /// passing this through to the base class as follows:
        /// <code>
        /// public class Entity
        /// {
        ///    public Entity() {}
        ///    public Entity(ClassDef def): base(def) { }
        /// }
        /// </code>
        /// </summary>
        /// <returns>Returns the new object</returns>
        public IBusinessObject CreateNewBusinessObject()
        {
            bool instantiateWithClassDef = !String.IsNullOrEmpty(TypeParameter);
            return InstantiateBusinessObject(instantiateWithClassDef);
        }

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
            if (SuperClassDef != null)
            {
                var superClassDef = (ClassDef) SuperClassDef.SuperClassClassDef;
                propCol.Add(superClassDef.CreateBOPropertyCol(newObject));
                switch (SuperClassDef.ORMapping)
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
                        if (_primaryKeyDef != null)
                        {
                            foreach (PropDef def in _primaryKeyDef)
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
                    return (BusinessObject) Activator.CreateInstance(MyClassType, new object[] {this});
                }
                catch (MissingMethodException ex)
                {
                    throw new MissingMethodException(
                        "Unable to instantiate a " + MyClassType.Name +
                        " with a ClassDef.  Please inherit the constructor that takes a ClassDef as parameter. Or alternately call CreateNewBusinessObject with no parameter (or false) to use the default classdef for the type.",
                        ex);
                }
            }
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
            RelationshipCol relCol = ((RelationshipDefCol) _relationshipDefCol).CreateRelationshipCol(propCol, bo);
            if (SuperClassClassDef != null)
            {
                ClassDef superClassClassDef = (ClassDef) SuperClassDef.SuperClassClassDef;
                relCol.Add(superClassClassDef.CreateRelationshipCol(propCol, bo));
            }
            return relCol;
        }

        /// <summary>
        /// Creates a new collection of BO keys using the properties provided
        /// </summary>
        /// <param name="col">The collection of properties</param>
        /// <returns>Returns the new key collection</returns>
        public BOKeyCol CreateBOKeyCol(IBOPropCol col)
        {
            BOKeyCol keyCol = _keysCol.CreateBOKeyCol(col);
            if (SuperClassClassDef != null)
            {
                ClassDef superClassClassDef = (ClassDef) SuperClassDef.SuperClassClassDef;

                keyCol.Add(superClassClassDef.CreateBOKeyCol(col));
            }
            return keyCol;
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
        public ISuperClassDef SuperClassDef { get; set; }

        /// <summary>
        /// Returns the class definition of the super-class, or null
        /// if there is no super-class
        /// </summary>
        public IClassDef SuperClassClassDef
        {
            get
            {
                return SuperClassDef == null ? null : SuperClassDef.SuperClassClassDef;
            }
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
                foreach (IClassDef def in ClassDefs)
                {
                    if (def.SuperClassDef != null && def.SuperClassDef.SuperClassClassDef == this)
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

                foreach (IClassDef def in immediateChildren)
                {
                    children.Add(def);
                    foreach (IClassDef child in def.AllChildren)
                    {
                        children.Add(child);
                    }
                }
                return children;
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
            return (SuperClassDef != null) && (SuperClassDef.ORMapping == ORMapping.ClassTableInheritance);
        }

        /// <summary>
        /// This parameter can be used to allow multiple classdefs for one .NET type, as long as the
        /// type parameter between the classdefs are different.
        /// </summary>
        public string TypeParameter { get; set; }

        /// <summary>
        /// Indicates whether ConcreteTableInheritance is being used. See
        /// the ORMapping enumeration for more detail.
        /// </summary>
        /// <returns>Returns true if so, or false if there is no
        /// super class or another type of inheritance is being used</returns>
        public bool IsUsingConcreteTableInheritance()
        {
            return
                (SuperClassDef != null) && (SuperClassDef.ORMapping == ORMapping.ConcreteTableInheritance);
        }

        /// <summary>
        /// Indicates whether SingleTableInheritance is being used. See
        /// the ORMapping enumeration for more detail.
        /// </summary>
        /// <returns>Returns true if so, or false if there is no
        /// super class or another type of inheritance is being used</returns>
        public bool IsUsingSingleTableInheritance()
        {
            return (SuperClassDef != null) && (SuperClassDef.ORMapping == ORMapping.SingleTableInheritance);
        }

        /// <summary>
        /// Creates a list of classdefs in this inheritance hierarchy.  The first item in the list is this classdef,
        /// followed by the one higher up the hierarchy (ie, this classdef's base or super class), followed by that
        /// one's base/super class and so on.
        /// </summary>
        /// <returns>The list of classdefs in order from lowest to highest in the hierarchy.</returns>
        public IList<ClassDef> GetAllClassDefsInHierarchy()
        {
            IList<ClassDef> classDefs = new List<ClassDef>();
            IClassDef tempClassDef = this;
            while (tempClassDef != null)
            {
                classDefs.Add((ClassDef) tempClassDef);
                tempClassDef = tempClassDef.SuperClassClassDef;
            }
            return classDefs;
        }

        #endregion //Superclasses&inheritance

        #region Returning Defs

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
/*            if (PropDefcol.Contains(propertyName))
            {
                return PropDefcol[propertyName].LookupList;
            }
            return SuperClassDef == null
                       ? new NullLookupList()
                       : SuperClassClassDef.GetLookupList(propertyName);*/
            PropDef propDef = (PropDef)GetPropDef(propertyName, false);
            return propDef == null
                       ? new NullLookupList()
                       : propDef.LookupList;
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
            if (!RelationshipDefCol.Contains(source.Name))
            {
                if (throwError)
                {
                    throw new ArgumentException("The ClassDef for " + ClassName +
                                                " does not contain a relationship with the name " +
                                                source.Name +
                                                " and thus cannot retrieve the PropDef through this relationship");
                }
                return null;
            }

            IClassDef relatedClassDef = RelationshipDefCol[source.Name].RelatedObjectClassDef;
            if (source.Joins.Count > 0)
            {
                return relatedClassDef.GetPropDef(source.Joins[0].ToSource, propertyName, throwError);
            }
            return relatedClassDef.GetPropDef(propertyName, throwError);
        }



        /// <summary>
        /// Searches the relationship definition collection and returns 
        /// the relationship definition found under the
        /// relationship with the name specified.
        /// This searches through all the entire inheritance Hierachy i.e. all the
        /// superclasses relationships are also searched for.
        /// </summary>
        /// <param name="relationshipName">The relationship name in question</param>
        /// <returns>Returns the relationship definition if found, 
        /// or null if not found</returns>
        public IRelationshipDef GetRelationship(string relationshipName)
        {
            IClassDef currentClassDef = this;
            while (currentClassDef != null)
            {
                if (currentClassDef.RelationshipDefCol.Contains(relationshipName))
                {
                    return currentClassDef.RelationshipDefCol[relationshipName];
                }
                currentClassDef = currentClassDef.SuperClassClassDef;
            }
            return null;
        }

        /// <summary>
        /// Searches the UI definition collection and returns 
        /// the UI definition found under the UI with the name specified.
        /// </summary>
        /// <param name="uiDefName">The UI name in question</param>
        /// <returns>Returns the UI definition if found, or null if not found</returns>
        public IUIDef GetUIDef(string uiDefName)
        {
            IClassDef currentClassDef = this;
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
            if (obj.GetType() != typeof (ClassDef)) return false;
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
            if (otherClsDef.TypeParameter != TypeParameter) return false;
            //This is a rough and ready equals test later need to improve
            if (PropDefcol == null) return false;
            if (PropDefcol.Count != otherClsDef.PropDefcol.Count)
            {
                return false;
            }
            foreach (PropDef def in PropDefcol)
            {
                if (!otherClsDef.PropDefcol.Contains(def.PropertyName))
                {
                    return false;
                }
            }
            if (this._className != otherClsDef._className) return false;
            return true;
            //return Equals(otherClsDef._className, _className) && Equals(otherClsDef._classType, _classType) && Equals(otherClsDef._primaryKeyDef, _primaryKeyDef) && Equals(otherClsDef._propDefCol, _propDefCol) && Equals(otherClsDef.TypeParameter, TypeParameter);
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
                result = (result*397) ^ (_classType != null ? _classType.GetHashCode() : 0);
                result = (result*397) ^ (_primaryKeyDef != null ? _primaryKeyDef.GetHashCode() : 0);
                result = (result*397) ^ (_propDefCol != null ? _propDefCol.GetHashCode() : 0);
                result = (result*397) ^ (TypeParameter != null ? TypeParameter.GetHashCode() : 0);
                return result;
            }
        }

        #endregion

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
        /// The ClassID that identifies this Class in the case where the class is loaded from a database.
        ///</summary>
        public Guid? ClassID { get; set; }

        ///<summary>
        /// The module name that identifies this class for the case of building a menu for the standard menu editor.
        ///</summary>
        public string Module { get; set; }

        #region IClassDef Members

        ///<summary>
        /// Gets the type of the specified property for this classDef.
        /// The specified property can also have a format like the custom properties for a UiGridColumn or UiFormField def.
        /// eg: MyRelatedBo.MyFurtherRelatedBo|MyAlternateRelatedBo.Name
        ///</summary>
        ///<param name="propertyName">The property to get the type for.</param>
        ///<returns>The type of the specified property</returns>
        public Type GetPropertyType(string propertyName)
        {
            if (IsReflectiveProperty(propertyName))
            {
                return GetReflectivePropertyType(propertyName);
            }
            PropDef propDef = (PropDef) GetPropDef(propertyName, false);
            return propDef != null ? propDef.PropertyType : typeof (object);
        }

        private static bool IsReflectiveProperty(string propertyName)
        {
            return propertyName.IndexOf("-") != -1;
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

        private IPropDef GetInheritedPropDef(string propertyName, bool throwError)
        {
/*            IPropDef foundPropDef = null;
            IClassDef currentClassDef = this;
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
            }*/

            try
            {
                return this.PropDefColIncludingInheritance[propertyName];
            }
            catch (Exception)
            {
                if (throwError)
                {
                    throw new InvalidPropertyNameException(String.Format(
                                                               "The property definition for the property '{0}' could not be " +
                                                               "found on a ClassDef of type '{1}'", propertyName,
                                                               ClassNameFull));
                }
            }

            return null;
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
            //If this prop def is from a related class e.g. this.Car.EngineID then
            // need to get the PropDef from the related class.
            return IsRelatedProperty(propertyName)
                       ? GetRelatedPropertyDef(propertyName, throwError)
                       : GetInheritedPropDef(propertyName, throwError);
        }

        private static bool IsRelatedProperty(string propertyName)
        {
            return propertyName.IndexOf(".") != -1;
        }

        private IPropDef GetRelatedPropertyDef(string propertyName, bool throwError)
        {
            string relationshipName = propertyName.Substring(0, propertyName.IndexOf("."));
            propertyName = propertyName.Remove(0, propertyName.IndexOf(".") + 1);
            //If there are some alternative relationships to traverse through then
            //  go through each alternative and check if there is a related object and return the first one
            // else get the related object

            string[] parts = relationshipName.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            var relNames = new List<string>(parts);
            foreach (string relationship in relNames)
            {
                IRelationshipDef relationshipDef = GetRelationship(relationship);
                if (relationshipDef == null) continue;

                IClassDef relatedObjectClassDef = relationshipDef.RelatedObjectClassDef;
                if (relatedObjectClassDef == null) continue;

                IPropDef propDef = relatedObjectClassDef.GetPropDef(propertyName, false);
                if (propDef != null) return propDef;
            }
            if (throwError)
            {
                ThrowPropertyNotFoundError(propertyName);
            }
            return null;
        }

        private void ThrowPropertyNotFoundError(string propertyName)
        {
            throw new InvalidPropertyNameException(String.Format(
                "The property definition for the property '{0}' could not be " +
                "found on a ClassDef of type '{1}'", propertyName,
                ClassNameFull));
        }

        private Type GetReflectivePropertyType(string propertyName)
        {
            string trimmedPropName = propertyName.Trim('-');
            return ReflectionUtilities.GetUndelyingPropertType(MyClassType, trimmedPropName);
        }

        ///<summary>
        /// Creates a property comparer for the given property
        /// The specified property can also have a format like the custom properties 
        /// for a UiGridColumn or UiFormField def.
        /// eg: MyRelatedBo.MyFurtherRelatedBo|MyAlternateRelatedBo.Name
        ///</summary>
        ///<param name="propertyName">The property to get the type for.</param>
        ///<returns>The type of the specified property</returns>
        public IPropertyComparer<T> CreatePropertyComparer<T>(string propertyName) where T : IBusinessObject
        {
            Type comparerType = typeof (PropertyComparer<,>);
            Type propertyType = GetPropertyType(propertyName);
            comparerType = comparerType.MakeGenericType(typeof (T), propertyType);
            var comparer = (IPropertyComparer<T>) Activator.CreateInstance(comparerType, propertyName);
            return comparer;
        }

        /// <summary>
        /// Returns the table name for this class
        /// </summary>
        /// <returns>Returns the table name of first real table for this class.</returns>
        public string GetTableName()
        {
            if (!string.IsNullOrEmpty(_cachedTableName))
            {
                return _cachedTableName;
            }
            if (IsUsingSingleTableInheritance())
            {
                IClassDef superClassClassDef = SuperClassClassDef;
                if (superClassClassDef != null)
                {
                    _cachedTableName = superClassClassDef.GetTableName();
                    return _cachedTableName;
                }
            }
            _cachedTableName = TableName;
            return _cachedTableName;
        }


        /// <summary>
        /// Returns the table name of the specified property
        /// </summary>
        /// <param name="propDef">The property to fine the table name for</param>
        /// <returns>Returns the table name of the table that the specified property belongs to</returns>
        public string GetTableName(IPropDef propDef)
        {
            if (IsUsingConcreteTableInheritance() || propDef.ClassDef == null)
            {
                return TableName;
            }
            return propDef.ClassDef.GetTableName();
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
            IPropDefCol propDefClone = PropDefcol != null ? PropDefcol.Clone(clonePropDefs) : null;
            UIDefCol uiDefClone = UIDefCol != null ? UIDefCol.Clone() : null;
            var newClassDef = new ClassDef(AssemblyName, ClassName, PrimaryKeyDef,
                                           propDefClone, KeysCol,
                                           RelationshipDefCol, uiDefClone);
            newClassDef.TableName = TableName;
            newClassDef.DisplayName = DisplayName;

            return newClassDef;
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
                currentClassDef = (ClassDef) currentClassDef.SuperClassClassDef;
            }
            return currentClassDef;
        }


        ///<summary>
        /// Returns the Class Definition for a <see cref="IBusinessObject"/> of type T.
        ///</summary>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        public static ClassDef Get<T>() where T : class, IBusinessObject
        {
            return (ClassDef) ClassDefs[typeof (T)];
        }
    }
}