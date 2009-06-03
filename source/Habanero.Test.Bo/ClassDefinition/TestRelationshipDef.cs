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
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestRelationshipDef : TestUsingDatabase
    {
        private RelationshipDef mRelationshipDef;
        private RelKeyDef mRelKeyDef;
        private IPropDefCol mPropDefCol;
        private MockBO mMockBo;

        [TestFixtureSetUp]
        public void init()
        {
            this.SetupDBConnection();

            mMockBo = new MockBO();
            mPropDefCol = mMockBo.PropDefCol;

            mRelKeyDef = new RelKeyDef();
            IPropDef propDef = mPropDefCol["MockBOProp1"];

            RelPropDef lRelPropDef = new RelPropDef(propDef, "MockBOID");
            mRelKeyDef.Add(lRelPropDef);

            mRelationshipDef = new SingleRelationshipDef
                ("Relation1", typeof (MockBO), mRelKeyDef, false, DeleteParentAction.Prevent);
            //DatabaseConnection.CurrentConnection.ConnectionString = MyDBConnection.GetConnectionString();
        }

        [SetUp]
        public void SetupTest()
        {
            BORegistry.DataAccessor = new DataAccessorDB();
        }

        [Test]
        public void TestCreateRelationshipDef()
        {
            Assert.AreEqual("Relation1", mRelationshipDef.RelationshipName);
            Assert.AreEqual(typeof (MockBO), mRelationshipDef.RelatedObjectClassType);
            Assert.AreEqual(mRelKeyDef, mRelationshipDef.RelKeyDef);
        }

        [Test]
        [ExpectedException(typeof (HabaneroArgumentException))]
        public void TestCreateRelationshipWithNonBOType()
        {
            new SingleRelationshipDef
                ("Relation1", typeof (String), mRelKeyDef, false, DeleteParentAction.Prevent);
        }

        [Test]
        public void TestCreateRelationship()
        {
            ISingleRelationship rel =
                (ISingleRelationship) mRelationshipDef.CreateRelationship(mMockBo, mMockBo.PropCol);
            Assert.AreEqual(mRelationshipDef.RelationshipName, rel.RelationshipName);
            Assert.IsTrue(mMockBo.GetPropertyValue("MockBOProp1") == null);
            Assert.IsFalse(rel.HasRelatedObject(), "Should be false since props are not defaulted in Mock bo");
            mMockBo.SetPropertyValue("MockBOProp1", mMockBo.GetPropertyValue("MockBOID"));
            mMockBo.Save();
            Assert.IsTrue(rel.HasRelatedObject(), "Should be true since prop MockBOProp1 has been set");

            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1"), mMockBo.GetPropertyValue("MockBOID"));
            MockBO ltempBO = (MockBO) rel.GetRelatedObject();
            Assert.IsFalse(ltempBO == null);
            Assert.AreEqual
                (mMockBo.GetPropertyValue("MockBOID"), ltempBO.GetPropertyValue("MockBOID"),
                 "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual
                (mMockBo.GetPropertyValueString("MockBOProp1"), ltempBO.GetPropertyValueString("MockBOID"),
                 "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual
                (mMockBo.GetPropertyValue("MockBOProp1"), ltempBO.GetPropertyValue("MockBOID"),
                 "The object returned should be the one with the ID = MockBOID");
            mMockBo.MarkForDelete();
            mMockBo.Save();
        }

        [Test]
        public void TestProtectedSets()
        {
            RelationshipDefInheritor relDef = new RelationshipDefInheritor();
            RelKeyDef relKeyDef = new RelKeyDef();
            PropDef propDef = new PropDef("prop", typeof (string), PropReadWriteRule.ReadWrite, null);
            relKeyDef.Add(new RelPropDef(propDef, ""));

            Assert.AreEqual("rel", relDef.RelationshipName);
            relDef.SetRelationshipName("newrel");
            Assert.AreEqual("newrel", relDef.RelationshipName);

            Assert.AreEqual(typeof (MyRelatedBo), relDef.RelatedObjectClassType);
            relDef.SetRelatedObjectClassType(typeof (MyBO));
            Assert.AreEqual(typeof (MyBO), relDef.RelatedObjectClassType);

            Assert.AreEqual("Habanero.Test", relDef.RelatedObjectAssemblyName);
            relDef.SetRelatedObjectAssemblyName("someassembly");
            Assert.AreEqual("someassembly", relDef.RelatedObjectAssemblyName);

            Assert.AreEqual("Habanero.Test.MyBO", relDef.RelatedObjectClassName);
            relDef.SetRelatedObjectClassName("someclass");
            Assert.AreEqual("someclass", relDef.RelatedObjectClassName);

            Assert.AreEqual(0, relDef.RelKeyDef.Count);
            relDef.SetRelKeyDef(relKeyDef);
            Assert.AreEqual(1, relDef.RelKeyDef.Count);

            Assert.IsTrue(relDef.KeepReferenceToRelatedObject);
            relDef.SetKeepReferenceToRelatedObject(false);
            Assert.IsFalse(relDef.KeepReferenceToRelatedObject);
        }

        [Test]
        public void TestConstruct_WithTimeout_ShouldSetUpTimeout()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            int expectedTimout = 10000;
            MultipleRelationshipDef relDef = new MultipleRelationshipDef
                ("rel", "", "", new RelKeyDef(), true, "", DeleteParentAction.Prevent, InsertParentAction.DoNothing, RelationshipType.Association,
                 expectedTimout);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedTimout, relDef.TimeOut);
        }

        [Test]
        public void TestOwningBOHasForeignKey_Single_Default()
        {
            //---------------Execute Test ----------------------
            SingleRelationshipDef relDef = new SingleRelationshipDef
                ("rel", typeof (MyRelatedBo), new RelKeyDef(), true, DeleteParentAction.Prevent);

            //---------------Test Result -----------------------
            Assert.IsTrue(relDef.OwningBOHasForeignKey);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestOwningBOHasForeignKey_Single()
        {
            //---------------Set up test pack-------------------
            SingleRelationshipDef relDef = new SingleRelationshipDef
                ("rel", typeof (MyRelatedBo), new RelKeyDef(), true, DeleteParentAction.Prevent);

            //---------------Execute Test ----------------------
            relDef.OwningBOHasForeignKey = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(relDef.OwningBOHasForeignKey);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestOwningBOHasForeignKey_Multiple_Default()
        {
            //---------------Execute Test ----------------------
            MultipleRelationshipDef relDef = new MultipleRelationshipDef
                ("rel", typeof (MyRelatedBo), new RelKeyDef(), true, "", DeleteParentAction.Prevent);

            //---------------Test Result -----------------------
            Assert.IsFalse(relDef.OwningBOHasForeignKey);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestOwningBOHasForeignKey_Multiple()
        {
            //---------------Set up test pack-------------------
            MultipleRelationshipDef relDef = new MultipleRelationshipDef
                ("rel", typeof (MyRelatedBo), new RelKeyDef(), true, "", DeleteParentAction.Prevent);

            //---------------Execute Test ----------------------
            relDef.OwningBOHasForeignKey = true;
            //---------------Test Result -----------------------
            Assert.IsFalse(relDef.OwningBOHasForeignKey);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_CheckCanAdd_NullObject()
        {
            //---------------Set up test pack-------------------
            MultipleRelationshipDef relDef = new MultipleRelationshipDef
                ("rel", typeof (MyRelatedBo), new RelKeyDef(), true, "", DeleteParentAction.Prevent);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                relDef.CheckCanAddChild(null);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("could not be added since the  business object is null", ex.Message);
            }
            //---------------Test Result -----------------------
        }

        private class RelationshipDefInheritor : RelationshipDef
        {
            public RelationshipDefInheritor()
                : base("rel", typeof (MyRelatedBo), new RelKeyDef(), true, DeleteParentAction.Prevent)
            {
            }

            public override bool OwningBOHasForeignKey
            {
                get { return true; }
                set { }
            }

            public override IRelationship CreateRelationship(IBusinessObject owningBo, IBOPropCol lBOPropCol)
            {
                return null;
            }

            public void SetRelationshipName(string name)
            {
                RelationshipName = name;
            }

            public void SetRelatedObjectAssemblyName(string name)
            {
                RelatedObjectAssemblyName = name;
            }

            public void SetRelatedObjectClassName(string name)
            {
                RelatedObjectClassName = name;
            }

            public void SetRelatedObjectClassType(Type type)
            {
                RelatedObjectClassType = type;
            }

            public void SetRelKeyDef(IRelKeyDef relKeyDef)
            {
                RelKeyDef = relKeyDef;
            }

            public void SetKeepReferenceToRelatedObject(bool keepRef)
            {
                KeepReferenceToRelatedObject = keepRef;
            }
        }
    }

    #region MockBO For Testing

    public class MockBO : BusinessObject
    {
        public MockBO()
        {
        }

        public MockBO(ClassDef def) : base(def)
        {
        }

        public static MockBO Create()
        {
            return (MockBO) ClassDef.ClassDefs[typeof (MockBO)].CreateNewBusinessObject();
        }

        protected static ClassDef GetClassDef()
        {
            return !ClassDef.IsDefined(typeof (MockBO)) ? CreateClassDef() : ClassDef.ClassDefs[typeof (MockBO)];
        }

        protected override ClassDef ConstructClassDef()
        {
            return GetClassDef();
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = CreateBOPropDef();

            KeyDefCol keysCol = new KeyDefCol();

            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsGuidObjectID = true;
            primaryKey.Add(lPropDefCol["MockBOID"]);
            ClassDef lClassDef = new ClassDef
                (typeof (MockBO), primaryKey, lPropDefCol, keysCol, new RelationshipDefCol());
            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef = new PropDef
                ("MockBOProp1", typeof (Guid), PropReadWriteRule.ReadWrite, "MockBOProp1", null);
            lPropDefCol.Add(propDef);
            lPropDefCol.Add("MockBOProp2", typeof (string), PropReadWriteRule.WriteOnce, "MockBOProp2", null);
            lPropDefCol.Add("MockBOID", typeof (Guid), PropReadWriteRule.WriteOnce, "MockBOID", null);
            return lPropDefCol;
        }

        #region forTesting

        internal IPropDefCol PropDefCol
        {
            get { return _classDef.PropDefcol; }
        }

        internal IBOPropCol PropCol
        {
            get { return _boPropCol; }
        }

        public Guid MockBOID
        {
            get { return (Guid) this.GetPropertyValue("MockBOID"); }
        }

        public Guid? MockBOProp1
        {
            get { return (Guid?) this.GetPropertyValue("MockBOProp1"); }
            set { this.SetPropertyValue("MockBOProp1", value); }
        }

        #endregion //For Testing
    }

    internal class MockBOWithCompulsoryField : BusinessObject
    {
        public MockBOWithCompulsoryField()
        {
        }

        public MockBOWithCompulsoryField(ClassDef def) : base(def)
        {
        }

        protected override ClassDef ConstructClassDef()
        {
            return GetClassDef();
        }

        protected static ClassDef GetClassDef()
        {
            return !ClassDef.IsDefined(typeof (MockBOWithCompulsoryField))
                       ? CreateClassDef()
                       : ClassDef.ClassDefs[typeof (MockBOWithCompulsoryField)];
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = CreateBOPropDef();

            KeyDefCol keysCol = new KeyDefCol();

            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsGuidObjectID = true;
            primaryKey.Add(lPropDefCol["MockBOID"]);
            ClassDef lClassDef = new ClassDef
                (typeof (MockBOWithCompulsoryField), primaryKey, lPropDefCol, keysCol, new RelationshipDefCol());
            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef = new PropDef
                ("MockBOProp1", typeof (Guid), PropReadWriteRule.ReadWrite, "MockBOProp1", null, true, false);
            lPropDefCol.Add(propDef);
            lPropDefCol.Add("MockBOProp2", typeof (string), PropReadWriteRule.WriteOnce, "MockBOProp2", null);
            lPropDefCol.Add("MockBOID", typeof (Guid), PropReadWriteRule.WriteOnce, "MockBOID", null);
            return lPropDefCol;
        }

        public Guid? MockBOProp1
        {
            get { return (Guid?) this.GetPropertyValue("MockBOProp1"); }
            set { this.SetPropertyValue("MockBOProp1", value); }
        }
    }

    #endregion
}