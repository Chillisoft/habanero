using System;
using System.Reflection;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Testability;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOPropertyMapperFactory
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            ClassDef.ClassDefs.Clear();
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerNull();
            AddressTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
        }
        [SetUp]
        public void SetupTest()
        {

        }

        [Test]
        public void Test_Create_WhenDirectProp_ShouldCreateBOPropMapper()
        {
            //---------------Set up test pack-------------------
            const string propName = "Surname";
            var bo = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Props.Contains(propName));
            //---------------Execute Test ----------------------
            IBOPropertyMapper propMapper = BOPropMapperFactory.CreateMapper(bo, propName);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<BOPropertyMapper>(propMapper);
            Assert.AreEqual(propName, propMapper.PropertyName);
            Assert.AreSame(bo, propMapper.BusinessObject);
        }

        [Test]
        public void Test_Create_WhenRelatedProp_ShouldCreateBOPropMapper()
        {
            //---------------Set up test pack-------------------
            const string propName = "Organisation.OrganisationID";
            var bo = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Relationships.Contains("Organisation"));
            //---------------Execute Test ----------------------
            IBOPropertyMapper propMapper = BOPropMapperFactory.CreateMapper(bo, propName);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<BOPropertyMapper>(propMapper);
            Assert.AreEqual(propName, propMapper.PropertyName);
            Assert.AreSame(bo, propMapper.BusinessObject);
        }

        [Test]
        public void Test_Create_WhenReflectiveProp_ShouldCreateReflectivePropMapper()
        {
            //---------------Set up test pack-------------------
            const string propName = "-ReflectiveProp-";
            const string propNameWithoutDash = "ReflectiveProp";
            var bo = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            var propertyInfo = ReflectionUtilities.GetPropertyInfo(bo.GetType(), propNameWithoutDash);
            Assert.IsNotNull(propertyInfo);
            //---------------Execute Test ----------------------
            IBOPropertyMapper propMapper = BOPropMapperFactory.CreateMapper(bo, propName);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<ReflectionPropertyMapper>(propMapper);
            Assert.AreEqual(propNameWithoutDash, propMapper.PropertyName);
            Assert.AreSame(bo, propMapper.BusinessObject);
        }
        [Test]
        public void Test_Create_WhenReflectiveProp_WhenNotDefinedWithDash_ShouldCreateReflectivePropMapper()
        {
            //---------------Set up test pack-------------------
            const string propName = "ReflectiveProp";
            var bo = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            var propertyInfo = ReflectionUtilities.GetPropertyInfo(bo.GetType(), propName);
            Assert.IsNotNull(propertyInfo);
            //---------------Execute Test ----------------------
            IBOPropertyMapper propMapper = BOPropMapperFactory.CreateMapper(bo, propName);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<ReflectionPropertyMapper>(propMapper);
            Assert.AreEqual(propName, propMapper.PropertyName);
            Assert.AreSame(bo, propMapper.BusinessObject);
        }
        [Test]
        public void Test_Create_WhenReflectiveProp_WhenWhenReflectivePropNotExists_ShouldCreateReflectivePropMapper()
        {
            //---------------Set up test pack-------------------
            const string propName = "NonExistentReflectiveProp";
            var bo = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            var propertyInfo = ReflectionUtilities.GetPropertyInfo(bo.GetType(), propName);
            Assert.IsNull(propertyInfo);
            //---------------Execute Test ----------------------
            try
            {
                BOPropMapperFactory.CreateMapper(bo, propName);
                Assert.Fail("Expected to throw an InvalidPropertyException");
            }
                //---------------Test Result -----------------------
            catch (InvalidPropertyException ex)
            {
                StringAssert.Contains("The property 'NonExistentReflectiveProp' on 'ContactPersonTestBO' cannot be found", ex.Message);
            }

        }

    }
}