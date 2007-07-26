using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.Bo
{
    /// <summary>
    /// Checks string values against property rules that test for validity.
    /// See System.Text.RegularExpressions for more information on regular
    /// expression formatting.
    /// </summary>
    public class PropRuleString : PropRuleBase
    {
        private int _maxLength = -1;
        private int _minLength = 0;
    	private string _patternMatch = ""; //regex pattern match

        ///// <summary>
        ///// Constructor to initialise a new rule
        ///// </summary>
        ///// <param name="ruleName">The rule name</param>
        ///// <param name="isCompulsory">Whether a value is compulsory and
        ///// null values are invalid</param>
        ///// <param name="minLength">The minimum length required for the string</param>
        ///// <param name="maxLength">The maximum length allowed for the string</param>
        //public PropRuleString(string ruleName,
        //                      int minLength,
        //                      int maxLength) : base(ruleName, typeof (string))
        //{
        //    _maxLength = maxLength;
        //    _minLength = minLength;
        //}

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">This rule's failure message</param>
        /// <param name="parameters">The parameters for this rule.</param>
        public PropRuleString(string name, string message, Dictionary<string, object> parameters)
			: base(name, message)
		{
			base.Parameters = parameters;
		}

        /// <summary>
        /// Sets up the parameters to the rule, that is the individual pairs
        /// of rule type and rule value that make up the composite rule
        /// </summary>
		protected internal override void SetupParameters()
		{
            try
            {
                foreach (string key in _parameters.Keys)
                {
                	object value = _parameters[key];
					if (value != null)
					{
						switch (key)
						{
							case "patternMatch":
								_patternMatch = Convert.ToString(value);
								break;
							case "minLength":
								_minLength = Convert.ToInt32(value);
								break;
							case "maxLength":
								_maxLength = Convert.ToInt32(value);
								break;
							default:
								throw new InvalidXmlDefinitionException(String.Format(
                                	"The rule type '{0}' for strings does not exist. " +
                                	"Check spelling and capitalisation, or see the " +
                                	"documentation for existing options or ways to " +
                                	"add options of your own.", key));
						}
					}
                }
            }
            catch (InvalidXmlDefinitionException ex)
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
            //TODO_Err: how to test for a valid regexpression?
            _minLength = minLength;
            _maxLength = maxLength;
            _patternMatch = patternMatch ?? "";
        }

        /// <summary>
        /// Indicates whether the property value is valid against the rules
        /// </summary>
        /// <param name="propName">The property name being checked</param>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        protected internal override bool isPropValueValid(string propName, Object propValue,
                                                          ref string errorMessage)
        {
            errorMessage = "";
            //Check if propertyValue is of the correct type.
            if (propValue == null)
            {
                return true;
            }
            if (!(propValue is string))
            {
                errorMessage = String.Format("'{0}' is not valid for the rule '{1}'. " +
                    "It is not a type of string.",
                    propName, Name);
                return false;
            }
            if (!base.isPropValueValid(propName, propValue, ref errorMessage))
            {
                return false;
            }
 
            if (!CheckLengthRule(propName, propValue, ref errorMessage))
            {
                return false;
            }

            if (!CheckPatternMatchRule(propName, propValue, ref errorMessage))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the list of available parameter names for the rule.
        /// </summary>
        /// <returns>A list of the parameters that this rule uses</returns>
    	protected internal override List<string> AvailableParameters()
    	{
			List<string> parameters = new List<string>();
			parameters.Add("minLength");
			parameters.Add("maxLength");
			parameters.Add("patternMatch");
			return parameters;
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
            if (_patternMatch.Length == 0)
            {
                return true;
            }
            if (!Regex.IsMatch((string) propValue, _patternMatch))
            {
                errorMessage = String.Format("'{0}' is not valid for the rule '{1}'. ",
                    propName, Name);
                if (Message != null) errorMessage += Message;
                else errorMessage += "It does not fit the required format.";
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
            if (_minLength > 0 && ((string) propValue).Length < _minLength)
            {
                errorMessage = String.Format("'{0}' is not valid for the rule '{1}'. ",
                    propName, Name);
                if (Message != null) errorMessage += Message;
                else errorMessage += "The length cannot be less than " + _minLength + " character(s).";
                return false;
            }
            if (_maxLength > 0 && ((string) propValue).Length > _maxLength)
            {
                errorMessage = String.Format("'{0}' is not valid for the rule '{1}'. ",
                    propName, Name);
                if (Message != null) errorMessage += Message;
                else errorMessage += "The length cannot be more than " + _maxLength + " character(s).";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the maximum length the string can be
        /// </summary>
        public int MaxLength
        {
			get { return _maxLength; }
			protected set { _maxLength = value; }
        }

        /// <summary>
        /// Returns the minimum length the string can be
        /// </summary>
        public int MinLength
        {
			get { return _minLength; }
			protected set { _minLength = value; }
        }

		/// <summary>
		/// Returns the pattern match regular expression for the string
		/// </summary>
		public string PatternMatch
		{
			get { return _patternMatch; }
			protected set { _patternMatch = value; }
		}
    }
}