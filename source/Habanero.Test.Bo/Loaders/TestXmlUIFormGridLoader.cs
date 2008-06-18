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
//using Habanero.UI.Grid;

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

        [Test]
        public void TestDefaults()
        {
            UIFormGrid formGrid = loader.LoadUIFormGrid(@"
                <formGrid relationship=""rel"" reverseRelationship=""correl"" type=""MyBO"" assembly=""Habanero.Test""/>");
            Assert.AreEqual("rel", formGrid.RelationshipName);
            Assert.AreEqual("correl", formGrid.CorrespondingRelationshipName);
            Assert.AreEqual(typeof(MyBO), formGrid.GridType);
        }

        [Test]
        public void TestDefaultGridType()
        {
            UIFormGrid formGrid = loader.LoadUIFormGrid(@"<formGrid relationship=""rel"" reverseRelationship=""correl"" />");
            Assert.AreEqual("EditableGrid", formGrid.GridType.Name);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestGridTypeNotExists()
        {
            loader.LoadUIFormGrid(@"
                <formGrid relationship=""rel"" reverseRelationship=""correl"" type=""mygrid"" assembly=""somewhere""/>");
        }
    }
}