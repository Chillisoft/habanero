using System.Windows.Forms;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Db.v2;
using Chillisoft.Generic.v2;
using Chillisoft.Test.Setup.v2;
using Chillisoft.UI.BOControls.v2;
using Chillisoft.UI.Generic.v2;
using NMock;
using NUnit.Framework;
using BusinessObject=Chillisoft.Bo.v2.BusinessObject;

namespace Chillisoft.Test.UI.BOControls.v2
{
    /// <summary>
    /// Summary description for TestDefaultBOEditory.
    /// </summary>
    [TestFixture]
    public class TestDefaultBOEditorForm : TestUsingDatabase
    {
        private ClassDef itsClassDefMyBo;
        private BusinessObject itsBo;
        private DefaultBOEditorForm itsEditor;
        private Mock itsDatabaseConnectionMockControl;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            GlobalRegistry.UIExceptionNotifier = new ConsoleExceptionNotifier();
            this.SetupDBConnection();
            ClassDef.GetClassDefCol.Clear();
            itsClassDefMyBo = MyBo.LoadClassDefWithNoLookup();
        }

        [SetUp]
        public void SetupTest()
        {
            itsDatabaseConnectionMockControl = new DynamicMock(typeof (IDatabaseConnection));
            IDatabaseConnection conn = (IDatabaseConnection) itsDatabaseConnectionMockControl.MockInstance;
            itsBo = itsClassDefMyBo.CreateNewBusinessObject(conn);
            itsEditor = new DefaultBOEditorForm(itsBo);
        }

        [Test]
        public void TestLayout()
        {
            Assert.AreEqual(2, itsEditor.Controls.Count);
            Control boCtl = itsEditor.Controls[0];
            Assert.AreEqual(4, boCtl.Controls.Count);
            Control buttonControl = itsEditor.Controls[1];
            Assert.AreSame(typeof (ButtonControl), buttonControl.GetType());
            Assert.AreEqual(2, buttonControl.Controls.Count);
        }

        [Test]
        public void TestSuccessfulEdit()
        {
            itsEditor.Show();
            itsBo.SetPropertyValue("TestProp", "TestValue");
            itsBo.SetPropertyValue("TestProp2", "TestValue2");
            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});
            itsEditor.Buttons.ClickButton("OK");
            Assert.IsFalse(itsEditor.Visible);
            Assert.AreEqual(DialogResult.OK, itsEditor.DialogResult);
            Assert.AreEqual("TestValue", itsBo.GetPropertyValue("TestProp"));
            itsEditor.Dispose();
        }

        [Test]
        public void TestUnsuccessfulEdit()
        {
            itsEditor.Show();
            itsBo.SetPropertyValue("TestProp", "TestValue");
            itsBo.SetPropertyValue("TestProp2", "TestValue2");
            itsEditor.Buttons.ClickButton("Cancel");
            Assert.IsFalse(itsEditor.Visible);
            Assert.AreEqual(DialogResult.Cancel, itsEditor.DialogResult);
            Assert.AreEqual("", itsBo.GetPropertyValue("TestProp"), itsBo.GetPropertyValue("TestProp").ToString());
            itsEditor.Dispose();
        }
    }
}