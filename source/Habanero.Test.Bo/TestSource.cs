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
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestToString()
        {
            //---------------Set up test pack-------------------
            string sourceName = TestUtil.CreateRandomString();
            string entityName = TestUtil.CreateRandomString();
            //---------------Execute Test ----------------------
            Source source = new Source(sourceName, entityName);
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(sourceName, source.Name);
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

    }
}
