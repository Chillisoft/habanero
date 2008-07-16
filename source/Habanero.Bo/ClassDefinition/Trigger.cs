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
using System.Collections;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Manages a trigger that causes another event based on a value change
    /// in a given user interface control.  Triggers can be used for a range
    /// of purposes, such as filtering different drop-down boxes based on
    /// earlier choices by a user (eg. country > province > city), or disabling
    /// certain controls when the user sets some control to a certain value.
    /// </summary>
    /// TODO ERIC:
    /// - Consider switching enabled to readonly, or adding support for
    /// readonly or adding attribute/parameter in field for enabled (problem is
    /// you can't have something disabled initially so you enable it using a
    /// trigger)
    public class Trigger
    {
        private string _triggeredBy;
        private string _target;
        private string _conditionValue;
        private string _action;
        private string _value;

        /// <summary>
        /// Constructor to initialise a new trigger.<br/>
        /// NOTE: you cannot set both a target and triggeredBy value.
        /// </summary>
        /// <param name="triggeredBy">The property name of the field
        /// whose value change fires the trigger</param>
        /// <param name="target">The property name of the field that
        /// is affected by a fired trigger</param>
        /// <param name="conditionValue">A literal value that must be
        /// held by the triggered-by source in order for the trigger
        /// to fire.  In the case of lookup-lists, this value can be
        /// the string as shown in the drop-down.</param>
        /// <param name="action">The type of action to take when the
        /// trigger is ready to fire - see Action for options available.</param>
        /// <param name="value">The value used by the action - see
        /// Action for relevant options.</param>
        /// <exception cref="ArgumentException">Thrown if both the
        /// triggered-by and target properties are set - only one can
        /// be set at any time.</exception>
        public Trigger(string triggeredBy, string target,
            string conditionValue, string action, string value)
        {
            _triggeredBy = triggeredBy;
            _target = target;
            _conditionValue = conditionValue;
            _action = action;
            _value = value;

            CheckTargetOrSourceAreNull();
        }

        /// <summary>
        /// Gets the property name of the field, if any, which must have a value change
        /// in order for this trigger to be fired.  This property is used when the
        /// trigger has been created on the target - alternatively place the trigger on
        /// the source and indicate the Target.
        /// </summary>
        public string TriggeredBy
        {
            get { return _triggeredBy; }
            internal set
            {
                _triggeredBy = value;
                CheckTargetOrSourceAreNull();
            }
        }

        /// <summary>
        /// Gets the property name of the field, if any, which will be affected by the
        /// designated action when the source field has had a value change.  This property
        /// is used when the trigger has been created on the source field with the value
        /// change - alternatively place the trigger on the Target and indicate
        /// the source using TriggeredBy.
        /// </summary>
        public string Target
        {
            get { return _target; }
            internal set
            {
                _target = value;
                CheckTargetOrSourceAreNull();
            }
        }

        /// <summary>
        /// Gets and sets a condition value that the source field must
        /// hold in order to cause the trigger to fire.  In most cases you can
        /// simply specify the value as a string literal, but in lookup lists you
        /// can specify the string value as shown in the drop-down.
        /// </summary>
        public string ConditionValue
        {
            get { return _conditionValue; }
            set { _conditionValue = value; }
        }

        /// <summary>
        /// Gets and sets the name of the action to take when the trigger is
        /// ready to fire.  Possible options include:
        /// <ul>
        /// <li>assignLiteral - assigns the literal value as specified between the quotes
        /// in the value</li>
        /// <li>assignProperty - assigns the current value of the property given in the value</li>
        /// <li>filter - filters a lookup list with an exact match on the
        /// trigger property value (assumes that the target has a field with the same name
        /// as the trigger source)</li>
        /// <li>execute - calls a given method as provided in the value</li>
        /// <li>enable - enables/disables the control of the targer property, provide
        /// "true" or "false" as the value</li>
        /// </ul>
        /// </summary>
        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }

        /// <summary>
        /// Gets and sets the relevant value to use for the action given.  See Action
        /// for further clarification of the options available.
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Checks that the target and the triggered-by source have not both been set
        /// </summary>
        private void CheckTargetOrSourceAreNull()
        {
            if (!String.IsNullOrEmpty(_triggeredBy) &&
                !String.IsNullOrEmpty(_target))
            {
                throw new ArgumentException("Both a target and a triggered-by " +
                    "source were declared for a trigger.  Only one can be set at " +
                    "any time.");
            }
        }

        /// <summary>
        /// Checks that the various attributes of a trigger are valid
        /// </summary>
        /// <param name="trigger">The affected trigger</param>
        /// <returns>Returns true if valid, or throws an exception if not</returns>
        public static bool CheckTriggerValid(Trigger trigger)
        {
            CheckTargetTriggerCombinationValid(trigger);
            CheckActionValid(trigger.Action);
            CheckValueValid(trigger);
            return true;
        }

        /// <summary>
        /// Checks if the given action is valid.  This is a standard
        /// checkpoint for all code using triggers.
        /// </summary>
        /// <param name="action">The action string to check</param>
        /// <returns>Returns true if valid, false if not</returns>
        private static bool CheckActionValid(string action)
        {
            string[] actions = {
                                   "assignLiteral", "assignProperty",
                                   "execute", "filter", "filterReverse", "setEditable", "setEditableOnce"
                               };
            ArrayList actionsList = new ArrayList(actions);
            if (!actionsList.Contains(action))
            {
                throw new ArgumentException(String.Format(
                    "In a 'trigger', the action given as '{0}' is not valid. " +
                    "See the documentation for available options.", action));
            }
            return true;
        }

        /// <summary>
        /// Checks that the triggeredBy and target settings are valid
        /// for the specified action
        /// </summary>
        private static bool CheckTargetTriggerCombinationValid(Trigger trigger)
        {
            trigger.CheckTargetOrSourceAreNull();

            string[] exemptActions = {
                                         "assignLiteral", "assignProperty",
                                         "execute", "setEditable", "setEditableOnce"
                                     };
            ArrayList actionsList = new ArrayList(exemptActions);
            if (String.IsNullOrEmpty(trigger.TriggeredBy) &&
                String.IsNullOrEmpty(trigger.Target) &&
                !actionsList.Contains(trigger.Action))
            {
                throw new ArgumentException(String.Format(
                    "In a 'trigger', with the action given as '{0}' cannot have " +
                    "both 'triggeredBy' and 'target' as null.", trigger.Action));
            }
            return true;
        }

        /// <summary>
        /// Checks that the value attribute is appropriate for the type
        /// of action, or that the value attribute is included where required
        /// </summary>
        private static bool CheckValueValid(Trigger trigger)
        {
            string[] exemptActions = {
                                         "filter", "filterReverse"
                                     };
            ArrayList actionsList = new ArrayList(exemptActions);
            if (!actionsList.Contains(trigger.Action) &&
                String.IsNullOrEmpty(trigger.Value))
            {
                throw new ArgumentException(String.Format(
                    "In a 'trigger', the action given as '{0}' cannot have " +
                    "'value' as null or empty.", trigger.Action));
            }
            return true;
        }
    }
}