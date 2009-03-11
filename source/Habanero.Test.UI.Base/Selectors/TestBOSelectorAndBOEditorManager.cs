using System;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestBOSelectorAndBOEditorManager
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            MyBO.LoadDefaultClassDef();
            GlobalUIRegistry.ControlFactory = new ControlFactoryWin();
        }

        public IControlFactory GetControlFactory()
        {
            return GlobalUIRegistry.ControlFactory;
        }

        [Test]
        public void Test_Contructor()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ClassDef.Get<MyBO>();
            IBOColSelectorControl boColSelector = GetControlFactory().CreateReadOnlyGridControl();
            IBusinessObjectControl boEditor = new BOEditorControl(classDef);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            BOSelectorAndEditorManager boSelectorAndEditorManager = new BOSelectorAndEditorManager(boColSelector, boEditor);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boSelectorAndEditorManager);
            Assert.AreSame(boColSelector, boSelectorAndEditorManager.BOColSelector);
            Assert.AreSame(boEditor, boSelectorAndEditorManager.BOEditor);
        }

        [Test]
        public void Test_Constructor_BOSelectorNull_ShouldRaiseError()
        {
            ClassDef classDef = ClassDef.Get<MyBO>();
            IBusinessObjectControl boEditor = new BOEditorControl(classDef); 

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOSelectorAndEditorManager(null, boEditor);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("boColSelector", ex.ParamName);
            }
        }
        [Test]
        public void Test_Constructor_BOEditorNull_ShouldRaiseError()
        {
            IBOColSelectorControl boColSelector = GetControlFactory().CreateReadOnlyGridControl();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOSelectorAndEditorManager(boColSelector, null);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("boEditor", ex.ParamName);
            }
        }

        [Test]
        public void Test_SetCollectionInSelector_SetsFirstItemInBOEditor()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl boColSelector;
            IBusinessObjectControl boEditor;
            CreateBoSelectorAndEditorManager(out boColSelector, out boEditor);
            MyBO myBO;
            BusinessObjectCollection<MyBO> col = GetBOCol_WithOneBO(out myBO);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, col.Count);
            //---------------Execute Test ----------------------
            boColSelector.BusinessObjectCollection = col;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, boColSelector.NoOfItems);
            Assert.IsNotNull(boColSelector.SelectedBusinessObject);
            Assert.AreSame(myBO, boColSelector.SelectedBusinessObject);
            Assert.AreSame(myBO, boEditor.BusinessObject, "BOEditor Should have the Bo Set");
        }

        [Test]
        public void Test_SelectItem_SetsItemInBOEditor()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl boColSelector;
            IBusinessObjectControl boEditor;
            CreateBoSelectorAndEditorManager(out boColSelector, out boEditor);
            MyBO myBO;
            BusinessObjectCollection<MyBO> col = GetBOCol_WithOneBO(out myBO);
            col.CreateBusinessObject();
            boColSelector.BusinessObjectCollection = col;
            MyBO secondBO = col[1];
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(myBO, boEditor.BusinessObject, "BOEditor Should have the first BO Set");
            //---------------Execute Test ----------------------
            boColSelector.SelectedBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, boColSelector.NoOfItems);
            Assert.AreSame(secondBO, boColSelector.SelectedBusinessObject);
            Assert.AreSame(secondBO, boEditor.BusinessObject, "BOEditor Should have the Bo Set");
        }

        private static BusinessObjectCollection<MyBO> GetBOCol_WithOneBO(out MyBO myBO)
        {
            myBO = new MyBO();
            return new BusinessObjectCollection<MyBO> {myBO};
        }

        private void CreateBoSelectorAndEditorManager(out IBOColSelectorControl boColSelector, out IBusinessObjectControl boEditor)
        {
            ClassDef classDef = ClassDef.Get<MyBO>();
            boColSelector = GetControlFactory().CreateReadOnlyGridControl();
            boEditor = new BOEditorControl(classDef);
            new BOSelectorAndEditorManager(boColSelector, boEditor);
            FormWin form = new FormWin();
            form.Controls.Add((Control) boColSelector);
            return;
        }
    }
}
