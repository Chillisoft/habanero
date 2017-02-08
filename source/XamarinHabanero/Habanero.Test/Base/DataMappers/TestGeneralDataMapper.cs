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
using System;
using System.Drawing;
using System.Net.Mime;
using Habanero.Base.DataMappers;
using NUnit.Framework;

namespace Habanero.Test.Base.DataMappers
{
    [TestFixture]
    public class TestGeneralDataMapper
    {
        [Test]
        public void TryParsePropValue_WhenValueNull_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var propMapper = new GeneralDataMapper(typeof(int));
            object returnValue;
            //---------------Execute Test ----------------------
            var result = propMapper.TryParsePropValue(null, out returnValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            Assert.IsNull(returnValue);
        }

        [Test]
        public void TryParsePropValue_ShouldSetReturnValueEqual_WhenValueToParseIsAlreadyCorrectType_ForValueType()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new GeneralDataMapper(typeof (int));
            int valueToParse = TestUtil.GetRandomInt();
            object returnValue;
            //---------------Execute Test ----------------------
            var result = dataMapper.TryParsePropValue(valueToParse, out returnValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            Assert.AreEqual(valueToParse, returnValue);
        }      
        
        [Test]
        public void TryParsePropValue_ShouldSetReturnValueSame_WhenValueToParseIsAlreadyCorrectType_ForReferenceType()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new GeneralDataMapper(typeof (MediaTypeNames.Image));
            Image valueToParse = new Bitmap(200, 200);
            object returnValue;
            //---------------Execute Test ----------------------
            var result = dataMapper.TryParsePropValue(valueToParse, out returnValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            Assert.AreSame(valueToParse, returnValue);
        }
      
        [Test]
        public void TryParsePropValue_ShouldParseValue_ForCustomTypeWithATypeConverter()
        {
            //---------------Set up test pack-------------------
            const string emailAddToParse = "xxxx.yyyy@ccc.aa.zz";

            var propMapper = new GeneralDataMapper(typeof(EmailAddressWithTypeConverter));
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            var tryParsePropValue = propMapper.TryParsePropValue(emailAddToParse, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(tryParsePropValue);
            Assert.IsInstanceOf<EmailAddressWithTypeConverter>(parsedValue);
            Assert.AreEqual(emailAddToParse, ((EmailAddressWithTypeConverter)parsedValue).EmailAddress);
        }
      
        [Test]
        public void TryParsePropValue_ShouldTryToInstantiateCustomerProperty_ForCustomTypeWithNoTypeConverter()
        {
            //---------------Set up test pack-------------------
            const string emailAddToParse = "xxxx.yyyy@ccc.aa.zz";

            var propMapper = new GeneralDataMapper(typeof(EmailAddressAsCustomProperty));
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            var tryParsePropValue = propMapper.TryParsePropValue(emailAddToParse, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(tryParsePropValue);
            Assert.IsInstanceOf<EmailAddressAsCustomProperty>(parsedValue);
            Assert.AreEqual(emailAddToParse, ((EmailAddressAsCustomProperty)parsedValue).EmailAddress);
        }

        [Test]
        public void TryParsePropValue_ForEnumType_ShouldParseStringToEnumType()
        {
            //---------------Set up test pack-------------------
            var propMapper = new GeneralDataMapper(typeof(ConsoleColor));
            var expectedValue = ConsoleColor.Blue;
            string valueAsString = Enum.GetName(typeof (ConsoleColor), expectedValue);
            object parsedValue;
            //---------------Execute Test ----------------------
            var result = propMapper.TryParsePropValue(valueAsString, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            Assert.AreEqual(expectedValue, parsedValue);
        }

        [Test]
        public void TryParsePropValue_ShouldThrowInvalidCastException_WhenTypeCannotBeConverted()
        {
            //---------------Set up test pack-------------------
            var propMapper = new GeneralDataMapper(typeof(ConsoleColor));
            object parsedValue;
            //---------------Execute Test ----------------------
            try
            {
                propMapper.TryParsePropValue(ConsoleKey.Z, out parsedValue);
                Assert.Fail("should fail when there's no way to convert");
                //---------------Test Result -----------------------
            }
            catch (InvalidCastException ex)
            {
                StringAssert.Contains("Invalid cast from", ex.Message);
            }

        }

        [Test]
        public void TryParsePropValue_ShouldConvert_WhenTypeCanBeConverted()
        {
            //---------------Set up test pack-------------------
            var propMapper = new GeneralDataMapper(typeof(int));
            int expectedValue = TestUtil.GetRandomInt();
            object parsedValue;
            //---------------Execute Test ----------------------
            var result = propMapper.TryParsePropValue(expectedValue.ToString(), out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            Assert.AreEqual(expectedValue, parsedValue);
        }
    }
}