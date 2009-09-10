//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlUIPropertyCollectionLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlFormTabLoader
    {
        private XmlUIFormTabLoader loader;

        [SetUp]
        public void SetupTest() {
            Initialise();
        }

        protected void Initialise() {
            loader = new XmlUIFormTabLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        [Test]
        public void TestLoadColumn()
        {
            IUIFormTab col =
                loader.LoadUIFormTab(
                    @"
						<tab name=""testname"">
							<columnLayout>
								<field label=""testlabel1"" property=""testpropname1"" />
								<field label=""testlabel2"" property=""testpropname2"" />
							</columnLayout>
							<columnLayout>
								<field label=""testlabel3"" property=""testpropname3"" />
							</columnLayout>
						</tab>");
            Assert.AreEqual("testname", col.Name);
            Assert.AreEqual(2, col.Count, "There should be two column.");
            Assert.AreEqual(2, col[0].Count, "There should be two props in column 1");
            Assert.AreEqual(1, col[1].Count, "There should be one prop in column 2");
            Assert.AreEqual("testlabel1", col[0][0].Label);
            Assert.AreEqual("testlabel3", col[1][0].Label);
        }

        [Test]
        public void TestTabWithFields()
        {
            IUIFormTab col =
                loader.LoadUIFormTab(
                    @"
						<tab name=""testname"">
							<field label=""testlabel1"" property=""testpropname1"" />
							<field label=""testlabel2"" property=""testpropname2"" />
							<field label=""testlabel3"" property=""testpropname3"" />
						</tab>");
            Assert.AreEqual("testname", col.Name);
            Assert.AreEqual(1, col.Count, "There should be one column.");
            Assert.AreEqual(3, col[0].Count, "There should be two props in column 1");
            Assert.AreEqual("testlabel1", col[0][0].Label);
            Assert.AreEqual("testlabel2", col[0][1].Label);
            Assert.AreEqual("testlabel3", col[0][2].Label);
        }
    }
}