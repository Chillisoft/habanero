using System;

namespace Habanero.Base
{
    /// <summary>
    /// Code with this attribute will be excluded from Test Coverage Reports.
    /// </summary>
    [CoverageExcludeAttribute]
    public class CoverageExcludeAttribute : Attribute
    {
        /// <summary>
        /// The Reason that this code is excluded from Test Converage
        /// </summary>
        public string ExcludeReason { get; set; }
    }
}
