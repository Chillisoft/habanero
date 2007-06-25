using System;
using System.Xml;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Base;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Provides a super-class for loaders that read property rules from
    /// xml data
    /// </summary>
    public abstract class XmlPropertyRuleLoader : XmlLoader
    {
        protected string _ruleName;
        protected bool _isCompulsory;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdPath">The dtd path</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
		public XmlPropertyRuleLoader(string dtdPath, IDefClassFactory defClassFactory)
			: base(dtdPath, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlPropertyRuleLoader()
        {
        }

        /// <summary>
        /// Loads a property rule from the given xml string
        /// </summary>
        /// <param name="propertyRuleElement">The xml string containing the
        /// property rule</param>
        /// <returns>Returns the property rule object</returns>
        public PropRuleBase LoadPropertyRule(string propertyRuleElement)
        {
            return this.LoadPropertyRule(this.CreateXmlElement(propertyRuleElement));
        }

        /// <summary>
        /// Loads a property rule from the given xml element
        /// </summary>
        /// <param name="propertyRuleElement">The xml element containing the
        /// property rule</param>
        /// <returns>Returns the property rule object</returns>
        public PropRuleBase LoadPropertyRule(XmlElement propertyRuleElement)
        {
            return (PropRuleBase) this.Load(propertyRuleElement);
        }

        /// <summary>
        /// Loads the property rule data from the reader
        /// </summary>
        protected override sealed void LoadFromReader()
        {
            _reader.Read();
            _ruleName = _reader.GetAttribute("name");
            if (_reader.GetAttribute("isCompulsory") == "true")
            {
                _isCompulsory = true;
            }
            else
            {
                _isCompulsory = false;
            }
            LoadPropertyRuleFromReader();
        }

        /// <summary>
        /// Loads the property rule data from the reader - to be implemented
        /// in a subclass of XmlPropertyRuleLoader
        /// </summary>
        protected abstract void LoadPropertyRuleFromReader();

        /// <summary>
        /// Loads the property rule from the given xml string and applies
        /// this to the specified property definition
        /// </summary>
        /// <param name="propertyRuleElement">The xml string containing the
        /// property rule</param>
        /// <param name="def">The property definition</param>
		/// <param name="dtdPath">The dtd path</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public static void LoadRuleIntoProperty(string propertyRuleElement, PropDef def, string dtdPath, IDefClassFactory defClassFactory)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(propertyRuleElement);
            string loaderClassName = "Xml" + doc.DocumentElement.Name + "Loader";
            Type loaderType = Type.GetType(typeof (XmlPropertyRuleLoader).Namespace + "." + loaderClassName, true, true);
            XmlPropertyRuleLoader loader =
                (XmlPropertyRuleLoader) Activator.CreateInstance(loaderType, new object[] {dtdPath, defClassFactory});
            def.assignPropRule(loader.LoadPropertyRule(doc.DocumentElement));
        }
    }
}