using System;
using Habanero.Bo;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads a string property rule from xml data
    /// </summary>
    public class XmlPropertyRuleStringLoader : XmlPropertyRuleLoader
    {
        private int _minLength;
        private int _maxLength;
		private string _patternMatch;
		private string _patternMatchErrorMesssage;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
		public XmlPropertyRuleStringLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
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
			return _defClassFactory.CreatePropRuleString(_ruleName, _isCompulsory, _minLength, _maxLength, _patternMatch, _patternMatchErrorMesssage);
            //return new PropRuleString(_name, _isCompulsory, _minLength, _maxLength);
        }

        /// <summary>
        /// Loads the property rule from the reader
        /// </summary>
        protected override void LoadPropertyRuleFromReader()
        {
			_patternMatch = _reader.GetAttribute("patternMatch");
			_patternMatchErrorMesssage = _reader.GetAttribute("patternMatchErrorMessage");
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