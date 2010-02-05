using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestBOSelectorAndBOEditorManager
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
        }

        protected abstract IControlFactory GetControlFactory();

        [Test]
        public void Test_Contructor()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ClassDef.Get<MyBO>();
            IBOColSelectorControl boColSelector = GetControlFactory().CreateReadOnlyGridControl();
            IBusinessObjectControl boEditor = GetControlFactory().CreateBOEditorControl(classDef);

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
            IClassDef classDef = ClassDef.Get<MyBO>();
            IBusinessObjectControl boEditor = GetControlFactory().CreateBOEditorControl(classDef);

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
            IClassDef classDef = ClassDef.Get<MyBO>();
            boColSelector = GetControlFactory().CreateReadOnlyGridControl();
            boEditor = GetControlFactory().CreateBOEditorControl(classDef);
            new BOSelectorAndEditorManager(boColSelector, boEditor);
            IFormHabanero form = GetControlFactory().CreateForm();
            form.Controls.Add(boColSelector);
            return;
        }
    }

}
