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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectLoader
{
    [TestFixture]
    public class TestQueryBuilder
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            new Address();
        }

        //TODO: make the fields and order fields case insensitive
        [Test]
        public void TestBuiltFields()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            IClassDef classdef = ClassDef.ClassDefs[typeof(MyBO)];
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
            IClassDef classdef = ClassDef.ClassDefs[typeof(MyBO)];
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, "bob");
            Criteria preparedCriteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, "bob");
            QueryBuilder.PrepareCriteria(classdef, preparedCriteria);
            //---------------Execute Test ----------------------
            ISelectQuery query = QueryBuilder.CreateSelectQuery(classdef, criteria);
            //---------------Test Result -----------------------
            Assert.AreSame(criteria, query.Criteria);
            Assert.AreEqual(preparedCriteria.ToString(), query.Criteria.ToString());
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestBuiltSource()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            IClassDef classdef = ClassDef.ClassDefs[typeof(MyBO)];
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
            IClassDef classdef = ClassDef.ClassDefs[typeof(MyBO)];
            //---------------Execute Test ----------------------
            ISelectQuery query = QueryBuilder.CreateSelectQuery(classdef);

            //---------------Test Result -----------------------
            Assert.AreSame(classdef, query.ClassDef);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateOrderCriteria()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDefWithDifferentTableAndFieldNames();
            IClassDef classdef = ClassDef.ClassDefs[typeof(MyBO)];
            //---------------Execute Test ----------------------
            IOrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(classdef, "TestProp");
            //---------------Test Result -----------------------
            IOrderCriteriaField orderCriteriaField = orderCriteria.Fields[0];
            Assert.AreEqual(classdef.ClassName, orderCriteriaField.Source.Name);
            Assert.AreEqual(classdef.GetTableName(), orderCriteriaField.Source.EntityName);
            Assert.AreEqual(classdef.GetPropDef("TestProp").DatabaseFieldName, orderCriteriaField.FieldName);
            Assert.AreEqual(0, orderCriteriaField.Source.Joins.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateOrderCriteria_InvalidProperty()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDefWithDifferentTableAndFieldNames();
            const string propName = "NonExistantTestProp";
            IClassDef classdef = ClassDef.ClassDefs[typeof(MyBO)];
            //---------------Execute Test ----------------------
            Exception exception = null;
            try
            {
               
                QueryBuilder.CreateOrderCriteria(classdef, propName);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(InvalidOrderCriteriaException), exception);
            if (exception != null)
                StringAssert.Contains(String.Format(
                                          "The orderByString '{0}' is not valid for the classDef '{1}", propName, classdef.ClassNameFull), exception.Message);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateOrderCriteria_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            IClassDef myRelatedBoClassDef = MyRelatedBo.LoadClassDefWithDifferentTableAndFieldNames();
            IClassDef myBoClassdef = MyBO.LoadClassDefWithRelationship(); 

            //---------------Execute Test ----------------------
            IOrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(myBoClassdef, "MyRelationship.MyRelatedTestProp");
            //---------------Test Result -----------------------
            IOrderCriteriaField orderCriteriaField = orderCriteria.Fields[0];
            Assert.AreEqual(myRelatedBoClassDef.GetPropDef("MyRelatedTestProp").DatabaseFieldName, orderCriteriaField.FieldName);
            Assert.AreEqual(myBoClassdef.ClassName, orderCriteriaField.Source.Name);
            Assert.AreEqual(myBoClassdef.GetTableName(), orderCriteriaField.Source.EntityName);
            Assert.AreEqual(1, orderCriteriaField.Source.Joins.Count);
            Source.Join relJoin = orderCriteriaField.Source.Joins[0];
            Assert.AreEqual("MyRelationship", relJoin.ToSource.Name);
            Assert.AreEqual(myRelatedBoClassDef.GetTableName(), relJoin.ToSource.EntityName);
            Assert.AreEqual(1, relJoin.JoinFields.Count);
            Assert.AreEqual("RelatedID", relJoin.JoinFields[0].FromField.PropertyName);
            Assert.AreEqual("MyRelatedBoID", relJoin.JoinFields[0].ToField.PropertyName);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCreateOrderCriteria_ThroughRelationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            const string orderByString = "Car.Owner.Surname";

            //---------------Execute Test ----------------------
            IOrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(engineClassDef, orderByString);
            //---------------Test Result -----------------------
            IOrderCriteriaField orderCriteriaField = orderCriteria.Fields[0];
            Assert.AreEqual("Surname", orderCriteriaField.PropertyName);
            Assert.AreEqual("Surname_field", orderCriteriaField.FieldName);
            Assert.AreEqual(1, orderCriteriaField.Source.Joins.Count);
            Assert.AreEqual("Engine.Car.Owner", orderCriteriaField.Source.ToString());
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestPrepareCriteria()
        {
            //---------------Set up test pack-------------------
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            Criteria criteria = new Criteria("EngineNo", Criteria.ComparisonOp.Equals, 100);

            //---------------Execute Test ----------------------
            QueryBuilder.PrepareCriteria(engineClassDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual("EngineNo", criteria.Field.PropertyName);
            Assert.AreEqual("ENGINE_NO", criteria.Field.FieldName);
            Assert.AreEqual("Engine", criteria.Field.Source.Name);
            Assert.AreEqual("Table_Engine", criteria.Field.Source.EntityName);
            Assert.AreEqual(0, criteria.Field.Source.Joins.Count);
            Assert.AreEqual("Engine.EngineNo = '100'", criteria.ToString());
        }

        [Test]
        public void TestPrepareCriteria_ConvertsValue_IntToString()
        {
            //---------------Set up test pack-------------------
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            Criteria criteria = new Criteria("EngineNo", Criteria.ComparisonOp.Equals, 100);

            //---------------Execute Test ----------------------
            QueryBuilder.PrepareCriteria(engineClassDef, criteria);

            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(String), criteria.FieldValue);
        }

        [Test]
        public void TestPrepareCriteria_ConvertsValue_StringToDateTime()
        {
            //---------------Set up test pack-------------------
            IClassDef myBoClassDef = MyBO.LoadClassDefWithDateTime();
            const string dateTimeString = "01 Jan 2000 01:30:45";
            DateTime dateTime = DateTime.Parse(dateTimeString);
            Criteria criteria = new Criteria("TestDateTime", Criteria.ComparisonOp.Equals, dateTimeString);

            //---------------Execute Test ----------------------
            QueryBuilder.PrepareCriteria(myBoClassDef, criteria);

            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(DateTime), criteria.FieldValue);
            Assert.AreEqual(dateTime, criteria.FieldValue);
        }

        [Test]
        public void TestPrepareCriteria_ConvertsValue_StringTodayToDateTimeToday()
        {
            //---------------Set up test pack-------------------
            IClassDef myBoClassDef = MyBO.LoadClassDefWithDateTime();
            const string dateTimeString = "Today";
            DateTimeToday dateTimeToday = new DateTimeToday();
            Criteria criteria = new Criteria("TestDateTime", Criteria.ComparisonOp.Equals, dateTimeString);

            //---------------Execute Test ----------------------
            QueryBuilder.PrepareCriteria(myBoClassDef, criteria);

            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(DateTimeToday), criteria.FieldValue);
            Assert.AreEqual(dateTimeToday, criteria.FieldValue);
        }

        [Test]
        public void TestPrepareCriteria_ConvertsValue_StringNowToDateTimeToday()
        {
            //---------------Set up test pack-------------------
            IClassDef myBoClassDef = MyBO.LoadClassDefWithDateTime();
            const string dateTimeString = "Now";
            Criteria criteria = new Criteria("TestDateTime", Criteria.ComparisonOp.Equals, dateTimeString);

            //---------------Execute Test ----------------------
            QueryBuilder.PrepareCriteria(myBoClassDef, criteria);

            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(DateTimeNow), criteria.FieldValue);
            DateTime dateTimeFieldValue = Convert.ToDateTime(criteria.FieldValue.ToString());
            Assert.Greater(dateTimeFieldValue, DateTimeToday.Value);
            Assert.Less(dateTimeFieldValue, DateTimeToday.Value.AddDays(1));
        }
        [Test]
        public void TestPrepareCriteria_ConvertsValue_StringToGuid()
        {
            //---------------Set up test pack-------------------
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            string carid = Guid.NewGuid().ToString("B");
            Criteria criteria = new Criteria("CarID", Criteria.ComparisonOp.Equals, carid);

            //---------------Execute Test ----------------------
            QueryBuilder.PrepareCriteria(engineClassDef, criteria);

            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(Guid), criteria.FieldValue);
            Assert.AreEqual(carid, new Guid(criteria.FieldValue.ToString()).ToString("B"));
        }


        [Test]
        public void TestPrepareCriteria_Null()
        {
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            const Criteria nullCriteria = null;
            //-------------Execute test ---------------------
            QueryBuilder.PrepareCriteria(engineClassDef, nullCriteria);
            //-------------Test Result ----------------------
            Assert.IsNull(nullCriteria);
        }

        [Test]
        public void TestPrepareCriteria_Composite()
        {
            //---------------Set up test pack-------------------
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            Criteria criteria1 = new Criteria("EngineNo", Criteria.ComparisonOp.Equals, 100);
            Guid carId = Guid.NewGuid();
            Criteria criteria2 = new Criteria("CarID", Criteria.ComparisonOp.Equals, carId);
            Criteria criteria = new Criteria(criteria1, Criteria.LogicalOp.And, criteria2);

            //---------------Execute Test ----------------------
            QueryBuilder.PrepareCriteria(engineClassDef, criteria);

            //---------------Test Result -----------------------
            Assert.AreEqual("EngineNo", criteria1.Field.PropertyName);
            Assert.AreEqual("ENGINE_NO", criteria1.Field.FieldName);
            Assert.AreEqual("Engine", criteria1.Field.Source.Name);
            Assert.AreEqual("Table_Engine", criteria1.Field.Source.EntityName);
            Assert.AreEqual(0, criteria1.Field.Source.Joins.Count);

            Assert.AreEqual("CarID", criteria2.Field.PropertyName);
            Assert.AreEqual("CAR_ID", criteria2.Field.FieldName);
            Assert.AreEqual("Engine", criteria2.Field.Source.Name);
            Assert.AreEqual("Table_Engine", criteria2.Field.Source.EntityName);
            Assert.AreEqual(0, criteria2.Field.Source.Joins.Count);

            Assert.AreEqual("(Engine.EngineNo = '100') AND (Engine.CarID = '" + carId.ToString("B") + "')", criteria.ToString());
        }

        [Test]
        public void TestPrepareCriteria_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            const string carRegNoValue = "1234";
            Criteria criteria = new Criteria("Car.CarRegNo", Criteria.ComparisonOp.Equals, carRegNoValue);
            //-------------Execute test ---------------------
            QueryBuilder.PrepareCriteria(engineClassDef, criteria);
            //-------------Test Result ----------------------
            Assert.IsFalse(criteria.IsComposite());
            Assert.AreEqual(Criteria.ComparisonOp.Equals, criteria.ComparisonOperator);
            Assert.AreEqual(carRegNoValue, criteria.FieldValue);
            Assert.AreEqual("CarRegNo", criteria.Field.PropertyName);
            Assert.AreEqual("CAR_REG_NO", criteria.Field.FieldName);
            Assert.AreEqual("Engine.Car", criteria.Field.Source.ToString());
        }

        [Test]
        public void TestPrepareCriteria_ThroughRelationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            string surname = TestUtil.GetRandomString();
            Criteria criteria = new Criteria("Car.Owner.Surname", Criteria.ComparisonOp.Equals, surname);
            //-------------Execute test ---------------------
            QueryBuilder.PrepareCriteria(engineClassDef, criteria);
            //-------------Test Result ----------------------
            Assert.IsFalse(criteria.IsComposite());
            Assert.AreEqual(Criteria.ComparisonOp.Equals, criteria.ComparisonOperator);
            Assert.AreEqual(surname, criteria.FieldValue);
            Assert.AreEqual("Surname", criteria.Field.PropertyName);
            Assert.AreEqual("Surname_field", criteria.Field.FieldName);
            Assert.AreEqual("Engine.Car.Owner", criteria.Field.Source.ToString());
        }

        [Test]
        public void TestPrepareCriteria_Twice()
        {
            //---------------Set up test pack-------------------
            Structure.Entity.LoadDefaultClassDef();
            IClassDef classDef = Test.Structure.Part.LoadClassDef_WithClassTableInheritance();
            string entityType = TestUtil.GetRandomString();
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            Criteria criteria = new Criteria("EntityType", Criteria.ComparisonOp.Equals, entityType);
            QueryBuilder.PrepareCriteria(classDef, criteria);
            selectQuery.Criteria = criteria;
            //---------------Assert preconditions---------------
            Assert.AreEqual("table_Entity", criteria.Field.Source.EntityName);            
            //---------------Execute Test ----------------------
            QueryBuilder.PrepareCriteria(classDef, criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual("table_Entity", criteria.Field.Source.EntityName);            
        }


        [Test]
        public void TestPrepareSource()
        {
            //---------------Set up test pack-------------------
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            Source source = null;

            //---------------Execute Test ----------------------
            QueryBuilder.PrepareSource(engineClassDef, ref source);

            //---------------Test Result -----------------------
            Assert.IsNotNull(source);
            Assert.AreEqual("Engine", source.Name);
            Assert.AreEqual("Table_Engine", source.EntityName);
        }

        [Test]
        public void TestPrepareSource_Inheritance_ClassTable()
        {
            //---------------Set up test pack-------------------
            IClassDef circleClassDef = Circle.GetClassDefWithClassTableInheritance();
            Source source = null;

            //---------------Execute Test ----------------------
            QueryBuilder.PrepareSource(circleClassDef, ref source);
            //---------------Test Result -----------------------
            Assert.IsNotNull(source);
            Assert.AreEqual("Circle", source.Name);
            Assert.AreEqual("circle_table", source.EntityName);
        }

        [Test]
        public void TestPrepareSource_ExistingSource()
        {
            //---------------Set up test pack-------------------
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            Source originalSource = new Source("Engine", "Table_Engine");
            Source source = originalSource;

            //---------------Execute Test ----------------------
            QueryBuilder.PrepareSource(engineClassDef, ref source);

            //---------------Test Result -----------------------
            Assert.IsNotNull(source);
            Assert.AreSame(originalSource, source);
            Assert.AreEqual("Engine", source.Name);
            Assert.AreEqual("Table_Engine", source.EntityName);
        }

        [Test]
        public void TestPrepareSource_ExistingSource_CompletesEntityName()
        {
            //---------------Set up test pack-------------------
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            Source originalSource = new Source("Engine", null);
            Source source = originalSource;

            //---------------Execute Test ----------------------
            QueryBuilder.PrepareSource(engineClassDef, ref source);

            //---------------Test Result -----------------------
            Assert.IsNotNull(source);
            Assert.AreSame(originalSource, source);
            Assert.AreEqual("Engine", source.Name);
            Assert.AreEqual("Table_Engine", source.EntityName);
        }

        [Test]
        public void TestPrepareSource_ExistingSource_Relationship_DoesntExist()
        {
            //---------------Set up test pack-------------------
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            Source originalSource = new Source("NotExistingRelationship", null);
            Source source = originalSource;

            //---------------Execute Test ----------------------
            Exception exception = null;
            try
            {
                QueryBuilder.PrepareSource(engineClassDef, ref source);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception);
            Assert.IsInstanceOf(typeof(RelationshipNotFoundException), exception);
            Assert.AreEqual("'Engine' does not have a relationship called 'NotExistingRelationship'.", exception.Message);
        }

        [Test]
        public void TestPrepareSource_ExistingSource_RelationshipName()
        {
            //---------------Set up test pack-------------------
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            Source originalSource = new Source("Car", null);
            Source source = originalSource;

            //---------------Execute Test ----------------------
            QueryBuilder.PrepareSource(engineClassDef, ref source);

            //---------------Test Result -----------------------
            Assert.IsNotNull(source);
            Assert.AreNotSame(originalSource, source);
            Assert.AreEqual("Engine", source.Name);
            Assert.AreEqual("Table_Engine", source.EntityName);
            Assert.AreEqual(1, source.Joins.Count);
            Source.Join join = source.Joins[0];
            Source childSource = join.ToSource;
            Assert.IsNotNull(childSource);
            Assert.AreSame(originalSource, childSource);
            Assert.AreEqual("Car", childSource.Name);
            Assert.AreEqual("car_table", childSource.EntityName);
            Assert.AreEqual(1, join.JoinFields.Count);
            Source.Join.JoinField field = join.JoinFields[0];
            QueryField fromField = field.FromField;
            Assert.AreSame(source, fromField.Source);
            Assert.AreEqual("CarID", fromField.PropertyName);
            Assert.AreEqual("CAR_ID", fromField.FieldName);
            QueryField toField = field.ToField;
            Assert.AreSame(childSource, toField.Source);
            Assert.AreEqual("CarID", toField.PropertyName);
            Assert.AreEqual("CAR_ID", toField.FieldName);
        }

        [Test]
        public void TestPrepareSource_ExistingSource_Relationship_TwoLevels()
        {
            //---------------Set up test pack-------------------
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            Source carSource = Source.FromString("Car.Owner");
            Source source = carSource;

            //---------------Execute Test ----------------------
            QueryBuilder.PrepareSource(engineClassDef, ref source);

            //---------------Test Result -----------------------
            Assert.IsNotNull(source);
            Assert.AreNotSame(carSource, source);
            Assert.AreEqual("Engine", source.Name);
            Assert.AreEqual("Table_Engine", source.EntityName);
            Assert.AreEqual(1, source.Joins.Count);
            Source.Join join = source.Joins[0];
            Source childSource = join.ToSource;
            Assert.IsNotNull(childSource);
            Assert.AreSame(carSource, childSource);
            Assert.AreEqual("Car", childSource.Name);
            Assert.AreEqual("car_table", childSource.EntityName);
            Assert.AreEqual(1, join.JoinFields.Count);
            Source.Join.JoinField joinField = join.JoinFields[0];
            QueryField fromField = joinField.FromField;
            Assert.AreSame(source, fromField.Source);
            Assert.AreEqual("CarID", fromField.PropertyName);
            Assert.AreEqual("CAR_ID", fromField.FieldName);
            QueryField toField = joinField.ToField;
            Assert.AreSame(childSource, toField.Source);
            Assert.AreEqual("CarID", toField.PropertyName);
            Assert.AreEqual("CAR_ID", toField.FieldName);

            Assert.AreEqual(1, childSource.Joins.Count);
            Source.Join childJoin = childSource.Joins[0];
            Source grandChildSource = childJoin.ToSource;
            Assert.IsNotNull(grandChildSource);
            Assert.AreSame(carSource.ChildSource, grandChildSource);
            Assert.AreEqual("Owner", grandChildSource.Name);
            Assert.AreEqual("contact_person", grandChildSource.EntityName);
            Assert.AreEqual(1, childJoin.JoinFields.Count);
            Source.Join.JoinField childJoinField = childJoin.JoinFields[0];
            QueryField childFromField = childJoinField.FromField;
            Assert.AreSame(carSource, childFromField.Source);
            Assert.AreEqual("OwnerId", childFromField.PropertyName);
            Assert.AreEqual("OWNER_ID", childFromField.FieldName);
            QueryField childToField = childJoinField.ToField;
            Assert.AreSame(grandChildSource, childToField.Source);
            Assert.AreEqual("ContactPersonID", childToField.PropertyName);
            Assert.AreEqual("ContactPersonID", childToField.FieldName);
        }

        //[Test]
        //public void TestCreateCriteria()
        //{
        //    //-------------Setup Test Pack ------------------
        //    ClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
        //    string propertyName = "EngineNo";
        //    Criteria criteria = QueryBuilder.CreateCriteria(engineClassDef, )
        //    QueryBuilder.CreateOrderCriteria()
        //    //-------------Test Pre-conditions --------------

        [Test]
        public void TestPrepareDiscriminatorCriteria()
        {
            //---------------Set up test pack-------------------
            IClassDef circleClassDef = CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            SelectQuery selectQuery = (SelectQuery)QueryBuilder.CreateSelectQuery(circleClassDef);
            Criteria discCriteria = selectQuery.DiscriminatorCriteria;
            //---------------Execute Test ----------------------
            QueryBuilder.PrepareDiscriminatorCriteria(circleClassDef, discCriteria);
            //---------------Test Result -----------------------
            Assert.AreEqual("ShapeType_field", discCriteria.Field.PropertyName);
            Assert.AreEqual("ShapeType_field", discCriteria.Field.FieldName);
            Assert.AreEqual("Shape", discCriteria.Field.Source.Name);
            Assert.AreEqual("Shape_table", discCriteria.Field.Source.EntityName);
        }

        [Test]
        public void TestSetOrderCriteria_AddsJoinToSource()
        {
            //---------------Set up test pack-------------------
            MyRelatedBo.LoadClassDefWithDifferentTableAndFieldNames();
            IClassDef myBoClassdef = MyBO.LoadClassDefWithRelationship_DifferentTableAndFieldNames();

            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(myBoClassdef);
            IOrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(myBoClassdef, "MyRelationship.MyRelatedTestProp");

            //---------------Execute Test ----------------------
            selectQuery.OrderCriteria = orderCriteria;

            //---------------Test Result -----------------------
            Assert.AreEqual(1, selectQuery.Source.Joins.Count);
            Source.Join join = selectQuery.Source.Joins[0];
            Assert.AreEqual(selectQuery.Source, join.FromSource);
            Assert.AreEqual("MyRelationship", join.ToSource.Name);
            Assert.AreEqual(1, join.JoinFields.Count);
            Assert.AreEqual("RelatedID" ,join.JoinFields[0].FromField.PropertyName);
            Assert.AreEqual("related_id", join.JoinFields[0].FromField.FieldName);
            Assert.AreEqual("MyRelatedBoID", join.JoinFields[0].ToField.PropertyName);
            Assert.AreEqual("My_Related_Bo_ID", join.JoinFields[0].ToField.FieldName);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetOrderCriteria_AddsJoinToSource_OnlyOnce()
        {
            //---------------Set up test pack-------------------
            MyRelatedBo.LoadClassDefWithDifferentTableAndFieldNames();
            IClassDef myBoClassdef = MyBO.LoadClassDefWithRelationship();

            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(myBoClassdef);
            IOrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(myBoClassdef, "MyRelationship.MyRelatedTestProp, MyRelationship.MyRelatedTestProp2");

            //---------------Execute Test ----------------------
            selectQuery.OrderCriteria = orderCriteria;

            //---------------Test Result -----------------------
            Assert.AreEqual(1, selectQuery.Source.Joins.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetOrderCriteria_AddsJoinToSource_TwoLevels()
        {
            //---------------Set up test pack-------------------
            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();

            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(engineClassDef);
            IOrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(engineClassDef, "Car.Owner.Surname");

            //---------------Execute Test ----------------------
            selectQuery.OrderCriteria = orderCriteria;

            //---------------Test Result -----------------------
            Assert.AreEqual(1, selectQuery.Source.Joins.Count);
            Source carSource = selectQuery.Source.Joins[0].ToSource;
            Assert.AreEqual("Car", carSource.Name);
            Assert.AreEqual(1, carSource.Joins.Count);
            Assert.AreSame(carSource, carSource.Joins[0].FromSource);
            Assert.AreEqual("Owner", carSource.Joins[0].ToSource.Name);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSingleTableInheritance_Fields()
        {
            //---------------Set up test pack-------------------
            IClassDef circleClassDef = CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(circleClassDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, selectQuery.Fields.Count);
            Assert.IsTrue(selectQuery.Fields.ContainsKey("ShapeID"));
            Assert.IsTrue(selectQuery.Fields.ContainsKey("ShapeName"));
            Assert.IsTrue(selectQuery.Fields.ContainsKey("Radius"));
            Assert.IsTrue(selectQuery.Fields.ContainsKey("ShapeType_field"));
            Assert.AreEqual("Shape_table", selectQuery.Source.EntityName);
        }

        [Test]
        public void TestSingleTableInheritance_BaseHasDiscriminator()
        {
            //---------------Set up test pack-------------------
            ClassDef circleClassDef = (ClassDef) CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            ClassDef shapeClassDef = circleClassDef.SuperClassClassDef;
            Criteria expectedShapeCriteria = new Criteria("ShapeType_field", Criteria.ComparisonOp.Is, "null");
            Criteria expectedCircleCriteria = new Criteria("ShapeType_field", Criteria.ComparisonOp.Equals, "CircleNoPrimaryKey");
            Criteria expected = new Criteria(expectedShapeCriteria, Criteria.LogicalOp.Or, expectedCircleCriteria);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SelectQuery selectQuery = (SelectQuery) QueryBuilder.CreateSelectQuery(shapeClassDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, selectQuery.Fields.Count);
            Assert.IsTrue(selectQuery.Fields.ContainsKey("ShapeID"));
            Assert.IsTrue(selectQuery.Fields.ContainsKey("ShapeName"));
            Assert.IsTrue(selectQuery.Fields.ContainsKey("ShapeType_field"));

            Assert.AreEqual(expected, selectQuery.DiscriminatorCriteria);
            Assert.AreEqual("Shape_table", selectQuery.Source.EntityName);
        }

        [Test]
        public void TestSingleTableInheritance_SubTypeHasDiscriminator()
        {
            //---------------Set up test pack-------------------
            IClassDef circleClassDef = CircleNoPrimaryKey.GetClassDefWithSingleInheritance();
            Criteria expected = new Criteria("ShapeType_field", Criteria.ComparisonOp.Equals, "CircleNoPrimaryKey");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SelectQuery selectQuery = (SelectQuery) QueryBuilder.CreateSelectQuery(circleClassDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, selectQuery.DiscriminatorCriteria);
            Criteria discCriteria = selectQuery.DiscriminatorCriteria;
            Assert.AreEqual("ShapeType_field", discCriteria.Field.PropertyName);
            Assert.AreEqual("ShapeType_field", discCriteria.Field.FieldName);
            Assert.AreEqual("Shape", discCriteria.Field.Source.Name);
            Assert.AreEqual("Shape_table", discCriteria.Field.Source.EntityName);
        }

        [Test]
        public void TestSingleTableInheritance_TwoLevelsDeep()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef filledCircleClassDef = (ClassDef) FilledCircleNoPrimaryKey.GetClassDefWithSingleInheritanceHierarchy();
            ClassDef shapeClassDef = filledCircleClassDef.SuperClassClassDef.SuperClassClassDef;
            Criteria expectedShapeCriteria = new Criteria("ShapeType_field", Criteria.ComparisonOp.Is, "null");
            Criteria expectedCircleCriteria = new Criteria("ShapeType_field", Criteria.ComparisonOp.Equals, "CircleNoPrimaryKey");
            Criteria expectedFilledCircleCriteria = new Criteria("ShapeType_field", Criteria.ComparisonOp.Equals, "FilledCircleNoPrimaryKey");
            Criteria shapeOrCircleCriteria = new Criteria(expectedShapeCriteria, Criteria.LogicalOp.Or, expectedCircleCriteria);
            Criteria expected = new Criteria(shapeOrCircleCriteria, Criteria.LogicalOp.Or, expectedFilledCircleCriteria);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SelectQuery selectQuery = (SelectQuery)QueryBuilder.CreateSelectQuery(shapeClassDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, selectQuery.DiscriminatorCriteria);
        }


    }
}