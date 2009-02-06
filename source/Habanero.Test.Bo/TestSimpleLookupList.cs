using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestSimpleLookupList 
    {
        [SetUp]
        public void SetupTest()
        {
        }

       
//        [Test]
//        public void TestGetLookupList() 
//        {
//            SimpleLookupList source = new SimpleLookupList(typeof (ContactPersonTestBO));
//            source.PropDef = new PropDef("name", typeof (string), PropReadWriteRule.ReadWrite, null);
//            Dictionary<string, string> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
//            Assert.AreEqual(3, col.Count);
//            foreach (string o in col.Values) {
//                Assert.AreSame(typeof(string), o.GetType());
//                Guid parsedGuid;
//                Assert.IsTrue(StringUtilities.GuidTryParse(o, out parsedGuid));
//            }
//        }

//        [Test]
//        public void TestCallingGetLookupListTwiceOnlyAccessesDbOnce()
//        {
//            SimpleLookupList source = new SimpleLookupList(typeof(ContactPersonTestBO));
//            source.PropDef = new PropDef("name", typeof(string), PropReadWriteRule.ReadWrite, null);
//            Dictionary<string, string> col = source.GetLookupList(DatabaseConnection.CurrentConnection);
//            Dictionary<string, string> col2 = source.GetLookupList(DatabaseConnection.CurrentConnection);
//            Assert.AreSame(col2, col);
//        }

 
        [Test]
        public void Test_LimitToList_Attribute_Default()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            SimpleLookupList source = new SimpleLookupList(new Dictionary<string, string>());
            //---------------Test Result -----------------------
            Assert.IsFalse(source.LimitToList);
        }

        [Test]
        public void Test_Constructor_WithLimitToList_AsTrue()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            SimpleLookupList source = new SimpleLookupList(new Dictionary<string, string>(), true);
            //---------------Test Result -----------------------
            Assert.IsTrue(source.LimitToList);
        }

        [Test]
        public void Test_Constructor_WithLimitToList_AsFalse()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            SimpleLookupList source = new SimpleLookupList(new Dictionary<string, string>(), false);

            //---------------Test Result -----------------------
            Assert.IsFalse(source.LimitToList);
        }
    }
}