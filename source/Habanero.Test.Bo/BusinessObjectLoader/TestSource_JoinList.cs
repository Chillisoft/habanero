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
using Habanero.Base;
using Habanero.Base.Exceptions;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectLoader
{
    [TestFixture]
    public class TestSource_JoinList
    {
        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            Source source = new Source("TestSource");
            //---------------Execute Test ----------------------
            Source.JoinList joinList = new Source.JoinList(source);
            //---------------Test Result -----------------------
            Assert.AreSame(source, joinList.FromSource);
        }

        [Test]
        public void TestConstructor_NullFromSource()
        {
            //---------------Set up test pack-------------------
            const Source source = null;
            //---------------Execute Test ----------------------
            Exception exception = null;
            try
            {
                Source.JoinList joinList = new Source.JoinList(source);
            } catch (Exception ex)
            {
                exception = ex;
            }
            
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Expected a constructor with null parameter to throw an exception");
            Assert.IsInstanceOfType(typeof(ArgumentNullException), exception);
            ArgumentNullException argumentNullException = (ArgumentNullException)exception;
            Assert.AreEqual("fromSource", argumentNullException.ParamName);
        }

        [Test]
        public void TestAddNewJoinTo()
        {
            //---------------Set up test pack-------------------
            Source source = new Source("TestSource");
            Source.JoinList joinList = new Source.JoinList(source);
            Source toSource = new Source("TestToSource");

            //---------------Execute Test ----------------------
            Source.Join join = joinList.AddNewJoinTo(toSource, Source.JoinType.InnerJoin);

            //---------------Test Result -----------------------
            Assert.IsNotNull(join);
            Assert.AreEqual(1, joinList.Count);
            Assert.AreSame(join, joinList[0]);
        }

        [Test]
        public void TestAddNewJoinTo_AlreadyJoined()
        {
            //---------------Set up test pack-------------------
            Source fromSource = new Source("FromSource", "FromSourceEntity");
            Source.JoinList joinList = new Source.JoinList(fromSource);
            Source toSource = new Source("ToSource", "ToSourceEntity");
            Source toSource2 = new Source("ToSource", "ToSourceEntity");
            joinList.AddNewJoinTo(toSource, Source.JoinType.InnerJoin);

            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, joinList.Count);
            
            //---------------Execute Test ----------------------
            joinList.AddNewJoinTo(toSource2, Source.JoinType.InnerJoin);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, joinList.Count);
            Assert.AreSame(fromSource, joinList[0].FromSource);
            Assert.AreSame(toSource, joinList[0].ToSource);
        }

        [Test]
        public void TestAddNewJoinTo_NullSource()
        {
            //---------------Set up test pack-------------------
            Source fromSource = new Source("FromSource", "FromSourceEntity");
            Source.JoinList joinList = new Source.JoinList(fromSource);

            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, joinList.Count);

            //---------------Execute Test ----------------------
            joinList.AddNewJoinTo(null, Source.JoinType.InnerJoin);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, joinList.Count);

        }

        [Test]
        public void TestMergeWith()
        {
            //-------------Setup Test Pack ------------------
            Source originalSource = new Source("FromSource", "FromSourceEntity");
            Source.JoinList originalJoinList = new Source.JoinList(originalSource);

            Source otherSource = new Source("FromSource", "FromSourceEntity");
            Source.JoinList joinList = new Source.JoinList(otherSource);
            Source childSource = new Source("ToSource", "ToSourceEntity");
            Source.Join join = joinList.AddNewJoinTo(childSource, Source.JoinType.InnerJoin);

            QueryField field1 = new QueryField("FromSourceProp1", "FromSourceProp1Field", otherSource);
            QueryField field2 = new QueryField("ToSourceProp1", "ToSourceProp1Field", childSource);
            join.JoinFields.Add(new Source.Join.JoinField(field1, field2));

            //-------------Execute test ---------------------
            originalJoinList.MergeWith(joinList);

            //-------------Test Result ----------------------
            Assert.AreEqual(1, originalJoinList.Count);
            Assert.AreEqual(1, originalJoinList[0].JoinFields.Count);
            Assert.AreEqual(field1, originalJoinList[0].JoinFields[0].FromField);
            Assert.AreEqual(field2, originalJoinList[0].JoinFields[0].ToField);
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
            }
            catch (Exception ex) { exception = ex; }
            //-------------Test Result ----------------------
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(typeof(HabaneroDeveloperException), exception);
            StringAssert.Contains("A source cannot merge with another source if they do not have the same base source.", exception.Message);
        }

        [Test]
        public void TestMergeWith_EmptySource()
        {
            //-------------Setup Test Pack ------------------
            Source originalSource = new Source("FromSource", "FromSourceEntity");
            Source.JoinList originalJoinList = new Source.JoinList(originalSource);
            Source otherSource = new Source("", "FromSourceEntity");
            Source.JoinList joinList = new Source.JoinList(otherSource);

            otherSource.JoinToSource(new Source("ToSource", "ToSourceEntity"));
            //-------------Execute test ---------------------
            originalSource.MergeWith(otherSource);
            //-------------Test Result ----------------------
            Assert.AreEqual(0, originalSource.Joins.Count);
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
        public void TestMergeWith_LeftJoin()
        {
            //-------------Setup Test Pack ------------------
            Source fromSource = new Source("FromSource", "FromSourceEntity");
            Source toSource = new Source("ToSource", "ToSourceEntity");
            Source.Join join = new Source.Join(fromSource, toSource, Source.JoinType.LeftJoin);
            Source.JoinList joinList = new Source.JoinList(fromSource);
            joinList.Add(join);
            //-------------Execute test ---------------------
            fromSource.Joins.MergeWith(joinList);
            //-------------Test Result ----------------------
            Assert.AreEqual(1, fromSource.Joins.Count);
            Assert.AreEqual(Source.JoinType.LeftJoin, fromSource.Joins[0].JoinType);
        }

        [Test]
        public void TestMergeWith_Simple()
        {
            //-------------Setup Test Pack ------------------
            Source originalSource = new Source("FromSource", "FromSourceEntity");
            Source.JoinList originalJoinList = new Source.JoinList(originalSource);
            Source otherSource = new Source("FromSource", "FromSourceEntity");
            Source.JoinList joinList = new Source.JoinList(otherSource);
            joinList.AddNewJoinTo(new Source("ToSource", "ToSourceEntity"), Source.JoinType.InnerJoin);

            //-------------Execute test ---------------------
            originalJoinList.MergeWith(joinList);

            //-------------Test Result ----------------------
            Assert.AreEqual(1, originalJoinList.Count);
            Assert.AreSame(joinList[0].ToSource, originalJoinList[0].ToSource);
        }

        [Test]
        public void TestMergeWith_TwoLevels()
        {
            //-------------Setup Test Pack ------------------

            Source originalSource = new Source("FromSource", "FromSourceEntity");
            Source.JoinList originalJoinList = originalSource.Joins;
            Source otherSource = new Source("FromSource", "FromSourceEntity");
            Source.JoinList joinList = otherSource.Joins;
            Source childSource = new Source("ChildSource", "ChildSourceEntity");
            Source grandchildSource = new Source("GrandChildSource", "GrandchildSourceEntity");
            otherSource.JoinToSource(childSource);
            childSource.JoinToSource(grandchildSource);
            //-------------Assert Preconditions -------------
            Assert.IsNull(originalSource.ChildSource);
            Assert.IsNotNull(otherSource.ChildSource);
            Assert.IsNotNull(otherSource.ChildSource.ChildSource);
            //-------------Execute test ---------------------
            originalJoinList.MergeWith(joinList);

            //-------------Test Result ----------------------
            Assert.AreEqual(1, originalJoinList.Count);
            Source originalJoinListNewChild = originalJoinList[0].ToSource;
            Assert.AreSame(childSource, originalJoinListNewChild);
            Assert.AreSame(grandchildSource, originalSource.ChildSource.ChildSource);
        }

        //[Test]
        //public void TestMergeWith_IncludesJoinFields()
        //{
        //    //-------------Setup Test Pack ------------------
        //    Source originalSource = new Source("FromSource", "FromSourceEntity");
        //    Source.JoinList originalJoinList = originalSource.Joins;
        //    Source otherSource = new Source("FromSource", "FromSourceEntity");
        //    Source.JoinList joinList = otherSource.Joins;
            
        //    Source childSource = new Source("ToSource", "ToSourceEntity");
        //    otherSource.JoinToSource(childSource);
        //    QueryField field1 = new QueryField("Prop1", "Prop1Field", otherSource);
        //    QueryField field2 = new QueryField("Prop1", "Prop1Field", childSource);
        //    otherSource.Joins[0].JoinFields.Add(new Source.Join.JoinField(field1, field2));

        //    //-------------Execute test ---------------------
        //    originalJoinList.MergeWith(joinList);

        //    //-------------Test Result ----------------------
        //    Assert.AreEqual(1, originalSource.Joins.Count);
        //    Assert.AreEqual(1, originalSource.Joins[0].JoinFields.Count);
        //    Assert.AreEqual(field1, originalSource.Joins[0].JoinFields[0].FromField);
        //    Assert.AreEqual(field2, originalSource.Joins[0].JoinFields[0].ToField);
        //}

        //[Test]
        //public void TestMergeWith_EqualSource()
        //{
        //    //-------------Setup Test Pack ------------------
        //    Source originalSource = new Source("FromSource", "FromSourceEntity");
        //    originalSource.JoinToSource(new Source("ToSource", "ToSourceEntity"));
        //    Source otherSource = new Source("FromSource", "FromSourceEntity");
        //    otherSource.JoinToSource(new Source("ToSource", "ToSourceEntity"));
        //    //-------------Test Pre-conditions --------------

        //    //-------------Execute test ---------------------
        //    originalSource.MergeWith(otherSource);
        //    //-------------Test Result ----------------------
        //    Assert.AreEqual(1, originalSource.Joins.Count);
        //}

        //[Test]
        //public void TestMergeWith_EqualSource_DifferentChildSources()
        //{
        //    //-------------Setup Test Pack ------------------
        //    Source originalSource = new Source("FromSource", "FromSourceEntity");
        //    originalSource.JoinToSource(new Source("ToSource", "ToSourceEntity"));
        //    Source otherSource = new Source("FromSource", "FromSourceEntity");
        //    Source childSource = new Source("ToSource", "ToSourceEntity");
        //    otherSource.JoinToSource(childSource);
        //    childSource.JoinToSource(new Source("GrandchildSource", "GrandchildSourceEntity"));
        //    //-------------Test Pre-conditions --------------

        //    //-------------Execute test ---------------------
        //    originalSource.MergeWith(otherSource);
        //    //-------------Test Result ----------------------
        //    Assert.AreEqual(1, originalSource.Joins.Count);
        //    Assert.AreEqual(1, originalSource.ChildSource.Joins.Count);
        //}
    }
}