using System.Windows.Forms;
using Habanero.Base.Exceptions;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.DB;
using Habanero.Base;
using Habanero.Test;
using Habanero.UI.Forms;
using NMock;
using NUnit.Framework;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Test.Ui.BoControls
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
            ClassDef.ClassDefs.Clear();
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