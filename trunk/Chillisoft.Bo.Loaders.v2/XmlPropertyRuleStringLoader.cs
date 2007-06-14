using System;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads a string property rule from xml data
    /// </summary>
    public class XmlPropertyRuleStringLoader : XmlPropertyRuleLoader
    {
        private int _minLength;
        private int _maxLength;

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
            return new PropRuleString(_ruleName, _isCompulsory, _minLength, _maxLength);
        }

        /// <summary>
        /// Loads the property rule from the reader
        /// </summary>
        protected override void LoadPropertyRuleFromReader()
        {
            try
            {
                _minLength = Convert.ToInt32(_reader.GetAttribute("minLength"));
                _maxLength = Convert.ToInt32(_reader.GetAttribute("maxLength"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("In a 'propertyRuleString' " +
                    "element, the 'minLength' or 'maxLength' was not set to a valid " +
                    "integer format.", ex);
            }
        }
    }
}