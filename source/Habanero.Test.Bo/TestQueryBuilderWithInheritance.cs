//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestQueryBuilderWithInheritance
    {
        [Test]
        public void TestPrepareSource_OneLevel()
        {
            //---------------Set up test pack-------------------
            Entity.LoadDefaultClassDef();
            ClassDef classDef = Part.LoadClassDef_WithClassTableInheritance();
            Source source = null;
            //---------------Execute Test ----------------------
            QueryBuilder.PrepareSource(classDef, ref source);

            //---------------Test Result -----------------------
            Assert.IsNotNull(source);
            AssertPartInheritanceStructureCorrect(source);
            //---------------Tear down -------------------------
        }

        protected virtual void AssertPartInheritanceStructureCorrect(Source source)
        {
            Source correctPartSourceStructure = GetCorrectPartSourceStructure();
            AssertSourcesEqual(correctPartSourceStructure, source);
            //Assert.AreEqual("Part", source.Name);
            //Assert.AreEqual("table_Part", source.EntityName);
            //Assert.AreEqual(0, source.Joins.Count);
            //Assert.AreEqual(1, source.InheritanceJoins.Count);
            //Source.Join inheritanceJoin = source.InheritanceJoins[0];
            //Assert.AreEqual("Entity", inheritanceJoin.ToSource.Name);
            //Assert.AreEqual("table_Entity", inheritanceJoin.ToSource.EntityName);
            //Assert.AreEqual(1, inheritanceJoin.JoinFields);
            //Source.Join.JoinField joinField = inheritanceJoin.JoinFields[0];
            //Assert.AreEqual(source, joinField.FromField.Source);
            //Assert.AreEqual("PartID", joinField.FromField.PropertyName);
            //Assert.AreEqual("field_Part_ID", joinField.FromField.FieldName);
            //Assert.AreEqual(inheritanceJoin.ToSource, joinField.ToField.Source);
            //Assert.AreEqual("EntityID", joinField.FromField.PropertyName);
            //Assert.AreEqual("field_Entity_ID", joinField.FromField.FieldName);
        }

        protected static void AssertSourcesEqual(Source expected, Source actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.EntityName, actual.EntityName);
            AssertJoinListsEqual(expected.Joins, actual.Joins);
            AssertJoinListsEqual(expected.InheritanceJoins, actual.InheritanceJoins);
        }

        private static void AssertJoinListsEqual(Source.JoinList expected, Source.JoinList actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            foreach (Source.Join expectedJoin in expected)
            {
                string joinToSourceName = expectedJoin.ToSource.Name;
                Source.Join actualJoin = actual.Find(delegate(Source.Join join1)
                {
                    return join1.ToSource.Name == joinToSourceName;
                });
                Assert.IsNotNull(actualJoin, string.Format("Could not find a join from {0} to {1}", expected.FromSource.Name, joinToSourceName));
                AssertJoinsEqual(expectedJoin, actualJoin);
            }
        }

        private static void AssertJoinsEqual(Source.Join expectedJoin, Source.Join actualJoin)
        {
            AssertSourcesEqual(expectedJoin.ToSource, actualJoin.ToSource);
            AssertJoinFieldsListEqual(expectedJoin.JoinFields, actualJoin.JoinFields);
        }

        private static void AssertJoinFieldsListEqual(List<Source.Join.JoinField> expectedJoinFields, List<Source.Join.JoinField> actualJoinFields)
        {
            Assert.AreEqual(expectedJoinFields.Count, actualJoinFields.Count);

            foreach (Source.Join.JoinField expectedJoinField in expectedJoinFields)
            {
                string expectedJoinToPropertyName = expectedJoinField.ToField.PropertyName;
                string expectedJoinFromPropertyName = expectedJoinField.FromField.PropertyName;
                Source.Join.JoinField actualJoinField = actualJoinFields.Find(delegate(Source.Join.JoinField joinField)
                {
                    return joinField.FromField.PropertyName == expectedJoinFromPropertyName 
                        && joinField.ToField.PropertyName == expectedJoinToPropertyName;
                });
                Assert.IsNotNull(actualJoinField, string.Format(
                    "Could not find a join field from {0}.{1} to {2}.{3}.",
                    expectedJoinField.FromField.Source.Name, expectedJoinFromPropertyName,
                    expectedJoinField.ToField.Source.Name, expectedJoinToPropertyName));
                AssertJoinFieldsEqual(expectedJoinField, actualJoinField);
            }
        }

        private static void AssertJoinFieldsEqual(Source.Join.JoinField expectedJoinField, Source.Join.JoinField actualJoinField)
        {
            AssertQueryFieldsEqual(expectedJoinField.FromField, actualJoinField.FromField);
            AssertQueryFieldsEqual(expectedJoinField.ToField, actualJoinField.ToField);
        }

        private static void AssertQueryFieldsEqual(QueryField expectedQueryField, QueryField actualQueryField)
        {
            Assert.AreEqual(expectedQueryField.Source, actualQueryField.Source);
            Assert.AreEqual(expectedQueryField.PropertyName, actualQueryField.PropertyName);
            Assert.AreEqual(expectedQueryField.FieldName, actualQueryField.FieldName);
        }

        protected virtual Source GetCorrectPartSourceStructure()
        {
            Source partSource = new Source("Part", "table_Part");
            Source entitySource = new Source("Entity", "table_Entity");
            Source.Join join = partSource.InheritanceJoins.AddNewJoinTo(entitySource);
            QueryField partQueryField = new QueryField("PartID", "field_Part_ID", partSource);
            QueryField entityQueryField = new QueryField("EntityID", "field_Entity_ID", entitySource);
            Source.Join.JoinField joinField = new Source.Join.JoinField(partQueryField, entityQueryField);
            join.JoinFields.Add(joinField);
            return partSource;
        }

        //[Test]
        //public void TestPrepareSource_TwoLevels()
        //{
        //    //---------------Set up test pack-------------------
        //    Entity.LoadDefaultClassDef();
        //    Part.LoadClassDef_WithClassTableInheritance();
        //    ClassDef classDef = Structure.Engine.LoadClassDef_WithClassTableInheritance();
        //    Source source = null;
        //    //---------------Execute Test ----------------------
        //    QueryBuilder.PrepareSource(classDef, ref source);

        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(source);
        //    AssertPartInheritanceStructureCorrect(source);
        //    //---------------Tear down -------------------------
        //}
    }
}
