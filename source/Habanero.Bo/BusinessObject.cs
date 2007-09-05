using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
using Habanero.BO.SqlGeneration;
using Habanero.DB;
using Habanero.Base;
using Habanero.Util;
using log4net;


namespace Habanero.BO
{


    //public delegate void BusinessObjectUpdatedHandler(Object sender, BOEventArgs e);

    /// <summary>
    /// Provides a super-class for business objects. This class contains all
    /// the common functionality used by business objects.
    /// </summary>
    public class BusinessObject : ITransaction
    {



        
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.BusinessObject");

        public event EventHandler<BOEventArgs> Updated;
        public event EventHandler<BOEventArgs> Deleted;

        #region Fields

        private static Dictionary<string, WeakReference> _allLoadedBusinessObjects = new Dictionary<string, WeakReference>();

        //set object as new by default.
        private BOState _boState = new BOState();

        protected ClassDef _classDef;
        protected BOPropCol _boPropCol;
        protected BOKeyCol _keysCol;
        protected BOPrimaryKey _primaryKey;
        protected IRelationshipCol _relationshipCol;
        private IConcurrencyControl _concurrencyControl;
        private ITransactionLog _transactionLog;
        protected IDatabaseConnection _connection;

        #endregion //Fields

        #region Constructors

        /// <summary>
        /// Constructor to initialise a new business object
        /// </summary>
        protected internal BusinessObject() : this(DatabaseConnection.CurrentConnection)
        {
        }

        /// <summary>
        /// Constructor that specifies a database connection
        /// </summary>
        /// <param name="conn">A database connection</param>
        protected internal BusinessObject(IDatabaseConnection conn) : this((ClassDef) null, conn)
        {
        }

        /// <summary>
        /// Constructor that specifies a class definition
        /// </summary>
        /// <param name="def">The class definition</param>
        protected internal BusinessObject(ClassDef def) : this(def, DatabaseConnection.CurrentConnection)
        {
        }

        /// <summary>
        /// Constructor that specifies a class definition and a database
        /// connection
        /// </summary>
        /// <param name="def">The class definition</param>
        /// <param name="conn">A database connection</param>
        protected internal BusinessObject(ClassDef def, IDatabaseConnection conn)
        {
            Initialise(conn, def);
            Guid myID = Guid.NewGuid();
            _primaryKey.SetObjectID(myID);
            ClassDef currentClassDef = this.ClassDef;
            if (currentClassDef != null)
            {
                while (currentClassDef.IsUsingClassTableInheritance())
                {
                    this.InitialisePropertyValue(currentClassDef.SuperClassClassDef.PrimaryKeyDef.KeyName, myID);
                    currentClassDef = currentClassDef.SuperClassClassDef;
                }
            }
            AddToLoadedBusinessObjectCol(this);

        }

        private void Initialise(IDatabaseConnection conn, ClassDef def) {
            State.IsDeleted = false;
            State.IsDirty = false;
            State.IsEditing = false;
            State.IsNew = true;
            if (conn != null)
            {
                _connection = conn;
            }
            else
            {
                _connection = DatabaseConnection.CurrentConnection;
            }
            if (def != null) {
                _classDef = def;
            } else {
                _classDef = ClassDef.ClassDefs[this.GetType()];
            }

            ConstructFromClassDef(true);
        }

        /// <summary>
        /// Constructor that specifies a primary key ID
        /// </summary>
        /// <param name="id">The primary key ID</param>
        protected BusinessObject(BOPrimaryKey id) : this(id, DatabaseConnection.CurrentConnection)
        {
        }

        /// <summary>
        /// Constructor that specifies a primary key ID and a database connection
        /// </summary>
        /// <param name="id">The primary key ID</param>
        /// <param name="conn">A database connection</param>
        protected BusinessObject(BOPrimaryKey id, IDatabaseConnection conn)
        {
            //todo: Check if not already loaded in object manager if already loaded raise error
            //TODO: think about moving these to after load
            Initialise(conn, null);
            //_connection = conn;
            //SetIsNew(false);
            //SetIsDeleted(false);
            //SetIsDirty(false);
            //SetIsEditing(false);
            //ConstructFromClassDef(false);
            _primaryKey = id;
            if (!BOLoader.Instance.Load(this))
            {
                //If the item is not found then throw the appropriate exception
                throw (new BusinessObjectNotFoundException());
            }
            AddToLoadedBusinessObjectCol(this);
        }

        /// <summary>
        /// Constructor that specifies a search expression
        /// </summary>
        /// <param name="searchExpression">A search expression</param>
        protected BusinessObject(IExpression searchExpression)
            : this(searchExpression, DatabaseConnection.CurrentConnection)
        {
        }

        /// <summary>
        /// Constructor that specifies a search expression and a database
        /// connection
        /// </summary>
        /// <param name="searchExpression">A search expression</param>
        /// <param name="conn">A database connection</param>
        protected BusinessObject(IExpression searchExpression, IDatabaseConnection conn)
        {
            //todo: Check if not already loaded in object manager if already loaded raise error
            _connection = conn;
            State.IsNew = false;
            State.IsDeleted = false;
            State.IsDirty = false;
            State.IsEditing = false;
            ConstructFromClassDef(false);
            if (!BOLoader.Instance.Load(this, searchExpression))
            {
                //If the item is not found then throw the appropriate exception
                throw (new BusinessObjectNotFoundException());
            }
        }

        /// <summary>
        /// A destructor for the object
        /// </summary>
        ~BusinessObject()
        {
            AllLoaded().Remove(this.ID.ToString());
            if (_primaryKey.GetOrigObjectID().Length > 0)
            {
                if (AllLoaded().ContainsKey(_primaryKey.GetOrigObjectID()))
                {
                    AllLoaded().Remove(_primaryKey.GetOrigObjectID());
                }
            }
            ReleaseWriteLocks();
            ReleaseReadLocks();
        }

        #endregion //Constructors

        #region Business Object Loaders

        /// <summary>
        /// Constructs the class
        /// </summary>
        /// <param name="newObject">Whether the object is new or not</param>
        protected virtual void ConstructFromClassDef(bool newObject)
        {
            if (_classDef == null) _classDef = ConstructClassDef();
            if (_classDef == null)
            {
                throw new NullReferenceException(String.Format(
                    "An error occurred while loading the class definitions for " +
                    "'{0}'. Check that the class exists in that " +
                    "namespace and assembly and that there are corresponding " +
                    "class definitions for this class.", GetType()));
            }
            _boPropCol = _classDef.createBOPropertyCol(newObject);
            _keysCol = _classDef.createBOKeyCol(_boPropCol);
            ClassDef classDefToUseForPrimaryKey = _classDef;
            while (classDefToUseForPrimaryKey.SuperClassDef != null &&
                   classDefToUseForPrimaryKey.SuperClassDef.ORMapping == ORMapping.SingleTableInheritance)
            {
                classDefToUseForPrimaryKey = classDefToUseForPrimaryKey.SuperClassClassDef;
            }
            if ((classDefToUseForPrimaryKey.SuperClassDef == null) ||
                (classDefToUseForPrimaryKey.SuperClassDef.ORMapping == ORMapping.ConcreteTableInheritance) ||
                (_classDef.SuperClassDef.ORMapping == ORMapping.ClassTableInheritance))
            {
                _primaryKey = (BOPrimaryKey)classDefToUseForPrimaryKey.PrimaryKeyDef.CreateBOKey(_boPropCol);
            }
            else
            {
                _primaryKey =
                    (BOPrimaryKey)classDefToUseForPrimaryKey.SuperClassClassDef.PrimaryKeyDef.CreateBOKey(_boPropCol);
            }
            _relationshipCol = _classDef.CreateRelationshipCol(_boPropCol, this);
        }

        /// <summary>
        /// Constructs a class definition
        /// </summary>
        /// <returns>Returns a class definition</returns>
        protected virtual ClassDef ConstructClassDef()
        {
            if (ClassDef.ClassDefs.Contains(this.GetType()))
                return ClassDef.ClassDefs[this.GetType()];
            else
                return null;

        }

        /// <summary>
        /// Adds a weak reference so that the object will be cleaned up if no 
        /// other objects in the system access it and if the garbage collector 
        /// runs
        /// </summary>
        /// <param name="myBusinessObject">The business object</param>
        /// TODO ERIC - "will be cleaned" or "will not"?
        private static void AddToLoadedBusinessObjectCol(BusinessObject myBusinessObject)
        {

            AllLoaded().Add(myBusinessObject.ID.GetObjectId(),
                                                 new WeakReference(myBusinessObject));
        }

        /// <summary>
        /// Returns a Hashtable containing the loaded business objects
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<string, WeakReference> AllLoaded()
        {
            return _allLoadedBusinessObjects;
        }

        /// <summary>
        /// Clears the loaded objects collection
        /// </summary>
        internal static void ClearLoadedBusinessObjectBaseCol()
        {
            _allLoadedBusinessObjects.Clear();
        }

        #endregion //Business Object Loaders

        #region Properties

        /// <summary>
        /// Returns the ID
        /// </summary>
        public BOPrimaryKey ID
        {
            get { return _primaryKey; }
        }

        /// <summary>
        /// Returns the ID as a string
        /// </summary>
        /// <returns>Returns a string</returns>
        string ITransaction.StrID()
        {
            return ID.ToString();
        }

        /// <summary>
        /// Sets the concurrency control object
        /// </summary>
        /// <param name="concurrencyControl">The concurrency control</param>
        protected void SetConcurrencyControl(IConcurrencyControl concurrencyControl)
        {
            _concurrencyControl = concurrencyControl;
        }

        /// <summary>
        /// Returns an XML string that contains the changes in the object
        /// since the last persistance to the database
        /// </summary>
        internal string DirtyXML
        {
            get
            {
                return "<" + this.ClassName + " ID=" + this.ID + ">" +
                       _boPropCol.DirtyXml + "<" + this.ClassName + ">";
            }
        }

        /// <summary>
        /// Sets the transaction log to that specified
        /// </summary>
        /// <param name="transactionLog">A transaction log</param>
        protected void SetTransactionLog(ITransactionLog transactionLog)
        {
            _transactionLog = transactionLog;
        }

        /// <summary>
        /// Gets and sets the collection of relationships
        /// </summary>
        public IRelationshipCol Relationships
        {
            get { return _relationshipCol; }
            set { _relationshipCol = value; }
        }

        /// <summary>
        /// Returns the class definition
        /// </summary>
        public ClassDef ClassDef
        {
            get { return _classDef; }
        }

        /// <summary>
        /// Returns the class name as specified in the class definition
        /// </summary>
        protected internal string ClassName
        {
            get { return _classDef.ClassName; }
        }

        /// <summary>
        /// Returns the table name
        /// </summary>
        protected internal string TableName
        {
            get
            {
                ClassDef classDefToUseForPrimaryKey = _classDef;
                while (classDefToUseForPrimaryKey.SuperClassDef != null &&
                       classDefToUseForPrimaryKey.SuperClassDef.ORMapping == ORMapping.SingleTableInheritance)
                {
                    classDefToUseForPrimaryKey = classDefToUseForPrimaryKey.SuperClassClassDef;
                }
                if ((classDefToUseForPrimaryKey.SuperClassDef != null) &&
                    (classDefToUseForPrimaryKey.SuperClassDef.ORMapping == ORMapping.SingleTableInheritance))
                {
                    return classDefToUseForPrimaryKey.SuperClassClassDef.TableName;
                }
                else
                {
                    return classDefToUseForPrimaryKey.TableName;
                }
            }
        }

        /// <summary>
        /// Returns the collection of BOKeys
        /// </summary>
        /// <returns>Returns a BOKeyCol object</returns>
        internal BOKeyCol GetBOKeyCol()
        {
            return _keysCol;
        }

        /// <summary>
        /// Returns a string useful for debugging output
        /// </summary>
        /// <returns>Returns an output string</returns>
        internal string GetDebugOutput()
        {
            string output = "";
            output += "Type: " + this.GetType().Name + Environment.NewLine;
            foreach (DictionaryEntry entry in _boPropCol)
            {
                BOProp prop = (BOProp)entry.Value;
                output += prop.PropertyName + " - " + prop.PropertyValueString + Environment.NewLine;
            }
            return output;
        }

        #endregion //Properties

        #region Editing Property Values

        /// <summary>
        /// Sets the object's state into editing mode.  The original state can
        /// be restored with Restore() and changes can be committed to the
        /// database by calling Save().
        /// </summary>
        private void BeginEdit()
        {
            CheckNotEditing();
            CheckConcurrencyBeforeBeginEditing();
            State.IsEditing = true;
        }

        /// <summary>
        /// Checks whether editing is already taking place, in which case
        /// an exception is thrown
        /// </summary>
        /// <exception cref="EditingException">Thrown if editing is taking
        /// place</exception>
        /// TODO ERIC - this method can probably be combined
        /// back into BeginEdit()
        private void CheckNotEditing()
        {
            if (State.IsEditing)
            {
                throw new EditingException(ClassName, WhereClause(null), this);
            }
        }

        /// <summary>
        /// Returns the value under the property name specified
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the value if found</returns>
        public object GetPropertyValue(string propName)
        {
            if (Props.Contains(propName))
                return Props[propName].Value;
            else
                throw new InvalidPropertyNameException("Property '" + propName + "' does not exist on a business object of type '" +
                                                       this.GetType().Name + "'");
        }

        /// <summary>
        /// Returns the value under the property name specified
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns a string</returns>
        internal string GetPropertyValueString(string propName)
        {
            return Props[propName].PropertyValueString;
        }

        /// <summary>
        /// Returns the named property value as long as it is a Guid and is
        /// contained in the primary key
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the property value</returns>
        internal object GetPropertyValueToDisplay(string propName)
        {
            if (Props[propName].PropertyType == typeof(Guid) && this.GetPropertyValue(propName) != null &&
                !this.ID.Contains(propName))
            {
                Guid myGuid = (Guid) GetPropertyValue(propName);
                Dictionary<string, object> lookupList = this.ClassDef.GetLookupList(propName).GetLookupList();

                foreach (KeyValuePair<string, object> pair in lookupList) {
                    if (pair.Value == null) continue;
                    if (pair.Value is BusinessObject)
                    {
                        if (((BusinessObject)pair.Value)._primaryKey.GetGuid().Equals(myGuid)) {
                            return pair.Key;
                        }
                    }
                    else {
                        if (pair.Value.Equals(myGuid)) {
                            return pair.Key;
                        }
                    }
                }
                return myGuid;
            } else if (ClassDef.GetPropDef(propName).HasLookupList()) {
                Dictionary<string, object> lookupList = this.ClassDef.GetLookupList(propName).GetLookupList();
                foreach (KeyValuePair<string, object> pair in lookupList)
                {
                    if (pair.Value == null) continue;
                    if (pair.Value.Equals(GetPropertyValue(propName)))
                    {
                        return pair.Key;
                    } else if (pair.Value is BusinessObject) {
                        if (String.Compare(((BusinessObject) pair.Value).ID.ToString(), GetPropertyValueString(propName)) == 0)
                            return pair.Value.ToString();
                    }
                    
                }
                return GetPropertyValue(propName);
            }
            else
            {
                return GetPropertyValue(propName);
            }

        }

        /// <summary>
        /// Returns the property value as in GetPropertyValueToDisplay(), but
        /// returns the value as a string
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns a string</returns>
        internal string GetPropertyStringValueToDisplay(string propName)
        {
            object val = this.GetPropertyValueToDisplay(propName);
            if (val != null)
            {
                return val.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Sets a property value to a new value
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <param name="propValue">The new value to set to</param>
        public void SetPropertyValue(string propName, object propValue)
        {
            BOProp prop = Props[propName];
            if (prop == null)
            {
                throw new InvalidPropertyNameException(String.Format(
                    "The given property name '{0}' does not exist in the " +
                    "collection of properties for the class '{1}'.",
                    propName, ClassName));
            }

            if (!(propValue is Guid))
            {
                if (propValue is string && prop.PropertyType == typeof(Guid))
                {
                    try
                    {
                        Guid g = new Guid((string) propValue);
                        propValue = g;
                    }
                    catch (FormatException)
                    {
                        if (this.ClassDef.GetPropDef(propName).HasLookupList()) {
                            Dictionary<string, object> lookupList = this.ClassDef.GetPropDef(propName).LookupList.GetLookupList();
                            propValue = lookupList[(string)propValue];
                            if (propValue is BusinessObject) {
                                propValue = ((BusinessObject) (propValue))._primaryKey.GetGuid();
                            }
                        }
                    }
                }
                if (propValue != null && propValue.Equals(DBNull.Value) && prop.PropertyType == typeof(bool))
                {
                    propValue = false;
                }
            }
            if (prop.PropertyType.IsSubclassOf(typeof (CustomProperty)))
            {
                if (propValue != null && prop.PropertyType != propValue.GetType())
                {
                    propValue = System.Activator.CreateInstance(prop.PropertyType, new object[] {propValue, false});
                }
            }
            if (propValue is BusinessObject)
            {
                if (prop.PropertyType == typeof(Guid))
                    propValue = ((BusinessObject)propValue)._primaryKey.GetGuid();
                else propValue = ((BusinessObject)propValue).ID.ToString();
            } else if (propValue is string && ClassDef.GetPropDef(propName).HasLookupList()) {
                Dictionary<string, object> lookupList = this.ClassDef.GetPropDef(propName).LookupList.GetLookupList();
                if (lookupList.ContainsKey((string)propValue))
                    propValue = lookupList[(string)propValue];
                if (propValue is BusinessObject) {
                    propValue = ((BusinessObject) (propValue)).ID.ToString();
                }
            }
            // If the property will be changed by this set then
            // check if object is already editing (i.e. another property value has 
            // been changed if it is not then check that this object is still fresh
            // if the object is not fresh then throw appropriate exception.
            if (prop.Value != propValue)
            {
                if (!State.IsEditing)
                {
                    BeginEdit();
                }
                State.IsDirty = true;
                prop.Value = propValue;
                FireUpdatedEvent();
            }
        }

        /// <summary>
        /// The BOProps in this business object
        /// </summary>
        public BOPropCol Props
        {
            get
            {
                return _boPropCol;
            }
        }

        /// <summary>
        /// Sets an initial property value
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <param name="propValue">The value to initialise to</param>
        private void InitialisePropertyValue(string propName, object propValue)
        {
            BOProp prop = Props[propName];
            prop.Value = propValue;
        }

        ///// <summary>
        ///// Returns the property collection
        ///// </summary>
        ///// <returns>Returns a BOPropCol object</returns>
        //internal BOPropCol GetBOPropCol()
        //{
        //    return _boPropCol;
        //}

        ///// <summary>
        ///// Returns the property having the name specified
        ///// </summary>
        ///// <param name="propName">The property name</param>
        ///// <returns>Returns a BOProp object</returns>
        ///// <exception cref="InvalidPropertyNameException">Thrown if no
        ///// property exists by the name specified</exception>
        //protected internal BOProp GetBOProp(string propName)
        //{
        //    BOProp prop = _boPropCol[propName];
        //    if (prop == null)
        //    {
        //        throw new InvalidPropertyNameException("The property '" + propName + 
        //            "' for the class '" + ClassName + "' has not been declared " +
        //            "in the class definitions.  Add the property to the class " +
        //            "definitions and add the property to the class, or check that " +
        //            "spelling and capitalisation are correct.");
        //    }
        //    return prop;
        //}

        /// <summary>
        /// Indicates whether all of the property values are valid
        /// </summary>
        /// <param name="invalidReason">A string to modify with a reason
        /// for any invalid values</param>
        /// <returns>Returns true if all are valid</returns>
        protected internal bool IsValid(out string invalidReason)
        {
            return Props.IsValid(out invalidReason);
        }

        /// <summary>
        /// Indicates whether all of the property values are valid
        /// </summary>
        /// <returns>Returns true if all are valid</returns>
        protected internal bool IsValid()
        {
            string invalidReason;
            return Props.IsValid(out invalidReason);
        }

        /// <summary>
        /// The BOState object for this BusinessObject, which records the state information of the object
        /// </summary>
        public BOState State
        {
            get
            {
                return _boState;
            }
        }


        internal BOPrimaryKey PrimaryKey
        {
            set { _primaryKey = value; }
        }

        #endregion //Editing Property Values

        #region Persistance

        /// <summary>
        /// Returns the database connection
        /// </summary>
        /// <returns>Returns an IDbConnection object</returns>
        protected IDbConnection GetConnection()
        {
            return _connection.GetConnection();
        }

        /// <summary>
        /// Returns the database connection
        /// </summary>
        /// <returns>Returns an IDatabaseConnection object</returns>
        internal IDatabaseConnection GetDatabaseConnection()
        {
            return _connection;
        }

        /// <summary>
        /// Sets the database connection
        /// </summary>
        /// <param name="connection">The database connection to set to</param>
        internal void SetDatabaseConnection(IDatabaseConnection connection)
        {
            this._connection = connection;
        }

        /// <summary>
        /// Returns the database connection string
        /// </summary>
        protected virtual string ConnectionString
        {
            get { return _connection.ConnectionString; }
        }

        /// <summary>
        /// Extra preparation or steps to take out after loading the business
        /// object
        /// </summary>
        protected internal virtual void AfterLoad()
        {
        }


        /// <summary>
        /// Commits to the database any changes made to the object
        /// </summary>
        public void Save()
        {
            if (!BeforeSave())
            {
                return;
            }

            string reasonNotSaved = "";
            if (!(State.IsDeleted && State.IsNew))
            {
                if (State.IsDirty || State.IsNew)
                {
                    //log.Debug("Save - Object of type " + this.GetType().Name + " is dirty or new, saving.") ;
                    if (IsValid(out reasonNotSaved))
                    {
                        CheckPersistRules();
                        UpdatedConcurrencyControlProperties();
                        //GlobalRegistry.SynchronisationController.UpdateSynchronisationProperties(this);

                        BeforeUpdateToDB();
                        int numRowsUpdated;
                        ISqlStatementCollection statementCollection = GetPersistSql();
                        numRowsUpdated = _connection.ExecuteSql(statementCollection, null);
                        if (_transactionLog != null)
                        {
                            _transactionLog.RecordTransactionLog(this, WindowsIdentity.GetCurrent().Name);
                        }
                        if (numRowsUpdated == statementCollection.Count)
                        {
                            UpdateBusinessObjectBaseCol();
                            _boPropCol.BackupPropertyValues();
                        }
                        else
                        {
                            throw new DatabaseReadException(
                                "An Error occured while saving an object to the database. Please contact your system administrator",
                                "An Error occured while saving an object to the database: " + numRowsUpdated +
                                " were updated whereas " + statementCollection.Count +
                                " row(s) should have been updated ",
                                GetPersistSql().ToString(), this.GetDatabaseConnection().ErrorSafeConnectString());
                        }
                    }
                    else
                    {
                        throw (new BusObjectInAnInvalidStateException(reasonNotSaved));
                    }
                } //Isdirty || new
            }

            UpdateAfterSave();

            AfterSave();
        }

        /// <summary>
        /// Carries out updates to the object after changes have been
        /// committed to the database
        /// </summary>
        protected void UpdateAfterSave()
        {
            if (!State.IsDeleted)
            {
                if (AllLoaded().ContainsKey(ID.GetObjectNewID()))
                {
                    //System.Console.WriteLine("My line");//TODO ??
                }
                // set the flags back
                State.IsEditing = false;
                State.IsNew = false;
                if (!(_boPropCol == null))
                {
                    _boPropCol.setIsObjectNew(false);
                }
                State.IsDeleted = false;
                if (!AllLoaded().ContainsKey(ID.GetObjectId()))
                {
                    AddToLoadedBusinessObjectCol(this);
                }
            }
            else
            {
                //Set the object state to an invalid state since the object has been deleted
                State.IsEditing = false;
                State.IsNew = true;
                State.IsDeleted = true;
                //Remove this object from the collection of objects since is is now invalid
                AllLoaded().Remove(this.ID.ToString());
                FireDeleted();
            } //!IsDeleted
            State.IsDirty = false;
            State.IsEditing = false;
            ReleaseWriteLocks();
        }

        /// <summary>
        /// Steps to carry out before the Save() command is run
        /// </summary>
        /// <returns>Returns true</returns>
        /// TODO ERIC - what is meant to be returned? (in overridden methods)
        protected internal virtual bool BeforeSave()
        {
            return true;
        }

        /// <summary>
        /// Steps to carry out after the Save() command is run
        /// </summary>
        protected virtual void AfterSave()
        {
        }

        /// <summary>
        /// Steps to carry out before the object values are updated to the
        /// database
        /// </summary>
        /// TODO ERIC - this method surprises me - what about Save()
        /// and where is the UpdateToDB() method?
        protected virtual void BeforeUpdateToDB()
        {
        }

        /// <summary>
        /// Updates the business object collection with the new primary key
        /// if the object's primary key has been changed/edited
        /// </summary>
        protected void UpdateBusinessObjectBaseCol()
        {
            //No need to do anything if the object does not have an ID.
            if (!_classDef.HasObjectID)
            {
                //If the primary key has not changed then do nothing.
                if (_primaryKey.IsDirty)
                {
                    //If there was an id before then
                    if (_primaryKey.GetOrigObjectID().Length > 0)
                    {
                        //If the ID was not a temp objectId then remove it from the collection
                        if (!State.IsNew)
                        {
                            AllLoaded().Remove(_primaryKey.GetOrigObjectID());
                        }
                        //If the object with the new ID does not exist in the collection then 
                        // add it.
                        if (!AllLoaded().ContainsKey(this.ID.GetObjectId()))
                        {
                            AddToLoadedBusinessObjectCol(this);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cancel all edits made to the object since it was loaded from the 
        /// database or last saved to the database
        /// </summary>
        public void Restore()
        {
            _boPropCol.RestorePropertyValues();
            State.IsDeleted = false;
            State.IsEditing = false;
            State.IsDirty = false;
            ReleaseWriteLocks();
            FireUpdatedEvent();
        }

        /// <summary>
        /// Deletes the object
        /// </summary>
        /// TODO ERIC - clarify these comments
        public void Delete()
        {
            if (!State.IsEditing)
            {
                BeginEdit();
            }
            State.IsDirty = true;
            State.IsDeleted = true;
        }

        /// <summary>
        /// Calls the Updated() method
        /// </summary>
        private void FireUpdatedEvent()
        {
            if (this.Updated != null)
            {
                this.Updated(this, new BOEventArgs(this));
            }
        }

        /// <summary>
        /// Calls the Deleted() handler
        /// </summary>
        private void FireDeleted()
        {
            if (this.Deleted != null)
            {
                this.Deleted(this, new BOEventArgs(this));
            }
        }

        #endregion //Persistance

        #region Concurrency

        /// <summary>
        /// Checks a number of rules, including concurrency, duplicates and
        /// duplicate primary keys
        /// </summary>
        protected virtual void CheckPersistRules()
        {
            CheckConcurrencyBeforePersisting();
            CheckForDuplicatePrimaryKey();
            CheckForDuplicates();
        }

        /// <summary>
        /// Checks concurrency before persisting to the database
        /// </summary>
        protected virtual void CheckConcurrencyBeforePersisting()
        {
            if (!(_concurrencyControl == null))
            {
                _concurrencyControl.CheckConcurrencyBeforePersisting(this);
            }
        }

        /// <summary>
        /// Checks concurrency before getting an object from the object manager
        /// </summary>
        protected internal virtual void CheckConcurrencyOnGettingObjectFromObjectManager()
        {
            if (!(_concurrencyControl == null))
            {
                _concurrencyControl.CheckConcurrencyOnGettingObjectFromObjectManager(this);
            }
        }

        /// <summary>
        /// Checks concurrency before beginning to edit an object's values
        /// </summary>
        protected virtual void CheckConcurrencyBeforeBeginEditing()
        {
            if (!(_concurrencyControl == null))
            {
                _concurrencyControl.CheckConcurrencyBeforeBeginEditing(this);
            }
        }

        /// <summary>
        /// Updates the concurrency control properties
        /// </summary>
        protected virtual void UpdatedConcurrencyControlProperties()
        {
            if (!(_concurrencyControl == null))
            {
                _concurrencyControl.UpdatePropertiesWithLatestConcurrencyInfo();
            }
        }

        /// <summary>
        /// Releases read locks
        /// </summary>
        protected virtual void ReleaseReadLocks()
        {
            if (!(_concurrencyControl == null))
            {
                _concurrencyControl.ReleaseReadLocks();
            }
        }

        /// <summary>
        /// Releases write locks
        /// </summary>
        protected virtual void ReleaseWriteLocks()
        {
            if (!(_concurrencyControl == null))
            {
                _concurrencyControl.ReleaseWriteLocks();
            }
        }

        ///// <summary>
        ///// Increments the synchronisation version number
        ///// </summary>
        //public void IncrementVersionNumber()
        //{
        //    if (this.ClassDef.SupportsSynchronising && this.GetBOPropCol().Contains("SyncVersionNumber"))
        //    {
        //        BOProp syncVersionNoProp = this.GetBOProp("SyncVersionNumber");
        //        syncVersionNoProp.PropertyValue = (int)syncVersionNoProp.PropertyValue + 1;
        //    }
        //    else
        //    {
        //        throw new NotSupportedException("The Business Object of type " + this.GetType() +
        //                                        " does not support version number synchronising.");
        //    }
        //}

        #endregion //Concurrency

        #region Check Duplicates

        /// <summary>
        /// Checks for duplicates, throwing an exception if found
        /// </summary>
        /// <exception cref="BusObjDuplicateConcurrencyControlException">Thrown
        /// if duplicates are found</exception>
        protected void CheckForDuplicates()
        {
            if (_keysCol == null)
            {
                return;
            }

            if (!State.IsDeleted)
            {
                foreach (BOKey lBOKey in _keysCol)
                {
                    if (lBOKey.MustCheckKey())
                    {
                        SqlStatement checkDuplicateSql =
                            new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
                        checkDuplicateSql.Statement.Append(this.GetSelectSql());
                        string whereClause = " ( NOT (" + WhereClause(checkDuplicateSql) + ")) AND " +
                                             GetCheckForDuplicateWhereClause(lBOKey, checkDuplicateSql);
                        checkDuplicateSql.AppendCriteria(whereClause);
                        //string whereClause = " WHERE ( NOT " + WhereClause() +
                        //	") AND " + GetCheckForDuplicateWhereClause(lBOKey);
                        using (IDataReader dr = this._connection.LoadDataReader(checkDuplicateSql))
                        {
                            //_classDef.SelectSql + whereClause)) {  //)  DatabaseConnection.CurrentConnection.LoadDataReader
                            try
                            {
                                if (dr != null && dr.Read()) //Database object with these criteria already exists
                                {
                                    log.Error(String.Format("For key: {6}. Duplicate record error occurred for: " +
                                            "Class: {0}, Username: {1}, Machinename: {2}, " +
                                            "Time: {3}, Sql: {4}, Object: {5}",
                                            ClassName, WindowsIdentity.GetCurrent().Name, Environment.MachineName,
                                            DateTime.Now, whereClause, this, lBOKey.KeyDef.KeyNameForDisplay));
                                    
                                    string keyNameLead = "";
                                    if (lBOKey.KeyDef.KeyNameForDisplay != null && lBOKey.KeyDef.KeyNameForDisplay.Length > 0)
                                    {
                                        keyNameLead = lBOKey.KeyDef.KeyNameForDisplay + ": ";
                                    }

                                    if (lBOKey.KeyDef.Message != null)
                                    {
                                        throw new BusObjDuplicateConcurrencyControlException(
                                            keyNameLead + lBOKey.KeyDef.Message);
                                    }

                                    string propNames = "";
                                    foreach (BOProp prop in lBOKey.GetBOPropCol())
                                    {
                                        if (propNames.Length > 0) propNames += ", ";
                                        propNames += prop.PropertyName;
                                    }
                                    if (lBOKey.Count == 1)
                                    {
                                        throw new BusObjDuplicateConcurrencyControlException(
                                            keyNameLead + "A record already exists with that value for " +
                                            propNames + ".");
                                    }
                                    else
                                    {
                                        throw new BusObjDuplicateConcurrencyControlException(
                                            keyNameLead + "A record already exists with that combination " +
                                            "of values for: " + propNames + ".");
                                    }
                                }
                            }
                            finally
                            {
                                if (dr != null && !(dr.IsClosed))
                                {
                                    dr.Close();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks for duplicate primary keys, throwing an exception if
        /// found
        /// </summary>
        /// <exception cref="BusObjDuplicateConcurrencyControlException">Thrown
        /// if duplicates are found</exception>
        protected void CheckForDuplicatePrimaryKey()
        {
            //Only check if this does not have an object ID since an object id
            // is guaranteed to be unique
            if (!_classDef.HasObjectID && !State.IsDeleted)
            {
                //Only check if the primaryKey is 
                if (_primaryKey.MustCheckKey())
                {
                    SqlStatement checkSql = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
                    checkSql.Statement.Append(this.GetSelectSql());
                    string whereClause = " WHERE " + _primaryKey.DatabaseWhereClause(checkSql);
                    checkSql.Statement.Append(whereClause);

                    using (IDataReader dr = DatabaseConnection.CurrentConnection.LoadDataReader(checkSql))
                    {
                        //_classDef.SelectSql + whereClause)) {
                        try
                        {
                            if (dr.Read()) //Database object with these criteria already exists
                            {
                                log.Error(String.Format("Duplicate record error occurred on primary key for: " +
                                            "Class: {0}, Username: {1}, Machinename: {2}, " +
                                            "Time: {3}, Sql: {4}, Object: {5}",
                                            ClassName, WindowsIdentity.GetCurrent().Name, Environment.MachineName,
                                            DateTime.Now, whereClause, this));
                                
                                string propNames = "";
                                foreach (BOProp prop in _primaryKey.GetBOPropCol())
                                {
                                    if (propNames.Length > 0) propNames += ", ";
                                    propNames += prop.PropertyName;
                                }
                                if (_primaryKey.Count == 1)
                                {
                                    throw new BusObjDuplicateConcurrencyControlException(
                                        "A record already exists with that value for " +
                                        propNames + ".");
                                }
                                else
                                {
                                    throw new BusObjDuplicateConcurrencyControlException(
                                        "A record already exists with that combination " +
                                        "of values for: " + propNames + ".");
                                }
                            }
                        }
                        finally
                        {
                            if (dr != null & !(dr.IsClosed))
                            {
                                dr.Close();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates a "where" clause used to check for duplicate keys
        /// </summary>
        /// <param name="lBOKey">The business object key</param>
        /// <param name="sql">The sql statement used to generate and track
        /// parameters</param>
        /// <returns>Returns a string</returns>
        private string GetCheckForDuplicateWhereClause(BOKey lBOKey, SqlStatement sql)
        {
            if (lBOKey == null)
            {
                throw new InvalidKeyException("An error occurred because a " +
                    "BOKey argument was null.");
            }
            return lBOKey.DatabaseWhereClause(sql);
        }

        #endregion //Concurrency

        #region Sql Statements

        /// <summary>
        /// Parses the parameter sql information into the given search
        /// expression
        /// </summary>
        /// <param name="searchExpression">The search expression</param>
        internal void ParseParameterInfo(IExpression searchExpression)
        {
            foreach (BOProp prop in _boPropCol)
            {
               searchExpression.SetParameterSqlInfo(prop, _classDef.TableName);
            }
        }

        /// <summary>
        /// Returns the primary key's sql "where" clause
        /// </summary>
        /// <param name="sql">A sql statement used to generate and track
        /// parameters</param>
        /// <returns>Returns a string</returns>
        protected internal virtual string WhereClause(SqlStatement sql)
        {
            return _primaryKey.PersistedDatabaseWhereClause(sql);
        }

        /// <summary>
        /// Returns the super class's sql "where" clause
        /// </summary>
        /// <param name="sql">A sql statement used to generate and track
        /// parameters</param>
        /// <param name="subClassDef">The sub class</param>
        /// <returns>Returns a string</returns>
        protected internal virtual string WhereClauseForSuperClass(SqlStatement sql, ClassDef subClassDef)
        {
            BOKey msuperKey = BOPrimaryKey.GetSuperClassKey(subClassDef, this);
            return msuperKey.PersistedDatabaseWhereClause(sql);
        }

        /// <summary>
        /// Returns a sql "select" statement with an attached "where" clause
        /// </summary>
        /// <param name="selectSql">The sql statement used to generate and track
        /// parameters</param>
        /// <returns>Returns a string</returns>
        protected internal  virtual string SelectSqlStatement(SqlStatement selectSql)
        {
            string statement = SelectSqlWithNoSearchClause();
            if (statement.IndexOf(" WHERE ") == -1)
            {
                statement += " WHERE ";
            }
            else
            {
                statement += " AND ";
            }
            //			ClassDef currentClassDef = this.ClassDef ;
            //			while (currentClassDef.IsUsingClassTableInheritance()) {
            //				foreach (DictionaryEntry entry in currentClassDef.SuperClassClassDef.PrimaryKeyDef) {
            //					PropDef def = (PropDef) entry.Value;
            //					statement += currentClassDef.SuperClassClassDef.TableName + "." + def.FieldName;
            //					statement += " = " + currentClassDef.TableName + "." + def.FieldName;
            //					statement += " AND ";
            //				}
            //				currentClassDef = currentClassDef.SuperClassClassDef ;
            //			}
            statement += WhereClause(selectSql);
            return statement;
        }

        /// <summary>
        /// Generates a sql statement with no search clause
        /// </summary>
        /// <returns>Returns a sql string</returns>
        /// TODO ERIC - seems to add a "where" clause anyway
        protected virtual string SelectSqlWithNoSearchClause()
        {
            return new SelectStatementGenerator(this, this._connection).Generate(-1);
        }

        /// <summary>
        /// Returns a sql statement with no search clause but including a
        /// "where " (or "and " where appropriate) statement
        /// </summary>
        /// <returns>Returns a sql string</returns>
        /// TODO ERIC - if this uses above then it will have "where" already
        internal string SelectSqlWithNoSearchClauseIncludingWhere()
        {
            string basicSelect = SelectSqlWithNoSearchClause();
            if (basicSelect.IndexOf(" WHERE ") == -1)
            {
                basicSelect += " WHERE ";
            }
            else
            {
                basicSelect += " AND ";
            }
            return basicSelect;
        }

        /// <summary>
        /// Returns a set of sql statements needed to persist the data changes
        /// to the database
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        ISqlStatementCollection ITransaction.GetPersistSql()
        {
            return this.GetPersistSql();
        }

        protected internal ISqlStatementCollection GetPersistSql()
        {
            if (State.IsNew && !(State.IsDeleted))
            {
                return GetInsertSql();
            }
            else if (!(State.IsDeleted))
            {
                return GetUpdateSql();
            }
            else if (State.IsDeleted && !(State.IsNew))
            {
                return GetDeleteSql();
            }
            else
            {
                throw new HabaneroApplicationException("A serious error has occurred " +
                    "since a business object is in an invalid state.");
                //return null;
            }
        }

        /// <summary>
        /// Builds a "delete" sql statement list for this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        protected internal SqlStatementCollection GetDeleteSql()
        {
            SqlStatement deleteSql;
            SqlStatementCollection statementCollection = new SqlStatementCollection();
            deleteSql = new SqlStatement(this.GetConnection());
            deleteSql.Statement = new StringBuilder(@"DELETE FROM " + TableName +
                                                    " WHERE " + WhereClause(deleteSql));
            statementCollection.Add(deleteSql);
            ClassDef currentClassDef = this.ClassDef;
            while (currentClassDef.IsUsingClassTableInheritance())
            {
                deleteSql = new SqlStatement(this.GetConnection());
                deleteSql.Statement.Append("DELETE FROM " + currentClassDef.SuperClassClassDef.TableName + " WHERE " +
                                           WhereClauseForSuperClass(deleteSql, currentClassDef));
                statementCollection.Add(deleteSql);
                currentClassDef = currentClassDef.SuperClassClassDef;
            }
            return statementCollection;
        }

        /// <summary>
        /// Returns an "insert" sql statement list for inserting this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        protected internal SqlStatementCollection GetInsertSql()
        {
            InsertStatementGenerator gen = new InsertStatementGenerator(this, this.GetConnection());
            return gen.Generate();
        }

        /// <summary>
        /// Returns an "update" sql statement list for updating this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        protected internal SqlStatementCollection GetUpdateSql()
        {
            UpdateStatementGenerator gen = new UpdateStatementGenerator(this, this.GetConnection());
            return gen.Generate();
        }

        /// <summary>
        /// Returns a "select" sql statement string that is used to load this
        /// object from the database
        /// </summary>
        /// <param name="limit">The limit</param>
        /// <returns>Returns a sql string</returns>
        protected internal string GetSelectSql(int limit)
        {
            return new SelectStatementGenerator(this, this._connection).Generate(limit);
        }

        /// <summary>
        /// Returns a "select" sql statement string that is used to load this
        /// object from the database
        /// </summary>
        /// <returns>Returns a sql string</returns>
        protected internal string GetSelectSql()
        {
            return GetSelectSql(-1);
        }

        

        #endregion //Sql Statements

        #region Implement ITransaction

        /// <summary>
        /// Carries out additional steps once the transaction has been committed
        /// to the database
        /// </summary>
        void ITransaction.TransactionCommited()
        {
            _boPropCol.BackupPropertyValues();
            this.UpdateAfterSave();
        }

        /// <summary>
        /// Checks the persistance rules
        /// </summary>
        void ITransaction.CheckPersistRules()
        {
            this.CheckPersistRules();
        }

        /// <summary>
        /// Cancels the edit
        /// </summary>
        void ITransaction.TransactionCancelEdits()
        {
            this.Restore();
        }

        /// <summary>
        /// Rolls back the transactions. Does nothing in this implementation.
        /// </summary>
        void ITransaction.TransactionRolledBack()
        {
            //Do nothing
        }

        /// <summary>
        /// Returns the transaction ranking
        /// </summary>
        /// <returns>Returns zero</returns>
        /// TODO ERIC - what is a transaction ranking? some kind of priority
        /// scheme?
        int ITransaction.TransactionRanking()
        {
            return 0;
        }

        //TODO!!! This stuff needs to have a better solution - database operations 
        //performed in after apply edit, should be in the
        // same transaction as the applyedit.  Look at Transaction for more.
        /// <summary>
        /// Carries out additional steps before committing changes to the
        /// database
        /// </summary>
        void ITransaction.BeforeCommit()
        {
            string reasonNotSaved;
            if (IsValid(out reasonNotSaved))
            {
                this.BeforeSave();
                this.CheckPersistRules();
            }
            else
            {
                throw new UserException(reasonNotSaved);
            }
        }

        /// <summary>
        /// Carries out additional steps after committing changes to the
        /// database
        /// </summary>
        void ITransaction.AfterCommit()
        {
            this.AfterSave();
        }

        #endregion //ITransaction

    }
}
