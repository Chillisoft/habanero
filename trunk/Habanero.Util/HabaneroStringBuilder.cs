using System;
using System.Collections;
using System.Text;
using NUnit.Framework;

namespace Habanero.Util
{
    /// <summary>
    /// Wraps StringBuilder, adding some additional string manipulation
    /// functions to parse expressions and remove quoted sections from a string.
    /// Some example scenarios that would need to be resolved would be: <br/>
    /// companyname = 'henry''s station' and ....<br/>
    /// companyname = 'brad and partners'<br/>
    /// companyname = 'peter (pty) ltd'<br/>
    /// Note: Syntax like companyname = 'henry's station' will result in
    /// incorrectly parsing of quoted sections, so it is essential that the 
    /// client of this class parses this to 'henry''s station' prior to 
    /// using this class.
    /// </summary>
    public class HabaneroStringBuilder
    {
        private StringBuilder _string;
        private ArrayList _quotedSections = new ArrayList(); //this stores an 
            // array list of all quoted sections that have been removed from 
            // the original string, along with the position the quoted section 
            // was removed from (so that it can be replaced when required)
        private String[] _quotes = new String[] {"'", "\""};

        /// <summary>
        /// Constructor to initialise a new builder
        /// </summary>
        public HabaneroStringBuilder() : base()
        {
            _string = new StringBuilder();
        }

        /// <summary>
        /// Constructor to initialise a new builder with an initial string
        /// </summary>
        /// <param name="s">The initial string</param>
        public HabaneroStringBuilder(String s) : base()
        {
            _string = new StringBuilder(s);
        }

        /// <summary>
        /// Sets the quotes
        /// </summary>
        /// <param name="quotes">The quotes</param>
        /// TODO ERIC - hmm?
        public void SetQuotes(string[] quotes)
        {
            _quotes = quotes;
        }

        /// <summary>
        /// Removes all quoted sections from the string and stores them
        /// so that they can be replaced later using PutBackQuotedSections()
        /// </summary>
        /// <returns>Returns a HabaneroStringBuilder string without quotes</returns>
        public HabaneroStringBuilder RemoveQuotedSections()
        {
            _quotedSections.Clear();
            String s = _string.ToString();
            IList doubleQuotedSections = new ArrayList();
            foreach (String quote in _quotes)
            {
                int pos = s.IndexOf(quote);
                while (pos != -1)
                {
                    int pos2 = s.Substring(pos + 1, s.Length - pos - 1).IndexOf(quote);
                    if (pos2 == 0)
                    {
                        doubleQuotedSections.Add(new QuotedSection(pos, quote));
                        s = s.Remove(pos, pos2 + 2);
                    }
                    pos = s.IndexOf(quote, pos + 1);
                }
            }
            foreach (String quote in _quotes)
            {
                int pos = s.IndexOf(quote);
                while (pos != -1)
                {
                    int pos2 = s.Substring(pos + 1, s.Length - pos - 1).IndexOf(quote);
                    if (pos2 != -1)
                    {
                        //if (pos2 == 0) {
                        //	quotedSections.Add(new QuotedSection(pos, quote)) ;
                        //} 
                        //else 
                        //{
                        string quoteString = s.Substring(pos, pos2 + 2);
                        for (int i = doubleQuotedSections.Count - 1; i >= 0; i--)
                        {
                            QuotedSection doubleQuotedSection = (QuotedSection) doubleQuotedSections[i];
                            int adjustedPos;
                            int insertionPos;
                            if (doubleQuotedSection.pos > pos + pos2)
                            {
                                adjustedPos = doubleQuotedSection.pos - (i + 1)*2;
                                insertionPos = adjustedPos - pos + (i + 1)*2;
                            }
                            else
                            {
                                adjustedPos = doubleQuotedSection.pos;
                                insertionPos = adjustedPos - pos;
                            }
                            if (adjustedPos >= pos && adjustedPos <= pos + pos2)
                            {
                                quoteString = quoteString.Insert(insertionPos, doubleQuotedSection.quotedSection);
                                doubleQuotedSections.Remove(doubleQuotedSection);
                            }
                        }
                        _quotedSections.Add(new QuotedSection(pos, quoteString));
                        //}
                        s = s.Remove(pos, pos2 + 2);
                        pos = s.IndexOf(quote);
                    }
                    else
                    {
                        pos = -1;
                    }
                }
            }


            _string = new StringBuilder(s);
            if (doubleQuotedSections.Count > 0)
            {
                for (int i = doubleQuotedSections.Count - 1; i >= 0; i--)
                {
                    _string.Insert(((QuotedSection) doubleQuotedSections[i]).pos,
                                   ((QuotedSection) doubleQuotedSections[i]).quotedSection);
                }
            }
            return this;
        }

        /// <summary>
        /// Replaces all stored quoted sections into the string in their 
        /// original positions (these would have been removed using
        /// RemoveQuotedSections())
        /// </summary>
        /// <returns>Returns a HabaneroStringBuilder object with previously 
        /// removed quoted sections put back</returns>
        public HabaneroStringBuilder PutBackQuotedSections()
        {
            if (this._quotedSections.Count > 0)
            {
                for (int i = _quotedSections.Count - 1; i >= 0; i--)
                {
                    _string.Insert(((QuotedSection) this._quotedSections[i]).pos,
                                   ((QuotedSection) this._quotedSections[i]).quotedSection);
                }
                this._quotedSections.Clear();
            }
            return this;
        }

        /// <summary>
        /// Returns the string being held
        /// </summary>
        /// <returns>Returns the string</returns>
        public override String ToString()
        {
            return _string.ToString();
        }

        /// <summary>
        /// Manages a quoted section that has been temporarily removed
        /// </summary>
        internal struct QuotedSection
        {
            public int pos;
            public String quotedSection;

            /// <summary>
            /// Constructor to initialise the quoted section
            /// </summary>
            /// <param name="pos">The position of the quote</param>
            /// <param name="quotedSection">The quoted section as a string</param>
            public QuotedSection(int pos, String quotedSection)
            {
                this.pos = pos;
                this.quotedSection = quotedSection;
            }
        }

        /// <summary>
        /// Returns a sub-string of the string
        /// </summary>
        /// <param name="startIndex">The starting index</param>
        /// <param name="length">The length to return</param>
        /// <returns>Returns a HabaneroStringBuilder object</returns>
        public HabaneroStringBuilder Substring(int startIndex, int length)
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder(_string.ToString().Substring(startIndex, length));
            if (this._quotedSections != null)
            {
                foreach (QuotedSection quote in this._quotedSections)
                {
                    if ((quote.pos >= startIndex) && (quote.pos <= startIndex + length))
                    {
                        s._quotedSections.Add(new QuotedSection(quote.pos - startIndex, quote.quotedSection));
                    }
                }
            }
            return s;
        }

        /// <summary>
        /// Returns a sub-string of the string, beginning at the start index
        /// </summary>
        /// <param name="startIndex">The start index to begin from</param>
        /// <returns>Returns a HabaneroStringBuilder object</returns>
        public HabaneroStringBuilder Substring(int startIndex)
        {
            HabaneroStringBuilder s = new HabaneroStringBuilder(_string.ToString().Substring(startIndex));
            if (this._quotedSections != null)
            {
                foreach (QuotedSection quote in this._quotedSections)
                {
                    if (quote.pos >= startIndex)
                    {
                        s._quotedSections.Add(new QuotedSection(quote.pos - startIndex, quote.quotedSection));
                    }
                }
            }
            return s;
        }

        /// <summary>
        /// Returns the index position of the given string segment
        /// </summary>
        /// <param name="value">The string segment to search for</param>
        /// <returns>Returns the index position if found, or -1</returns>
        public int IndexOf(String value)
        {
            return _string.ToString().IndexOf(value);
        }

        /// <summary>
        /// Returns the index position of the given string segment, beginning
        /// the search at the specified start index point
        /// </summary>
        /// <param name="value">The string segment to search for</param>
        /// <param name="startIndex">The start index point to begin searching 
        /// from </param>
        /// <returns>Returns the index position if found, or -1</returns>
        public int IndexOf(String value, int startIndex)
        {
            return _string.ToString().IndexOf(value, startIndex);
        }

        /// <summary>
        /// Drops the outer quotes from the string
        /// </summary>
        /// <returns>Returns the string after the outer quotes have been
        /// removed</returns>
        public HabaneroStringBuilder DropOuterQuotes()
        {
            if (_string.Length > 0)
            {
                if ((_string[0] == '\'') && (_string[_string.Length - 1] == '\''))
                {
                    _string.Remove(0, 1);
                    _string.Remove(_string.Length - 1, 1);
                }
            }
            return this;
        }

        #region Tests

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
                s._quotedSections = new ArrayList();
                s._quotedSections.Add(new QuotedSection(9, "'test'"));
                s.PutBackQuotedSections();
                Assert.AreEqual("A quoted 'test' is needed to test this functionality", s.ToString());
                s = new HabaneroStringBuilder("A quoted  is needed to  this functionality");
                s._quotedSections = new ArrayList();
                s._quotedSections.Add(new QuotedSection(9, "'test'"));
                s._quotedSections.Add(new QuotedSection(23, "\"test\""));
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
                s._quotedSections = new ArrayList();
                s._quotedSections.Add(new QuotedSection(9, "'test'"));
                s._quotedSections.Add(new QuotedSection(23, "\"test\""));
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

        #endregion //tests
    }
        
}