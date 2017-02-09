using Habanero.Base.Data;
using NUnit.Framework;

namespace Habanero.Test.Base.Data
{
    [TestFixture]
    public class TestQueryResultField
    {
        [Test]
        public void ConstructField()
        {
            //---------------Set up test pack-------------------
            var propName = TestUtil.GetRandomString();
            var index = TestUtil.GetRandomInt();
            //---------------Execute Test ----------------------
            var field = new QueryResultField(propName, index);
            //---------------Test Result -----------------------
            Assert.AreEqual(propName, field.PropertyName);
            Assert.AreEqual(index, field.Index);
        }
    }
}