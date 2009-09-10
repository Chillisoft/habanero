//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectLoader
{
    [TestFixture]
    public class TestQueryBuilderWithInheritance
    {

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
        }


        #region Utility Methods

        protected static void AssertSourcesEqual(Source expected, Source actual, string context)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.EntityName, actual.EntityName);
            AssertJoinListsEqual(expected.Joins, actual.Joins, context + ".Joins");
            AssertJoinListsEqual(expected.InheritanceJoins, actual.InheritanceJoins, context + ".InheritanceJoins");
        }

        private static void AssertJoinListsEqual(Source.JoinList expected, Source.JoinList actual, string context)
        {
            Assert.AreEqual(expected.Count, actual.Count, context + ".Count");

            foreach (Source.Join expectedJoin in expected)
            {
                string joinToSourceName = expectedJoin.ToSource.Name;
                Source.Join actualJoin = actual.Find(delegate(Source.Join join1)
                                                     {
                                                         return join1.ToSource.Name == joinToSourceName;
                                                     });
                Assert.IsNotNull(actualJoin, string.Format("{0}: Could not find a join from {1} to {2}", context, expected.FromSource.Name, joinToSourceName));
                AssertJoinsEqual(expectedJoin, actualJoin, context + string.Format("(Join to '{0}')", joinToSourceName));
            }
        }

        private static void AssertJoinsEqual(Source.Join expectedJoin, Source.Join actualJoin, string context)
        {
            Assert.AreEqual(expectedJoin.FromSource, actualJoin.FromSource, context + ".FromSource");
            AssertSourcesEqual(expectedJoin.ToSource, actualJoin.ToSource, context + ".ToSource");
            AssertJoinFieldsListEqual(expectedJoin.JoinFields, actualJoin.JoinFields, context + ".JoinFields");
        }

        private static void AssertJoinFieldsListEqual(List<Source.Join.JoinField> expectedJoinFields, List<Source.Join.JoinField> actualJoinFields, string context)
        {
            Assert.AreEqual(expectedJoinFields.Count, actualJoinFields.Count, context + ".Count");

            foreach (Source.Join.JoinField expectedJoinField in expectedJoinFields)
            {
                string expectedJoinToPropertyName = expectedJoinField.ToField.PropertyName;
                string expectedJoinFromPropertyName = expectedJoinField.FromField.PropertyName;
                Source.Join.JoinField actualJoinField = actualJoinFields.Find(delegate(Source.Join.JoinField joinField)
                                                                              {
                                                                                  return joinField.FromField.PropertyName == expectedJoinFromPropertyName
                                                                                         && joinField.ToField.PropertyName == expectedJoinToPropertyName;
                                                                              });
                string expectedJoinFieldDesc = String.Format("from {0}.{1} to {2}.{3}",
                                                             expectedJoinField.FromField.Source.Name,
                                                             expectedJoinFromPropertyName,
                                                             expectedJoinField.ToField.Source.Name,
                                                             expectedJoinToPropertyName);
                Assert.IsNotNull(actualJoinField, string.Format(
                                                      "{0}: Could not find a join field {1}.", context, expectedJoinFieldDesc));
                AssertJoinFieldsEqual(expectedJoinField, actualJoinField,
                                      string.Format("{0}(JoinField {1})", context, expectedJoinFieldDesc));
            }
        }

        private static void AssertJoinFieldsEqual(Source.Join.JoinField expectedJoinField, Source.Join.JoinField actualJoinField, string context)
        {
            AssertQueryFieldsEqual(expectedJoinField.FromField, actualJoinField.FromField, context + ".FromField");
            AssertQueryFieldsEqual(expectedJoinField.ToField, actualJoinField.ToField, context + ".ToField");
        }

        private static void AssertQueryFieldsEqual(QueryField expectedQueryField, QueryField actualQueryField, string context)
        {
            Assert.AreEqual(expectedQueryField.Source, actualQueryField.Source, context + ".Source");
            Assert.AreEqual(expectedQueryField.PropertyName, actualQueryField.PropertyName, context + ".PropertyName");
            Assert.AreEqual(expectedQueryField.FieldName, actualQueryField.FieldName, context + ".FieldName");
        }

        #endregion
        
        [Test]
        public void TestPrepareSource_OneLevel()
        {
            //---------------Set up test pack-------------------
            Entity.LoadDefaultClassDef();
            IClassDef classDef = Part.LoadClassDef_WithClassTableInheritance();
            Source source = null;
            //---------------Execute Test ----------------------
            QueryBuilder.PrepareSource(classDef, ref source);

            //---------------Test Result -----------------------
            Assert.IsNotNull(source);
            Source correctPartSourceStructure = GetCorrectPartSourceStructure();
            AssertSourcesEqual(correctPartSourceStructure, source, "Part");
            //---------------Tear down -------------------------
        }
        
        protected virtual Source GetCorrectPartSourceStructure()
        {
            Source partSource = new Source("Part", "table_class_Part");
            Source entitySource = new Source("Entity", "table_Entity");
            Source.Join join = partSource.InheritanceJoins.AddNewJoinTo(entitySource, Source.JoinType.InnerJoin);
            QueryField partQueryField = new QueryField("PartID", "field_Part_ID", partSource);
            QueryField entityQueryField = new QueryField("EntityID", "field_Entity_ID", entitySource);
            Source.Join.JoinField joinField = new Source.Join.JoinField(partQueryField, entityQueryField);
            join.JoinFields.Add(joinField);
            return partSource;
        }

        [Test]
        public void TestPrepareSource_TwoLevels()
        {
            //---------------Set up test pack-------------------
            Entity.LoadDefaultClassDef();
            Part.LoadClassDef_WithClassTableInheritance();
            IClassDef classDef = Structure.Engine.LoadClassDef_WithClassTableInheritance();
            Source source = null;
            //---------------Execute Test ----------------------
            QueryBuilder.PrepareSource(classDef, ref source);

            //---------------Test Result -----------------------
            Assert.IsNotNull(source);
            Source correctEngineSourceStructure = GetCorrectEngineSourceStructure();
            AssertSourcesEqual(correctEngineSourceStructure, source, "Part");
            //---------------Tear down -------------------------
        }

        protected virtual Source GetCorrectEngineSourceStructure()
        {
            Source engineSource = new Source("Engine", "table_class_Engine");
            Source partSource = GetCorrectPartSourceStructure();
            Source.Join join = engineSource.InheritanceJoins.AddNewJoinTo(partSource, Source.JoinType.InnerJoin);
            QueryField engineQueryField = new QueryField("EngineID", "field_Engine_ID", engineSource);
            QueryField partQueryField = new QueryField("PartID", "field_Part_ID", partSource);
            Source.Join.JoinField joinField = new Source.Join.JoinField(engineQueryField, partQueryField);
            join.JoinFields.Add(joinField);
            return engineSource;
        }
    }
}