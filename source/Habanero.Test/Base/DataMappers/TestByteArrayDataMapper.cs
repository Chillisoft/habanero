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
using Habanero.Base.DataMappers;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Base.DataMappers
{
    [TestFixture]
    public class TestByteArrayDataMapper
    {
        [Test]
        public void ConvertValueToString()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new ByteArrayDataMapper();
            var val = CreateByteArray();
            //---------------Execute Test ----------------------
            string strValue = dataMapper.ConvertValueToString(val);
            //---------------Test Result -----------------------
            Assert.AreNotEqual("byte[]", strValue);
        }

        [Test]
        public void TryParsePropValue_WorksForNull()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new ByteArrayDataMapper();
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(null, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void TryParsePropValue_WorksForByteArray()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new ByteArrayDataMapper();
            var valueToParse = new byte[200];
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.AreSame(valueToParse, parsedValue);
        }

        [Test]
        public void TryParsePropValue_ConvertsStringToByteArray()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new ByteArrayDataMapper();
            var val = CreateByteArray();
            var valueToParse = dataMapper.ConvertValueToString(val);
            object parsedValue;
            //---------------Execute Test ----------------------
            var parseSucceed = dataMapper.TryParsePropValue(valueToParse, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(parseSucceed);
            Assert.IsInstanceOf(typeof(byte[]), parsedValue);
            Assert.AreEqual(val.Length, ((byte[])parsedValue).Length);
            CollectionAssert.AreEqual(val, (byte[])parsedValue);
        }

        private byte[] CreateByteArray()
        {
            var val = new byte[200];
            for (var i = 0; i < 200; i++) val[i] = (byte)TestUtil.GetRandomInt(0, 255);
            return val;
        }

        [Test]
        public void TryParsePropValue_FailsForOtherTypes()
        {
            //---------------Set up test pack-------------------
            var dataMapper = new ByteArrayDataMapper();
            object parsedValue;
            //---------------Execute Test ----------------------
            var parsedSucceed = dataMapper.TryParsePropValue(3, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsFalse(parsedSucceed);
        }
    }
}