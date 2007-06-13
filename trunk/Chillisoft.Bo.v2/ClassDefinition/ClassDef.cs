using System;
using System.Collections;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;
using log4net;

namespace Chillisoft.Bo.ClassDefinition.v2
{
    /// <summary>
    /// Defines the properties of a class that need to be managed at a 
    /// Business Object (BO) level.<br/><br/>
    /// Required properties are:
    /// <ul>
    /// <li>The primary key, which is the object identifier that uniquely
    ///     identifies the object in the database and object manager.
    ///     Note that under Object-Oriented development philosophy, this
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
        protected static ClassDefCol _ClassDefCol;
        private static readonly ILog log = LogManager.GetLogger("Chillisoft.Bo.ClassDefinition.v2.ClassDef");

		private string _AssemblyName;
		private string _ClassName;
		private Type _ClassType;
		private string _DatabaseName = "Default";
		//private string _SelectSql = "";
		private string _TableName = "";
		private bool _HasObjectID = true;
		private PrimaryKeyDef _PrimaryKeyDef;
		private PropDefCol _PropDefCol;
		private KeyDefCol _KeysCol;
		private RelationshipDefCol _RelationshipDefCol;

		private SuperClassDesc _SuperClassDesc;
        private UIDefCol _UIDefCol;
        private bool _SupportsSynchronisation;

        private static PropDef _VersionNumberAtLastSyncPropDef =
            new PropDef("SyncVersionNumberAtLastSync", typeof (int), cbsPropReadWriteRule.ReadManyWriteMany, 0);

        private static PropDef _VersionNumberPropDef =
            new PropDef("SyncVersionNumber", typeof (int), cbsPropReadWriteRule.ReadManyWriteMany, 0);


        #region Constructors

        /// <summary>
        /// Constructor to create a new class definition object
        /// </summary>
        /// <param name="classType">The type of the class definition</param>
        /// <param name="primaryKeyDef">The primary key definition</param>
        /// <param name="tableName">The name of the table</param>
        /// <param name="propDefCol">The collection of property definitions</param>
        /// <param name="keysCol">The collection of key definitions</param>
        /// <param name="relationshipDefCol">The collection of relationship definitions</param>
        /// <param name="uiDefCol">The collection of user interface definitions</param>
        internal ClassDef(Type classType,
                          PrimaryKeyDef primaryKeyDef,
                          string tableName,
                          PropDefCol propDefCol,
                          KeyDefCol keysCol,
                          RelationshipDefCol relationshipDefCol,
                          UIDefCol uiDefCol)
        {
            //_SelectSql = "SELECT * FROM " + tableName;
            _ClassType = classType;
			_ClassName = _ClassType.Name;
			_AssemblyName = _ClassType.Assembly.ManifestModule.ScopeName;
			if (tableName == null || tableName.Length == 0)
				_TableName = "tb" + _ClassName;
            else
				_TableName = tableName;
            _PropDefCol = propDefCol;
            _KeysCol = keysCol;
            _PrimaryKeyDef = primaryKeyDef;
            _RelationshipDefCol = relationshipDefCol;
            _UIDefCol = uiDefCol;
            GetClassDefCol.Add(this);

            //			mConcurrencyControl= concurrencyControl;
        }

        /// <summary>
        /// As before, but excludes the table name
        /// </summary>
        public ClassDef(Type classType,
                        PrimaryKeyDef primaryKeyDef,
                        PropDefCol propDefCol,
                        KeyDefCol keysCol,
                        RelationshipDefCol relationshipDefCol,
                        UIDefCol uiDefCol) :
                            this(
                            classType, primaryKeyDef, "", propDefCol, keysCol, relationshipDefCol,
                            uiDefCol)
        {
        }

        /// <summary>
        /// As before, but excludes the table name and UI def collection
        /// </summary>
        public ClassDef(Type classType,
                        PrimaryKeyDef primaryKeyDef,
                        PropDefCol propDefCol,
                        KeyDefCol keysCol,
                        RelationshipDefCol relationshipDefCol) :
                            this(classType, primaryKeyDef, propDefCol, keysCol, relationshipDefCol, new UIDefCol())
        {
        }

        /// <summary>
        /// As before, but allows a database name to be specified
        /// </summary>
        public ClassDef(string databaseName,
                        Type classType,
                        PrimaryKeyDef primaryKeyDef,
                        string tableName,
                        PropDefCol propDefCol,
                        KeyDefCol keysCol,
                        RelationshipDefCol relationshipDefCol) :
                            this(
                            databaseName, classType, primaryKeyDef, tableName, propDefCol, keysCol, relationshipDefCol,
                            new UIDefCol())
        {
        }

        /// <summary>
        /// As before, but allows a database name and a user interface 
        /// definition collection to be specified
        /// </summary>
        public ClassDef(string databaseName,
                        Type classType,
                        PrimaryKeyDef primaryKeyDef,
                        string tableName,
                        PropDefCol propDefCol,
                        KeyDefCol keysCol,
                        RelationshipDefCol relationshipDefCol,
                        UIDefCol uiDefCol)
        {
            _DatabaseName = databaseName;
            _TableName = tableName;
            _ClassType = classType;
			_ClassName = _ClassType.Name;
			_AssemblyName = _ClassType.Assembly.FullName;
			_PropDefCol = propDefCol;
            _KeysCol = keysCol;
            _PrimaryKeyDef = primaryKeyDef;
            _RelationshipDefCol = relationshipDefCol;
            _UIDefCol = uiDefCol;
        }

		/// <summary>
		/// As before, but excludes the table name
		/// </summary>
		public ClassDef(string assemblyName,
						string className,
						PrimaryKeyDef primaryKeyDef,
						PropDefCol propDefCol,
						KeyDefCol keysCol,
						RelationshipDefCol relationshipDefCol,
						UIDefCol uiDefCol)
		{
			_ClassType = null;
			_ClassName = className;
			_AssemblyName = assemblyName;
			_TableName = "tb" + _ClassName;
			_PropDefCol = propDefCol;
			_KeysCol = keysCol;
			_PrimaryKeyDef = primaryKeyDef;
			_RelationshipDefCol = relationshipDefCol;
			_UIDefCol = uiDefCol;
		}

        #endregion Constructors


        #region properties

		/// <summary>
		/// The name of the assembly for the class definition
		/// </summary>
		public string AssemblyName
		{
			get { return _AssemblyName; }
			protected set 
			{
				if (_AssemblyName != value)
				{
					_ClassType = null;
					_ClassName = null;
				}
				_AssemblyName = value;
			}
		}

		/// <summary>
		/// The name of the class definition
		/// </summary>
		public string ClassName
		{
			get { return _ClassName; }
			protected set
			{
				if(_ClassName != value)
					_ClassType = null;
				_ClassName = value;
			}
		}

		///<summary>
		/// The full name of the class definition.
		///</summary>
		public string ClassFullName
		{
			get { return ClassDefCol.GetTypeId(_AssemblyName, _ClassName); }
		}

        /// <summary>
        /// The type of the class definition
        /// </summary>
        internal Type ClassType
        {
            get { return getMyClassType; }
        }

        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName
        {
            get
            {
                //log.Debug(_TableName.ToLower());
                return _TableName; //.ToLower() ;
            }
            set { _TableName = value; }
        }


//		public string SelectSql {
//			get {
//				return GetSelectSql(null);
//			}
//			set { _SelectSql = value; }
//		}

        /// <summary>
        /// The collection of property definitions
        /// </summary>
        public PropDefCol PropDefcol
        {
            get { return _PropDefCol; }
			protected set { _PropDefCol = value; }
        }

        /// <summary>
        /// The collection of key definitions
        /// </summary>
        public KeyDefCol KeysCol
        {
			get { return _KeysCol; }
			protected set { _KeysCol = value; }
        }

        /// <summary>
        /// The primary key definition
        /// </summary>
        public PrimaryKeyDef PrimaryKeyDef
        {
            get { return _PrimaryKeyDef; }
			protected set { _PrimaryKeyDef = value; }
        }

        /// <summary>
        /// Indicates if this object has an ID
        /// </summary>
        public bool HasObjectID
        {
            get { return _HasObjectID; }
            set { _HasObjectID = value; }
        }

        /// <summary>
        /// The collection of relationship definitions
        /// </summary>
        public RelationshipDefCol RelationshipDefCol
        {
            get { return _RelationshipDefCol; }
			protected set { _RelationshipDefCol = value; }
        }

        /// <summary>
        /// The collection of user interface definitions
        /// </summary>
        public UIDefCol UIDefCol
        {
            get { return _UIDefCol; }
			protected set { _UIDefCol = value; }
        }

        /// <summary>
        /// Indicates whether synchronising is supported
        /// </summary>
        public bool SupportsSynchronising
        {
            get { return _SupportsSynchronisation; }
            set { _SupportsSynchronisation = value; }
        }

        #endregion properties


        #region FactoryMethods

        /// <summary>
        /// Returns the collection of class definitions of which this is
        /// a part
        /// </summary>
        /// <returns>Returns a ClassDefCol object</returns>
        public static ClassDefCol GetClassDefCol
        {
			get
			{
				if (_ClassDefCol == null)
				{
					_ClassDefCol = ClassDefCol.GetColClassDef();
				}
				return _ClassDefCol;
			}
        }

        /// <summary>
        /// Indicates whether the specified class type is amongst those
        /// contained in the class definition collection
        /// </summary>
        /// <param name="classTye">The class type in question</param>
        /// <returns>Returns true if found, false if not</returns>
        public static bool IsDefined(Type classType)
        {
            return GetClassDefCol.Contains(classType);
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
            BOPropCol propCol = _PropDefCol.CreateBOPropertyCol(newObject);
            if (this.SuperClassDesc != null)
            {
                propCol.Add(SuperClassDesc.SuperClassDef.createBOPropertyCol(newObject));
                if (this.SuperClassDesc.ORMapping == ORMapping.ConcreteTableInheritance)
                {
                    foreach (DictionaryEntry entry in SuperClassDesc.SuperClassDef.PrimaryKeyDef)
                    {
                        propCol.Remove((string) entry.Key);
                    }
                } 
                else if (this.SuperClassDesc.ORMapping == ORMapping.SingleTableInheritance)
                {
                    if (this.PrimaryKeyDef != null)
                    {
                        foreach (DictionaryEntry entry in this.PrimaryKeyDef)
                        {
                            propCol.Remove((string) entry.Key);
                        }
                    }
                }
            }
            if (this.SupportsSynchronising)
            {
                propCol.Add(_VersionNumberPropDef.CreateBOProp(newObject));
                propCol.Add(_VersionNumberAtLastSyncPropDef.CreateBOProp(newObject));
            }
            return propCol;
        }

        /// <summary>
        /// Creates a new business object
        /// </summary>
        /// <returns>Returns a new business object</returns>
        internal BusinessObjectBase InstantiateBusinessObject()
        {
            return (BusinessObjectBase) Activator.CreateInstance(getMyClassType, true);
        }

        /// <summary>
        /// Creates a new business object using this class definition
        /// </summary>
        /// <returns>Returns a new business object</returns>
        internal BusinessObjectBase InstantiateBusinessObjectWithClassDef()
        {
            return (BusinessObjectBase) Activator.CreateInstance(getMyClassType, new object[] {this});
        }

        /// <summary>
        /// Creates a new business object using this class definition
        /// and the database connection provided
        /// </summary>
        /// <param name="conn">A database connection</param>
        /// <returns>Returns a new business object</returns>
        internal BusinessObjectBase InstantiateBusinessObjectWithClassDef(IDatabaseConnection conn)
        {
            return (BusinessObjectBase) Activator.CreateInstance(getMyClassType, new object[] {this, conn});
        }

        /// <summary>
        /// Creates a new collection of relationships.
        /// </summary>
        /// <param name="propCol">The collection of properties</param>
        /// <param name="bo">The business object that owns this collection</param>
        /// <returns>Returns the new collection or null if no relationship 
        /// definition collection exists</returns>
        public RelationshipCol CreateRelationshipCol(BOPropCol propCol, BusinessObjectBase bo)
        {
            if (_RelationshipDefCol == null)
            {
                return null;
            }
            RelationshipCol relCol = new RelationshipCol(bo);
            relCol = _RelationshipDefCol.CreateRelationshipCol(propCol, bo);
            if (this.SuperClassDef != null)
            {
                relCol.Add(this.SuperClassDesc.SuperClassDef.CreateRelationshipCol(propCol, bo));
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
            BOKeyCol keyCol = _KeysCol.CreateBOKeyCol(col);
            if (this.SuperClassDef != null)
            {
                keyCol.Add(SuperClassDesc.SuperClassDef.createBOKeyCol(col));
            }
            return keyCol;
        }

        /// <summary>
        /// Creates a new business object using this class definition
        /// </summary>
        /// <returns>Returns the new object</returns>
        public BusinessObjectBase CreateNewBusinessObject()
        {
            return this.InstantiateBusinessObjectWithClassDef();
        }

        /// <summary>
        /// Creates a new business object using this class definition
        /// and a provided database connection
        /// </summary>
        /// <param name="conn">A database connection</param>
        /// <returns>Returns the new object</returns>
        public BusinessObjectBase CreateNewBusinessObject(IDatabaseConnection conn)
        {
            return this.InstantiateBusinessObjectWithClassDef(conn);
        }


    	protected Type getMyClassType
    	{
			get
			{
				//TODO error: What happens if the AssemblyName or Classname is null?
				if (_ClassType == null && _AssemblyName != null && _ClassName != null)
				{
					try
					{
						_ClassType = TypeLoader.LoadType(_AssemblyName, _ClassName);
					}
					catch (UnknownTypeNameException ex)
					{
						//TODO: Is this the correct thing to do?
						throw new UnknownTypeNameException("Unable to load the class type while " +
							"attempting to load a type from a class definition, given the 'assembly' as: '" +
							_AssemblyName + "', and the 'class' as: '" + _ClassName +
							"'. Check that the class exists in the given assembly name and " +
							"that spelling and capitalisation are correct.", ex);
						//_ClassType = null;
					}
				}
				return _ClassType;
			}
    	}

        #endregion //Creating BOs


        #region Superclasses & Inheritance

        /// <summary>
        /// Gets and sets the super-class of this class definition
        /// </summary>
        public SuperClassDesc SuperClassDesc
        {
            get { return _SuperClassDesc; }
            set { _SuperClassDesc = value; }
        }

        /// <summary>
        /// Returns the class definition of the super-class, or null
        /// if there is no super-class
        /// </summary>
        internal ClassDef SuperClassDef
        {
            get
            {
                if (this.SuperClassDesc != null)
                {
                    return this.SuperClassDesc.SuperClassDef;
                }
                else
                {
                    return null;
                }
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
            return (this.SuperClassDesc != null) && (this.SuperClassDesc.ORMapping == ORMapping.ClassTableInheritance);
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
                (this.SuperClassDesc != null) && (this.SuperClassDesc.ORMapping == ORMapping.ConcreteTableInheritance);
        }

        /// <summary>
        /// Indicates whether SingleTableInheritance is being used. See
        /// the ORMapping enumeration for more detail.
        /// </summary>
        /// <returns>Returns true if so, or false if there is no
        /// super class or another type of inheritance is being used</returns>
        public bool IsUsingSingleTableInheritance()
        {
            return (this.SuperClassDesc != null) && (this.SuperClassDesc.ORMapping == ORMapping.SingleTableInheritance);
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
            foreach (ClassDef classDef in loader.LoadClassDefs())
            {
                if (!ClassDef.GetClassDefCol.Contains(classDef))
                {
                    ClassDef.GetClassDefCol.Add(classDef);
                }
                else
                {
                    Console.Out.WriteLine("Attempted to load a class def when it was already defined.");
                }
            }
        }

        /// <summary>
        /// Searches the property definition collection and returns 
        /// the lookup-list found under the property with the
        /// name specified.  Also checks the super-class.
        /// </summary>
        /// <param name="propertyName">The property name in question</param>
        /// <returns>Returns the lookup-list if the property is
        /// found, or a NullLookupListSource object if not</returns>
        public ILookupListSource GetLookupListSource(string propertyName)
        {
            if (this.PropDefcol[propertyName] != null)
            {
                return this.PropDefcol[propertyName].LookupListSource;
            }
            else
            {
                if (this.SuperClassDesc != null)
                {
                    return this.SuperClassDef.GetLookupListSource(propertyName);
                }
                else
                {
                    return new NullLookupListSource();
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
                if (currentClassDef.RelationshipDefCol[relationshipName] != null)
                {
                    return currentClassDef.RelationshipDefCol[relationshipName];
                }
                else
                {
                    currentClassDef = currentClassDef.SuperClassDef;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Searches the property definition collection and returns the 
        /// property definition for the property with the name provided.
        /// </summary>
        /// <param name="propertyName">The property name in question</param>
        /// <returns>Returns the property definition if found, or
        /// null if not</returns>
        public PropDef GetPropDef(string propertyName)
        {
            ClassDef currentClassDef = this;
            while (currentClassDef != null)
            {
                if (currentClassDef.PropDefcol[propertyName] != null)
                {
                    return currentClassDef.PropDefcol[propertyName];
                }
                else
                {
                    currentClassDef = currentClassDef.SuperClassDef;
                }
            }
            return null;
        }

        #endregion //Returning defs
    }
}