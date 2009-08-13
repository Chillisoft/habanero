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
    ///<summary>
    /// loads the Filter Defs from the ClassDefs.xml so
    ///</summary>
    public class XmlFilterLoader : XmlLoader
    {
        private readonly IList<IFilterPropertyDef> _propertyDefs = new List<IFilterPropertyDef>();
        private FilterModes _filterMode;
        private int _columns;
        ///<summary>
        ///</summary>
        ///<param name="dtdLoader"></param>
        ///<param name="defClassFactory"></param>
        public XmlFilterLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory) : base(dtdLoader, defClassFactory) { }

        /// <summary>
        /// Creates the object using the data that has been read in using
        /// LoadFromReader(). This method needs to be implemented by the
        /// sub-class.
        /// Creates the FilterDef with the propDefs, columns and filtermode.
        /// <returns></returns>
        /// </summary>
        /// <returns>Returns the object created</returns>
        protected override object Create()
        {
            IFilterDef filterDef = _defClassFactory.CreateFilterDef(_propertyDefs);
            filterDef.Columns = _columns;
            filterDef.FilterMode = _filterMode;
            return filterDef;
        }

        /// <summary>
        /// Loads all the data out of the reader, assuming the document is 
        /// well-formed, otherwise the error must be caught and thrown.
        /// By the end of this method the reader must be finished reading.
        /// This method needs to be implemented by the sub-class.
        /// </summary>
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
                IFilterPropertyDef filterPropertyDef = 
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
                if (!_reader.IsStartElement()) _reader.ReadEndElement();
            }
            while (_reader.Name == "filter") _reader.Read();
        }
        ///<summary>
        /// Loads the FilterDef from an XML string.
        ///</summary>
        ///<param name="xml"></param>
        ///<returns></returns>
        public IFilterDef LoadFilterDef(string xml)
        {
            return (IFilterDef)base.Load(this.CreateXmlElement(xml));
        }
    }
}