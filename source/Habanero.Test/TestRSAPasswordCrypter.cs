using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Habanero.Base;
using NUnit.Framework;


namespace Habanero.Test
{
    [TestFixture]
    public class TestRSAPasswordCrypter
    {
        [Test]
        public void TestEncrypt()
        {
            RSA rsa = RSA.Create();
            ICrypter crypter = new RSAPasswordCrypter(rsa);
            string encrypted = crypter.EncryptString("testmessage");
            Assert.AreEqual(256, encrypted.Length);
        }

        [Test]
        public void TestDecrypt()
        {
            RSA rsa = RSA.Create();
            ICrypter crypter = new RSAPasswordCrypter(rsa);
            string encrypted = crypter.EncryptString("testmessage");
            string decrypted = crypter.DecryptString(encrypted);
            Assert.AreEqual("testmessage", decrypted);
        }
    }
}
