using System;
using System.Collections.Generic;
using System.Reflection;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Builds an <see cref="IFilterControl"/> up using a <see cref="FilterDef"/>.
    /// </summary>
    public class FilterControlBuilder
    {
        private readonly IControlFactory _controlFactory;

        /// <summary>
        /// The constructor.  
        /// </summary>
        /// <param name="controlFactory">The control factory to use in creating controls to go on the <see cref="IFilterControl"/></param>
        public FilterControlBuilder(IControlFactory controlFactory) { _controlFactory = controlFactory; }

        /// <summary>
        /// Creates an <see cref="IFilterControl"/> using the <see cref="IControlFactory"/> and creates all the filter controls
        /// defined by the <see cref="FilterDef"/> given.
        /// </summary>
        /// <param name="filterDef">The <see cref="FilterDef"/> to use in creation.</param>
        /// <returns>The created <see cref="IFilterControl"/></returns>
        public IFilterControl BuildFilterControl(FilterDef filterDef)
        {
            IFilterControl filterControl = _controlFactory.CreateFilterControl();
            BuildFilterControl(filterDef, filterControl);
            return filterControl;
        }

        /// <summary>
        /// Clears and populates the given <see cref="IFilterControl"/> using the <see cref="FilterDef"/> given.
        /// </summary>
        /// <param name="filterDef">The <see cref="FilterDef"/> defining what filter fields are required</param>
        /// <param name="filterControl">The <see cref="IFilterControl"/> to place the filter controls on.</param>
        public void BuildFilterControl(FilterDef filterDef, IFilterControl filterControl) {
            filterControl.Controls.Clear();
            filterControl.FilterControls.Clear();
            filterControl.FilterMode = filterDef.FilterMode;

            SetupLayoutManager(filterControl, filterDef);

            foreach (FilterPropertyDef filterPropertyDef in filterDef.FilterPropertyDefs)
            {
                Type filterType = TypeLoader.LoadType(filterPropertyDef.FilterTypeAssembly, filterPropertyDef.FilterType);
                ICustomFilter customFilter = (ICustomFilter)Activator.CreateInstance(filterType, _controlFactory,
                                                                                     filterPropertyDef.PropertyName,
                                                                                     FilterClauseOperator.OpEquals);
                SetParametersOnFilter(filterPropertyDef, filterType, customFilter);
                filterControl.AddCustomFilter(filterPropertyDef.Label, filterPropertyDef.PropertyName, customFilter);
            }

            
        }

        private void SetupLayoutManager(IFilterControl filterControl, FilterDef filterDef)
        {
            if (filterDef.Columns > 0)
            {
                GridLayoutManager layoutManager = new GridLayoutManager(filterControl.FilterPanel, _controlFactory);
                int rows = filterDef.FilterPropertyDefs.Count/filterDef.Columns + 1;
                int cols = filterDef.Columns * 2;
                layoutManager.SetGridSize(rows, cols);
                filterControl.LayoutManager = layoutManager;
            }
        }

        private void SetParametersOnFilter(FilterPropertyDef filterPropertyDef, Type filterType, ICustomFilter customFilter)
        {
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
        }

        public ICustomFilter BuildCustomFilter(FilterPropertyDef filterPropertyDef) {
            Type filterType = TypeLoader.LoadType(filterPropertyDef.FilterTypeAssembly, filterPropertyDef.FilterType);
            ICustomFilter customFilter = (ICustomFilter)Activator.CreateInstance(filterType, _controlFactory,
                                                                                 filterPropertyDef.PropertyName,
                                                                                 filterPropertyDef.FilterClauseOperator);
            SetParametersOnFilter(filterPropertyDef, filterType, customFilter);
            return customFilter;
        
        }
    }
}