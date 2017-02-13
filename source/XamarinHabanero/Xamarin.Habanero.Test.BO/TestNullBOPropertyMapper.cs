using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestNullBOPropertyMapper
    {// ReSharper disable InconsistentNaming

        [Test]
        public void Construct_ShouldSetPropertyNameAndInvalidReason()
        {
            //---------------Set up test pack-------------------
            var propertyName = "TestPropertyName";
            var invalidReason = "TestInvalidReason";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var propertyMapper = new NullBOPropertyMapper(propertyName, invalidReason);
            //---------------Test Result -----------------------
            Assert.AreEqual(propertyName, propertyMapper.PropertyName);
            Assert.AreEqual(invalidReason, propertyMapper.InvalidReason);
        }

        [Test]
        public void GetPropertyValue_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var propertyMapper = CreatePropertyMapper();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var propertyValue = propertyMapper.GetPropertyValue();
            //---------------Test Result -----------------------
            Assert.IsNull(propertyValue);
        }

        private static NullBOPropertyMapper CreatePropertyMapper()
        {
            return new NullBOPropertyMapper("qwerrt", "Some Invalid Reason");
        }

        [Test]
        public void SetPropertyValue_ShouldNotSetValue()
        {
            //---------------Set up test pack-------------------
            var propertyMapper = CreatePropertyMapper();
            //---------------Assert Precondition----------------
            Assert.IsNull(propertyMapper.GetPropertyValue());
            //---------------Execute Test ----------------------
            propertyMapper.SetPropertyValue("NewValue");
            //---------------Test Result -----------------------
            Assert.IsNull(propertyMapper.GetPropertyValue());
        }
    }
}