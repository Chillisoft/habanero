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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// This test class tests the XmlBusinessObjectLookupListLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlBusinessObjectLookupListLoader
    {
        protected XmlBusinessObjectLookupListLoader _loader;

        [SetUp]
        public virtual void SetupTest()
        {
            Initialise();
            ClassDef.ClassDefs.Clear();
        }

        protected void Initialise() {
            _loader = new XmlBusinessObjectLookupListLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        //TODO - Mark 02 Feb 2009 : Add DTD validation tests, possibly?

        [Test]
        public virtual void TestBusinessObjectLookupList()
        {
            //---------------Set up test pack-------------------
            const string xml = @"<businessObjectLookupList class=""MyBO"" assembly=""Habanero.Test"" />";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ILookupList lookupList = _loader.LoadLookupList(xml);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IBusinessObjectLookupList), lookupList);
            IBusinessObjectLookupList source = (IBusinessObjectLookupList)lookupList;
            //Assert.AreEqual(5, source.GetLookupList().Count, "LookupList should have 5 keyvaluepairs");
            Assert.AreEqual("MyBO", source.ClassName);
            Assert.AreEqual("Habanero.Test", source.AssemblyName);

        }

        [Test]
        public virtual void TestBusinessObjectLookupListWithCriteria()
        {
            //---------------Set up test pack-------------------
            const string xml = @"<businessObjectLookupList class=""MyBO"" assembly=""Habanero.Test"" criteria=""TestProp=Test"" />";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ILookupList lookupList = _loader.LoadLookupList(xml);
            //---------------Test Result -----------------------
            IBusinessObjectLookupList source = (IBusinessObjectLookupList)lookupList;
            Assert.AreEqual("TestProp=Test", source.CriteriaString);
        }

        [Test]
        public virtual void TestBusinessObjectLookupListWithSort()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            const string xml = @"<businessObjectLookupList class=""MyBO"" assembly=""Habanero.Test"" sort=""TestProp asc"" />";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ILookupList lookupList = _loader.LoadLookupList(xml);
            //---------------Test Result -----------------------
            IBusinessObjectLookupList source = (IBusinessObjectLookupList)lookupList;
            Assert.AreEqual("TestProp asc", source.SortString);
        }
    }
}