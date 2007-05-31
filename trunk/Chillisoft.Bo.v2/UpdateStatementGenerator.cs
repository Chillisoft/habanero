using System.Collections;
using System.Data;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Db.v2;

namespace Chillisoft.Bo.SqlGeneration.v2
{
    /// <summary>
    /// Generates "update" sql statements to update a specified business
    /// object's properties in the database
    /// </summary>
    public class UpdateStatementGenerator
    {
        private BusinessObjectBase mBO;
        private IDbConnection mConn;
        private SqlStatementCollection statementCollection;
        private SqlStatement updateSQL;

        /// <summary>
        /// Constructor to initialise the generator
        /// </summary>
        /// <param name="bo">The business object whose properties are to
        /// be updated</param>
        /// <param name="conn">A database connection</param>
        public UpdateStatementGenerator(BusinessObjectBase bo, IDbConnection conn)
        {
            mBO = bo;
            mConn = conn;
        }

        /// <summary>
        /// Generates a collection of sql statements to update the business
        /// object's properties in the database
        /// </summary>
        /// <returns>Returns a sql statement collection</returns>
        public SqlStatementCollection Generate()
        {
            statementCollection = new SqlStatementCollection();
            bool includeAllProps;
            BOPropCol propsToInclude;
            string tableName;
            ClassDef currentClassDef = mBO.ClassDef;
            while (currentClassDef.IsUsingClassTableInheritance())
            {
                includeAllProps = false;
                propsToInclude = currentClassDef.SuperClassDef.PropDefcol.CreateBOPropertyCol(true);
                if (propsToInclude.Count > 0)
                {
                    tableName = currentClassDef.SuperClassDef.TableName;
                    GenerateSingleUpdateStatement(tableName, includeAllProps, propsToInclude, true, currentClassDef);
                }
                currentClassDef = currentClassDef.SuperClassDef;
            }
            includeAllProps = !mBO.ClassDef.IsUsingClassTableInheritance();
            propsToInclude = mBO.ClassDef.PropDefcol.CreateBOPropertyCol(true);
            tableName = mBO.TableName;
            GenerateSingleUpdateStatement(tableName, includeAllProps, propsToInclude, false, mBO.ClassDef);
            return statementCollection;
        }

        /// <summary>
        /// Generates an "update" sql statement for the properties in the
        /// business object
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="includeAllProps">Whether to include all the object's
        /// properties</param>
        /// <param name="propsToInclude">A collection of properties to update,
        /// if the previous include-all boolean was not set to true</param>
        /// <param name="isSuperClassStatement">Whether a super-class is involved</param>
        /// <param name="currentClassDef">The current class definition</param>
        private void GenerateSingleUpdateStatement(string tableName, bool includeAllProps, BOPropCol propsToInclude,
                                                   bool isSuperClassStatement, ClassDef currentClassDef)
        {
            updateSQL = new SqlStatement(mConn);
            updateSQL.Statement.Append(@"UPDATE " + tableName + " SET ");
            int includedProps = 0;
            foreach (DictionaryEntry item in mBO.GetBOPropCol())
            {
                BOProp prop = (BOProp) item.Value;
                if (includeAllProps || propsToInclude.Contains(prop.PropertyName))
                {
                    if (prop.IsDirty &&
                        ((mBO.ClassDef.PrimaryKeyDef.IsObjectID &&
                          !mBO.ClassDef.PrimaryKeyDef.Contains(prop.PropertyName)) ||
                         !mBO.ClassDef.PrimaryKeyDef.IsObjectID))
                    {
                        includedProps++;
                        updateSQL.Statement.Append(prop.DataBaseFieldName);
                        updateSQL.Statement.Append(" = ");
                        updateSQL.AddParameterToStatement(prop.PropertyValue);
                        //updateSQL.AddParameterToStatement(DatabaseUtil.PrepareValue(prop.PropertyValue));
                        updateSQL.Statement.Append(", ");
                    }
                }
            }
            updateSQL.Statement.Remove(updateSQL.Statement.Length - 2, 2); //remove the last ", "
            if (isSuperClassStatement)
            {
                updateSQL.Statement.Append(" WHERE " + mBO.WhereClauseForSuperClass(updateSQL, currentClassDef));
            }
            else
            {
                updateSQL.Statement.Append(" WHERE " + mBO.WhereClause(updateSQL));
            }
            if (includedProps > 0)
            {
                statementCollection.Add(updateSQL);
            }
        }
    }
}