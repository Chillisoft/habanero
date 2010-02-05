using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;


using NUnit.Framework;

namespace Habanero.Test.UI.Base.Mappers
{
    public abstract class TestEnumComboBoxMapper
    {
        protected const string ENUM_PROP_NAME = "EnumProp";
        private const string ENUM_PROP_NAME_EMPTY = "EnumPropEmpty";
        private const string ENUM_PROP_NAME_PASCAL = "EnumPropPascal";
        private const string ENUM_PKPROP_NAME = "EnumPropPK";

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.ClassDefs.Add((IClassDef) GetClassDef());
        }

        protected abstract EnumComboBoxMapper CreateComboBox(string propertyName, bool setBO);

        [Test]
        public void Test_SetupComboBoxItems_BONotSet_ThrowsException()
        {
            //---------------Set up test pack-------------------
            EnumComboBoxMapper enumComboBoxMapper = CreateComboBox(ENUM_PROP_NAME, false);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                enumComboBoxMapper.SetupComboBoxItems();
                //---------------Test Result -----------------------
                Assert.Fail("Expected to throw an InvalidOperationException");
            } 
            catch (InvalidOperationException ex)
            {
                StringAssert.Contains("The BusinessObject must be set on the EnumComboBoxMapper before calling SetupComboBoxItems", ex.Message);
            }
            //---------------Test Result -----------------------

        }

        [Test]
        public void Test_SetupComboBoxItems_PopulatesComboBoxWithEnum()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            EnumComboBoxMapper enumComboBoxMapper = CreateComboBox(ENUM_PROP_NAME, true);
            IComboBox comboBox = (IComboBox) enumComboBoxMapper.Control;
            //---------------Test Result -----------------------
            Assert.AreEqual(4, comboBox.Items.Count);
            Assert.AreEqual("", comboBox.Items[0].ToString());
            Assert.AreEqual("Option 1", comboBox.Items[1].ToString());
            Assert.AreEqual("Option 2", comboBox.Items[2].ToString());
            Assert.AreEqual("Option 3", comboBox.Items[3].ToString());
        }

        [Test]
        public void Test_SetupComboBoxItems_PopulatesComboBoxWithEnum_ClearsItemsOnRepopulation()
        {
            //---------------Set up test pack-------------------
            EnumComboBoxMapper enumComboBoxMapper = CreateComboBox(ENUM_PROP_NAME, true);
            IComboBox comboBox = (IComboBox)enumComboBoxMapper.Control;
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, comboBox.Items.Count);
            //---------------Execute Test ----------------------
            enumComboBoxMapper.SetupComboBoxItems();
            //---------------Test Result -----------------------
            Assert.AreEqual(4, comboBox.Items.Count);
            Assert.AreEqual("", comboBox.Items[0].ToString());
            Assert.AreEqual("Option 1", comboBox.Items[1].ToString());
            Assert.AreEqual("Option 2", comboBox.Items[2].ToString());
            Assert.AreEqual("Option 3", comboBox.Items[3].ToString());
        }

        [Test]
        public void Test_SetupComboBoxItems_PopulatesComboBoxWithSpacedEnum()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            EnumComboBoxMapper enumComboBoxMapper = CreateComboBox(ENUM_PROP_NAME_PASCAL, true);
            IComboBox comboBox = (IComboBox)enumComboBoxMapper.Control;
            //---------------Test Result -----------------------
            Assert.AreEqual(5, comboBox.Items.Count);
            Assert.AreEqual("", comboBox.Items[0].ToString());
            Assert.AreEqual("Simple", comboBox.Items[1].ToString());
            Assert.AreEqual("Double Double", comboBox.Items[2].ToString());
            Assert.AreEqual("Number 11", comboBox.Items[3].ToString());
            Assert.AreEqual("Class ID", comboBox.Items[4].ToString());
        }

        [Test]
        public void Test_SetupComboBoxItems_PopulatesComboBoxWithEmptyEnum_StillWorksFine()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, Enum.GetNames(typeof(TestEnumEmpty)).Length);
            //---------------Execute Test ----------------------
            EnumComboBoxMapper enumComboBoxMapper = CreateComboBox(ENUM_PROP_NAME_EMPTY, true);
            IComboBox comboBox = (IComboBox)enumComboBoxMapper.Control;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, comboBox.Items.Count);
            Assert.AreEqual("", comboBox.Items[0].ToString());
        }

        [Test]
        public void Test_SetupComboBoxItems_ExceptionThrownIfPropertyTypeNotEnum()
        {
            //---------------Set up test pack-------------------
            EnumComboBoxMapper enumComboBoxMapper = CreateComboBox(ENUM_PKPROP_NAME, false);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                enumComboBoxMapper.BusinessObject = new EnumBO();
                //---------------Test Result -----------------------
                Assert.Fail("Expected to throw an InvalidPropertyException");
            }
            catch (InvalidPropertyException ex)
            {
                StringAssert.Contains("EnumComboBoxMapper can only be used for an enum property type", ex.Message);
            }
        }

        [Test]
        public void Test_InternalUpdateControlValueFromBo_IfComboBoxNotSetup_CallSetUpComboBoxItems()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            EnumComboBoxMapper enumComboBoxMapper = CreateComboBox(ENUM_PROP_NAME, true);
            IComboBox comboBox = (IComboBox)enumComboBoxMapper.Control;
            //---------------Test Result -----------------------
            Assert.AreEqual(4, comboBox.Items.Count);
        }


        private static ClassDef GetClassDef()
        {
            PropDef propDefPK = new PropDef(ENUM_PKPROP_NAME, typeof(Guid), PropReadWriteRule.WriteNew, null);
            PropDef propDef = new PropDef(ENUM_PROP_NAME, typeof(TestEnum), PropReadWriteRule.ReadWrite, TestEnum.Option1);
            PropDef propDef2 = new PropDef(ENUM_PROP_NAME_EMPTY, typeof(TestEnumEmpty), PropReadWriteRule.ReadWrite, null);
            PropDef propDef3 = new PropDef(ENUM_PROP_NAME_PASCAL, typeof(TestEnumPascalCase), PropReadWriteRule.ReadWrite, null);
            PrimaryKeyDef primaryKeyDef = new PrimaryKeyDef { propDefPK };
            PropDefCol propDefCol = new PropDefCol { propDefPK, propDef, propDef2, propDef3 };

            UIFormField uiFormField = new UIFormField(TestUtil.GetRandomString(), propDef.PropertyName,
                                                      typeof(IComboBox), "EnumComboBoxMapper", "Habanero.UI.Base", true, null, null, LayoutStyle.Label);
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

        public enum TestEnumEmpty
        {

        }

        public enum TestEnumPascalCase
        {
            Simple,
            DoubleDouble,
            Number11,
            ClassID
        }

        public class EnumBO : BusinessObject
        {
            public virtual TestEnum? EnumProp
            {
                get
                {
                    return ((TestEnum?)(base.GetPropertyValue(ENUM_PROP_NAME)));
                }
                set
                {
                    base.SetPropertyValue(ENUM_PROP_NAME, value);
                }
            }
        }
    }

  



}
