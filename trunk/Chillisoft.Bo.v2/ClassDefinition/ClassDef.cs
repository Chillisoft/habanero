using System;
using System.Collections;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
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
        private static readonly ILog log = LogManager.GetLogger("Chillisoft.Bo.ClassDefinition.v2.ClassDef");
        protected static ClassDefCol mClassDefCol;
        protected string mDatabaseName = "Default";

        protected string mTableName = "";
        protected readonly Type mClassType;
        protected PropDefCol mPropDefCol;
        protected string mSelectSql = "";
        protected KeyDefCol mKeysCol;
        protected PrimaryKeyDef mPrimaryKeyDef;
        protected RelationshipDefCol mRelationshipDefCol;
        protected bool mHasObjectID = true;
        private SuperClassDesc mSuperClassDesc;
        private UIDefCol itsUIDefCol;
        private bool itsSupportsSynchronisation;

        private static PropDef itsVersionNumberAtLastSyncPropDef =
            new PropDef("SyncVersionNumberAtLastSync", typeof (int), cbsPropReadWriteRule.ReadManyWriteMany, 0);

        private static PropDef itsVersionNumberPropDef =
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
                          UIDefCol uiDefCol
            )
        {
            mTableName = tableName;
            //mSelectSql = "SELECT * FROM " + tableName;
            mClassType = classType;
            mPropDefCol = propDefCol;
            mKeysCol = keysCol;
            mPrimaryKeyDef = primaryKeyDef;
            mRelationshipDefCol = relationshipDefCol;
            itsUIDefCol = uiDefCol;
            GetClassDefCol().Add(mClassType, this);

            //			mConcurrencyControl= concurrencyControl;
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
        /// As before, but excludes the table name
        /// </summary>
        public ClassDef(Type classType,
                        PrimaryKeyDef primaryKeyDef,
                        PropDefCol propDefCol,
                        KeyDefCol keysCol,
                        RelationshipDefCol relationshipDefCol,
                        UIDefCol uiDefCol) :
                            this(
                            classType, primaryKeyDef, "tb" + classType.Name, propDefCol, keysCol, relationshipDefCol,
                            uiDefCol)
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
            mDatabaseName = databaseName;
            mTableName = tableName;
            mClassType = classType;
            mPropDefCol = propDefCol;
            mKeysCol = keysCol;
            mPrimaryKeyDef = primaryKeyDef;
            mRelationshipDefCol = relationshipDefCol;
            itsUIDefCol = uiDefCol;
        }

        #endregion Constructors


        #region properties

        /// <summary>
        /// The name of the class definition
        /// </summary>
        public string ClassName
        {
            get { return mClassType.Name; }
        }

        /// <summary>
        /// The type of the class definition
        /// </summary>
        internal Type ClassType
        {
            get { return mClassType; }
        }

        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName
        {
            get
            {
                //log.Debug(mTableName.ToLower());
                return mTableName; //.ToLower() ;
            }
            set { mTableName = value; }
        }


//		public string SelectSql {
//			get {
//				return GetSelectSql(null);
//			}
//			set { mSelectSql = value; }
//		}

        /// <summary>
        /// The collection of property definitions
        /// </summary>
        public PropDefCol PropDefcol
        {
            get { return mPropDefCol; }
        }

        /// <summary>
        /// The collection of key definitions
        /// </summary>
        public KeyDefCol KeysCol
        {
            get { return mKeysCol; }
        }

        /// <summary>
        /// The primary key definition
        /// </summary>
        public PrimaryKeyDef PrimaryKeyDef
        {
            get { return mPrimaryKeyDef; }
        }

        /// <summary>
        /// Indicates if this object has an ID
        /// </summary>
        public bool HasObjectID
        {
            get { return mHasObjectID; }
            set { mHasObjectID = value; }
        }

        /// <summary>
        /// The collection of relationship definitions
        /// </summary>
        public RelationshipDefCol RelationshipDefCol
        {
            get { return mRelationshipDefCol; }
        }

        /// <summary>
        /// The collection of user interface definitions
        /// </summary>
        public UIDefCol UIDefCol
        {
            get { return itsUIDefCol; }
        }

        /// <summary>
        /// Indicates whether synchronising is supported
        /// </summary>
        public bool SupportsSynchronising
        {
            get { return itsSupportsSynchronisation; }
            set { itsSupportsSynchronisation = value; }
        }

        #endregion properties


        #region FactoryMethods

        /// <summary>
        /// Returns the collection of class definitions of which this is
        /// a part
        /// </summary>
        /// <returns>Returns a ClassDefCol object</returns>
        public static ClassDefCol GetClassDefCol()
        {
            if (mClassDefCol == null)
            {
                mClassDefCol = ClassDefCol.GetColClassDef();
            }
            return mClassDefCol;
        }

        /// <summary>
        /// Indicates whether the specified class type is amongst those
        /// contained in the class definition collection
        /// </summary>
        /// <param name="classTye">The class type in question</param>
        /// <returns>Returns true if found, false if not</returns>
        public static bool IsDefined(Type classType)
        {
            return GetClassDefCol().Contains(classType);
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
            BOPropCol propCol = mPropDefCol.CreateBOPropertyCol(newObject);
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
                propCol.Add(itsVersionNumberPropDef.CreateBOProp(newObject));
                propCol.Add(itsVersionNumberAtLastSyncPropDef.CreateBOProp(newObject));
            }
            return propCol;
        }

        /// <summary>
        /// Creates a new business object
        /// </summary>
        /// <returns>Returns a new business object</returns>
        internal BusinessObjectBase InstantiateBusinessObject()
        {
            return (BusinessObjectBase) Activator.CreateInstance(mClassType, true);
        }

        /// <summary>
        /// Creates a new business object using this class definition
        /// </summary>
        /// <returns>Returns a new business object</returns>
        internal BusinessObjectBase InstantiateBusinessObjectWithClassDef()
        {
            return (BusinessObjectBase) Activator.CreateInstance(mClassType, new object[] {this});
        }

        /// <summary>
        /// Creates a new business object using this class definition
        /// and the database connection provided
        /// </summary>
        /// <param name="conn">A database connection</param>
        /// <returns>Returns a new business object</returns>
        internal BusinessObjectBase InstantiateBusinessObjectWithClassDef(IDatabaseConnection conn)
        {
            return (BusinessObjectBase) Activator.CreateInstance(mClassType, new object[] {this, conn});
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
            if (mRelationshipDefCol == null)
            {
                return null;
            }
            RelationshipCol relCol = new RelationshipCol(bo);
            relCol = mRelationshipDefCol.CreateRelationshipCol(propCol, bo);
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
            BOKeyCol keyCol = mKeysCol.CreateBOKeyCol(col);
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

        #endregion //Creating BOs


        #region Superclasses & Inheritance

        /// <summary>
        /// Gets and sets the super-class of this class definition
        /// </summary>
        public SuperClassDesc SuperClassDesc
        {
            get { return mSuperClassDesc; }
            set { mSuperClassDesc = value; }
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
                if (!ClassDef.GetClassDefCol().Contains(classDef.mClassType))
                {
                    ClassDef.GetClassDefCol().Add(classDef.mClassType, classDef);
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