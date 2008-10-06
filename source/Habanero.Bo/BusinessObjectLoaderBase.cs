using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    public abstract class BusinessObjectLoaderBase
    {
        private static Criteria GetCriteriaObject(IClassDef classDef, string criteriaString)
        {
            Criteria criteria = CriteriaParser.CreateCriteria(criteriaString);
            QueryBuilder.PrepareCriteria(classDef, criteria);
            return criteria;
        }

        #region GetBusinessObjectCollection

        private static void CheckNotTypedAsBusinessObject<T>()
            where T : class, IBusinessObject, new()
        {
            if (typeof(T) == typeof(BusinessObject))
            {
                throw ExceptionHelper.CreateLoaderGenericTypeMethodException();
            }
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given. 
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteria">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria)
            where T : class, IBusinessObject, new()
        {
            CheckNotTypedAsBusinessObject<T>();
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.SelectQuery.Criteria = criteria;
            Refresh(col);
            return col;
        }
        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given. 
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteriaString">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(string criteriaString) where T : class, IBusinessObject, new()
        {
            CheckNotTypedAsBusinessObject<T>();
            Criteria criteria = GetCriteriaObject(ClassDef.ClassDefs[typeof(T)], criteriaString);
            //            Criteria criteria = CriteriaParser.CreateCriteria(criteriaString);
            //            QueryBuilder.PrepareCriteria(ClassDef.ClassDefs[typeof(T)], criteria);
            return GetBusinessObjectCollection<T>(criteria);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given. 
        /// </summary>
        /// <param name="classDef">The ClassDef for the collection to load</param>
        /// <param name="criteria">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, Criteria criteria)
        {
            IBusinessObjectCollection col = CreateCollectionOfType(classDef.ClassType);
            col.SelectQuery.Criteria = criteria;
            Refresh(col);
            return col;
        }

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="collection">The collection to refresh</param>
        public abstract void Refresh<T>(BusinessObjectCollection<T> collection)
            where T : class, IBusinessObject, new();

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        /// <param name="collection">The collection to refresh</param>
        public abstract void Refresh(IBusinessObjectCollection collection);

        /// <summary>
        /// Loads a BusinessObjectCollection using the searchCriteria an given. It's important to make sure that the ClassDef given
        /// has the properties defined in the fields of the select searchCriteria and orderCriteria.  
        /// </summary>
        /// <param name="classDef">The ClassDef for the collection to load</param>
        /// <param name="criteriaString">The select query to use to load from the data source</param>
        /// <param name="orderCriteria">The order that the collections must be loaded in e.g. Surname, FirstName</param>
        /// <returns>The loaded collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, string criteriaString, string orderCriteria)
        {
            Criteria criteria = GetCriteriaObject(classDef, criteriaString);
            OrderCriteria orderCriteriaObj = QueryBuilder.CreateOrderCriteria(classDef, orderCriteria);
            return GetBusinessObjectCollection(classDef, criteria, orderCriteriaObj);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the searchCriteria an given. It's important to make sure that the ClassDef given
        /// has the properties defined in the fields of the select searchCriteria and orderCriteria.  
        /// </summary>
        /// <param name="classDef">The ClassDef for the collection to load</param>
        /// <param name="criteriaString">The select query to use to load from the data source</param>
        /// <returns>The loaded collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, string criteriaString)
        {
            Criteria criteria = GetCriteriaObject(classDef, criteriaString);
            return GetBusinessObjectCollection(classDef, criteria);
        }


        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given, applying the order criteria to order the collection that is returned. 
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteriaString">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        /// <param name="orderCriteria">The order criteria to use (ie what fields to order the collection on)</param>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(string criteriaString, string orderCriteria) where T : class, IBusinessObject, new()
        {
            CheckNotTypedAsBusinessObject<T>();
            ClassDef classDef = ClassDef.ClassDefs[typeof(T)];
            Criteria criteria = GetCriteriaObject(classDef, criteriaString);
            OrderCriteria orderCriteriaObj = QueryBuilder.CreateOrderCriteria(classDef, orderCriteria);
            return GetBusinessObjectCollection<T>(criteria, orderCriteriaObj);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given, applying the order criteria to order the collection that is returned. 
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteria">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        /// <param name="orderCriteria">The order criteria to use (ie what fields to order the collection on)</param>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria, OrderCriteria orderCriteria)
            where T : class, IBusinessObject, new()
        {
            CheckNotTypedAsBusinessObject<T>();
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.SelectQuery.Criteria = criteria;
            col.SelectQuery.OrderCriteria = orderCriteria;
            Refresh(col);
            return col;
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given, applying the order criteria to order the collection that is returned. 
        /// </summary>
        /// <param name="classDef">The ClassDef for the collection to load</param>
        /// <param name="criteria">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        /// <param name="orderCriteria">The order criteria to use (ie what fields to order the collection on)</param>
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, Criteria criteria,
                                                                     OrderCriteria orderCriteria)
        {
            IBusinessObjectCollection col = CreateCollectionOfType(classDef.ClassType);
            QueryBuilder.PrepareCriteria(classDef, criteria);
            col.SelectQuery.Criteria = criteria;
            col.SelectQuery.OrderCriteria = orderCriteria;
            Refresh(col);
            return col;
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the SelectQuery given. It's important to make sure that T (meaning the ClassDef set up for T)
        /// has the properties defined in the fields of the select query.  
        /// This method allows you to define a custom query to load a businessobjectcollection so that you can perhaps load from multiple
        /// tables using a join (if loading from a database source).
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="selectQuery">The select query to use to load from the data source</param>
        /// <returns>The loaded collection</returns>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(ISelectQuery selectQuery)
            where T : class, IBusinessObject, new()
        {
            CheckNotTypedAsBusinessObject<T>();
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.SelectQuery = selectQuery;
            Refresh(col);
            return col;
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the SelectQuery given. It's important to make sure that the ClassDef given
        /// has the properties defined in the fields of the select query.  
        /// This method allows you to define a custom query to load a businessobjectcollection so that you can perhaps load from multiple
        /// tables using a join (if loading from a database source).
        /// </summary>
        /// <param name="classDef">The ClassDef for the collection to load</param>
        /// <param name="selectQuery">The select query to use to load from the data source</param>
        /// <returns>The loaded collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, ISelectQuery selectQuery)
        {
            IBusinessObjectCollection col = CreateCollectionOfType(classDef.ClassType);
            col.SelectQuery = selectQuery;
            Refresh(col);
            return col;
        }

        #endregion

        protected static IBusinessObjectCollection CreateCollectionOfType(Type BOType)
        {
            Type boColType = typeof(BusinessObjectCollection<>).MakeGenericType(BOType);
            return (IBusinessObjectCollection)Activator.CreateInstance(boColType);
        }

    }
}
