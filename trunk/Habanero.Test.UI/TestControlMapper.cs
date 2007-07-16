using System.Windows.Forms;
using Habanero.Test.General;
using Habanero.Ui.Forms;
using NUnit.Framework;

namespace Habanero.Test.Ui.BoControls
{
    /// <summary>
    /// Summary description for TestControlMapper.
    /// </summary>
    [TestFixture]
    public class TestControlMapper : TestUsingDatabase
    {
        TextBox tb;
        Shape sh;
        TextBoxMapper onceMapper;

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            base.SetupDBConnection();
        }

        [SetUp]
        public void Setup()
        {
            tb = new TextBox();
            onceMapper = new TextBoxMapper(tb, "ShapeName", true);
            sh = new Shape();
            sh.ShapeName = "TestShapeName";
        }

        [Test]
        public void TestCreateMapper()
        {
            TextBox b = new TextBox();
            ControlMapper mapper = ControlMapper.Create("TextBoxMapper", "", b, "Test", false);
            Assert.AreSame(typeof (TextBoxMapper), mapper.GetType());
            Assert.AreSame(b, mapper.Control);
        }

        [Test]
        public void TestCreateMapperWithAssembly()
        {
            TextBox b = new TextBox();
            ControlMapper mapper = ControlMapper.Create("Habanero.UI.Forms.TextBoxMapper", "Habanero.UI.Pro", b, "Test", false);
            Assert.AreSame(typeof(TextBoxMapper), mapper.GetType());
            Assert.AreSame(b, mapper.Control);
        }

        [Test]
        public void TestReadOnceOnlyDisablesControl()
        {
            Assert.IsFalse(tb.Enabled);
        }

        [Test]
        public void TestReadOnceOnlyChangeValue()
        {
            onceMapper.BusinessObject = sh;
            Assert.AreEqual("TestShapeName", tb.Text);
            sh.ShapeName = "TestShapeName2";
            Assert.AreEqual("TestShapeName", tb.Text);
        }

        [Test]
        public void TestReadOnceOnlyChangeBO()
        {
            onceMapper.BusinessObject = sh;
            Assert.AreEqual("TestShapeName", tb.Text);
            Shape sh2 = new Shape();
            sh2.ShapeName = "Different";
            onceMapper.BusinessObject = sh2;
            Assert.AreEqual("Different", tb.Text);
            sh2.ShapeName = "Different2";
            Assert.AreEqual("Different", tb.Text);
        }
    }
}