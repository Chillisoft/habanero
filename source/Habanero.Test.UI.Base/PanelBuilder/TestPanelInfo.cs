using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.PanelBuilder
{
    [TestFixture]
    public class TestPanelInfo 
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [Test]
        public void TestPanel()
        {
            //---------------Set up test pack-------------------
            IPanelInfo panelInfo = new PanelInfo();
            IPanel panel = new ControlFactoryWin().CreatePanel();
            //---------------Assert Precondition----------------
            Assert.IsNull(panelInfo.Panel);
            //---------------Execute Test ----------------------
            panelInfo.Panel = panel;
            //---------------Test Result -----------------------
            Assert.AreSame(panel, panelInfo.Panel);

        }
       
    }
    
    public class PanelInfo : IPanelInfo
    {
        private IPanel _panel;

        public IPanel Panel
        {
            get { return _panel; }
            set { _panel = value; }
        }
    }

    public interface IPanelInfo
    {
        IPanel Panel { get; set; }
    }
}