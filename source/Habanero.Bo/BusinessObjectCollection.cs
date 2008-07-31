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
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Comparer;
using Habanero.BO.CriteriaManager;
using Habanero.DB;

namespace Habanero.BO
{
    //public delegate void BusinessObjectEventHandler(Object sender, BOEventArgs e);

    /// <summary>
    /// Manages a collection of business objects.  This class also serves
    /// as a base class from which most types of business object collections
    /// can be derived.<br/>
    /// To create a collection of business objects, inherit from this 
    /// class. The business objects contained in this collection must
    /// inherit from BusinessObject.
    /// </summary>
    public class BusinessObjectCollection<TBusinessObject>
        : List<TBusinessObject>, IBusinessObjectCollection
        where TBusinessObject : class, IBusinessObject, new()
    {
        private readonly ClassDef _boClassDef;
//        private Criteria _criteriaExpression;
//        private string _orderByClause;
        private readonly IBusinessObject _sampleBo;
//        private string _extraSearchCriteriaLiteral = "";
//        private int _limit = -1;
        private readonly Hashtable _lookupTable;
        private readonly List<TBusinessObject> _createdBusinessObjects = new List<TBusinessObject>();
        private ISelectQuery _selectQuery;

        /// <summary>
        /// Default constructor. 
        /// The classdef will be implied from TBusinessObject and the Current Database Connection will be used.
        /// </summary>
        public BusinessObjectCollection()
            : this(null, null)
        {
        }

        /// <summary>
        /// Use this constructor if you will only know TBusinessObject at run time - BusinessObject will be the generic type
        /// and the objects in the collection will be determined from the classDef passed in.
        /// </summary>
        /// <param name="classDef">The classdef of the objects to be contained in this collection</param>
        public BusinessObjectCollection(IClassDef classDef)
            : this(classDef, null)
        {
        }

        /// <summary>
        /// Constructor to initialise a new collection with a
        /// class definition provided by an existing business object
        /// </summary>
        /// <param name="bo">The business object whose class definition
        /// is used to initialise the collection</param>
        public BusinessObjectCollection(TBusinessObject bo)
            : this(null, bo)
        {
        }

        ///// <summary>
        ///// Constructor to initialise a new collection with a specified Database Connection.
        ///// This Database Connection will be used to load the collection.
        ///// </summary>
        ///// <param name="databaseConnection">The Database Connection to used to load the collection</param>
        //public BusinessObjectCollection(IDatabaseConnection databaseConnection)
        //    : this(null, null)
        //{
        //    _sampleBo.SetDatabaseConnection(databaseConnection);
        //}

        private BusinessObjectCollection(IClassDef classDef, TBusinessObject sampleBo)
        {
            if (classDef == null)
            {
                if (sampleBo == null)
                {
                    _boClassDef = ClassDefinition.ClassDef.ClassDefs[typeof (TBusinessObject)];
                }
                else
                {
                    _boClassDef = (ClassDef) sampleBo.ClassDef;
                }
            }
            else
            {
                _boClassDef = (ClassDef) classDef;
            }
            if (sampleBo != null)
            {
                _sampleBo = sampleBo;
            }
            else
            {
                if (_boClassDef == null)
                {
                    throw new HabaneroDeveloperException(String.Format("A business object collection is " +
                                                          "being created for the type '{0}', but no class definitions have " +
                                                          "been loaded for this type.", typeof (TBusinessObject).Name),
                                                          "Class Definitions not loaded");
                }
                _sampleBo = _boClassDef.CreateNewBusinessObject();
//                BusinessObject.AllLoadedBusinessObjects().Remove(_sampleBo.ID.GetObjectId());
            }  
            _lookupTable = new Hashtable();
            _selectQuery = QueryBuilder.CreateSelectQuery(_boClassDef);
        }

        /// <summary>
        /// Handles the event of a business object being added
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectAdded;

        /// <summary>
        /// Handles the event of a business object being removed
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectRemoved;

        ///// <summary>
        ///// Handles the event of any business object in this collection being edited
        ///// </summary>
        //public event EventHandler<BOEventArgs> BusinessObjectEdited;

        /// <summary>
        /// Calls the BusinessObjectAdded() handler
        /// </summary>
        /// <param name="bo">The business object added</param>
        public void FireBusinessObjectAdded(TBusinessObject bo)
        {
            if (this.BusinessObjectAdded != null)
            {
                this.BusinessObjectAdded(this, new BOEventArgs(bo));
            }
        }

        /// <summary>
        /// Calls the BusinessObjectRemoved() handler
        /// </summary>
        /// <param name="bo">The business object removed</param>
        public void FireBusinessObjectRemoved(TBusinessObject bo)
        {
            if (this.BusinessObjectRemoved != null)
            {
                this.BusinessObjectRemoved(this, new BOEventArgs(bo));
            }
        }

        ///// <summary>
        ///// Calls the BusinessObjectRemoved() handler
        ///// </summary>
        ///// <param name="bo">The business object removed</param>
        //public void FireBusinessObjectEdited(TBusinessObject bo)
        //{
        //    if (this.BusinessObjectEdited != null)
        //    {
        //        this.BusinessObjectEdited(this, new BOEventArgs(bo));
        //    }
        //}

        /// <summary>
        /// Adds a business object to the collection
        /// </summary>
        /// <param name="bo">The business object to add</param>
        public new void Add(TBusinessObject bo)
        {
            if (bo == null) throw new ArgumentNullException("bo");
            AddInternal(bo);
            this.FireBusinessObjectAdded(bo);
        }

        internal void AddInternal(TBusinessObject bo)
        {
            if (bo == null) throw new ArgumentNullException("bo");
        
            base.Add(bo);
            _lookupTable.Add(bo.ID.ToString(), bo);
            bo.Deleted += BusinessObjectDeletedHandler;
            if (bo.ID != null)
            {
                bo.ID.Updated += UpdateLookupTable;
            }
        }

        /// <summary>
        /// Updates the lookup table when a primary key property has
        /// changed
        /// </summary>
        private void UpdateLookupTable(object sender, BOKeyEventArgs e)
        {
            string oldID = e.BOKey.PropertyValueStringBeforeLastEdit();
            if (_lookupTable.Contains(oldID))
            {
                BusinessObject bo = (BusinessObject) _lookupTable[oldID];
                _lookupTable.Remove(oldID);
                _lookupTable.Add(bo.ID.ToString(), bo);
            }
        }

        /// <summary>
        /// Handles the event of a business object being deleted
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectDeletedHandler(object sender, BOEventArgs e)
        {
            this.RemoveInternal((TBusinessObject) e.BusinessObject);
        }

        /// <summary>
        /// Copies the business objects in one collection across to this one
        /// </summary>
        /// <param name="col">The collection to copy from</param>
        public void Add(BusinessObjectCollection<TBusinessObject> col)
        {
            Add((List<TBusinessObject>) col);
            //foreach (TBusinessObject bo in col)
            //{
            //    this.Add(bo);
            //}
        }

        /// <summary>
        /// Adds the business objects from col into this collection
        /// </summary>
        /// <param name="col"></param>
        public void Add(IEnumerable<TBusinessObject> col)
        {
            foreach (TBusinessObject bo in col)
            {
                this.Add(bo);
            }
        }

        ///<summary>
        /// Adds the specified business objects to this collection
        ///</summary>
        ///<param name="businessObjects">A parameter array of business objects to add to the collection</param>
        public void Add(params TBusinessObject[] businessObjects)
        {
            Add(new List<TBusinessObject>(businessObjects));
        }

        ///// <summary>
        ///// Provides a numerical indexing facility so that the objects
        ///// in the collection can be accessed with square brackets like an array
        ///// </summary>
        ///// <param name="index">The index position</param>
        ///// <returns>Returns the business object at the position specified</returns>
        //public BusinessObject this[int index]
        //{
        //    get { return (BusinessObject) _list[index]; }
        //}

        /// <summary>
        /// Refreshes the business objects in the collection
        /// </summary>
        [ReflectionPermission(SecurityAction.Demand)]
        public void Refresh()
        {
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(this);
            //BusinessObjectCollection<TBusinessObject> oldCol = this.Clone();
            //Clear();
            //IDatabaseConnection boDatabaseConnection = DatabaseConnection.CurrentConnection;
            //ISqlStatement refreshSql = CreateLoadSqlStatement((BusinessObject) _sampleBo, _boClassDef,
            //    _criteriaExpression, _limit, _extraSearchCriteriaLiteral, _orderByClause);
            //using (IDataReader dr = boDatabaseConnection.LoadDataReader(refreshSql))
            //{
            //    try
            //    {
            //        while (dr.Read())
            //        {
            //            TBusinessObject bo = (TBusinessObject)BOLoader.Instance.GetBusinessObject(_sampleBo, dr);
            //            if (Contains(bo)) continue;
            //            if (oldCol.Contains(bo))
            //            {
            //                AddInternal(bo);
            //            }
            //            else
            //            {
            //                Add(bo);
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        if (dr != null && !dr.IsClosed)
            //        {
            //            dr.Close();
            //        }
            //    }
            //}
        }

        #region Create Load Statement

        internal static ISqlStatement CreateLoadSqlStatement(BusinessObject businessObject, ClassDef classDef,
                                                             IExpression criteriaExpression, int limit,
                                                             string extraSearchCriteriaLiteral, string orderByClause)
        {
            IDatabaseConnection boDatabaseConnection =  DatabaseConnection.CurrentConnection;
            ISqlStatement loadSqlStatement = new SqlStatement(boDatabaseConnection);
            loadSqlStatement.Statement.Append(businessObject.GetSelectSql(limit));
            if (criteriaExpression != null)
            {
                IExpression loadCriteria = criteriaExpression.Clone();
                DetermineJoins(loadSqlStatement, classDef, loadCriteria, ref orderByClause, boDatabaseConnection);
                loadSqlStatement.AppendWhere();
                SqlCriteriaCreator creator = new SqlCriteriaCreator(loadCriteria, classDef, boDatabaseConnection);
                creator.AppendCriteriaToStatement(loadSqlStatement);
            } else
            {
                DetermineJoins(loadSqlStatement, classDef, null, ref orderByClause, boDatabaseConnection);
            }
            if (!String.IsNullOrEmpty(extraSearchCriteriaLiteral))
            {
                loadSqlStatement.AppendWhere();
                loadSqlStatement.Statement.Append(extraSearchCriteriaLiteral);
            }

            if (limit > 0)
            {
                string limitClause = boDatabaseConnection.GetLimitClauseForEnd(limit);
                if (!String.IsNullOrEmpty(limitClause)) loadSqlStatement.Statement.Append(" " + limitClause);
            }
            loadSqlStatement.AppendOrderBy(orderByClause);
            return loadSqlStatement;
        }

        private static void DetermineJoins(ISqlStatement sqlStatement, ClassDef classDef, IExpression criteriaExpression, 
            ref string orderByClause, IDatabaseConnection databaseConnection)
        {
            List<string> joinedRelationshipTables = new List<string>();
            DetermineCriteriaParameterJoins(sqlStatement, classDef, databaseConnection, criteriaExpression, joinedRelationshipTables);
            DetermineOrderByParameterJoins(sqlStatement, classDef, databaseConnection, ref orderByClause, joinedRelationshipTables);
        }

        private static void DetermineOrderByParameterJoins(ISqlStatement sqlStatement, ClassDef classDef, 
            IDatabaseConnection databaseConnection, ref string orderByClause, List<string> joinedRelationshipTables)
        {
            if (String.IsNullOrEmpty(orderByClause)) return;
            //if (orderByClause.IndexOf(".") == -1) return;
            string[] orderByParameters = orderByClause.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            List<string> reconstructedParameters = new List<string>();
            List<string> orderByColumns = new List<string>();
            foreach (string thisOrderByParameter in orderByParameters)
            {
                string orderByParameter = thisOrderByParameter.Trim();
                bool isRelationshipParameter = IsRelationshipParameter(orderByParameter);
                string[] parts = orderByParameter.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0)
                {
                    string parameterName = parts[0];
                    if (isRelationshipParameter)
                    {
                        string fullTableName;
                        IPropDef propDef;
                        InsertJoinsForRelatedProperty(sqlStatement, classDef, parameterName, databaseConnection,
                                                      ref joinedRelationshipTables, out fullTableName, out propDef);
                        if (propDef == null)
                        {
                            throw new HabaneroDeveloperException("An application error has occured please contact your system administrator", "The prop def for '" + parameterName + "' is null");
                        }
                        string newParameterName = SqlFormattingHelper.FormatTableAndFieldName(
                            fullTableName, propDef.DatabaseFieldName, databaseConnection);
                        //string newParameterName = fullTableName + "." + propDef.DatabaseFieldName;
                        parts[0] = newParameterName;
                        orderByParameter = String.Join(" ", parts);
                    } else
                    {
                        IPropDef propDef = classDef.GetPropDef(parameterName, false);
                        if (propDef != null)
                        {
                            string fullTableName = classDef.GetTableName(propDef);
                            string newParameterName = SqlFormattingHelper.FormatTableAndFieldName(
                                fullTableName, propDef.DatabaseFieldName, databaseConnection);
                            parts[0] = newParameterName;
                            orderByParameter = String.Join(" ", parts);
                        }
                    }
                    orderByColumns.Add(parts[0]);
                }
                reconstructedParameters.Add(orderByParameter);
            }
            orderByClause = String.Join(", ", reconstructedParameters.ToArray());
            if (orderByColumns.Count > 0)
            {
                //This code adds the order by fields to the select statement, this is specifically required by SQL Server
                sqlStatement.AddSelectFields(orderByColumns);
            }
            return;
        }
        
        private static void DetermineCriteriaParameterJoins(ISqlStatement sqlStatement, ClassDef classDef, IDatabaseConnection databaseConnection, IExpression criteriaExpression, List<string> joinedRelationshipTables)
        {
            if (criteriaExpression == null) return;
            List<Parameter> relationshipParameters = new List<Parameter>();
            AnalyzeExpressionForRelationshipParameters(criteriaExpression, ref relationshipParameters);
            Dictionary<string, IParameterSqlInfo> relationshipParameterInformation =
                new Dictionary<string, IParameterSqlInfo>();
            foreach (Parameter parameter in relationshipParameters)
            {
                string parameterName = parameter.ParameterName;
                string fullTableName;
                IPropDef propDef;
                InsertJoinsForRelatedProperty(sqlStatement, classDef, parameterName, databaseConnection,
                                              ref joinedRelationshipTables, out fullTableName, out propDef);

                if (propDef != null && !relationshipParameterInformation.ContainsKey(parameterName))
                {
                    PropDefParameterSQLInfo parameterSQLInfo = new PropDefParameterSQLInfo(
                        parameterName, propDef, fullTableName);
                    relationshipParameterInformation.Add(parameterName, parameterSQLInfo);
                }
            }
            foreach (KeyValuePair<string, IParameterSqlInfo> keyValuePair in relationshipParameterInformation)
            {
                criteriaExpression.SetParameterSqlInfo(keyValuePair.Value);
            }
        }

        /// <summary>
        /// This method adds the necessary joins to a sql statement for a Related Property for the specified class.
        /// For example: If the class is 'Invoice' and the "Parameter Name" is 'Order.Customer.Name' then joins 
        /// will be added to the sql statement from the 'Invoice' table to the 'Order' table to the 'Customer' table 
        /// so that the 'Name' field can be used.
        /// </summary>
        /// <param name="sqlStatement">The SQL Statement to be updated with the joins if necessary.</param>
        /// <param name="classDef">The classDef for which the SQL Statement is being built.</param>
        /// <param name="parameterName">The parameter that is being used</param>
        /// <param name="databaseConnection">The database connection to use to format the SQL.</param>
        /// <param name="alreadyJoinedRelationshipTables">A List of full table names that have already 
        /// been joined for this SQL Statement (This is used to avoid adding a duplicate join).</param>
        /// <param name="fullTableName">A return parameter to get the constructed Alias for this joined table.</param>
        /// <param name="propDef">A return parameter to get the propDef that represents the related property.</param>
        private static void InsertJoinsForRelatedProperty(ISqlStatement sqlStatement, ClassDef classDef, string parameterName, 
            IDatabaseConnection databaseConnection, ref List<string> alreadyJoinedRelationshipTables, out string fullTableName, out IPropDef propDef)
        {
            string[] parts = parameterName.Split('.');
            string propertyName = parts[parts.Length - 1];
            ClassDef currentClassDef = classDef;
            fullTableName = currentClassDef.InheritedTableName;
            for (int i = 0; i < parts.Length - 1; i++)
            {
                string relationshipName = parts[i];
                string previousFullTableName = fullTableName;
                fullTableName += relationshipName;
                RelationshipDef relationshipDef = currentClassDef.GetRelationship(relationshipName);
                if (relationshipDef != null)
                {
                    if (relationshipDef.RelatedObjectClassDef == null)
                    {
                        throw new SqlStatementException(String.Format(
                                                            "The relationship '{0}' of the class '{1}' referred to in " +
                                                            "the Business Object Collection load criteria in the parameter " +
                                                            "'{2}' refers to the class '{3}' from the assembly '{4}'. " +
                                                            "This related class is not found in the loaded class definitions.",
                                                            relationshipName, currentClassDef.ClassName,
                                                            parameterName,
                                                            relationshipDef.RelatedObjectClassName,
                                                            relationshipDef.RelatedObjectAssemblyName));
                    }
                    currentClassDef = relationshipDef.RelatedObjectClassDef;
                }
                else
                {
                    throw new SqlStatementException(String.Format("The relationship '{0}' of the class '{1}'" +
                                                                  " referred to in the Business Object Collection load criteria in the parameter '{2}' does not exist.",
                                                                  relationshipName, currentClassDef.ClassName,
                                                                  parameterName));
                }
                if (alreadyJoinedRelationshipTables.Contains(fullTableName))
                {
                    //Dont add join
                }
                else
                {
                    //Add join
                    string joinTableAs =
                        SqlFormattingHelper.FormatTableName(currentClassDef.InheritedTableName, databaseConnection);
                    joinTableAs += " AS ";
                    joinTableAs += SqlFormattingHelper.FormatTableName(fullTableName, databaseConnection);
                    string joinCriteria = "";
                    foreach (RelPropDef relPropDef in relationshipDef.RelKeyDef)
                    {
                        joinCriteria += SqlFormattingHelper.FormatTableAndFieldName(
                            previousFullTableName, relPropDef.OwnerPropertyName, databaseConnection);
                        joinCriteria += " = ";
                        joinCriteria += SqlFormattingHelper.FormatTableAndFieldName(
                            fullTableName, relPropDef.RelatedClassPropName, databaseConnection);
                    }
                    sqlStatement.AddJoin("LEFT JOIN", joinTableAs, joinCriteria);
                    alreadyJoinedRelationshipTables.Add(fullTableName);
                }
            }
            propDef = null;
            if (currentClassDef != null)
            {
                propDef = currentClassDef.GetPropDef(propertyName, false);
            }
        }

        private static void AnalyzeExpressionForRelationshipParameters(IExpression criteriaExpression, ref List<Parameter> parameters)
        {
            if (criteriaExpression is Expression)
            {
                Expression expression = criteriaExpression as Expression;
                GetRelationshipParameters(expression.LeftExpression, ref parameters);
                GetRelationshipParameters(expression.RightExpression, ref parameters);
            }
            else if (criteriaExpression is Parameter)
            {
                GetRelationshipParameters(criteriaExpression, ref parameters);
            }
        }

        private static void GetRelationshipParameters(IExpression expression, ref List<Parameter> parameters)
        {
            if (expression is Parameter)
            {
                Parameter parameter = (Parameter) expression;
                if (IsRelationshipParameter(parameter))
                {
                    parameters.Add(parameter);
                }
            }
            else
            {
                AnalyzeExpressionForRelationshipParameters(expression, ref parameters);
            }
        }

        private static bool IsRelationshipParameter(Parameter parameter)
        {
            string parameterName = parameter.ParameterName;
            return IsRelationshipParameter(parameterName);
        }

        private static bool IsRelationshipParameter(string parameterName)
        {
            return parameterName.Contains(".");
        }

        #endregion //Create Load Statement

        #region Load Methods

        /// <summary>
        /// Loads the entire collection for the type of object.
        /// </summary>
        public void LoadAll()
        {
            LoadAll("");
        }

        /// <summary>
        /// Loads the entire collection for the type of object,
        /// loaded in the order specified. 
        /// To load the collection in any order use the LoadAll() method.
        /// </summary>
        /// <param name="orderByClause">The order-by clause</param>
        public void LoadAll(string orderByClause)
        {
            Load("", orderByClause);
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided,
        /// loaded in the order specified.  
        /// Use empty quotes, (or the LoadAll method) to load the
        /// entire collection for the type of object.
        /// </summary>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        public void Load(string searchCriteria, string orderByClause)
        {
            LoadWithLimit(searchCriteria, orderByClause, -1);
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided in
        /// an expression, loaded in the order specified
        /// </summary>
        /// <param name="searchExpression">The search expression</param>
        /// <param name="orderByClause">The order-by clause</param>
        public void Load(Criteria searchExpression, string orderByClause)
        {
            LoadWithLimit(searchExpression, orderByClause, -1);
        }


        ///// <summary>
        ///// Loads business objects that match the search criteria provided
        ///// and an extra criteria literal,
        ///// loaded in the order specified
        ///// </summary>
        ///// <param name="searchCriteria">The search criteria</param>
        ///// <param name="orderByClause">The order-by clause</param>
        ///// <param name="extraSearchCriteriaLiteral">Extra search criteria</param>
        //public void Load(string searchCriteria, string orderByClause, string extraSearchCriteriaLiteral)
        //{
        //    LoadWithLimit(searchCriteria, orderByClause, extraSearchCriteriaLiteral, -1);
        //}

        ///// <summary>
        ///// Loads business objects that match the search criteria provided in
        ///// an expression and an extra criteria literal, 
        ///// loaded in the order specified
        ///// </summary>
        ///// <param name="searchExpression">The search expression</param>
        ///// <param name="orderByClause">The order-by clause</param>
        ///// <param name="extraSearchCriteriaLiteral">Extra search criteria</param>
        ///// TODO ERIC - what is the last one?
        //public void Load(IExpression searchExpression, string orderByClause, string extraSearchCriteriaLiteral)
        //{
        //    LoadWithLimit(searchExpression, orderByClause, extraSearchCriteriaLiteral, -1);
        //}

//        /// <summary>
//        /// Loads business objects that match the search criteria provided, 
//        /// loaded in the order specified, 
//        /// and limiting the number of objects loaded
//        /// </summary>
//        /// <param name="searchCriteria">The search criteria</param>
//        /// <param name="orderByClause">The order-by clause</param>
//        /// <param name="limit">The limit</param>
//        public void LoadWithLimit(string searchCriteria, string orderByClause, int limit)
//        {
//            LoadWithLimit(searchCriteria, orderByClause, limit);
//        }
        /// <summary>
        /// Loads business objects that match the search criteria provided
        /// and an extra criteria literal, 
        /// loaded in the order specified, 
        /// and limiting the number of objects loaded
        /// </summary>
        /// <param name="searchCriteria">The search expression</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <param name="limit">The limit</param>
        public void LoadWithLimit(string searchCriteria, string orderByClause, int limit)
        {
            Criteria criteriaExpression = null;
            if (searchCriteria.Length > 0)
            {
                criteriaExpression = CriteriaParser.CreateCriteria(searchCriteria);
            }
            LoadWithLimit(criteriaExpression, orderByClause, limit);
        }
//        /// <summary>
//        /// Loads business objects that match the search criteria provided in
//        /// an expression, loaded in the order specified, 
//        /// and limiting the number of objects loaded
//        /// </summary>
//        /// <param name="searchExpression">The search expression</param>
//        /// <param name="orderByClause">The order-by clause</param>
//        /// <param name="limit">The limit</param>
//        public void LoadWithLimit(IExpression searchExpression, string orderByClause, int limit)
//        {
//            LoadWithLimit(searchExpression, orderByClause, limit);
//        }



        /// <summary>
        /// Loads business objects that match the search criteria provided in
        /// an expression and an extra criteria literal, 
        /// loaded in the order specified, 
        /// and limiting the number of objects loaded
        /// </summary>
        /// <param name="searchExpression">The search expression</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <param name="limit">The limit</param>
        public void LoadWithLimit(Criteria searchExpression, string orderByClause, int limit)
        {
            this.SelectQuery.Criteria = searchExpression;

            this.SelectQuery.OrderCriteria = QueryBuilder.CreateOrderCriteria(this.ClassDef, orderByClause);
            if (limit > -1) this.SelectQuery.Limit = limit;

            Refresh();
        }

        #endregion

        ///// <summary>
        ///// Returns the business object at the index position specified
        ///// </summary>
        ///// <param name="index">The index position</param>
        ///// <returns>Returns the business object at that position</returns>
        //public BusinessObject item(int index)
        //{
        //    return (BusinessObject) _list[index];
        //}

        /// <summary>
        /// Clears the collection
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            _lookupTable.Clear();
        }

        /// <summary>
        /// Removes the business object at the index position specified
        /// </summary>
        /// <param name="index">The index position to remove from</param>
        public new void RemoveAt(int index)
        {
            TBusinessObject boToRemove = this[index];
            Remove(boToRemove);
            //base.RemoveAt(index);
            //_lookupTable.Remove(boToRemove.ID.ToString());
            //boToRemove.Deleted -= BusinessObjectDeletedHandler;
            //this.FireBusinessObjectRemoved(boToRemove);
        }

        /// <summary>
        /// Removes the specified business object from the collection
        /// </summary>
        /// <param name="bo">The business object to remove</param>
        public new virtual bool Remove(TBusinessObject bo)
        {
            return RemoveInternal(bo);
        }

        /// <summary>
        /// Removes the specified business object from the collection. This is used when refreshing
        /// a collection so that any overriden behaviour (from overriding Remove) is not applied
        /// when loading and refreshing.
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        internal bool RemoveInternal(TBusinessObject businessObject)
        {
            bool removed = base.Remove(businessObject);
            _lookupTable.Remove(businessObject.ID.ToString());
            businessObject.Deleted -= BusinessObjectDeletedHandler;
            this.FireBusinessObjectRemoved(businessObject);
            return removed;
        }

        ///// <summary>
        ///// Indicates whether the collection contains the specified 
        ///// business object
        ///// </summary>
        ///// <param name="bo">The business object</param>
        ///// <returns>Returns true if contained</returns>
        //public bool Contains(TBusinessObject bo)
        //{
        //    return Contains(bo);
        //}

        /// <summary>
        /// Indicates whether any of the business objects have been amended 
        /// since they were last persisted
        /// </summary>
        public bool IsDirty
        {
            get
            {
                foreach (TBusinessObject child in this)
                {
                    if (child.State.IsDirty)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Indicates whether all of the business objects in the collection
        /// have valid values
        /// </summary>
        /// <returns>Returns true if all are valid</returns>
        public bool IsValid()
        {
            foreach (TBusinessObject child in this)
            {
                if (!child.IsValid())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Indicates whether all of the business objects in the collection
        /// have valid values, amending an error message if any object is
        /// invalid
        /// </summary>
        /// <param name="errorMessage">An error message to amend</param>
        /// <returns>Returns true if all are valid</returns>
        public bool IsValid(out string errorMessage)
        {
            errorMessage = "";
            foreach (TBusinessObject child in this)
            {
                if (!child.IsValid(out errorMessage))
                {
                    return false;
                }
            }
            return true;
        }


        ///<summary>
        /// The select query that is used to load this business object collection.
        ///</summary>
        public ISelectQuery SelectQuery
        {
            get { return _selectQuery; }
            set
            {
                if (value == null)
                {
                    throw new HabaneroDeveloperException("A collections select query cannot be set to null", 
                            "A collections select query cannot be set to null");
                }
                _selectQuery = value;
            }
        }

        /// <summary>
        /// Finds a business object that has the key string specified.<br/>
        /// Note: the format of the search term is strict, so that a Guid ID
        /// may be stored as "boIDname=########-####-####-####-############".
        /// In the case of such Guid ID's, rather use the FindByGuid() function.
        /// Composite primary keys may be stored otherwise, such as a
        /// concatenation of the key names.
        /// </summary>
        /// <param name="key">The orimary key as a string</param>
        /// <returns>Returns the business object if found, or null if not</returns>
        public TBusinessObject Find(string key)
        {
            if (_lookupTable.ContainsKey(key))
            {
                TBusinessObject bo = (TBusinessObject) _lookupTable[key];
                if (this.Contains(bo))
                    return (TBusinessObject) _lookupTable[key];
                return null;
            }
            return null;
//			foreach (BusinessObjectBase bo in this._list) {
//				if (bo.StrID() == strID) {
//					return bo;
//				}
//			}
//			return null;
        }

        /// <summary>
        /// Finds a business object in the collection that contains the
        /// specified Guid ID
        /// </summary>
        /// <param name="searchTerm">The Guid to search for</param>
        /// <returns>Returns the business object if found, or null if not
        /// found</returns>
        public TBusinessObject FindByGuid(Guid searchTerm)
        {
//            string formattedSearchItem = searchTerm.ToString();
//            formattedSearchItem.Replace("{", "");
//            formattedSearchItem.Replace("}", "");
//            formattedSearchItem.Insert(0, _boClassDef.PrimaryKeyDef.KeyName + "=");

            string formattedSearchItem =
                string.Format("{0}={1}", _boClassDef.GetPrimaryKeyDef().KeyName, searchTerm.ToString("B"));

            if (_lookupTable.ContainsKey(formattedSearchItem))
            {
                return (TBusinessObject) _lookupTable[formattedSearchItem];
            }
            return null;
        }

        /// <summary>
        /// Returns the class definition of the collection
        /// </summary>
        public IClassDef ClassDef
        {
            get { return _boClassDef; }
        }

        /// <summary>
        /// Returns a sample business object held by the collection, which is
        /// constructed from the class definition
        /// </summary>
        public IBusinessObject SampleBo
        {
            get { return _sampleBo; }
        }

        /// <summary>
        /// Returns an intersection of the set of objects held in this
        /// collection with the set in another specified collection (an
        /// intersection refers to a set of objects held in common between
        /// two sets)
        /// </summary>
        /// <param name="col2">Another collection to intersect with</param>
        /// <returns>Returns a new collection containing the intersection</returns>
        public BusinessObjectCollection<TBusinessObject> Intersection(BusinessObjectCollection<TBusinessObject> col2)
        {
            BusinessObjectCollection<TBusinessObject> intersectionCol = new BusinessObjectCollection<TBusinessObject>();
            foreach (TBusinessObject businessObjectBase in this)
            {
                if (col2.Contains(businessObjectBase))
                {
                    intersectionCol.Add(businessObjectBase);
                }
            }
            return intersectionCol;
        }

        /// <summary>
        /// Returns a union of the set of objects held in this
        /// collection with the set in another specified collection (a
        /// union refers to a set of all objects held in either of two sets)
        /// </summary>
        /// <param name="col2">Another collection to unite with</param>
        /// <returns>Returns a new collection containing the union</returns>
        public BusinessObjectCollection<TBusinessObject> Union(BusinessObjectCollection<TBusinessObject> col2)
        {
            BusinessObjectCollection<TBusinessObject> unionCol = new BusinessObjectCollection<TBusinessObject>();
            foreach (TBusinessObject businessObjectBase in this)
            {
                unionCol.Add(businessObjectBase);
            }
            foreach (TBusinessObject businessObjectBase in col2)
            {
                if (!unionCol.Contains(businessObjectBase))
                {
                    unionCol.Add(businessObjectBase);
                }
            }

            return unionCol;
        }

        /// <summary>
        /// Returns a new collection that is a copy of this collection
        /// </summary>
        /// <returns>Returns the cloned copy</returns>
        public BusinessObjectCollection<TBusinessObject> Clone()
        {
            BusinessObjectCollection<TBusinessObject> clonedCol =
                new BusinessObjectCollection<TBusinessObject>(_boClassDef);
            foreach (TBusinessObject businessObjectBase in this)
            {
                clonedCol.Add(businessObjectBase);
            }
            return clonedCol;
        }

        /// <summary>
        /// Returns a new collection that is a copy of this collection
        /// </summary>
        /// <returns>Returns the cloned copy</returns>
        public BusinessObjectCollection<DestType> Clone<DestType>()
            where DestType : BusinessObject, new()
        {
            BusinessObjectCollection<DestType> clonedCol = new BusinessObjectCollection<DestType>(_boClassDef);
            if (!typeof (DestType).IsSubclassOf(typeof (TBusinessObject)) &&
                !typeof (TBusinessObject).IsSubclassOf(typeof (DestType)) &&
                !typeof (TBusinessObject).Equals(typeof (DestType)))
            {
                throw new InvalidCastException(String.Format("Cannot cast a collection of type '{0}' to " +
                                                             "a collection of type '{1}'.",
                                                             typeof (TBusinessObject).Name, typeof (DestType).Name));
            }
            foreach (TBusinessObject businessObject in this)
            {
                DestType obj = businessObject as DestType;
                if (obj != null)
                {
                    clonedCol.Add(obj);
                }
            }
            return clonedCol;
        }

        /// <summary>
        /// Sorts the collection by the property specified. The second parameter
        /// indicates whether this property is a business object property or
        /// whether it is a property defined in the code.  For example, a full name
        /// would be a code-calculated property that is not itself a business
        /// object property, even though it uses the BO properties of first name
        /// and surname, and the argument would thus be set as false.
        /// </summary>
        /// <param name="propertyName">The property name to sort on</param>
        /// <param name="isBoProperty">Whether the property is a business
        /// object property</param>
        /// <param name="isAscending">Whether to sort in ascending order, set
        /// false for descending order</param>
        public void Sort(string propertyName, bool isBoProperty, bool isAscending)
        {
            if (isBoProperty)
            {
                Sort(ClassDef.GetPropDef(propertyName).GetPropertyComparer<TBusinessObject>());
            }
            else
            {
                Sort(new ReflectedPropertyComparer<TBusinessObject>(propertyName));
            }

            if (!isAscending)
            {
                Reverse();
            }
        }

        void IBusinessObjectCollection.Sort(IComparer comparer)
        {
            this.Sort(new StronglyTypedComperer<TBusinessObject>(comparer));
        }

        #region StronglyTypedComperer 
        private class StronglyTypedComperer<T> : IComparer<T>
        {
            private readonly IComparer _comparer;

            public StronglyTypedComperer(IComparer comparer)
            {
                _comparer = comparer;
            }

            public int Compare(T x, T y)
            {
                return _comparer.Compare(x, y);
            }
        }
        #endregion

        /// <summary>
        /// Returns a list containing all the objects sorted by the property
        /// name and in the order specified
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <param name="isAscending">True for ascending, false for descending
        /// </param>
        /// <returns>Returns a sorted list</returns>
        public List<TBusinessObject> GetSortedList(string propertyName, bool isAscending)
        {
            List<TBusinessObject> list = new List<TBusinessObject>(this.Count);
            foreach (TBusinessObject o in this)
            {
                list.Add(o);
            }
            list.Sort(ClassDef.GetPropDef(propertyName).GetPropertyComparer<TBusinessObject>());
            if (!isAscending)
            {
                list.Reverse();
            }
            return list;
        }

        /// <summary>
        /// Returns a copied business object collection with the objects sorted by 
        /// the property name and in the order specified
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <param name="isAscending">True for ascending, false for descending
        /// </param>
        /// <returns>Returns a sorted business object collection</returns>
        public BusinessObjectCollection<TBusinessObject> GetSortedCollection(string propertyName, bool isAscending)
        {
            //test
            BusinessObjectCollection<TBusinessObject> sortedCol = new BusinessObjectCollection<TBusinessObject>();
            foreach (TBusinessObject bo in GetSortedList(propertyName, isAscending))
            {
                sortedCol.Add(bo);
            }
            return sortedCol;
        }

        /// <summary>
        /// Returns the business object collection as a List
        /// </summary>
        /// <returns>Returns an IList object</returns>
        public List<TBusinessObject> GetList()
        {
            List<TBusinessObject> list = new List<TBusinessObject>(this.Count);
            foreach (TBusinessObject o in this)
            {
                list.Add(o);
            }
            return list;
        }

        /// <summary>
        /// Commits to the database all the business objects that are either
        /// new or have been altered since the last committal
        /// </summary>
        public virtual void SaveAll()
        {
            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();

            // Transaction transaction = new Transaction(DatabaseConnection.CurrentConnection);
            SaveAllInTransaction(committer);
        }

        protected virtual void SaveAllInTransaction(ITransactionCommitter transaction)
        {
            foreach (TBusinessObject bo in this)
            {
                if (bo.State.IsDirty || bo.State.IsNew)
                {
                    transaction.AddBusinessObject(bo);
                }
            }
            foreach (TBusinessObject bo in this._createdBusinessObjects)
            {
                transaction.AddBusinessObject(bo);
            }
            transaction.CommitTransaction();
            _createdBusinessObjects.Clear();
        }

        /// <summary>
        /// Restores all the business objects to their last persisted state, that
        /// is their state and values at the time they were last saved to the database
        /// </summary>
        /// TODO: Consider implications for the lookup table and any other caching
        public void RestoreAll()
        {
            foreach (TBusinessObject bo in this)
            {
                bo.Restore();
            }
        }

        #region IBusinessObjectCollection Members

        // TODO ERIC: are these really necessary? and if so, why aren't all of them copied
        // across from IBusinessObjectCollection?

        /// <summary>
        /// Finds a business object that has the key string specified.<br/>
        /// Note: the format of the search term is strict, so that a Guid ID
        /// may be stored as "boIDname=########-####-####-####-############".
        /// In the case of such Guid ID's, rather use the FindByGuid() function.
        /// Composite primary keys may be stored otherwise, such as a
        /// concatenation of the key names.
        /// </summary>
        /// <param name="key">The orimary key as a string</param>
        /// <returns>Returns the business object if found, or null if not</returns>
        IBusinessObject IBusinessObjectCollection.Find(string key)
        {
            return this.Find(key);
        }

        /// <summary>
        /// Returns a new collection that is a copy of this collection
        /// </summary>
        /// <returns>Returns the cloned copy</returns>
        IBusinessObjectCollection IBusinessObjectCollection.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        /// <returns>
        /// The index of item if found in the list; otherwise, -1.
        /// </returns>
        int IBusinessObjectCollection.IndexOf(IBusinessObject item)
        {
            return this.IndexOf((TBusinessObject) item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.
        /// </summary>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        void IBusinessObjectCollection.Insert(int index, IBusinessObject item)
        {
            this.Insert(index, (TBusinessObject) item);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        /// <returns>The element at the specified index.</returns>
        IBusinessObject IBusinessObjectCollection.this[int index]
        {
            get { return base[index]; }
            set { base[index] = (TBusinessObject) value; }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        void IBusinessObjectCollection.Add(IBusinessObject item)
        {
            this.Add((TBusinessObject) item);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <returns>
        /// True if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        bool IBusinessObjectCollection.Contains(IBusinessObject item)
        {
            return this.Contains((TBusinessObject) item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="T:System.ArgumentNullException">Array is null.</exception>
        /// <exception cref="T:System.ArgumentException">Array is multidimensional or arrayIndex
        /// is equal to or greater than the length of array.-or-The number of elements in
        /// the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is 
        /// greater than the available space from arrayIndex to the end of the destination array, or 
        /// Type T cannot be cast automatically to the type of the destination array.</exception>
        void IBusinessObjectCollection.CopyTo(IBusinessObject[] array, int arrayIndex)
        {
            TBusinessObject[] thisArray = new TBusinessObject[array.LongLength];
            this.CopyTo(thisArray, arrayIndex);
            int count = Count;
            for (int index = 0; index < count; index++)
                array[arrayIndex + index] = thisArray[arrayIndex + index];
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        /// <returns>
        /// True if item was successfully removed from the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// This method also returns false if item is not found in the original
        /// <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        bool IBusinessObjectCollection.Remove(IBusinessObject item)
        {
            return this.Remove((TBusinessObject) item);
        }

        ///<summary>
        ///Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        ///</summary>
        ///
        ///<returns>
        ///true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.
        ///</returns>
        ///
        bool IBusinessObjectCollection.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// The list of business objects that have been created via this collection (@see CreateBusinessObject) and have not
        /// yet been persisted.
        /// </summary>
        public List<TBusinessObject> CreatedBusinessObjects
        {
            get { return _createdBusinessObjects; }
        }


        #endregion

        /// <summary>
        /// Creates a business object of type TBusinessObject
        /// Adds this BO to the CreatedBusinessObjects list. When the object is saved it will
        /// be added to the actual bo collection.
        /// </summary>
        /// <returns></returns>
        public virtual TBusinessObject CreateBusinessObject()
        {
            TBusinessObject newBO = (TBusinessObject) Activator.CreateInstance(typeof (TBusinessObject));
            EventHandler<BOEventArgs> savedEventHandler = null;
            savedEventHandler = delegate(object sender, BOEventArgs e)
                                    {
                                        if (CreatedBusinessObjects.Remove((TBusinessObject) e.BusinessObject))
                                        {
                                            Add((TBusinessObject) e.BusinessObject);
                                            e.BusinessObject.Saved -= savedEventHandler;
                                        }
                                    };
            EventHandler<BOEventArgs> restoredEventHandler = null;
            restoredEventHandler = delegate(object sender, BOEventArgs e)
                                       {
                                           CreatedBusinessObjects.Remove((TBusinessObject) e.BusinessObject);
                                           e.BusinessObject.Updated -= restoredEventHandler;
                                       };
            newBO.Restored += restoredEventHandler;
            newBO.Saved += savedEventHandler;
            CreatedBusinessObjects.Add(newBO);
            return newBO;
        }

        IBusinessObject IBusinessObjectCollection.CreateBusinessObject()
        {
            return CreateBusinessObject();
        }
    }
}
