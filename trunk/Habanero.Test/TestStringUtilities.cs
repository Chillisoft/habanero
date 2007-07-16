using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test
{
    [TestFixture]
    public class TestStringUtilities
    {
        [Test]
        public void TestDelimitPascalCase()
        {
            string output = StringUtilities.DelimitPascalCase("TestString", " ");
            Assert.AreEqual("Test String", output);

            output = StringUtilities.DelimitPascalCase("Test   String", " ");
            Assert.AreEqual("Test   String", output);

            output = StringUtilities.DelimitPascalCase("TestStringAgain", " ");
            Assert.AreEqual("Test String Again", output);

            output = StringUtilities.DelimitPascalCase("TESTString", " ");
            Assert.AreEqual("T E S T String", output);

            output = StringUtilities.DelimitPascalCase("TestSTRING", " ");
            Assert.AreEqual("Test S T R I N G", output);

            output = StringUtilities.DelimitPascalCase("TESTSTRING ", " ");
            Assert.AreEqual("T E S T S T R I N G ", output);

            output = StringUtilities.DelimitPascalCase("TestString", ",");
            Assert.AreEqual("Test,String", output);

            output = StringUtilities.DelimitPascalCase(" TestString ", " ");
            Assert.AreEqual(" Test String ", output);

            output = StringUtilities.DelimitPascalCase("smallTestString", " ");
            Assert.AreEqual("small Test String", output);

            output = StringUtilities.DelimitPascalCase("", " ");
            Assert.AreEqual("", output);
        }
    }
}
