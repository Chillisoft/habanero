using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{

    /// <summary>
    /// Defines a field to filter on.  A <see cref="FilterDef"/> consists of a set of these, and so a filter is a set of
    /// filter properties.
    /// The default filter type is the a text box that filters using a 'like' operator.
    /// To choose your own filter type specify the <see cref="FilterType"/> and the <see cref="FilterTypeAssembly"/>.
    /// </summary>
    public class FilterPropertyDef
    {
        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="propertyName">See <see cref="PropertyName"/></param>
        /// <param name="label">See <see cref="Label"/></param>
        /// <param name="filterType">See <see cref="FilterType"/></param>
        /// <param name="filterTypeAssembly">See <see cref="FilterTypeAssembly"/></param>
        /// <param name="filterClauseOperator">See <see cref="FilterClauseOperator"/></param>
        /// <param name="parameters">See <see cref="Parameters"/></param>
        public FilterPropertyDef(string propertyName, string label, string filterType, 
                                string filterTypeAssembly, FilterClauseOperator filterClauseOperator,
                                Dictionary<string, string> parameters)
        {
            PropertyName = propertyName;
            Label = label;
            FilterType = filterType;
            FilterTypeAssembly = filterTypeAssembly;
            FilterClauseOperator = filterClauseOperator;
            Parameters = parameters;
            if (string.IsNullOrEmpty(FilterTypeAssembly)) FilterTypeAssembly = "Habanero.UI.Base";
        }


        /// <summary>
        /// The name of the property this filter applies to.
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// The text to use to label the filter control.  This shows up as a label to the left of the control.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// The type of filter to use.  This type must implement ICustomFilter.  The default is StringTextBoxFilter
        /// </summary>
        public string FilterType { get; set; }

        /// <summary>
        /// The assembly to find the filter class in.  The default is Habanero.UI.Base
        /// </summary>
        public string FilterTypeAssembly { get; set; }

        /// <summary>
        /// The operator to use in this filter, for those filters that allow the use of different operators.
        /// </summary>
        public FilterClauseOperator FilterClauseOperator { get; set; }

        /// <summary>
        /// A set of parameters to apply to the instantiated filter.  These are applied using reflection.  For instance, if
        /// a parameter is specified in this dictionary called 'AllowNull' with a value of 'true', then once the filter is 
        /// instantiated (which must be of type ICustomFilter), the AllowNull property will be set with a value of true via
        /// reflection.
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }

    }
}