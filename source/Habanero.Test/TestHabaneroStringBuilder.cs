using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test
{

    [TestFixture]
    public class TestHabaneroStringBuilder
    {
        [Test]
        public void TestRemoveQuotedSections()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder("A quoted 'test' is needed to test this functionality");
            s.RemoveQuotedSections();
            Assert.AreEqual("A quoted  is needed to test this functionality", s.ToString());
            s = new HabaneroStringBuilder("A quoted \"test\" is needed to test this functionality");
            s.RemoveQuotedSections();
            Assert.AreEqual("A quoted  is needed to test this functionality", s.ToString());
            s = new HabaneroStringBuilder("A quoted \"test\" is needed to 'test' this functionality");
            s.RemoveQuotedSections();
            Assert.AreEqual("A quoted  is needed to  this functionality", s.ToString());
            s = new HabaneroStringBuilder("Name = 'Peter'");
            s.RemoveQuotedSections();
            Assert.AreEqual("Name = ", s.ToString());
        }

        [Test]
        public void TestPutBackQuotedSections()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder("A quoted  is needed to test this functionality");
            s.QuotedSections = new ArrayList();
            s.QuotedSections.Add(new HabaneroStringBuilder.QuotedSection(9, "'test'"));
            s.PutBackQuotedSections();
            Assert.AreEqual("A quoted 'test' is needed to test this functionality", s.ToString());
            s = new HabaneroStringBuilder("A quoted  is needed to  this functionality");
            s.QuotedSections = new ArrayList();
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
        public void TestSubString()
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder("Hello");
            Assert.AreEqual("el", s.Substring(1, 2).ToString(), "SubString returned invalid result.");
            Assert.AreEqual("llo", s.Substring(2).ToString());

            s = new HabaneroStringBuilder("A quoted  is needed to  this functionality");
            s.QuotedSections = new ArrayList();
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
