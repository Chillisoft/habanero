using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    public class FilterDef
    {
        public IList<FilterPropertyDef> FilterPropertyDefs { get; set; }
        public FilterModes FilterMode { get; set; }
        public int Columns { get; set; }

        public FilterDef(IList<FilterPropertyDef> filterPropertyDefs)
        {
            FilterPropertyDefs = filterPropertyDefs;
        }
    }
}