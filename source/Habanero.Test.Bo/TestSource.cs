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
using Habanero.Base.Exceptions;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestSource
    {
        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            string sourceName = TestUtil.CreateRandomString();
            string entityName = TestUtil.CreateRandomString();
            //---------------Execute Test ----------------------
            Source source = new Source(sourceName, entityName);
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(sourceName, source.Name);
            StringAssert.AreEqualIgnoringCase(entityName, source.EntityName);
            Assert.AreEqual(0, source.Joins.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestToString()
        {
            //---------------Set up test pack-------------------
            string sourceName = TestUtil.CreateRandomString();
            Source source = new Source(sourceName);

            //---------------Execute Test ----------------------
            string sourceToString = source.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(sourceName, sourceToString);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestToString_WithJoin()
        {
            //---------------Set up test pack-------------------
            string sourceName = TestUtil.CreateRandomString();
            Source source = new Source(sourceName);
            string subSourceName = TestUtil.CreateRandomString();
            source.JoinToSource(new Source(subSourceName));
            //---------------Execute Test ----------------------
            string sourceToString = source.ToString();

            //---------------Test Result -----------------------
            string expectedSourceToString = string.Format("{0}.{1}", sourceName, subSourceName);
            StringAssert.AreEqualIgnoringCase(expectedSourceToString, sourceToString);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestToString_WithJoin_WithFurtherJoin()
        {
            //---------------Set up test pack-------------------
            string sourceName = TestUtil.CreateRandomString();
            string subSourceName = TestUtil.CreateRandomString();
            string subsubSourceName = TestUtil.CreateRandomString();
            Source source = new Source(sourceName);
            Source subSource = new Source(subSourceName);
            source.JoinToSource(subSource);
            subSource.JoinToSource(new Source(subsubSourceName));
            //---------------Execute Test ----------------------
            string sourceToString = source.ToString();

            //---------------Test Result -----------------------
            string expectedSourceToString = string.Format("{0}.{1}.{2}", sourceName, subSourceName, subsubSourceName);
            StringAssert.AreEqualIgnoringCase(expectedSourceToString, sourceToString);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEquals()
        {
            //---------------Set up test pack-------------------
            string sourceName = TestUtil.CreateRandomString();
            Source source = new Source(sourceName);
            Source source2 = new Source(sourceName);
            //---------------Execute Test ----------------------
            bool success = source.Equals(source2);
            //---------------Test Result -----------------------
            Assert.IsTrue(success);
            Assert.AreEqual(source.GetHashCode(), source2.GetHashCode());
            //---------------Tear Down -------------------------

        }

        [Test]
        public void TestEquals_Fails()
        {
            //---------------Set up test pack-------------------
            Source source = new Source(TestUtil.CreateRandomString());
            Source source2 = new Source(TestUtil.CreateRandomString());
            //---------------Execute Test ----------------------
            bool success = source.Equals(source2);
            //---------------Test Result -----------------------
            Assert.IsFalse(success);
            Assert.AreNotEqual(source.GetHashCode(), source2.GetHashCode());
            //---------------Tear Down -------------------------

        }

        [Test]
        public void TestEquals_ComparedToNull()
        {
            //---------------Set up test pack-------------------
            string sourceName = TestUtil.CreateRandomString();
            Source source = new Source(sourceName);
            //---------------Execute Test ----------------------
            bool success = source.Equals(null);
            //---------------Test Result -----------------------
            Assert.IsFalse(success);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestName()
        {
            //---------------Set up test pack-------------------
            string sourceName = TestUtil.CreateRandomString();
            Source source = new Source(TestUtil.CreateRandomString());
            //---------------Execute Test ----------------------
            source.Name = sourceName;
            //---------------Test Result -----------------------
            Assert.AreEqual(sourceName, source.Name);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEntityName()
        {
            //---------------Set up test pack-------------------
            string entityName = TestUtil.CreateRandomString();
            Source source = new Source(TestUtil.CreateRandomString());
            //---------------Execute Test ----------------------
            source.EntityName = entityName;
            //---------------Test Result -----------------------
            Assert.AreEqual(entityName, source.EntityName);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestJoinToSource()
        {
            //---------------Set up test pack-------------------
            Source fromSource = new Source("FromSource", "FromSourceEntity");
            Source toSource = new Source("ToSource", "ToSourceEntity");

            //---------------Execute Test ----------------------
            fromSource.JoinToSource(toSource);

            //---------------Test Result -----------------------

            Assert.AreEqual(1, fromSource.Joins.Count);
            Assert.AreSame(fromSource, fromSource.Joins[0].FromSource);
            Assert.AreSame(toSource, fromSource.Joins[0].ToSource);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestJoinToSource_AlreadyJoined()
        {
            //---------------Set up test pack-------------------
            Source fromSource = new Source("FromSource", "FromSourceEntity");
            Source toSource = new Source("ToSource", "ToSourceEntity");
            Source toSource2 = new Source("ToSource", "ToSourceEntity");
            fromSource.JoinToSource(toSource);

            //---------------Execute Test ----------------------
            fromSource.JoinToSource(toSource2);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, fromSource.Joins.Count);
            Assert.AreSame(fromSource, fromSource.Joins[0].FromSource);
            Assert.AreSame(toSource, fromSource.Joins[0].ToSource);
        }

        [Test]
        public void TestJoinToSource_NullSource()
        {
            //---------------Set up test pack-------------------
            Source fromSource = new Source("FromSource", "FromSourceEntity");

            //---------------Execute Test ----------------------
            fromSource.JoinToSource(null);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, fromSource.Joins.Count);
        }

        [Test]
        public void TestMergeWith_Impossible()
        {
            //-------------Setup Test Pack ------------------
            Source originalSource = new Source("FromSource", "FromSourceEntity");
            Source toSource = new Source("ToSource", "ToSourceEntity");

            //-------------Execute test ---------------------
            Exception exception = null;
            try
            {
                originalSource.MergeWith(toSource);
            } catch (Exception ex) { exception = ex; }
            //-------------Test Result ----------------------
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(HabaneroDeveloperException), exception);
            StringAssert.Contains("A source cannot merge with another source if they do not have the same base source.", exception.Message);
        }

        [Test]
        public void TestMergeWith_EmptySource()
        {
            //-------------Setup Test Pack ------------------
            Source fromSource = new Source("FromSource", "FromSourceEntity");
            Source otherSource = new Source("", "FromSourceEntity");
            otherSource.JoinToSource(new Source("ToSource", "ToSourceEntity"));

            //-------------Execute test ---------------------
            fromSource.MergeWith(otherSource);
            //-------------Test Result ----------------------
            Assert.AreEqual(0, fromSource.Joins.Count);
        }

        [Test]
        public void TestMergeWith_NullSource()
        {
            //-------------Setup Test Pack ------------------
            Source fromSource = new Source("FromSource", "FromSourceEntity");

            //-------------Execute test ---------------------
            fromSource.MergeWith(null);
            //-------------Test Result ----------------------
            Assert.AreEqual(0, fromSource.Joins.Count);
        }

        [Test]
        public void TestMergeWith_Simple()
        {
            //-------------Setup Test Pack ------------------
            Source fromSource = new Source("FromSource", "FromSourceEntity");
            Source otherSource = new Source("FromSource", "FromSourceEntity");
            otherSource.JoinToSource(new Source("ToSource", "ToSourceEntity"));

            //-------------Execute test ---------------------
            fromSource.MergeWith(otherSource);
            //-------------Test Result ----------------------
            Assert.AreSame(fromSource.ChildSource, otherSource.ChildSource);
        }

        [Test]
        public void TestMergeWith_TwoLevels()
        {
            //-------------Setup Test Pack ------------------
            Source originalSource = new Source("FromSource", "FromSourceEntity");
            Source otherSource = new Source("FromSource", "FromSourceEntity");
            Source childSource = new Source("ChildSource", "ChildSourceEntity");
            Source grandchildSource = new Source("GrandChildSource", "GrandchildSourceEntity");
            childSource.JoinToSource(grandchildSource);
            otherSource.JoinToSource(childSource);

            //-------------Execute test ---------------------
            originalSource.MergeWith(otherSource);

            //-------------Test Result ----------------------
            Assert.AreSame(grandchildSource, originalSource.ChildSource.ChildSource);
        }

        [Test]
        public void TestMergeWith_IncludesJoinFields()
        {
            //-------------Setup Test Pack ------------------
            Source originalSource = new Source("FromSource", "FromSourceEntity");
            Source otherSource = new Source("FromSource", "FromSourceEntity");
            Source childSource = new Source("ToSource", "ToSourceEntity");
            otherSource.JoinToSource(childSource);
            QueryField field1 = new QueryField("Prop1", "Prop1Field", otherSource);
            QueryField field2 = new QueryField("Prop1", "Prop1Field", childSource);
            otherSource.Joins[0].JoinFields.Add(new Source.Join.JoinField(field1, field2));

            //-------------Execute test ---------------------
            originalSource.MergeWith(otherSource);

            //-------------Test Result ----------------------
            Assert.AreEqual(1, originalSource.Joins.Count);
            Assert.AreEqual(1, originalSource.Joins[0].JoinFields.Count);
            Assert.AreEqual(field1, originalSource.Joins[0].JoinFields[0].FromField);
            Assert.AreEqual(field2, originalSource.Joins[0].JoinFields[0].ToField);
        }

        [Test]
        public void TestMergeWith_EqualSource()
        {
            //-------------Setup Test Pack ------------------
            Source originalSource = new Source("FromSource", "FromSourceEntity");
            originalSource.JoinToSource(new Source("ToSource", "ToSourceEntity"));
            Source otherSource = new Source("FromSource", "FromSourceEntity");
            otherSource.JoinToSource(new Source("ToSource", "ToSourceEntity"));
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            originalSource.MergeWith(otherSource);
            //-------------Test Result ----------------------
            Assert.AreEqual(1, originalSource.Joins.Count);
        }

        [Test]
        public void TestMergeWith_EqualSource_DifferentChildSources()
        {
            //-------------Setup Test Pack ------------------
            Source originalSource = new Source("FromSource", "FromSourceEntity");
            originalSource.JoinToSource(new Source("ToSource", "ToSourceEntity"));
            Source otherSource = new Source("FromSource", "FromSourceEntity");
            Source childSource = new Source("ToSource", "ToSourceEntity");
            otherSource.JoinToSource(childSource);
            childSource.JoinToSource(new Source("GrandchildSource", "GrandchildSourceEntity"));
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            originalSource.MergeWith(otherSource);
            //-------------Test Result ----------------------
            Assert.AreEqual(1, originalSource.Joins.Count);
            Assert.AreEqual(1, originalSource.ChildSource.Joins.Count);
        }


        [Test]
        public void TestFromString_SimpleCase()
        {
            //---------------Set up test pack-------------------
            string sourcename = "SourceName";
            
            //---------------Execute Test ----------------------
            Source source = Source.FromString(sourcename);
            //---------------Test Result -----------------------

            Assert.AreEqual(sourcename, source.Name);
            Assert.AreEqual(0, source.Joins.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFromString_EmptySourceName()
        {
            //---------------Set up test pack-------------------
            string sourcename = "";

            //---------------Execute Test ----------------------
            Source source = Source.FromString(sourcename);
            //---------------Test Result -----------------------

            Assert.IsNull(source);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFromString_TwoLevels()
        {
            //---------------Set up test pack-------------------
            const string sourcename = "OneSource.TwoSource";

            //---------------Execute Test ----------------------
            Source source = Source.FromString(sourcename);
            //---------------Test Result -----------------------

            string[] sourceParts = sourcename.Split('.');
            Assert.AreEqual(sourceParts[0], source.Name);
            Assert.AreEqual(1, source.Joins.Count);
            Source.Join join = source.Joins[0];
            Assert.AreSame(source, join.FromSource);
            Assert.AreEqual(sourceParts[1], join.ToSource.Name);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFromString_TwoLevels_CreatesLeftJoins()
        {
            //---------------Set up test pack-------------------
            const string sourcename = "OneSource.TwoSource";

            //---------------Execute Test ----------------------
            Source source = Source.FromString(sourcename);
            //---------------Test Result -----------------------

            Source.Join join = source.Joins[0];
            Assert.AreEqual(Source.JoinType.LeftJoin, join.JoinType);
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestFromString_ThreeLevels()
        {
            //---------------Set up test pack-------------------
            const string sourcename = "OneSource.TwoSource.ThreeSource";

            //---------------Execute Test ----------------------
            Source oneSource = Source.FromString(sourcename);
            //---------------Test Result -----------------------

            string[] sourceParts = sourcename.Split('.');
            Assert.AreEqual(sourceParts[0], oneSource.Name);
            Assert.AreEqual(1, oneSource.Joins.Count);
            Source.Join oneJoin = oneSource.Joins[0];
            Assert.AreSame(oneSource, oneJoin.FromSource);
            Assert.AreEqual(sourceParts[1], oneJoin.ToSource.Name);
            Source twoSource = oneJoin.ToSource;
            Source.Join twoJoin = twoSource.Joins[0];
            Assert.AreSame(twoSource, twoJoin.FromSource);
            Assert.AreEqual(sourceParts[2], twoJoin.ToSource.Name);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestChildSource()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_TABLE");
            Source joinSource = new Source("JoinSource", "MY_JOINED_TABLE");
            source.Joins.Add(new Source.Join(source, joinSource));
            //-------------Execute test ---------------------
            Source childSource = source.ChildSource;
            //-------------Test Result ----------------------
            Assert.AreSame(joinSource, childSource);
        }

        [Test]
        public void TestChildSource_NoChild()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_TABLE");

            //-------------Execute test ---------------------
            Source childSource = source.ChildSource;
            //-------------Test Result ----------------------
            Assert.IsNull(childSource);
        }

        [Test]
        public void TestChildSourceLeaf()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_TABLE");
            Source joinSource = new Source("JoinSource", "MY_JOINED_TABLE");
            source.JoinToSource(joinSource);
            Source joinSource2 = new Source("JoinSource2", "MY_JOINED_TABLE2");
            joinSource.JoinToSource(joinSource2);

            //-------------Execute test ---------------------
            Source childSourceLeaf = source.ChildSourceLeaf;
            //-------------Test Result ----------------------
            Assert.AreSame(joinSource2, childSourceLeaf);
        }

        [Test]
        public void TestChildSourceLeaf_NoChildren()
        {
            //-------------Setup Test Pack ------------------
            Source source = new Source("MySource", "MY_TABLE");

            //-------------Execute test ---------------------
            Source childSourceLeaf = source.ChildSourceLeaf;
            //-------------Test Result ----------------------
            Assert.AreSame(source, childSourceLeaf);
        }

        [Test]
        public void TestInheritanceJoinStructure()
        {
            //-------------Setup Test Pack ------------------
            const string tableName = "MY_SOURCE";
            Source source = new Source("MySource", tableName);
            const string joinTableName = "MY_BASE_TABLE";
            Source joinSource = new Source("MyBaseSource", joinTableName);

            Source.Join join = new Source.Join(source, joinSource);
            QueryField fromField = new QueryField("FromField", "FROM_FIELD", source);
            QueryField toField = new QueryField("ToField", "TO_FIELD", joinSource);

            //-------------Execute test ---------------------
            join.JoinFields.Add(new Source.Join.JoinField(fromField, toField));
            source.InheritanceJoins.Add(join);
            //-------------Test Result ----------------------

            Assert.AreEqual(1, source.InheritanceJoins.Count);
            Assert.AreSame(join, source.InheritanceJoins[0]);
            Assert.AreSame(fromField, join.JoinFields[0].FromField);
            Assert.AreSame(toField, join.JoinFields[0].ToField);
        }

        //TODO: Write Tests for MergeWith with joins to multiple classes from one source

        [Test]
        public void TestMergeWith_IncludesInheritanceJoinFields()
        {
            //-------------Setup Test Pack ------------------
            Source originalSource = new Source("FromSource", "FromSourceEntity");
            Source otherSource = new Source("FromSource", "FromSourceEntity");
            Source childSource = new Source("ToSource", "ToSourceEntity");
            Source.Join join = new Source.Join(otherSource, childSource);
            QueryField field1 = new QueryField("FromSourceProp1", "FromSourceProp1Field", otherSource);
            QueryField field2 = new QueryField("ToSourceProp1", "ToSourceProp1Field", childSource);
            otherSource.InheritanceJoins.Add(join);
            join.JoinFields.Add(new Source.Join.JoinField(field1, field2));

            //-------------Execute test ---------------------
            originalSource.MergeWith(otherSource);

            //-------------Test Result ----------------------
            Assert.AreEqual(1, originalSource.InheritanceJoins.Count);
            Assert.AreEqual(1, originalSource.InheritanceJoins[0].JoinFields.Count);
            Assert.AreEqual(field1, originalSource.InheritanceJoins[0].JoinFields[0].FromField);
            Assert.AreEqual(field2, originalSource.InheritanceJoins[0].JoinFields[0].ToField);
        }

        
    }
}
