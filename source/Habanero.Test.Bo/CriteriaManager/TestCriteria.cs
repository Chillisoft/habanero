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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestCriteria
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            DataAccessorInMemory dataAccessor = new DataAccessorInMemory(dataStore);
            BORegistry.DataAccessor = dataAccessor;
        }

        [Test]
        public void TestLeafProperties()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            Criteria criteria = new Criteria("MyField", Criteria.ComparisonOp.Equals, "MyValue");

            //-------------Test Result ----------------------
            Assert.IsNotNull(criteria.Field);
            Assert.AreEqual("MyField", criteria.Field.PropertyName);
            Assert.IsNull(criteria.Field.Source);
            Assert.AreEqual("MyField", criteria.Field.FieldName);
            Assert.AreEqual("MyValue", criteria.FieldValue);
            Assert.AreEqual(Criteria.ComparisonOp.Equals, criteria.ComparisonOperator);
        }


        [Test]
        public void TestLeafProperties_AlternateConstructor()
        {
            //-------------Setup Test Pack ------------------
            QueryField field1 = new QueryField("MyField", "MyField", null);
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            Criteria criteria = new Criteria(field1, Criteria.ComparisonOp.Equals, "MyValue");

            //-------------Test Result ----------------------
            Assert.AreSame(field1, criteria.Field);
            Assert.AreEqual("MyValue", criteria.FieldValue);
            Assert.AreEqual(Criteria.ComparisonOp.Equals, criteria.ComparisonOperator);
        }

        [Test]
        public void TestEquals_Null()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue = DateTime.Now;
            const string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue);

            //---------------Execute Test ----------------------
            bool areEquals = criteria1.Equals(null);
            //---------------Test Result -----------------------
            Assert.IsFalse(areEquals);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEquals_WrongType()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue = DateTime.Now;
            const string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue);

            //---------------Execute Test ----------------------
            bool areEquals = criteria1.Equals(6);
            //---------------Test Result -----------------------
            Assert.IsFalse(areEquals);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEquals_Leaf_True()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue = DateTime.Now;
            const string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue);
            Criteria criteria2 = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue);
            //---------------Execute Test ----------------------
            bool areEquals = criteria1.Equals(criteria2);
            //---------------Test Result -----------------------
            Assert.IsTrue(areEquals);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEquals_Leaf_False()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue = DateTime.Now;
            const string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue);
            Criteria criteria2 = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue.AddDays(34));
            //---------------Execute Test ----------------------
            bool areEquals = criteria1.Equals(criteria2);
            //---------------Test Result -----------------------
            Assert.IsFalse(areEquals);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEquals_Leaf_FieldValueIsNull_True()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue = DateTime.Now;
            const string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.ComparisonOp.Equals, null);
            Criteria criteria2 = new Criteria(datetimePropName, Criteria.ComparisonOp.Equals, null);
            //---------------Execute Test ----------------------
            bool areEquals = criteria1.Equals(criteria2);
            //---------------Test Result -----------------------
            Assert.IsTrue(areEquals);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEquals_Leaf_FieldValueIsNull_False()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue = DateTime.Now;
            const string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.ComparisonOp.Equals, null);
            Criteria criteria2 = new Criteria(datetimePropName, Criteria.ComparisonOp.Equals, DateTime.Now);
            //---------------Execute Test ----------------------
            bool areEquals = criteria1.Equals(criteria2);
            //---------------Test Result -----------------------
            Assert.IsFalse(areEquals);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEquals_CompositeAndLeaf()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue1 = DateTime.Now;
            DateTime dateTimeValue2 = DateTime.Now.AddDays(20);
            const string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue1);
            Criteria criteria2 = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue2);
            Criteria composite1 = new Criteria(criteria1, Criteria.LogicalOp.And, criteria2);

            Criteria criteria3 = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue1);
            //---------------Execute Test ----------------------
            bool areEquals = composite1.Equals(criteria3);
            //---------------Test Result -----------------------
            Assert.IsFalse(areEquals);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEquals_Composite_True()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue1 = DateTime.Now;
            DateTime dateTimeValue2 = DateTime.Now.AddDays(20);
            const string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue1);
            Criteria criteria2 = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue2);
            Criteria composite1 = new Criteria(criteria1, Criteria.LogicalOp.And, criteria2);

            Criteria criteria3 = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue1);
            Criteria criteria4 = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue2);
            Criteria composite2 = new Criteria(criteria3, Criteria.LogicalOp.And, criteria4);
            //---------------Execute Test ----------------------
            bool areEquals = composite1.Equals(composite2);
            //---------------Test Result -----------------------
            Assert.IsTrue(areEquals);
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestToString_WithSource()
        {
            //-------------Setup Test Pack ------------------
            const string propName = "PropName";
            const string propValue = "PropValue";
            Criteria surnameCriteria = new Criteria(propName, Criteria.ComparisonOp.Equals, propValue);
            const string sourceName = "SourceName";
            surnameCriteria.Field.Source = new Source(sourceName);

            //-------------Execute test ---------------------
            string criteriaString = surnameCriteria.ToString();
            //-------------Test Result ----------------------
            Assert.AreEqual(string.Format("{0}.{1} = '{2}'", sourceName, propName, propValue), criteriaString);
        }

        [Test]
        public void TestQueryField_FromString()
        {
            //---------------Set up test pack-------------------
            string propertyName = "PropName";

            //---------------Execute Test ----------------------

            QueryField queryField = QueryField.FromString(propertyName);
            //---------------Test Result -----------------------
            Assert.AreEqual("PropName", queryField.PropertyName);
            Assert.IsNull(queryField.Source);

        }

        [Test]
        public void TestQueryField_FromString_WithSource()
        {
            //---------------Set up test pack-------------------
            string propertyName = "SourceName.PropName";

            //---------------Execute Test ----------------------

            QueryField queryField = QueryField.FromString(propertyName);
            //---------------Test Result -----------------------
            Assert.AreEqual("PropName", queryField.PropertyName);
            Assert.IsNotNull(queryField.Source);
            Assert.AreEqual("SourceName", queryField.Source.Name);

        }

        [Test]
        public void TestQueryField_FromString_WithSource_TwoLevels()
        {
            //---------------Set up test pack-------------------
            string propertyName = "BaseSource.ChildSource.PropName";

            //---------------Execute Test ----------------------

            QueryField queryField = QueryField.FromString(propertyName);
            //---------------Test Result -----------------------
            Assert.AreEqual("PropName", queryField.PropertyName);
            Source baseSource = queryField.Source;
            Assert.IsNotNull(baseSource);
            Assert.AreEqual("BaseSource", baseSource.Name);
            Assert.AreEqual(1, baseSource.Joins.Count);
            Source.Join join = baseSource.Joins[0];
            Assert.AreSame(baseSource, join.FromSource);
            Source childSource = join.ToSource;
            Assert.IsNotNull(childSource);
            Assert.AreEqual("ChildSource", childSource.Name);
            Assert.AreEqual(0, childSource.Joins.Count);
        }

        [Test]
        public void TestConstruct_WithSpecifiedSource()
        {
            //---------------Set up test pack-------------------
            const string propvalue = "PropValue";

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("SourceName.PropName", Criteria.ComparisonOp.Equals, propvalue);

            //---------------Test Result -----------------------
            Assert.IsFalse(criteria.IsComposite());
            Assert.AreEqual(Criteria.ComparisonOp.Equals, criteria.ComparisonOperator);
            Assert.AreEqual(propvalue, criteria.FieldValue);
            Assert.AreEqual("PropName", criteria.Field.PropertyName);
            Assert.AreEqual("PropName", criteria.Field.FieldName);
            Assert.AreEqual("SourceName", criteria.Field.Source.ToString());
        }

        [Test]
        public void TestMergeCriteria_BothNull()
        {
            //---------------Execute Test ----------------------
            Criteria newCriteria = Criteria.MergeCriteria(null, null);
            //---------------Test Result -----------------------
            Assert.IsNull(newCriteria);
            //---------------Tear Down -------------------------

        }

        [Test]
        public void TestMergeCriteria_FirstNull()
        {
            //-------------Setup Test Pack ------------------
            const string propName = "PropName";
            const string propValue = "PropValue";
            Criteria surnameCriteria = new Criteria(propName, Criteria.ComparisonOp.Equals, propValue);
            //---------------Execute Test ----------------------
            Criteria newCriteria = Criteria.MergeCriteria(surnameCriteria, null);
            //---------------Test Result -----------------------
            Assert.AreEqual(surnameCriteria, newCriteria);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestMergeCriteria_SecondNull()
        {
            //-------------Setup Test Pack ------------------
            const string propName = "PropName";
            const string propValue = "PropValue";
            Criteria surnameCriteria = new Criteria(propName, Criteria.ComparisonOp.Equals, propValue);
            //---------------Execute Test ----------------------
            Criteria newCriteria = Criteria.MergeCriteria(null, surnameCriteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(surnameCriteria, newCriteria);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestMergeCriteria()
        {
            //-------------Setup Test Pack ------------------
            const string propName = "PropName";
            const string propValue = "PropValue";
            Criteria criteria1 = new Criteria(propName, Criteria.ComparisonOp.Equals, propValue);
            const string propName2 = "PropName2";
            const string propValue2 = "PropValue2";
            Criteria criteria2 = new Criteria(propName2, Criteria.ComparisonOp.Equals, propValue2);
            Criteria expectedCriteria = new Criteria(criteria1, Criteria.LogicalOp.And, criteria2);
            //---------------Execute Test ----------------------

            Criteria newCriteria = Criteria.MergeCriteria(criteria1, criteria2);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, newCriteria);
            //---------------Tear Down -------------------------
        }

        #region Composite Properties

        [Test]
        public void TestCompositeProperties()
        {
            //-------------Setup Test Pack ------------------
            Criteria dobCriteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, "sfd");
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "dfsd");
            Criteria twoPropCriteria = new Criteria(dobCriteria, Criteria.LogicalOp.And, nameCriteria);
            //-------------Test Pre-conditions --------------
            
            //-------------Execute test ---------------------
            Criteria leftCriteria = twoPropCriteria.LeftCriteria;
            Criteria rightCriteria = twoPropCriteria.RightCriteria;
            Criteria.LogicalOp logicalOp = twoPropCriteria.LogicalOperator;
            //-------------Test Result ----------------------

            Assert.AreSame(dobCriteria, leftCriteria);
            Assert.AreSame(nameCriteria, rightCriteria);
            Assert.AreEqual(Criteria.LogicalOp.And, logicalOp);
        }

        [Test]
        public void TestIsComposite_False()
        {
            //---------------Set up test pack-------------------
            Criteria criteria = new Criteria("bob", Criteria.ComparisonOp.LessThan, "hello");
            //---------------Execute Test ----------------------
            bool isComposite = criteria.IsComposite();
            //---------------Test Result -----------------------
            Assert.IsFalse(isComposite, "A single criteria should not be composite");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestIsComposite_True()
        {
            //---------------Set up test pack-------------------
            Criteria dobCriteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, "sfd");
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "dfsd");
            Criteria twoPropCriteria = new Criteria(dobCriteria, Criteria.LogicalOp.And, nameCriteria);
            //---------------Execute Test ----------------------
            bool isComposite = twoPropCriteria.IsComposite();
            //---------------Test Result -----------------------
            Assert.IsTrue(isComposite, "A criteria made up of two others should be composite");
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestIsComposite_TrueForNotCriteria()
        {
            //---------------Set up test pack-------------------
            Criteria dateTimeCriteria = new Criteria("DateTime", Criteria.ComparisonOp.GreaterThan, DateTime.Now);

            //---------------Execute Test ----------------------
            Criteria notCriteria = new Criteria(Criteria.LogicalOp.Not, dateTimeCriteria);
            //---------------Test Result -----------------------
            Assert.IsTrue(notCriteria.IsComposite());
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestIsMatch_TwoProps_Or()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            DateTime dob = DateTime.Now;
            cp.DateOfBirth = dob;
            string surname = Guid.NewGuid().ToString("N");
            cp.Surname = surname;
            cp.Save();
            Criteria dobCriteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, dob.AddDays(2));
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, surname);

            //---------------Execute Test ----------------------
            Criteria twoPropCriteria = new Criteria(dobCriteria, Criteria.LogicalOp.Or, nameCriteria);
            bool isMatch = twoPropCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestIsMatch_TwoProps_And()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            DateTime dob = DateTime.Now;
            cp.DateOfBirth = dob;
            string surname = Guid.NewGuid().ToString("N");
            cp.Surname = surname;
            cp.Save();

            Criteria dobCriteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, dob);
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, surname);

            //---------------Execute Test ----------------------
            Criteria twoPropCriteria = new Criteria(dobCriteria, Criteria.LogicalOp.And, nameCriteria);
            bool isMatch = twoPropCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestIsMatch_TwoProps_Not()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithBoolean();
            MyBO bo = new MyBO();

            bo.TestBoolean = false;

            Criteria notCriteria = new Criteria(null, Criteria.LogicalOp.Not, new Criteria("TestBoolean", Criteria.ComparisonOp.Equals, true));

            //---------------Execute Test ----------------------
            bool isMatch = notCriteria.IsMatch(bo);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
        }

        [Test]
        public void TestIsMatch_UsesPersistedValue()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithBoolean();
            MyBO bo = new MyBO();
            bo.TestBoolean = false;
            bo.Save();

            bo.TestBoolean = true;
            Criteria criteria = new Criteria("TestBoolean", Criteria.ComparisonOp.Equals, false);

            //--------------- Execute Test ----------------------
            bool isMatch = criteria.IsMatch(bo);
            //--------------- Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since its persisted values match the criteria given.");
        }

        [Test]
        public void TestIsMatch_UsingCurrentValue()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithBoolean();
            MyBO bo = new MyBO();
            bo.TestBoolean = false;
            bo.Save();

            bo.TestBoolean = true;
            Criteria criteria = new Criteria("TestBoolean", Criteria.ComparisonOp.Equals, true);

            //--------------- Execute Test ----------------------
            bool isMatch = criteria.IsMatch(bo, false);
            //--------------- Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since its current values match the criteria given.");
        }

        [Test]
        public void TestToString_Guid()
        {
            //---------------Set up test pack-------------------
            Guid guidValue = Guid.NewGuid();
            Criteria guidCriteria = new Criteria("MyID", Criteria.ComparisonOp.Equals, guidValue);

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            string criteriaAsString = guidCriteria.ToString();
            //---------------Test Result -----------------------
            string expectedString = string.Format("MyID = '{0}'", guidValue.ToString("B"));
            StringAssert.AreEqualIgnoringCase(expectedString, criteriaAsString);
            //---------------Tear Down -------------------------          
        }



        [Test]
        public void TestFromIPrimaryKey_SingleProp()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            IPrimaryKey primaryKey = myBO.ID;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = Criteria.FromPrimaryKey(primaryKey);
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("MyBOID = '" + myBO.MyBoID.Value.ToString("B") + "'", criteria.ToString());
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFromIPrimaryKey_MultipleProp()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKeyNameSurname();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            string surnameValue = Guid.NewGuid().ToString();
            cp.Surname = surnameValue;
            string firstNameValue = Guid.NewGuid().ToString();
            cp.FirstName = firstNameValue;
            IPrimaryKey primaryKey = cp.ID;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = Criteria.FromPrimaryKey(primaryKey);
            //---------------Test Result -----------------------
            string expectedString = string.Format("(Surname = '{0}') AND (FirstName = '{1}')", surnameValue, firstNameValue);
            StringAssert.AreEqualIgnoringCase(expectedString, criteria.ToString());
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFromIRelationship()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteDoNothing();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            IRelationship relationship = cp.Relationships["Addresses"];
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = Criteria.FromRelationship(relationship);
            //---------------Test Result -----------------------
            string expectedString = string.Format("ContactPersonID = '{0}'", cp.ContactPersonID.ToString("B"));
            StringAssert.AreEqualIgnoringCase(expectedString, criteria.ToString());

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFromIRelationship_MultipleProps()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            RelKeyDef relKeyDef = new RelKeyDef();
            const string propValue1 = "bob1";
            PropDef boPropDef1 = new PropDef("Prop1", typeof(String), PropReadWriteRule.ReadWrite, propValue1);
            relKeyDef.Add(new RelPropDef(boPropDef1, "RelatedProp1"));
            const string propValue2 = "bob2";
            PropDef boPropDef2 = new PropDef("Prop2", typeof(String), PropReadWriteRule.ReadWrite, propValue2);
            relKeyDef.Add(new RelPropDef(boPropDef2, "RelatedProp2"));
            RelationshipDef reldef =
                new MultipleRelationshipDef("bob", "Habanero.Test", "MyBO", relKeyDef, false, "", DeleteParentAction.DoNothing);
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            BOPropCol col = new BOPropCol();
            col.Add(boPropDef1.CreateBOProp(true));
            col.Add(boPropDef2.CreateBOProp(true));
            IRelationship relationship = reldef.CreateRelationship(cp, col);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = Criteria.FromRelationship(relationship);
            //---------------Test Result -----------------------
            string expectedString = string.Format("(RelatedProp1 = '{0}') AND (RelatedProp2 = '{1}')", propValue1, propValue2);
            StringAssert.AreEqualIgnoringCase(expectedString, criteria.ToString());

            //---------------Tear Down -------------------------          
        }

        #endregion


        [Test]
        public void TestToString_And()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria surnameCriteria = new Criteria(surname, Criteria.ComparisonOp.Equals, surnameValue);
            DateTime dateTimeValue = DateTime.Now;
            const string datetimePropName = "DateTime";
            Criteria dateTimeCriteria = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue);

            Criteria andCriteria = new Criteria(surnameCriteria, Criteria.LogicalOp.And, dateTimeCriteria);
            //---------------Execute Test ----------------------
            string criteriaAsString = andCriteria.ToString();
            //---------------Test Result -----------------------
            string expectedString = string.Format("(Surname = '{0}') AND (DateTime > '{1}')", surnameValue, dateTimeValue.ToString(Criteria.DATE_FORMAT));
            StringAssert.AreEqualIgnoringCase(expectedString, criteriaAsString);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestToString_Not()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria surnameCriteria = new Criteria(surname, Criteria.ComparisonOp.Equals, surnameValue);
            DateTime dateTimeValue = DateTime.Now;
            const string datetimePropName = "DateTime";
            Criteria dateTimeCriteria = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue);
            Criteria notCriteria = new Criteria(Criteria.LogicalOp.Not, dateTimeCriteria);
            Criteria andCriteria = new Criteria(surnameCriteria, Criteria.LogicalOp.And, notCriteria);
            
            //---------------Execute Test ----------------------
            string criteriaAsString = andCriteria.ToString();
            //---------------Test Result -----------------------
            string expectedString = string.Format("(Surname = '{0}') AND (NOT (DateTime > '{1}'))", surnameValue, dateTimeValue.ToString(Criteria.DATE_FORMAT));
            StringAssert.AreEqualIgnoringCase(expectedString, criteriaAsString);
            //---------------Tear Down -------------------------
        }

        [Test, ExpectedException(typeof(ArgumentException), "And is not a valid Logical Operator for a Unary Criteria")]
        public void TestUnaryConstructorDoesntWorkForAnd()
        {
            //---------------Set up test pack-------------------
            Criteria dateTimeCriteria = new Criteria("DateTime", Criteria.ComparisonOp.GreaterThan, DateTime.Now);
            
            //---------------Execute Test ----------------------
            new Criteria(Criteria.LogicalOp.And, dateTimeCriteria);
            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------

        }
        [Test, ExpectedException(typeof(ArgumentException), "Or is not a valid Logical Operator for a Unary Criteria")]
        public void TestUnaryConstructorDoesntWorkForOr()
        {
            //---------------Set up test pack-------------------
            Criteria dateTimeCriteria = new Criteria("DateTime", Criteria.ComparisonOp.GreaterThan, DateTime.Now);

            //---------------Execute Test ----------------------
            new Criteria(Criteria.LogicalOp.Or, dateTimeCriteria);
            //---------------Test Result -----------------------

            //---------------Tear Down -------------------------

        }



        #region Test Comparison operators

        #region Equals

        [Test]
        public void TestIsMatch_OneProp_Equals_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, Guid.NewGuid().ToString("N"));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_OneProp_Equals()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, cp.Surname);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestIsMatch_NullValue_Equals_Fail()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "surname");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should be not a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_NullValue_Equals()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, null);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestToString_LeafCriteria_String_Equals()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.Equals, surnameValue);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname = '" + surnameValue + "'", criteriaAsString);
        }
 
        [Test]
        public void TestToString_LeafCriteria_String_Equals_Null()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.Equals, null);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname IS NULL", criteriaAsString);
        }
  
        [Test]
        public void TestToString_LeafCriteria_String_Is_Null()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.Is, null);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname IS NULL", criteriaAsString);
        }
 

       


        #endregion

        #region Greater Than

        [Test]
        public void TestIsMatch_OneProp_GreaterThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;
            cp.Surname = TestUtil.GetRandomString();
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, DateTime.Now.AddDays(-1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_OneProp_GreaterThan_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, DateTime.Now.AddDays(1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_DateTimeToday_GreaterThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeToday dateTimeToday = new DateTimeToday();
            cp.DateOfBirth = DateTime.Today.AddDays(1);
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, dateTimeToday);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_DateTimeToday_GreaterThan_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeToday dateTimeToday = new DateTimeToday();
            cp.DateOfBirth = DateTime.Today.AddDays(-1);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, dateTimeToday);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestIsMatch_DateTimeNow_GreaterThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeNow dateTimeToday = new DateTimeNow();
            cp.DateOfBirth = DateTime.Today.AddDays(1);
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, dateTimeToday);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_DateTimeNow_GreaterThan_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeNow dateTimeToday = new DateTimeNow();
            cp.DateOfBirth = DateTime.Today.AddDays(-1);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, dateTimeToday);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_NullValue_GreaterThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.GreaterThan, "bob");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should be not a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestToString_LeafCriteria_String_GreaterThan()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.GreaterThan, surnameValue);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname > '" + surnameValue + "'", criteriaAsString);
        }

        #endregion

        #region Less Than

        [Test]
        public void TestIsMatch_OneProp_LessThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;
            cp.Surname = TestUtil.GetRandomString();
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.LessThan, DateTime.Now.AddDays(1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_OneProp_LessThan_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.LessThan, DateTime.Now.AddDays(-1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_NullValue_LessThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.LessThan, "bob");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should be a match since matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void TestIsMatch_LessThan_Incomparable()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithImageProperty();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.SetPropertyValue("Image", new System.Drawing.Bitmap(10, 10));
            Criteria nameCriteria = new Criteria("Image", Criteria.ComparisonOp.LessThan, new System.Drawing.Bitmap(20, 20));
            cp.Surname = TestUtil.GetRandomString();
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            try
            {
                nameCriteria.IsMatch(cp);
                Assert.Fail("expected InvalidOperationException because you Bitmap does not implement IComparable");

                //---------------Test Result -----------------------
            }
            catch (InvalidOperationException ex)
            {
                StringAssert.Contains("does not implement IComparable and cannot be matched", ex.Message);
                throw;
            }
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestToString_LeafCriteria_String_LessThan()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.LessThan, surnameValue);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname < '" + surnameValue + "'", criteriaAsString);
        }

        #endregion

        #region LessThanEqual

        [Test]
        public void TestLessThanEqual()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            Criteria criteria = new Criteria("MyField", Criteria.ComparisonOp.LessThanEqual, "MyValue");

            //-------------Test Result ----------------------
            Assert.IsNotNull(criteria.Field);
            Assert.AreEqual("MyField", criteria.Field.PropertyName);
            Assert.IsNull(criteria.Field.Source);
            Assert.AreEqual("MyField", criteria.Field.FieldName);
            Assert.AreEqual("MyValue", criteria.FieldValue);
            Assert.AreEqual(Criteria.ComparisonOp.LessThanEqual, criteria.ComparisonOperator);
        }

        [Test]
        public void TestIsMatch_OneProp_LessThanEqual()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;
            cp.Surname = TestUtil.GetRandomString();
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.LessThanEqual, DateTime.Now.AddDays(1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_OneProp_LessThanEqual_ValuesEqual()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;
            cp.Surname = TestUtil.GetRandomString();
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.LessThanEqual, cp.DateOfBirth);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_OneProp_LessThanEqual_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.LessThanEqual, DateTime.Now.AddDays(-1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_NullValue_LessThanEqual()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.LessThanEqual, "bob");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should be a match since matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void TestIsMatch_LessThanEqual_Incomparable()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithImageProperty();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.SetPropertyValue("Image", new System.Drawing.Bitmap(10, 10));
            Criteria nameCriteria = new Criteria("Image", Criteria.ComparisonOp.LessThanEqual, new System.Drawing.Bitmap(20, 20));
            cp.Surname = TestUtil.GetRandomString();
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            try
            {
                nameCriteria.IsMatch(cp);
                Assert.Fail("expected InvalidOperationException because you Bitmap does not implement IComparable");

                //---------------Test Result -----------------------
            }
            catch (InvalidOperationException ex)
            {
                StringAssert.Contains("does not implement IComparable and cannot be matched", ex.Message);
                throw;
            }
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestToString_LeafCriteria_String_LessThanEqual()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.LessThanEqual, surnameValue);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname <= '" + surnameValue + "'", criteriaAsString);
        }

        #endregion

        #region Greater Than Equal

        [Test]
        public void TestGreaterThanEqual()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            Criteria criteria = new Criteria("MyField", Criteria.ComparisonOp.GreaterThanEqual, "MyValue");

            //-------------Test Result ----------------------
            Assert.IsNotNull(criteria.Field);
            Assert.AreEqual("MyField", criteria.Field.PropertyName);
            Assert.IsNull(criteria.Field.Source);
            Assert.AreEqual("MyField", criteria.Field.FieldName);
            Assert.AreEqual("MyValue", criteria.FieldValue);
            Assert.AreEqual(Criteria.ComparisonOp.GreaterThanEqual, criteria.ComparisonOperator);
        }

        [Test]
        public void TestIsMatch_OneProp_GreaterThanEqual()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;
            cp.Surname = TestUtil.GetRandomString();
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThanEqual, DateTime.Now.AddDays(-1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_OneProp_GreaterThanEqual_ValuesEqual()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;
            cp.Surname = TestUtil.GetRandomString();
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThanEqual, cp.DateOfBirth);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_OneProp_GreaterThanEqual_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThanEqual, DateTime.Now.AddDays(1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_NullValue_GreaterThanEqual()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.GreaterThanEqual, "bob");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should be a match since matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void TestIsMatch_GreaterThanEqual_Incomparable()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithImageProperty();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.SetPropertyValue("Image", new System.Drawing.Bitmap(10, 10));
            Criteria nameCriteria = new Criteria("Image", Criteria.ComparisonOp.GreaterThanEqual, new System.Drawing.Bitmap(20, 20));
            cp.Surname = TestUtil.GetRandomString();
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            try
            {
                nameCriteria.IsMatch(cp);
                Assert.Fail("expected InvalidOperationException because you Bitmap does not implement IComparable");

                //---------------Test Result -----------------------
            }
            catch (InvalidOperationException ex)
            {
                StringAssert.Contains("does not implement IComparable and cannot be matched", ex.Message);
                throw;
            }
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestToString_LeafCriteria_String_GreaterThanEqual()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.GreaterThanEqual, surnameValue);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname >= '" + surnameValue + "'", criteriaAsString);
        }

        
        #endregion


        #region NotEquals
        [Test]
        public void TestNotEquals()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            Criteria criteria = new Criteria("MyField", Criteria.ComparisonOp.NotEquals, "MyValue");

            //-------------Test Result ----------------------
            Assert.IsNotNull(criteria.Field);
            Assert.AreEqual("MyField", criteria.Field.PropertyName);
            Assert.IsNull(criteria.Field.Source);
            Assert.AreEqual("MyField", criteria.Field.FieldName);
            Assert.AreEqual("MyValue", criteria.FieldValue);
            Assert.AreEqual(Criteria.ComparisonOp.NotEquals, criteria.ComparisonOperator);
        }


        [Test]
        public void TestIsMatch_OneProp_NotEquals_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotEquals, Guid.NewGuid().ToString("N"));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the Not equals criteria given.");
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestIsMatch_OneProp_NotEquals()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotEquals, cp.Surname);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should be a not match since it does not match the not equals criteria given");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_NullValue_NotEquals_Fail()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.NotEquals, "surname");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should match since it matches the Not Equals criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_NullValue_NotEquals_Pass()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.NotEquals, null);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since does not match the not equals criteria given.");
        }
        [Test]
        public void TestIsMatch_NullValue_NotEquals_NonNullProperty()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "Surname";
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.NotEquals, null);
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should match since matches the not equals criteria given.");
        }

        [Test]
        public void TestToString_LeafCriteria_String_NotEquals()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.NotEquals, surnameValue);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname <> '" + surnameValue + "'", criteriaAsString);
        }

        #endregion

        #region Like
        [Test]
        public void TestLike()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            Criteria criteria = new Criteria("MyField", Criteria.ComparisonOp.Like, "%MyValue%");

            //-------------Test Result ----------------------
            Assert.IsNotNull(criteria.Field);
            Assert.AreEqual("MyField", criteria.Field.PropertyName);
            Assert.IsNull(criteria.Field.Source);
            Assert.AreEqual("MyField", criteria.Field.FieldName);
            Assert.AreEqual("%MyValue%", criteria.FieldValue);
            Assert.AreEqual(Criteria.ComparisonOp.Like, criteria.ComparisonOperator);
        }

        [Test]
        public void TestIsMatch_OneProp_Like()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "This is MyValue Surname";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "%MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_OneProp_Like_ValuesIdentical()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "MyValue";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "%MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_OneProp_Like_ValuesStartsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "MyValue is surname";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "%MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_OneProp_Like_ValuesEndsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "surname is MyValue";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "%MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_OneProp_Like_CriteriaStartsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "This is MyValue Surname";
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not start with MyValue.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_OneProp_Like_ValuesIdentical_CriteriaStartsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "MyValue";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since surname starts with MyValue.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_OneProp_Like_ValuesStartsWith_CriteriaStartsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "MyValue is surname";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestNotIsMatch_OneProp_Like_ValuesEndsWith_CriteriaStartsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "surname is MyValue";

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestIsMatch_OneProp_Like_CriteriaEndsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "This is MyValue Surname";
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "%MyValue");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not end with MyValue.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_OneProp_Like_ValuesIdentical_CriteriaEndsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "MyValue";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "%MyValue");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since surname ends with MyValue.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_OneProp_Like_ValuesStartsWith_CriteriaEndsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "MyValue is surname";

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "%MyValue");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since surname does not end with MyValue.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestNotIsMatch_OneProp_Like_ValuesEndsWith_CriteriaEndsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "surname is MyValue";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "%MyValue");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_OneProp_Like_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "surname does not contain searchstring";
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "%MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_NullValue_Like()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "bob%");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_NullPropertyValue_Like_NullMatchValue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.Like, null);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void TestIsMatch_Like_Incomparable()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithImageProperty();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.SetPropertyValue("Image", new System.Drawing.Bitmap(10, 10));
            Criteria nameCriteria = new Criteria("Image", Criteria.ComparisonOp.Like, new System.Drawing.Bitmap(20, 20));
            cp.Surname = TestUtil.GetRandomString();
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            try
            {
                nameCriteria.IsMatch(cp);
                Assert.Fail("expected InvalidOperationException because you Bitmap does not implement IComparable");

                //---------------Test Result -----------------------
            }
            catch (InvalidOperationException ex)
            {
                StringAssert.Contains("does not implement IComparable and cannot be matched", ex.Message);
                throw;
            }
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_ToString_LeafCriteria_String_Like()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N") + "%";
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.Like, surnameValue);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname Like '" + surnameValue + "'", criteriaAsString);
        }


        #endregion

        #region Not Like
        [Test]
        public void TestNotLike()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            Criteria criteria = new Criteria("MyField", Criteria.ComparisonOp.NotLike, "%MyValue%");

            //-------------Test Result ----------------------
            Assert.IsNotNull(criteria.Field);
            Assert.AreEqual("MyField", criteria.Field.PropertyName);
            Assert.IsNull(criteria.Field.Source);
            Assert.AreEqual("MyField", criteria.Field.FieldName);
            Assert.AreEqual("%MyValue%", criteria.FieldValue);
            Assert.AreEqual(Criteria.ComparisonOp.NotLike, criteria.ComparisonOperator);
        }

        [Test]
        public void TestIsMatch_OneProp_NotLike()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "This is MyValue Surname";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, "%MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_OneProp_NotLike_ValuesIdentical()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "MyValue";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, "%MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_OneProp_NotLike_ValuesStartsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "MyValue is surname";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, "%MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_OneProp_NotLike_ValuesEndsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "surname is MyValue";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, "%MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_OneProp_NotLike_CriteriaStartsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "This is MyValue Surname";
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, "MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it does not start with MyValue.");
        }
        [Test]
        public void TestIsMatch_OneProp_NotLike_ValuesIdentical_CriteriaStartsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "MyValue";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, "MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it matches the criteria given.");
        }
        [Test]
        public void TestIsMatch_OneProp_NotLike_ValuesStartsWith_CriteriaStartsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "MyValue is surname";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, "MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it matches the criteria given.");
        }

        [Test]
        public void TestNotIsMatch_OneProp_NotLike_ValuesEndsWith_CriteriaStartsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "surname is MyValue";

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, "MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
        }

        [Test]
        public void TestIsMatch_OneProp_NotLike_CriteriaEndsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "This is MyValue Surname";
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, "%MyValue");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it does not end with MyValue.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestIsMatch_OneProp_NotLike_ValuesIdentical_CriteriaEndsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "MyValue";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, "%MyValue");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it matches the criteria given.");
        }
        [Test]
        public void TestIsMatch_OneProp_NotLike_ValuesStartsWith_CriteriaEndsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "MyValue is surname";

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, "%MyValue");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since surname does not end with MyValue.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestNotIsMatch_OneProp_NotLike_ValuesEndsWith_CriteriaEndsWith()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "surname is MyValue";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, "%MyValue");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it matches the criteria given.");
        }

        [Test]
        public void TestIsMatch_OneProp_NotLike_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "surname does not contain searchstring";
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, "%MyValue%");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it does not contain the criteria given.");
        }

        [Test]
        public void TestIsMatch_NullValue_NotLike()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, "bob%");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since matches the criteria given.");
        }

        [Test]
        public void TestIsMatch_NullPropertyValue_NotLike_NullMatchValue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.NotLike, null);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it matches the criteria given.");
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void TestIsMatch_NotLike_Incomparable()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithImageProperty();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.SetPropertyValue("Image", new System.Drawing.Bitmap(10, 10));
            Criteria nameCriteria = new Criteria("Image", Criteria.ComparisonOp.NotLike, new System.Drawing.Bitmap(20, 20));
            cp.Surname = TestUtil.GetRandomString();
            cp.Save();
            
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            try
            {
                nameCriteria.IsMatch(cp);
                Assert.Fail("expected InvalidOperationException because you Bitmap does not implement IComparable");

                //---------------Test Result -----------------------
            }
            catch (InvalidOperationException ex)
            {
                StringAssert.Contains("does not implement IComparable and cannot be matched", ex.Message);
                throw;
            }
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_ToString_LeafCriteria_String_NotLike()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N") + "%";
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.NotLike, surnameValue);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname Not Like '" + surnameValue + "'", criteriaAsString);
        }


        #endregion

        #region Is Not
        [Test]
        public void TestIsNot()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            Criteria criteria = new Criteria("MyField", Criteria.ComparisonOp.IsNot, "NULL");

            //-------------Test Result ----------------------
            Assert.IsNotNull(criteria.Field);
            Assert.AreEqual("MyField", criteria.Field.PropertyName);
            Assert.IsNull(criteria.Field.Source);
            Assert.AreEqual("MyField", criteria.Field.FieldName);
            Assert.AreEqual("NULL", criteria.FieldValue);
            Assert.AreEqual(Criteria.ComparisonOp.IsNot, criteria.ComparisonOperator);
            Assert.IsTrue(criteria.CannotBeParametrised());
        }

        [Test]
        public void TestIsMatch_OneProp_ISNot()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.IsNot, "NULL");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not matches the criteria given.");
        }
        [Test]
        public void TestNotIsMatch_OneProp_ISNot()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "MyValue";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.IsNot, "NULL"); 
            bool isMatch = criteria.IsMatch(cp);

            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should  be a match since its surname is not null.");
        }
        [Test]
        public void Test_ToString_LeafCriteria_String_IsNot()
        {
            //---------------Set up test pack-------------------
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.IsNot, "Null");

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname IS NOT NULL", criteriaAsString);
        }
        #endregion

        #region Is
        [Test]
        public void TestIS()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            Criteria criteria = new Criteria("MyField", Criteria.ComparisonOp.Is, "NULL");

            //-------------Test Result ----------------------
            Assert.IsNotNull(criteria.Field);
            Assert.AreEqual("MyField", criteria.Field.PropertyName);
            Assert.IsNull(criteria.Field.Source);
            Assert.AreEqual("MyField", criteria.Field.FieldName);
            Assert.AreEqual("NULL", criteria.FieldValue);
            Assert.AreEqual(Criteria.ComparisonOp.Is, criteria.ComparisonOperator);
            Assert.IsTrue(criteria.CannotBeParametrised());
        }

        [Test]
        public void TestIsMatch_OneProp_IS()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Is, "NULL");
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestNotIsMatch_OneProp_IS()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "MyValue";
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Is, "NULL"); 
            bool isMatch = criteria.IsMatch(cp);

            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since its surname is not null.");
        }
        [Test]
        public void Test_ToString_LeafCriteria_String_Is()
        {
            //---------------Set up test pack-------------------
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.Is, "Null");

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname IS NULL", criteriaAsString);
        }
        #endregion


        [Test, Ignore("IN NOT IN not yet implemented")]
        public void TestOtherOperators()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.Fail("Todo");
            //Todo:  
            //                " NOT IN",
            //                " IN"
            //
            //            Test using objects and enums TestCriteria
            //test parsing from strings. TestCriteriaParser
            //col.Load(New Criteria("Surname" , Op.Equals, "Powell") TestbusinessObjectCollection
            //col.Load("Surname = Powell"); //Test what happens if we parse using an unsupported operator
            //test for value is null
            //---------------Test Result -----------------------

        }

        #endregion //Comparison Operators


        #region DateTime

        [Test]
        public void TestIsMatch_DateTimeToday_Equals()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeToday dateTimeToday = new DateTimeToday();
            cp.DateOfBirth = DateTime.Today;
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, dateTimeToday);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_DateTimeToday_Equals_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeToday dateTimeToday = new DateTimeToday();
            cp.DateOfBirth = DateTime.Today.AddDays(-1);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, dateTimeToday);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_DateTimeNow_Equals_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeNow dateTimeToday = new DateTimeNow();
            cp.DateOfBirth = DateTime.Today.AddDays(-1);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, dateTimeToday);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_DateTimeToday_LessThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeToday dateTimeToday = new DateTimeToday();
            cp.DateOfBirth = DateTime.Today.AddDays(-1);
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.LessThan, dateTimeToday);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_DateTimeToday_LessThan_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeToday dateTimeToday = new DateTimeToday();
            cp.DateOfBirth = DateTime.Today.AddDays(1);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.LessThan, dateTimeToday);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_DateTimeNow_LessThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeNow dateTimeToday = new DateTimeNow();
            cp.DateOfBirth = DateTime.Today.AddDays(-1);
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.LessThan, dateTimeToday);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_DateTimeNow_LessThan_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeNow dateTimeNow = new DateTimeNow();
            cp.DateOfBirth = DateTime.Today.AddDays(1);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.LessThan, dateTimeNow);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestToString_LeafCriteria_DateTime_GreaterThan()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue = DateTime.Now;
            const string datetimePropName = "DateTime";
            Criteria criteria = new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("DateTime > '" + dateTimeValue.ToString(Criteria.DATE_FORMAT) + "'", criteriaAsString);
        }

        #endregion

    }
}
