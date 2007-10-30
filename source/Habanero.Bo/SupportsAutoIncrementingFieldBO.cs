using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    public class SupportsAutoIncrementingFieldBO : ISupportsAutoIncrementingField {
        private readonly BusinessObject _bo;

        public SupportsAutoIncrementingFieldBO(BusinessObject bo)
        {
            _bo = bo;
        }
        public void SetAutoIncrementingFieldValue(long value)
        {
            foreach (PropDef def in _bo.ClassDef.PropDefcol) {
                if (def.AutoIncrementing) {
                    _bo.SetPropertyValue(def.PropertyName, value);
                }
            }
        }
    }
}
