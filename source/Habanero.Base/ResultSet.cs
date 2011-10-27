#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;

namespace Habanero.Base
{
    public class ResultSet
    {
        private readonly List<Row> _rows;
        private readonly List<Field> _fields;

        public ResultSet()
        {
            _fields = new List<Field>();
            _rows = new List<Row>();
        }

        public IEnumerable<Row> Rows { get { return _rows; } }
        public IEnumerable<Field> Fields { get { return _fields; } }

        public class Field
        {
            public Field(string propertyName, int index)
            {
                PropertyName = propertyName;
                Index = index;
            }

            public string PropertyName { get; private set; }
            public int Index { get; set; }
        }

        public class Row
        {
            public Row()
            {
                RawValues = new List<object>();
                Values = new List<object>();
            }

            public Row(object[] rawValues) : this()
            {
                rawValues.ForEach(RawValues.Add);
                rawValues.ForEach(Values.Add);
            }

            public IList<object> RawValues { get; private set; }
            public IList<object> Values { get; private set; }
        }

        public void AddField(string propertyName)
        {
            _fields.Add(new Field(propertyName, _fields.Count));
        }

        public void AddResult(object[] rawValues)
        {
            _rows.Add(new Row(rawValues));   
        }

        public void Sort(IOrderCriteria orderCriteria)
        {
            _rows.Sort(new RowComparer(orderCriteria, this));
        }

        private class RowComparer : IComparer<Row>
        {
            private readonly IOrderCriteria _orderCriteria;
            private readonly ResultSet _resultSet;

            public RowComparer(IOrderCriteria orderCriteria, ResultSet resultSet)
            {
                _orderCriteria = orderCriteria;
                _resultSet = resultSet;
            }

            public int Compare(Row x, Row y)
            {
                foreach (var orderField in _orderCriteria.Fields)
                {
                    var propertyName = orderField.PropertyName;
                    var fieldIndex = _resultSet.Fields.First(field => field.PropertyName == propertyName).Index;
                    var result = ((IComparable)x.Values[fieldIndex]).CompareTo(y.Values[fieldIndex]);
                    if (orderField.SortDirection == SortDirection.Descending) result = -result;
                    if (result != 0) return result;
                }
                return 0;
            }
        }

      
    }
}