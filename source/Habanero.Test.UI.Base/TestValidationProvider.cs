using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestValidationProvider
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestValidationProviderWin : TestValidationProvider
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
        }

        [TestFixture]
        public class TestValidationProviderGiz : TestValidationProvider
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
        }

        [Test]
        public void TestOneValidationRule()
        {
            //---------------Set up test pack-------------------
            ValidationProvider validationProvider = new ValidationProvider(GetControlFactory().CreateErrorProvider());
            ITextBox textBox1 = GetControlFactory().CreateTextBox();
            ValidationRule validationRule1 = new ValidationRule();
            //---------------Execute Test ----------------------
            validationProvider.SetValidationRule(textBox1,validationRule1);
            //---------------Test Result -----------------------
            ValidationRule textBox1Rule = validationProvider.GetValidationRules(textBox1)[0];
            Assert.AreEqual(1,validationProvider.GetValidationRules(textBox1).Count);
           // Assert.AreEqual(validationRule1,textBox1Rule);
            //---------------Tear down -------------------------
        }
    }
}
