using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Mappers
{
    [TestFixture]
    public class TestEnumComboBoxMapper
    {
        private const string ENUM_PROP_NAME = "EnumProp";
        private const string ENUM_PKPROP_NAME = "EnumPropPK";

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.ClassDefs.Add(GetClassDef());
        }

        [Test]
        public void Test_PopulatesComboBoxWithEnum()
        {
            //---------------Set up test pack-------------------
            EnumBO bo = new EnumBO();
            ComboBoxWin comboBox = new ComboBoxWin();
            IControlFactory controlFactory = new ControlFactoryWin();
            EnumComboBoxMapper enumComboBoxMapper = new EnumComboBoxMapper(comboBox, ENUM_PROP_NAME, false, controlFactory);
            enumComboBoxMapper.BusinessObject = bo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            enumComboBoxMapper.SetupComboBoxItems();
            //---------------Test Result -----------------------
            Assert.AreEqual(3, comboBox.Items.Count);
            Assert.AreEqual(TestEnum.Option1.ToString(), comboBox.Items[0].ToString());
            Assert.AreEqual(TestEnum.Option2.ToString(), comboBox.Items[1].ToString());
            Assert.AreEqual(TestEnum.Option3.ToString(), comboBox.Items[2].ToString());
        }

        [Test]
        public void Test_ExceptionThrownIfPropertyTypeNotEnum()
        {
            //---------------Set up test pack-------------------
            EnumBO bo = new EnumBO();
            ComboBoxWin comboBox = new ComboBoxWin();
            IControlFactory controlFactory = new ControlFactoryWin();
            EnumComboBoxMapper enumComboBoxMapper = new EnumComboBoxMapper(comboBox, ENUM_PKPROP_NAME, false, controlFactory);
            enumComboBoxMapper.BusinessObject = bo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                enumComboBoxMapper.SetupComboBoxItems();
                //---------------Test Result -----------------------
                Assert.Fail("Expected to throw an InvalidPropertyException");
            } 
            catch (InvalidPropertyException ex)
            {
                StringAssert.Contains("EnumComboBoxMapper can only be used for an enum property type", ex.Message);
            }
        }

        //TODO Eric 04 Aug 2009: implement other methods, add tests for catch scenarios/boundary conditions

        private static ClassDef GetClassDef()
        {
            PropDef propDefPK = new PropDef(ENUM_PKPROP_NAME, typeof(Guid), PropReadWriteRule.WriteNew, null);
            PropDef propDef = new PropDef(ENUM_PROP_NAME, typeof(TestEnum), PropReadWriteRule.ReadWrite, TestEnum.Option1);
            PrimaryKeyDef primaryKeyDef = new PrimaryKeyDef { propDefPK };
            PropDefCol propDefCol = new PropDefCol { propDefPK, propDef };

            UIFormField uiFormField = new UIFormField(TestUtil.GetRandomString(), propDef.PropertyName,
                typeof(ComboBox), "EnumComboBoxMapper", "Habanero.UI.Base", true, null, null, LayoutStyle.Label);
            UIFormColumn uiFormColumn = new UIFormColumn { uiFormField };
            UIFormTab uiFormTab = new UIFormTab { uiFormColumn };
            UIForm uiForm = new UIForm { uiFormTab };
            UIDef uiDef = new UIDef("default", uiForm, null);
            UIDefCol uiDefCol = new UIDefCol { uiDef };

            ClassDef classDef = new ClassDef(typeof(EnumBO), primaryKeyDef, propDefCol, new KeyDefCol(), null, uiDefCol);
            return classDef;
        }

        public enum TestEnum
        {
            Option1,
            Option2,
            Option3
        }

        public class EnumBO : BusinessObject
        {
            
        }
    }
}
