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
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader(), new DefClassFactory()));
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