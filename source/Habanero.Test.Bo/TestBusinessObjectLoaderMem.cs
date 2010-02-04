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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBusinessObjectLoaderMem
    {
        [Test]
        public void TestDataStoreConstructor()
        {
            //---------------Set up test pack-------------------

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, dataStore.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDataStoreAdd()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            dataStore.Add(new ContactPerson());
            //---------------Test Result -----------------------
            Assert.AreEqual(1, dataStore.Count);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestGetBusinessObjectWithPrimaryKey()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();

            dataStore.Add(cp);
            BusinessObjectLoaderInMemory loader = new BusinessObjectLoaderInMemory(dataStore);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = loader.GetBusinessObject<ContactPersonTestBO>(cp.PrimaryKey);
            //---------------Test Result -----------------------
            Assert.AreSame(cp, loadedCP);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetBusinessObjectWhenNotExists()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectLoaderInMemory loader = new BusinessObjectLoaderInMemory(dataStore);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                loader.GetBusinessObject<ContactPersonTestBO>(new ContactPersonTestBO().PrimaryKey);
            //---------------Test Result -----------------------
            Assert.IsNull(loadedCP);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestQueryObject_IsMatch_OneProp_Equals_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            QueryObject queryObject = new QueryObject("Surname", QueryOperator.Equals, Guid.NewGuid().ToString("N"));
            bool isMatch = queryObject.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestQueryObject_IsMatch_OneProp_Equals()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            QueryObject queryObject = new QueryObject("Surname", QueryOperator.Equals, cp.Surname);
            bool isMatch = queryObject.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestQueryObject_IsMatch_OneProp_GreaterThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            QueryObject queryObject = new QueryObject("DateOfBirth", QueryOperator.GreaterThan, DateTime.Now.AddDays(-1));
            bool isMatch = queryObject.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestQueryObject_IsMatch_OneProp_GreaterThan_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            QueryObject queryObject = new QueryObject("DateOfBirth", QueryOperator.GreaterThan, DateTime.Now.AddDays(1));
            bool isMatch = queryObject.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }


        //[Test]
        //public void TestGetBusinessObjectWithQueryObject()
        //{
        //    //---------------Set up test pack-------------------
        //    ClassDef.ClassDefs.Clear();
        //    ContactPersonTestBO.LoadDefaultClassDef();
        //    ContactPersonTestBO cp = new ContactPersonTestBO();
        //    cp.Surname = Guid.NewGuid().ToString("N");
        //    DataStoreInMemory dataStore = new DataStoreInMemory();
        //    dataStore.Add(cp);
        //    BusinessObjectLoaderInMemory loader = new BusinessObjectLoaderInMemory(dataStore);

        //    QueryObject queryObject = new QueryObject("Surname", QueryOperator.Equals, cp.Surname);

        //    //---------------Execute Test ----------------------
        //    ContactPersonTestBO loadedCP = loader.GetBusinessObject<ContactPersonTestBO>(queryObject);

        //    //---------------Test Result -----------------------
        //    //TODO: assert are same
        //    Assert.AreEqual(cp.ID, loadedCP.ID);
        //    //---------------Tear Down -------------------------          
     
        //}
    }

    internal enum QueryOperator
    {
        Equals,
        GreaterThan
    }
    internal class QueryObject
    {
        private readonly string _propName;
        private readonly QueryOperator _queryOperator;
        private readonly object _value;

        public QueryObject(string propName, QueryOperator queryOperator, object value)
        {
            _propName = propName;
            _queryOperator = queryOperator;
            _value = value;
        }

        public bool IsMatch<T>(T businessObject) where T: BusinessObject
        {
            switch (_queryOperator)
            {
                case QueryOperator.Equals: return (businessObject.GetPropertyValue(_propName).Equals(_value));
                case QueryOperator.GreaterThan:
                    IComparable x = businessObject.GetPropertyValue(_propName) as IComparable;
                    IComparable y = _value as IComparable;
                    return x.CompareTo(y) > 0;
                    default: return false;
            }
            
        }
    }

    internal class BusinessObjectLoaderInMemory
    {
        private readonly DataStoreInMemory _dataStore;

        public BusinessObjectLoaderInMemory(DataStoreInMemory dataStore)
        {
            _dataStore = dataStore;
        }

        public T GetBusinessObject<T>(IPrimaryKey key) where T : class, IBusinessObject
        {
            if (_dataStore.AllObjects.ContainsKey(key))
                return (T) _dataStore.AllObjects[key];
            else
                return null;
        }
    }

    internal class DataStoreInMemory
    {
        private Dictionary<IPrimaryKey, IBusinessObject> _objects = new Dictionary<IPrimaryKey, IBusinessObject>();

        public int Count
        {
            get { return _objects.Count; }
        }

        public void Add(IBusinessObject businessObject)
        {
            _objects.Add(businessObject.PrimaryKey, businessObject);
        }

        public Dictionary<IPrimaryKey, IBusinessObject> AllObjects
        {
            get { return _objects; }
        }
    }
}