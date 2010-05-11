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
using System.Text.RegularExpressions;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// Checks string values against property rules that test for validity.
    /// See System.Text.RegularExpressions for more information on regular
    /// expression formatting.
    /// </summary>
    public class PropRuleString : PropRuleBase
    {
        #region Constructors

        ///// <summary>
        ///// Constructor to initialise a new rule
        ///// </summary>
        ///// <param name="ruleName">The rule name</param>
        ///// <param name="message">This rule's failure message</param>
        ///// <param name="parameters">The parameters for this rule.</param>
        public PropRuleString(string name, string message) : base(name, message)
        {
            InitialiseParameters(0, -1, "", "");
        }

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="message">The rule failure message</param>
        /// <param name="minLength">The minimum length required for the string</param>
        /// <param name="maxLength">The maximum length allowed for the string</param>
        /// <param name="patternMatch">The pattern match as a regular
        /// expression that the string must conform to.  See 
        /// System.Text.RegularExpressions for more information on regular
        /// expression formatting.</param>
        public PropRuleString(string ruleName,
                               string message,
                                int minLength,
                                int maxLength,
                                string patternMatch)
            : base(ruleName, message)
        {
            InitialiseParameters(minLength, maxLength, "", patternMatch ?? "");
        }

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="message">The rule failure message</param>
        /// <param name="minLength">The minimum length required for the string</param>
        /// <param name="maxLength">The maximum length allowed for the string</param>
        /// <param name="patternMatch">The pattern match as a regular expression that the string must conform to.  
        /// See System.Text.RegularExpressions for more information on regular expression formatting.</param>
        /// <param name="patternMatchMessage">This message will be used when the Pattern Match rule has failed.</param>
        public PropRuleString(string ruleName,
                               string message,
                                int minLength,
                                int maxLength,
                                string patternMatch,
                                string patternMatchMessage)
            : base(ruleName, message)
        {
            InitialiseParameters(minLength, maxLength, patternMatchMessage, patternMatch);
        }

        private void InitialiseParameters(int minLength, int maxLength, string patternMatchMessage, string patternMatch)
        {
            MinLength = minLength;
            MaxLength = maxLength;
            PatternMatchMessage = patternMatchMessage;
            PatternMatch = patternMatch ?? "";
        }

        #endregion //Constructors

        /// <summary>
        /// Sets up the parameters to the rule, that is the individual pairs
        /// of rule type and rule value that make up the composite rule
        /// </summary>
		protected internal override void SetupParameters()
		{
            try
            {
                string[] keys = new string[_parameters.Keys.Count];
                _parameters.Keys.CopyTo(keys, 0);
                foreach (string key in keys)
                {
                    object value = _parameters[key];
                    if (value == null) continue;
                    //if (value is string)
                    //{
                    //    if (string.IsNullOrEmpty(Convert.ToString(value))) return;
                    //}
                    switch (key)
                    {
                        case "patternMatch":
                            PatternMatch = Convert.ToString(value);
                            break;
                        case "patternMatchMessage":
                            PatternMatchMessage = Convert.ToString(value);
                            break;
                        case "minLength":
                            if (string.IsNullOrEmpty(Convert.ToString(value)))
                                MinLength = 0;
                            else MinLength = Convert.ToInt32(value);
                            break;
                        case "maxLength":
                            if (string.IsNullOrEmpty(Convert.ToString(value)))
                                MaxLength = -1;
                            else MaxLength = Convert.ToInt32(value);
                            break;
                        default:
                            throw new InvalidXmlDefinitionException
                                (String.Format
                                     ("The rule type '{0}' for strings does not exist. "
                                      + "Check spelling and capitalisation, or see the "
                                      + "documentation for existing options or ways to "
                                      + "add options of your own.", key));
                    }
                }
            }
            catch (InvalidXmlDefinitionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("An error occurred " +
                    "while processing the property rules for a string.  The " +
                    "likely cause is that one of the attributes in the 'add' " +
                    "element has an invalid value.", ex);
            }
        }

        /// <summary>
        /// Indicates whether the property value is valid against the rules
        /// </summary>
        /// <param name="displayName">The property name being checked</param>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        public override bool IsPropValueValid(string displayName, Object propValue,
                                                          ref string errorMessage)
        {
            errorMessage = "";
            //Check if propertyValue is of the correct type.
            if (propValue == null || String.IsNullOrEmpty(Convert.ToString(propValue)))
            {
                return true;
            }
            if (!(propValue is string))
            {
                errorMessage = GetBaseErrorMessage(propValue, displayName)
                        + "It is not a type of string.";
                return false;
            }
            if (!base.IsPropValueValid(displayName, propValue, ref errorMessage))
            {
                return false;
            }
 
            return CheckLengthRule(displayName, propValue, ref errorMessage) && CheckPatternMatchRule(displayName, propValue, ref errorMessage);
        }

        /// <summary>
        /// Returns the list of available parameter names for the rule.
        /// </summary>
        /// <returns>A list of the parameters that this rule uses</returns>
        public override List<string> AvailableParameters
        {
            get
            {
                List<string> parameters = new List<string> { "minLength", "maxLength", "patternMatch", "patternMatchMessage" };
                return parameters;
            }
        }

    	/// <summary>
        /// Checks if the value matches the pattern set required. If no
        /// expression was specified, then the method will return true
        /// </summary>
        /// <param name="propName">The property name being checked</param>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        protected bool CheckPatternMatchRule(string propName, Object propValue,
                                             ref string errorMessage)
        {
            if (PatternMatch.Length == 0 || String.IsNullOrEmpty(Convert.ToString(propValue)))
            {
                return true;
            }
            if (!Regex.IsMatch((string) propValue, PatternMatch))
            {
                errorMessage = GetBaseErrorMessage(propValue, propName);
                if (!String.IsNullOrEmpty(PatternMatchMessage))
                {
                    errorMessage += PatternMatchMessage;
                }
                else if (!String.IsNullOrEmpty(Message))
                {
                    errorMessage += Message;
                }
                else
                {
                    errorMessage += "It does not fit the required format of '" + PatternMatch + "'.";
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks that the string length falls within the length range
        /// specified
        /// </summary>
        /// <param name="propName">The property name being checked</param>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        protected bool CheckLengthRule(string propName, Object propValue,
                                       ref string errorMessage)
        {
            //Check the appropriate length rules
            if (MinLength > 0 && ((string) propValue).Length < MinLength)
            {
                errorMessage = GetBaseErrorMessage(propValue, propName);
                if (!String.IsNullOrEmpty(Message))
                {
                    errorMessage += Message;
                }
                else
                {
                    errorMessage += "The length cannot be less than " + MinLength + " character(s).";
                }
                return false;
            }
            if (MaxLength > 0 && ((string) propValue).Length > MaxLength)
            {
                errorMessage = GetBaseErrorMessage(propValue, propName);
                if (!String.IsNullOrEmpty(Message))
                {
                    errorMessage += Message;
                }
                else
                {
                    errorMessage += "The length cannot be more than " + MaxLength + " character(s).";
                }
                return false;
            }
            return true;
        }



        /// <summary>
        /// Returns the maximum length the string can be
        /// </summary>
        public int MaxLength
        {
            get { return Convert.ToInt32(Parameters["maxLength"]); }
			protected set { Parameters["maxLength"] = value; }
        }

        /// <summary>
        /// Returns the minimum length the string can be
        /// </summary>
        public int MinLength
        {
            get { return Convert.ToInt32(Parameters["minLength"]); }
            protected set { Parameters["minLength"] = value; }
        }

        /// <summary>
        /// Returns the pattern match regular expression for the string
        /// </summary>
        public string PatternMatch
        {
            get { return Convert.ToString(Parameters["patternMatch"]); }
            protected set { Parameters["patternMatch"] = value; }
        }
 
        /// <summary>
        /// Returns the pattern match error message which is displayed if the pattern match fails.
        /// </summary>
        public string PatternMatchMessage
        {
            get { return Convert.ToString(Parameters["patternMatchMessage"]); }
            protected set { Parameters["patternMatchMessage"] = value; }
        }
    }
}