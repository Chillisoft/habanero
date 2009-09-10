// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.DB
{
    internal class StatementGeneratorUtils
    {
        public static string GetTableName(BusinessObject bo)
        {
            ClassDef classDefToUseForPrimaryKey = GetClassDefToUseForPrimaryKey(bo);
            return (classDefToUseForPrimaryKey.IsUsingSingleTableInheritance())
                       ? classDefToUseForPrimaryKey.SuperClassClassDef.TableName
                       : classDefToUseForPrimaryKey.TableName;
        }

        private static ClassDef GetClassDefToUseForPrimaryKey(BusinessObject bo)
        {
            ClassDef classDefToUseForPrimaryKey = bo.ClassDef;
            while (classDefToUseForPrimaryKey.IsUsingSingleTableInheritance())
            {
                classDefToUseForPrimaryKey = classDefToUseForPrimaryKey.SuperClassClassDef;
            }
            return classDefToUseForPrimaryKey;
        }


        /// <summary>
        /// Creates a "where" clause from the persisted properties held
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sql">The sql statement used to generate and track
        /// parameters</param>
        /// <returns>Returns a string</returns>
        public static string PersistedDatabaseWhereClause(IBOKey key, ISqlStatement sql)
        {
            StringBuilder whereClause = new StringBuilder(key.Count * 30);
            foreach (BOProp prop in ((BOKey)key).SortedValues)
            {
                if (whereClause.Length > 0)
                {
                    whereClause.Append(" AND ");
                }
                if (prop.PersistedPropertyValue == null)
                {
                    whereClause.Append(DatabaseNameFieldNameValuePair(prop, (SqlStatement)sql));
                }
                else
                {
                    whereClause.Append(PersistedDatabaseNameFieldNameValuePair(prop, (SqlStatement)sql));
                }
            }
            return whereClause.ToString();
        }


        /// <summary>
        /// This property returns the 
        /// Returns a string containing the database field name and the 
        /// persisted value, in the format of:<br/>
        /// "[fieldname] = '[value]'" (eg. "children = '2'")<br/>
        /// If a sql statement is provided, then the arguments are added
        /// in parameterised form.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="sql">A sql statement used to generate and track parameters</param>
        /// <returns>Returns a string</returns>
        private static string PersistedDatabaseNameFieldNameValuePair(IBOProp prop, SqlStatement sql)
        {
            if (prop.PersistedPropertyValue == null)
            {
                return SqlFormattingHelper.FormatFieldName(prop.DatabaseFieldName, sql.Connection) + " is NULL ";
            }
            if (sql == null)
            {
                return prop.DatabaseFieldName + " = '" + prop.PersistedPropertyValueString + "'";
            }
            string paramName = sql.ParameterNameGenerator.GetNextParameterName();
            sql.AddParameter(paramName, prop.PersistedPropertyValue);
            return SqlFormattingHelper.FormatFieldName(prop.DatabaseFieldName, sql.Connection) + " = " + paramName;
        }

        /// <summary>
        /// Returns a string containing the database field name and the 
        /// property value, in the format of:<br/>
        /// "[fieldname] = '[value]'" (eg. "children = '2'")<br/>
        /// If a sql statement is provided, then the arguments are added
        /// in parameterised form.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="sql">A sql statement used to generate and track
        /// parameters</param>
        /// <returns>Returns a string</returns>
        private static string DatabaseNameFieldNameValuePair(IBOProp prop, SqlStatement sql)
        {
            if (prop.Value == null)
            {
                if (sql == null)
                {
                    return prop.DatabaseFieldName + " = '" + prop.PropertyValueString + "'";
                }
                return SqlFormattingHelper.FormatFieldName(prop.DatabaseFieldName, sql.Connection) + " is NULL ";
            }
            if (sql == null)
            {
                return prop.DatabaseFieldName + " = '" + prop.PropertyValueString + "'";
            }
            String paramName = sql.ParameterNameGenerator.GetNextParameterName();
            sql.AddParameter(paramName, prop.Value);
            return SqlFormattingHelper.FormatFieldName(prop.DatabaseFieldName, sql.Connection) + " = " + paramName;
        }

    }
}