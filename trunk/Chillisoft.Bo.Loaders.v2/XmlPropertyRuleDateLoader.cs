using System;
using Chillisoft.Bo.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads a date property rule from xml data
    /// </summary>
    public class XmlPropertyRuleDateLoader : XmlPropertyRuleLoader
    {
        private DateTime itsMinValue = DateTime.MinValue;
        private DateTime itsMaxValue = DateTime.MinValue;

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
            if (!itsMinValue.Equals(DateTime.MinValue))
            {
                return new PropRuleDate(itsRuleName, itsIsCompulsory, itsMinValue, itsMaxValue);
            }
            else
            {
                return new PropRuleDate(itsRuleName, itsIsCompulsory);
            }
        }

        /// <summary>
        /// Loads the property rule from the reader
        /// </summary>
        protected override void LoadPropertyRuleFromReader()
        {
            if (itsReader.GetAttribute("minValue") != null)
            {
                itsMinValue = Convert.ToDateTime(itsReader.GetAttribute("minValue"));
                itsMaxValue = Convert.ToDateTime(itsReader.GetAttribute("maxValue"));
            }
        }
    }
}