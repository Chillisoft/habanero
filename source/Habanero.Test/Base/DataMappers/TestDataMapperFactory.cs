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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Habanero.Base.DataMappers;
using NUnit.Framework;

namespace Habanero.Test.Base.DataMappers
{
    [TestFixture]
    public class TestDataMapperFactory
    {
        [Test]
        public void GetDataMapper_ShouldReturnGuidDataMapper_WhenTargetTypeIsGuid()
        {
            //---------------Set up test pack-------------------
            var factory = new DataMapperFactory();
            var targetType = typeof(Guid);
            //---------------Execute Test ----------------------
            var dataMapper = factory.GetDataMapper(targetType);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<GuidDataMapper>(dataMapper);
        }

        [Test]
        public void GetDataMapper_ShouldReturnDateTimeDataMapper_WhenTargetTypeIsDateTime()
        {
            //---------------Set up test pack-------------------
            var factory = new DataMapperFactory();
            var targetType = typeof(DateTime);
            //---------------Execute Test ----------------------
            var dataMapper = factory.GetDataMapper(targetType);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<DateTimeDataMapper>(dataMapper);
        }

        [Test]
        public void GetDataMapper_ShouldReturnStringDataMapper_WhenTargetTypeIsString()
        {
            //---------------Set up test pack-------------------
            var factory = new DataMapperFactory();
            var targetType = typeof(String);
            //---------------Execute Test ----------------------
            var dataMapper = factory.GetDataMapper(targetType);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<StringDataMapper>(dataMapper);
        }

        [Test]
        public void GetDataMapper_ShouldReturnBoolDataMapper_WhenTargetTypeIsBool()
        {
            //---------------Set up test pack-------------------
            var factory = new DataMapperFactory();
            var targetType = typeof(bool);
            //---------------Execute Test ----------------------
            var dataMapper = factory.GetDataMapper(targetType);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<BoolDataMapper>(dataMapper);
        }

        [Test]
        public void GetDataMapper_ShouldReturnIntDataMapper_WhenTargetTypeIsInt()
        {
            //---------------Set up test pack-------------------
            var factory = new DataMapperFactory();
            var targetType = typeof(int);
            //---------------Execute Test ----------------------
            var dataMapper = factory.GetDataMapper(targetType);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<IntDataMapper>(dataMapper);
        }

        [Test]
        public void GetDataMapper_ShouldReturnLongDataMapper_WhenTargetTypeIsLong()
        {
            //---------------Set up test pack-------------------
            var factory = new DataMapperFactory();
            var targetType = typeof(long);
            //---------------Execute Test ----------------------
            var dataMapper = factory.GetDataMapper(targetType);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<LongDataMapper>(dataMapper);
        }

        [Test]
        public void GetDataMapper_ShouldReturnImageDataMapper_WhenTargetTypeIsImage()
        {
            //---------------Set up test pack-------------------
            var factory = new DataMapperFactory();
            var targetType = typeof(Image);
            //---------------Execute Test ----------------------
            var dataMapper = factory.GetDataMapper(targetType);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<ImageDataMapper>(dataMapper);
        }

        [Test]
        public void GetDataMapper_ShouldReturnTimeSpanDataMapper_WhenTargetTypeIsTimeSpan()
        {
            //---------------Set up test pack-------------------
            var factory = new DataMapperFactory();
            var targetType = typeof(TimeSpan);
            //---------------Execute Test ----------------------
            var dataMapper = factory.GetDataMapper(targetType);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<TimeSpanDataMapper>(dataMapper);
        }

        [Test]
        public void GetDataMapper_ShouldReturnByteArrayDataMapper_WhenTargetTypeIsByteArray()
        {
            //---------------Set up test pack-------------------
            var factory = new DataMapperFactory();
            var targetType = typeof(byte[]);
            //---------------Execute Test ----------------------
            var dataMapper = factory.GetDataMapper(targetType);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<ByteArrayDataMapper>(dataMapper);
        }

        [Test]
        public void GetDataMapper_ShouldReturnGeneralDataMapper_WhenTargetTypeIsOther()
        {
            //---------------Set up test pack-------------------
            var factory = new DataMapperFactory();
            var targetType = typeof(TestDataMapperFactory);
            //---------------Execute Test ----------------------
            var dataMapper = factory.GetDataMapper(targetType);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<GeneralDataMapper>(dataMapper);
        }

        [Test]
        public void GetDataMapper_ShouldReturnSameObjectEachTime_WhenSameTypeIsAskedFor()
        {
            //---------------Set up test pack-------------------
            var factory = new DataMapperFactory();
            var targetType = typeof(Image);
            var expectedDataMapper = factory.GetDataMapper(targetType);
            //---------------Execute Test ----------------------
            var dataMapper = factory.GetDataMapper(targetType);
            //---------------Test Result -----------------------
            Assert.AreSame(expectedDataMapper, dataMapper);
        }

        [Test]
        public void GetDataMapper_ShouldReturnSameGeneralDataMapperEachTime_WhenSameTypeIsAskedFor()
        {
            //---------------Set up test pack-------------------
            var factory = new DataMapperFactory();
            var targetType = typeof(TestDataMapperFactory);
            var expectedDataMapper = factory.GetDataMapper(targetType);
            //---------------Execute Test ----------------------
            var dataMapper = factory.GetDataMapper(targetType);
            //---------------Test Result -----------------------
            Assert.AreSame(expectedDataMapper, dataMapper);
        }

        [Test]
        public void GetDataMapper_ShouldReturnDifferentGeneralDataMapper_WhenDifferentTypeIsAskedFor()
        {
            //---------------Set up test pack-------------------
            var factory = new DataMapperFactory();
            var targetType = typeof(TestDataMapperFactory);
            var expectedDataMapper = factory.GetDataMapper(typeof(DataMapper));
            //---------------Execute Test ----------------------
            var dataMapper = factory.GetDataMapper(targetType);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<GeneralDataMapper>(dataMapper);
            Assert.AreNotSame(expectedDataMapper, dataMapper);
        }

        [Test]
        public void SetDataMapper_ShouldSetMapperForType()
        {
            //---------------Set up test pack-------------------
            var factory = new DataMapperFactory();
            var targetType = typeof(Image);
            var expectedDataMapper = new GuidDataMapper();
            //---------------Execute Test ----------------------
            factory.SetDataMapper(targetType, expectedDataMapper);
            //---------------Test Result -----------------------
            Assert.AreSame(expectedDataMapper, factory.GetDataMapper(targetType));
        }
    }

   
}
