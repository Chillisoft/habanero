using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Test.Setup.v2;
using NUnit.Framework;

namespace Chillisoft.Test.Bo.v2
{
    /// <summary>
    /// Summary description for TestSingleRelationship.
    /// </summary>
    [TestFixture]
    public class TestSingleRelationship
    {
        private ClassDef itsClassDef;
        private ClassDef itsRelatedClassDef;

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            ClassDef.GetClassDefCol.Clear();
            itsClassDef = MyBo.LoadClassDefWithRelationship();
            itsRelatedClassDef = MyRelatedBo.LoadClassDef();
        }

        [Test]
        public void TestSetRelatedObject()
        {
            MyBo bo1 = (MyBo) itsClassDef.CreateNewBusinessObject();
            MyRelatedBo relatedBo1 = (MyRelatedBo) itsRelatedClassDef.CreateNewBusinessObject();
            ((SingleRelationship) bo1.Relationships["MyRelationship"]).SetRelatedObject(relatedBo1);
            Assert.AreSame(relatedBo1, bo1.Relationships.GetRelatedBusinessObject("MyRelationship"));
            Assert.AreSame(bo1.GetPropertyValue("RelatedID"), relatedBo1.GetPropertyValue("MyRelatedBoID"));
        }
    }
}