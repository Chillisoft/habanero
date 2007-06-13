using System;
using Chillisoft.Bo.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads a date property rule from xml data
    /// </summary>
    public class XmlPropertyRuleDateLoader : XmlPropertyRuleLoader
    {
        private DateTime _minValue = DateTime.MinValue;
        private DateTime _maxValue = DateTime.MinValue;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlPropertyRuleDateLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlPropertyRuleDateLoader()
        {
        }

        /// <summary>
        /// Creates the property rule object from the data already loaded
        /// </summary>
        /// <returns>Returns a PropRuleDate object</returns>
        protected override object Create()
        {
            if (!_minValue.Equals(DateTime.MinValue))
            {
                return new PropRuleDate(_ruleName, _isCompulsory, _minValue, _maxValue);
            }
            else
            {
                return new PropRuleDate(_ruleName, _isCompulsory);
            }
        }

        /// <summary>
        /// Loads the property rule from the reader
        /// </summary>
        protected override void LoadPropertyRuleFromReader()
        {
            if (_reader.GetAttribute("minValue") != null)
            {
                _minValue = Convert.ToDateTime(_reader.GetAttribute("minValue"));
                _maxValue = Convert.ToDateTime(_reader.GetAttribute("maxValue"));
            }
        }
    }
}