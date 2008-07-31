//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

using System;
using System.Collections;
using System.Text;
using NUnit.Framework;

namespace CoreBiz.Bo.CriteriaManager {
	/// <summary>
	/// Summary description for CoreStringBuilder.
	/// This class wraps the string builder and adds some additional
	/// string manipulation functionality required by the expression parsing.
	/// Primarily it has the capability of removing quoted sections from a string.
	/// and replacing them when required.
	///   This is essential for parsing a sql statement of type
	///   companyname = 'henry''s station' and ....
	///   or 
	///   companyname = 'brad and partners'
	///   or
	///   companyname = 'peter (pty) ltd'
	///   
	///   Note: syntax like companyname = 'henry's station' and .... will result in an
	///   incorrectly parsing of quoted sections so it is essential that the client of this class
	///   parses this to 'henry''s station' prior to using this class.
	/// </summary>
	public class CoreStringBuilder {
		private StringBuilder mString;
		private ArrayList quotedSections = new ArrayList(); //this stores an array list of all 
		// quoted sections that have been removed from the origional string. along with the position 
		// the quoted section was removed from (so that it can be replaced when required.0
		private String[] quotes = new String[] {"'", "\""};

		public CoreStringBuilder() : base() {
			mString = new StringBuilder();
		}

		public CoreStringBuilder(String s) : base() {
			mString = new StringBuilder(s);
		}

		/// <summary>
		/// Remove all quoted sections from the CoreStringbuilder and store them
		/// so that they can be replaced later <see>PutBackQuotedSections</see>
		/// </summary>
		/// <returns>return a CoreStringBuilder without quotes.</returns>
		public CoreStringBuilder RemoveQuotedSections() {
			quotedSections.Clear();
			String s = mString.ToString();
			foreach (String quote in quotes) {
				int pos = s.IndexOf(quote);
				while (pos != -1) {
					int pos2 = s.Substring(pos + 1, s.Length - pos - 1).IndexOf(quote);
					if (pos2 != -1) {
						quotedSections.Add(new QuotedSection(pos, s.Substring(pos, pos2 + 2)));
						s = s.Remove(pos, pos2 + 2);
						pos = s.IndexOf("'");
					}
					else {
						pos = -1;
					}
				}
			}
			mString = new StringBuilder(s);
			return this;
		}

		/// <summary>
		/// replace all stored quoted sections into the string in its origional position
		///<see>RemoveQuotedSections</see>
		/// </summary>
		/// <returns>return a CoreStringBuilder with previously removed quoted sections put back.</returns>
		public CoreStringBuilder PutBackQuotedSections() {
			if (this.quotedSections.Count > 0) {
				for (int i = quotedSections.Count - 1; i >= 0; i--) {
					mString.Insert(((QuotedSection)this.quotedSections[i]).pos, ((QuotedSection)this.quotedSections[i]).quotedSection);
				}
				this.quotedSections.Clear();
			}
			return this;
		}

		public override String ToString() {
			return mString.ToString();
		}

		internal struct QuotedSection {
			public int pos;
			public String quotedSection;

			public QuotedSection(int pos, String quotedSection) {
				this.pos = pos;
				this.quotedSection = quotedSection;
			}
		}

		public CoreStringBuilder Substring(int startIndex, int length) {
			CoreStringBuilder s = new CoreStringBuilder(mString.ToString().Substring(startIndex, length));
			if (this.quotedSections != null) {
				foreach (QuotedSection quote in this.quotedSections) {
					if ((quote.pos > startIndex) && (quote.pos <= startIndex + length)) {
						s.quotedSections.Add(new QuotedSection(quote.pos - startIndex, quote.quotedSection));
					}
				}
			}
			return s;
		}

		public CoreStringBuilder Substring(int startIndex) {
			CoreStringBuilder s = new CoreStringBuilder(mString.ToString().Substring(startIndex));
			if (this.quotedSections != null) {
				foreach (QuotedSection quote in this.quotedSections) {
					if (quote.pos > startIndex) {
						s.quotedSections.Add(new QuotedSection(quote.pos - startIndex, quote.quotedSection));
					}
				}
			}
			return s;
		}

		public int IndexOf(String value) {
			return mString.ToString().IndexOf(value);
		}

		public int IndexOf(String value, int startIndex) {
			return mString.ToString().IndexOf(value, startIndex);
		}

		public CoreStringBuilder DropOuterQuotes() {
			if (mString.Length > 0) 
			{
				if ((mString[0] == '\'') && (mString[mString.Length - 1] == '\'')) 
				{
					mString.Remove(0, 1);
					mString.Remove(mString.Length - 1, 1);
				}
			}
			return this;
		}

		[TestFixture]
		private class TestCoreStringBuilder {
			[Test]
			public void TestRemoveQuotedSections() {
				CoreStringBuilder s = new CoreStringBuilder("A quoted 'test' is needed to test this functionality");
				s.RemoveQuotedSections();
				Assert.AreEqual("A quoted  is needed to test this functionality", s.ToString());
				s = new CoreStringBuilder("A quoted \"test\" is needed to test this functionality");
				s.RemoveQuotedSections();
				Assert.AreEqual("A quoted  is needed to test this functionality", s.ToString());
				s = new CoreStringBuilder("A quoted \"test\" is needed to 'test' this functionality");
				s.RemoveQuotedSections();
				Assert.AreEqual("A quoted  is needed to  this functionality", s.ToString());
				s = new CoreStringBuilder("Name = 'Peter'");
				s.RemoveQuotedSections();
				Assert.AreEqual("Name = ", s.ToString());

			}

			[Test]
			public void TestPutBackQuotedSections() {
				CoreStringBuilder s = new CoreStringBuilder("A quoted  is needed to test this functionality");
				s.quotedSections = new ArrayList();
				s.quotedSections.Add(new QuotedSection(9, "'test'"));
				s.PutBackQuotedSections();
				Assert.AreEqual("A quoted 'test' is needed to test this functionality", s.ToString());
				s = new CoreStringBuilder("A quoted  is needed to  this functionality");
				s.quotedSections = new ArrayList();
				s.quotedSections.Add(new QuotedSection(9, "'test'"));
				s.quotedSections.Add(new QuotedSection(23, "\"test\""));
				s.PutBackQuotedSections();
				Assert.AreEqual("A quoted 'test' is needed to \"test\" this functionality", s.ToString());
				s = new CoreStringBuilder("A quoted  is needed to  this functionality");
				s.PutBackQuotedSections();
				Assert.AreEqual("A quoted  is needed to  this functionality", s.ToString());
			}

			[Test]
			public void TestRemoveAndPutBackQuotedSections() {
				CoreStringBuilder s = new CoreStringBuilder("A quoted 'test' is needed to test this functionality");
				s.RemoveQuotedSections();
				Assert.AreEqual("A quoted  is needed to test this functionality", s.ToString());
				s.PutBackQuotedSections();
				Assert.AreEqual("A quoted 'test' is needed to test this functionality", s.ToString());

			}

			[Test]
			public void TestSubString() {
				CoreStringBuilder s = new CoreStringBuilder("Hello");
				Assert.AreEqual("el", s.Substring(1, 2).ToString(), "SubString returned invalid result.");
				Assert.AreEqual("llo", s.Substring(2).ToString());

				s = new CoreStringBuilder("A quoted  is needed to  this functionality");
				s.quotedSections = new ArrayList();
				s.quotedSections.Add(new QuotedSection(9, "'test'"));
				s.quotedSections.Add(new QuotedSection(23, "\"test\""));
				CoreStringBuilder sub = s.Substring(9);
				Assert.AreEqual(" is needed to  this functionality", sub.ToString());
				CoreStringBuilder sub2 = sub.Substring(10, 4);
				sub.PutBackQuotedSections();
				Assert.AreEqual(" is needed to \"test\" this functionality", sub.ToString());
				sub2.PutBackQuotedSections();
				Assert.AreEqual(" to \"test\"", sub2.ToString());
			}

			[Test]
			public void TestIndexOf() {
				CoreStringBuilder s = new CoreStringBuilder("Hello");
				Assert.AreEqual(2, s.IndexOf("l"));
				s = new CoreStringBuilder("This is a 'test'");
				s.RemoveQuotedSections();
				Assert.AreEqual(-1, s.IndexOf("e"));
			}

			[Test]
			public void TestDropOuterQuotes() {
				CoreStringBuilder s = new CoreStringBuilder("'Test'");
				Assert.AreEqual("Test", s.DropOuterQuotes().ToString());
			}
		}


	}


}