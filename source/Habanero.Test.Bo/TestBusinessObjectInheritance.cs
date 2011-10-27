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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
        [TestFixture]
        public class TestBusinessObjectInheritance
        {
            // ReSharper disable InconsistentNaming

            [TestFixtureSetUp]
            public void TestFixtureSetup()
            {
                ClassDef.ClassDefs.Clear();
            }
            [SetUp]
            public void TestSetup()
            {
                ClassDef.ClassDefs.Clear();
            }

            [Test]
            public void Test_Instantiate_SubClassWithNoPrimaryKey()
            {
                //---------------Set up test pack-------------------

                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                CircleNoPrimaryKey circle = new CircleNoPrimaryKey();

                //---------------Test Result -----------------------
                Assert.IsNotNull(circle);

            }


            [Test]
            public void TestSuperClassDefProperty()
            {
                //---------------Set up test pack-------------------
                IClassDef shapeClassDef = Shape.GetClassDef();
                IClassDef circleClassDef = Circle.GetClassDef();
                //---------------Execute Test ----------------------
                ISuperClassDef superClassDef = circleClassDef.SuperClassDef;
                //---------------Test Result -----------------------
                Assert.AreSame(shapeClassDef, superClassDef.SuperClassClassDef,
                               "SuperClassDef.ClassDef property on ClassDef should return the SuperClass's ClassDef");
            }

            [Test]
            public void TestCreateShapeObject()
            {
                //---------------Set up test pack-------------------
                //---------------Execute Test ----------------------
                IBusinessObject objShape = new Shape();
                //---------------Test Result -----------------------
                Assert.AreSame(typeof(Shape), objShape.GetType(),
                               "objShape should be of type Shape, but is of type " + objShape.GetType().Name);
            }

            [Test]
            public void TestCreateCircleObject()
            {
                //---------------Set up test pack-------------------
                //---------------Execute Test ----------------------
                BusinessObject objCircle = new Circle();
                //---------------Test Result -----------------------
                Assert.AreSame(typeof(Circle), objCircle.GetType(),
                               "objCircle should be of type Circle, but is of type " + objCircle.GetType().Name);
                Assert.IsTrue(objCircle is Shape, "A Circle object should be a Shape");
            }

            [Test]
            public void TestObjCircleHasCorrectProperties()
            {
                //---------------Set up test pack-------------------
                //---------------Execute Test ----------------------
                BusinessObject objCircle = new Circle();
                //---------------Test Result -----------------------
                objCircle.GetPropertyValue("ShapeName");
            }

            [Test]
            public void TestObjCircleHasShapeKeys()
            {
                //---------------Set up test pack-------------------
                //---------------Execute Test ----------------------
                BusinessObject objCircle = new Circle();
                //---------------Test Result -----------------------
                Assert.AreEqual(1, objCircle.GetBOKeyCol().Count, "The Circle should have one key inherited from Shape");
            }

            [Test]
            public void TestCircleNoPrimaryKeyInheritsID()
            {
                //---------------Set up test pack-------------------
                IClassDef shapeClassDef = Shape.GetClassDef();
                //---------------Execute Test ----------------------
                BusinessObject objCircleNoPrimaryKey = new CircleNoPrimaryKey();
                //---------------Test Result -----------------------
                Assert.IsNotNull(shapeClassDef.PrimaryKeyDef);
                Shape parent = (Shape)objCircleNoPrimaryKey;
                Assert.AreEqual(objCircleNoPrimaryKey.ID, parent.ID);
                Assert.AreEqual(objCircleNoPrimaryKey.GetPropertyValue("ShapeID"), parent.GetPropertyValue("ShapeID"));
            }

            [Test]
            public void Test_ConstructCircle_WithSingleTableInheritance_ShouldSetDiscriminatorField_Bug1044()
            {
                //---------------Set up test pack-------------------
                Circle.GetClassDefWithSingleTableInheritance();
                Shape.GetClassDef().PropDefcol.Add(new PropDef("SHAPETYPE", typeof(string), PropReadWriteRule.WriteOnce, "ShapeType_field", null));
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                Circle circle = new Circle();
                //---------------Test Result -----------------------
                Assert.AreEqual("Circle", circle.GetPropertyValue("SHAPETYPE"));
            }
            [Test]
            public void Test_ConstructCircle_WithSingleTableInheritance_ShouldSetDiscriminatorField_Bug252()
            {
                //---------------Set up test pack-------------------
                Circle.GetClassDefWithSingleTableInheritance();
                Shape.GetClassDef().PropDefcol.Add(new PropDef("ShapeType", typeof(string), PropReadWriteRule.WriteOnce, "ShapeType_field", null));
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                Circle circle = new Circle();
                //---------------Test Result -----------------------
                Assert.AreEqual("Circle", circle.GetPropertyValue("ShapeType"));
            }
            [Test]
            public void Test_ConstructCircle_WithSingleTableInheritance_WhenDiscrimatorFieldDoesNotExist_ShouldRaiseError_Bug252()
            {
                //---------------Set up test pack-------------------
               
                Circle.GetClassDefWithSingleTableInheritance();
                Shape.GetClassDef();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                
                try
                {
                    new Circle();
                    Assert.Fail("Expected to throw an HabaneroDeveloperException");
                }
                    //---------------Test Result -----------------------
                catch (HabaneroDeveloperException ex)
                {
                    StringAssert.Contains("Your discriminator field is not included in the properties of the class and you are using Single Table Inheritanc", ex.Message);
                }
            }

        }
        // ReSharper restore InconsistentNaming

    }

