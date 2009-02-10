//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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