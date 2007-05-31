using System;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Manages a pair of a string and a Guid.  Possible uses include
    /// lookup lists.
    /// </summary>
    public class StringGuidPair
    {
        private string itsString;
        private Guid itsGuid;

        /// <summary>
        /// Constructor to initialise a new pair
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="g">The Guid</param>
        public StringGuidPair(string str, Guid g)
        {
            itsString = str;
            itsGuid = g;
        }

        /// <summary>
        /// Returns the string
        /// </summary>
        public string Str
        {
            get { return itsString; }
        }

        /// <summary>
        /// Returns the Guid
        /// </summary>
        public Guid Id
        {
            get { return itsGuid; }
        }

        /// <summary>
        /// Indicates whether a given string-guid pair is equal in content
        /// to this one
        /// </summary>
        /// <param name="obj">A StringGuidPair object</param>
        /// <returns>Returns true if equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is StringGuidPair)
            {
                StringGuidPair pair = (StringGuidPair) obj;
                return (this.itsGuid.Equals(pair.Id) && this.itsString == pair.Str);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        /// <summary>
        /// Returns the string part
        /// </summary>
        /// <returns>Returns a string</returns>
        public override string ToString()
        {
            return Str;
        }

        /// <summary>
        /// Returns a hashcode of the string and Guid joined together
        /// </summary>
        /// <returns>Returns a hashcode integer</returns>
        public override int GetHashCode()
        {
            return (Str + Id.ToString()).GetHashCode();
        }
    }
}