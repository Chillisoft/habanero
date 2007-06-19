using System;
using System.Collections;
using System.Data;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.CriteriaManager.v2;
using Chillisoft.Bo.SqlGeneration.v2;
using Chillisoft.Db.v2;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;
using log4net;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// An enumeration that describes the object's state
    /// </summary>
    [Flags()]
    public enum States
    {
        /// <summary>The object is new</summary>
        isNew = 1,
        /// <summary>The object has changed since its last persistance to
        /// the database</summary>
        isDirty = 2,
        /// <summary>The object has been deleted</summary>
        isDeleted = 4,
        /// <summary>The object is being edited</summary>
        isEditing = 8,
        // isIndependent = 32  //TODO: True if this Status is independent of its parent for editing
    }

    public delegate void BusinessObjectUpdatedHandler(Object sender, BOEventArgs e);

    /// <summary>
    /// Provides a super-class for business objects. This class contains all
    /// the common functionality used by business objects.
    /// </summary>
    public abstract class BusinessObject : ITransaction, Synchronisable, Generic.v2.BusinessObject
    {
        private static readonly ILog log = LogManager.GetLogger("Chillisoft.Bo.v2.BusinessObject");

        public event BusinessObjectUpdatedHandler Updated;
        public event BusinessObjectUpdatedHandler Deleted;

        #region Fields

        protected static Hashtable _businessObjectBaseCol;

        //set object as new by default.
        protected States _flagState = States.isNew; //TODO: | BOFlagState.isIndependent;

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
            SetIsDeleted(false);
            SetIsDirty(false);
            SetIsEditing(false);
            SetIsNew(true);
            _classDef = def;
            ConstructClass(true);
            Guid myID = Guid.NewGuid();
            _primaryKey.SetObjectID(myID);
            ClassDef currentClassDef = this.ClassDef;
            if (currentClassDef != null)
            {
                while (currentClassDef.IsUsingClassTableInheritance())
                {
                    this.InitialisePropertyValue(currentClassDef.SuperClassDef.PrimaryKeyDef.KeyName, myID);
                    currentClassDef = currentClassDef.SuperClassDef;
                }
            }
            if (conn != null)
            {
                _connection = conn;
            }
            else
            {
                _connection = DatabaseConnection.CurrentConnection;
            }
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
            _connection = conn;
            SetIsNew(false);
            SetIsDeleted(false);
            SetIsDirty(false);
            SetIsEditing(false);
            ConstructClass(false);
            _primaryKey = id;
            if (!Load())
            {
                //If the item is not found then throw the appropriate exception
                throw (new BusinessObjectNotFoundException());
            }
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
            SetIsNew(false);
            SetIsDeleted(false);
            SetIsDirty(false);
            SetIsEditing(false);
            ConstructClass(false);
            if (!Load(searchExpression))
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
            GetLoadedBusinessObjectBaseCol().Remove(this.ID);
            if (_primaryKey.GetOrigObjectID().Length > 0)
            {
                if (GetLoadedBusinessObjectBaseCol().Contains(_primaryKey.GetOrigObjectID()))
                {
                    GetLoadedBusinessObjectBaseCol().Remove(_primaryKey.GetOrigObjectID());
                }
            }
            ReleaseWriteLocks();
            ReleaseReadLocks();
        }

        #endregion //Constructors

        #region Business Object Loaders

        /// <summary>
        /// Loads a business object that meets the specified search criteria
        /// </summary>
        /// <param name="searchExpression">The search expression</param>
        /// <returns>Returns a business object, or null if none is found that
        /// meets the criteria</returns>
        [ReflectionPermission(SecurityAction.Demand)]
        protected internal BusinessObject GetBusinessObject(IExpression searchExpression)
        {
			BusinessObject lTempBusObj = _classDef.InstantiateBusinessObject();
            //BusinessObject lTempBusObj = (BusinessObject) Activator.CreateInstance(_classDef.ClassType, true);
            lTempBusObj.SetDatabaseConnection(this.GetDatabaseConnection());
            IDataReader dr = lTempBusObj.LoadDataReader(this.GetDatabaseConnection(), searchExpression);
            try
            {
                if (dr.Read())
                {
                    return GetBusinessObject(dr);
                }
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a new object loaded from the data reader or one from the 
        /// object manager that is refreshed with data from the data reader
        /// </summary>
        /// <param name="dr">A data reader pointing at a valid record</param>
        /// <returns>A valid business object for the data in the 
        /// data reader</returns>
        [ReflectionPermission(SecurityAction.Demand)]
        protected internal BusinessObject GetBusinessObject(IDataRecord dr)
        {
            // This method creates a primary key object with the data from the 
            // datareader and then checks if this is loaded, if it is then the 
            // properties for the object are reloaded from the datareader else 
            // a new object is created and the data for it is loaded from the 
            // datareader. The object is then added to the object manager.
            BOProp prop;
            BOPropCol propCol = new BOPropCol();
            BOPrimaryKey lPrimaryKey;
            foreach (DictionaryEntry item in  _classDef.PrimaryKeyDef)
            {
                PropDef lPropDef = (PropDef) item.Value;
                prop = lPropDef.CreateBOProp(false);

                prop.InitialiseProp(dr[prop.DatabaseFieldName]);
                propCol.Add(prop);

                _primaryKey = (BOPrimaryKey) _classDef.PrimaryKeyDef.CreateBOKey(_boPropCol);
            }
            lPrimaryKey = (BOPrimaryKey) _classDef.PrimaryKeyDef.CreateBOKey(propCol);

            BusinessObject lTempBusObj =
                BusinessObject.GetLoadedBusinessObject(lPrimaryKey.GetObjectId(), false);
            bool isReplacingSuperClassObject = false;
            if (lTempBusObj != null && this.GetType().IsSubclassOf(lTempBusObj.GetType()))
            {
                isReplacingSuperClassObject = true;
            }
            if (lTempBusObj == null || isReplacingSuperClassObject)
            {
                lTempBusObj = (this.CreateNewBusinessObject());
                // BusinessObject)Activator.CreateInstance(_classDef.ClassType, true);
                lTempBusObj.LoadFromDataReader(dr);
                try
                {
                    if (isReplacingSuperClassObject)
                    {
                        GetLoadedBusinessObjectBaseCol().Remove(lTempBusObj.ID.GetObjectId());
                    }
                    GetLoadedBusinessObjectBaseCol().Add(lTempBusObj.ID.GetObjectId(), new WeakReference(lTempBusObj));
                }
                catch (Exception ex)
                {
                    throw new BaseApplicationException("The object with id " +
                                                       lTempBusObj.ID.GetObjectId(), ex);
                }
            }
            else if (lTempBusObj.GetType().IsSubclassOf(this.GetType()))
            {
                //TODO - refresh this subclass object.  It can be done using the current datareader because
                //the current data reader is for an object of the superclass type.
            }
            else
            {
                if (lTempBusObj.IsDirty)
                {
                    log.Debug(
                        "An attempt was made to load an object already loaded that was in edit mode.  Refresh from database ignored." +
                        Environment.NewLine +
                        "BO Type: " + lTempBusObj.GetType().Name + Environment.NewLine + " Stack Trace: " +
                        Environment.StackTrace);
                }
                else
                {
                    lTempBusObj.LoadProperties(dr);
                }
            }
            return lTempBusObj;
        }

        //TODO:Peter - make a better load that doesn't use a bo col.
        /// <summary>
        /// Returns the business object of the type provided and that meets the
        /// search criteria
        /// </summary>
        /// <param name="criteria">The search criteria</param>
        /// <param name="boType">The business object type</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object
        /// matches the criteria</exception>
        /// TODO ERIC - i don't understand why the exception is thrown, since
        /// the normal pattern is just to return the first one that meets
        /// expectations
        public static BusinessObject GetBusinessObject(string criteria, Type boType)
        {
            BusinessObjectCollection col = new BusinessObjectCollection(ClassDef.GetClassDefCol[boType]);
            col.Load(criteria, "");
            if (col.Count < 1)
            {
                return null;
            }
            else if (col.Count > 1)
            {
                throw new UserException("Loading a " + boType.Name + " with criteria " + criteria +
                                        " returned more than one record when only one was expected.");
            }
            else
            {
                return col[0];
            }
        }

        /// <summary>
        /// Creates a new business object from the class definition
        /// </summary>
        /// <returns></returns>
        public BusinessObject CreateNewBusinessObject()
        {
            return _classDef.InstantiateBusinessObject();
        }

        /// <summary>
        /// Constructs the class
        /// </summary>
        /// <param name="newObject">Whether the object is new or not</param>
        protected virtual void ConstructClass(bool newObject)
        {
            _classDef = ConstructClassDef();
            _boPropCol = _classDef.createBOPropertyCol(newObject);
            _keysCol = _classDef.createBOKeyCol(_boPropCol);
            ClassDef classDefToUseForPrimaryKey = _classDef;
            while (classDefToUseForPrimaryKey.SuperClassDesc != null &&
                   classDefToUseForPrimaryKey.SuperClassDesc.ORMapping == ORMapping.SingleTableInheritance)
            {
                classDefToUseForPrimaryKey = classDefToUseForPrimaryKey.SuperClassDef;
            }
            if ((classDefToUseForPrimaryKey.SuperClassDesc == null) ||
                (classDefToUseForPrimaryKey.SuperClassDesc.ORMapping == ORMapping.ConcreteTableInheritance) ||
                (_classDef.SuperClassDesc.ORMapping == ORMapping.ClassTableInheritance))
            {
                _primaryKey = (BOPrimaryKey)classDefToUseForPrimaryKey.PrimaryKeyDef.CreateBOKey(_boPropCol);
            }
            else
            {
                _primaryKey =
                    (BOPrimaryKey)classDefToUseForPrimaryKey.SuperClassDef.PrimaryKeyDef.CreateBOKey(_boPropCol);
            }
            _relationshipCol = _classDef.CreateRelationshipCol(_boPropCol, this);
        }

        /// <summary>
        /// Constructs a class definition
        /// </summary>
        /// <returns>Returns a class definition</returns>
        protected virtual ClassDef ConstructClassDef()
        {
            if (ClassDef.GetClassDefCol.Contains(this.GetType()))
                return ClassDef.GetClassDefCol[this.GetType()];
            else
                return null;

        }
        
        /// <summary>
        /// Loads a business object by ID
        /// </summary>
        /// <param name="id">The ID</param>
        /// <returns>Returns a business object</returns>
        protected static BusinessObject GetLoadedBusinessObject(BOPrimaryKey id)
        {
            return GetLoadedBusinessObject(id.GetObjectId());
        }

        /// <summary>
        /// Loads a business object with the specified ID
        /// </summary>
        /// <param name="id">The ID</param>
        /// <returns>Returns a business object</returns>
        protected static BusinessObject GetLoadedBusinessObject(string id)
        {
            return GetLoadedBusinessObject(id, true);
        }

        /// <summary>
        /// Loads a business object with the specified ID from a collection
        /// or loaded objects
        /// </summary>
        /// <param name="id">The ID</param>
        /// <param name="refreshIfReqNotCurrent">Whether to check for
        /// object concurrency at the time of loading</param>
        /// <returns>Returns a business object</returns>
        /// TODO ERIC - review refresh above
        /// - review how loaded object collection works and what it does
        protected static BusinessObject GetLoadedBusinessObject(string id, bool refreshIfReqNotCurrent)
        {
            //If the object is already in loaded then refresh it and return it if required.
            if (GetLoadedBusinessObjectBaseCol().Contains(id))
            {
                BusinessObject lBusinessObject;
                WeakReference weakRef = (WeakReference) GetLoadedBusinessObjectBaseCol()[id];
                //If the reference is valid return object else remove object from 
                // Collection
                if (weakRef.IsAlive && weakRef.Target != null)
                {
                    lBusinessObject = (BusinessObject) weakRef.Target;
                    //Apply concurrency Control Strategy to the Business Object
                    if (refreshIfReqNotCurrent)
                    {
                        lBusinessObject.CheckConcurrencyOnGettingObjectFromObjectManager();
                    }
                    return lBusinessObject;
                }
                else
                {
                    GetLoadedBusinessObjectBaseCol().Remove(id);
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the business object from the loaded object collection
        /// </summary>
        /// <returns>Returns a business object</returns>
        protected internal BusinessObject GetLoadedBusinessObject()
        {
            BusinessObject busObj = GetLoadedBusinessObject(ID);
            if (busObj == null)
            {
                AddToLoadedBusinessObjectCol(this);
                return this;
            }
            return busObj;
        }

        /// <summary>
        /// Returns the business object from the loaded object collection
        /// </summary>
        /// <param name="dr">The IDataReader object</param>
        /// <returns>Returns a business object</returns>
        /// TODO ERIC - the code is identical to above and param not used
        protected internal BusinessObject GetLoadedBusinessObject(IDataReader dr)
        {
            BusinessObject busObj = GetLoadedBusinessObject(ID);
            if (busObj == null)
            {
                AddToLoadedBusinessObjectCol(this);
                return this;
            }
            return busObj;
        }

        /// <summary>
        /// Adds a weak reference so that the object will be cleaned up if no 
        /// other objects in the system access it and if the garbage collector 
        /// runs
        /// </summary>
        /// <param name="myBusinessObject">The business object</param>
        /// TODO ERIC - "will be cleaned" or "will not"?
        protected static void AddToLoadedBusinessObjectCol(BusinessObject myBusinessObject)
        {
            GetLoadedBusinessObjectBaseCol().Add(myBusinessObject.ID.GetObjectId(),
                                                 new WeakReference(myBusinessObject));
        }

        /// <summary>
        /// Returns a Hashtable containing the loaded business objects
        /// </summary>
        /// <returns></returns>
        protected internal static Hashtable GetLoadedBusinessObjectBaseCol()
        {
            if (_businessObjectBaseCol == null)
            {
                _businessObjectBaseCol = new Hashtable();
            }
            return _businessObjectBaseCol;
        }

        /// <summary>
        /// Clears the loaded objects collection
        /// </summary>
        protected internal static void ClearLoadedBusinessObjectBaseCol()
        {
            _businessObjectBaseCol = null;
            //TODO_Future: write to a log file since this 
            //   should only be allowed to be called in test mode.
        }

        /// <summary>
        /// Returns a business object collection with objects that meet the
        /// given search criteria, ordered as specified
        /// </summary>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <returns>Returns a business object collection</returns>
        public virtual BusinessObjectCollection GetBusinessObjectCol(string searchCriteria,
                                                                         string orderByClause)
        {
            BusinessObjectCollection bOCol = new BusinessObjectCollection(_classDef);
            bOCol.Load(searchCriteria, orderByClause);
            return bOCol;
        }

        /// /// <summary>
        /// Returns a business object collection with objects that meet the
        /// given search expression, ordered as specified
        /// </summary>
        /// <param name="searchExpression">The search expression</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <returns>Returns a business object collection</returns>
        public virtual BusinessObjectCollection GetBusinessObjectCol(IExpression searchExpression,
                                                                         string orderByClause)
        {
            BusinessObjectCollection bOCol = new BusinessObjectCollection(_classDef);
            bOCol.Load(searchExpression, orderByClause);
            return bOCol;
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
        public string StrID()
        {
            return this.ID.ToString();
        }

        /// <summary>
        /// Returns the primary key object
        /// </summary>
        public BOPrimaryKey PrimaryKey
        {
            get { return _primaryKey; }
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
        public string DirtyXML
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

        //		private Relationship GetRelationship(string relationshipName) 
        //		{
        //			//TODO: sensible errors if relationship not configured.
        //			return _relationshipCol[relationshipName];
        //		}
        //
        //		/// <summary>
        //		/// TODO: think about names for these
        //		/// </summary>
        //		/// <param name="relationshipName"></param>
        //		/// <returns></returns>
        //		public BusinessObject GetRelatedBusinessObject(string relationshipName) 
        //		{
        //			ArgumentValidationHelper.CheckStringArgumentNotEmpty(relationshipName, "relationshipName");
        //			if (!(_relationshipCol == null)) 
        //			{
        //				SingleRelationship rel = (SingleRelationship) GetRelationship(relationshipName);
        //				if (!(rel == null)) 
        //				{
        //					return rel.GetRelatedObject(this.GetDatabaseConnection() );
        //				}
        //			}
        //			return null;
        //		}
        //
        //		/// <summary>
        //		/// TODO: thinkg about names for these
        //		/// </summary>
        //		/// <param name="relationshipName"></param>
        //		/// <returns></returns>
        //		public BusinessObjectBaseCollection GetRelatedBusinessObjectCol(string relationshipName) 
        //		{
        //			ArgumentValidationHelper.CheckStringArgumentNotEmpty(relationshipName, "relationshipName");
        //			if (!(_relationshipCol == null)) 
        //			{
        //				MultipleRelationship rel = (MultipleRelationship) GetRelationship(relationshipName);
        //				if (!(rel == null)) 
        //				{
        //					return rel.GetRelatedBusinessObjectCol();
        //				}
        //			}
        //			return null;
        //		}

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
                while (classDefToUseForPrimaryKey.SuperClassDesc != null &&
                       classDefToUseForPrimaryKey.SuperClassDesc.ORMapping == ORMapping.SingleTableInheritance)
                {
                    classDefToUseForPrimaryKey = classDefToUseForPrimaryKey.SuperClassDef;
                }
                if ((classDefToUseForPrimaryKey.SuperClassDesc != null) &&
                    (classDefToUseForPrimaryKey.SuperClassDesc.ORMapping == ORMapping.SingleTableInheritance))
                {
                    return classDefToUseForPrimaryKey.SuperClassDef.TableName;
                }
                else
                {
                    return classDefToUseForPrimaryKey.TableName;
                }
            }
        }

        /// <summary>
        /// Returns the logged-on Windows user name
        /// </summary>
        /// <returns>Returns the name as a string</returns>
        protected string CurrentLoggedOnUserName()
        {
            return WindowsIdentity.GetCurrent().Name;
        }

        /// <summary>
        /// Returns the logged-on Windows user name
        /// </summary>
        /// <returns>Returns the name as a string</returns>
        /// TODO ERIC - why is this exactly as above?
        protected string CurrentWindowsUserName()
        {
            return WindowsIdentity.GetCurrent().Name;
        }

        /// <summary>
        /// Returns the current machine name
        /// </summary>
        /// <returns>Returns the name as a string</returns>
        protected string CurrentMachineName()
        {
            return Environment.MachineName;
        }

        /// <summary>
        /// Returns the collection of BOKeys
        /// </summary>
        /// <returns>Returns a BOKeyCol object</returns>
        /// TODO: This shouldn't be public, but how do we test without that?.
        public BOKeyCol GetBOKeyCol()
        {
            return _keysCol;
        }

        /// <summary>
        /// Returns a string useful for debugging output
        /// </summary>
        /// <returns>Returns an output string</returns>
        public string GetDebugOutput()
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
        /// be restored with CancelEdit() and changes can be committed to the
        /// database by calling ApplyEdit().
        /// </summary>
        public void BeginEdit()
        {
            CheckNotEditing();
            CheckConcurrencyBeforeBeginEditing();
            IsEditing = true;
        }

        /// <summary>
        /// Checks whether editing is already taking place, in which case
        /// an exception is thrown
        /// </summary>
        /// <exception cref="EditingException">Thrown if editing is taking
        /// place</exception>
        /// TODO ERIC - this method can probably be combined
        /// back into BeginEdit()
        protected void CheckNotEditing()
        {
            if (IsEditing)
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
            return GetBOProp(propName).PropertyValue;
        }

        /// <summary>
        /// Returns the value under the property name specified
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns a string</returns>
        public string GetPropertyValueString(string propName)
        {
            return GetBOProp(propName).PropertyValueString;
        }

        /// <summary>
        /// Returns the named property value as long as it is a Guid and is
        /// contained in the primary key
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the property value</returns>
        public object GetPropertyValueToDisplay(string propName)
        {
            if (this.GetBOProp(propName).PropertyType == typeof (Guid) && this.GetPropertyValue(propName) != null &&
                !this.PrimaryKey.Contains(propName))
            {
                Guid myGuid = (Guid) GetPropertyValue(propName);
                StringGuidPair val = this.ClassDef.GetLookupListSource(propName).GetLookupList().FindByGuid(myGuid);
                if (val.Id.Equals(myGuid))
                {
                    return val.Str;
                }
                else
                {
                    return myGuid;
                }
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
        public string GetPropertyStringValueToDisplay(string propName)
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
            BOProp prop = GetBOProp(propName);
            if (!(propValue is Guid))
            {
                if (propValue is string)
                {
                    try
                    {
                        Guid g = new Guid((string) propValue);
                        propValue = g;
                    }
                    catch (FormatException)
                    {
                        if (this.ClassDef.GetPropDef(propName).HasLookupList())
                        {
                            propValue =
                                this.ClassDef.GetPropDef(propName).LookupListSource.GetLookupList().FindByValue(
                                    propValue).Id;
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
            // If the property will be changed by this set then
            // check if object is already editing (i.e. another property value has 
            // been changed if it is not then check that this object is still fresh
            // if the object is not fresh then throw appropriate exception.
            if (prop.PropertyValue != propValue)
            {
                if (!IsEditing)
                {
                    BeginEdit();
                }
                SetIsDirty(true);
                prop.PropertyValue = propValue;
                FireUpdatedEvent();
            }
        }

        /// <summary>
        /// Sets an initial property value
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <param name="propValue">The value to initialise to</param>
        private void InitialisePropertyValue(string propName, object propValue)
        {
            BOProp prop = GetBOProp(propName);
            prop.PropertyValue = propValue;
        }

        /// <summary>
        /// Returns the property collection
        /// </summary>
        /// <returns>Returns a BOPropCol object</returns>
        internal BOPropCol GetBOPropCol()
        {
            return _boPropCol;
        }

        /// <summary>
        /// Returns the property having the name specified
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns a BOProp object</returns>
        /// <exception cref="HabaneroArgumentException">Thrown if no
        /// property exists by the name specified</exception>
        protected internal BOProp GetBOProp(string propName)
        {
            BOProp prop = _boPropCol[propName];
            if (prop == null)
            {
                throw new PropertyNameInvalidException("The property '" + propName + 
                    "' for the class '" + ClassName + "' has not been declared " +
                    "in the class definitions.  Add the property to the class " +
                    "definitions and add the property to the class, or check that " +
                    "spelling and capitalisation are correct.");
            }
            return prop;
        }

        /// <summary>
        /// Indicates whether all of the property values are valid
        /// </summary>
        /// <param name="invalidReason">A string to modify with a reason
        /// for any invalid values</param>
        /// <returns>Returns true if all are valid</returns>
        public bool IsValid(out string invalidReason)
        {
            return GetBOPropCol().IsValid(out invalidReason);
        }

        /// <summary>
        /// Indicates whether all of the property values are valid
        /// </summary>
        /// <returns>Returns true if all are valid</returns>
        /// TODO ERIC - one of these two methods could be removed
        public bool IsValid()
        {
            string invalidReason;
            return GetBOPropCol().IsValid(out invalidReason);
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
        public IDatabaseConnection GetDatabaseConnection()
        {
            return _connection;
        }

        /// <summary>
        /// Sets the database connection
        /// </summary>
        /// <param name="connection">The database connection to set to</param>
        public void SetDatabaseConnection(IDatabaseConnection connection)
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
        /// Returns the database connection string, but with the password
        /// removed
        /// </summary>
        protected virtual string ErrorSafeConnectString
        {
            get { return _connection.ErrorSafeConnectString(); }
        }

        /// <summary>
        /// Reloads the business object from the database
        /// </summary>
        /// <returns>Returns true if the object was successfully loaded</returns>
        protected virtual bool Load()
        {
            bool loaded;

            loaded = Refresh();
            AfterLoad();
            return loaded;
        }

        /// <summary>
        /// Loads the business object from the database as long as it meets
        /// the search expression provided
        /// </summary>
        /// <param name="searchExpression">The search expression used to
        /// locate the business object</param>
        /// <returns>Returns true if the object was successfully loaded</returns>
        protected virtual bool Load(IExpression searchExpression)
        {
            bool loaded;

            loaded = Refresh(searchExpression);
            AfterLoad();
            return loaded;
        }

        /// <summary>
        /// Extra preparation or steps to take out after loading the business
        /// object
        /// </summary>
        protected virtual void AfterLoad()
        {
        }

        /// <summary>
        /// Refreshes the business object by reloading from the database using
        /// a search expression
        /// </summary>
        /// <param name="searchExpression">The search expression used to
        /// locate the business object</param>
        /// <returns>Returns true if refreshed successfully</returns>
        /// <exception cref="BusinessObjectNotFoundException">Thrown if the
        /// business object was not found in the database</exception>
        /// TODO ERIC - return false anywhere for failed operation? methods
        /// higher up the chain return the bool
        public virtual bool Refresh(IExpression searchExpression)
        {
            using (IDataReader dr = LoadDataReader(this.GetDatabaseConnection(), searchExpression))
            {
                try
                {
                    if (dr.Read())
                    {
                        return LoadProperties(dr);
                    }
                    else
                    {
                        throw new BusinessObjectNotFoundException(
                            "A serious error has occured please contact your system administrator" +
                            "There are no records in the database for the Class: " + _classDef.ClassName +
                            " identified by " + this.ID + " \n" + SelectSqlStatement(null) + " \n" + ErrorSafeConnectString);
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

        /// <summary>
        /// Refreshes the business object by reloading from the database
        /// </summary>
        /// <returns>Returns true if refreshed successfully</returns>
        /// <exception cref="BusinessObjectNotFoundException">Thrown if the
        /// business object was not found in the database</exception>
        public virtual bool Refresh()
        {
            return Refresh(null);
        }

        /// <summary>
        /// Loads an IDataReader object using the database connection provided
        /// </summary>
        /// <param name="connection">The database connection</param>
        /// <param name="searchExpression">The search expression used to
        /// locate the business object to be read</param>
        /// <returns>Returns an IDataReader object</returns>
        protected internal IDataReader LoadDataReader(IDatabaseConnection connection, IExpression searchExpression)
        {
            SqlStatement selectSQL = new SqlStatement(connection.GetConnection());
            if (searchExpression == null)
            {
                selectSQL.Statement.Append(SelectSqlStatement(selectSQL));
                return DatabaseConnection.CurrentConnection.LoadDataReader(selectSQL);
            }
            else
            {
                ParseParameterInfo(searchExpression);
                //TODO- Parametrize this.
                selectSQL.Statement.Append(SelectSQLWithNoSearchClauseIncludingWhere());
                searchExpression.SqlExpressionString(selectSQL, DatabaseConnection.CurrentConnection.LeftFieldDelimiter,
                                                     DatabaseConnection.CurrentConnection.RightFieldDelimiter);
//					searchExpression.SqlExpressionString(DatabaseConnection.CurrentConnection.LeftFieldDelimiter,
//					                                     DatabaseConnection.CurrentConnection.RightFieldDelimiter,
//					                                     DatabaseConnection.CurrentConnection.LeftFieldDelimiter,
//					                                     DatabaseConnection.CurrentConnection.RightDateDelimiter)) ;);
                return this.GetDatabaseConnection().LoadDataReader(selectSQL);
            }
        }

        /// <summary>
        /// Loads the properties, using the data record provided
        /// </summary>
        /// <param name="dr">The IDataRecord object</param>
        protected internal void LoadFromDataReader(IDataRecord dr)
        {
            LoadProperties(dr);
        }

        /// <summary>
        /// Loads the business object properties
        /// </summary>
        /// <param name="dr">An IDataRecord object</param>
        /// <returns>Returns true if loaded successfully</returns>
        /// TODO ERIC - where does datarecord come from?
        protected bool LoadProperties(IDataRecord dr)
        {
            //TODO_ERR: check that dr open valid etc.
            int i = 0;
            foreach (BOProp prop  in GetBOPropCol().SortedValues )
            {
                try
                {
                    prop.InitialiseProp(dr[i++]);
                }
                catch (IndexOutOfRangeException)
                {
                }
            }
            this.SetBOFlagValue(States.isNew, false);
            return true;
        }

        /// <summary>
        /// Commits to the database any changes made to the object
        /// </summary>
        public void ApplyEdit()
        {
            if (!BeforeApplyEdit())
            {
                return;
            }

            string reasonNotSaved = "";
            if (!(IsDeleted && IsNew))
            {
                if (IsDirty || IsNew)
                {
                    //log.Debug("ApplyEdit - Object of type " + this.GetType().Name + " is dirty or new, saving.") ;
                    if (IsValid(out reasonNotSaved))
                    {
                        CheckPersistRules();
                        UpdatedConcurrencyControlProperties();
                        GlobalRegistry.SynchronisationController.UpdateSynchronisationProperties(this);

                        BeforeUpdateToDB();
                        int numRowsUpdated;
                        ISqlStatementCollection statementCollection = GetPersistSql();
                        numRowsUpdated = _connection.ExecuteSql(statementCollection, null);
                        if (_transactionLog != null)
                        {
                            _transactionLog.RecordTransactionLog(this, CurrentLoggedOnUserName());
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
                                GetPersistSql().ToString(), ErrorSafeConnectString);
                        }
                    }
                    else
                    {
                        throw (new BusObjectInAnInvalidStateException(reasonNotSaved));
                    }
                } //Isdirty || new
            }

            UpdateAfterApplyEdit();

            AfterApplyEdit();
        }

        /// <summary>
        /// Carries out updates to the object after changes have been
        /// committed to the database
        /// </summary>
        protected void UpdateAfterApplyEdit()
        {
            if (!IsDeleted)
            {
                if (GetLoadedBusinessObjectBaseCol().Contains(ID.GetObjectNewID()))
                {
                    //System.Console.WriteLine("My line");//TODO ??
                }
                // set the flags back
                SetIsNew(false);
                SetIsDeleted(false);
                if (!GetLoadedBusinessObjectBaseCol().Contains(ID.GetObjectId()))
                {
                    AddToLoadedBusinessObjectCol(this);
                }
            }
            else
            {
                //Set the object state to an invalid state since the object has been deleted
                SetIsNew(true);
                SetIsDeleted(true);
                //Remove this object from the collection of objects since is is now invalid
                GetLoadedBusinessObjectBaseCol().Remove(this.ID);
                FireDeleted();
            } //!IsDeleted
            SetIsDirty(false);
            SetIsEditing(false);
            ReleaseWriteLocks();
        }

        /// <summary>
        /// Steps to carry out before the ApplyEdit() command is run
        /// </summary>
        /// <returns>Returns true</returns>
        /// TODO ERIC - what is meant to be returned? (in overridden methods)
        protected internal virtual bool BeforeApplyEdit()
        {
            return true;
        }

        /// <summary>
        /// Steps to carry out after the ApplyEdit() command is run
        /// </summary>
        protected virtual void AfterApplyEdit()
        {
        }

        /// <summary>
        /// Steps to carry out before the object values are updated to the
        /// database
        /// </summary>
        /// TODO ERIC - this method surprises me - what about ApplyEdit()
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
                        if (!IsNew)
                        {
                            GetLoadedBusinessObjectBaseCol().Remove(_primaryKey.GetOrigObjectID());
                        }
                        //If the object with the new ID does not exist in the collection then 
                        // add it.
                        if (!GetLoadedBusinessObjectBaseCol().Contains(this.ID.GetObjectId()))
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
        public void CancelEdit()
        {
            _boPropCol.RestorePropertyValues();
            SetIsDeleted(false);
            SetIsEditing(false);
            SetIsDirty(false);
            ReleaseWriteLocks();
            FireUpdatedEvent();
        }

        /// <summary>
        /// Deletes the object
        /// </summary>
        /// TODO ERIC - hmmm?
        public void Delete()
        {
            if (!IsEditing)
            {
                BeginEdit();
            }
            SetIsDirty(true);
            SetIsDeleted(true);
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
        protected virtual void CheckConcurrencyOnGettingObjectFromObjectManager()
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

        /// <summary>
        /// Increments the synchronisation version number
        /// </summary>
        public void IncrementVersionNumber()
        {
            if (this.ClassDef.SupportsSynchronising && this.GetBOPropCol().Contains("SyncVersionNumber"))
            {
                BOProp syncVersionNoProp = this.GetBOProp("SyncVersionNumber");
                syncVersionNoProp.PropertyValue = (int)syncVersionNoProp.PropertyValue + 1;
            }
            else
            {
                throw new NotSupportedException("The Business Object of type " + this.GetType() +
                                                " does not support version number synchronising.");
            }
        }

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

            if (!IsDeleted)
            {
                foreach (DictionaryEntry item in _keysCol)
                {
                    BOKey lBOKey = (BOKey) item.Value;
                    if (lBOKey.MustCheckKey())
                    {
                        SqlStatement checkDuplicateSQL =
                            new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
                        checkDuplicateSQL.Statement.Append(this.GetSelectSql());
                        string whereClause = " ( NOT (" + WhereClause(checkDuplicateSQL) + ")) AND " +
                                             GetCheckForDuplicateWhereClause(lBOKey, checkDuplicateSQL);
                        checkDuplicateSQL.AppendCriteria(whereClause);
                        //string whereClause = " WHERE ( NOT " + WhereClause() +
                        //	") AND " + GetCheckForDuplicateWhereClause(lBOKey);
                        using (IDataReader dr = this._connection.LoadDataReader(checkDuplicateSQL))
                        {
                            //_classDef.SelectSql + whereClause)) {  //)  DatabaseConnection.CurrentConnection.LoadDataReader
                            try
                            {
                                if (dr != null && dr.Read()) //There is already an object in the 
                                    //database matching these criteria.
                                {
                                    throw new BusObjDuplicateConcurrencyControlException(ClassName,
                                                                                         CurrentLoggedOnUserName(),
                                                                                         CurrentMachineName(),
                                                                                         DateTime.Now, whereClause, this);
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
            if (!_classDef.HasObjectID && !IsDeleted)
            {
                //Only check if the primaryKey is 
                if (_primaryKey.MustCheckKey())
                {
                    SqlStatement checkSQL = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
                    checkSQL.Statement.Append(this.GetSelectSql());
                    string whereClause = " WHERE " + _primaryKey.DatabaseWhereClause(checkSQL);
                    checkSQL.Statement.Append(whereClause);

                    using (IDataReader dr = DatabaseConnection.CurrentConnection.LoadDataReader(checkSQL))
                    {
                        //_classDef.SelectSql + whereClause)) {
                        try
                        {
                            if (dr.Read()) //There is already an object in the 
                                //database matching these criteria.
                            {
                                throw new BusObjDuplicateConcurrencyControlException(ClassName,
                                                                                     CurrentLoggedOnUserName(),
                                                                                     CurrentMachineName(), DateTime.Now,
                                                                                     whereClause, this);
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
            //TODO_Err: raise appropriate error if lBOKey == null
            return lBOKey.DatabaseWhereClause(sql);
        }

        #endregion //Concurrency

        #region Sql Statements

        /// <summary>
        /// Parses the parameter sql information into the given search
        /// expression
        /// </summary>
        /// <param name="searchExpression">The search expression</param>
        private void ParseParameterInfo(IExpression searchExpression)
        {
            BOProp prop;
            foreach (DictionaryEntry item in _boPropCol)
            {
                prop = (BOProp)item.Value;
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
        /// <param name="selectSQL">The sql statement used to generate and track
        /// parameters</param>
        /// <returns>Returns a string</returns>
        public virtual string SelectSqlStatement(SqlStatement selectSQL)
        {
            string statement = SelectSQLWithNoSearchClause();
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
            //				foreach (DictionaryEntry entry in currentClassDef.SuperClassDef.PrimaryKeyDef) {
            //					PropDef def = (PropDef) entry.Value;
            //					statement += currentClassDef.SuperClassDef.TableName + "." + def.FieldName;
            //					statement += " = " + currentClassDef.TableName + "." + def.FieldName;
            //					statement += " AND ";
            //				}
            //				currentClassDef = currentClassDef.SuperClassDef ;
            //			}
            statement += WhereClause(selectSQL);
            return statement;
        }

        /// <summary>
        /// Generates a sql statement with no search clause
        /// </summary>
        /// <returns>Returns a sql string</returns>
        /// TODO ERIC - seems to add a "where" clause anyway
        protected virtual string SelectSQLWithNoSearchClause()
        {
            return new SelectStatementGenerator(this, this._connection).Generate(-1);
        }

        /// <summary>
        /// Returns a sql statement with no search clause but including a
        /// "where " (or "and " where appropriate) statement
        /// </summary>
        /// <returns>Returns a sql string</returns>
        /// TODO ERIC - if this uses above then it will have "where" already
        private string SelectSQLWithNoSearchClauseIncludingWhere()
        {
            string basicSelect = SelectSQLWithNoSearchClause();
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
        public ISqlStatementCollection GetPersistSql()
        {
            if (IsNew && !(IsDeleted))
            {
                return GetInsertSQL();
            }
            else if (!(IsDeleted))
            {
                return GetUpdateSQL();
            }
            else if (IsDeleted && !(IsNew))
            {
                return GetDeleteSQL();
            }
            else
            {
                //TODO: raise appropriate error
                Console.WriteLine("A serious error has occured since the object is in an invalid state");
                return null;
            }
        }

        /// <summary>
        /// Builds a "delete" sql statement list for this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        public SqlStatementCollection GetDeleteSQL()
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
                deleteSql.Statement.Append("DELETE FROM " + currentClassDef.SuperClassDef.TableName + " WHERE " +
                                           WhereClauseForSuperClass(deleteSql, currentClassDef));
                statementCollection.Add(deleteSql);
                currentClassDef = currentClassDef.SuperClassDef;
            }
            return statementCollection;
        }

        /// <summary>
        /// Returns an "insert" sql statement list for inserting this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        public SqlStatementCollection GetInsertSQL()
        {
            InsertStatementGenerator gen = new InsertStatementGenerator(this, this.GetConnection());
            return gen.Generate();
        }

        /// <summary>
        /// Returns an "update" sql statement list for updating this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        public SqlStatementCollection GetUpdateSQL()
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
        /// TODO ERIC - clarify on the limit
        public string GetSelectSql(int limit)
        {
            return new SelectStatementGenerator(this, this._connection).Generate(limit);
        }

        /// <summary>
        /// Returns a "select" sql statement string that is used to load this
        /// object from the database
        /// </summary>
        /// <returns>Returns a sql string</returns>
        public string GetSelectSql()
        {
            return GetSelectSql(-1);
        }

        

        #endregion //Sql Statements

        #region Flags

        /// <summary>
        /// Indicates if the business object is new
        /// </summary>
        public bool IsNew
        {
            get { return GetBOFlagValue(States.isNew); }
        }

        /// <summary>
        /// Indicates if the business object has been deleted
        /// </summary>
        public bool IsDeleted
        {
            get { return GetBOFlagValue(States.isDeleted); }
        }

        /// <summary>
        /// Gets and sets the flag which indicates if the business object
        /// is currently being edited
        /// </summary>
        protected bool IsEditing
        {
            get { return GetBOFlagValue(States.isEditing); }
            set { SetBOFlagValue(States.isEditing, value); }
        }

        /// <summary>
        /// Indicates whether the business object has been amended since it
        /// was last persisted to the database
        /// </summary>
        public bool IsDirty
        {
            get { return GetBOFlagValue(States.isDirty); }
        }

        /// <summary>
        /// Sets the "IsDirty" flag to the specified value, to indicate
        /// if the business object has been edited since it was last persisted
        /// to the database
        /// </summary>
        /// <param name="dirtyValue">True if dirty, false if not</param>
        /// TODO ERIC - why not integrate this into the property above (and
        /// others below)
        protected void SetIsDirty(bool dirtyValue)
        {
            if (dirtyValue)
            {
                //log.Debug("obj " + this.GetType().Name + "  is being set to dirty") ;
                //throw new Exception("obj set to dirty.");
            }
            SetBOFlagValue(States.isDirty, dirtyValue);
        }

        /// <summary>
        /// Sets the "IsDeleted" flag to the specified value, to indicate
        /// if the business object has been deleted
        /// </summary>
        /// <param name="deletedValue">True if deleted, false if not</param>
        protected void SetIsDeleted(bool deletedValue)
        {
            SetBOFlagValue(States.isDeleted, deletedValue);
        }

        /// <summary>
        /// Sets the "IsNew" flag to the specified value, to indicate if the
        /// business object is new
        /// </summary>
        /// <param name="newValue">True if new, false if not</param>
        protected void SetIsNew(bool newValue)
        {
            SetBOFlagValue(States.isNew, newValue);
            if (!(_boPropCol == null))
            {
                _boPropCol.setIsObjectNew(newValue);
            }
        }

        /// <summary>
        /// Sets the "IsEditing" flag to the specified value, to indicate if the
        /// business object is being edited
        /// </summary>
        /// <param name="editValue"></param>
        protected void SetIsEditing(bool editValue)
        {
            SetBOFlagValue(States.isEditing, editValue);
        }

        /// <summary>
        /// Indicates if the specified flag is currently set
        /// </summary>
        /// <param name="objFlag">The flag in question. See the States
        /// enumeration for more detail.</param>
        /// <returns>Returns true if set, false if not</returns>
        protected bool GetBOFlagValue(States objFlag)
        {
            return Convert.ToBoolean(_flagState & objFlag);
        }

        /// <summary>
        /// Checks that the specified flag value matches the value specified,
        /// and throws an exception if it does not
        /// </summary>
        /// <param name="objFlag">The flag to check. See the States
        /// enumeration for more detail.</param>
        /// <param name="bValue">The value the flag should hold</param>
        protected void CheckBOFlagValue(States objFlag, bool bValue)
        {
            if (GetBOFlagValue(objFlag) != bValue)
            {
                CheckBOFlagValue(objFlag, bValue, "The " + this.GetType().Name +
                                              " object is " + (bValue ? "not " : "") + objFlag.ToString());
            }
        }

        /// <summary>
        /// Checks that the specified flag value matches the value specified,
        /// and throws an exception if it does not, using the exception message
        /// provided
        /// </summary>
        /// <param name="objFlag">The flag to check. See the States
        /// enumeration for more detail.</param>
        /// <param name="bValue">The value the flag should hold</param>
        /// <param name="errorMessage">The error message to display if the flag
        /// value is not as expected</param>
        protected void CheckBOFlagValue(States objFlag,
                                        bool bValue,
                                        string errorMessage)
        {
            if (GetBOFlagValue(objFlag) != bValue)
            {
                throw (new Exception(errorMessage));
            }
        }

        /// <summary>
        /// Sets the flag value as specified
        /// </summary>
        /// <param name="flag">The flag value to set. See the States
        /// enumeration for more detail.</param>
        /// <param name="bValue">The value to set to</param>
        protected void SetBOFlagValue(States flag, bool bValue)
        {
            if (bValue)
            {
                _flagState = _flagState | flag;
            }
            else
            {
                _flagState = _flagState & ~flag;
            }
        }

        #endregion //flags

        #region Implement ITransaction

        /// <summary>
        /// Carries out additional steps once the transaction has been committed
        /// to the database
        /// </summary>
        public void TransactionCommited()
        {
            _boPropCol.BackupPropertyValues();
            this.UpdateAfterApplyEdit();
        }

        //		SqlStatementList ITransaction.GetPersistSql() {
        //			return this.GetPersistSQL();
        //		}

        /// <summary>
        /// Checks the persistance rules
        /// </summary>
        /// TODO ERIC - this formatting surprises me - is it correct?
        void ITransaction.CheckPersistRules()
        {
            this.CheckPersistRules();
        }

        /// <summary>
        /// Cancels the edit
        /// </summary>
        public void TransactionCancelEdits()
        {
            this.CancelEdit();
        }

        /// <summary>
        /// Rolls back the transactions. Does nothing in this implementation.
        /// </summary>
        public void TransactionRolledBack()
        {
            //Do nothing
        }

        /// <summary>
        /// Returns the transaction ranking
        /// </summary>
        /// <returns>Returns zero</returns>
        /// TODO ERIC - what is a transaction ranking? some kind of priority
        /// scheme?
        public int TransactionRanking()
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
        public void BeforeCommit()
        {
            string reasonNotSaved;
            if (IsValid(out reasonNotSaved))
            {
                this.BeforeApplyEdit();
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
        public void AfterCommit()
        {
            this.AfterApplyEdit();
        }

        #endregion //ITransaction

        #region UI Support

        /// <summary>
        /// Returns the user interface mapper
        /// </summary>
        /// <returns>Returns an IUserInterfaceMapper object</returns>
        public virtual IUserInterfaceMapper GetUserInterfaceMapper()
        {
            return this._classDef.UIDefCol["default"];
        }

        /// <summary>
        /// Returns the user interface mapper with the UIDefName provided
        /// </summary>
        /// <param name="uiDefName">The UIDefName</param>
        /// <returns>Returns an IUserInterfaceMapper object</returns>
        public virtual IUserInterfaceMapper GetUserInterfaceMapper(string uiDefName)
        {
            return this._classDef.UIDefCol[uiDefName];
        }

        #endregion //UI Support
        
    }
}
