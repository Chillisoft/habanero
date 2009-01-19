using System.Collections.Generic;

namespace Habanero.BO.ClassDefinition
{
    public class FilterDef
    {
        public IList<FilterPropertyDef> FilterPropertyDefs { get; set; }

        public FilterDef(IList<FilterPropertyDef> filterPropertyDefs)
        {
            FilterPropertyDefs = filterPropertyDefs;
        }
    }
}