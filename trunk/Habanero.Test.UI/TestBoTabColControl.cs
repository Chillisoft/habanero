using System;
using System.Windows.Forms;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Test;
using Habanero.Ui.Base;
using Habanero.Ui.Forms;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.Ui.Application
{
    /// <summary>
    /// Summary description for TestBoTabColControl.
    /// </summary>
    [TestFixture]
    public class TestBoTabColControl : TestUsingDatabase
    {
        private BoTabColControl itsTabColControl;
        private ClassDef itsClassDef;
        private BusinessObjectCollection<BusinessObject> itsCol;
        private MyBo itsBo1;
        private MyBo itsBo2;

        [TestFixtureSetUp]
        public void SetupTextFixture()
        {
            ClassDef.ClassDefs.Clear();
            itsClassDef = MyBo.LoadDefaultClassDef();
            itsCol = new BusinessObjectCollection<BusinessObject>(itsClassDef);

			itsBo1 = MyBo.Create();
            itsBo2 = MyBo.Create();
            itsCol.Add(itsBo1);
            itsCol.Add(itsBo2);

            itsTabColControl = new BoTabColControl(new NullBusinessObjectControl());
            itsTabColControl.SetCollection(itsCol.GetList());
        }

        [
            Test,
                ExpectedException(typeof (ArgumentException),
                    "boControl must be of type Control or one of its subtypes.")]
        public void TestCheckForControlSubClass()
        {
            Mock mock = new DynamicMock(typeof (IBusinessObjectControl));
            IBusinessObjectControl mockBoControl = (IBusinessObjectControl) mock.MockInstance;
            BoTabColControl testControl = new BoTabColControl(mockBoControl);
        }

        [Test]
        public void TestNumberOfTabs()
        {
            Assert.AreEqual(2, itsTabColControl.TabControl.TabPages.Count);
        }

        [Test]
        public void TestCorrespondingBO()
        {
            Assert.AreSame(itsBo1, itsTabColControl.GetBo(itsTabColControl.TabControl.TabPages[0]));
            Assert.AreSame(itsBo2, itsTabColControl.GetBo(itsTabColControl.TabControl.TabPages[1]));
        }

        [Test]
        public void TestCorrespondingBONull()
        {
            Assert.IsNull(itsTabColControl.GetBo(new TabPage()));
        }


        [Test]
        public void TestGetBoWithNullTab()
        {
            Assert.IsNull(itsTabColControl.GetBo(null));
        }

        [Test]
        public void TestCorrespondingTabPage()
        {
            Assert.AreSame(itsTabColControl.TabControl.TabPages[0], itsTabColControl.GetTabPage(itsBo1));
            Assert.AreSame(itsTabColControl.TabControl.TabPages[1], itsTabColControl.GetTabPage(itsBo2));
        }

        [Test]
        public void TestSettingCollectionTwice()
        {
            BoTabColControl tabColControl = new BoTabColControl(new NullBusinessObjectControl());
            tabColControl.SetCollection(itsCol.GetList());
            tabColControl.SetCollection(itsCol.GetList());
            Assert.AreEqual(2, itsTabColControl.TabControl.TabPages.Count);
        }

        [Test]
        public void TestCurrentBusinessObject()
        {
            itsTabColControl.TabControl.SelectedTab = itsTabColControl.TabControl.TabPages[1];
            Assert.AreSame(itsBo2, itsTabColControl.CurrentBusinessObject);
            itsTabColControl.CurrentBusinessObject = itsBo1;
            Assert.AreSame(itsTabColControl.TabControl.TabPages[0], itsTabColControl.TabControl.SelectedTab);
        }


        private class NullBusinessObjectControl : Control, IBusinessObjectControl
        {
            public void SetBusinessObject(BusinessObject bo)
            {
            }
        }
    }
}