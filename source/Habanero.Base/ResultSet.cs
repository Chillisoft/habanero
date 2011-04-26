using System;
using System.Collections.Generic;

namespace Habanero.Base
{
    public class ResultSet
    {
        public IList<Field> Fields { get; private set; }
        public IList<Row> Rows { get; private set; }

        public ResultSet()
        {
            Fields = new List<Field>();
            Rows = new List<Row>();
        }

        public class Field
        {
            public Field(string propertyName)
            {
                PropertyName = propertyName;
            }

            public string PropertyName { get; private set; }
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

        public void AddResult(object[] rawValues)
        {
            this.Rows.Add(new Row(rawValues));   
        }
    }
}