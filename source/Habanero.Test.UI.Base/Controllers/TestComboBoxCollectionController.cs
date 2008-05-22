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

        [Test, Ignore("This component is currently not tested")]
        public void TestCreateTestComboBoxCollectionController()
        {
            //---------------Set up test pack-------------------
            IComboBox cmb = GetControlFactory().CreateComboBox();
            ComboBoxCollectionController controller = new ComboBoxCollectionController(cmb,GetControlFactory());
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            controller.SetCollection(null, false);
            //---------------Verify Result -----------------------

            //---------------Tear Down -------------------------   
        }

    }
}
