//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Collections.Generic;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    [TestFixture]
    public class TestHabaneroStringBuilder
    {
        #region Test RemoveQuotedSections

        [Test]
        public void TestRemoveQuotedSections_SingleQuotes()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder(
                "A quoted 'test' is needed to test this functionality");
            s.RemoveQuotedSections();
            Assert.AreEqual("A quoted  is needed to test this functionality", s.ToString());
        }

        [Test]
        public void TestRemoveQuotedSections_DoubleQuotes()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder(
                "A quoted \"test\" is needed to test this functionality");
            s.RemoveQuotedSections();
            Assert.AreEqual("A quoted  is needed to test this functionality", s.ToString());
        }

        [Test]
        public void TestRemoveQuotedSections_MixedQuotes()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder(
                "A quoted \"test\" is needed to 'test' this functionality");
            s.RemoveQuotedSections();
            Assert.AreEqual("A quoted  is needed to  this functionality", s.ToString());
        }

        [Test]
        public void TestRemoveQuotedSections_SimpleCriteria()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder("Name = 'Peter'");
            s.RemoveQuotedSections();
            Assert.AreEqual("Name = ", s.ToString());
        }

        [Test, Ignore("This needs to be fixed some time.")]
        public void TestRemoveQuotedSections_EmptyQuotes()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder("testProp = ''");
            s.RemoveQuotedSections();
            Assert.AreEqual("testProp = ", s.ToString());
        }

        [Test]
        public void TestRemoveQuotedSections_QuotedQuotes()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder("Description = 'Mark''s Car'");
            s.RemoveQuotedSections();
            Assert.AreEqual("Description = ", s.ToString());
        }

        #endregion //Test RemoveQuotedSections

        [Test]
        public void TestPutBackQuotedSections()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder("A quoted  is needed to test this functionality");
            s.QuotedSections = new List<HabaneroStringBuilder.QuotedSection>();
            s.QuotedSections.Add(new HabaneroStringBuilder.QuotedSection(9, "'test'"));
            s.PutBackQuotedSections();
            Assert.AreEqual("A quoted 'test' is needed to test this functionality", s.ToString());
            s = new HabaneroStringBuilder("A quoted  is needed to  this functionality");
            s.QuotedSections = new List<HabaneroStringBuilder.QuotedSection>();
            s.QuotedSections.Add(new HabaneroStringBuilder.QuotedSection(9, "'test'"));
            s.QuotedSections.Add(new HabaneroStringBuilder.QuotedSection(23, "\"test\""));
            s.PutBackQuotedSections();
            Assert.AreEqual("A quoted 'test' is needed to \"test\" this functionality", s.ToString());
            s = new HabaneroStringBuilder("A quoted  is needed to  this functionality");
            s.PutBackQuotedSections();
            Assert.AreEqual("A quoted  is needed to  this functionality", s.ToString());
        }

        [Test]
        public void TestRemoveAndPutBackQuotedSections()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder("A quoted 'test' is needed to test this functionality");
            s.RemoveQuotedSections();
            Assert.AreEqual("A quoted  is needed to test this functionality", s.ToString());
            s.PutBackQuotedSections();
            Assert.AreEqual("A quoted 'test' is needed to test this functionality", s.ToString());
        }

        [Test]
        public void TestRemoveAndPutBackQuotedSectionsAdvanced()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder(
                "Peter''s car''s engine said: 'That''s Mark''s Car' and 'That''s Eric''s car'.");
            s.RemoveQuotedSections();
            Assert.AreEqual("Peter's car's engine said:  and .", s.ToString());
            s.PutBackQuotedSections();
            Assert.AreEqual("Peter's car's engine said: 'That's Mark's Car' and 'That's Eric's car'.", s.ToString());
        }

        [Test]
        public void TestSubString()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder("Hello");
            Assert.AreEqual("el", s.Substring(1, 2).ToString(), "SubString returned invalid result.");
            Assert.AreEqual("llo", s.Substring(2).ToString());

            s = new HabaneroStringBuilder("A quoted  is needed to  this functionality");
            s.QuotedSections = new List<HabaneroStringBuilder.QuotedSection>();
            s.QuotedSections.Add(new HabaneroStringBuilder.QuotedSection(9, "'test'"));
            s.QuotedSections.Add(new HabaneroStringBuilder.QuotedSection(23, "\"test\""));
            HabaneroStringBuilder sub = s.Substring(9);
            Assert.AreEqual(" is needed to  this functionality", sub.ToString());
            HabaneroStringBuilder sub2 = sub.Substring(10, 4);
            sub.PutBackQuotedSections();
            Assert.AreEqual("'test' is needed to \"test\" this functionality", sub.ToString());
            sub2.PutBackQuotedSections();
            Assert.AreEqual(" to \"test\"", sub2.ToString());
        }

        [Test]
        public void TestIndexOf()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder("Hello");
            Assert.AreEqual(2, s.IndexOf("l"));
            s = new HabaneroStringBuilder("This is a 'test'");
            s.RemoveQuotedSections();
            Assert.AreEqual(-1, s.IndexOf("e"));
        }

        [Test]
        public void TestDropOuterQuotes()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder("'Test'");
            Assert.AreEqual("Test", s.DropOuterQuotes().ToString());
        }

        [Test]
        public void TestDoubleQuotes()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder("Hi There Peter, what''s going on");
            Assert.AreEqual("Hi There Peter, what's going on", s.RemoveQuotedSections().ToString());
            Assert.AreEqual("Hi There Peter, what's going on", s.PutBackQuotedSections().ToString());

            s = new HabaneroStringBuilder("Hi There Peter, 'what''s going on'");
            Assert.AreEqual("Hi There Peter, ", s.RemoveQuotedSections().ToString());
            Assert.AreEqual("Hi There Peter, 'what's going on'", s.PutBackQuotedSections().ToString());

            s = new HabaneroStringBuilder("Installation,Pipeclamps,'MP HI 1/4''',4.04");
            Assert.AreEqual("Installation,Pipeclamps,,4.04", s.RemoveQuotedSections().ToString());
            Assert.AreEqual("Installation,Pipeclamps,'MP HI 1/4'',4.04", s.PutBackQuotedSections().ToString());
        }
    }
}
