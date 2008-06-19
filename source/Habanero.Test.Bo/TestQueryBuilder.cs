using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestQueryBuilder
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }


        //TODO: make the fields and order fields case insensitive
        [Test]
        public void TestBuiltFields()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            ClassDef classdef = ClassDef.ClassDefs[typeof(MyBO)];
            //---------------Execute Test ----------------------
            SelectQuery query = QueryBuilder.CreateSelectQuery(classdef);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, query.Fields.Count);
            Assert.AreEqual("MyBoID", query.Fields["MyBoID"].PropertyName);
            Assert.AreEqual("MyBO.MyBoID", query.Fields["MyBoID"].FieldName);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestBuiltWithCriteria()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            ClassDef classdef = ClassDef.ClassDefs[typeof(MyBO)];
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, DateTime.Now);
            //---------------Execute Test ----------------------
            SelectQuery query = QueryBuilder.CreateSelectQuery(classdef, criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, query.Criteria);
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestBuiltSource()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            ClassDef classdef = ClassDef.ClassDefs[typeof(MyBO)];
            //---------------Execute Test ----------------------
            SelectQuery query = QueryBuilder.CreateSelectQuery(classdef);
            //---------------Test Result -----------------------
            Assert.AreEqual("MyBO", query.Source);
            //---------------Tear Down -------------------------
        }
    }

}
