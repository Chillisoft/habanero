using Habanero.Test.UI.Base.Mappers;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Mappers
{
    [TestFixture]
    public class TestEnumComboBoxMapperWin : TestEnumComboBoxMapper
    {
        protected override EnumComboBoxMapper CreateComboBox(string propertyName, bool setBO)
        {
            EnumBO bo = new EnumBO();
            ComboBoxWin comboBox = new ComboBoxWin();
            IControlFactory controlFactory = new ControlFactoryWin();
            EnumComboBoxMapper enumComboBoxMapper = new EnumComboBoxMapper(comboBox, propertyName, false, controlFactory);
            if (setBO) enumComboBoxMapper.BusinessObject = bo;
            return enumComboBoxMapper;
        }

        [Test]
        public void Test_InternalUpdateControlValueFromBo_IfNull_SelectsBlank()
        {
            //---------------Set up test pack-------------------
            EnumComboBoxMapper enumComboBoxMapper = CreateComboBox(ENUM_PROP_NAME, true);
            IComboBox comboBox = (IComboBox)enumComboBoxMapper.Control;
            EnumBO enumBO = (EnumBO)enumComboBoxMapper.BusinessObject;
            enumComboBoxMapper.SetupComboBoxItems();
            //---------------Assert Precondition----------------
            Assert.AreEqual(-1, comboBox.SelectedIndex);
            //---------------Execute Test ----------------------
            enumBO.EnumProp = null;
            //---------------Test Result -----------------------
            Assert.AreEqual(0, comboBox.SelectedIndex);
        }

        [Test]
        public void Test_InternalUpdateControlValueFromBo_IfNotNull_SelectsNonBlank()
        {
            //---------------Set up test pack-------------------
            EnumComboBoxMapper enumComboBoxMapper = CreateComboBox(ENUM_PROP_NAME, true);
            IComboBox comboBox = (IComboBox)enumComboBoxMapper.Control;
            EnumBO enumBO = (EnumBO)enumComboBoxMapper.BusinessObject;
            enumComboBoxMapper.SetupComboBoxItems();
            //---------------Assert Precondition----------------
            Assert.AreEqual(-1, comboBox.SelectedIndex);
            //---------------Execute Test ----------------------
            enumBO.EnumProp = TestEnum.Option3;
            //---------------Test Result -----------------------
            Assert.AreEqual(3, comboBox.SelectedIndex);
        }

        [Test]
        public void Test_ApplyChangesToBusinessObject_SelectBlank_SetsPropertyToNull()
        {
            //---------------Set up test pack-------------------
            EnumComboBoxMapper enumComboBoxMapper = CreateComboBox(ENUM_PROP_NAME, true);
            IComboBox comboBox = (IComboBox)enumComboBoxMapper.Control;
            EnumBO enumBO = (EnumBO)enumComboBoxMapper.BusinessObject;
            enumComboBoxMapper.SetupComboBoxItems();
            enumBO.EnumProp = TestEnum.Option3;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, comboBox.SelectedIndex);
            Assert.AreEqual((object) TestEnum.Option3, enumBO.EnumProp.Value);
            //---------------Execute Test ----------------------
            comboBox.SelectedIndex = 0;
            //---------------Test Result -----------------------
            Assert.IsFalse(enumBO.EnumProp.HasValue);
        }

        [Test]
        public void Test_ApplyChangesToBusinessObject_SelectMinusOne_SetsPropertyToNull()
        {
            //---------------Set up test pack-------------------
            EnumComboBoxMapper enumComboBoxMapper = CreateComboBox(ENUM_PROP_NAME, true);
            IComboBox comboBox = (IComboBox)enumComboBoxMapper.Control;
            EnumBO enumBO = (EnumBO)enumComboBoxMapper.BusinessObject;
            enumComboBoxMapper.SetupComboBoxItems();
            enumBO.EnumProp = TestEnum.Option3;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, comboBox.SelectedIndex);
            Assert.AreEqual((object) TestEnum.Option3, enumBO.EnumProp.Value);
            //---------------Execute Test ----------------------
            comboBox.SelectedIndex = -1;
            //---------------Test Result -----------------------
            Assert.IsFalse(enumBO.EnumProp.HasValue);
        }

        [Test]
        public void Test_ApplyChangesToBusinessObject_SelectNonBlank_SetsPropertyToNonNull()
        {
            //---------------Set up test pack-------------------
            EnumComboBoxMapper enumComboBoxMapper = CreateComboBox(ENUM_PROP_NAME, true);
            IComboBox comboBox = (IComboBox)enumComboBoxMapper.Control;
            EnumBO enumBO = (EnumBO)enumComboBoxMapper.BusinessObject;
            enumComboBoxMapper.SetupComboBoxItems();
            enumBO.EnumProp = null;
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, comboBox.SelectedIndex);
            Assert.IsFalse(enumBO.EnumProp.HasValue);
            //---------------Execute Test ----------------------
            comboBox.SelectedIndex = 3;
            //---------------Test Result -----------------------
            Assert.IsTrue(enumBO.EnumProp.HasValue);
            Assert.AreEqual((object) TestEnum.Option3, enumBO.EnumProp.Value);
        }
    }
}