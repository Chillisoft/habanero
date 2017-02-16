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
using System.Resources;
using Android.Media;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    [TestFixture]
    public class TestSerialisationUtilities
    {
        private ResourceManager _resourceManager;

        [SetUp]
        public void SetUpResources()
        {
            _resourceManager = new ResourceManager("Habanero.Test.TestResources", typeof(TestSerialisationUtilities).Assembly);
        }

        [Test]
        [Ignore("Xamarin Port - Needs to be in NUnitLite Test Project")]
        public void TestTwoWayConversion()
        {
            Image image = (Image)_resourceManager.GetObject("TestJpeg");

            byte[] bytesOut = SerialisationUtilities.ObjectToByteArray(image);
            Object objectOut = SerialisationUtilities.ByteArrayToObject(bytesOut);

            Assert.AreEqual(image.GetType(), objectOut.GetType());
        }
    }
}
