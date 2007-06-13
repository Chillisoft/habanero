using System;
using Chillisoft.Bo.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads an integer property rule from xml data
    /// </summary>
    public class XmlPropertyRuleIntegerLoader : XmlPropertyRuleLoader
    {
        private int _minValue;
        private int _maxValue;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlPropertyRuleIntegerLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlPropertyRuleIntegerLoader()
        {
        }

        /// <summary>
        /// Creates the property rule object from the data already loaded
        /// </summary>
        /// <returns>Returns a PropRuleInteger object</returns>
        protected override object Create()
        {
            return new PropRuleInteger(_ruleName, _isCompulsory, _minValue, _maxValue);
        }

        /// <summary>
        /// Loads the property rule from the reader
        /// </summary>
        protected override void LoadPropertyRuleFromReader()
        {
            if (_reader.GetAttribute("minValue") != "")
            {
                _minValue = Convert.ToInt32(_reader.GetAttribute("minValue"));
            }
            else
            {
                _minValue = int.MinValue;
            }
            if (_reader.GetAttribute("maxValue") != "")
            {
                _maxValue = Convert.ToInt32(_reader.GetAttribute("maxValue"));
            }
            else
            {
                _maxValue = int.MaxValue;
            }
        }
    }
}