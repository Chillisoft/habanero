using System;

namespace Habanero.Base
{
    /// <summary>
    /// Provides an interface for BusinessObject rules that test the validity of
    /// a BusinessObject.  If you would like to implement your own BusinessObject
    /// rule checker, implement this interface.
    /// The BusinessObject rules for the <see cref="IBusinessObject"/> are implemented
    ///  using the GOF Strategy Pattern.
    /// </summary>
    public interface IBusinessObjectRule
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
        /// The <see cref="ErrorLevel"/> for this BusinessObjectRule e.g. Warning, Error. 
        /// </summary>
        ErrorLevel ErrorLevel { get; }

        /// <summary>
        /// Indicates whether the property value is valid against the rules
        /// </summary>
        /// <returns>Returns true if valid</returns>
        bool IsValid();

//        /// <summary>
//        /// Indicates whether the property value is valid against the rules
//        /// </summary>
//        /// <param name="errorMessage">A string to amend with an error
//        /// message indicating why the value might have been invalid</param>
//        /// <returns>Returns true if valid</returns>
//        bool IsValid(ref string errorMessage);
    }
}