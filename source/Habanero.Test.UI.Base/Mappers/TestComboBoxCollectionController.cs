using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Mappers
{
    public abstract class TestComboBoxCollectionController
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestComboBoxCollectionControllerGiz : TestComboBoxCollectionController
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
        }

        [TestFixture]
        public class TestComboBoxCollectionControllerWin : TestComboBoxCollectionController
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            //---------------Execute Test ----------------------
            ComboBoxCollectionController mapper = new ComboBoxCollectionController(cmbox, controlFactory);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mapper);
            Assert.AreSame(cmbox, mapper.Control);
            Assert.AreSame(controlFactory, mapper.ControlFactory);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetComboBoxCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionController mapper = new ComboBoxCollectionController(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            //---------------Execute Test ----------------------
            mapper.SetCollection(myBoCol,false);
            //---------------Test Result -----------------------
            Assert.AreEqual(3,mapper.Collection.Count);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionController mapper = new ComboBoxCollectionController(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO selectedBO = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(selectedBO);
            myBoCol.Add(new MyBO());
            //---------------Execute Test ----------------------
            mapper.SetCollection(myBoCol, false);
            mapper.Control.SelectedIndex = 1;
            //---------------Test Result -----------------------
            Assert.AreEqual(selectedBO, mapper.SelectedBusinessObject);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestBusinessObejctAddedToCollection()
        {
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionController mapper = new ComboBoxCollectionController(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO addedBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            mapper.SetCollection(myBoCol, false);
            //---------------Execute Test ----------------------
            mapper.Collection.Add(addedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(4,mapper.Control.Items.Count);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestBusinessObejctRemovedFromCollection()
        {
            ClassDef.ClassDefs.Clear();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionController mapper = new ComboBoxCollectionController(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO removedBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(removedBo);
            myBoCol.Add(new MyBO());
            mapper.SetCollection(myBoCol, false);
            //---------------Execute Test ----------------------
            mapper.Collection.Remove(removedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, mapper.Control.Items.Count);
            //---------------Tear down -------------------------
        }
    }
}
