using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// These are tested in their own class since the windows and gizmox behaviour are
    /// very different.
    /// </summary>
    [TestFixture]
    public class TestControlMapperStrategyWin//:TestBase
    {
        [SetUp]
        public  void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }
        [TearDown]
        public  void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        [Test, Ignore("currently working on")]
        public void Test_CreateControlMapperStrategy()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            ControlMapperStrategyWin strategyWin = new ControlMapperStrategyWin();
            ControlFactoryWin factory = new Habanero.UI.Win.ControlFactoryWin();
            ITextBox tb = factory.CreateTextBox();
            ControlMapperStub stubMapper = new ControlMapperStub(tb, "PropName", false, factory);
            MyBO bo = new MyBO();
            IBOProp prop = bo.Props["TestProp"];
            string origvalue = "origValue";
            prop.Value = origvalue;
            stubMapper.BusinessObject = bo;

            strategyWin.AddCurrentBOPropHandlers(stubMapper, prop);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(origvalue, tb.Text);

            //---------------Execute Test ----------------------
            string newValue = "New value";
            prop.Value = newValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(newValue, tb.Text);

        }
    }
}
