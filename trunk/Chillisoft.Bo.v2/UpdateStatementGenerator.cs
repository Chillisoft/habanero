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
        private BusinessObjectBase _bo;
        private IDbConnection _conn;
        private SqlStatementCollection _statementCollection;
        private SqlStatement _updateSQL;

        /// <summary>
        /// Constructor to initialise the generator
        /// </summary>
        /// <param name="bo">The business object whose properties are to
        /// be updated</param>
        /// <param name="conn">A database connection</param>
        public UpdateStatementGenerator(BusinessObjectBase bo, IDbConnection conn)
        {
            _bo = bo;
            _conn = conn;
        }

        /// <summary>
        /// Generates a collection of sql statements to update the business
        /// object's properties in the database
        /// </summary>
        /// <returns>Returns a sql statement collection</returns>
        public SqlStatementCollection Generate()
        {
            _statementCollection = new SqlStatementCollection();
            bool includeAllProps;
            BOPropCol propsToInclude;
            string tableName;
            ClassDef currentClassDef = _bo.ClassDef;
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
            includeAllProps = !_bo.ClassDef.IsUsingClassTableInheritance();
            propsToInclude = _bo.ClassDef.PropDefcol.CreateBOPropertyCol(true);
            tableName = _bo.TableName;
            GenerateSingleUpdateStatement(tableName, includeAllProps, propsToInclude, false, _bo.ClassDef);
            return _statementCollection;
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
            _updateSQL = new SqlStatement(_conn);
            _updateSQL.Statement.Append(@"UPDATE " + tableName + " SET ");
            int includedProps = 0;
            foreach (BOProp prop in _bo.GetBOPropCol().SortedValues)
            {
                if (includeAllProps || propsToInclude.Contains(prop.PropertyName))
                {
                    if (prop.IsDirty &&
                        ((_bo.ClassDef.PrimaryKeyDef.IsObjectID &&
                          !_bo.ClassDef.PrimaryKeyDef.Contains(prop.PropertyName)) ||
                         !_bo.ClassDef.PrimaryKeyDef.IsObjectID))
                    {
                        includedProps++;
                        _updateSQL.Statement.Append(prop.DatabaseFieldName);
                        _updateSQL.Statement.Append(" = ");
                        _updateSQL.AddParameterToStatement(prop.PropertyValue);
                        //_updateSQL.AddParameterToStatement(DatabaseUtil.PrepareValue(prop.PropertyValue));
                        _updateSQL.Statement.Append(", ");
                    }
                }
            }
            _updateSQL.Statement.Remove(_updateSQL.Statement.Length - 2, 2); //remove the last ", "
            if (isSuperClassStatement)
            {
                _updateSQL.Statement.Append(" WHERE " + _bo.WhereClauseForSuperClass(_updateSQL, currentClassDef));
            }
            else
            {
                _updateSQL.Statement.Append(" WHERE " + _bo.WhereClause(_updateSQL));
            }
            if (includedProps > 0)
            {
                _statementCollection.Add(_updateSQL);
            }
        }
    }
}
