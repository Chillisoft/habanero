using System;
using System.Collections.Generic;

namespace Habanero.Bo
{
    /// <summary>
    /// Provides a super-class for property rules that test the validity of
    /// a property value.  If you would like to implement your own property
    /// rule checker, inherit from this class, override the
    /// isPropValueValid method and add a constructor with the same arguments
    /// as this one and pass back these arguments to base().
    /// In the class definitions, in the 'rule'
    /// element under the relevant 'property', specify the class and assembly
    /// of your newly implemented class.
    /// </summary>
    public abstract class PropRuleBase
    {
		private string _name;
        private string _message;

		/// <summary>
		/// Constructor to initialise a new property rule
		/// </summary>
		/// <param name="name">The name of the rule</param>
		/// <param name="message">This rule's failure message</param>
		/// <param name="parameters">The parameters for this rule.</param>
		public PropRuleBase(string name, string message, Dictionary<string, object> parameters)
		{
			_name = name;
			_message = message;
		}

        /// <summary>
        /// Indicates whether the property value is valid against the rules
        /// </summary>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        protected internal virtual bool isPropValueValid(Object propValue,
                                                         ref string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        /// <summary>
        /// Returns the rule name
        /// </summary>
        public string Name
        {
            get { return _name; }
			protected set { _name = value; }
        }

        /// <summary>
        /// Returns the error message for if the rule fails.
        /// </summary>
        public string  Message {
			get { return _message; }
			protected set { _message = value; }
        }
    }
}