#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.DB
{
    /// <summary>
    /// Generates "insert" sql statements to insert a specified business
    /// object's properties into the database
    /// </summary>
    public class InsertStatementGenerator : ModifyStatementGenerator
    {
        private readonly BusinessObject _bo;
        private StringBuilder _dbFieldList;
        private StringBuilder _dbValueList;
        private IParameterNameGenerator _gen;
        private SqlStatement _insertSql;
        private List<ISqlStatement> _statements;
        private readonly IDatabaseConnection _connection;
        private bool _firstField;
        private ClassDef _currentClassDef;

        /// <summary>
        /// Constructor to initialise the generator
        /// </summary>
        /// <param name="bo">The business object whose properties are to
        /// be inserted</param>
        /// <param name="connection">A database connection</param>
        public InsertStatementGenerator(IBusinessObject bo, IDatabaseConnection connection)
        {
            _bo = (BusinessObject) bo;
            _connection = connection;
        }

        /// <summary>
        /// Generates a collection of sql statements to insert the business
        /// object's properties into the database
        /// </summary>
        /// <returns>Returns a sql statement collection</returns>
        public IEnumerable<ISqlStatement> Generate()
        {
            _statements = new List<ISqlStatement>();
            _currentClassDef = _bo.ClassDef;
            IBOPropCol propsToInclude;
            string tableName;

            propsToInclude = GetPropsToInclude(_currentClassDef);
            tableName = StatementGeneratorUtils.GetTableName(_bo);
            GenerateSingleInsertStatement(propsToInclude, tableName);

            if (_bo.ClassDef.IsUsingClassTableInheritance())
            {
                _currentClassDef = (ClassDef) _bo.ClassDef.SuperClassClassDef;
                while (_currentClassDef.IsUsingClassTableInheritance())
                {
                    propsToInclude = GetPropsToInclude(_currentClassDef);
                    tableName = _currentClassDef.TableName;
                    GenerateSingleInsertStatement(propsToInclude, tableName);
                    _currentClassDef = (ClassDef) _currentClassDef.SuperClassClassDef;
                }
                propsToInclude = GetPropsToInclude(_currentClassDef);
                tableName = _currentClassDef.InheritedTableName;
                GenerateSingleInsertStatement(propsToInclude, tableName);
            }

            return _statements;
        }

        /// <summary>
        /// Generates an "insert" sql statement for the properties in the
        /// business object
        /// </summary>
        /// <param name="propsToInclude">A collection of properties to insert,
        /// if the previous include-all boolean was not set to true</param>
        /// <param name="tableName">The table name</param>
        private void GenerateSingleInsertStatement(IBOPropCol propsToInclude, string tableName)
        {
            ISupportsAutoIncrementingField supportsAutoIncrementingField = null;
            if (_bo.Props.HasAutoIncrementingField)
            {
                supportsAutoIncrementingField = new SupportsAutoIncrementingFieldBO(_bo);
            }
            this.InitialiseStatement(tableName, supportsAutoIncrementingField);

            ModifyForInheritance(propsToInclude);

            foreach (BOProp prop in _bo.Props.SortedValues)
            {
                if (propsToInclude.Contains(prop.PropertyName))
                {
                    if (!prop.PropDef.AutoIncrementing) 
                        AddPropToInsertStatement(prop);
                }
            }

            _insertSql.Statement.Append(String.Format(
                                            "INSERT INTO {0} ({1}) VALUES ({2})",
                                            _connection.SqlFormatter.DelimitTable(tableName),
                                            _dbFieldList, _dbValueList));
            _statements.Insert(0, _insertSql);
        }

        private void ModifyForInheritance(IBOPropCol propsToInclude)
        {
            //Recursively
            //Look at the superclass and if it is single table inheritance then 
            // add the discriminator property to the BOPropCol if it doesnt exist 
            // and set the value to the this super class' name

            ClassDef classDef = _currentClassDef; //rather than _bo.ClassDef\

            IBOPropCol discriminatorProps = new BOPropCol();
            AddDiscriminatorProperties(classDef, propsToInclude, discriminatorProps);
            foreach (BOProp boProp in discriminatorProps)
            {
                AddPropToInsertStatement(boProp);
            }
        }

        private void AddDiscriminatorProperties(ClassDef classDef, IBOPropCol propsToInclude, IBOPropCol discriminatorProps)
        {
            ClassDef classDefWithSTI = null;
            if (classDef.IsUsingSingleTableInheritance() || classDefWithSTI != null)
            {
                string discriminator = null;
                if (classDef.SuperClassDef != null)
                {
                    discriminator = classDef.SuperClassDef.Discriminator;
                }
                else if (classDefWithSTI != null)
                {
                    discriminator = classDefWithSTI.SuperClassDef.Discriminator;
                }
                if (discriminator == null)
                {
                    throw new InvalidXmlDefinitionException("A super class has been defined " +
                                                            "using Single Table Inheritance, but no discriminator column has been set.");
                }
                if (propsToInclude.Contains(discriminator) && _bo.Props.Contains(discriminator))
                {
                    var boProp = _bo.Props[discriminator];
                    boProp.Value = _bo.ClassDef.ClassName;
                }
                else if (!discriminatorProps.Contains(discriminator))
                {
                    var propDef = new PropDef(discriminator, typeof (string), PropReadWriteRule.ReadWrite, null);
                    var discriminatorProp = new BOProp(propDef, _bo.ClassDef.ClassName);
                    discriminatorProps.Add(discriminatorProp);
                }
            }

            if (classDef.IsUsingSingleTableInheritance())
            {
                IClassDef superClassClassDef = classDef.SuperClassClassDef;
                AddDiscriminatorProperties((ClassDef) superClassClassDef, propsToInclude, discriminatorProps);
            }
        }

        /// <summary>
        /// Initialises the sql statement with a autoincrementing object
        /// </summary>
        private void InitialiseStatement(string tableName, ISupportsAutoIncrementingField supportsAutoIncrementingField)
        {
            _dbFieldList = new StringBuilder(_bo.Props.Count * 20);
            _dbValueList = new StringBuilder(_bo.Props.Count * 20);
            var statement = new InsertSqlStatement(_connection);
            statement.TableName = tableName;
            statement.SupportsAutoIncrementingField = supportsAutoIncrementingField;

            _insertSql = statement;

            _gen = _connection.CreateParameterNameGenerator();
            _firstField = true;
        }

        /// <summary>
        /// Adds the specified property value as a parameter
        /// </summary>
        /// <param name="prop">The business object property</param>
        private void AddPropToInsertStatement(BOProp prop)
        {
            if (!_firstField)
            {
                _dbFieldList.Append(", ");
                _dbValueList.Append(", ");
            }
            _dbFieldList.Append(_connection.SqlFormatter.DelimitField(prop.DatabaseFieldName));
            string paramName = _gen.GetNextParameterName();
            _dbValueList.Append(paramName);
            _insertSql.AddParameter(paramName, prop.Value, prop.PropertyType);
            _firstField = false;
        }

        /// <summary>
        /// Determines which parent ID field to add to the insertion list, depending on which
        /// ID attribute was specified in the class definition.  There are four possibilities:
        ///    1) The child contains a foreign key to the parent, with the parent ID's name
        ///    2) No attribute was given, assumes the above.
        ///    3) The child's ID has a copy of the parent's ID value
        ///    4) The child has no ID and just inherits the parent's ID (still has the parent's
        ///        ID as a field in its own table)
        /// </summary>
        private void AddParentID(IBOPropCol propsToInclude)
        {
            IClassDef currentClassDef = _currentClassDef;
            while (currentClassDef.SuperClassClassDef != null &&
                   currentClassDef.SuperClassClassDef.PrimaryKeyDef == null)
            {
                currentClassDef = currentClassDef.SuperClassClassDef;
            }
            if (currentClassDef.SuperClassClassDef ==  null ||currentClassDef.SuperClassClassDef.PrimaryKeyDef == null) return;

            var superClassDef = (SuperClassDef) currentClassDef.SuperClassDef;
            var parentIDCopyFieldName = superClassDef.ID;
            var superClassPrimaryKeyDef = (PrimaryKeyDef) currentClassDef.SuperClassClassDef.PrimaryKeyDef;
            if (string.IsNullOrEmpty(parentIDCopyFieldName) ||
                superClassPrimaryKeyDef.KeyName == parentIDCopyFieldName)
            {
                propsToInclude.Add(
                    superClassPrimaryKeyDef.CreateBOKey(_bo.Props).GetBOPropCol());
            }
            else if (parentIDCopyFieldName != currentClassDef.PrimaryKeyDef.KeyName)
            {
                if (superClassPrimaryKeyDef.Count > 1)
                {
                    throw new InvalidXmlDefinitionException("For a super class definition " +
                                                            "using class table inheritance, the ID attribute can only refer to a " +
                                                            "parent with a single primary key.  Leaving out the attribute will " +
                                                            "allow composite primary keys where the child's copies have the same " +
                                                            "field name as the parent.");
                }
                var parentProp = superClassPrimaryKeyDef.CreateBOKey(_bo.Props).GetBOPropCol()[superClassPrimaryKeyDef.KeyName];
                var profDef = new PropDef(parentIDCopyFieldName, parentProp.PropertyType, PropReadWriteRule.ReadWrite, null);
                var newProp = new BOProp(profDef) {Value = parentProp.Value};
                propsToInclude.Add(newProp);
            }
        }

        /// <summary>
        /// Builds a collection of properties to include in the insertion,
        /// depending on the inheritance type.
        /// </summary>
        protected override IBOPropCol GetPropsToInclude(IClassDef currentClassDef)
        {
            var propsToInclude = base.GetPropsToInclude(currentClassDef);
            if (currentClassDef.IsUsingClassTableInheritance())
            {
                AddParentID(propsToInclude);
            }

            return propsToInclude;
        }
    }
}