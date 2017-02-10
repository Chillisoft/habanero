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

        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }


        [Test]
        public void TestDatabaseLookupListWithInvalidTimeout()
        {
            try
            {
                XmlDatabaseLookupListLoader loader = new XmlDatabaseLookupListLoader(new DtdLoader(), GetDefClassFactory());
                loader.LoadLookupList(
                    @"<databaseLookupList sql=""Source"" timeout=""aaa"" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("'databaseLookupList' element, an invalid integer was assigned to the 'timeout' attribute", ex.Message);
            }
        }

        [Test]
        public void TestDatabaseLookupListWithNegativeTimeout()
        {
            XmlDatabaseLookupListLoader  loader = new XmlDatabaseLookupListLoader(new DtdLoader(), GetDefClassFactory());
            try
            {
                loader.LoadLookupList(
                    @"<databaseLookupList sql=""Source"" timeout=""-1"" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("invalid integer was assigned to the 'timeout' attribute", ex.Message);
            }
        }

        [Test]
        public void TestDatabaseLookupListWithClassDef()
        {
            XmlDatabaseLookupListLoader loader = new XmlDatabaseLookupListLoader(new DtdLoader(), GetDefClassFactory());
            MyBO.LoadDefaultClassDef();
            ILookupList def =
                loader.LoadLookupList(
                    @"<databaseLookupList sql=""Source"" class=""MyBO"" assembly=""Habanero.Test"" />");
            IDatabaseLookupList source = (IDatabaseLookupList)def;
            Assert.AreEqual("MyBO", source.ClassName);
            Assert.AreEqual("Habanero.Test", source.AssemblyName);
            Assert.AreEqual(10000, source.TimeOut);
        }

    }
}
