using System;

namespace Habanero.Bo
{
    /// <summary>
    /// Checks Guid values against property rules that test for validity
    /// </summary>
    public class PropRuleGuid : PropRuleBase
    {
        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="isCompulsory">Whether a value is compulsory and
        /// null values are invalid</param>
        public PropRuleGuid(string ruleName, bool isCompulsory) : base(ruleName, isCompulsory, typeof (Guid))
        {
        }
    }
}