using System;
using System.Collections.Generic;
using System.Reflection;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.UI.Base
{
    public class FilterControlBuilder
    {
        private readonly IControlFactory _controlFactory;
        public FilterControlBuilder(IControlFactory controlFactory) { _controlFactory = controlFactory; }
        public IFilterControl BuildFilterControl(FilterDef filterDef)
        {
            IFilterControl filterControl = _controlFactory.CreateFilterControl();
            BuildFilterControl(filterDef, filterControl);
            return filterControl;
        }

        public void BuildFilterControl(FilterDef filterDef, IFilterControl filterControl) {
            filterControl.Controls.Clear();
            filterControl.FilterMode = filterDef.FilterMode;

            if (filterDef.Columns > 0)
            {
                GridLayoutManager layoutManager = new GridLayoutManager(filterControl.FilterPanel, _controlFactory);
                int rows = filterDef.FilterPropertyDefs.Count/filterDef.Columns + 1;
                int cols = filterDef.Columns * 2;
                layoutManager.SetGridSize(rows, cols);
                filterControl.LayoutManager = layoutManager;
            }

            foreach (FilterPropertyDef filterPropertyDef in filterDef.FilterPropertyDefs)
            {
                Type filterType = TypeLoader.LoadType(filterPropertyDef.FilterTypeAssembly, filterPropertyDef.FilterType);
                ICustomFilter customFilter = (ICustomFilter)Activator.CreateInstance(filterType, _controlFactory,
                                                                                     filterPropertyDef.PropertyName,
                                                                                     FilterClauseOperator.OpEquals);
                if (filterPropertyDef.Parameters != null)
                {
                    foreach (KeyValuePair<string, string> parameter in filterPropertyDef.Parameters)
                    {
                        System.Reflection.PropertyInfo propertyInfo = filterType.GetProperty(parameter.Key,
                                                                                             BindingFlags.Instance |
                                                                                             BindingFlags.Public);
                        if (propertyInfo == null)
                        {
                            throw new HabaneroDeveloperException(
                                string.Format("The property '{0}' was not found on a filter of type '{1}' for property '{2}'",
                                              parameter.Key, filterPropertyDef.FilterType, filterPropertyDef.PropertyName), "");
                        }

                        propertyInfo.SetValue(customFilter, Convert.ChangeType(parameter.Value, propertyInfo.PropertyType), null);
                    }
                }
                filterControl.AddCustomFilter(filterPropertyDef.Label, filterPropertyDef.PropertyName, customFilter);
            }

            
        }
    }
}