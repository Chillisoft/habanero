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