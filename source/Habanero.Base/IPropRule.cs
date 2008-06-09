using System;

namespace Habanero.Base
{
    public interface IPropRule
    {
        /// <summary>
        /// Returns the rule name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Returns the error message for if the rule fails.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Indicates whether the property value is valid against the rules
        /// </summary>
        /// <param name="displayName">The property name being checked</param>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        bool IsPropValueValid(string displayName, Object propValue,
            ref string errorMessage);
    }
}