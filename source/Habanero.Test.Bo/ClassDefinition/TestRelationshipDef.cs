//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
        private PropDefCol mPropDefCol;
        private MockBO mMockBo;

        [TestFixtureSetUp]
        public void init()
        {
            this.SetupDBConnection();
            mMockBo = new MockBO();
            mPropDefCol = mMockBo.PropDefCol;

            mRelKeyDef = new RelKeyDef();
            PropDef propDef = mPropDefCol["MockBOProp1"];

            RelPropDef lRelPropDef = new RelPropDef(propDef, "MockBOID");
            mRelKeyDef.Add(lRelPropDef);

            mRelationshipDef = new SingleRelationshipDef("Relation1", typeof(MockBO), mRelKeyDef, false);
            //DatabaseConnection.CurrentConnection.ConnectionString = MyDBConnection.GetConnectionString();
        }

        [Test]
        public void TestCreateRelationshipDef()
        {
            Assert.AreEqual("Relation1", mRelationshipDef.RelationshipName);
            Assert.AreEqual(typeof(MockBO), mRelationshipDef.RelatedObjectClassType);
            Assert.AreEqual(mRelKeyDef, mRelationshipDef.RelKeyDef);
        }

        [Test]
        [ExpectedException(typeof(HabaneroArgumentException))]
        public void TestCreateRelationshipWithNonBOType()
        {
            RelationshipDef relDef = new SingleRelationshipDef("Relation1", typeof(String), mRelKeyDef, false);
        }

        [Test]
        public void TestCreateRelationship()
        {
            SingleRelationship rel = (SingleRelationship)mRelationshipDef.CreateRelationship(mMockBo, mMockBo.PropCol);
            Assert.AreEqual(mRelationshipDef.RelationshipName, rel.RelationshipName);
            Assert.IsTrue(mMockBo.GetPropertyValue("MockBOProp1") == null);
            Assert.IsFalse(rel.HasRelationship(), "Should be false since props are not defaulted in Mock bo");
            mMockBo.SetPropertyValue("MockBOProp1", mMockBo.GetPropertyValue("MockBOID"));
            mMockBo.Save();
            Assert.IsTrue(rel.HasRelationship(), "Should be true since prop MockBOProp1 has been set");

            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1"), mMockBo.GetPropertyValue("MockBOID"));
            MockBO ltempBO = (MockBO)rel.GetRelatedObject(DatabaseConnection.CurrentConnection);
            Assert.IsFalse(ltempBO == null);
            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOID"), ltempBO.GetPropertyValue("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual(mMockBo.GetPropertyValueString("MockBOProp1"), ltempBO.GetPropertyValueString("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1"), ltempBO.GetPropertyValue("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            mMockBo.Delete();
            mMockBo.Save();
        }

        [Test]
        public void TestProtectedSets()
        {
            RelationshipDefInheritor relDef = new RelationshipDefInheritor();
            RelKeyDef relKeyDef = new RelKeyDef();
            PropDef propDef = new PropDef("prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            relKeyDef.Add(new RelPropDef(propDef, ""));

            Assert.AreEqual("rel", relDef.RelationshipName);
            relDef.SetRelationshipName("newrel");
            Assert.AreEqual("newrel", relDef.RelationshipName);

            Assert.AreEqual(typeof(MyRelatedBo), relDef.RelatedObjectClassType);
            relDef.SetRelatedObjectClassType(typeof(MyBO));
            Assert.AreEqual(typeof(MyBO), relDef.RelatedObjectClassType);

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

        private class RelationshipDefInheritor : RelationshipDef
        {
            public RelationshipDefInheritor() : base("rel", typeof(MyRelatedBo), new RelKeyDef(), true)
            {}

            public override Relationship CreateRelationship(BusinessObject owningBo, BOPropCol lBOPropCol)
            {
                return new SingleRelationship(null, null, null);
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

            public void SetRelKeyDef(RelKeyDef relKeyDef)
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

    internal class MockBO : BusinessObject
    {
        public MockBO() {}

        public MockBO(ClassDef def) : base(def)
        {}

        public static MockBO Create()
        {
            return (MockBO)ClassDef.ClassDefs[typeof(MockBO)].CreateNewBusinessObject();
        }

        protected static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof(MockBO)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.ClassDefs[typeof(MockBO)];
            }
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
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["MockBOID"]);
            ClassDef lClassDef = new ClassDef(typeof(MockBO), primaryKey, lPropDefCol, keysCol, new RelationshipDefCol());
            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("MockBOProp1", typeof(Guid), PropReadWriteRule.ReadWrite, "MockBOProp1", null);
            lPropDefCol.Add(propDef);
            lPropDefCol.Add("MockBOProp2", typeof(string), PropReadWriteRule.WriteOnce, "MockBOProp2", null);
            lPropDefCol.Add("MockBOID", typeof(Guid), PropReadWriteRule.WriteOnce, "MockBOID", null);
            return lPropDefCol;
        }

        #region forTesting

        internal PropDefCol PropDefCol
        {
            get { return _classDef.PropDefcol; }
        }

        internal BOPropCol PropCol
        {
            get { return _boPropCol; }
        }
        
        #endregion //For Testing
    }

    #endregion

    
}
