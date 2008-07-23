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
            ISelectQuery query = QueryBuilder.CreateSelectQuery(classdef);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, query.Fields.Count);
            Assert.AreEqual("MyBoID", query.Fields["MyBoID"].PropertyName);
            Assert.AreEqual("MyBoID", query.Fields["MyBoID"].FieldName);
            Assert.AreEqual(new Source("MyBO"), query.Fields["MyBoID"].Source);
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
            ISelectQuery query = QueryBuilder.CreateSelectQuery(classdef, criteria);
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
            ISelectQuery query = QueryBuilder.CreateSelectQuery(classdef);
            //---------------Test Result -----------------------
            Assert.AreEqual("MyBO", query.Source.EntityName);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestHasClassDef()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            ClassDef classdef = ClassDef.ClassDefs[typeof(MyBO)];
            //---------------Execute Test ----------------------
            ISelectQuery query = QueryBuilder.CreateSelectQuery(classdef);

            //---------------Test Result -----------------------
            Assert.AreSame(classdef, query.ClassDef);
            //---------------Tear Down -------------------------
        }

        //TODO: busy removing this
        //[Test]
        //public void TestIncludeFieldsFromOrderCriteria()
        //{
        //    //---------------Set up test pack-------------------
        //    new ContactPerson();
        //    ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(new Address().ClassDef);
        //    selectQuery.OrderCriteria = OrderCriteria.FromString("ContactPerson.Surname");
        //    int startingFields = selectQuery.Fields.Count;
        //    //---------------Execute Test ----------------------
        //    QueryBuilder.IncludeFieldsFromOrderCriteria(selectQuery);
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(startingFields + 1, selectQuery.Fields.Count);
        //    Assert.IsTrue(selectQuery.Fields.ContainsKey("ContactPerson.Surname"));
        //    QueryField newField = selectQuery.Fields["ContactPerson.Surname"];
        //    Assert.AreEqual("contact_person", newField.SourceName);
        //    Assert.AreEqual("Surname", newField.PropertyName);
        //    Assert.AreEqual("Surname_field", newField.FieldName);
        //    //---------------Tear Down -------------------------
        //}

        //TODO: busy removing this
        //[Test]
        //public void TestIncludeFieldsFromOrderCriteria_AlreadyExisting()
        //{
        //    //---------------Set up test pack-------------------
        //    new ContactPerson();
        //    ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(new Address().ClassDef);
        //    selectQuery.OrderCriteria = OrderCriteria.FromString("AddressLine1");
        //    int startingFields = selectQuery.Fields.Count;
        //    //---------------Execute Test ----------------------
        //    QueryBuilder.IncludeFieldsFromOrderCriteria(selectQuery);
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(startingFields, selectQuery.Fields.Count);
        //    //---------------Tear Down -------------------------
        //}
        //TODO: busy removing this
        //[Test]
        //public void TestOrderCriteria_FieldNameIsMappedFromPropertyName()
        //{
        //    //---------------Set up test pack-------------------
            
        //    ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(new Car().ClassDef);
        //    string carregno = "CarRegNo";
        //    selectQuery.OrderCriteria = OrderCriteria.FromString(carregno);
        //    //---------------Execute Test ----------------------
        //    QueryBuilder.IncludeFieldsFromOrderCriteria(selectQuery);
        //    //---------------Test Result -----------------------
        //    Assert.IsTrue(selectQuery.Fields.ContainsKey(carregno));
        //    QueryField newField = selectQuery.Fields[carregno];
        //    Assert.AreEqual(carregno, newField.PropertyName);
        //    Assert.AreEqual("CAR_REG_NO", newField.FieldName);
        //    //---------------Tear Down -------------------------
        //}

        //TODO: busy removing this
        //[Test]
        //public void TestOrderCriteria_FieldNameIsMappedFromPropertyName_OverRelationship()
        //{
        //    //---------------Set up test pack-------------------
        //    new Car();
        //    ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(new Engine().ClassDef);
        //    string carregno = "CarRegNo";

        //    string car_carregno = "Car." + carregno;
        //    selectQuery.OrderCriteria = OrderCriteria.FromString(car_carregno);
        //    //---------------Execute Test ----------------------
        //    QueryBuilder.IncludeFieldsFromOrderCriteria(selectQuery);
        //    //---------------Test Result -----------------------
        //    Assert.IsTrue(selectQuery.Fields.ContainsKey(car_carregno));
        //    QueryField newField = selectQuery.Fields[car_carregno];
        //    Assert.AreEqual(carregno, newField.PropertyName);
        //    Assert.AreEqual("CAR_REG_NO", newField.FieldName);
        //    //---------------Tear Down -------------------------
        //}


        [Test]
        public void TestCreateOrderCriteria()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDefWithDifferentTableAndFieldNames();
            ClassDef classdef = ClassDef.ClassDefs[typeof(MyBO)];
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(classdef, "TestProp");
            //---------------Test Result -----------------------
            OrderCriteria.Field field = orderCriteria.Fields[0];
            Assert.AreEqual("", field.Source.Name);
            Assert.AreEqual(classdef.GetTableName(), field.Source.EntityName);
            Assert.AreEqual(classdef.GetPropDef("TestProp").DatabaseFieldName, field.FieldName);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateOrderCriteria_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            ClassDef myRelatedBoClassDef = MyRelatedBo.LoadClassDefWithDifferentTableAndFieldNames();
            ClassDef myBoClassdef = MyBO.LoadClassDefWithRelationship(); 

            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(myBoClassdef, "MyRelationship.MyRelatedTestProp");
            //---------------Test Result -----------------------
            OrderCriteria.Field field = orderCriteria.Fields[0];

            Assert.AreEqual("MyRelationship", field.Source.Name);
            Assert.AreEqual(myRelatedBoClassDef.GetTableName(), field.Source.EntityName);
            Assert.AreEqual(myRelatedBoClassDef.GetPropDef("MyRelatedTestProp").DatabaseFieldName, field.FieldName);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSingleTableInheritance_Fields()
        {
            //---------------Set up test pack-------------------
            ClassDef circleClassDef = CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(circleClassDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, selectQuery.Fields.Count);
            Assert.IsTrue(selectQuery.Fields.ContainsKey("ShapeID"));
            Assert.AreEqual("Shape_table", selectQuery.Source.EntityName);
        }

        [Test]
        public void TestSingleTableInheritance_BaseHasDiscriminator()
        {
            //---------------Set up test pack-------------------
            ClassDef circleClassDef = CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            ClassDef shapeClassDef = circleClassDef.SuperClassClassDef;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(shapeClassDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, selectQuery.Fields.Count);
            Assert.IsTrue(selectQuery.Fields.ContainsKey("ShapeID"));
            Assert.IsTrue(selectQuery.Fields.ContainsKey("ShapeName"));
            Assert.IsTrue(selectQuery.Fields.ContainsKey("ShapeType_field"));
            Assert.AreEqual("Shape_table", selectQuery.Source.EntityName);
        }


    }

}
