using System.Collections;
using System.Data;
using System.Text;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Db.v2;

namespace Chillisoft.Bo.SqlGeneration.v2
{
    /// <summary>
    /// Generates "insert" sql statements to insert a specified business
    /// object's properties into the database
    /// </summary>
    public class InsertStatementGenerator
    {
        private BusinessObjectBase mBO;
        private StringBuilder dbFieldList;
        private StringBuilder dbValueList;
        private ParameterNameGenerator gen;
        private SqlStatement insertSQL;
        private SqlStatementCollection statementCollection;
        private IDbConnection mConn;
        private bool firstField;

        /// <summary>
        /// Constructor to initialise the generator
        /// </summary>
        /// <param name="bo">The business object whose properties are to
        /// be inserted</param>
        /// <param name="conn">A database connection</param>
        public InsertStatementGenerator(BusinessObjectBase bo, IDbConnection conn)
        {
            mBO = bo;
            mConn = conn;
        }

        /// <summary>
        /// Generates a collection of sql statements to insert the business
        /// object's properties into the database
        /// </summary>
        /// <returns>Returns a sql statement collection</returns>
        public SqlStatementCollection Generate()
        {
            statementCollection = new SqlStatementCollection();
            bool includeAllProps;
            BOPropCol propsToInclude;
            string tableName;

            includeAllProps = !mBO.ClassDef.IsUsingClassTableInheritance();
            propsToInclude = mBO.ClassDef.PropDefcol.CreateBOPropertyCol(true);
            if (mBO.ClassDef.IsUsingClassTableInheritance())
            {
                propsToInclude.Add(
                    mBO.ClassDef.SuperClassDef.PrimaryKeyDef.CreateBOKey(mBO.GetBOPropCol()).GetBOPropCol());
            }
            tableName = mBO.TableName;
            GenerateSingleInsertStatement(includeAllProps, propsToInclude, tableName);

            if (mBO.ClassDef.IsUsingClassTableInheritance())
            {
                ClassDef currentClassDef = mBO.ClassDef.SuperClassDef;
                while (currentClassDef.IsUsingClassTableInheritance())
                {
                    includeAllProps = false;
                    propsToInclude = currentClassDef.PropDefcol.CreateBOPropertyCol(true);
                    propsToInclude.Add(
                        currentClassDef.SuperClassDef.PrimaryKeyDef.CreateBOKey(mBO.GetBOPropCol()).GetBOPropCol());
                    tableName = currentClassDef.TableName;
                    GenerateSingleInsertStatement(includeAllProps, propsToInclude, tableName);
                    currentClassDef = currentClassDef.SuperClassDef;
                }
                includeAllProps = false;
                propsToInclude = currentClassDef.PropDefcol.CreateBOPropertyCol(true);
                tableName = currentClassDef.TableName;
                GenerateSingleInsertStatement(includeAllProps, propsToInclude, tableName);
            }


            return statementCollection;
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
            this.initialiseStatement();

            foreach (BOProp prop in mBO.GetBOPropCol().SortedValues)
            {
               // BOProp prop = (BOProp) item.Value;
                if (includeAllProps || propsToInclude.Contains(prop.PropertyName))
                {
                    AddPropToInsertStatement(prop);
                }
            }
            insertSQL.Statement.Append(@"INSERT INTO " + tableName + " (" + dbFieldList.ToString() + ") VALUES (" +
                                       dbValueList.ToString() + ")");
            statementCollection.Insert(0, insertSQL);
        }

        /// <summary>
        /// Initialises the sql statement
        /// </summary>
        /// TODO ERIC - capitalise
        private void initialiseStatement()
        {
            dbFieldList = new StringBuilder(mBO.GetBOPropCol().Count*20);
            dbValueList = new StringBuilder(mBO.GetBOPropCol().Count*20);
            insertSQL = new SqlStatement(mConn);
            gen = new ParameterNameGenerator(mConn);
            firstField = true;
        }

        /// <summary>
        /// Adds the specified property value as a parameter
        /// </summary>
        /// <param name="prop">The business object property</param>
        private void AddPropToInsertStatement(BOProp prop)
        {
            string paramName;
            if (!firstField)
            {
                dbFieldList.Append(", ");
                dbValueList.Append(", ");
            }
            dbFieldList.Append(prop.DataBaseFieldName);
            paramName = gen.GetNextParameterName();
            dbValueList.Append(paramName);
            insertSQL.AddParameter(paramName, prop.PropertyValue);
            //insertSQL.AddParameter(paramName, DatabaseUtil.PrepareValue(prop.PropertyValue));
            firstField = false;
        }
    }
}
