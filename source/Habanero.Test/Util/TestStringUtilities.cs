//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.Base.Exceptions;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
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

            output = StringUtilities.DelimitPascalCase("Test3", " ");
            Assert.AreEqual("Test 3", output);

            output = StringUtilities.DelimitPascalCase("Test36", " ");
            Assert.AreEqual("Test 36", output);

            output = StringUtilities.DelimitPascalCase("365", " ");
            Assert.AreEqual("365", output);

            output = StringUtilities.DelimitPascalCase("", " ");
            Assert.AreEqual("", output);

            output = StringUtilities.DelimitPascalCase(null, " ");
            Assert.AreEqual(null, output);
        }

        [Test]
        public void TestIdNumberUtilities()
        {
            IdNumberUtilities idUtil = new IdNumberUtilities();
            DateTime testDate = new DateTime(2007,1,1);
            Assert.AreEqual(testDate, IdNumberUtilities.GetDateOfBirth("070101"));
        }

        [Test, ExpectedException(typeof(FormatException))]
        public void TestIdNumberUtilitiesException()
        {
            DateTime testDate = new DateTime(2007, 1, 1);
            Assert.AreEqual(testDate, IdNumberUtilities.GetDateOfBirth("07010"));
        }

        [Test]
        public void TestGuidTryParse()
        {
            // dddddddddddddddddddddddddddddddd
            GuidTryParseExpectation("ca761232ed4211cebacd00aa0057b223", true);

            // dddddddd-dddd-dddd-dddd-dddddddddddd
            GuidTryParseExpectation("CA761232-ED42-11CE-BACD-00AA0057B223", true);

            // {dddddddd-dddd-dddd-dddd-dddddddddddd}
            GuidTryParseExpectation("{CA761232-ED42-11CE-BACD-00AA0057B223}", true);

            // (dddddddd-dddd-dddd-dddd-dddddddddddd)
            GuidTryParseExpectation("(CA761232-ED42-11CE-BACD-00AA0057B223)", true);

            // {0xdddddddd, 0xdddd, 0xdddd,{0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd}}
            GuidTryParseExpectation("{0xCA761232, 0xED42, 0x11CE, {0xBA, 0xCD, 0x00, 0xAA, 0x00, 0x57, 0xB2, 0x23}}", true);
            GuidTryParseExpectation("{0xA,0xA,0xA,{0xA,0xA,0xA,0xA,0xA,0xA,0xA,0xA}}", true);
			
            // Expecting Failure
            GuidTryParseExpectation("testfail", false);
            GuidTryParseExpectation("123456", false);

            // Too short
            GuidTryParseExpectation("ca761232ed4211cebacd00aa0057b22", false);
            GuidTryParseExpectation("CA761232-ED42-11CE-BACD-00AA0057B22", false);
            GuidTryParseExpectation("{CA761232-ED42-11CE-BACD-00AA0057B22}", false);
            GuidTryParseExpectation("(CA761232-ED42-11CE-BACD-00AA0057B22)", false);
            GuidTryParseExpectation("{0xCA761232, 0xED42, 0x11CE, {0xBA, 0xCD, 0x00, 0xAA, 0x00, 0x57, 0xB2}}", false);
            GuidTryParseExpectation("{0xA,0xA,0xA,{0xA,0xA,0xA,0xA,0xA,0xA,0xA}}", false);

            // Invalid letter
            GuidTryParseExpectation("ca761232ed4211cebacd00aa0057b22z", false);
            GuidTryParseExpectation("CA761232-ED42-11CE-BACD-00AA0057B22Z", false);
            GuidTryParseExpectation("{CA761232-ED42-11CE-BACD-00AA0057B22Z}", false);
            GuidTryParseExpectation("(CA761232-ED42-11CE-BACD-00AA0057B22Z)", false);
            GuidTryParseExpectation("{0xCA761232, 0xED42, 0x11CE, {0xBA, 0xCD, 0x00, 0xAA, 0x00, 0x57, 0xB2, 0x2Z}}", false);
            GuidTryParseExpectation("{0xA,0xA,0xA,{0xA,0xA,0xA,0xA,0xA,0xA,0xA,0xZ}}", false);
        }

        private static void GuidTryParseExpectation(string testString, bool expectingSuccess)
        {
            bool isValid;
            Guid result;
            Guid resultValue;
            isValid = StringUtilities.GuidTryParse(testString, out result);
            if (expectingSuccess)
                Assert.IsTrue(isValid, "Parsing should succeed");
            else
                Assert.IsFalse(isValid, "Parsing should fail");
            try 
            {
                resultValue = new Guid(testString);
                Assert.IsTrue(expectingSuccess, "Conversion using constructor should succeed");
            } catch(Exception)
            {
                Assert.IsFalse(expectingSuccess, "Conversion using constructor should fail");
                resultValue = Guid.Empty;
            }
            if (expectingSuccess)
                Assert.AreEqual(result, resultValue, "Converted values should be equal");
        }

        [Test]
        public void TestCountOccurrences()
        {
            string test = "a";
            Assert.AreEqual(0, StringUtilities.CountOccurrences(test, "b"));
            Assert.AreEqual(1, StringUtilities.CountOccurrences(test, "a"));
            Assert.AreEqual(0, StringUtilities.CountOccurrences(test, "A"));

            test = "a bc a abc";
            Assert.AreEqual(3, StringUtilities.CountOccurrences(test, "a"));
            Assert.AreEqual(2, StringUtilities.CountOccurrences(test, "c"));
            Assert.AreEqual(3, StringUtilities.CountOccurrences(test, " "));
            Assert.AreEqual(1, StringUtilities.CountOccurrences(test, "abc"));
            Assert.AreEqual(2, StringUtilities.CountOccurrences(test, "bc"));
        }

        [Test]
        public void TestGetLeftSection()
        {
            string test = "abcdef";
            Assert.AreEqual("abc", StringUtilities.GetLeftSection(test, "d"));
            Assert.AreEqual("abcde", StringUtilities.GetLeftSection(test, "f"));
            Assert.AreEqual("a", StringUtilities.GetLeftSection(test, "bcdef"));
            Assert.AreEqual("", StringUtilities.GetLeftSection(test, "abcdef"));
        }

        [Test, ExpectedException(typeof(UserException))]
        public void TestGetLeftSectionException()
        {
            string test = "abcdef";
            StringUtilities.GetLeftSection(test, "g");
        }

        [Test]
        public void TestGetRightSection()
        {
            string test = "abcdef";
            Assert.AreEqual("ef", StringUtilities.GetRightSection(test, "d"));
            Assert.AreEqual("bcdef", StringUtilities.GetRightSection(test, "a"));
            Assert.AreEqual("f", StringUtilities.GetRightSection(test, "abcde"));
            Assert.AreEqual("", StringUtilities.GetRightSection(test, "abcdef"));
        }

        [Test, ExpectedException(typeof(UserException))]
        public void TestGetRightSectionException()
        {
            string test = "abcdef";
            StringUtilities.GetRightSection(test, "g");
        }
    }
}
