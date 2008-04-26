using System.Collections.Generic;
using Gizmox.WebGUI.Common.Interfaces;
using Win = System.Windows.Forms;
using Giz = Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.UI.Base
{
    [TestFixture]
    public class TestFilterControlBase
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }

        [Test]
        public void TestAddTextBoxWin()
        {
            //---------------Set up test pack-------------------
            IChilliFilterControl ctl = new FilterControlWin();

            //---------------Execute Test ----------------------
            IChilliTextBox myTextBox = ctl.AddTextBox();
            //---------------Test Result -----------------------
            Assert.IsNotNull(myTextBox);
            Assert.AreEqual(1, ctl.ChilliControls.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddTextBoxGizmox()
        {
            //---------------Set up test pack-------------------
            IChilliFilterControl ctl = new FilterControlGizmox();

            //---------------Execute Test ----------------------
            IChilliTextBox myTextBox = ctl.AddTextBox();
            //---------------Test Result -----------------------
            Assert.IsNotNull(myTextBox);
            Assert.AreEqual(1, ctl.ChilliControls.Count);
            //---------------Tear Down -------------------------          
        }

        //[Test]
        //public void TestAddTextBoxGeneral()
        //{
        //    //---------------Set up test pack-------------------
        //    FilterControl ctl = new FilterControl();
        //    //---------------Execute Test ----------------------
        //    Chilli.TextBox myTextBox = ctl.AddTextBox();
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(myTextBox);
        //    Assert.AreEqual(1, ctl.ChilliControls.Count);
        //    //---------------Tear Down -------------------------          
        //}
    }

    internal interface IChilliFilterControl : IChilliControl
    {
        IChilliTextBox AddTextBox();
    }

    internal interface IChilliControl
    {
        IList<IChilliControl> ChilliControls { get; }
    }

    internal class FilterControl 
    {
        private readonly IList<IChilliControl> _controls = new List<IChilliControl>();
        //private ChilliTextBox AddTextBox()
        //{
        //    ChilliTextBox tb = CreateTextBox();
        //    this.ChilliControls.Add(tb);
        //    return tb;
        //}

        //protected ChilliTextBox CreateTextBox();

        public IList<IChilliControl> ChilliControls
        {
            get { return _controls; }
        }
    }

    internal class FilterControlGizmox : Gizmox.WebGUI.Forms.UserControl, IChilliFilterControl
    {
        private FilterControl _filterControl = new FilterControl();


        public IChilliTextBox AddTextBox()
        {
            IChilliTextBox tb = new ChilliTextBoxGiz();
            this.ChilliControls.Add(tb);
            return tb;
        }

        public IList<IChilliControl> ChilliControls
        {
            get { return _filterControl.ChilliControls; }
        }
    }

    internal class ChilliTextBoxGiz : Giz.TextBox, IChilliTextBox
    {
        private readonly ChilliTextBox _tb = new ChilliTextBox();

        public ChilliTextBoxGiz()
        {
            //Text = "My Textbox";
        }
        //public string Text
        //{
        //    get
        //    {
        //        return _tb.Text;
        //    }
        //}

        IList<IChilliControl> IChilliControl.ChilliControls
        {
            get { return _tb.ChilliControls; }
        }
    }

    internal class FilterControlWin : System.Windows.Forms.UserControl, IChilliFilterControl

    {
        private readonly FilterControl _filterControl = new FilterControl();

        //private ChilliTextBox AddTextBox()
        //{
        //    ChilliTextBox tb = new ChilliTextBoxWin();
        //    this.ChilliControls.Add(tb);
        //    return tb;
        //}

        public IChilliTextBox AddTextBox()
        {
            IChilliTextBox tb = new ChilliTextBoxWin();
            this.ChilliControls.Add(tb);
            return tb;
        }

        public IList<IChilliControl> ChilliControls
        {
            get { return _filterControl.ChilliControls; }
        }
    }

    internal class ChilliTextBoxWin : Win.TextBox, IChilliTextBox
    {
        private readonly ChilliTextBox _tb = new ChilliTextBox();

        public ChilliTextBoxWin()
        {
            //Text = "My Textbox";
        }
        //public string Text
        //{
        //    get
        //    {
        //        return _tb.Text;
        //    }
        //}

        IList<IChilliControl> IChilliControl.ChilliControls
        {
            get { return _tb.ChilliControls; }
        }
    }

    internal class ChilliTextBox : ChilliControl
    {
    }

    internal interface IChilliTextBox : IChilliControl
    {
        string Text{ get;}
    }

    internal class ChilliControl
    {
        private readonly IList<IChilliControl> _controls = new List<IChilliControl>();
        public IList<IChilliControl> ChilliControls
        {
            get { return _controls; }
        }
    }
}