#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Habanero.Base;
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
        public void Equals_WhenValuesAreEnumerable_ShouldCompareEnumerables_True()
        {
            //---------------Set up test pack-------------------
            const string propName = "IntValue";
            Criteria criteria1 = new Criteria(propName, Criteria.ComparisonOp.In, new[] {1, 3, 5});
            Criteria criteria2 = new Criteria(propName, Criteria.ComparisonOp.In, new[] { 1, 3, 5 });
            //---------------Execute Test ----------------------
            bool areEquals = criteria1.Equals(criteria2);
            //---------------Test Result -----------------------
            Assert.IsTrue(areEquals);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void Equals_WhenValuesAreEnumerable_ShouldCompareEnumerables_False()
        {
            //---------------Set up test pack-------------------
            const string propName = "IntValue";
            Criteria criteria1 = new Criteria(propName, Criteria.ComparisonOp.In, new[] {1, 3, 5});
            Criteria criteria2 = new Criteria(propName, Criteria.ComparisonOp.In, new[] { 1, 2, 3, 5 });
            //---------------Execute Test ----------------------
            bool areEquals = criteria1.Equals(criteria2);
            //---------------Test Result -----------------------
            Assert.IsFalse(areEquals);
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
            const string propertyName = "PropName";

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
            const string propertyName = "SourceName.PropName";

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
            const string propertyName = "BaseSource.ChildSource.PropName";

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
        public void Test_IsMatch_GuidCriteriaAsString_UserPersistedValueFalse()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef_WOrganisationID();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO { Surname = Guid.NewGuid().ToString("N") };
            cp.OrganisationID = OrganisationTestBO.CreateSavedOrganisation().OrganisationID;
//            cp.Save();
            Criteria criteria = CriteriaParser.CreateCriteria("OrganisationID = " + cp.OrganisationID);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool isMatch = criteria.IsMatch(cp, false);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch);

        }

        [Test]
        public void Test_IsMatch_GuidCriteriaAsString_UserPersistedValueTrue()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef_WOrganisationID();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO { Surname = Guid.NewGuid().ToString("N") };
            cp.OrganisationID = OrganisationTestBO.CreateSavedOrganisation().OrganisationID;
            cp.Save();
            Criteria criteria = CriteriaParser.CreateCriteria("OrganisationID = " + cp.OrganisationID);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch);
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
            StringAssert.AreEqualIgnoringCase("MyBOID = '" + myBO.MyBoID.GetValueOrDefault().ToString("B") + "'", criteria.ToString());
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestFromIPrimaryKey_WhenPrimaryKeyIsNull_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            IPrimaryKey primaryKey =null;
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            try
            {
                Criteria.FromPrimaryKey(primaryKey);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("primaryKey", ex.ParamName);
            }
   
        }

        [Test]
        public void TestFromIPrimaryKey_MultipleProp()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKeyNameSurname();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            var surnameValue = Guid.NewGuid().ToString();
            cp.Surname = surnameValue;
            var firstNameValue = Guid.NewGuid().ToString();
            cp.FirstName = firstNameValue;
            var primaryKey = cp.ID;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            var criteria = Criteria.FromPrimaryKey(primaryKey);
            //---------------Test Result -----------------------
            var expectedString = string.Format("(Surname = '{0}') AND (FirstName = '{1}')", surnameValue, firstNameValue);
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

        [Test]
        public void TestUnaryConstructorDoesntWorkForAnd()
        {
            //---------------Set up test pack-------------------
            Criteria dateTimeCriteria = new Criteria("DateTime", Criteria.ComparisonOp.GreaterThan, DateTime.Now);
            
            //---------------Execute Test ----------------------
            try
            {
                new Criteria(Criteria.LogicalOp.And, dateTimeCriteria);
                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("And is not a valid Logical Operator for a Unary Criteria", ex.Message);
            }
        }
        [Test]
        public void TestUnaryConstructorDoesntWorkForOr()
        {
            //---------------Set up test pack-------------------
            Criteria dateTimeCriteria = new Criteria("DateTime", Criteria.ComparisonOp.GreaterThan, DateTime.Now);

            //---------------Execute Test ----------------------
            try
            {
                new Criteria(Criteria.LogicalOp.Or, dateTimeCriteria);
                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("Or is not a valid Logical Operator for a Unary Criteria", ex.Message);
            }
        }

        [Test]
        public void Construct_UsingLambdas()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.Equals;
            var val = "hello";
            var expectedCriteria = new Criteria("TestProp", op, val);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, val);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
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
            DateTimeNow dateTimeNow = new DateTimeNow();
            cp.DateOfBirth = DateTime.Today.AddDays(1);
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, dateTimeNow);
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
            DateTimeNow dateTimeNow = new DateTimeNow();
            cp.DateOfBirth = DateTime.Today.AddDays(-1);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, dateTimeNow);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }
        
        [Test]
        public void TestIsMatch_DateTimeUtcNow_GreaterThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            cp.DateOfBirth = DateTime.Today.AddDays(1);
            cp.Save();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, dateTimeUtcNow);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_DateTimeUtcNow_GreaterThan_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            cp.DateOfBirth = DateTime.Today.AddDays(-1);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, dateTimeUtcNow);
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

        [Test]
        public void TestIsMatch_LessThan_Incomparable()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithImageProperty();
            ContactPersonTestBO cp = new ContactPersonTestBO();

            var testImage = LoadBitmapForTest("sample.bmp");
            var testImage2 = LoadBitmapForTest("sample2.bmp");

            cp.SetPropertyValue("Image", testImage);
            Criteria nameCriteria = new Criteria("Image", Criteria.ComparisonOp.LessThan, testImage2);
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

        [Test]
        public void TestIsMatch_LessThanEqual_Incomparable()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithImageProperty();
            ContactPersonTestBO cp = new ContactPersonTestBO();

            var testImage = LoadBitmapForTest("sample.bmp");
            var testImage2 = LoadBitmapForTest("sample2.bmp");

            cp.SetPropertyValue("Image", testImage);
            Criteria nameCriteria = new Criteria("Image", Criteria.ComparisonOp.LessThanEqual, testImage2);
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
            }
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

        [Test]
        public void TestIsMatch_GreaterThanEqual_Incomparable()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithImageProperty();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            var testImage = LoadBitmapForTest("sample.bmp");
            var testImage2 = LoadBitmapForTest("sample2.bmp");

            cp.SetPropertyValue("Image", testImage);
            Criteria nameCriteria = new Criteria("Image", Criteria.ComparisonOp.GreaterThanEqual, testImage2);
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
            }
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

        [Test]
        public void TestIsMatch_Like_Incomparable()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithImageProperty();
            ContactPersonTestBO cp = new ContactPersonTestBO();

            var testImage = LoadBitmapForTest("sample.bmp");
            var testImage2 = LoadBitmapForTest("sample2.bmp");

            cp.SetPropertyValue("Image", testImage);
            Criteria nameCriteria = new Criteria("Image", Criteria.ComparisonOp.Like, testImage2);
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
            }
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
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
        }

        [Test]
        public void TestIsMatch_NullPropertyValue_Equals_NullMatchValue_ShouldMatch()
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
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
        }

        [Test]
        public void TestIsMatch_NonNullPropertyValue_Equals_NullMatchValue_ShouldNotMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = TestUtil.GetRandomString();
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, null);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp, false);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
        }

        [Test]
        public void TestIsMatch_NonNullGuidValue_Equals_NullMatchValue_ShouldNotMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef_WOrganisationID();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            OrganisationTestBO organisationTestBO = new OrganisationTestBO();
            cp.OrganisationID = organisationTestBO.OrganisationID;
            Criteria nameCriteria = new Criteria("OrganisationID", Criteria.ComparisonOp.Equals, null);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp, false);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
        }

        [Test]
        public void TestIsMatch_NullPersistedPropertyValue_Equals_NullMatchValue_ShouldMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = TestUtil.GetRandomString();
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, null);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
        }

        [Test]
        public void TestIsMatch_NotLike_Incomparable()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithImageProperty();
            ContactPersonTestBO cp = new ContactPersonTestBO();

            var testImage = LoadBitmapForTest("sample.bmp");
            var testImage2 = LoadBitmapForTest("sample2.bmp");
            
            cp.SetPropertyValue("Image", testImage);
            Criteria nameCriteria = new Criteria("Image", Criteria.ComparisonOp.NotLike, testImage2);
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
            }
        }

        private static Image LoadBitmapForTest(string name)
        {
            var asm = Assembly.GetExecutingAssembly();
            var path = asm.GetManifestResourceNames().FirstOrDefault(x => x.Contains(name));

            using (var stream = asm.GetManifestResourceStream(path))
            {
                var foo = Image.FromStream(stream);
                return foo;
            }
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
        public void TestIsMatch_OneProp_IsNot()
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
        public void TestNotIsMatch_OneProp_IsNot()
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

        #region In
        [Test]
        public void TestIn()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            object[] values = new object[] {"100", "200", "300"};
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);
            Criteria criteria = new Criteria("MyField", Criteria.ComparisonOp.In, criteriaValues);

            //-------------Test Result ----------------------
            Assert.IsNotNull(criteria.Field);
            Assert.AreEqual("MyField", criteria.Field.PropertyName);
            Assert.IsNull(criteria.Field.Source);
            Assert.AreEqual("MyField", criteria.Field.FieldName);
            Assert.AreEqual(Criteria.ComparisonOp.In, criteria.ComparisonOperator);
            Assert.IsTrue(criteria.CanBeParametrised());
        }

        [Test]
        public void TestIsMatch_OneProp_In()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "Surname1";
            cp.Save();
            object[] values = new object[] { "Surname1", "Surname2" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.In, criteriaValues);

            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestNotIsMatch_OneProp_In()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "Surname3";
            cp.Save();
            object[] values = new object[] {"Surname1", "Surname2"};
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.In, criteriaValues);

            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it doesn't match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_OneProp_In_Null()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = null;
            object[] values = new object[] { null, "Surname2" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.In, criteriaValues);

            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestNotIsMatch_OneProp_In_Null()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "Surname3";
            cp.Save();
            object[] values = new object[] { null, "Surname2" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.In, criteriaValues);
            bool isMatch = criteria.IsMatch(cp);

            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it doesn't match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        #endregion



        #region Not In
        [Test]
        public void TestNotIn()
        {
            //-------------Setup Test Pack ------------------
            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------
            object[] values = new object[] { "100", "200", "300" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);
            Criteria criteria = new Criteria("MyField", Criteria.ComparisonOp.NotIn, criteriaValues);

            //-------------Test Result ----------------------
            Assert.IsNotNull(criteria.Field);
            Assert.AreEqual("MyField", criteria.Field.PropertyName);
            Assert.IsNull(criteria.Field.Source);
            Assert.AreEqual("MyField", criteria.Field.FieldName);
            Assert.AreEqual(Criteria.ComparisonOp.NotIn, criteria.ComparisonOperator);
            Assert.IsTrue(criteria.CanBeParametrised());
        }

        [Test]
        public void TestIsMatch_OneProp_NotIn()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "Surname3";
            cp.Save();
            object[] values = new object[] { "Surname1", "Surname2" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotIn, criteriaValues);

            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestNotIsMatch_OneProp_NotIn()   
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "Surname1";
            cp.Save();
            object[] values = new object[] { "Surname1", "Surname2" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotIn, criteriaValues);

            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it doesn't match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_OneProp_NotIn_Null()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "Surname1";
            cp.Save();
            object[] values = new object[] { null, "Surname2" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.NotIn, criteriaValues);

            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestNotIsMatch_OneProp_NotIn_Null()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "Surname1";
            cp.FirstName = null;
            cp.Save();
            object[] values = new object[] { null, "FirstName1" };
            Criteria.CriteriaValues criteriaValues = new Criteria.CriteriaValues(values);

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("FirstName", Criteria.ComparisonOp.NotIn, criteriaValues);
            bool isMatch = criteria.IsMatch(cp);

            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it doesn't match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        #endregion


        [Test, Ignore("some tests not yet implemented")]
        public void TestOtherOperators()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.Fail("Todo");
            //Todo:  
            //                
            //
            //            Test using objects and enums TestCriteria
            //test parsing from strings. TestCriteriaParser
            //col.Load(New Criteria("Surname" , Op.Equals, "Powell") TestbusinessObjectCollection
            //col.Load("Surname = Powell"); //Test what happens if we parse using an unsupported operator
            //test for value is null
            //---------------Test Result -----------------------

        }

        [Test]
        public void Test_Create_WhenPropNameNull_ShouldThrowError()
        {
            //---------------Set up test pack-------------------
            string leftCriteria = null;            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new Criteria(leftCriteria, Criteria.ComparisonOp.NotIn, "fdafasdf");
                Assert.Fail("Expected to throw an ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("fieldString", ex.ParamName);
            }
        }

        #endregion //Comparison Operators

        #region IsMatch on BusinessObjectDTO


        [Test]
        public void TestIsMatch_OnBusinessObjectDTO_OneProp_Like()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = "This is MyValue Surname";
            BusinessObjectDTO dto = new BusinessObjectDTO(cp);

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Like, "%MyValue%");
            bool isMatch = criteria.IsMatch(dto);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_OnBusinessObjectDTO_TwoProps_And()
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
            BusinessObjectDTO dto = new BusinessObjectDTO(cp);

            Criteria dobCriteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, dob);
            Criteria nameCriteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, surname);

            //---------------Execute Test ----------------------
            Criteria twoPropCriteria = new Criteria(dobCriteria, Criteria.LogicalOp.And, nameCriteria);
            bool isMatch = twoPropCriteria.IsMatch(dto);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestIsMatch_OnBusinessObjectDTO_TwoProps_Not()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithBoolean();
            MyBO bo = new MyBO();
            bo.TestBoolean = false;
            BusinessObjectDTO dto = new BusinessObjectDTO(bo);

            Criteria notCriteria = new Criteria(null, Criteria.LogicalOp.Not, new Criteria("TestBoolean", Criteria.ComparisonOp.Equals, true));

            //---------------Execute Test ----------------------
            bool isMatch = notCriteria.IsMatch(dto);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
        }

        [Test]
        public void TestIsMatch_OnBusinessObjectDTO_TwoProps_Or()
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
            BusinessObjectDTO dto = new BusinessObjectDTO(cp);

            //---------------Execute Test ----------------------
            Criteria twoPropCriteria = new Criteria(dobCriteria, Criteria.LogicalOp.Or, nameCriteria);
            bool isMatch = twoPropCriteria.IsMatch(dto);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------
        }

        #endregion


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
        public void TestIsMatch_DateTimeUtcNow_Equals_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            cp.DateOfBirth = DateTime.Today.AddDays(-1);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, dateTimeUtcNow);
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
        public void TestIsMatch_DateTimeUtcNow_LessThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            cp.DateOfBirth = DateTime.Today.AddDays(-1);
            cp.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.LessThan, dateTimeUtcNow);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestIsMatch_DateTimeUtcNow_LessThan_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            cp.DateOfBirth = DateTime.Today.AddDays(1);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.LessThan, dateTimeUtcNow);
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
