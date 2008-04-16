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
using System.Data;
using System.Security.Permissions;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
using Habanero.DB;
using log4net;

namespace Habanero.BO
{
    /// <summary>
    /// Provides methods to load business objects for given search criteria.
    /// In practice, you would typically call BOLoader.Instance.SomeMethod.
    /// </summary>
    public class BOLoader
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.BOLoader");

        private BOLoader() {}

        /// <summary>
        /// Provides an instance of the loader which can be used to call
        /// individual methods
        /// </summary>
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
            using (IDataReader dr = Instance.LoadDataReader(obj, obj.GetDatabaseConnection(), searchExpression))
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
            BOPropCol propCol = new BOPropCol();
            ClassDef classDef = obj.ClassDef;
            Type boType = obj.GetType();
            PrimaryKeyDef primaryKeyDef = classDef.GetPrimaryKeyDef();
            if (primaryKeyDef == null)
            {
                primaryKeyDef = (PrimaryKeyDef)obj.ID.KeyDef;
            }

            foreach (PropDef propDef in primaryKeyDef)
            {
                BOProp prop = propDef.CreateBOProp(false);
                prop.InitialiseProp(dr[prop.DatabaseFieldName]);
                propCol.Add(prop);
                //obj.PrimaryKey = (BOPrimaryKey)obj.ClassDef.PrimaryKeyDef.CreateBOKey(obj.Props);
            }
            BOPrimaryKey primaryKey = (BOPrimaryKey)primaryKeyDef.CreateBOKey(propCol);
            BusinessObject tempBusObj = GetLoadedBusinessObject(primaryKey.GetObjectId(), false);
            //BusinessObject lTempBusObj =
            //    BOLoader.Instance.GetLoadedBusinessObject(primaryKey.GetObjectId(), false);
            Type tempBusObjType = null;
            if (tempBusObj != null)
            {
                tempBusObjType = tempBusObj.GetType();
            }
            if (tempBusObj != null && boType != tempBusObjType &&
                !boType.IsSubclassOf(tempBusObjType) && !tempBusObjType.IsSubclassOf(boType))
            {
                throw new Exception(String.Format("There are two objects of differing types with the same primary key. " + 
                    "The object type for the object being loaded is '{0}', where the object type found in " +
                    "the loaded objects with the same primary key is '{1}'. The shared primary key is '{2}'.",
                    boType.Name, tempBusObjType.Name, primaryKey.GetObjectId()));
            }
            bool isReplacingSuperClassObject = false;
            if (tempBusObj != null && boType.IsSubclassOf(tempBusObjType))
            {
                isReplacingSuperClassObject = true;
            }
            if (tempBusObj == null || isReplacingSuperClassObject)
            {
                tempBusObj = classDef.CreateNewBusinessObject(obj.GetDatabaseConnection()); //InstantiateBusinessObject();
                LoadFromDataReader(tempBusObj, dr);
                try
                {
                    if (isReplacingSuperClassObject)
                    {
                        BusinessObject.AllLoaded().Remove(tempBusObj.ID.GetObjectId());
                    }
                    BusinessObject.AllLoaded().Add(tempBusObj.ID.GetObjectId(), new WeakReference(tempBusObj));
                }
                catch (Exception ex)
                {
                    throw new Exception("The object with id " +
                                                       tempBusObj.ID.GetObjectId(), ex);
                }
            }
            else if (tempBusObjType.IsSubclassOf(boType))
            {
                //TODO - refresh this subclass object.  It can be done using the current datareader because
                //the current data reader is for an object of the superclass type.
            }
            else
            {
                if (tempBusObj.State.IsDirty)
                {
                    log.Debug(
                        "An attempt was made to load an object already loaded that was in edit mode.  Refresh from database ignored." +
                        Environment.NewLine +
                        "BO Type: " + tempBusObjType.Name + Environment.NewLine + " Stack Trace: " +
                        Environment.StackTrace);
                }
                else
                {
                    LoadProperties(tempBusObj, dr);
                }
            }
            return tempBusObj;
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
            IDatabaseConnection databaseConnection = obj.GetDatabaseConnection();
            BusinessObject lTempBusObj = obj.ClassDef.CreateNewBusinessObject(databaseConnection);
            IDataReader dr = LoadDataReader(lTempBusObj, databaseConnection, searchExpression);
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
        /// <param name="businessObject">The business object to be loaded</param>
        /// <param name="searchExpression">The search expression used to locate the business object</param>
        /// <returns>Returns true if the object was successfully loaded</returns>
        internal virtual bool Load(BusinessObject businessObject, IExpression searchExpression)
        {
            bool loaded;

            loaded = Refresh(businessObject, searchExpression);
            businessObject.AfterLoad();
            return loaded;
        }

        /// <summary>
        /// Loads the properties, using the data record provided
        /// </summary>
        /// <param name="businessObject">The business object to be loaded</param>
        /// <param name="dr">The IDataRecord object</param>
        internal void LoadFromDataReader(BusinessObject businessObject, IDataRecord dr)
        {
            LoadProperties(businessObject, dr);
        }

        #region Single Business Object Methods

        /// <summary>
        /// Returns the business object of the type provided that meets the search criteria.  
        /// An exception is thrown if more than one business object is found that matches the criteria.  
        /// If that situation could arise, rather use GetBusinessObjectCol.
        /// </summary>
        /// <param name="criteria">The search criteria</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        public T GetBusinessObject<T>(string criteria) where T: BusinessObject, new()
        {
            return GetBusinessObject<T>(null, criteria);
        }

        /// <summary>
        /// Returns the business object of the type provided that meets the search criteria.  
        /// An exception is thrown if more than one business object is found that matches the criteria.  
        /// If that situation could arise, rather use GetBusinessObjectCol.
        /// </summary>
        /// <param name="boType">The type of the business objects to be loaded</param>
        /// <param name="criteria">The search criteria</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        public BusinessObject GetBusinessObject(Type boType, string criteria)
        {
            return GetBusinessObject(ClassDef.ClassDefs[boType], criteria);
        }

        /// <summary>
        /// Returns the business object of the type provided that meets the search criteria.  
        /// An exception is thrown if more than one business object is found that matches the criteria.  
        /// If that situation could arise, rather use GetBusinessObjectCol.
        /// </summary>
        /// <param name="classDef">The class definition for the businessobject to be loaded</param>
        /// <param name="criteria">The search criteria</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        public BusinessObject GetBusinessObject(ClassDef classDef, string criteria)
        {
            return GetBusinessObject<BusinessObject>(classDef, criteria);
        }

        //TODO:Peter - make a better load that doesn't use a bo col.
        private static T GetBusinessObject<T>(ClassDef classDef, string criteria) where T: BusinessObject, new()
        {
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>(classDef);
            if (classDef == null)
            {
                classDef = col.ClassDef;
            }
            if (classDef == null)
            {
                return null;
            }
            col.Load(criteria, "");
            if (col.Count < 1)
            {
                return null;
            }
            else if (col.Count > 1)
            {
                throw new UserException("Loading a '" + classDef.DisplayName + "' with criteria '" + criteria +
                                        "' returned more than one record when only one was expected.");
            }
            else
            {
                return col[0];
            }
        }

        #endregion //Single Business Object Methods

        #region Loaded Business Object Methods

        /// <summary>
        /// Loads a business object by ID
        /// </summary>
        /// <param name="id">The ID</param>
        /// <returns>Returns a business object</returns>
        internal BusinessObject GetLoadedBusinessObject(BOPrimaryKey id)
        {
            return GetLoadedBusinessObject(id.GetObjectId());
        }

        /// <summary>
        /// Loads a business object with the specified ID
        /// </summary>
        /// <param name="id">The ID</param>
        /// <returns>Returns a business object</returns>
		internal BusinessObject GetLoadedBusinessObject(string id)
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
		internal BusinessObject GetLoadedBusinessObject(string id, bool refreshIfReqNotCurrent)
        {
            //If the object is already in loaded then refresh it and return it if required.
            if (BusinessObject.AllLoaded().ContainsKey(id))
            {
                WeakReference weakRef = BusinessObject.AllLoaded()[id];
                //If the reference is valid return object else remove object from 
                // Collection
                if (weakRef.IsAlive && weakRef.Target != null)
                {
                    BusinessObject loadedBusinessObject;
                    loadedBusinessObject = (BusinessObject)weakRef.Target;
                    //Apply concurrency Control Strategy to the Business Object
                    if (refreshIfReqNotCurrent)
                    {
                        loadedBusinessObject.CheckConcurrencyOnGettingObjectFromObjectManager();
                    }
                    return loadedBusinessObject;
                }
                else
                {
                    BusinessObject.AllLoaded().Remove(id);
                }
            }
            return null;
        }

        #endregion //Loaded Business Object Methods

        #region Business Object Collection Methods

        /// <summary>
        /// Returns a business object collection with objects that meet the
        /// given search criteria, ordered as specified
        /// </summary>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <returns>Returns a business object collection</returns>
        public BusinessObjectCollection<T> GetBusinessObjectCol<T>(string searchCriteria, string orderByClause) 
            where T:BusinessObject, new()
        {
            return GetBusinessObjectCollection<T>(null, searchCriteria, orderByClause);
        }

        /// <summary>
        /// Returns a business object collection with objects that meet the
        /// given search criteria, ordered as specified
        /// </summary>
        /// <param name="boType">The type of the business objects to be loaded</param>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <returns>Returns a business object collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCol(Type boType, string searchCriteria, string orderByClause)
        {
            return GetBusinessObjectCol(ClassDef.ClassDefs[boType], searchCriteria, orderByClause);
        }

        /// <summary>
        /// Returns a business object collection with objects that meet the
        /// given search criteria, ordered as specified
        /// </summary>
        /// <param name="classDef">The class definition for the business objects to be loaded</param>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <returns>Returns a business object collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCol(ClassDef classDef, string searchCriteria, string orderByClause)
        {
            return GetBusinessObjectCollection(classDef, null, searchCriteria, orderByClause);
        }

		/// <summary>
		/// Returns a business object collection with objects that meet the
		/// given search expression, ordered as specified
		/// </summary>
		/// <param name="searchExpression">The search expression</param>
		/// <param name="orderByClause">The order-by clause</param>
		/// <returns>Returns a business object collection</returns>
		public BusinessObjectCollection<T> GetBusinessObjectCol<T>(IExpression searchExpression, string orderByClause)
			where T: BusinessObject, new()
		{
            return GetBusinessObjectCollection<T>(searchExpression, null, orderByClause);
		}

		/// <summary>
		/// Returns a business object collection with objects that meet the
		/// given search expression, ordered as specified
		/// </summary>
		/// <param name="boType">The type of the business objects to be loaded</param>
		/// <param name="searchExpression">The search expression</param>
		/// <param name="orderByClause">The order-by clause</param>
		/// <returns>Returns a business object collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCol(Type boType, IExpression searchExpression, string orderByClause)
		{
			return GetBusinessObjectCol(ClassDef.ClassDefs[boType], searchExpression, orderByClause);
        }

        /// <summary>
        /// Returns a business object collection with objects that meet the
        /// given search expression, ordered as specified
        /// </summary>
        /// <param name="classDef">The class definition for the business objects to be loaded</param>
        /// <param name="searchExpression">The search expression</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <returns>Returns a business object collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCol(ClassDef classDef, IExpression searchExpression, string orderByClause)
        {
            return GetBusinessObjectCollection(classDef, searchExpression, null, orderByClause);
        }

        private static BusinessObjectCollection<T> GetBusinessObjectCollection<T>(IExpression searchExpression, string searchCriteria, string orderByClause)
            where T: BusinessObject, new()
        {
            BusinessObjectCollection<T> businessObjectCollection = new BusinessObjectCollection<T>();
            if (searchExpression != null)
            {
                businessObjectCollection.Load(searchExpression, orderByClause);
            } else
            {
                businessObjectCollection.Load(searchCriteria, orderByClause);
            }
            return businessObjectCollection;
        }

        private static IBusinessObjectCollection GetBusinessObjectCollection(ClassDef classDef, IExpression searchExpression, string searchCriteria, string orderByClause)
        {
            if (classDef == null) throw new ArgumentNullException("classDef");
            IBusinessObjectCollection businessObjectCollection = CreateBusinessObjectCollection(classDef);
            if (searchExpression != null)
            {
                businessObjectCollection.Load(searchExpression, orderByClause);
            }
            else
            {
                businessObjectCollection.Load(searchCriteria, orderByClause);
            }
            return businessObjectCollection;
        }

        ///<summary>
        /// Creates a BusinessObjectCollection for classes of the type specified by the class definition.
        ///</summary>
        ///<param name="classDef">The class definition to use in creating the BusinessObjectCollection. </param>
        ///<returns> A BusinessObjectCollection of the correct type. </returns>
        private static IBusinessObjectCollection CreateBusinessObjectCollection(ClassDef classDef)
        {
            if (classDef == null)
            {
                return null;
            }
            Type type = typeof(BusinessObjectCollection<>);
            type = type.MakeGenericType(classDef.ClassType);
            return (IBusinessObjectCollection)Activator.CreateInstance(type, classDef);
        }

        

        #endregion //Business Object Collection Methods

        /// <summary>
        /// Loads an IDataReader object using the database connection provided
        /// </summary>
        /// <param name="connection">The database connection</param>
        /// <param name="searchExpression">The search expression used to
        /// locate the business object to be read</param>
        /// <returns>Returns an IDataReader object</returns>
        internal IDataReader LoadDataReader(BusinessObject obj, IDatabaseConnection connection, IExpression searchExpression)
        {
            SqlStatement selectSql = new SqlStatement(connection);
            if (searchExpression == null)
            {
                selectSql.Statement.Append(obj.SelectSqlStatement(selectSql));
                return connection.LoadDataReader(selectSql);
                //return DatabaseConnection.CurrentConnection.LoadDataReader(selectSql);
            }
            else
            {
                obj.ParseParameterInfo(searchExpression);
                selectSql.Statement.Append(obj.SelectSqlWithNoSearchClauseIncludingWhere());
                searchExpression.SqlExpressionString(selectSql, connection.LeftFieldDelimiter,
                                                     connection.RightFieldDelimiter);
                //searchExpression.SqlExpressionString(selectSql, DatabaseConnection.CurrentConnection.LeftFieldDelimiter,
                //                                     DatabaseConnection.CurrentConnection.RightFieldDelimiter);
                return obj.GetDatabaseConnection().LoadDataReader(selectSql);
            }
        }

        /// <summary>
        /// Returns the database connection for the given business object
        /// </summary>
        /// <param name="bo">The business object</param>
        /// <returns>Returns the connection</returns>
        public IDatabaseConnection GetDatabaseConnection(BusinessObject bo)
        {
            return bo.GetDatabaseConnection();
        }

        /// <summary>
        /// Sets the database connection for a given business object
        /// </summary>
        /// <param name="bo">The business object</param>
        /// <param name="connection">The connection</param>
        public void SetDatabaseConnection(BusinessObject bo, IDatabaseConnection connection)
        {
            bo.SetDatabaseConnection(connection);
        }


        #region Load By ID Methods

        #region Generic Methods

        /// <summary>
        /// Returns the business object of the type provided that has the given primary key.
        /// </summary>
        /// <param name="id">The primary key ID</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        /// <exception cref="InvalidPropertyException">Thrown if there is a multiple primary key</exception>
        public T GetBusinessObjectByID<T>(Guid id) 
            where T: BusinessObject, new()
        {
            return GetBusinessObjectByID<T>(id.ToString("B"));
        }

        /// <summary>
        /// Returns the business object of the type provided that has the given primary key.
        /// </summary>
        /// <param name="id">The primary key ID</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        /// <exception cref="InvalidPropertyException">Thrown if there is a multiple primary key</exception>
        public T GetBusinessObjectByID<T>(int id) 
            where T: BusinessObject, new()
        {
            return GetBusinessObjectByID<T>(id.ToString());
        }

        /// <summary>
        /// Returns the business object of the type provided that has the given primary key.
        /// </summary>
        /// <param name="id">The primary key ID</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        /// <exception cref="InvalidPropertyException">Thrown if there is a multiple primary key</exception>
        public T GetBusinessObjectByID<T>(string id) 
            where T: BusinessObject, new()
        {
            return GetBusinessObjectByID<T>(null, id);
        }

        /// <summary>
        /// Returns the business object of the type provided that has the given primary key.  
        /// This method is useful when you have a composite primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key object that contains the specific search values</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        public T GetBusinessObjectByID<T>(BOPrimaryKey primaryKey)
            where T: BusinessObject, new()
        {
            return GetBusinessObjectByID<T>(null, primaryKey);
        }

        #endregion //Generic Methods

        #region Type Loaded Methods

        /// <summary>
        /// Returns the business object of the type provided that has the given primary key.
        /// </summary>
        /// <param name="boType">The type of the business object to be loaded</param>
        /// <param name="id">The primary key ID</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        /// <exception cref="InvalidPropertyException">Thrown if there is a multiple primary key</exception>
        public BusinessObject GetBusinessObjectByID(Type boType, Guid id)
        {
            return GetBusinessObjectByID(boType, id.ToString("B"));
        }

        /// <summary>
        /// Returns the business object of the type provided that has the given primary key.
        /// </summary>
        /// <param name="boType">The type of the business object to be loaded</param>
        /// <param name="id">The primary key ID</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        /// <exception cref="InvalidPropertyException">Thrown if there is a multiple primary key</exception>
        public BusinessObject GetBusinessObjectByID(Type boType, int id)
        {
            return GetBusinessObjectByID(boType, id.ToString());
        }

        /// <summary>
        /// Returns the business object of the type provided that has the given primary key.
        /// </summary>
        /// <param name="boType">The type of the business object to be loaded</param>
        /// <param name="id">The primary key ID</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        /// <exception cref="InvalidPropertyException">Thrown if there is a multiple primary key</exception>
        public BusinessObject GetBusinessObjectByID(Type boType, string id)
        {
            return GetBusinessObjectByID<BusinessObject>(ClassDef.ClassDefs[boType], id);
        }

        /// <summary>
        /// Returns the business object of the type provided that has the given primary key.  
        /// This method is useful when you have a composite primary key.
        /// </summary>
        /// <param name="boType">The type of the business object to be loaded</param>
        /// <param name="primaryKey">The primary key object that contains the specific search values</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        public BusinessObject GetBusinessObjectByID(Type boType, BOPrimaryKey primaryKey)
        {
            return GetBusinessObjectByID<BusinessObject>(ClassDef.ClassDefs[boType], primaryKey);
        }

        #endregion //Type Loaded Methods

        #region ClassDef Loaded Methods

        /// <summary>
        /// Returns the business object of the type provided that has the given primary key.
        /// </summary>
        /// <param name="classDef">The class definition for the business object to be loaded</param>
        /// <param name="id">The primary key ID</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        /// <exception cref="InvalidPropertyException">Thrown if there is a multiple primary key</exception>
        public BusinessObject GetBusinessObjectByID(ClassDef classDef, Guid id)
        {
            return GetBusinessObjectByID(classDef, id.ToString("B"));
        }

        /// <summary>
        /// Returns the business object of the type provided that has the given primary key.
        /// </summary>
        /// <param name="classDef">The class definition for the business object to be loaded</param>
        /// <param name="id">The primary key ID</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        /// <exception cref="InvalidPropertyException">Thrown if there is a multiple primary key</exception>
        public BusinessObject GetBusinessObjectByID(ClassDef classDef, int id)
        {
            return GetBusinessObjectByID(classDef, id.ToString());
        }

        /// <summary>
        /// Returns the business object of the type provided that has the given primary key.
        /// </summary>
        /// <param name="classDef">The class definition for the business object to be loaded</param>
        /// <param name="id">The primary key ID</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        /// <exception cref="InvalidPropertyException">Thrown if there is a multiple primary key</exception>
        public BusinessObject GetBusinessObjectByID(ClassDef classDef, string id)
        {
            return GetBusinessObjectByID<BusinessObject>(classDef, id);
        }

        /// <summary>
        /// Returns the business object of the type provided that has the given primary key.  
        /// This method is useful when you have a composite primary key.
        /// </summary>
        /// <param name="classDef">The class definition for the business object to be loaded</param>
        /// <param name="primaryKey">The primary key object that contains the specific search values</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        public BusinessObject GetBusinessObjectByID(ClassDef classDef, BOPrimaryKey primaryKey)
        {
            return GetBusinessObjectByID<BusinessObject>(classDef, primaryKey);
        }

        #endregion //ClassDef Loaded Methods

        #region Private Methods

        private static T GetBusinessObjectByID<T>(ClassDef classDef, string id) 
            where T: BusinessObject, new()
        {
            if (classDef == null)
            {
                classDef = ClassDef.ClassDefs[typeof(T)];
                if (classDef == null)
                {
                    return null;
                }
            }
            PrimaryKeyDef primaryKeyDef = classDef.GetPrimaryKeyDef();
            if (primaryKeyDef.Count > 1)
            {
                throw new InvalidPropertyException("A business object cannot be loaded " +
                   "with a single ID when there are multiple properties making up " +
                   "the primary key.");
            }
            string criteria = string.Format("{0}='{1}'", primaryKeyDef.KeyName, id);
            return GetBusinessObject<T>(classDef, criteria);
        }        
        
        private static T GetBusinessObjectByID<T>(ClassDef classDef, BOPrimaryKey primaryKey)
            where T: BusinessObject, new()
        {
            if (classDef == null)
            {
                classDef = ClassDef.ClassDefs[typeof(T)];
            }
            string criteria = "";
            foreach (BOProp boProp in primaryKey.GetBOPropCol().Values)
            {
                if (criteria.Length > 0) criteria += " AND ";
                string propValue = boProp.Value.ToString();
                if (boProp.PropertyType == typeof(Guid))
                {
                    propValue = ((Guid)boProp.Value).ToString("B");
                }
                criteria += String.Format("{0}='{1}'", boProp.DatabaseFieldName, propValue);
            }

            return GetBusinessObject<T>(classDef,criteria);
        }

        #endregion //Private Methods

        #endregion //Load By ID Methods
    }
}
