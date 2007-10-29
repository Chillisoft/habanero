using System.Collections;
using System.Data;
using System.Text;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.DB;

namespace Habanero.BO.SqlGeneration
{
    /// <summary>
    /// Generates "insert" sql statements to insert a specified business
    /// object's properties into the database
    /// </summary>
    public class InsertStatementGenerator
    {
        private BusinessObject _bo;
        private StringBuilder _dbFieldList;
        private StringBuilder _dbValueList;
        private ParameterNameGenerator _gen;
        private SqlStatement _insertSql;
        private SqlStatementCollection _statementCollection;
        private IDbConnection _conn;
        private bool _firstField;
        private ClassDef _currentClassDef;

        /// <summary>
        /// Constructor to initialise the generator
        /// </summary>
        /// <param name="bo">The business object whose properties are to
        /// be inserted</param>
        /// <param name="conn">A database connection</param>
        public InsertStatementGenerator(BusinessObject bo, IDbConnection conn)
        {
            _bo = bo;
            _conn = conn;
        }

        /// <summary>
        /// Generates a collection of sql statements to insert the business
        /// object's properties into the database
        /// </summary>
        /// <returns>Returns a sql statement collection</returns>
        public SqlStatementCollection Generate()
        {
            _statementCollection = new SqlStatementCollection();
            _currentClassDef = _bo.ClassDef;
            bool includeAllProps;
            BOPropCol propsToInclude;
            string tableName;

            includeAllProps = !_currentClassDef.IsUsingClassTableInheritance();
            propsToInclude = _currentClassDef.PropDefcol.CreateBOPropertyCol(true);
            if (_currentClassDef.IsUsingClassTableInheritance())
            {
                propsToInclude.Add(
                    _currentClassDef.SuperClassClassDef.PrimaryKeyDef.CreateBOKey(_bo.Props).GetBOPropCol());
            }
            tableName = _bo.TableName;
            GenerateSingleInsertStatement(includeAllProps, propsToInclude, tableName);

            if (_bo.ClassDef.IsUsingClassTableInheritance())
            {
                _currentClassDef = _bo.ClassDef.SuperClassClassDef;
                while (_currentClassDef.IsUsingClassTableInheritance())
                {
                    includeAllProps = false;
                    propsToInclude = _currentClassDef.PropDefcol.CreateBOPropertyCol(true);
                    propsToInclude.Add(
                        _currentClassDef.SuperClassClassDef.PrimaryKeyDef.CreateBOKey(_bo.Props).GetBOPropCol());
                    tableName = _currentClassDef.TableName;
                    GenerateSingleInsertStatement(includeAllProps, propsToInclude, tableName);
                    _currentClassDef = _currentClassDef.SuperClassClassDef;
                }
                includeAllProps = false;
                propsToInclude = _currentClassDef.PropDefcol.CreateBOPropertyCol(true);
                tableName = _currentClassDef.TableName;
                GenerateSingleInsertStatement(includeAllProps, propsToInclude, tableName);
            }


            return _statementCollection;
        }

        /// <summary>
        /// Generates an "insert" sql statement for the properties in the
        /// business object
        /// </summary>
        /// <param name="includeAllProps">Whether to include all the object's
        /// properties</param>
        /// <param name="propsToInclude">A collection of properties to insert,
        /// if the previous include-all boolean was not set to true</param>
        /// <param name="tableName">The table name</param>
        private void GenerateSingleInsertStatement(bool includeAllProps, BOPropCol propsToInclude, string tableName)
        {
            ISupportsAutoIncrementingField supportsAutoIncrementingField = null;
            if (_bo.HasAutoIncrementingField)
            {
                supportsAutoIncrementingField = new SupportsAutoIncrementingFieldBO(_bo);
            }
            this.InitialiseStatement(tableName, supportsAutoIncrementingField);
           

            foreach (BOProp prop in _bo.Props.SortedValues)
            {
                // BOProp prop = (BOProp) item.Value;
                if (includeAllProps || propsToInclude.Contains(prop.PropertyName))
                {
                    if (propsToInclude.AutoIncrementingPropertyName != prop.PropertyName)
                        AddPropToInsertStatement(prop);
                }
            }
            _insertSql.Statement.Append(@"INSERT INTO " + tableName + " (" + _dbFieldList.ToString() + ") VALUES (" +
                                       _dbValueList.ToString() + ")");
            _statementCollection.Insert(0, _insertSql);
        }

        /// <summary>
        /// Initialises the sql statement
        /// </summary>
        private void InitialiseStatement(string tableName, ISupportsAutoIncrementingField supportsAutoIncrementingField)
        {
            _dbFieldList = new StringBuilder(_bo.Props.Count * 20);
            _dbValueList = new StringBuilder(_bo.Props.Count * 20);
            InsertSqlStatement statement = new InsertSqlStatement(_conn);
            statement.TableName = tableName;
            statement.SupportsAutoIncrementingField = supportsAutoIncrementingField;

            _insertSql = statement;
            
            _gen = new ParameterNameGenerator(_conn);
            _firstField = true;
        }

        /// <summary>
        /// Adds the specified property value as a parameter
        /// </summary>
        /// <param name="prop">The business object property</param>
        private void AddPropToInsertStatement(BOProp prop)
        {
            string paramName;
            if (!_firstField)
            {
                _dbFieldList.Append(", ");
                _dbValueList.Append(", ");
            }
            _dbFieldList.Append(prop.DatabaseFieldName);
            paramName = _gen.GetNextParameterName();
            _dbValueList.Append(paramName);
            _insertSql.AddParameter(paramName, prop.Value);
            //_insertSql.AddParameter(paramName, DatabaseUtil.PrepareValue(prop.PropertyValue));
            _firstField = false;
        }
    }
}
