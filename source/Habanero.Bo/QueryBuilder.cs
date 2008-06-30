using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.DB;

namespace Habanero.BO
{

    /// <summary>
    /// Provides utility methods that create SelectQuery objects given a set of information.
    /// </summary>
    public class QueryBuilder
    {

        /// <summary>
        /// Creates a SelectQuery using the given classdef without any Criteria. All information in the ClassDef will be taken into account
        /// (such as inheritance structures).
        /// </summary>
        /// <param name="classDef">The <see cref="ClassDef" /> to create the SelectQuery for.</param>
        /// <returns>A SelectQuery that can be used to load objects of the type the given ClassDef represents</returns>
        public static SelectQuery CreateSelectQuery(IClassDef classDef)
        {
            return CreateSelectQuery(classDef, null);
        }

        /// <summary>
        /// Creates a SelectQuery using the given classdef with the given Criteria. All information in the ClassDef will be taken into account
        /// (such as inheritance structures).
        /// </summary>
        /// <param name="classDef">The <see cref="ClassDef" /> to create the SelectQuery for.</param>
        /// <param name="criteria">The criteria to be set on the SelectQuery</param>
        /// <returns>A SelectQuery that can be used to load objects of the type the given ClassDef represents</returns>
        public static SelectQuery CreateSelectQuery(IClassDef classDef, Criteria criteria)
        {
            SelectQuery selectQuery = new SelectQuery();
            foreach (IPropDef propDef in classDef.PropDefcol)
            {
                selectQuery.Fields.Add(propDef.PropertyName, new QueryField(propDef.PropertyName, propDef.DatabaseFieldName, classDef.GetTableName(propDef)));
            }
            selectQuery.Source = classDef.TableName;
            selectQuery.Criteria = criteria;
            selectQuery.ClassDef = classDef;
            return selectQuery;
        }

        /// <summary>
        /// Goes through the OrderCritieria of a SelectQuery and adds to the query fields
        /// any order fields that are not already included in the query fields.
        /// </summary>
        /// <param name="query">The query to modify - any order fields not in the query fields will be added to them</param>
        public static void IncludeFieldsFromOrderCriteria(ISelectQuery query)
        {
            foreach (OrderCriteria.Field orderField in query.OrderCriteria.Fields)
            {
                if (query.Fields.ContainsKey(orderField.FullName)) continue;

                RelationshipDef relationshipDef = ((ClassDef)query.ClassDef).GetRelationship(orderField.Source);
                IPropDef relatedPropDef = relationshipDef.RelatedObjectClassDef.GetPropDef(orderField.Name);
                QueryField queryField = new QueryField(orderField.Name, relatedPropDef.DatabaseFieldName, orderField.Source);
                query.Fields.Add(orderField.FullName, queryField);
            }

        }
    }
}