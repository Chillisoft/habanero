//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Base;

namespace Habanero.BO.SqlGeneration
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
                string limitClause = _connection.GetLimitClauseForBeginning(limit);
                if (!string.IsNullOrEmpty(limitClause)) statement += limitClause + " ";
            }

            foreach (BOProp prop in _bo.Props.SortedValues)
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
                where += GetParentKeyMatchWhereClause(currentClassDef);
                currentClassDef = currentClassDef.SuperClassClassDef;
            }

            //TODO Eric - because of the class structure, this doesn't use parameterised SQL
            if (_classDef.IsUsingSingleTableInheritance())
            {
                if (_bo.ClassDef.SuperClassDef.Discriminator == null)
                {
                    throw new InvalidXmlDefinitionException("A super class has been defined " +
                        "using Single Table Inheritance, but no discriminator column has been set.");
                }
                where += string.Format("{0} = '{1}'", _classDef.SuperClassDef.Discriminator, _classDef.ClassName);
                where += " AND ";
            }
            //while (true)
            //{
            //    ClassDef classDefWithSTI = null;
            //    foreach (ClassDef def in currentClassDef.ImmediateChildren)
            //    {
            //        if (def.IsUsingSingleTableInheritance())
            //        {
            //            classDefWithSTI = def;
            //            break;
            //        }
            //    }

            //    if (currentClassDef.IsUsingSingleTableInheritance() || classDefWithSTI != null)
            //    {
            //        string discriminator;
            //        if (currentClassDef.SuperClassDef != null)
            //        {
            //            discriminator = currentClassDef.SuperClassDef.Discriminator;
            //        }
            //        else
            //        {
            //            discriminator = classDefWithSTI.SuperClassDef.Discriminator;
            //        }
            //        if (discriminator == null)
            //        {
            //            throw new InvalidXmlDefinitionException("A super class has been defined " +
            //                "using Single Table Inheritance, but no discriminator column has been set.");
            //        }
            //        where += string.Format("{0} = '{1}'", _classDef.SuperClassDef.Discriminator, _classDef.ClassName);
            //        where += " AND ";
            //    }
            //    else break;
            //}

            if (where.Length > 7)
            {
                statement += where.Substring(0, where.Length - 5);
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

        /// <summary>
        /// Creates the where clause which determines how the parent
        /// table should match up with child table, depending on the ID attribute
        /// given by the user.  For ClassTableInheritance only.
        /// </summary>
        private static string GetParentKeyMatchWhereClause(ClassDef currentClassDef)
		{
            string parentIDCopyFieldName = currentClassDef.SuperClassDef.ID;
            string where = "";
            foreach (PropDef def in currentClassDef.SuperClassClassDef.PrimaryKeyDef)
            {
                //TODO: Mark - Shouldn't this also have the field Delimiters?
                where += currentClassDef.SuperClassClassDef.TableName + "." + def.FieldName;

                PrimaryKeyDef parentID = currentClassDef.SuperClassClassDef.PrimaryKeyDef;
                if (parentIDCopyFieldName == null ||
                    parentIDCopyFieldName == "")
                {
                    where += " = " + currentClassDef.TableName + "." + def.FieldName;
                }
                else
                {
                    if (parentID.Count > 1)
                    {
                        throw new InvalidXmlDefinitionException("For a super class definition " +
                            "using class table inheritance, the ID attribute can only refer to a " +
                            "parent with a single primary key.  Leaving out the attribute will " +
                            "allow composite primary keys where the child's copies have the same " +
                            "field name as the parent.");
                    }
                    where += " = " + currentClassDef.TableName + "." + parentIDCopyFieldName;
                }
                where += " AND ";
            }
            return where;
		}
    }
}
