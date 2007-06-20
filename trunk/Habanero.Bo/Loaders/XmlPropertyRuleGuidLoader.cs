using Habanero.Bo;

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
        /// <param name="dtdPath">The dtd path</param>
        public XmlPropertyRuleGuidLoader(string dtdPath) : base(dtdPath)
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
            return new PropRuleGuid(_ruleName, _isCompulsory);
        }

        /// <summary>
        /// Loads the property rule from the reader
        /// </summary>
        protected override void LoadPropertyRuleFromReader()
        {
        }
    }
}
