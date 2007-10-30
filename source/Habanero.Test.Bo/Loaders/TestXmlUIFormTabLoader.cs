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
    /// Summary description for TestXmlUIPropertyCollectionLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlFormTabLoader
    {
        private XmlUIFormTabLoader loader;

        [SetUp]
        public void SetupTest()
        {
            loader = new XmlUIFormTabLoader();
        }

        [Test]
        public void TestLoadColumn()
        {
            UIFormTab col =
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
            UIFormTab col =
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


//        [Test]
//        public void TestLoadWithGrid()
//        {
//            UIFormTab col =
//                loader.LoadUIFormTab(
//                    @"
//						<tab name=""testname"">
//							<uiFormGrid relationshipName=""test"" correspondingRelationshipName=""testCor"" />
//						</tab>");
//            Assert.IsNotNull(col.UIFormGrid);
//            Assert.AreEqual("test", col.UIFormGrid.RelationshipName);
//            Assert.AreEqual("testCor", col.UIFormGrid.CorrespondingRelationshipName);
//        }

        //TODO: make sure that having both a grid and formproperties in the same tab causes errror.
    }
}