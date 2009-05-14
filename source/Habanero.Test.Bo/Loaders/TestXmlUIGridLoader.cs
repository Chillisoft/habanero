//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.Base.Exceptions;
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
            Assert.IsEmpty(def.SortColumn);
        }

        [Test]
        public void TestSortColumn()
        {
            UIGrid def =
                loader.LoadUIGridDef(
                    @"
					<grid sortColumn=""testpropname1 desc"">
						<column heading=""testheading1"" property=""testpropname1""  />
					</grid>");
            Assert.AreEqual("testpropname1 desc", def.SortColumn);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestNoColumns()
        {
            loader.LoadUIGridDef(@"<grid/>");
        }

        [Test]
        public void TestFilterDef()
        {
            //---------------Set up test pack-------------------
            string gridDefXml = string.Format(
                @"
					<grid>
                        <filter>
                            <filterProperty name=""{0}"" label=""{1}"" />
                        </filter>
						<column heading=""testheading1"" property=""{0}""  />
					</grid>", "testpropname1", "testlabel1");
           
            //---------------Execute Test ----------------------
            UIGrid def = loader.LoadUIGridDef(gridDefXml);
            FilterDef filterDef = def.FilterDef;
            //---------------Test Result -----------------------

            Assert.IsNotNull(filterDef);
            Assert.AreEqual(1, def.Count);
            //---------------Tear Down -------------------------          
        }
    }
}