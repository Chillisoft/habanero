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
        }
        [Test]
        public void TestCriteria_IsMatch_OneProp_Equals_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.Op.Equals, Guid.NewGuid().ToString("N"));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCriteria_IsMatch_OneProp_Equals()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.Op.Equals, cp.Surname);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCriteria_IsMatch_OneProp_GreaterThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.GreaterThan, DateTime.Now.AddDays(-1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCriteria_IsMatch_OneProp_GreaterThan_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.GreaterThan, DateTime.Now.AddDays(1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestCriteria_IsMatch_OneProp_LessThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.LessThan, DateTime.Now.AddDays(1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestCriteria_IsMatch_OneProp_LessThan_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.LessThan, DateTime.Now.AddDays(-1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCriteria_IsMatch_TwoProps_And()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            DateTime dob = DateTime.Now;
            cp.DateOfBirth = dob;
            string surname = Guid.NewGuid().ToString("N");
            cp.Surname = surname;

            Criteria dobCriteria = new Criteria("DateOfBirth", Criteria.Op.Equals, dob);
            Criteria nameCriteria = new Criteria("Surname", Criteria.Op.Equals, surname);

            //---------------Execute Test ----------------------
            Criteria twoPropCriteria = new Criteria(dobCriteria, Criteria.LogicalOp.And, nameCriteria);
            bool isMatch = twoPropCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestIsComposite_False()
        {
            //---------------Set up test pack-------------------
            Criteria criteria = new Criteria("bob", Criteria.Op.LessThan, "hello");
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
            Criteria dobCriteria = new Criteria("DateOfBirth", Criteria.Op.Equals, "sfd");
            Criteria nameCriteria = new Criteria("Surname", Criteria.Op.Equals, "dfsd");
            Criteria twoPropCriteria = new Criteria(dobCriteria, Criteria.LogicalOp.And, nameCriteria);
            //---------------Execute Test ----------------------
            bool isComposite = twoPropCriteria.IsComposite();
            //---------------Test Result -----------------------
            Assert.IsTrue(isComposite, "A criteria made up of two others should be composite");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCriteria_IsMatch_TwoProps_Or()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            DateTime dob = DateTime.Now;
            cp.DateOfBirth = dob;
            string surname = Guid.NewGuid().ToString("N");
            cp.Surname = surname;

            Criteria dobCriteria = new Criteria("DateOfBirth", Criteria.Op.Equals, dob.AddDays(2));
            Criteria nameCriteria = new Criteria("Surname", Criteria.Op.Equals, surname);

            //---------------Execute Test ----------------------
            Criteria twoPropCriteria = new Criteria(dobCriteria, Criteria.LogicalOp.Or, nameCriteria);
            bool isMatch = twoPropCriteria.IsMatch(cp);
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
            Criteria nameCriteria = new Criteria("Surname", Criteria.Op.Equals, "surname");
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
            Criteria nameCriteria = new Criteria("Surname", Criteria.Op.Equals, null);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since matches the criteria given.");
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
            Criteria nameCriteria = new Criteria("Surname", Criteria.Op.GreaterThan, "bob");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should be not a match since it does not match the criteria given.");
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
            Criteria nameCriteria = new Criteria("Surname", Criteria.Op.LessThan, "bob");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            bool isMatch = nameCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since matches the criteria given.");
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
            Criteria nameCriteria = new Criteria("Image", Criteria.Op.LessThan, new System.Drawing.Bitmap(20, 20));
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            try
            {
                nameCriteria.IsMatch(cp);
                Assert.Fail("expected InvalidOperationException because you Bitmap does not implement IComparable");
  
            //---------------Test Result -----------------------
                } catch (InvalidOperationException ex) 
                {
                    StringAssert.Contains("does not implement IComparable and cannot be matched", ex.Message);
                    throw;
                }
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestToString_LeafCriteria_String_Equals()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.Op.Equals, surnameValue);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname = '" + surnameValue + "'", criteriaAsString);
        }

        [Test]
        public void TestToString_LeafCriteria_DateTime_GreaterThan()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue = DateTime.Now;
            string datetimePropName = "DateTime";
            Criteria criteria = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("DateTime > '" + dateTimeValue.ToString(Criteria.DATE_FORMAT) + "'", criteriaAsString);
        }

        [Test]
        public void TestToString_And()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            string surname = "Surname";
            Criteria surnameCriteria = new Criteria(surname, Criteria.Op.Equals, surnameValue);
            DateTime dateTimeValue = DateTime.Now;
            string datetimePropName = "DateTime";
            Criteria dateTimeCriteria = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);

            Criteria andCriteria = new Criteria(surnameCriteria, Criteria.LogicalOp.And, dateTimeCriteria);
            //---------------Execute Test ----------------------
            string criteriaAsString = andCriteria.ToString();
            //---------------Test Result -----------------------
            string expectedString = string.Format("(Surname = '{0}') AND (DateTime > '{1}')", surnameValue, dateTimeValue.ToString(Criteria.DATE_FORMAT));
            StringAssert.AreEqualIgnoringCase(expectedString, criteriaAsString);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestToString_UsingDelegates()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            string surname = "Surname";
            Criteria surnameCriteria = new Criteria(surname, Criteria.Op.Equals, surnameValue);
            DateTime dateTimeValue = DateTime.Now;
            string datetimePropName = "DateTime";
            Criteria dateTimeCriteria = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);

            Criteria andCriteria = new Criteria(surnameCriteria, Criteria.LogicalOp.And, dateTimeCriteria);

            //---------------Execute Test ----------------------
            string criteriaAsString = andCriteria.ToString(delegate(string propName) { return propName + "1"; }, delegate { return "param"; });

            //---------------Test Result -----------------------
            string expectedString = "(Surname1 = param) AND (DateTime1 > param)";
            StringAssert.AreEqualIgnoringCase(expectedString, criteriaAsString);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestToString_Guid()
        {
            //---------------Set up test pack-------------------
            Guid guidValue = Guid.NewGuid();
            Criteria guidCriteria = new Criteria("MyID", Criteria.Op.Equals, guidValue);

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            string criteriaAsString = guidCriteria.ToString();
            //---------------Test Result -----------------------
            string expectedString = string.Format("MyID = '{0}'", guidValue.ToString("B"));
            StringAssert.AreEqualIgnoringCase(expectedString, criteriaAsString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestEquals_Null()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue = DateTime.Now;
            string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);
           
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
            string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);

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
            string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);
            Criteria criteria2 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);
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
            string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);
            Criteria criteria2 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue.AddDays(34));
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
            string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue1);
            Criteria criteria2 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue2);
            Criteria composite1 = new Criteria(criteria1, Criteria.LogicalOp.And, criteria2);

            Criteria criteria3 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue1);
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
            string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue1);
            Criteria criteria2 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue2);
            Criteria composite1 = new Criteria(criteria1, Criteria.LogicalOp.And, criteria2);

            Criteria criteria3 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue1);
            Criteria criteria4 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue2);
            Criteria composite2 = new Criteria(criteria3, Criteria.LogicalOp.And, criteria4);
            //---------------Execute Test ----------------------
            bool areEquals = composite1.Equals(composite2);
            //---------------Test Result -----------------------
            Assert.IsTrue(areEquals);
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
            StringAssert.AreEqualIgnoringCase("MyBOID = '" + myBO.MyBoID.ToString("B") + "'", criteria.ToString());
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
            RelKeyDef relKeyDef = new RelKeyDef();
            string propValue1 = "bob1";
            PropDef boPropDef1 = new PropDef("Prop1", typeof(String), PropReadWriteRule.ReadWrite, propValue1);
            relKeyDef.Add(new RelPropDef(boPropDef1, "RelatedProp1"));
            string propValue2 = "bob2";
            PropDef boPropDef2 = new PropDef("Prop2", typeof(String), PropReadWriteRule.ReadWrite, propValue2);
            relKeyDef.Add(new RelPropDef(boPropDef2, "RelatedProp2"));
            RelationshipDef reldef =
                new MultipleRelationshipDef("bob", "bob", "bob", relKeyDef, false, "", DeleteParentAction.DoNothing);
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

    }
}
