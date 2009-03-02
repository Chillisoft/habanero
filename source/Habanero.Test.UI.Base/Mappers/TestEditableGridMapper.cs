using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TestTextBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestEditableGridControlMapper
    {

        protected virtual IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl editableGrid = GetControlFactory().CreateEditableGridControl();
            const string propName = "asdfa";

            //---------------Execute Test ----------------------
            EditableGridControlMapper mapper = new EditableGridControlMapper(editableGrid, propName, false, GetControlFactory());

            //---------------Test Result -----------------------
            Assert.AreSame(editableGrid, mapper.Control);
            Assert.AreEqual(propName, mapper.PropertyName);
            Assert.IsFalse(editableGrid.Buttons.Visible);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_BusinessObject()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl editableGrid = GetControlFactory().CreateEditableGridControl();

            const string propName = "Addresses";
            EditableGridControlMapper mapper = new EditableGridControlMapper(editableGrid, propName, false, GetControlFactory());
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();

            //---------------Assert PreConditions---------------        
            Assert.IsNull(mapper.BusinessObject);

            //---------------Execute Test ----------------------
            mapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------

            Assert.AreSame(contactPersonTestBO, mapper.BusinessObject);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_EditableGridIsPopulated()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl editableGrid = GetControlFactory().CreateEditableGridControl();
            const string propName = "Addresses";
            EditableGridControlMapper mapper = new EditableGridControlMapper(editableGrid, propName, false, GetControlFactory());
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_DeleteDoNothing(out address);

            //---------------Assert PreConditions---------------            
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);

            //---------------Execute Test ----------------------
            mapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------

            Assert.AreSame(contactPersonTestBO.Addresses, editableGrid.Grid.BusinessObjectCollection);
            //---------------Tear Down -------------------------          

        }
    }

    [TestFixture]
    public class TestEditableGridControlMapperVWG : TestEditableGridControlMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }

}