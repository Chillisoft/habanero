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
//using Habanero.UI.Grid;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlUIPropertyLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlUIFormGridLoader
    {
        private XmlUIFormGridLoader loader;

        [SetUp]
        public void SetupTest()
        {
            loader = new XmlUIFormGridLoader();
        }

		// TODO: This test uses Habanero.UI.Pro, please investigate if this is necessary
		// and then fix this so that it can be uncommented ( - Mark (2007/07/26) )
		//[Test]
		//public void TestSimpleGrid()
		//{
		//    UIFormGrid formGrid =
		//        loader.LoadUIFormGrid(
		//            @"<formGrid relationship=""testrelationshipname"" reverseRelationship=""testcorrespondingrelationshipname"" />");
		//    Assert.AreEqual("testrelationshipname", formGrid.RelationshipName);
		//    Assert.AreEqual("testcorrespondingrelationshipname", formGrid.CorrespondingRelationshipName);
		//    Assert.AreEqual(typeof (EditableGrid), formGrid.GridType);
		//}
    }
}