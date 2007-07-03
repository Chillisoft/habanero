using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Test.Bo
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
            itsClassDef = MyBo.LoadClassDefWithRelationship();
            itsRelatedClassDef = MyRelatedBo.LoadClassDef();
        }

        [
            Test,
                ExpectedException(typeof (RelationshipNotFoundException),
                    "The relationship WrongRelationshipName was not found on a BusinessObject of type Habanero.Test.MyBo"
                    )]
        public void TestMissingRelationshipErrorMessageSingle()
        {
            MyBo bo1 = (MyBo) itsClassDef.CreateNewBusinessObject();
            bo1.Relationships.GetRelatedBusinessObject("WrongRelationshipName");
        }

        [
            Test,
                ExpectedException(typeof (RelationshipNotFoundException),
                    "The relationship WrongRelationshipName was not found on a BusinessObject of type Habanero.Test.MyBo"
                    )]
        public void TestMissingRelationshipErrorMessageMultiple()
        {
            MyBo bo1 = (MyBo) itsClassDef.CreateNewBusinessObject();
            bo1.Relationships.GetRelatedBusinessObjectCol("WrongRelationshipName");
        }

        [
            Test,
                ExpectedException(typeof (InvalidRelationshipAccessException),
                    "The 'single' relationship MyRelationship was accessed as a 'multiple' relationship (using GetRelatedBusinessObjectCol())."
                    )]
        public void TestInvalidRelationshipAccessSingle()
        {
            MyBo bo1 = (MyBo) itsClassDef.CreateNewBusinessObject();
            bo1.Relationships.GetRelatedBusinessObjectCol("MyRelationship");
        }

        [
            Test,
                ExpectedException(typeof (InvalidRelationshipAccessException),
                    "The 'multiple' relationship MyMultipleRelationship was accessed as a 'single' relationship (using GetRelatedBusinessObject())."
                    )]
        public void TestInvalidRelationshipAccessMultiple()
        {
            MyBo bo1 = (MyBo) itsClassDef.CreateNewBusinessObject();
            bo1.Relationships.GetRelatedBusinessObject("MyMultipleRelationship");
        }

        [Test]
        public void TestSetRelatedBusinessObject()
        {
            MyBo bo1 = (MyBo) itsClassDef.CreateNewBusinessObject();
            MyRelatedBo relatedBo1 = (MyRelatedBo) itsRelatedClassDef.CreateNewBusinessObject();
            bo1.Relationships.SetRelatedBusinessObject("MyRelationship", relatedBo1);
            Assert.AreSame(relatedBo1, bo1.Relationships.GetRelatedBusinessObject("MyRelationship"));
            Assert.AreSame(bo1.GetPropertyValue("RelatedID"), relatedBo1.GetPropertyValue("MyRelatedBoID"));
        }

        [
            Test,
                ExpectedException(typeof (InvalidRelationshipAccessException),
                    "SetRelatedBusinessObject() was passed a relationship (MyMultipleRelationship) that is of type 'multiple' when it expects a 'single' relationship"
                    )]
        public void TestSetRelatedBusinessObjectWithWrongRelationshipType()
        {
            MyBo bo1 = (MyBo) itsClassDef.CreateNewBusinessObject();
            MyRelatedBo relatedBo1 = (MyRelatedBo) itsRelatedClassDef.CreateNewBusinessObject();
            bo1.Relationships.SetRelatedBusinessObject("MyMultipleRelationship", relatedBo1);
        }
    }
}