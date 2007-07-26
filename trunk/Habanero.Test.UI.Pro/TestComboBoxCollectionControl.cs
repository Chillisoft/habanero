using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.DB;
using Habanero.Base;
using Habanero.Test;
using Habanero.UI.Base;
using Habanero.UI.Forms;
using NMock;
using NUnit.Framework;
using BusinessObject=Habanero.BO.BusinessObject;

namespace Habanero.Test.UI.BoControls
{
    /// <summary>
    /// Summary description for TestComboBoxCollectionControl.
    /// </summary>
    [TestFixture]
    public class TestComboBoxCollectionControl : TestUsingDatabase
    {
        private ClassDef itsClassDef;
        private ComboBoxCollectionControl itsControl;
        private Mock itsConfirmerMockControl;
        private BusinessObjectCollection<BusinessObject> itsCollection;
        Mock itsDatabaseConnectionMockControl;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.SetupDBConnection();
        }

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            itsClassDef = MyBO.LoadDefaultClassDef();

            itsConfirmerMockControl = new DynamicMock(typeof(IConfirmer));
            IConfirmer confirmerMock = (IConfirmer)itsConfirmerMockControl.MockInstance;

            itsDatabaseConnectionMockControl = new DynamicMock(typeof (IDatabaseConnection));
            IDatabaseConnection databaseConnectionMock =
                (IDatabaseConnection) itsDatabaseConnectionMockControl.MockInstance;

            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});
            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});


            MyBO bo1 = (MyBO) itsClassDef.CreateNewBusinessObject(databaseConnectionMock);
            bo1.SetPropertyValue("TestProp", "abc");
            bo1.SetPropertyValue("TestProp2", "def");
            bo1.Save();

            MyBO bo2 = (MyBO) itsClassDef.CreateNewBusinessObject(databaseConnectionMock);
            bo2.SetPropertyValue("TestProp", "ghi");
            bo2.SetPropertyValue("TestProp2", "jkl");
            bo2.Save();

            itsCollection = new BusinessObjectCollection<BusinessObject>(itsClassDef);
            itsCollection.Add(bo1);
            itsCollection.Add(bo2);

            itsControl = new ComboBoxCollectionControl();
            itsControl.SetCollection(itsCollection);
            itsControl.Label = "Select MyBO:";
            itsControl.DatabaseConnection = databaseConnectionMock;
            itsControl.Confirmer = confirmerMock;
        }

        [TearDown]
        public void TearDownTest()
        {
            itsConfirmerMockControl.Verify();
            itsDatabaseConnectionMockControl.Verify();
        }

        [Test]
        public void TestControlSetup()
        {
            Assert.AreEqual(3, itsControl.CollectionComboBox.Items.Count);
            Assert.IsFalse(itsControl.BusinessObjectPanel.Enabled);
        }

        [Test]
        public void TestChangeSelectedIndex()
        {
            itsControl.CollectionComboBox.SelectedIndex = 1;
            Assert.IsTrue(itsControl.BusinessObjectPanel.Enabled);
            Assert.AreEqual(itsCollection[0].GetPropertyValue("TestProp"),
                            itsControl.PanelFactoryInfo.ControlMappers.BusinessObject.GetPropertyValue("TestProp"));
            itsControl.CollectionComboBox.SelectedIndex = 2;
            Assert.AreEqual(itsCollection[1].GetPropertyValue("TestProp"),
                            itsControl.PanelFactoryInfo.ControlMappers.BusinessObject.GetPropertyValue("TestProp"));
        }

        [Test]
        public void TestUpdateBusinessObjectWithConfirmTrue()
        {
            itsConfirmerMockControl.ExpectAndReturn("Confirm", true,
                                                    new object[] {"Do you want to want to save before moving on?"});
            itsControl.CollectionComboBox.SelectedIndex = 1;

            BusinessObject selectedBo = itsControl.SelectedBusinessObject;
            selectedBo.SetPropertyValue("TestProp", "xyz");
            Assert.IsTrue(selectedBo.State.IsDirty);

            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});

            itsControl.CollectionComboBox.SelectedIndex = 2;
            Assert.AreEqual(2, itsControl.CollectionComboBox.SelectedIndex);

            itsControl.CollectionComboBox.SelectedIndex = 1;
            Assert.AreEqual(1, itsControl.CollectionComboBox.SelectedIndex);

            Assert.IsFalse(selectedBo.State.IsDirty);
        }

        [Test]
        public void TestUpdateBusinessObjectWithConfirmFalse()
        {
            itsConfirmerMockControl.ExpectAndReturn("Confirm", false,
              new object[] {"Do you want to want to save before moving on?"});
            itsControl.CollectionComboBox.SelectedIndex = 1;

            BusinessObject selectedBo = itsControl.SelectedBusinessObject;
            selectedBo.SetPropertyValue("TestProp", "xyz");
            Assert.IsTrue(selectedBo.State.IsDirty);

            itsControl.CollectionComboBox.SelectedIndex = 2;
            Assert.AreEqual(1, itsControl.CollectionComboBox.SelectedIndex);
        }

        [Test]
        public void TestSaveButton()
        {
            itsControl.CollectionComboBox.SelectedIndex = 1;
            BusinessObject selectedBo = itsControl.SelectedBusinessObject;
            selectedBo.SetPropertyValue("TestProp", "xyz");

            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});
            itsControl.Buttons.ClickButton("Save");
            Assert.IsFalse(selectedBo.State.IsDirty);
        }

        [Test]
        public void TestCancelButton()
        {
            itsControl.CollectionComboBox.SelectedIndex = 1;
            BusinessObject selectedBo = itsControl.SelectedBusinessObject;
            selectedBo.SetPropertyValue("TestProp", "xyz");
            itsControl.Buttons.ClickButton("Cancel");
            Assert.AreEqual("abc", selectedBo.GetPropertyValue("TestProp"));
        }

        [Test]
        public void TestAddButtonWithSave()
        {
            itsControl.CollectionComboBox.SelectedIndex = 1;
            itsControl.Buttons.ClickButton("Add");
            Assert.IsTrue(itsControl.BusinessObjectPanel.Enabled);
            Assert.IsFalse(itsControl.CollectionComboBox.Enabled);
            Assert.IsFalse(itsControl.Buttons["Add"].Enabled);
            Assert.IsTrue(itsControl.SelectedBusinessObject.State.IsNew);
            itsControl.SelectedBusinessObject.SetPropertyValue("TestProp", "qwe");
            itsControl.SelectedBusinessObject.SetPropertyValue("TestProp2", "rty");
            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});
            itsControl.Buttons.ClickButton("Save");
            Assert.IsFalse(itsControl.SelectedBusinessObject.State.IsNew);
            Assert.IsTrue(itsControl.Buttons["Add"].Enabled);
            Assert.IsTrue(itsControl.CollectionComboBox.Enabled);
            Assert.AreEqual(4, itsControl.CollectionComboBox.Items.Count);
        }

        [Test]
        public void TestAddButtonWithCancel()
        {
            itsControl.CollectionComboBox.SelectedIndex = 1;
            itsControl.Buttons.ClickButton("Add");
            itsControl.Buttons.ClickButton("Cancel");
            Assert.IsTrue(itsControl.CollectionComboBox.Enabled);
            Assert.IsTrue(itsControl.Buttons["Add"].Enabled);
            Assert.AreEqual(3, itsControl.CollectionComboBox.Items.Count);
            Assert.AreEqual(-1, itsControl.CollectionComboBox.SelectedIndex);
        }
    }
}