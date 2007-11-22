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
using System.Collections;
using System.Collections.Generic;
using System.Text;

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
        private List<QuotedSection> _quotedSections = new List<QuotedSection>(); //this stores an 
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
        /// Sets the list of quote types to recognise (' and " by default)
        /// </summary>
        /// <param name="quotes">The quotes</param>
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
            String newString = _string.ToString();
            List<QuotedSection> doubleQuotedSections = new List<QuotedSection>();
            foreach (String quote in _quotes)
            {
                int pos = newString.IndexOf(quote);
                while (pos != -1)
                {
                    int pos2 = newString.Substring(pos + 1, newString.Length - pos - 1).IndexOf(quote);
                    if (pos2 == 0)
                    {
                        doubleQuotedSections.Add(new QuotedSection(pos, quote));
                        newString = newString.Remove(pos, pos2 + 2);
                    }
                    pos = newString.IndexOf(quote, pos + 1);
                }
            }
            int offset = 0;
            foreach (String quote in _quotes)
            {
                int pos = newString.IndexOf(quote);
                while (pos != -1)
                {
                    int pos2 = newString.Substring(pos + 1, newString.Length - pos - 1).IndexOf(quote);
                    if (pos2 != -1)
                    {
                        int quoteLength = pos2 + 2;
                        string quoteString = newString.Substring(pos, quoteLength);
                        int quoteOffset = 0;
                        for (int i = 0; i < doubleQuotedSections.Count; )
                        {
                            QuotedSection doubleQuotedSection = doubleQuotedSections[i];
                            int quotePos = doubleQuotedSection.pos - offset - pos;
                            if (quotePos < 0 || quotePos > quoteLength)
                            {
                                //Outside the quoted string
                                i++;
                            } else 
                            {
                                //Within the quoted string
                                quoteString = quoteString.Insert(quotePos + quoteOffset, doubleQuotedSection.quotedSection);
                                quoteOffset += doubleQuotedSection.quotedSection.Length;
                                doubleQuotedSections.Remove(doubleQuotedSection);
                            }
                        }

                        offset += quoteLength;
                        _quotedSections.Add(new QuotedSection(pos, quoteString));
                        newString = newString.Remove(pos, quoteLength);
                        pos = newString.IndexOf(quote);
                    }
                    else
                    {
                        pos = -1;
                    }
                }
            }

            _string = new StringBuilder(newString);
            if (doubleQuotedSections.Count > 0)
            {
                for (int i = doubleQuotedSections.Count - 1; i >= 0; i--)
                {
                    QuotedSection doubleQuotedSection = doubleQuotedSections[i];
                    _string.Insert(doubleQuotedSection.pos, doubleQuotedSection.quotedSection);
                    //Add the offset from this insert to each quoted section that follows.
                    for( int j = 0; j < _quotedSections.Count; j++) 
                    {
                        if (_quotedSections[j].pos > doubleQuotedSection.pos)
                        {
                            _quotedSections.Insert(j, new QuotedSection(
                                _quotedSections[j].pos + doubleQuotedSection.quotedSection.Length,
                                _quotedSections[j].quotedSection));
                            _quotedSections.RemoveAt(j + 1);
                        }
                    }
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
            if (_quotedSections.Count > 0)
            {
                for (int i = _quotedSections.Count - 1; i >= 0; i--)
                {
                    _string.Insert((_quotedSections[i]).pos,
                                   (_quotedSections[i]).quotedSection);
                }
                _quotedSections.Clear();
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
            HabaneroStringBuilder newHabaneroStringBuilder = new HabaneroStringBuilder(_string.ToString().Substring(startIndex, length));
            if (_quotedSections != null)
            {
                foreach (QuotedSection quote in _quotedSections)
                {
                    if ((quote.pos >= startIndex) && (quote.pos <= startIndex + length))
                    {
                        newHabaneroStringBuilder._quotedSections.Add(new QuotedSection(quote.pos - startIndex, quote.quotedSection));
                    }
                }
            }
            return newHabaneroStringBuilder;
        }

        /// <summary>
        /// Returns a sub-string of the string, beginning at the start index
        /// </summary>
        /// <param name="startIndex">The start index to begin from</param>
        /// <returns>Returns a HabaneroStringBuilder object</returns>
        public HabaneroStringBuilder Substring(int startIndex)
        {
            HabaneroStringBuilder newHabaneroStringBuilder = new HabaneroStringBuilder(_string.ToString().Substring(startIndex));
            if (_quotedSections != null)
            {
                foreach (QuotedSection quote in _quotedSections)
                {
                    if (quote.pos >= startIndex)
                    {
                        newHabaneroStringBuilder._quotedSections.Add(new QuotedSection(quote.pos - startIndex, quote.quotedSection));
                    }
                }
            }
            return newHabaneroStringBuilder;
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
        
        internal List<QuotedSection> QuotedSections
        {
            get { return _quotedSections; }
            set { _quotedSections = value; }
        }
    }
        
}