using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;

using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestEditableGridControlMapper
    {
        protected abstract IControlFactory GetControlFactory();

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
            RelatedBusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;
            //---------------Assert PreConditions---------------        
            Assert.IsNull(mapper.BusinessObject);

            //---------------Execute Test ----------------------
            mapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, mapper.BusinessObject);
            Assert.AreSame(addresses, editableGrid.BusinessObjectCollection);
        }

        [Test]
        public void Test_BusinessObject_WhenNull()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl editableGrid = GetControlFactory().CreateEditableGridControl();

            const string propName = "Addresses";
            EditableGridControlMapper mapper = new EditableGridControlMapper(editableGrid, propName, false, GetControlFactory());
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            mapper.BusinessObject = contactPersonTestBO;
            RelatedBusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;
            //---------------Assert PreConditions---------------        
            Assert.AreSame(contactPersonTestBO, mapper.BusinessObject);
            Assert.AreSame(addresses, editableGrid.BusinessObjectCollection);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.AreSame(null, editableGrid.BusinessObjectCollection);
        }

        [Test]
        public void Test_BusinessObject_WhenNullInitialValue()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl editableGrid = GetControlFactory().CreateEditableGridControl();

            const string propName = "Addresses";
            EditableGridControlMapper mapper = new EditableGridControlMapper(editableGrid, propName, false, GetControlFactory());
            //---------------Assert PreConditions---------------        
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.AreSame(null, editableGrid.BusinessObjectCollection);
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
        }
    }


}