using System.Collections;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;

namespace Habanero.Bo
{
    /// <summary>
    /// Generates "select" sql statements to read a specified business
    /// object's properties from the database
    /// </summary>
    public class SelectStatementGenerator
    {
        private readonly IDatabaseConnection _connection;
        private BusinessObject _bo;
        private ClassDef _classDef;

        /// <summary>
        /// Constructor to initialise the generator
        /// </summary>
        /// <param name="bo">The business object whose properties are to
        /// be read</param>
        /// <param name="classDef">The class definition</param>
        /// <param name="connection">A database connection</param>
        public SelectStatementGenerator(BusinessObject bo, ClassDef classDef, IDatabaseConnection connection)
        {
            _bo = bo;
            _classDef = classDef;
            _connection = connection;
        }

        /// <summary>
        /// Constructor to initialise the generator
        /// </summary>
        /// <param name="bo">The business object whose properties are to
        /// be read</param>
        /// <param name="connection">A database connection</param>
        public SelectStatementGenerator(BusinessObject bo, IDatabaseConnection connection)
            : this(bo, bo.ClassDef, connection)
        {
        }

        /// <summary>
        /// Generates a sql statement to read the business
        /// object's properties from the database
        /// </summary>
        /// <param name="limit">The limit</param>
        /// <returns>Returns a string</returns>
        public string Generate(int limit)
        {
            IList classDefs = new ArrayList();
            ClassDef currentClassDef = _classDef;
            while (currentClassDef != null)
            {
                classDefs.Add(currentClassDef);
                currentClassDef = currentClassDef.SuperClassClassDef;
            }

            string statement = "SELECT ";
            if (limit > 0)
            {
                statement += " " + _connection.GetLimitClauseForBeginning(limit) + " ";
            }

            foreach (BOProp prop in _bo.GetBOPropCol().SortedValues)
            {
                string tableName = GetTableName(prop, classDefs);
                statement += tableName + ".";
                statement += _connection.LeftFieldDelimiter;
                statement += prop.DatabaseFieldName;
                statement += _connection.RightFieldDelimiter;
                statement += ", ";
            }

            statement = statement.Remove(statement.Length - 2, 2);
            currentClassDef = _classDef;
            while (currentClassDef.IsUsingSingleTableInheritance())
            {
                currentClassDef = currentClassDef.SuperClassClassDef;
            }
            statement += " FROM " + currentClassDef.TableName;
            string where = " WHERE ";

            while (currentClassDef.IsUsingClassTableInheritance())
            {
                statement += ", " + currentClassDef.SuperClassClassDef.TableName;
                foreach (DictionaryEntry entry in currentClassDef.SuperClassClassDef.PrimaryKeyDef)
                {
                    PropDef def = (PropDef) entry.Value;
                    where += currentClassDef.SuperClassClassDef.TableName + "." + def.FieldName;
                    where += " = " + currentClassDef.TableName + "." + def.FieldName;
                    where += " AND ";
                }
                currentClassDef = currentClassDef.SuperClassClassDef;
            }
            if (where.Length > 7)
            {
                statement += where.Substring(0, where.Length - 5);
            }

            if (limit > 0)
            {
                statement += " " + _connection.GetLimitClauseForEnd(limit) + " ";
            }
            return statement;
        }

        /// <summary>
        /// Returns the table name
        /// </summary>
        /// <param name="prop">The property</param>
        /// <param name="classDefs">The class definitions</param>
        /// <returns>Returns a string</returns>
        private string GetTableName(BOProp prop, IList classDefs)
        {
            int i = 0;
            bool isSingleTableInheritance = false;
            do
            {
                ClassDef classDef = (ClassDef) classDefs[i];
                if (classDef.IsUsingConcreteTableInheritance())
                {
                    return classDef.TableName;
                }
                else if (classDef.PropDefcol.Contains(prop.PropertyName))
                {
                    if (classDef.SuperClassClassDef == null || classDef.IsUsingClassTableInheritance())
                    {
                        return classDef.TableName;
                    }
                    else if (classDef.IsUsingSingleTableInheritance())
                    {
                        isSingleTableInheritance = true;
                    }
                }
                else if (classDef.IsUsingSingleTableInheritance())
                {
                    isSingleTableInheritance = true;
                }
                else if (isSingleTableInheritance)
                {
                    return classDef.TableName;
                }
                i++;
            } while (i < classDefs.Count);
            return "";
        }
    }
}
