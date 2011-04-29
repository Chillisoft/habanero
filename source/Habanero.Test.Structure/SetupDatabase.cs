// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System.Collections.Generic;
using System.Linq;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;

namespace Habanero.Test.Structure
{
    public class SetupDatabase
    {
        private readonly IDatabaseConnection _databaseConnection;
        private List<string> _statements;
        
        public SetupDatabase(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public void ClearDatabase()
        {
            string database = _databaseConnection.GetConnection().Database;
            _databaseConnection.ExecuteRawSql(String.Format("DROP DATABASE IF EXISTS {0};CREATE DATABASE {0};", database));
        }

        public void CreateDatabase(ClassDefCol classDefCol)
        {
            _statements = new List<string>();
            foreach (ClassDef classDef in classDefCol)
            {
                CreateTable(classDef);
            }
            AddSettingTableIfNeeded(classDefCol);
            foreach (ClassDef classDef in classDefCol)
            {
                CreateRelationships(classDefCol, classDef);
            }
            Apply();
        }

        private void AddSettingTableIfNeeded(ClassDefCol classDefCol)
        {
            foreach (ClassDef classDef in classDefCol)
            {
                if (classDef.TableName.ToUpper() == "SETTING")
                {
                    return;
                }
            }
            const string sql = @"
                CREATE TABLE `setting` (
                  `SettingName` varchar(50) NOT NULL default '',
                  `SettingValue` varchar(1000) NOT NULL default '',
                  `StartDate` datetime default NULL,
                  `EndDate` datetime default NULL,
                  PRIMARY KEY  (`SettingName`)
                )";
            _statements.Add(sql);
        }

        private void Apply()
        {
            _databaseConnection.ExecuteSql(
                from statement in _statements
                select new SqlStatement(_databaseConnection, statement)
            );
        }

        private void CreateTable(ClassDef classDef)
        {
            //CREATE TABLE `setting` (
            //`SettingName` varchar(50) NOT NULL default '',
            //`SettingValue` varchar(1000) NOT NULL default '',
            //`StartDate` datetime default NULL,
            //`EndDate` datetime default NULL,
            //PRIMARY KEY  (`SettingName`)
            //)

            if (classDef.SuperClassDef != null && classDef.SuperClassDef.ORMapping == ORMapping.SingleTableInheritance)
            {
                return;
            }
            string sqlStatement = "CREATE TABLE ";
            sqlStatement += _databaseConnection.SqlFormatter.DelimitTable(classDef.TableName);
            sqlStatement += " (";
            foreach (PropDef propDef in classDef.PropDefcol)
            {
                sqlStatement += _databaseConnection.SqlFormatter.DelimitField(propDef.DatabaseFieldName);
                sqlStatement += " " + GetFieldType(propDef);
                if (propDef.Compulsory)
                {
                    sqlStatement += " NOT NULL";
                }
                sqlStatement += ", ";
            }
            sqlStatement += "PRIMARY KEY (";
            List<string> fields = new List<string>();
            foreach (PropDef propDef in classDef.PrimaryKeyDef)
            {
                fields.Add(_databaseConnection.SqlFormatter.DelimitField(propDef.DatabaseFieldName));
            }
            sqlStatement += String.Join(", ", fields.ToArray());
            sqlStatement += "));";
            _statements.Add(sqlStatement);
        }

        private static string GetFieldType(PropDef propDef)
        {
            string fieldType = "varchar(50)";
            switch(propDef.PropertyTypeName.ToUpper())
            {
                case "DATETIME":
                    fieldType = "datetime";
                    break;
                case "STRING":
                    int length = 50;
                    PropRuleString propRule = null;
                    if (propDef.PropRules.Count > 0)
                    {
                        propRule = propDef.PropRules[0] as PropRuleString;
                    }
                    if (propRule != null)
                    {
                        length = propRule.MaxLength;
                    }
                    if (length <= 0) length = 50;
                    fieldType = "varchar(" + length + ")";
                    break;
                case "GUID":
                    fieldType = "char(38)";
                    break;
                case "IMAGE":
                    fieldType = "blob";
                    break;
                case "BOOL":
                case "BOOLEAN":
                    fieldType = "tinyint(1)";// " unsigned";
                    break;
                case "INT":
                case "INTEGER":
                    fieldType = "integer";
                    break;
                case "SINGLE":
                case "DOUBLE":
                    fieldType = "float";
                    break;
            }
            return fieldType.ToUpper();
        }

        private void CreateRelationships(ClassDefCol classDefCol, ClassDef classDef)
        {
            //ALTER TABLE `invoice` 
            //  ADD CONSTRAINT `Invoice_InvoiceStatus_FK` FOREIGN KEY `Invoice_InvoiceStatus_FK` (`InvoiceStatusID`)
            //    REFERENCES `invoicestatus` (`InvoiceStatusID`)
            //    ON DELETE RESTRICT
            //    ON UPDATE RESTRICT;

            if (classDef.SuperClassDef != null && classDef.SuperClassDef.ORMapping == ORMapping.SingleTableInheritance)
            {
                return;
            }
            string sqlStatement;
            sqlStatement = "ALTER TABLE ";
            sqlStatement += _databaseConnection.SqlFormatter.DelimitTable(classDef.TableName);
            List<string> constraints = new List<string>();
            foreach (RelationshipDef relationshipDef in classDef.RelationshipDefCol)
            {
                if (relationshipDef is SingleRelationshipDef)
                {
                    if (!classDefCol.Contains(relationshipDef.RelatedObjectAssemblyName, relationshipDef.RelatedObjectClassName))
                    {
                        throw new Exception("Related class not found:" + relationshipDef.RelatedObjectAssemblyName + "." + relationshipDef.RelatedObjectClassName);
                    }
                    IClassDef relatedClassDef = classDefCol[relationshipDef.RelatedObjectAssemblyName, relationshipDef.RelatedObjectClassName];

                    string constraintName = _databaseConnection.SqlFormatter.DelimitField(
                        classDef.TableName + "_" + relationshipDef.RelationshipName + "_FK");
                    string constraintSql = " ADD CONSTRAINT " + constraintName;
                    constraintSql += " FOREIGN KEY " + constraintName;
                    List<string> props = new List<string>();
                    List<string> relProps = new List<string>();
                    foreach (RelPropDef relPropDef in relationshipDef.RelKeyDef)
                    {
                        string propName = relPropDef.OwnerPropertyName;
                        string relPropName = relPropDef.RelatedClassPropName;
                        IPropDef ownerPropDef = classDef.GetPropDef(propName);
                        if (ownerPropDef != null)
                        {
                            propName = ownerPropDef.DatabaseFieldName;
                        }
                        IPropDef relatedPropDef = relatedClassDef.GetPropDef(relPropName);
                        if (relatedPropDef != null)
                        {
                            relPropName = relatedPropDef.DatabaseFieldName;
                        }
                        props.Add(_databaseConnection.SqlFormatter.DelimitField(propName));
                        relProps.Add(_databaseConnection.SqlFormatter.DelimitField(relPropName));
                    }
                    constraintSql += " (" + String.Join(",", props.ToArray()) + ")";
                    constraintSql += " REFERENCES ";
                    IClassDef relatedBaseClassDef = relatedClassDef;
                    while (relatedBaseClassDef.SuperClassDef != null && relatedBaseClassDef.SuperClassDef.ORMapping == ORMapping.SingleTableInheritance)
                    {
                        relatedBaseClassDef = (ClassDef)relatedBaseClassDef.SuperClassDef.SuperClassClassDef;
                    }
                    string relatedTableName = relatedBaseClassDef.TableName;
                    constraintSql += _databaseConnection.SqlFormatter.DelimitTable(relatedTableName);
                    constraintSql += " (" + String.Join(",", relProps.ToArray()) + ")";
                    constraintSql += " ON DELETE RESTRICT ";
                    constraintSql += " ON UPDATE RESTRICT";
                    constraints.Add(constraintSql);
                }
            }
            sqlStatement += String.Join(", ", constraints.ToArray());
            sqlStatement += ";";
            if (constraints.Count > 0)
            {
                _statements.Add(sqlStatement);
            }
        }
    }
}