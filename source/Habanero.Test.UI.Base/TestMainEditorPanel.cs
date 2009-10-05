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
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestMainEditorPanelVWG
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
            GlobalUIRegistry.ControlFactory = CreateNewControlFactory();
        }

        protected virtual IControlFactory GetControlFactory()
        {
            IControlFactory factory = CreateNewControlFactory();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected virtual IMainEditorPanel CreateControl()
        {
            return CreateControl(GetControlFactory());
        }

        protected virtual IMainEditorPanel CreateControl(IControlFactory controlFactory)
        {
            return new MainEditorPanelVWG(controlFactory);
        }

        protected virtual IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }

        [Test]
        public void Test_Construct_ControlFactoryNull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                CreateControl(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }

        [Test]
        public void Test_Construct_ShouldSetMainTitleIconControl()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainEditorPanel mainEditorPanel = CreateControl(GetControlFactory());
            //---------------Test Result -----------------------
            Assert.IsNotNull(mainEditorPanel.MainTitleIconControl);
            Assert.IsNotNull(mainEditorPanel.EditorPanel);
            Assert.AreEqual(2, mainEditorPanel.Controls.Count);
            Assert.IsTrue(mainEditorPanel.Controls.Contains(mainEditorPanel.MainTitleIconControl));
            Assert.IsTrue(mainEditorPanel.Controls.Contains(mainEditorPanel.EditorPanel));
        }

        [Test]
        public void Test_Construct_ShouldDocTitleIconAndEditorPanel()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainEditorPanel mainEditorPanel = CreateControl(GetControlFactory());
            mainEditorPanel.Size = new Size(432, 567);
            //---------------Test Result -----------------------
            Assert.AreEqual(mainEditorPanel.Width, mainEditorPanel.MainTitleIconControl.Width);
            Assert.AreEqual(mainEditorPanel.Width, mainEditorPanel.EditorPanel.Width);
            Assert.AreEqual
                (mainEditorPanel.Height - mainEditorPanel.MainTitleIconControl.Height,
                 mainEditorPanel.EditorPanel.Height);
        }
    }

    [TestFixture]
    public class TestMainEditorPanelWin : TestMainEditorPanelVWG
    {
        protected override IMainEditorPanel CreateControl(IControlFactory controlFactory)
        {
            return new MainEditorPanelVWG(controlFactory);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}