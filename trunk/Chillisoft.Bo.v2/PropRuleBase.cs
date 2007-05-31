using System;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Provides a super-class for property rules that test the validity of
    /// a property value
    /// </summary>
    public abstract class PropRuleBase
    {
        protected readonly bool mIsCompulsory = false;
        protected readonly string mRuleName;
        protected readonly Type mPropType;

        /// <summary>
        /// Constructor to initialise a new property rule
        /// </summary>
        /// <param name="ruleName">The name of the rule</param>
        /// <param name="isCompulsory">Whether having a value is compulsory
        /// (otherwise null is acceptable)</param>
        /// <param name="propType">The property type</param>
        internal PropRuleBase(string ruleName, bool isCompulsory, Type propType)
        {
            //TODO_ErrCheck invalid inputs
            mIsCompulsory = isCompulsory;
            mRuleName = ruleName;
            mPropType = propType;
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

            if (!CheckCompulsoryRule(propValue, ref errorMessage))
            {
                return false;
            }

            //Else if the value is not set and not compulsory then it is ok
            if (propValue == null)
            {
                return true;
            }
            //Check if propertyValue is of the correct type.
            if (!mPropType.IsInstanceOfType(propValue))
            {
                errorMessage = propValue.ToString() +
                               " is not valid for " + mRuleName +
                               " since it is not of type " + mPropType.ToString();
                return false;
            }

            return true;
        }


        /// <summary>
        /// Checks the given value against the compulsory rule
        /// </summary>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">An error message to amend with a reason
        /// for a value being invalid</param>
        /// <returns>Returns true if valid and false if the value is null
        /// or empty when the compulsory rule is being enforced</returns>
        protected bool CheckCompulsoryRule(Object propValue,
                                           ref string errorMessage)
        {
            //If value not set and is compulsory then return the
            //  appropriate error
            if (mIsCompulsory && (propValue == null || (propValue is String && ((string) propValue).Length == 0)))
            {
                errorMessage = errorMessage +
                               " Value is not valid since " +
                               mRuleName +
                               " is compulsory \n";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Indicates whether values are compulsory (ie. null not allowed)
        /// </summary>
        public bool IsCompulsory
        {
            get { return mIsCompulsory; }
        }

        /// <summary>
        /// Returns the rule name
        /// </summary>
        public string RuleName
        {
            get { return mRuleName; }
        }

        /// <summary>
        /// Returns the property type
        /// </summary>
        public Type PropertyType
        {
            get { return mPropType; }
        }
    }
}