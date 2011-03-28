// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Base.Util;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Provides a super-class for loaders that read property rules from
    /// xml data
    /// </summary>
    public class XmlRuleLoader : XmlLoader
    {
        /// <summary>
        /// The name of the rule
        /// </summary>
        protected string _name;
        private string _propTypeName;
        private string _message;
        private Dictionary<string, object> _ruleParameters;
        private string _class;
        private string _assembly;
    	private IPropRule _propRule;

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
        /// Loads a property rule from the given xml string
        /// </summary>
        /// <param name="propTypeName"></param>
        /// <param name="ruleXml">The xml string containing the rule</param>
        /// <returns>Returns the property rule object</returns>
        public IPropRule LoadRule(string propTypeName, string ruleXml)
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
        public IPropRule LoadPropertyRule(XmlElement ruleElement)
        {
            return (IPropRule) this.Load(ruleElement);
        }

        /// <summary>
        /// Loads the property rule data from the reader
        /// </summary>
        protected override sealed void LoadFromReader()
        {
        	int counter = 0;
            _reader.Read();
            _name = _reader.GetAttribute("name");
            _message = _reader.GetAttribute("message");
            _class = _reader.GetAttribute("class");
            _assembly = _reader.GetAttribute("assembly");
        	_propRule = CreatePropRule();
            _ruleParameters = _propRule.Parameters;
            _reader.Read();
            while (_reader.Name == "add")
            {
                string keyAtt = _reader.GetAttribute("key");
                string valueAtt = _reader.GetAttribute("value");
                if (keyAtt == null || keyAtt.Length == 0)
                {
                    throw new InvalidXmlDefinitionException("An 'add' " +
                        "attribute in the class definitions was missing the " +
                        "required 'key' attribute, which specifies the name " +
                        "of the rule to check, such as 'max' for integers.");
                }
                if (valueAtt == null)
                {
                    throw new InvalidXmlDefinitionException("An 'add' " +
                        "attribute in the class definitions was missing the " +
                        "required 'value' attribute, which specifies the value " +
                        "to compare with for the rule named in the 'key' " +
                        "attribute.");
                }
				if (!_ruleParameters.ContainsKey(keyAtt))
				{
					throw new InvalidXmlDefinitionException("An 'add' " +
                        "attribute was specified for a property rule that " +
                        "does not apply to the property rule. The specified " +
                        "'add' attribute was '" + keyAtt + "' but the allowed " +
                        "attributes are " + String.Join(", ", _propRule.AvailableParameters.ToArray()) + ".");
				}
                _ruleParameters[keyAtt] = valueAtt;
            	counter++;
                ReadAndIgnoreEndTag();
            }

			if (counter == 0)
            {
                throw new InvalidXmlDefinitionException("A 'rule' element in " +
                    "the class definitions must contain at least one 'add' " +
                    "element for each component of the rule, such as the " +
                    "minimum value for an integer.");
            }
          
        	_propRule.Parameters = _ruleParameters;
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
        public void LoadRuleIntoProperty(string propertyRuleElement, IPropDef def)
        {
            def.AddPropRule( this.LoadRule(def.PropertyTypeName, propertyRuleElement));
        }

        /// <summary>
        /// Creates the object using the data that has been read in using
        /// LoadFromReader(). This method needs to be implemented by the
        /// sub-class.
        /// </summary>
        /// <returns>Returns the object created</returns>
        protected override object Create()
		{
			return _propRule;
		}
        /// <summary>
        /// Creates the Prop Rule for.
        /// </summary>
        /// <returns></returns>
    	protected IPropRule CreatePropRule() {
            if (!string.IsNullOrEmpty(_class) && !string.IsNullOrEmpty(_assembly)) 
			{
				Type customPropRuleType = null;
				TypeLoader.LoadClassType(ref customPropRuleType, _assembly, _class, 
					"PropRuleBase Subclass", "Property Rule Definition");
				if (customPropRuleType.IsSubclassOf(typeof(PropRuleBase)))
				{
                    try
                    {
                        //TODO andrew 23 Dec 2010: CF : needed to ensure that _message is not null to create an instance of string
                        if (string.IsNullOrEmpty(_message))
                            _message = "";
                        return
                            (PropRuleBase)
                            ReflectionUtilitiesCF.GetInstanceWithConstructorParameters(customPropRuleType,
                                                                                       new object[] {_name, _message});
                        //return (PropRuleBase)Activator.CreateInstance(customPropRuleType, new object[] { _name, _message });
                    }
                    catch (MissingMethodException)
                    {
                        return
                            (PropRuleBase)
                            ReflectionUtilitiesCF.GetInstanceWithConstructorParameters(customPropRuleType,
                                                                                       new object[] { _name, _message, _ruleParameters });
                        //return (PropRuleBase)Activator.CreateInstance(customPropRuleType, new object[] { _name, _message, _ruleParameters });
                    }
				}
			    throw new TypeLoadException("The prop rule '" + _name + "' must inherit from PropRuleBase.");
			}
            var propRule =  CreatePropRule(_propTypeName, _name, _message);
            if (propRule != null) return propRule;
            throw new InvalidXmlDefinitionException("Could not load the Property Rule " +
				"for this type('" + _propTypeName + "').");
        }

        protected IPropRule CreatePropRule(string propTypeName, string name, string message)
        {
            var type = typeof(int);
            if (propTypeName == type.Name || propTypeName == type.FullName)
            {
                return _defClassFactory.CreatePropRuleInteger(name, message);
            }
            type = typeof(string);
            if (propTypeName == type.Name || propTypeName == type.FullName)
            {
                return _defClassFactory.CreatePropRuleString(name, message);
            }

            type = typeof(DateTime);
            if (propTypeName == type.Name || propTypeName == type.FullName)
            {
                return _defClassFactory.CreatePropRuleDate(name, message);
            }
            type = typeof(Decimal);
            if (propTypeName == type.Name || propTypeName == type.FullName)
            {
                return _defClassFactory.CreatePropRuleDecimal(name, message);
            }
            type = typeof(Single);
            if (propTypeName == type.Name || propTypeName == type.FullName)
            {
                return _defClassFactory.CreatePropRuleSingle(name, message);
            }
            type = typeof(Double);
            if (propTypeName == type.Name || propTypeName == type.FullName)
            {
                return _defClassFactory.CreatePropRuleDouble(name, message);
            }
            type = typeof(Int64);
            if (propTypeName == type.Name || propTypeName == type.FullName)
            {
                return _defClassFactory.CreatePropRuleLong(name, message);
            }
            return null;
        }
    }


}
