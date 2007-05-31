using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;
using NUnit.Framework;

namespace Chillisoft.Test.UI.Generic.v2
{
    /// <summary>
    /// Summary description for TestFilterInputBoxCollection.
    /// </summary>
    [TestFixture]
    public class TestFilterInputBoxCollection
    {
        [Test]
        public void TestNumberOfControls()
        {
            FilterInputBoxCollection filterInputBoxCol = new FilterInputBoxCollection(new DataViewFilterClauseFactory());
            Label testLabel = filterInputBoxCol.AddLabel("Test");
            Assert.AreEqual(1, filterInputBoxCol.GetControls().Count);

            TextBox testTextBox = filterInputBoxCol.AddStringFilterTextBox("a");
            Assert.AreEqual(2, filterInputBoxCol.GetControls().Count);

            Assert.AreSame(testLabel, filterInputBoxCol.GetControls()[0]);
            Assert.AreSame(testTextBox, filterInputBoxCol.GetControls()[1]);
        }
    }
}