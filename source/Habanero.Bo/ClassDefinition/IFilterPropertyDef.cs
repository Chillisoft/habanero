using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    public interface IFilterPropertyDef
    {
        /// <summary>
        /// The name of the property this filter applies to.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// The text to use to label the filter control.  This shows up as a label to the left of the control.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// The type of filter to use.  This type must implement ICustomFilter.  The default is StringTextBoxFilter
        /// </summary>
        string FilterType { get; set; }

        /// <summary>
        /// The assembly to find the filter class in.  The default is Habanero.UI.Base
        /// </summary>
        string FilterTypeAssembly { get; set; }

        /// <summary>
        /// The operator to use in this filter, for those filters that allow the use of different operators.
        /// </summary>
        FilterClauseOperator FilterClauseOperator { get; set; }

        /// <summary>
        /// A set of parameters to apply to the instantiated filter.  These are applied using reflection.  For instance, if
        /// a parameter is specified in this dictionary called 'AllowNull' with a value of 'true', then once the filter
        /// (which must be of type ICustomFilter) is 
        /// instantiated , the AllowNull property will be set with a value of true via
        /// reflection.
        /// </summary>
        Dictionary<string, string> Parameters { get; set; }
    }
}