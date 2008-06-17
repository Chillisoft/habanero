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
    public class TestSelectQuery
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }
        [Test]
        public void TestConstruct()
        {
            //---------------Set up test pack-------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, DateTime.Now);
            //---------------Execute Test ----------------------
            SelectQuery<MyBO> query = new SelectQuery<MyBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(criteria, query.Criteria);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFields()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            //---------------Execute Test ----------------------
            SelectQuery<MyBO> query = new SelectQuery<MyBO>();
            //---------------Test Result -----------------------
            Assert.AreEqual(3, query.Fields.Count);
            Assert.AreEqual("MyBO.MyBoID", query.Fields[0]);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSource()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            //---------------Execute Test ----------------------
            SelectQuery<MyBO> query = new SelectQuery<MyBO>();
            //---------------Test Result -----------------------
            Assert.AreEqual("MyBO", query.Source);
            //---------------Tear Down -------------------------
        }
    }

    internal class SelectQuery<T> where T:class, IBusinessObject
    {
        private readonly Criteria _criteria;
        private readonly List<string> _fields = new List<string>( 5);
        private string _source;

        public SelectQuery()
        {
            InitFields();
        }

        private void InitFields()
        {
            ClassDef classDef = ClassDef.ClassDefs[typeof (T)];
            foreach (IPropDef propDef in classDef.PropDefcol)
            {
                _fields.Add(classDef.TableName + "." + propDef.DatabaseFieldName);
            }
            _source = classDef.TableName;
        }

        public SelectQuery(Criteria criteria)
        {
            _criteria = criteria;
        }


        public Criteria Criteria
        {
            get { return _criteria; }
        }

        public List<string> Fields
        {
            get { return _fields; }
        }

        /// <summary>
        /// The source of the data. In a database query this would be the first table listed in the FROM clause.
        /// </summary>
        public string Source
        {
            get { return _source; }
        }
    }
}
