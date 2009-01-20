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
    public class TestRelKey
    {
        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
        }
        [Test]
        public void Test_Criteria_OneProp()
        {
            //--------------- Set up test pack ------------------
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO myBO = new MyBO();
            MultipleRelationship<MyRelatedBo> relationship = myBO.Relationships.GetMultiple<MyRelatedBo>("MyMultipleRelationship");
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            Criteria relCriteria = relationship.RelKey.Criteria;
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("MyBoID = '" + myBO.MyBoID.Value.ToString("B") + "'", relCriteria.ToString());
        }

        [Test]
        public void Test_Criteria_OneProp_NullValue()
        {
            //--------------- Set up test pack ------------------
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyRelatedBo myRelatedBo = new MyRelatedBo();
            SingleRelationship<MyBO> relationship = myRelatedBo.Relationships.GetSingle<MyBO>("MyRelationship");
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            Criteria relCriteria = relationship.RelKey.Criteria;
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("MyBoID IS NULL", relCriteria.ToString());
        }

        [Test]
        public void Test_Criteria_EmptyRelKey()
        {
            //--------------- Set up test pack ------------------
            BOPropCol propCol = new BOPropCol();
            RelKeyDef relKeyDef = new RelKeyDef();
            RelKey relKey = new RelKey(relKeyDef, propCol);
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            Criteria relCriteria = relKey.Criteria;
            //--------------- Test Result -----------------------
            Assert.IsNull(relCriteria);
        }

        [Test]
        public void Test_Criteria_CompositeKey()
        {
            //--------------- Set up test pack ------------------
            new Car();
            ContactPersonCompositeKey person = new ContactPersonCompositeKey();
            person.PK1Prop1 = TestUtil.CreateRandomString();
            person.PK1Prop2 = TestUtil.CreateRandomString();
            MultipleRelationship<Car> relationship = person.Relationships.GetMultiple<Car>("Driver");
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            Criteria relCriteria = relationship.RelKey.Criteria;
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(string.Format("(DriverFK1 = '{0}') AND (DriverFK2 = '{1}')", person.PK1Prop1, person.PK1Prop2), relCriteria.ToString());

        }

    }
}
