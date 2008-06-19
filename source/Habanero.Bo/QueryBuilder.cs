using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    public class QueryBuilder
    {

        public static SelectQuery CreateSelectQuery(ClassDef classDef)
        {
            return CreateSelectQuery(classDef, null);
        }

        public static SelectQuery CreateSelectQuery(ClassDef classDef, Criteria criteria)
        {
            SelectQuery selectQuery = new SelectQuery();
            foreach (IPropDef propDef in classDef.PropDefcol)
            {
                selectQuery.Fields.Add(propDef.PropertyName, new QueryField(propDef.PropertyName, classDef.GetTableName(propDef) + "." + propDef.DatabaseFieldName));
            }
            selectQuery.Source = classDef.TableName;
            selectQuery.Criteria = criteria;
            return selectQuery;
        }
    }
}