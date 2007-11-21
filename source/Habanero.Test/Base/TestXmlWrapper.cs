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

using System.Xml;
using Habanero.UI.Forms;
using NUnit.Framework;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.Util;
using Habanero.DB;
using Habanero;

namespace Habanero.Test.Base
{
    /// <summary>
    /// TODO - Test:
    /// - Filepath constructor
    /// - ReadXmlValue
    /// - WriteXmlValue
    /// - WriteXmlDocToFile
    /// - WriteXmlDocToFile
    /// </summary>
    [TestFixture]
    public class TestXmlWrapper
    {
        [Test]
        public void TestWrapper()
        {
            XmlDocument doc = new XmlDocument();
            XmlWrapper wrapper = new XmlWrapper(doc);

            Assert.AreEqual(doc, wrapper.XmlDocument);
        }
    }
}
