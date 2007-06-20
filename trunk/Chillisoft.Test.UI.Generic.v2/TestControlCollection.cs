using System.Windows.Forms;
using Habanero.Ui.Generic;
using NUnit.Framework;

namespace Chillisoft.Test.UI.Generic.v2
{
    [TestFixture]
    public class TestControlCollection
    {
        [Test]
        public void TestAddControl()
        {
            ControlCollection col = new ControlCollection();
            Control ctl = new Control();
            col.Add(ctl);
            Assert.AreSame(ctl, col[0], "Control added should be the same object.");
        }

        [Test]
        public void TestAddNull()
        {
            ControlCollection col = new ControlCollection();
            col.Add(null);
        }
    }
}