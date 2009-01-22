using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    public class XmlFilterLoader : XmlLoader
    {
        private IList<FilterPropertyDef> _propertyDefs = new List<FilterPropertyDef>();
        private FilterModes _filterMode;
        private int _columns;
        public XmlFilterLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory) : base(dtdLoader, defClassFactory) { }
        public XmlFilterLoader() { }
        protected override object Create()
        {
            FilterDef filterDef = _defClassFactory.CreateFilterDef(_propertyDefs);
            filterDef.Columns = _columns;
            filterDef.FilterMode = _filterMode;
            return filterDef;
        }

        protected override void LoadFromReader()
        {
            if (_reader.Name == "filter")
            {
                _reader.Read();
                string filterModeStr = _reader.GetAttribute("filterMode");
                _filterMode = (FilterModes) Enum.Parse(typeof (FilterModes), filterModeStr);
                _columns = Convert.ToInt32(_reader.GetAttribute("columns"));

            }
            _reader.Read();

            while (_reader.Name == "filterProperty")
            {
                string propertyName = _reader.GetAttribute("name");
                string label = _reader.GetAttribute("label");
                string filterType = _reader.GetAttribute("filterType");
                string filterTypeAssembly = _reader.GetAttribute("filterTypeAssembly");
                string filterClauseOperatorStr = _reader.GetAttribute("operator");
                FilterClauseOperator filterClauseOperator 
                    = (FilterClauseOperator) Enum.Parse(typeof (FilterClauseOperator), filterClauseOperatorStr);
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                FilterPropertyDef filterPropertyDef = 
                    _defClassFactory.CreateFilterPropertyDef(propertyName, label, filterType, filterTypeAssembly, filterClauseOperator, parameters);
              
                _reader.Read();
              
                if (_reader.Name == "parameter")
                {
                    while (_reader.Name == "parameter")
                    {
                        string name = _reader.GetAttribute("name");
                        string value = _reader.GetAttribute("value");
                        filterPropertyDef.Parameters.Add(name, value);
                        _reader.Read();
                    }
                     _reader.Read();
                }
                _propertyDefs.Add(filterPropertyDef);
            }
            while (_reader.Name == "filter") _reader.Read();
        }
        public FilterDef LoadFilterDef(string xml)
        {
            return (FilterDef)base.Load(this.CreateXmlElement(xml));
        }
    }
}