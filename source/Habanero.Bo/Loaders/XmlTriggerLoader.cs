//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Xml;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads UI form trigger information from xml data
    /// </summary>
    public class XmlTriggerLoader : XmlLoader
    {
        private string _triggeredBy;
        private string _target;
        private string _conditionValue;
        private string _action;
        private string _value;

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlTriggerLoader()
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlTriggerLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Loads a form trigger definition from the xml string provided
        /// </summary>
        /// <param name="xmlUITrigger">The xml string</param>
        /// <returns>Returns a UIFormProperty object</returns>
        public Trigger LoadTrigger(string xmlUITrigger)
        {
            return this.LoadTrigger(this.CreateXmlElement(xmlUITrigger));
        }

        /// <summary>
        /// Loads a form trigger definition from the xml element provided
        /// </summary>
        /// <param name="uiTriggerElement">The xml element</param>
        /// <returns>Returns a UIFormProperty object</returns>
        public Trigger LoadTrigger(XmlElement uiTriggerElement)
        {
            return (Trigger)Load(uiTriggerElement);
        }

        /// <summary>
        /// Creates a form trigger definition from the data already loaded
        /// </summary>
        /// <returns>Returns a Trigger object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateTrigger(_triggeredBy, _target,
                _conditionValue, _action, _value);
        }

        /// <summary>
        /// Loads form trigger data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _reader.Read();
            LoadTriggeredByAndTarget();
            LoadConditionValue();
            LoadAction();
            LoadValue();
        }

        /// <summary>
        /// Loads the triggeredBy source and the target and throws
        /// an exception if both are declared
        /// </summary>
        private void LoadTriggeredByAndTarget()
        {
            _triggeredBy = _reader.GetAttribute("triggeredBy");
            _target = _reader.GetAttribute("target");
            if (!String.IsNullOrEmpty(_triggeredBy) &&
                !String.IsNullOrEmpty(_target))
            {
                throw new InvalidXmlDefinitionException("In a 'trigger' element, both 'target' and 'triggeredBy' " +
                    "were declared for a trigger.  Only one can be set at " +
                    "any time.");
            }
        }

        /// <summary>
        /// Loads the condition value
        /// </summary>
        private void LoadConditionValue()
        {
            _conditionValue = _reader.GetAttribute("conditionValue");
        }

        /// <summary>
        /// Loads the action (error checking is done at form construction time)
        /// </summary>
        private void LoadAction()
        {
            _action = _reader.GetAttribute("action");
        }

        /// <summary>
        /// Loads the assignment value
        /// </summary>
        private void LoadValue()
        {
            _value = _reader.GetAttribute("value");
        }
    }
}