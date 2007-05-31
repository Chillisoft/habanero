using System;
using Chillisoft.Bo.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads a string property rule from xml data
    /// </summary>
    public class XmlPropertyRuleStringLoader : XmlPropertyRuleLoader
    {
        private int itsMinLength;
        private int itsMaxLength;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlPropertyRuleStringLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlPropertyRuleStringLoader()
        {
        }

        /// <summary>
        /// Creates the property rule object from the data already loaded
        /// </summary>
        /// <returns>Returns a PropRuleString object</returns>
        protected override object Create()
        {
            return new PropRuleString(itsRuleName, itsIsCompulsory, itsMinLength, itsMaxLength);
        }

        /// <summary>
        /// Loads the property rule from the reader
        /// </summary>
        protected override void LoadPropertyRuleFromReader()
        {
            itsMinLength = Convert.ToInt32(itsReader.GetAttribute("minLength"));
            itsMaxLength = Convert.ToInt32(itsReader.GetAttribute("maxLength"));
        }
    }
}