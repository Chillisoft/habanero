using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Permissions;
using System.Text;
using Habanero.Base;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.CriteriaManager;
using Habanero.DB;
using log4net;

namespace Habanero.Bo
{
    public class BOLoader
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.Bo.BOLoader");

        private BOLoader() {}
        public static BOLoader Instance = new BOLoader();

        /// <summary>
        /// Refreshes the business object by reloading from the database
        /// </summary>
        /// <returns>Returns true if refreshed successfully</returns>
        /// <exception cref="BusinessObjectNotFoundException">Thrown if the
        /// business object was not found in the database</exception>
        public virtual bool Refresh(BusinessObject obj)
        {
            return Refresh(obj, null);
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
        internal virtual bool Refresh(BusinessObject obj, IExpression searchExpression)
        {
            using (IDataReader dr = BOLoader.Instance.LoadDataReader(obj, obj.GetDatabaseConnection(), searchExpression))
            {
                try
                {
                    if (dr.Read())
                    {
                        return LoadProperties(obj, dr);
                    }
                    else
                    {
                        throw new BusinessObjectNotFoundException(
                            "A serious error has occured please contact your system administrator" +
                            "There are no records in the database for the Class: " + obj.ClassDef.ClassName +
                            " identified by " + obj.ID + " \n" + obj.SelectSqlStatement(null) + " \n" + obj.GetDatabaseConnection().ErrorSafeConnectString());
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
        /// Loads the business object properties
        /// </summary>
        /// <param name="dr">An IDataRecord object</param>
        /// <returns>Returns true if loaded successfully</returns>
        /// TODO ERIC - where does datarecord come from?
        internal bool LoadProperties(BusinessObject obj, IDataRecord dr)
        {
            //TODO_ERR: check that dr open valid etc.
            int i = 0;
            foreach (BOProp prop in obj.Props.SortedValues)
            {
                try
                {
                    prop.InitialiseProp(dr[i++]);
                }
                catch (IndexOutOfRangeException)
                {
                }
            }
            obj.State.IsNew = false;
            return true;
        }

        /// <summary>
        /// Returns a new object loaded from the data reader or one from the 
        /// object manager that is refreshed with data from the data reader
        /// </summary>
        /// <param name="dr">A data reader pointing at a valid record</param>
        /// <returns>A valid business object for the data in the 
        /// data reader</returns>
        [ReflectionPermission(SecurityAction.Demand)]
        internal BusinessObject GetBusinessObject(BusinessObject obj, IDataRecord dr)
        {
            // This method creates a primary key object with the data from the 
            // datareader and then checks if this is loaded, if it is then the 
            // properties for the object are reloaded from the datareader else 
            // a new object is created and the data for it is loaded from the 
            // datareader. The object is then added to the object manager.
            BOProp prop;
            BOPropCol propCol = new BOPropCol();
            BOPrimaryKey lPrimaryKey;
            foreach (DictionaryEntry item in obj.ClassDef.PrimaryKeyDef)
            {
                PropDef lPropDef = (PropDef)item.Value;
                prop = lPropDef.CreateBOProp(false);

                prop.InitialiseProp(dr[prop.DatabaseFieldName]);
                propCol.Add(prop);

                obj.PrimaryKey = (BOPrimaryKey)obj.ClassDef.PrimaryKeyDef.CreateBOKey(obj.Props);
            }
            lPrimaryKey = (BOPrimaryKey)obj.ClassDef.PrimaryKeyDef.CreateBOKey(propCol);

            BusinessObject lTempBusObj =
                BOLoader.Instance.GetLoadedBusinessObject(lPrimaryKey.GetObjectId(), false);
            bool isReplacingSuperClassObject = false;
            if (lTempBusObj != null && obj.GetType().IsSubclassOf(lTempBusObj.GetType()))
            {
                isReplacingSuperClassObject = true;
            }
            if (lTempBusObj == null || isReplacingSuperClassObject)
            {
                lTempBusObj = obj.ClassDef.InstantiateBusinessObject();
                LoadFromDataReader(lTempBusObj, dr);
                try
                {
                    if (isReplacingSuperClassObject)
                    {
                        BusinessObject.AllLoaded().Remove(lTempBusObj.ID.GetObjectId());
                    }
                    BusinessObject.AllLoaded().Add(lTempBusObj.ID.GetObjectId(), new WeakReference(lTempBusObj));
                }
                catch (Exception ex)
                {
                    throw new Exception("The object with id " +
                                                       lTempBusObj.ID.GetObjectId(), ex);
                }
            }
            else if (lTempBusObj.GetType().IsSubclassOf(obj.GetType()))
            {
                //TODO - refresh this subclass object.  It can be done using the current datareader because
                //the current data reader is for an object of the superclass type.
            }
            else
            {
                if (lTempBusObj.State.IsDirty)
                {
                    log.Debug(
                        "An attempt was made to load an object already loaded that was in edit mode.  Refresh from database ignored." +
                        Environment.NewLine +
                        "BO Type: " + lTempBusObj.GetType().Name + Environment.NewLine + " Stack Trace: " +
                        Environment.StackTrace);
                }
                else
                {
                    LoadProperties(lTempBusObj, dr);
                }
            }
            return lTempBusObj;
        }

        /// <summary>
        /// Loads a business object that meets the specified search criteria
        /// </summary>
        /// <param name="searchExpression">The search expression</param>
        /// <returns>Returns a business object, or null if none is found that
        /// meets the criteria</returns>
        [ReflectionPermission(SecurityAction.Demand)]
        internal BusinessObject GetBusinessObject(BusinessObject obj, IExpression searchExpression)
        {
            BusinessObject lTempBusObj = obj.ClassDef.InstantiateBusinessObject();
            lTempBusObj.SetDatabaseConnection(obj.GetDatabaseConnection());
            IDataReader dr = LoadDataReader(lTempBusObj, obj.GetDatabaseConnection(), searchExpression);
            try
            {
                if (dr.Read())
                {
                    return GetBusinessObject(obj, dr);
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
        /// Reloads the business object from the database
        /// </summary>
        /// <returns>Returns true if the object was successfully loaded</returns>
        internal virtual bool Load(BusinessObject obj)
        {
            bool loaded;

            loaded = Refresh(obj);
            obj.AfterLoad();
            return loaded;
        }


        /// <summary>
        /// Loads the business object from the database as long as it meets
        /// the search expression provided
        /// </summary>
        /// <param name="searchExpression">The search expression used to
        /// locate the business object</param>
        /// <returns>Returns true if the object was successfully loaded</returns>
        internal virtual bool Load(BusinessObject obj, IExpression searchExpression)
        {
            bool loaded;

            loaded = Refresh(obj, searchExpression);
            obj.AfterLoad();
            return loaded;
        }

        /// <summary>
        /// Loads the properties, using the data record provided
        /// </summary>
        /// <param name="dr">The IDataRecord object</param>
        internal void LoadFromDataReader(BusinessObject obj, IDataRecord dr)
        {
            LoadProperties(obj, dr);
        }

        //TODO:Peter - make a better load that doesn't use a bo col.
        /// <summary>
        /// Returns the business object of the type provided and that meets the
        /// search criteria
        /// </summary>
        /// <param name="criteria">The search criteria</param>
        /// <param name="T">The business object type</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object
        /// matches the criteria</exception>
        /// TODO ERIC - i don't understand why the exception is thrown, since
        /// the normal pattern is just to return the first one that meets
        /// expectations
        public T GetBusinessObject<T>(string criteria) where T:BusinessObject
        {
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.Load(criteria, "");
            if (col.Count < 1)
            {
                return null;
            }
            else if (col.Count > 1)
            {
                throw new UserException("Loading a " + typeof(T).Name + " with criteria " + criteria +
                                        " returned more than one record when only one was expected.");
            }
            else
            {
                return col[0];
            }
        }


        ///// <summary>
        ///// Creates a new business object from the class definition
        ///// </summary>
        ///// <returns></returns>
        //public BusinessObject CreateNewBusinessObject(ClassDef classDef)
        //{
        //    return classDef.InstantiateBusinessObject();
        //}


        /// <summary>
        /// Loads a business object by ID
        /// </summary>
        /// <param name="id">The ID</param>
        /// <returns>Returns a business object</returns>
        public BusinessObject GetLoadedBusinessObject(BOPrimaryKey id)
        {
            return GetLoadedBusinessObject(id.GetObjectId());
        }

        /// <summary>
        /// Loads a business object with the specified ID
        /// </summary>
        /// <param name="id">The ID</param>
        /// <returns>Returns a business object</returns>
        public BusinessObject GetLoadedBusinessObject(string id)
        {
            return GetLoadedBusinessObject(id, true);
        }

        /// <summary>
        /// Loads a business object with the specified ID from a collection
        /// of loaded objects
        /// </summary>
        /// <param name="id">The ID</param>
        /// <param name="refreshIfReqNotCurrent">Whether to check for
        /// object concurrency at the time of loading</param>
        /// <returns>Returns a business object</returns>
        public BusinessObject GetLoadedBusinessObject(string id, bool refreshIfReqNotCurrent)
        {
            //If the object is already in loaded then refresh it and return it if required.
            if (BusinessObject.AllLoaded().ContainsKey(id))
            {
                BusinessObject lBusinessObject;
                WeakReference weakRef = BusinessObject.AllLoaded()[id];
                //If the reference is valid return object else remove object from 
                // Collection
                if (weakRef.IsAlive && weakRef.Target != null)
                {
                    lBusinessObject = (BusinessObject)weakRef.Target;
                    //Apply concurrency Control Strategy to the Business Object
                    if (refreshIfReqNotCurrent)
                    {
                        lBusinessObject.CheckConcurrencyOnGettingObjectFromObjectManager();
                    }
                    return lBusinessObject;
                }
                else
                {
                    BusinessObject.AllLoaded().Remove(id);
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a business object collection with objects that meet the
        /// given search criteria, ordered as specified
        /// </summary>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <returns>Returns a business object collection</returns>
        public BusinessObjectCollection<T> GetBusinessObjectCol<T>(string searchCriteria, string orderByClause) where T:BusinessObject
        {
            BusinessObjectCollection<T> bOCol = new BusinessObjectCollection<T>();
            bOCol.Load(searchCriteria, orderByClause);
            return bOCol;
        }

        /// <summary>
        /// Returns a business object collection with objects that meet the
        /// given search criteria, ordered as specified
        /// </summary>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <returns>Returns a business object collection</returns>
        public BusinessObjectCollection<BusinessObject> GetBusinessObjectCol(Type boType, string searchCriteria,
                                                                         string orderByClause)
        {
            BusinessObjectCollection<BusinessObject> bOCol = new BusinessObjectCollection<BusinessObject>(ClassDef.ClassDefs[boType]);
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
        public BusinessObjectCollection<BusinessObject> GetBusinessObjectCol(Type boType, IExpression searchExpression,
                                                                         string orderByClause)
        {
            BusinessObjectCollection<BusinessObject> bOCol = new BusinessObjectCollection<BusinessObject>(ClassDef.ClassDefs[boType]);
            bOCol.Load(searchExpression, orderByClause);
            return bOCol;
        }

        /// <summary>
        /// Loads an IDataReader object using the database connection provided
        /// </summary>
        /// <param name="connection">The database connection</param>
        /// <param name="searchExpression">The search expression used to
        /// locate the business object to be read</param>
        /// <returns>Returns an IDataReader object</returns>
        internal IDataReader LoadDataReader(BusinessObject obj, IDatabaseConnection connection, IExpression searchExpression)
        {
            SqlStatement selectSql = new SqlStatement(connection.GetConnection());
            if (searchExpression == null)
            {
                selectSql.Statement.Append(obj.SelectSqlStatement(selectSql));
                return DatabaseConnection.CurrentConnection.LoadDataReader(selectSql);
            }
            else
            {
                obj.ParseParameterInfo(searchExpression);
                selectSql.Statement.Append(obj.SelectSqlWithNoSearchClauseIncludingWhere());
                searchExpression.SqlExpressionString(selectSql, DatabaseConnection.CurrentConnection.LeftFieldDelimiter,
                                                     DatabaseConnection.CurrentConnection.RightFieldDelimiter);
                return obj.GetDatabaseConnection().LoadDataReader(selectSql);
            }
        }

        public IDatabaseConnection GetDatabaseConnection(BusinessObject bo)
        {
            return bo.GetDatabaseConnection();
        }

        public void SetDatabaseConnection(BusinessObject bo, IDatabaseConnection connection)
        {
            bo.SetDatabaseConnection(connection);
        }
    }
}
