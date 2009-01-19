using System.Collections.Generic;

namespace Habanero.BO.ClassDefinition
{
    public class FilterPropertyDef
    {
        public string PropertyName { get; private set; }
        public string Label { get; private set; }
        public string FilterType { get; set; }
        public string FilterTypeAssembly { get; set; }
        public Dictionary<string, string> Parameters { get; set; }

        public FilterPropertyDef(string propertyName, string label, string filterType, string filterTypeAssembly, Dictionary<string, string> parameters)
        {
            PropertyName = propertyName;
            Label = label;
            FilterType = filterType;
            FilterTypeAssembly = filterTypeAssembly;
            Parameters = parameters;
            if (string.IsNullOrEmpty(FilterTypeAssembly)) FilterTypeAssembly = "Habanero.UI.Base";
        }
    }
}