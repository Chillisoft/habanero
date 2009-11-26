using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.DB
{
    public interface IDatabaseConnectionCreator
    {
        IDatabaseConnection CreateConnection();
    }
}
