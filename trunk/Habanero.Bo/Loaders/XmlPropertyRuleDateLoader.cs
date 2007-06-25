using System;
using Habanero.Bo;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads a date property rule from xml data
    /// </summary>
    public class XmlPropertyRuleDateLoader : XmlPropertyRuleLoader
    {
        private DateTime? _minValue;
        private DateTime? _maxValue;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdPath">The dtd path</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
		public XmlPropertyRuleDateLoader(string dtdPath, IDefClassFactory defClassFactory)
			: base(dtdPath, defClassFactory)
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
			return _defClassFactory.CreatePropRuleDate(_ruleName, _isCompulsory, _minValue, _maxValue);
			//if (!_minValue.Equals(DateTime.MinValue))
			//{
			//    return new PropRuleDate(_ruleName, _isCompulsory, _minValue, _maxValue);
			//}
			//else
			//{
			//    return new PropRuleDate(_ruleName, _isCompulsory);
			//}
        }

        /// <summary>
        /// Loads the property rule from the reader
        /// </summary>
        protected override void LoadPropertyRuleFromReader()
        {
			_minValue = readDateTimeAttribute("minValue");
			_maxValue = readDateTimeAttribute("maxValue");
			//try
			//{
			//    if (_reader.GetAttribute("minValue") != null)
			//    {
			//        _minValue = Convert.ToDateTime(_reader.GetAttribute("minValue"));
			//        _maxValue = Convert.ToDateTime(_reader.GetAttribute("maxValue"));
			//    }
			//}
			//catch (Exception ex)
			//{
			//    throw new InvalidXmlDefinitionException("In a 'propertyRuleDate' " +
			//        "element, the 'minValue' or 'maxValue' was not set to a valid " +
			//        "date format. A typical date string would be in the form of " +
			//        "yyyy/mm/dd.", ex);
			//}
        }

		private DateTime? readDateTimeAttribute(string attributeName)
		{
			try
			{
				string attributeValue = _reader.GetAttribute(attributeName);
				if (attributeValue != null)
				{
					return Convert.ToDateTime(attributeValue);
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				throw new InvalidXmlDefinitionException(String.Format(
					"In a 'propertyRuleDate' element, the '{0}' " +
					"was not set to a valid date format. A typical date " +
					"string would be in the form of yyyy/mm/dd.", attributeName), ex);
			}
		}
    }
}