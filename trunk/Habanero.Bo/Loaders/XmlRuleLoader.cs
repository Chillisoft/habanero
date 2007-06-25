using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Habanero.Bo.ClassDefinition;
using Habanero.Util.File;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Provides a super-class for loaders that read property rules from
    /// xml data
    /// </summary>
    public class XmlRuleLoader : XmlLoader
    {
        protected string _name;
        private string _propTypeName;
        private string _message;
        private Dictionary<string, object> _ruleParameters;
        private string _class;
        private string _assembly;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdLoader">The dtd loader</param>
        /// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlRuleLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
            : base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlRuleLoader()
        {
        }

        /// <summary>
        /// Loads a property rule from the given xml string
        /// </summary>
        /// <param name="ruleXml">The xml string containing the
        /// rule</param>
        /// <returns>Returns the property rule object</returns>
        public PropRuleBase LoadRule(string propTypeName, string ruleXml)
        {
            _propTypeName = propTypeName;
            return this.LoadPropertyRule(this.CreateXmlElement(ruleXml));
        }

        /// <summary>
        /// Loads a property rule from the given xml element
        /// </summary>
        /// <param name="ruleElement">The xml element containing the
        ///  rule</param>
        /// <returns>Returns the rule object</returns>
        public PropRuleBase LoadPropertyRule(XmlElement ruleElement)
        {
            return (PropRuleBase)this.Load(ruleElement);
        }

        /// <summary>
        /// Loads the property rule data from the reader
        /// </summary>
        protected override sealed void LoadFromReader()
        {
            _reader.Read();
            _name = _reader.GetAttribute("name");
            _message = _reader.GetAttribute("message");
            _class = _reader.GetAttribute("class");
            _assembly = _reader.GetAttribute("assembly");
            _ruleParameters = new Dictionary<string, object>();
            _reader.Read();
            while (_reader.Name == "add")
            {
                _ruleParameters.Add(_reader.GetAttribute("key"), _reader.GetAttribute("value"));
                _reader.Read();
            }
        }

        ///// <summary>
        ///// Loads the property rule data from the reader - to be implemented
        ///// in a subclass of XmlPropertyRuleLoader
        ///// </summary>
        //protected void LoadPropertyRuleFromReader() {
        //    try
        //    {
        //        Dictionary<string, string> ruleParameters = new Dictionary<string, string>();
        //        _reader.Read();
        //        while (_reader.Name == "add") {
        //            ruleParameters.Add(_reader.GetAttribute("name"), _reader.GetAttribute("value"));
        //            _reader.Read();
        //        }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new InvalidXmlDefinitionException("In a " +
        //            "'PropertyRuleInteger' element, either the 'minValue' or " +
        //            "'maxValue' attribute was set to an invalid integer value.", ex);
        //    }
        //}

        /// <summary>
        /// Loads the property rule from the given xml string and applies
        /// this to the specified property definition
        /// </summary>
        /// <param name="propertyRuleElement">The xml string containing the
        /// property rule</param>
        /// <param name="def">The property definition</param>
        public void LoadRuleIntoProperty(string propertyRuleElement, PropDef def)
        {
            def.assignPropRule(this.LoadRule(def.PropertyTypeName, propertyRuleElement));
        }

        protected override object Create() {
            if (_class != null && _class.Length > 0 && _assembly != null && _assembly.Length > 0) {
                return Activator.CreateInstance(TypeLoader.LoadType(_assembly, _class), new object[] {_name, _message, _ruleParameters});
            }
            if (_propTypeName == typeof(int).Name) {
                return new PropRuleInteger(_name, _message, _ruleParameters);
            } else if (_propTypeName == typeof(string).Name ) {
                return new PropRuleString(_name, _message, _ruleParameters);
            } else if (_propTypeName == typeof(DateTime).Name ) {
                return new PropRuleDate(_name, _message, _ruleParameters);
            } else if (_propTypeName == typeof(Decimal).Name) {
                return new PropRuleDecimal(_name, _message, _ruleParameters);
            }
            return null;
        }
    }
}
