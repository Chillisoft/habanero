using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
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
        public void Test_Join_Constructor()
        {
            //---------------Set up test pack-------------------
            Source fromSource = new Source("From");
            Source toSource = new Source("To");
            //---------------Execute Test ----------------------
            Source.Join join = new Source.Join(fromSource, toSource);

            //---------------Test Result -----------------------
            Assert.AreSame(fromSource, join.FromSource);
            Assert.AreSame(toSource, join.ToSource);
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
            string sourcename = "OneSource.TwoSource";

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
        public void TestFromString_ThreeLevels()
        {
            //---------------Set up test pack-------------------
            string sourcename = "OneSource.TwoSource.ThreeSource";

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
    }
}
