using System.Windows.Forms;
using Habanero.Test.General;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.BoControls
{
    /// <summary>
    /// Summary description for TestCheckBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestCheckBoxMapper : TestMapperBase
    {
        private CheckBox cb;
        private CheckBoxMapper mapper;
        private Sample s;


        [SetUp]
        public void SetupTest()
        {
            cb = new CheckBox();
            mapper = new CheckBoxMapper(cb, "SampleBoolean", false);
            s = new Sample();
        }


        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(cb, mapper.Control);
            Assert.AreSame("SampleBoolean", mapper.PropertyName);
        }

        [Test]
        public void TestCheckBoxValue()
        {
            s.SampleBoolean = false;
            mapper.BusinessObject = s;
            Assert.IsFalse(cb.Checked);
            s.SampleBoolean = true;
            Assert.IsTrue(cb.Checked);
        }

        [Test]
        public void TestSettingCheckBoxCheckedUpdatesBO()
        {
            s.SampleBoolean = false;
            mapper.BusinessObject = s;
            cb.Checked = true;
            Assert.IsTrue(s.SampleBoolean);
        }

        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs(true);
            cb = new CheckBox();
            mapper = new CheckBoxMapper(cb, "MyRelationship.MyRelatedTestProp", true);
            mapper.BusinessObject = itsMyBo;
            Assert.IsNotNull(mapper.BusinessObject);
            Assert.AreEqual(true, cb.Checked);
        }
    }
}