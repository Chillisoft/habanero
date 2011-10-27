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
using System;
using Habanero.Base.DataMappers;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Base.DataMappers
{
    [TestFixture]
    public class TestDataMapper
    {
        [Test]
        public void TryParsePropValue_ShouldSetReturnValueToNull_WhenValueToParseIsNull()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStub();
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(null, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void TryParsePropValue_ShouldSetReturnValueToNull_WhenValueToParseIsEmptyString()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStub();
            var valueToParse = "";
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void TryParsePropValue_ShouldSetReturnValueToNull_WhenValueToParseIsDBNull()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStub();
            var valueToParse = DBNull.Value;
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void TryParsePropValue_ShouldFail_ForOtherValues()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStub();
            const int valueToParse = 5;
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsFalse(parseSucceed);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void ConvertValueToString_ShouldReturnEmptyString_WhenValueIsNull()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStub();
            //---------------Execute Test ----------------------
            var strValue = dataMapper.ConvertValueToString(null);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, strValue.Length);
        }

        [Test]
        public void ConvertValueToString_ShouldReturnToStringOfValue_WhenValueIsNonNull()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new DataMapperStub();
            //---------------Execute Test ----------------------
            var strValue = dataMapper.ConvertValueToString(5);
            //---------------Test Result -----------------------
            Assert.AreEqual("5", strValue);
        }
     
    }

    internal class DataMapperStub: DataMapper
    {
        
    }
}