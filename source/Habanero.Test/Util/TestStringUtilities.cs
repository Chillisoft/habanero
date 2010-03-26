// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections.Specialized;
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
            Assert.AreEqual("TEST String", output);

            output = StringUtilities.DelimitPascalCase("TestSTRING", " ");
            Assert.AreEqual("Test STRING", output);

            output = StringUtilities.DelimitPascalCase("TESTSTRING ", " ");
            Assert.AreEqual("TESTSTRING ", output);

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

            output = StringUtilities.DelimitPascalCase(" smallTest123IDString ", " ");
            Assert.AreEqual(" small Test 123 ID String ", output);
        }

        [Test]
        public void TestIdNumberUtilities()
        {
            new IdNumberUtilities();
            DateTime testDate = new DateTime(2007, 1, 1);
            Assert.AreEqual(testDate, IdNumberUtilities.GetDateOfBirth("070101"));
        }

        [Test, ExpectedException(typeof (FormatException))]
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
            GuidTryParseExpectation
                ("{0xCA761232, 0xED42, 0x11CE, {0xBA, 0xCD, 0x00, 0xAA, 0x00, 0x57, 0xB2, 0x23}}", true);
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
            GuidTryParseExpectation
                ("{0xCA761232, 0xED42, 0x11CE, {0xBA, 0xCD, 0x00, 0xAA, 0x00, 0x57, 0xB2, 0x2Z}}", false);
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
            }
            catch (Exception)
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

        [Test]
        public void TestGetLeftSectionException()
        {
            string test = "abcdef";
            Assert.AreEqual("", StringUtilities.GetLeftSection(test, "g"));
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

        [Test]
        public void TestGetRightSectionException()
        {
            string test = "abcdef";
            Assert.AreEqual("", StringUtilities.GetRightSection(test, "g"));
        }

        [Test]
        public void TestCountOccurences_String()
        {
            string test = "I say hello to you: 'Hello to you'. Did you hear me say hello to you?";
            Assert.AreEqual(2, StringUtilities.CountOccurrences(test, "hello to you"));
            Assert.AreEqual(1, StringUtilities.CountOccurrences(test, "Hello to you"));
            Assert.AreEqual(4, StringUtilities.CountOccurrences(test, "you"));
            Assert.AreEqual(2, StringUtilities.CountOccurrences(test, "'"));
            Assert.AreEqual(0, StringUtilities.CountOccurrences(test, "not found"));
        }

        [Test]
        public void TestCountOccurences_Char()
        {
            string test = "I say hello to you: 'Hello to you'. Did you hear me say hello to you?";
            Assert.AreEqual(1, StringUtilities.CountOccurrences(test, "I"));
            Assert.AreEqual(1, StringUtilities.CountOccurrences(test, "i"));
            Assert.AreEqual(5, StringUtilities.CountOccurrences(test, "e"));
            Assert.AreEqual(2, StringUtilities.CountOccurrences(test, "'"));
            Assert.AreEqual(1, StringUtilities.CountOccurrences(test, "?"));
            Assert.AreEqual(0, StringUtilities.CountOccurrences(test, "#"));
        }

        [Test]
        public void Test_TestGetUserID()
        {
            //---------------Set up test pack-------------------
            string strCookieString =
                "FullName=Super User&UserID=3&DepartmentID=2&StakeholderID=5&StakeholderPrefix=No St";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string userIDStr = StringUtilities.GetValueString(strCookieString, "UserID");
            //---------------Test Result -----------------------
            Assert.AreEqual("3", userIDStr);
        }

        [Test]
        public void Test_TestGetUserID_DiffUserId()
        {
            //---------------Set up test pack-------------------
            string strCookieString =
                "FullName=Super User&UserID=5&DepartmentID=2&StakeholderID=5&StakeholderPrefix=No St";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string userIDStr = StringUtilities.GetValueString(strCookieString, "UserID");
            //---------------Test Result -----------------------
            Assert.AreEqual("5", userIDStr);
        }

        [Test]
        public void Test_TestGetStakeholderID()
        {
            //---------------Set up test pack-------------------
            string strCookieString =
                "FullName=Super User&UserID=3&DepartmentID=2&StakeholderID=3&StakeholderPrefix=No St";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string stakeholderIDStr = StringUtilities.GetValueString(strCookieString, "StakeholderID");
            //---------------Test Result -----------------------
            Assert.AreEqual("3", stakeholderIDStr);
        }

        [Test]
        public void Test_TestGetStakeholderID_DiffStakeholderID()
        {
            //---------------Set up test pack-------------------
            string strCookieString =
                "FullName=Super User&UserID=5&DepartmentID=2&StakeholderID=5&StakeholderPrefix=No St";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string stakeholderIDStr = StringUtilities.GetValueString(strCookieString, "StakeholderID");
            //---------------Test Result -----------------------
            Assert.AreEqual("5", stakeholderIDStr);
        }

        [Test]
        public void Test_GetNameValuePairCollection()
        {
            //---------------Set up test pack-------------------
            string strCookieString =
                "FullName=Super User&UserID=5&DepartmentID=2&StakeholderID=7&StakeholderPrefix=No St";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            NameValueCollection col = StringUtilities.GetNameValueCollection(strCookieString);
            //---------------Test Result -----------------------
            Assert.AreEqual(5, col.Count);
            Assert.AreEqual("5", col["UserID"]);
            Assert.AreEqual("7", col["StakeholderID"]);
        }

        [Test]
        public void TestGetNameValueCollection_ZeroLengthString()
        {
            //---------------Set up test pack-------------------
            //---------------Set up test pack-------------------
            string strCookieString = "";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            NameValueCollection col = StringUtilities.GetNameValueCollection(strCookieString);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, col.Count);
        }
    }
}