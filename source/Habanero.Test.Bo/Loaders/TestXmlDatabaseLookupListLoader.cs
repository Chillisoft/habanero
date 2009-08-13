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

using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
   
    [TestFixture]
    public class TestXmlDatabaseLookupListLoader
    {
        private XmlDatabaseLookupListLoader _loader;

        [SetUp]
        public void SetupTest()
        {
            _loader = new XmlDatabaseLookupListLoader(new DtdLoader(), GetDefClassFactory());
            ClassDef.ClassDefs.Clear();
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }


        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestDatabaseLookupListWithInvalidTimeout()
        {
            _loader.LoadLookupList(
                    @"<databaseLookupList sql=""Source"" timeout=""aaa"" />");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestDatabaseLookupListWithNegativeTimeout()
        {
            _loader.LoadLookupList(
                    @"<databaseLookupList sql=""Source"" timeout=""-1"" />");
        }

        [Test]
        public void TestDatabaseLookupListWithClassDef()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef();

            ILookupList def =
                _loader.LoadLookupList(
                    @"<databaseLookupList sql=""Source"" class=""MyBO"" assembly=""Habanero.Test"" />");
            DatabaseLookupList source = (DatabaseLookupList)def;
            Assert.IsNotNull(source.ClassDef);
            Assert.AreEqual(classDef.ClassName, source.ClassDef.ClassName);
            Assert.AreEqual(10000, source.TimeOut);
        }

    }
}
