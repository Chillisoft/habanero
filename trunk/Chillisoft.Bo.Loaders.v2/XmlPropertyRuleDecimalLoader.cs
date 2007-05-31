using System;
using Chillisoft.Bo.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads a decimal property rule from xml data
    /// </summary>
    public class XmlPropertyRuleDecimalLoader : XmlPropertyRuleLoader
    {
        private decimal itsMinValue;
        private decimal itsMaxValue;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlPropertyRuleDecimalLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlPropertyRuleDecimalLoader()
        {
        }

        /// <summary>
        /// Creates the property rule object from the data already loaded
        /// </summary>
        /// <returns>Returns a PropRuleDecimal object</returns>
        protected override object Create()
        {
            return new PropRuleDecimal(itsRuleName, itsIsCompulsory, itsMinValue, itsMaxValue );
        }

        /// <summary>
        /// Loads the property rule from the reader
        /// </summary>
        protected override void LoadPropertyRuleFromReader()
        {
            if (itsReader.GetAttribute("minValue") != "")
            {
                itsMinValue = Convert.ToDecimal(itsReader.GetAttribute("minValue"));
            }
            else
            {
                itsMinValue = int.MinValue;
            }
            if (itsReader.GetAttribute("maxValue") != "")
            {
                itsMaxValue = Convert.ToDecimal(itsReader.GetAttribute("maxValue"));
            }
            else
            {
                itsMaxValue = int.MaxValue;
            }            
        }
    }
}