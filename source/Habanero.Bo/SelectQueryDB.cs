using System;
using System.Text;
using Habanero.Base;
using Habanero.DB;
using Habanero.Util;

namespace Habanero.BO
{
    public class SelectQueryDB<T> : SelectQuery<T> where T : class, IBusinessObject
    {

        public SelectQueryDB(Criteria criteria) : base(criteria)
        {
          
        }

        public SelectQueryDB() 
        {
            
        }

        public ISqlStatement CreateSqlStatement()
        {
            SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection);
            StringBuilder builder = statement.Statement.Append("SELECT ");
            foreach (QueryField field in Fields.Values)
            {
                builder.Append(field.FieldName + ", ");
            }
            builder.Remove(builder.Length - 2, 2);
            builder.Append(" FROM ");
            builder.Append(this.Source);
            if (this.Criteria != null)
            {
                builder.Append(" WHERE ");
                builder.Append(this.Criteria.ToString(delegate(string propName) { return Fields[propName].FieldName; }));
            }
            if (this.OrderCriteria != null)
            {
                builder.Append(" ORDER BY ");
                StringBuilder orderByClause = new StringBuilder();
                foreach (string orderField in this.OrderCriteria.Fields)
                {
                    StringUtilities.AppendMessage(orderByClause, Fields[orderField].FieldName, ", ");
                }
                builder.Append(orderByClause.ToString());
            }
            return statement;
        }
    }
}