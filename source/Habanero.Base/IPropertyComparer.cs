using System;
using System.Collections.Generic;

namespace Habanero.Base
{
    public interface IPropertyComparer<TBusinessObject> : IComparer<TBusinessObject>
        where TBusinessObject : IBusinessObject
    {
        string PropertyName
        {
            get;
            set;
        }

        string Source
        {
            get;
            set;
        }

        Type PropertyType { get; }
    }
}