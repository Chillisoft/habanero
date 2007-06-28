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
		//protected readonly bool _isCompulsory = false;
		private string _name;
        private string _message;
        //protected readonly Type _propType;

        ///// <summary>
        ///// Constructor to initialise a new property rule
        ///// </summary>
        ///// <param name="ruleName">The name of the rule</param>
        ///// <param name="isCompulsory">Whether having a value is compulsory
        ///// (otherwise null is acceptable)</param>
        ///// <param name="propType">The property type</param>
        //internal PropRuleBase(string ruleName, Type propType)
        //{
        //    //TODO_ErrCheck invalid inputs
        //    _isCompulsory = isCompulsory;
        //    _name = ruleName;
        //   // _propType = propType;
        //}

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
	   // /// <summary>
	   // /// Constructor to initialise a new property rule
	   // /// </summary>
	   // /// <param name="name">The name of the rule</param>
	   // /// <param name="message">This rule's failure message</param>
	   // protected internal PropRuleBase(string name, string message)
	   // {
	   //     //TODO_ErrCheck invalid inputs
	   //     _name = name;
	   //     _message = message;
	   //}


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

            //if (!CheckCompulsoryRule(propValue, ref errorMessage))
            //{
            //    return false;
            //}

            ////Else if the value is not set and not compulsory then it is ok
            //if (propValue == null)
            //{
            //    return true;
            //}
            ////Check if propertyValue is of the correct type.
            //if (!_propType.IsInstanceOfType(propValue))
            //{
            //    errorMessage = propValue.ToString() +
            //                   " is not valid for " + _name +
            //                   " since it is not of type " + _propType.ToString();
            //    return false;
            //}

            return true;
        }


		///// <summary>
		///// Checks the given value against the compulsory rule
		///// </summary>
		///// <param name="propValue">The value to check</param>
		///// <param name="errorMessage">An error message to amend with a reason
		///// for a value being invalid</param>
		///// <returns>Returns true if valid and false if the value is null
		///// or empty when the compulsory rule is being enforced</returns>
		//protected bool CheckCompulsoryRule(Object propValue,
		//                                   ref string errorMessage)
		//{
		//    //If value not set and is compulsory then return the
		//    //  appropriate error
		//    if (_isCompulsory && (propValue == null || (propValue is String && ((string) propValue).Length == 0)))
		//    {
		//        errorMessage = errorMessage +
		//                       " Value is not valid since " +
		//                       _name +
		//                       " is compulsory \n";
		//        return false;
		//    }
		//    return true;
		//}

		///// <summary>
		///// Indicates whether values are compulsory (ie. null not allowed)
		///// </summary>
		//public bool IsCompulsory
		//{
		//    get { return _isCompulsory; }
		//}

        /// <summary>
        /// Returns the rule name
        /// </summary>
        public string Name
        {
            get { return _name; }
			protected set { _name = value; }
        }

        public string  Message {
			get { return _message; }
			protected set { _message = value; }
        }

        ///// <summary>
        ///// Returns the property type
        ///// </summary>
        //public Type PropertyType
        //{
        //    get { return _propType; }
        //}
    }
}