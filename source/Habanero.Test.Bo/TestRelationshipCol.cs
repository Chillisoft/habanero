using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestRelationshipCol.
    /// </summary>
    [TestFixture]
    public class TestRelationshipCol
    {
        private ClassDef itsClassDef;
        private ClassDef itsRelatedClassDef;

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            ClassDef.ClassDefs.Clear();
            itsClassDef = MyBO.LoadClassDefWithRelationship();
            itsRelatedClassDef = MyRelatedBo.LoadClassDef();
        }

        [
            Test,
                ExpectedException(typeof (RelationshipNotFoundException),
                    "The relationship WrongRelationshipName was not found on a BusinessObject of type Habanero.Test.MyBO"
                    )]
        public void TestMissingRelationshipErrorMessageSingle()
        {
            MyBO bo1 = (MyBO) itsClassDef.CreateNewBusinessObject();
            bo1.Relationships.GetRelatedObject("WrongRelationshipName");
        }

        [
            Test,
                ExpectedException(typeof (RelationshipNotFoundException),
                    "The relationship WrongRelationshipName was not found on a BusinessObject of type Habanero.Test.MyBO"
                    )]
        public void TestMissingRelationshipErrorMessageMultiple()
        {
            MyBO bo1 = (MyBO) itsClassDef.CreateNewBusinessObject();
            bo1.Relationships.GetRelatedCollection("WrongRelationshipName");
        }

        [
            Test,
                ExpectedException(typeof (InvalidRelationshipAccessException),
                    "The 'single' relationship MyRelationship was accessed as a 'multiple' relationship (using GetRelatedCollection())."
                    )]
        public void TestInvalidRelationshipAccessSingle()
        {
            MyBO bo1 = (MyBO) itsClassDef.CreateNewBusinessObject();
            bo1.Relationships.GetRelatedCollection("MyRelationship");
        }

        [
            Test,
                ExpectedException(typeof (InvalidRelationshipAccessException),
                    "The 'multiple' relationship MyMultipleRelationship was accessed as a 'single' relationship (using GetRelatedObject())."
                    )]
        public void TestInvalidRelationshipAccessMultiple()
        {
            MyBO bo1 = (MyBO) itsClassDef.CreateNewBusinessObject();
            bo1.Relationships.GetRelatedObject("MyMultipleRelationship");
        }

        [Test]
        public void TestSetRelatedBusinessObject()
        {
            MyBO bo1 = (MyBO) itsClassDef.CreateNewBusinessObject();
            MyRelatedBo relatedBo1 = (MyRelatedBo) itsRelatedClassDef.CreateNewBusinessObject();
            bo1.Relationships.SetRelatedObject("MyRelationship", relatedBo1);
            Assert.AreSame(relatedBo1, bo1.Relationships.GetRelatedObject("MyRelationship"));
            Assert.AreSame(bo1.GetPropertyValue("RelatedID"), relatedBo1.GetPropertyValue("MyRelatedBoID"));
        }

        [
            Test,
                ExpectedException(typeof (InvalidRelationshipAccessException),
                    "SetRelatedObject() was passed a relationship (MyMultipleRelationship) that is of type 'multiple' when it expects a 'single' relationship"
                    )]
        public void TestSetRelatedBusinessObjectWithWrongRelationshipType()
        {
            MyBO bo1 = (MyBO) itsClassDef.CreateNewBusinessObject();
            MyRelatedBo relatedBo1 = (MyRelatedBo) itsRelatedClassDef.CreateNewBusinessObject();
            bo1.Relationships.SetRelatedObject("MyMultipleRelationship", relatedBo1);
        }
    }
}