using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.Base
{
    ///<summary>
    /// This is an interface for creating implementations of update log actions 
    /// to perform when a business object is updated.
    ///</summary>
    public interface IBusinessObjectUpdateLog
    {
        ///<summary>
        /// Perform the log action for this Update Log class.
        ///</summary>
        void Update();
    }
}
