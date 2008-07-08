using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Controllers
{

    public abstract class TestComboBoxCollectionController
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestComboBoxCollectionControllerWin : TestComboBoxCollectionController
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }
        }

        [TestFixture]
        public class TestComboBoxCollectionControllerGiz : TestComboBoxCollectionController
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.WebGUI.ControlFactoryGizmox();
            }

        }

        [Test]
        public void TestCreateTestComboBoxCollectionController()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithBoolean();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO>(); 
            MyBO myBO1 = new MyBO();
            MyBO myBO2 = new MyBO();
            myBOs.Add(myBO1,myBO2);
            IComboBox cmb = GetControlFactory().CreateComboBox();
            ComboBoxCollectionController controller = new ComboBoxCollectionController(cmb,GetControlFactory());
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            controller.SetCollection(myBOs, false);
            //---------------Verify Result -----------------------
            Assert.AreEqual(myBOs, controller.Collection);
            Assert.AreSame(cmb,controller.Control);
            //---------------Tear Down -------------------------   
        }

    }
}
