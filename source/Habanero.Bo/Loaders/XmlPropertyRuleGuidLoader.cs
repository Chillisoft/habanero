using Habanero.Bo;
using Habanero.Bo.ClassDefinition;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads a Guid property rule from xml data
    /// </summary>
    public class XmlPropertyRuleGuidLoader : XmlPropertyRuleLoader
    {
        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlPropertyRuleGuidLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {

        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlPropertyRuleGuidLoader()
        {
        }

        /// <summary>
        /// Creates the property rule object from the data already loaded
        /// </summary>
        /// <returns>Returns a PropRuleGuid object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreatePropRuleGuid(_ruleName, _isCompulsory);
			//return new PropRuleGuid(_name, _isCompulsory);
        }

        /// <summary>
        /// Loads the property rule from the reader
        /// </summary>
        protected override void LoadPropertyRuleFromReader()
        {
        }
    }
}
