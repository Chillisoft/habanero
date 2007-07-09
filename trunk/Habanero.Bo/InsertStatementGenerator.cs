using System.Collections;
using System.Data;
using System.Text;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.DB;

namespace Habanero.Bo.SqlGeneration
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
            bool includeAllProps;
            BOPropCol propsToInclude;
            string tableName;

            includeAllProps = !_bo.ClassDef.IsUsingClassTableInheritance();
            propsToInclude = _bo.ClassDef.PropDefcol.CreateBOPropertyCol(true);
            if (_bo.ClassDef.IsUsingClassTableInheritance())
            {
                propsToInclude.Add(
                    _bo.ClassDef.SuperClassClassDef.PrimaryKeyDef.CreateBOKey(_bo.Props).GetBOPropCol());
            }
            tableName = _bo.TableName;
            GenerateSingleInsertStatement(includeAllProps, propsToInclude, tableName);

            if (_bo.ClassDef.IsUsingClassTableInheritance())
            {
                ClassDef currentClassDef = _bo.ClassDef.SuperClassClassDef;
                while (currentClassDef.IsUsingClassTableInheritance())
                {
                    includeAllProps = false;
                    propsToInclude = currentClassDef.PropDefcol.CreateBOPropertyCol(true);
                    propsToInclude.Add(
                        currentClassDef.SuperClassClassDef.PrimaryKeyDef.CreateBOKey(_bo.Props).GetBOPropCol());
                    tableName = currentClassDef.TableName;
                    GenerateSingleInsertStatement(includeAllProps, propsToInclude, tableName);
                    currentClassDef = currentClassDef.SuperClassClassDef;
                }
                includeAllProps = false;
                propsToInclude = currentClassDef.PropDefcol.CreateBOPropertyCol(true);
                tableName = currentClassDef.TableName;
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
            this.InitialiseStatement();

            foreach (BOProp prop in _bo.Props.SortedValues)
            {
               // BOProp prop = (BOProp) item.Value;
                if (includeAllProps || propsToInclude.Contains(prop.PropertyName))
                {
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
        private void InitialiseStatement()
        {
            _dbFieldList = new StringBuilder(_bo.Props.Count * 20);
            _dbValueList = new StringBuilder(_bo.Props.Count * 20);
            _insertSql = new SqlStatement(_conn);
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
