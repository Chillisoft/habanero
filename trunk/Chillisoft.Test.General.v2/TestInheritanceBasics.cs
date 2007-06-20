using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using NUnit.Framework;

namespace Chillisoft.Test.General.v2
{
    [TestFixture]
    public class TestInheritanceBasics
    {
        private ClassDef shapeClassDef;
        private ClassDef circleClassDef;
        private BusinessObject objShape;
        private BusinessObject objCircle;

        [TestFixtureSetUp]
        public void SetupTest()
        {
            shapeClassDef = Shape.GetClassDef();
            circleClassDef = Circle.GetClassDef();
            objShape = Shape.GetNewObject();
            objCircle = Circle.GetNewObject();
        }

        [Test]
        public void TestSuperClassDefProperty()
        {
        	Assert.AreSame(shapeClassDef, circleClassDef.SuperClassDesc.SuperClassDef,
                           "SuperClassDesc.ClassDef property on ClassDef should return the SuperClass's ClassDef");
        }

        [Test]
        public void TestCreateShapeObject()
        {
            Assert.AreSame(typeof (Shape), objShape.GetType(),
                           "objShape should be of type Shape, but is of type " + objShape.GetType().Name);
        }

        [Test]
        public void TestCreateCircleObject()
        {
            Assert.AreSame(typeof (Circle), objCircle.GetType(),
                           "objCircle should be of type Circle, but is of type " + objCircle.GetType().Name);
        }

        [Test]
        public void TestObjCircleIsAShape()
        {
            Assert.IsTrue(objCircle is Shape, "A Circle object should be a Shape");
        }

        [Test]
        public void TestObjCircleHasCorrectProperties()
        {
            objCircle.GetPropertyValue("ShapeName");
        }

        [Test]
        public void TestObjCircleHasShapeKeys()
        {
            Assert.AreEqual(1, objCircle.GetBOKeyCol().Count, "The Circle should have one key inherited from Shape");
        }

        [Test]
        public void TestObjCircleHasShapeRelationship()
        {
            Assert.AreEqual(1, objCircle.Relationships.Count,
                            "The Circle object should have one relationship inherited from Shape");
        }
    }
}