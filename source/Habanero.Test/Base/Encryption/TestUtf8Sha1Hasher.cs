using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Base.Encryption
{
    [TestFixture]
    public class TestUtf8Sha1Hasher : TestBase64Sha1Hasher
    {
        protected override IHasher CreateHasher()
        {
            return new Utf8Sha1Hasher();
        }

        [Test]
        public void HashString_ShouldWorkWithSpaces()
        {
            //---------------Set up test pack-------------------
            var hasher = CreateHasher();
            //---------------Execute Test ----------------------
            var hash1 = hasher.HashString("abc def");
            //---------------Test Result -----------------------
        }

    }
}
