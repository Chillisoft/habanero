using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestSingleRelationshipDef
    {
        [Test]
        public void Test_IsOneToOne_WhenIsSingleRelationship_SetOneToOneSetToTrue_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var singleRelationshipDef = new FakeSingleRelationshipDef();
            singleRelationshipDef.SetAsOneToOne();
            IRelationshipDef relationshipDef = singleRelationshipDef;
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf(typeof(SingleRelationshipDef), relationshipDef);
            //---------------Execute Test ----------------------
            bool isOneToOne = relationshipDef.IsOneToOne;
            //---------------Test Result -----------------------
            Assert.IsTrue(isOneToOne);
        }
        [Test]
        public void Test_IsManyToOne_WhenIsSingleRelationship_SetOneToOneSetToTrue_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var singleRelationshipDef = new FakeSingleRelationshipDef();
            singleRelationshipDef.SetAsOneToOne();
            IRelationshipDef relationshipDef = singleRelationshipDef;
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf(typeof(SingleRelationshipDef), relationshipDef);
            //---------------Execute Test ----------------------
            bool isManyToOne = relationshipDef.IsManyToOne;
            //---------------Test Result -----------------------
            Assert.IsFalse(isManyToOne);
        }
        [Test]
        public void Test_SetAsCompulsory_ShouldSetIsCompulsoryTrue()
        {
            //---------------Set up test pack-------------------
            var singleRelationshipDef = new FakeSingleRelationshipDef();
            
            IRelationshipDef relationshipDef = singleRelationshipDef;
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf(typeof(SingleRelationshipDef), relationshipDef);
            Assert.IsFalse(relationshipDef.IsCompulsory);
            //---------------Execute Test ----------------------
            singleRelationshipDef.SetAsCompulsory();
            //---------------Test Result -----------------------
            Assert.IsTrue(relationshipDef.IsCompulsory);
        }



        [Test]
        public void Test_IsCompulsory_WhenHasCompulsoryFKProps_ShouldReturnTrue()
        {
            FakeSingleRelationshipDef singleRelationshipDef = new FakeSingleRelationshipDef();
            var relKeyDef = new RelKeyDef();
            var propDef = new PropDefFake { Compulsory = true };
            var relPropDef = new RelPropDef(propDef, "SomeThing");
            relKeyDef.Add(relPropDef);
            singleRelationshipDef.SetRelKeyDef(relKeyDef);
            singleRelationshipDef.OwningBOHasForeignKey = true;

            IRelationshipDef relationshipDef = singleRelationshipDef;
            //---------------Assert Precondition----------------
            Assert.IsTrue(propDef.Compulsory);
            Assert.IsTrue(singleRelationshipDef.OwningBOHasForeignKey);
            //---------------Execute Test ----------------------
            bool isCompulsory = relationshipDef.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsTrue(isCompulsory);
        }
        [Test]
        public void Test_IsCompulsory_WhenNotHasCompulsoryFKProps_ShouldReturnFalse()
        {
            FakeSingleRelationshipDef singleRelationshipDef = new FakeSingleRelationshipDef();
            var relKeyDef = new RelKeyDef();
            var propDef = new PropDefFake { Compulsory = false };
            var relPropDef = new RelPropDef(propDef, "SomeThing");
            relKeyDef.Add(relPropDef);
            singleRelationshipDef.SetRelKeyDef(relKeyDef);
            singleRelationshipDef.OwningBOHasForeignKey = true;

            IRelationshipDef relationshipDef = singleRelationshipDef;
            //---------------Assert Precondition----------------
            Assert.IsFalse(propDef.Compulsory);
            Assert.IsTrue(singleRelationshipDef.OwningBOHasForeignKey);
            //---------------Execute Test ----------------------
            bool isCompulsory = relationshipDef.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsFalse(isCompulsory);
        }

        [Test]
        public void Test_IsCompulsory_WhenPropCompButNotOwningBoHasFK_ShouldRetFalse()
        {
            //---------------Set up test pack-------------------
            FakeSingleRelationshipDef relationshipDef = new FakeSingleRelationshipDef();
            var relKeyDef = new RelKeyDef();
            var propDef = new PropDefFake { Compulsory = true };
            var relPropDef = new RelPropDef(propDef, "SomeThing");
            relKeyDef.Add(relPropDef);
            relationshipDef.SetRelKeyDef(relKeyDef);
            relationshipDef.OwningBOHasForeignKey = false;
            //---------------Assert Precondition----------------
            Assert.IsTrue(propDef.Compulsory);
            Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
            //---------------Execute Test ----------------------
            bool isCompulsory = relationshipDef.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsFalse(isCompulsory, "Rel Should not be compulsory");
        }
        [Test]
        public void Test_IsCompulsory_WhenOwnerPropDefNull_ShouldRetFalse()
        {
            //---------------Set up test pack-------------------
            FakeSingleRelationshipDef relationshipDef = new FakeSingleRelationshipDef();
            var relKeyDef = new RelKeyDef();
            IRelPropDef relPropDef = MockRepository.GenerateStub<IRelPropDef>();
            relPropDef.Stub(def => def.OwnerPropertyName).Return(TestUtil.GetRandomString());
            relKeyDef.Add(relPropDef);
            relationshipDef.SetRelKeyDef(relKeyDef);
            relationshipDef.OwningBOHasForeignKey = true;
            //---------------Assert Precondition----------------
            Assert.IsTrue(relationshipDef.OwningBOHasForeignKey);
            Assert.IsNull(relPropDef.OwnerPropDef);
            //---------------Execute Test ----------------------
            bool isCompulsory = relationshipDef.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsFalse(isCompulsory, "Rel Should not be compulsory");
        }
        [Test]
        public void Test_IsCompulsory_WhenRelKeyDefNull_ShouldRetFalse()
        {
            //---------------Set up test pack-------------------
            FakeSingleRelationshipDef relationshipDef = new FakeSingleRelationshipDef();
            IRelKeyDef relKeyDef = null;
            relationshipDef.SetRelKeyDef(relKeyDef);
            relationshipDef.OwningBOHasForeignKey = true;
            //---------------Assert Precondition----------------
            Assert.IsTrue(relationshipDef.OwningBOHasForeignKey);
            //---------------Execute Test ----------------------
            bool isCompulsory = relationshipDef.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsFalse(isCompulsory, "Rel Should not be compulsory");
        }
        [Test]
        public void Test_IsCompulsory_WhenNoPropDefs_ShouldRetFalse()
        {
            //---------------Set up test pack-------------------
            FakeSingleRelationshipDef relationshipDef = new FakeSingleRelationshipDef();
            IRelKeyDef relKeyDef = new RelKeyDef();
            relationshipDef.SetRelKeyDef(relKeyDef);
            relationshipDef.OwningBOHasForeignKey = true;
            //---------------Assert Precondition----------------
            Assert.IsTrue(relationshipDef.OwningBOHasForeignKey);
            //---------------Execute Test ----------------------
            bool isCompulsory = relationshipDef.IsCompulsory;
            //---------------Test Result -----------------------
            Assert.IsFalse(isCompulsory, "Rel Should not be compulsory");
        }
    }
}