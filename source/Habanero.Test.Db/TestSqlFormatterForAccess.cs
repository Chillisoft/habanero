using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestSqlFormatterForAccess
    {
        [Test]
        public void Test_PrepareValue_WithBool_WhenTrue_ShouldReturnNeg1()
        {
            //---------------Set up test pack-------------------
            SqlFormatterForAccess sqlFormatter = new SqlFormatterForAccess("", "", "", "");
            const bool value = true;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object preparedValue = sqlFormatter.PrepareValue(value);
            //---------------Test Result -----------------------
            Assert.AreEqual(-1, preparedValue, "PrepareValue is not preparing bools correctly for Access.");
        }

        [Test]
        public void Test_PrepareValue_WithBool_WhenFalse_ShouldreturnZero()
        {
            //---------------Set up test pack-------------------
            SqlFormatterForAccess sqlFormatter = new SqlFormatterForAccess("", "", "", "");
            const bool value = false;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object preparedValue = sqlFormatter.PrepareValue(value);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, preparedValue, "PrepareValue is not preparing bools correctly for Access.");
        }
    }
}