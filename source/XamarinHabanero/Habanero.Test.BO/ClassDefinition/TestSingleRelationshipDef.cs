#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO.ClassDefinition
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class TestSingleRelationshipDef
    {
        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
        }
        [Test]
        public void Test_IsOneToOne_WhenHasReverseRelationshipAndIsOneToOne_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            var classDef = MyBO.LoadClassDefWithSingleRelationshipWithReverseRelationship();
            var relationshipDef = classDef.RelationshipDefCol["MyRelationship"];
            MyRelatedBo.LoadClassDefWithSingleRelationshipBackToMyBo();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool isOneToOne = relationshipDef.IsOneToOne;
            //---------------Test Result -----------------------
            Assert.IsTrue(isOneToOne);
        }

        [Test]
        public void Test_IsOneToOne_WhenHasReverseRelationshipAndIsOneToMany_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            var classDef = MyBO.LoadClassDefWithSingleRelationshipWithReverseRelationship();
            var relationshipDef = classDef.RelationshipDefCol["MyRelationship"];
            MyRelatedBo.LoadClassDefWithMultipleRelationshipBackToMyBo();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool isOneToOne = relationshipDef.IsOneToOne;
            //---------------Test Result -----------------------
            Assert.IsFalse(isOneToOne);
        }

        [Test]
        public void Test_IsOneToOne_WhenHasNoReverseRelationship_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            var classDef = MyBO.LoadClassDefWithRelationship();
            var relationshipDef = classDef.RelationshipDefCol["MyRelationship"];
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool isOneToOne = relationshipDef.IsOneToOne;
            //---------------Test Result -----------------------
            Assert.IsFalse(isOneToOne);
        }

        [Test]
        public void Test_IsOneToOne_WhenHasNoReverseRelationship_ShouldBeTrueIfSetAsOneToOne()
        {
            //---------------Set up test pack-------------------
            var classDef = MyBO.LoadClassDefWithRelationship();
            var relationshipDef = (SingleRelationshipDef) classDef.RelationshipDefCol["MyRelationship"];
            relationshipDef.SetAsOneToOne();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool isOneToOne = relationshipDef.IsOneToOne;
            //---------------Test Result -----------------------
            Assert.IsTrue(isOneToOne);
        }

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
        public void Test_IsOneToOne_WhenNoRevRelTypeLoaded_SetOneToOneSetToFalse_ShouldReturnFalseBug944()
        {
            //---------------Set up test pack-------------------
            var singleRelationshipDef = new FakeSingleRelationshipDef
            {
                ReverseRelationshipName = GetRandomString()
            };
            IRelationshipDef relationshipDef = singleRelationshipDef;
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf(typeof(SingleRelationshipDef), relationshipDef);
            Assert.IsNotNullOrEmpty(relationshipDef.ReverseRelationshipName);
            //---------------Execute Test ----------------------
            bool isOneToOne = relationshipDef.IsOneToOne;
            //---------------Test Result -----------------------
            Assert.IsFalse(isOneToOne);
        }
        [Test]
        public void Test_IsOneToOne_WhenNoRevRelTypeLoaded_SetOneToOneSetToTrue_ShouldReturnTrue_Bug944()
        {
            //---------------Set up test pack-------------------
            var singleRelationshipDef = new FakeSingleRelationshipDef
                                            {
                                                ReverseRelationshipName = GetRandomString()
                                            };
            singleRelationshipDef.SetAsOneToOne();
            IRelationshipDef relationshipDef = singleRelationshipDef;
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf(typeof(SingleRelationshipDef), relationshipDef);
            Assert.IsNotNullOrEmpty(relationshipDef.ReverseRelationshipName);
            //---------------Execute Test ----------------------
            bool isOneToOne = relationshipDef.IsOneToOne;
            //---------------Test Result -----------------------
            Assert.IsTrue(isOneToOne);
        }

        private static string GetRandomString()
        {
            return TestUtil.GetRandomString();
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
        public void Test_IsManyToOne_WhenIsSingleRelationship_NotSetOneToOne_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var singleRelationshipDef = new FakeSingleRelationshipDef();
            IRelationshipDef relationshipDef = singleRelationshipDef;
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf(typeof(SingleRelationshipDef), relationshipDef);
            //---------------Execute Test ----------------------
            bool isManyToOne = relationshipDef.IsManyToOne;
            //---------------Test Result -----------------------
            Assert.IsTrue(isManyToOne);
        }

        [Test]
        public void Test_IsManyToOne_WhenHasReverseRelationshipAndIsOneToOne_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            var classDef = MyBO.LoadClassDefWithSingleRelationshipWithReverseRelationship();
            var relationshipDef = classDef.RelationshipDefCol["MyRelationship"];
            MyRelatedBo.LoadClassDefWithSingleRelationshipBackToMyBo();
            //---------------Assert Precondition----------------
            Assert.IsTrue(relationshipDef.IsOneToOne);
            //---------------Execute Test ----------------------
            bool isManyToOne = relationshipDef.IsManyToOne;
            //---------------Test Result -----------------------
            Assert.IsFalse(isManyToOne);
        }

        [Test]
        public void Test_IsManyToOne_WhenHasReverseRelationshipAndIsOneToMany_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            var classDef = MyBO.LoadClassDefWithSingleRelationshipWithReverseRelationship();
            var relationshipDef = classDef.RelationshipDefCol["MyRelationship"];
            MyRelatedBo.LoadClassDefWithMultipleRelationshipBackToMyBo();
            //---------------Assert Precondition----------------
            Assert.IsFalse(relationshipDef.IsOneToOne);
            //---------------Execute Test ----------------------
            bool isManyToOne = relationshipDef.IsManyToOne;
            //---------------Test Result -----------------------
            Assert.IsTrue(isManyToOne);
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

        #region ISingleValueDef

        [Test]
        public void Test_PropertyName_ShouldReturnRelationshipName()
        {
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef(GetRandomString());
            
            //---------------Assert Precondition----------------
            Assert.IsNotNullOrEmpty(relationshipDef.RelationshipName);
            //---------------Execute Test ----------------------
            var propertyName = relationshipDef.PropertyName;
            //---------------Test Result -----------------------
            Assert.AreEqual(relationshipDef.RelationshipName, propertyName);
        }
        [Test]
        public void Test_SetPropertyName_ShouldSetRelationshipName()
        {
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef(GetRandomString());
            var expectedRelationshipName = GetRandomString();

            //---------------Assert Precondition----------------
            Assert.AreNotEqual(expectedRelationshipName, relationshipDef.RelationshipName);
            //---------------Execute Test ----------------------
            relationshipDef.PropertyName = expectedRelationshipName;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedRelationshipName, relationshipDef.RelationshipName);
        }
        [Test]
        public void Test_DisplayName_ShouldReturnRelationshipNamePascalCased()
        {
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef("SomeName");
            const string expectedDisplayName = "Some Name";
            //---------------Assert Precondition----------------
            Assert.IsNotNullOrEmpty(relationshipDef.RelationshipName);
            //---------------Execute Test ----------------------
            var propertyName = relationshipDef.DisplayName;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedDisplayName, propertyName);
        }

        [Test]
        public void Test_GetCompulsory_WhenTrue_ShouldReturnRelationshipIsCompulsory()
        {
            //---------------Set up test pack-------------------

            var relationshipDef = new FakeSingleRelationshipDef();
            relationshipDef.SetAsCompulsory();
            //---------------Assert Precondition----------------
            Assert.IsTrue(relationshipDef.IsCompulsory);
            //---------------Execute Test ----------------------
            var compulsory = relationshipDef.Compulsory;
            //---------------Test Result -----------------------
            Assert.IsTrue(compulsory);
        }
        [Test]
        public void Test_GetCompulsory_WhenFalse_ShouldReturnRelationshipIsCompulsory()
        {
            //---------------Set up test pack-------------------

            var relationshipDef = new FakeSingleRelationshipDef();
            //---------------Assert Precondition----------------
            Assert.IsFalse(relationshipDef.IsCompulsory);
            //---------------Execute Test ----------------------
            var compulsory = relationshipDef.Compulsory;
            //---------------Test Result -----------------------
            Assert.IsFalse(compulsory);
        }

        [Test]
        public void Test_GetDescription_WhenNotSetShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actualDescription = relationshipDef.Description;
            //---------------Test Result -----------------------
            Assert.IsNullOrEmpty(actualDescription);
        }
        [Test]
        public void Test_SetDescription_ShouldSeet()
        {
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            var expectedRelationshipDescription = GetRandomString();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            relationshipDef.Description = expectedRelationshipDescription;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedRelationshipDescription, relationshipDef.Description);
        }

        //TODO mark 04 Oct 2010: For Charles Do tests for
        //PropertyTypeAssemblyName
        //PropertyTypeName
        //PropertyType
        //ClassDef
        //DisplayNameFull
        //ClassName
        #endregion

        #region ISingleValueDef_ImplementngAboveList
       
        [Test]
        public void Test_GetPropertyTypeAssemblyName_ShouldReturnRelatedObjectAssemblyName()
        {//PropertyTypeAssemblyName
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var propertyTypeAssemblyName = relationshipDef.PropertyTypeAssemblyName;
            //---------------Test Result -----------------------
            Assert.IsNotNullOrEmpty(propertyTypeAssemblyName);
            Assert.AreEqual(propertyTypeAssemblyName, relationshipDef.RelatedObjectAssemblyName);
         }

        [Test]
        public void Test_SetPropertyTypeAssemblyName_ShouldSetItToRelatedObjectAssemblyName()
        {  //PropertyTypeAssemblyName
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            var relatedObjectAssemblyName= GetRandomString();
             //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            relationshipDef.PropertyTypeAssemblyName = relatedObjectAssemblyName;
            //---------------Test Result -----------------------
            Assert.AreEqual(relatedObjectAssemblyName, relationshipDef.RelatedObjectAssemblyName);
        }

        [Test]
        public void Test_GetPropertyTypeName_ShouldReturnRelatedObjectClassName()
        {//PropertyTypeName
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var propertyTypeName = relationshipDef.PropertyTypeName;
            //---------------Test Result -----------------------
            Assert.IsNotNullOrEmpty(propertyTypeName);
            Assert.AreEqual(propertyTypeName, relationshipDef.RelatedObjectClassName);
        }

        [Test]
        public void Test_SetPropertyTypeName_ShouldSetItToRelatedObjectAssemblyName()
        {  ////PropertyTypeName
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            var relatedObjectAssemblyName = relationshipDef.RelatedObjectAssemblyName;
           
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            relationshipDef.PropertyTypeName = relatedObjectAssemblyName;
            //---------------Test Result -----------------------
            StringAssert.Contains(relatedObjectAssemblyName,relationshipDef.PropertyTypeName);
        
        }

        [Test]
        public void Test_GetPropertyType_ShouldReturnRelatedObjectClassType()
        {//PropertyType
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var propertyType = relationshipDef.PropertyType;
            //---------------Test Result -----------------------
            Assert.AreEqual(propertyType, relationshipDef.RelatedObjectClassType);
            Assert.AreSame(propertyType, relationshipDef.RelatedObjectClassType);
        }


        [Test]
        public void Test_SetPropertyType_ShouldSetItToRelatedObjectClassType()
        {  ////PropertyType
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            var randomType=relationshipDef.RelatedObjectClassType;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            relationshipDef.PropertyType = randomType;
            //---------------Test Result -----------------------
            Assert.AreSame(randomType, relationshipDef.PropertyType);
        }

        [Test]
        public void Test_GetClassDef_ShouldReturnOwningClassDef()
        {//ClassDef
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var classDef = relationshipDef.ClassDef;
            //---------------Test Result -----------------------
            Assert.AreSame(classDef, relationshipDef.OwningClassDef);
        }


        [Test]
        public void Test_SetClassDef_ShouldSetItToOwningClassDef()
        {  ////ClassDef
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            var classDef = relationshipDef.OwningClassDef;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            relationshipDef.ClassDef = classDef; 
            //---------------Test Result -----------------------
            Assert.AreEqual(classDef, relationshipDef.ClassDef);
        }


        [Test]
        public void Test_GetDisplayNameFull_ShouldReturnDisplayName()
        {//ClassName
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------

            var classNameFull = relationshipDef.DisplayNameFull;
            //---------------Test Result -----------------------
            Assert.AreEqual(classNameFull, relationshipDef.DisplayName);
        }

        [Test]
        public void Test_GetClassName_ShouldReturnOwningClassName()
        {//ClassName
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var className = relationshipDef.ClassName;
            //---------------Test Result -----------------------
            Assert.AreEqual(className, relationshipDef.OwningClassName);
        }
        

        #endregion

        #region Implement ISingleValueDef
        // in terms of redmine issue
        //Feature #1279 Modify Testability to use the new ISingleValueDef instead of PropDef
        //These methods have been moved onto the ISingleValueDef interface.
        //However in the future these should be implemented with more intelligent behavious
        // when we start rationalising code in Habanero.Faces to use the SingleValue 
        // e.g. AutoLoadingComboLookupMapper and LookupComboBox mappers do not inherit common
        //behaviour.

        [Test]
        public void Test_LookupList_ShouldReturnNullLookupList()
        {
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var lookupList = relationshipDef.LookupList;
            //---------------Test Result -----------------------
            Assert.IsNotNull(lookupList);
            Assert.IsInstanceOf<NullLookupList>(lookupList);
        }
        [Test]
        public void Test_SetLookupList_ShouldDoNothing_ShouldReturnNullLookupList()
        {
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var lookupList = relationshipDef.LookupList;
            //---------------Test Result -----------------------
            Assert.IsNotNull(lookupList);
            Assert.IsInstanceOf<NullLookupList>(lookupList);
        }
        [Test]
        public void Test_PropRules_ShouldReturnEmptyPropRules()
        {
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var propRules = relationshipDef.PropRules;
            //---------------Test Result -----------------------
            Assert.IsNotNull(propRules);
            Assert.IsEmpty(propRules, "Should be empty list of prop rules");
        }

        [Test]
        public void Test_ConstructRelDef_ShouldSetReadWriteRuleToReadWriteByDefault()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            //---------------Test Result -----------------------
            Assert.AreEqual(PropReadWriteRule.ReadWrite, relationshipDef.ReadWriteRule);
        }
        [Test]
        public void Test_SetReadWriteRule_ShouldSetRuleOnRelationship()
        {
            //---------------Set up test pack-------------------
            var relationshipDef = new FakeSingleRelationshipDef();
            
            //---------------Assert Precondition----------------
            Assert.AreEqual(PropReadWriteRule.ReadWrite, relationshipDef.ReadWriteRule);
            //---------------Execute Test ----------------------
            relationshipDef.ReadWriteRule = PropReadWriteRule.WriteNew;
            //---------------Test Result -----------------------
            Assert.AreEqual(PropReadWriteRule.WriteNew, relationshipDef.ReadWriteRule);
        }
        #endregion

    }
    // ReSharper restore InconsistentNaming
}