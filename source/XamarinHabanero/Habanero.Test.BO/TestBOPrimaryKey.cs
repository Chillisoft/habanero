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
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test;
using Habanero.Test.BO;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOPrimaryKey 
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }

        [Test]
        public void Test_CreateBOPrimaryKey()
        {
            //---------------Set up test pack-------------------
            PrimaryKeyDef pkDef = new PrimaryKeyDef();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            BOPrimaryKey boPrimaryKey = new BOPrimaryKey(pkDef);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boPrimaryKey);
        }
        [Test]
        public void Test_CreateBOPrimaryKey_IsObjectID()
        {
            //---------------Set up test pack-------------------
            PrimaryKeyDef pkDef = new PrimaryKeyDef 
                    {new PropDef("prop2", typeof(Guid), PropReadWriteRule.ReadWrite, null)};
            pkDef.IsGuidObjectID = true;
            BOPrimaryKey boPrimaryKey = new BOPrimaryKey(pkDef);
            //---------------Assert Precondition----------------
            Assert.IsTrue(pkDef.IsGuidObjectID);
            //---------------Execute Test ----------------------
            bool isObjectID = boPrimaryKey.IsGuidObjectID;
            //---------------Test Result -----------------------
            Assert.IsTrue(isObjectID);
        } 
        [Test]
        public void Test_CreateBOPrimaryKey_IsObjectID_False()
        {
            //---------------Set up test pack-------------------
            PrimaryKeyDef pkDef = new PrimaryKeyDef 
                    {new PropDef("prop2", typeof(Guid), PropReadWriteRule.ReadWrite, null)};
            pkDef.IsGuidObjectID = false;
            BOPrimaryKey boPrimaryKey = new BOPrimaryKey(pkDef);
            //---------------Assert Precondition----------------
            Assert.IsFalse(pkDef.IsGuidObjectID);
            //---------------Execute Test ----------------------
            bool isObjectID = boPrimaryKey.IsGuidObjectID;
            //---------------Test Result -----------------------
            Assert.IsFalse(isObjectID);
        }

        [Test]
        public void Test_GetAsValue_Guid()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            Guid contactPersonID = Guid.NewGuid();
            contactPersonTestBO.ContactPersonID = contactPersonID;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object value = contactPersonTestBO.ID.GetAsValue();
            //---------------Test Result -----------------------
            Assert.AreEqual(contactPersonID, value);
        }

        [Test]
        public void Test_GetAsValue_Int()
        {
            //---------------Set up test pack-------------------
            TestAutoInc.LoadClassDefWithIntID();
            const int expecteID = 4;
            TestAutoInc testBO = new TestAutoInc {TestAutoIncID = expecteID};
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object value = testBO.ID.GetAsValue();
            //---------------Test Result -----------------------
            Assert.AreEqual(expecteID, value);
        }

        [Test]
        public void Test_GetAsValue_CompositeKey()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKey();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            Guid contactPersonID = Guid.NewGuid();
            string surname = BOTestUtils.RandomString;
            contactPersonTestBO.ContactPersonID = contactPersonID;
            contactPersonTestBO.Surname = surname;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object value = contactPersonTestBO.ID.GetAsValue();
            //---------------Test Result -----------------------
            string valueString = (string) value;
            StringAssert.Contains("ContactPersonID=" + contactPersonID, valueString);
            StringAssert.Contains("Surname=" + surname, valueString);
        }

        [Test]
        public void Test_GetAsGuid()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Guid id = contactPersonTestBO.ID.GetAsGuid();
            //---------------Test Result -----------------------
            Assert.AreNotEqual(Guid.Empty, id);
        }

        [Test]
        public void Test_CreatePrimaryKey_TwoPropDefs()
        {
            //---------------Set up test pack-------------------
            PropDef propDef1 = new PropDef("prop1", typeof(String), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("prop2", typeof(String), PropReadWriteRule.ReadWrite, null);
            PrimaryKeyDef keyDef = new PrimaryKeyDef { IsGuidObjectID = false };
            keyDef.Add(propDef2);
            keyDef.Add(propDef1);

            BOPropCol boPropCol = new BOPropCol();
            boPropCol.Add(propDef1.CreateBOProp(false));
            boPropCol.Add(propDef2.CreateBOProp(false));

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, keyDef.Count);
            //---------------Execute Test ----------------------
            BOPrimaryKey boPrimaryKey = (BOPrimaryKey)keyDef.CreateBOKey(boPropCol);
            //---------------Test Result -----------------------
            Assert.AreEqual(keyDef.Count, boPrimaryKey.Count);
            Assert.IsTrue(boPrimaryKey.IsCompositeKey);
        }
        [Test]
        public void Test_CreatePrimaryKey_OnePropDefs()
        {
            //---------------Set up test pack-------------------
            PropDef propDef1 = new PropDef("prop1", typeof(String), PropReadWriteRule.ReadWrite, null);
            PrimaryKeyDef keyDef = new PrimaryKeyDef { IsGuidObjectID = false };
            keyDef.Add(propDef1);

            BOPropCol boPropCol = new BOPropCol();
            boPropCol.Add(propDef1.CreateBOProp(false));

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, keyDef.Count);
            //---------------Execute Test ----------------------
            BOPrimaryKey boPrimaryKey = (BOPrimaryKey)keyDef.CreateBOKey(boPropCol);
            //---------------Test Result -----------------------
            Assert.AreEqual(keyDef.Count, boPrimaryKey.Count);
            Assert.IsFalse(boPrimaryKey.IsCompositeKey);
        }

        [Test]
        public void Test_CreateWithValue_ClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            int value = TestUtil.GetRandomInt();
            IClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID { TestField = "PropValue", IntID = value };
            object expectedID = bo.ID;
            
            //---------------Execute Test ----------------------
            BOPrimaryKey key = BOPrimaryKey.CreateWithValue((ClassDef) autoIncClassDef, value);
            //---------------Test Result -----------------------

            Assert.AreEqual(expectedID.ToString(), key.ToString());
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_CreateWithValue_ClassDef_WriteNewProp()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            string value = TestUtil.GetRandomString();
            IClassDef classDef = ContactPersonTestBO.LoadClassDefWithSurnameAsPrimaryKey_WriteNew();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO {Surname = value};
            object expectedID = contactPersonTestBO.ID;

            //---------------Execute Test ----------------------
            BOPrimaryKey key = BOPrimaryKey.CreateWithValue((ClassDef) classDef, value);
            //---------------Test Result -----------------------

            Assert.AreEqual(expectedID.ToString(), key.ToString());
            //---------------Tear Down -------------------------          
        }




        [Test]
        public void Test_CreateWithValue_Type()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            int value = TestUtil.GetRandomInt();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID { TestField = "PropValue", IntID = value };
            object expectedID = bo.ID;
            
            //---------------Execute Test ----------------------
            BOPrimaryKey key = BOPrimaryKey.CreateWithValue(typeof(BOWithIntID), value);
            //---------------Test Result -----------------------

            Assert.AreEqual(expectedID.ToString(), key.ToString());
            //---------------Tear Down -------------------------          
        }

//        [Test]
//        public void Test_HashCode()
//        {
//            //---------------Set up test pack-------------------
//            ClassDef.ClassDefs.Clear();
//            int value = TestUtil.GetRandomInt();
//            BOWithIntID.LoadClassDefWithIntID();
//            BOWithIntID bo = new BOWithIntID { TestField = "PropValue", IntID = value };
//            BOPrimaryKey key = BOPrimaryKey.CreateWithValue(typeof(BOWithIntID), value);
//            //---------------Assert PreConditions---------------       
//            Assert.AreEqual(bo.ID, key);
//            //---------------Execute Test ----------------------
//            object expectedHashCode = bo.ID.GetHashCode();
//            object keyHashCode = key.GetHashCode();
//            //---------------Test Result -----------------------
//            Assert.AreEqual(expectedHashCode, keyHashCode);
//            //---------------Tear Down -------------------------          
//        }

        [Test]
        public void Test_HashCode_CompositeKey()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            new Car();
            ContactPersonCompositeKey contactPerson = new ContactPersonCompositeKey();
            object originalHashCode = contactPerson.ID.GetHashCode();
            contactPerson.Save();
            //---------------Execute Test ----------------------
            object hashCodeAfterSaving = contactPerson.ID.GetHashCode();
            //---------------Test Result -----------------------
            Assert.AreEqual(originalHashCode, hashCodeAfterSaving);
        }

        [Ignore("//TODO Brett 06 Feb 2009: This is an issue but only for tests or code using an in memory database where the Hash code of the ID is being used as the key for the dictionary")]
        [Test]
        public void Test_HashCode_CompositeKey_Edited()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            new Car();
            ContactPersonCompositeKey contactPerson = new ContactPersonCompositeKey();
            contactPerson.Save();
            object originalHashCodeAfterSave = contactPerson.ID.GetHashCode();
            //---------------Execute Test ----------------------
            contactPerson.PK1Prop1 = TestUtil.GetRandomString();
            contactPerson.Save();
            object hashCodeAfterSecondSaving = contactPerson.ID.GetHashCode();
            //---------------Test Result -----------------------
            Assert.AreEqual(originalHashCodeAfterSave, hashCodeAfterSecondSaving);
        }

        [Test]
        public void Test_AsString_CurrentValue()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreateBOPrimaryKeyString();
            Guid guid = Guid.NewGuid();

            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            primaryKey.SetObjectGuidID(guid);
            string keyAsString = primaryKey.AsString_CurrentValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(guid.ToString(), keyAsString);
        }

        [Test]
        public void Test_AsString_CurrentValue_SetValue()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreateBOPrimaryKeyString();
            Guid guid = Guid.NewGuid();
            string str = TestUtil.GetRandomString();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            primaryKey.SetObjectGuidID(guid);
            primaryKey[0].Value = str;
            string keyAsString = primaryKey.AsString_CurrentValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("ContactPersonTestBO.PropName1=" + str, keyAsString);
        }

        [Test]
        public void Test_AsString_PreviousValue()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreateBOPrimaryKeyString();
            Guid guid = Guid.NewGuid();

            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            primaryKey.SetObjectGuidID(guid);
            string keyAsString = primaryKey.AsString_PreviousValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(guid.ToString(), keyAsString);
        }
        
        [Test]
        public void Test_AsString_PreviousValue_SetValue()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreateBOPrimaryKeyString();
            Guid guid = Guid.NewGuid();
            string str = TestUtil.GetRandomString();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            primaryKey.SetObjectGuidID(guid);
            primaryKey[0].Value = str;
            string keyAsString = primaryKey.AsString_PreviousValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(guid.ToString(), keyAsString);
        }
                
        [Test]
        public void Test_AsString_PreviousValue_SetValueTwice()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreateBOPrimaryKeyString();
            Guid guid = Guid.NewGuid();
            primaryKey.SetObjectGuidID(guid);
            string str1 = TestUtil.GetRandomString();
            primaryKey[0].Value = str1;
            string str2 = TestUtil.GetRandomString();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            primaryKey[0].Value = str2;
            string keyAsString = primaryKey.AsString_PreviousValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("ContactPersonTestBO.PropName1=" + str1, keyAsString);
        }
        
        [Test]
        public void Test_AsString_CurrentValue_TwoPropKey()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreatePrimaryBOKeyGuidAndString();
            Guid guid = Guid.NewGuid();

            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            primaryKey.SetObjectGuidID(guid);
            string keyAsString = primaryKey.AsString_CurrentValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(guid.ToString(), keyAsString);
        }
       
        [Test]
        public void Test_AsString_CurrentValue_TwoPropKey_SetOneProp()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreatePrimaryBOKeyGuidAndString();
            Guid guid = Guid.NewGuid();

            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            primaryKey.SetObjectGuidID(guid);
            primaryKey[0].Value = Guid.NewGuid();
            string keyAsString = primaryKey.AsString_CurrentValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(guid.ToString(), keyAsString);
        }

        [Test]
        public void Test_AsString_CurrentValue_TwoPropKey_SetTwoProp()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreatePrimaryBOKeyGuidAndString();
            Guid guid = Guid.NewGuid();
            primaryKey.SetObjectGuidID(guid);
            primaryKey[0].Value = Guid.NewGuid();
            primaryKey[1].Value = TestUtil.GetRandomString();
            //--------------- Test Preconditions ----------------
            
            //--------------- Execute Test ----------------------
            string keyAsString = primaryKey.AsString_CurrentValue();
            //--------------- Test Result -----------------------
            Assert.AreNotEqual(guid.ToString(), keyAsString);
            Assert.AreEqual("ContactPersonTestBO.PropName1=" + primaryKey[0].Value + ";ContactPersonTestBO.PropName2=" + primaryKey[1].Value, keyAsString);

        }    
        
        [Test]
        public void Test_AsString_CurrentValue_TwoPropKey_ResetOneProp()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreatePrimaryBOKeyGuidAndString();
            Guid guid = Guid.NewGuid();
            primaryKey.SetObjectGuidID(guid);
            primaryKey[0].Value = Guid.NewGuid();
            primaryKey[1].Value = TestUtil.GetRandomString();
            //--------------- Test Preconditions ----------------
            
            //--------------- Execute Test ----------------------
            primaryKey[1].Value = TestUtil.GetRandomString();
            string keyAsString = primaryKey.AsString_CurrentValue();
            //--------------- Test Result -----------------------
            Assert.AreNotEqual(guid.ToString(), keyAsString);
            Assert.AreEqual("ContactPersonTestBO.PropName1=" + primaryKey[0].Value + ";ContactPersonTestBO.PropName2=" + primaryKey[1].Value, keyAsString);

        }        

        [Test]
        public void Test_AsString_PreviousValue_TwoPropKey_ResetOneProp()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreatePrimaryBOKeyGuidAndString();
            Guid guid = Guid.NewGuid();
            primaryKey.SetObjectGuidID(guid);
            primaryKey[0].Value = Guid.NewGuid();
            primaryKey[1].Value = TestUtil.GetRandomString();
            string origKeyAsString = primaryKey.AsString_CurrentValue();
            //--------------- Test Preconditions ----------------
            
            //--------------- Execute Test ----------------------
            primaryKey[1].Value = TestUtil.GetRandomString();
            string keyAsString = primaryKey.AsString_PreviousValue();
            //--------------- Test Result -----------------------
            Assert.AreNotEqual(guid.ToString(), keyAsString);
            Assert.AreEqual(origKeyAsString, keyAsString);
        }

        [Test]
        public void Test_ObjectID_NotSet()
        {
            //--------------- Set up test pack ------------------
            //--------------- Test Preconditions ----------------
            //--------------- Execute Test ----------------------
            BOPrimaryKey primaryKey = CreateBOPrimaryKeyString();
            //--------------- Test Result -----------------------
            Assert.AreEqual(Guid.Empty, primaryKey.ObjectID);
        }

        [Test]
        public void Test_ObjectID_SetObjectGuidID()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreateBOPrimaryKeyString();
            Guid id = Guid.NewGuid();
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(Guid.Empty, primaryKey.ObjectID);
            //--------------- Execute Test ----------------------
            primaryKey.SetObjectGuidID(id);
            //--------------- Test Result -----------------------
            Assert.AreEqual(id, primaryKey.ObjectID);
        }

        [Test]
        public void Test_ObjectID_SetObjectGuidID_Twice()
        {
            //--------------- Set up test pack ------------------
            BOPrimaryKey primaryKey = CreateBOPrimaryKeyString();
            Guid id = Guid.NewGuid();
            primaryKey.SetObjectGuidID(id);
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(id, primaryKey.ObjectID);
            //--------------- Execute Test ----------------------
            try
            {
                primaryKey.SetObjectGuidID(Guid.NewGuid());
                //--------------- Test Result -----------------------
                Assert.Fail("InvalidObjectIdException expected");
            }
            catch (InvalidObjectIdException ex)
            {
                Assert.AreEqual("The ObjectGuidID has already been set for this object.", ex.Message);
                Assert.AreEqual(id, primaryKey.ObjectID);
            }
        }

        [Test]
        public void Test_CreateWithValue_ClassDef_WithSuperClass()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            var value = Guid.NewGuid();
            var inheritedClassDef = MyInheritedType.LoadInheritedTypeClassDef();
            //---------------Execute Test ----------------------
            var key = BOPrimaryKey.CreateWithValue((ClassDef)inheritedClassDef, value);
            //---------------Test Result -----------------------
            Assert.AreEqual(value.ToString(), key.ToString());
            //---------------Tear Down -------------------------          
        }

        private static BOPrimaryKey CreateBOPrimaryKeyString()
        {
            PropDef propDef1 = new PropDef("PropName1", typeof(String), PropReadWriteRule.ReadWrite, null)
                                   {ClassDef = ContactPersonTestBO.LoadDefaultClassDef()};
            BOPropCol propCol = new BOPropCol();
            
            propCol.Add(propDef1.CreateBOProp(true));
            PrimaryKeyDef keyDef = new PrimaryKeyDef {IsGuidObjectID = false};
            keyDef.Add(propDef1);
            return (BOPrimaryKey) keyDef.CreateBOKey(propCol);
        }

        private static BOPrimaryKey CreatePrimaryBOKeyGuidAndString()
        {
            PropDef propDef1 = new PropDef("PropName1", typeof(Guid), PropReadWriteRule.ReadWrite, null)
                        { ClassDef = ContactPersonTestBO.LoadDefaultClassDef()};
            PropDef propDef2 = new PropDef("PropName2", typeof(string), PropReadWriteRule.ReadWrite, null) 
                        { ClassDef = propDef1.ClassDef};
            BOPropCol propCol = new BOPropCol();
            propCol.Add(propDef1.CreateBOProp(true));
            propCol.Add(propDef2.CreateBOProp(true));
//            BOPropCol propCol = new BOPropCol();
//            propCol.Add(propDef1.CreateBOProp(true));
//            propCol.Add(propDef2.CreateBOProp(true));
            PrimaryKeyDef keyDef = new PrimaryKeyDef {IsGuidObjectID = false};
            keyDef.Add(propDef1);
            keyDef.Add(propDef2);
            return (BOPrimaryKey)keyDef.CreateBOKey(propCol);
        }
    }
}
