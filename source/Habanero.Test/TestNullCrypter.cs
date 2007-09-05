using NUnit.Framework;
using Habanero.Base;

namespace Habanero.Test
{
    [TestFixture]
    public class TestNullCrypter
    {
        [Test]
        public void TestNullEncryption()
        {
            NullCrypter crypter = new NullCrypter();
            string value = "test";

            Assert.AreEqual("test", crypter.EncryptString(value));
            Assert.AreEqual("test", crypter.DecryptString(value));
        }
    }
}
