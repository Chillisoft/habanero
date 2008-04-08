//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
    /// Summary description for TestXmlUIPropertyCollectionLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlUIFormColumnLoader
    {
        private XmlUIFormColumnLoader loader;

        [SetUp]
        public void SetupTest()
        {
            loader = new XmlUIFormColumnLoader();
        }

        [Test]
        public void TestLoadColumn()
        {
            UIFormColumn col =
                loader.LoadUIFormColumn(
                    @"
							<columnLayout width=""123"">
								<field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
								<field label=""testlabel2"" property=""testpropname2"" type=""Button"" mapperType=""testmappertypename2"" />
							</columnLayout>");
            Assert.AreEqual(2, col.Count);
            Assert.AreEqual(123, col.Width);
            Assert.AreEqual("testlabel1", col[0].Label);
            Assert.AreEqual("testlabel2", col[1].Label);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestInvalidWidth()
        {
            loader.LoadUIFormColumn(@"
				<columnLayout width=""aaa"">
					<field property=""testpropname1"" />
				</columnLayout>");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestNoFields()
        {
            loader.LoadUIFormColumn(@"
				<columnLayout></columnLayout>");
        }

        [Test]
        public void TestTriggers()
        {
            UIFormColumn column = loader.LoadUIFormColumn(
                @"<columnLayout>
					<field property=""testpropname1"" >
                        <trigger action=""action"" value=""value"" />
                    </field>
                    <field property=""testpropname2"" />
				</columnLayout>");
            Assert.AreEqual(1, column[0].Triggers.Count);
            Assert.AreEqual(0, column[1].Triggers.Count);
        }
    }
}