//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlUIGridLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlUIGridLoader
    {
        private XmlUIGridLoader loader;

        [SetUp]
        public void SetupTest()
        {
            loader = new XmlUIGridLoader();
        }

        [Test]
        public void TestLoadPropertyCollection()
        {
            UIGrid def =
                loader.LoadUIGridDef(
                    @"
					<grid>
						<column heading=""testheading1"" property=""testpropname1""  />
						<column heading=""testheading2"" property=""testpropname2""  />
					</grid>");
            Assert.AreEqual(2, def.Count);
            Assert.AreEqual("testheading1", def[0].Heading);
            Assert.AreEqual("testheading2", def[1].Heading);
        }
    }
}