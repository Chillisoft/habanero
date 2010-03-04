// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System;
using System.Drawing;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test.Structure;
using Habanero.UI.Base;


using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestMainTitleIconControl
    {
        [SetUp]
        public void SetupTest()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.ClassDefs.Add(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader(), new DefClassFactory()).LoadClassDefs());
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GetControlFactory();
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract IControlFactory CreateNewControlFactory();

        [Test]
        public virtual void Test_Construction_WithControlFactory_ShouldSetControlFactory()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = CreateNewControlFactory();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainTitleIconControl outlookStyleMenu = factory.CreateMainTitleIconControl();
            //---------------Test Result -----------------------
            Assert.AreSame(factory, outlookStyleMenu.ControlFactory);
        }

      
        [Test]
        public virtual void TestSetTitle()
        {
            //---------------Set up test pack-------------------
            IMainTitleIconControl titleIconControl = GetControlFactory().CreateMainTitleIconControl();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, titleIconControl.Controls.Count);
            Assert.AreEqual(2, titleIconControl.Panel.Controls.Count);
            //---------------Execute Test ----------------------
            titleIconControl.Title.Text = "Test";
            //---------------Test Result -----------------------
            Assert.AreEqual("Test", titleIconControl.Title.Text);
        }
    }

}