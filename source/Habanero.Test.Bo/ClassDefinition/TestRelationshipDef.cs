// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;
using NUnit.Framework;
using Rhino.Mocks;

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
        public void TestCreateRelationshipWithNonBOType()
        {
            //---------------Execute Test ----------------------
            try
            {
                new SingleRelationshipDef
                    ("Relation1", typeof (String), mRelKeyDef, false, DeleteParentAction.Prevent);

                Assert.Fail("Expected to throw an HabaneroArgumentException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroArgumentException ex)
            {
                StringAssert.Contains("The 'relatedObjectClassType' argument is expected to be of type BusinessObject", ex.Message);
            }
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
            FakeRelationshipDef relDef = new FakeRelationshipDef();
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
            const int expectedTimout = 10000;
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
#pragma warning disable 168
        [Test]
        public void TestWithUnknownRelatedType()
        {
            //---------------Set up test pack-------------------
            DefClassFactory defClassFactory = new DefClassFactory();
            XmlRelationshipLoader loader = new XmlRelationshipLoader(new DtdLoader(), defClassFactory, "TestClass");
            IPropDefCol propDefs = defClassFactory.CreatePropDefCol();
            propDefs.Add(defClassFactory.CreatePropDef("TestProp", "System", "String", PropReadWriteRule.ReadWrite, null, null, false, false, 255, null, null, false));

            RelationshipDef relDef =
                (RelationshipDef) loader.LoadRelationship(
                                      @"<relationship 
						name=""TestRelationship"" 
						type=""single"" 
						relatedClass=""Habanero.Test.BO.Loaders.NonExistantTestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO"" 
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />

					</relationship>",
                                      propDefs);

            //---------------Execute Test ----------------------
            try
            {

                Type classType = relDef.RelatedObjectClassType;

                Assert.Fail("Expected to throw an UnknownTypeNameException");
            }
                //---------------Test Result -----------------------
            catch (UnknownTypeNameException ex)
            {
                StringAssert.Contains("Unable to load the related object type while attempting to load a relationship definition", ex.Message);
            }
        }
#pragma warning restore 168
        [Ignore("Need to write this test")]
        [Test]
        public void Test_LoadInheritedRelationship_UsingInheritedRelatedProps()
        {
            DefClassFactory defClassFactory = new DefClassFactory();
            XmlRelationshipLoader loader = new XmlRelationshipLoader(new DtdLoader(), defClassFactory, "TestClass");
            IPropDefCol propDefs = defClassFactory.CreatePropDefCol();
            propDefs.Add(defClassFactory.CreatePropDef("TestProp", "System", "String", PropReadWriteRule.ReadWrite, null, null, false, false, 255, null, null, false));

            RelationshipDef relDef =
                (RelationshipDef) loader.LoadRelationship(
                                      @"<relationship 
						name=""TestRelationship"" 
						type=""single"" 
						relatedClass=""Habanero.Test.BO.Loaders.NonExistantTestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO"" 
                    >
						    <relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />

					</relationship>",
                                      propDefs);
            Type classType = relDef.RelatedObjectClassType;
        }

        [Test]
        public void Test_SetClassDef_ShouldReturnClassDef()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MockRepository.GenerateMock<IClassDef>();
            RelationshipDef relationshipDef = new FakeRelationshipDef();
            //---------------Assert Precondition----------------
            Assert.IsNull(relationshipDef.OwningClassDef);
            //---------------Execute Test ----------------------
            relationshipDef.OwningClassDef = classDef;
            //---------------Test Result -----------------------
            Assert.AreSame(classDef, relationshipDef.OwningClassDef);
        }

        [Test]
        public void Test_SetOwningClassDef_ShouldSet()
        {
            //---------------Set up test pack-------------------
            IRelationshipDef relationshipDef = new FakeRelationshipDef();
            var expectedClassDef = new FakeClassDef();
            //---------------Assert Precondition----------------
            Assert.IsNull(relationshipDef.OwningClassDef);
            //---------------Execute Test ----------------------
            relationshipDef.OwningClassDef = expectedClassDef;
            //---------------Test Result -----------------------
            Assert.AreSame(expectedClassDef, relationshipDef.OwningClassDef);
        }
        [Test]
        public void Test_GetClassDef_ShouldReturnClassDef()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MockRepository.GenerateMock<IClassDef>();
            IRelationshipDef relationshipDef = new FakeRelationshipDef {OwningClassDef = classDef};
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IClassDef owningClassDef = relationshipDef.OwningClassDef;
            //---------------Test Result -----------------------
            Assert.AreSame(classDef, owningClassDef);
        }

        [Test]
        public void Test_OwningClassName_WhenOwningClassDefNull_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            IRelationshipDef relationshipDef = new FakeRelationshipDef();
            //---------------Assert Precondition----------------
            Assert.IsNull(relationshipDef.OwningClassDef);
            //---------------Execute Test ----------------------
            var owningClassName = relationshipDef.OwningClassName;
            //---------------Test Result -----------------------
            Assert.IsEmpty(owningClassName);
        }
        [Test]
        public void Test_OwningClassName_WhenOwningClassDefSet_ShouldReturnOwningClassDefName()
        {
            IClassDef classDef = MockRepository.GenerateStub<IClassDef>();
            classDef.ClassName = GetRandomString();
            IRelationshipDef relationshipDef = new FakeRelationshipDef { OwningClassDef = classDef };
            //---------------Assert Precondition----------------
            Assert.IsNotNullOrEmpty(classDef.ClassName);
            //---------------Execute Test ----------------------
            var owningClassName = relationshipDef.OwningClassName;
            //---------------Test Result -----------------------
            Assert.AreEqual(classDef.ClassName, owningClassName);
        }

        [Test]
        public void Test_IsOneToMany_WhenIsMultipleRelationship_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            IRelationshipDef relationshipDef = new FakeMultipleRelationshipDef();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf(typeof(MultipleRelationshipDef), relationshipDef);
            //---------------Execute Test ----------------------
            bool isOneToMany = relationshipDef.IsOneToMany;
            //---------------Test Result -----------------------
            Assert.IsTrue(isOneToMany);
        }

        [Test]
        public void Test_IsOneToMany_WhenIsSingleRelationship_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            IRelationshipDef relationshipDef = new FakeSingleRelationshipDef();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf(typeof(SingleRelationshipDef), relationshipDef);
            //---------------Execute Test ----------------------
            bool isOneToMany = relationshipDef.IsOneToMany;
            //---------------Test Result -----------------------
            Assert.IsFalse(isOneToMany);
        }

        [Test]
        public void Test_IsManyToOne_WhenIsMultipleRelationship_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            IRelationshipDef relationshipDef = new FakeMultipleRelationshipDef();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf(typeof(MultipleRelationshipDef), relationshipDef);
            //---------------Execute Test ----------------------
            bool isOneToMany = relationshipDef.IsManyToOne;
            //---------------Test Result -----------------------
            Assert.IsFalse(isOneToMany);
        }

        [Test]
        public void Test_IsManyToOne_WhenIsSingleRelationship_ByDefault_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            IRelationshipDef relationshipDef = new FakeSingleRelationshipDef();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf(typeof(SingleRelationshipDef), relationshipDef);
            //---------------Execute Test ----------------------
            bool isOneToMany = relationshipDef.IsManyToOne;
            //---------------Test Result -----------------------
            Assert.IsTrue(isOneToMany);
        }

        [Test]
        public void Test_IsOneToOne_WhenIsMultipleRelationship_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            IRelationshipDef relationshipDef = new FakeMultipleRelationshipDef();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf(typeof(MultipleRelationshipDef), relationshipDef);
            //---------------Execute Test ----------------------
            bool isOneToOne = relationshipDef.IsOneToOne;
            //---------------Test Result -----------------------
            Assert.IsFalse(isOneToOne);
        }

        [Test]
        public void Test_IsOneToOne_WhenIsSingleRelationship_ByDefault_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            IRelationshipDef relationshipDef = new FakeSingleRelationshipDef();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf(typeof(SingleRelationshipDef), relationshipDef);
            //---------------Execute Test ----------------------
            bool isOneToOne = relationshipDef.IsOneToOne;
            //---------------Test Result -----------------------
            Assert.IsFalse(isOneToOne);
        }

        private static string GetRandomString()
        {
            return TestUtil.GetRandomString();
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

//        public static MockBO Create()
//        {
//            return (MockBO) ClassDef.ClassDefs[typeof (MockBO)].CreateNewBusinessObject();
//        }

        protected static IClassDef GetClassDef()
        {
            return !ClassDef.IsDefined(typeof (MockBO)) ? CreateClassDef() : ClassDef.ClassDefs[typeof (MockBO)];
        }

        protected override IClassDef ConstructClassDef()
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
            lPropDefCol.Add(new PropDef("MockBOProp2", typeof (string), PropReadWriteRule.WriteOnce, "MockBOProp2", null));
            lPropDefCol.Add(new PropDef("MockBOID", typeof (Guid), PropReadWriteRule.WriteOnce, "MockBOID", null));
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
    // ReSharper disable UnusedMember.Global
    internal class MockBOWithCompulsoryField : BusinessObject
    {
/*        public MockBOWithCompulsoryField()
        {
        }*/
/*
        public MockBOWithCompulsoryField(ClassDef def) : base(def)
        {
        }*/

        protected override IClassDef ConstructClassDef()
        {
            return GetClassDef();
        }

        protected static IClassDef GetClassDef()
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
            lPropDefCol.Add(new PropDef("MockBOProp2", typeof (string), PropReadWriteRule.WriteOnce, "MockBOProp2", null));
            lPropDefCol.Add(new PropDef("MockBOID", typeof (Guid), PropReadWriteRule.WriteOnce, "MockBOID", null));
            return lPropDefCol;
        }

        public Guid? MockBOProp1
        {

            get { return (Guid?) this.GetPropertyValue("MockBOProp1"); }

            set { this.SetPropertyValue("MockBOProp1", value); }
        }
    }
    internal class FakeRelationshipDef : RelationshipDef
    {
        private bool _owningBOHasFK;

        public FakeRelationshipDef()
            : base("rel", typeof(MyRelatedBo), new RelKeyDef(), true, DeleteParentAction.Prevent)
        {
            _owningBOHasFK = true;
        }

        public override bool OwningBOHasForeignKey
        {
            get { return _owningBOHasFK; }
            set { _owningBOHasFK = value; }
        }

        public override IRelationship CreateRelationship(IBusinessObject owningBo, IBOPropCol lBOPropCol)
        {
            return null;
        }

        public override bool IsOneToMany
        {
            get { return true; }
        }

        public override bool IsManyToOne
        {
            get { return true; }
        }

        public override bool IsOneToOne
        {
            get { return true; }
        }

        public override bool IsCompulsory
        {
            get { throw new NotImplementedException(); }
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
    internal class FakeMultipleRelationshipDef : MultipleRelationshipDef
    {
        public FakeMultipleRelationshipDef()
            : base("rel", typeof(MyRelatedBo), new RelKeyDef(), true, "", DeleteParentAction.Prevent)
        {
        }
    }
    internal class FakeSingleRelationshipDef : SingleRelationshipDef
    {
        public FakeSingleRelationshipDef(string relationshipName)
            : base(relationshipName, typeof(MyRelatedBo), new RelKeyDef(), true, DeleteParentAction.Prevent)
        {
        }
        public FakeSingleRelationshipDef()
            : this("rel")
        {
        }

        public void SetRelKeyDef(IRelKeyDef relKeyDef)
        {
            RelKeyDef = relKeyDef;
        }
    }
    #endregion
    // ReSharper restore UnusedMember.Global
}