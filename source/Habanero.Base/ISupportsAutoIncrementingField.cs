using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.Base
{
    public interface ISupportsAutoIncrementingField
    {
        void SetAutoIncrementingFieldValue(long value);
    }
}
