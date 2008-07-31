using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.Base
{
    ///<summary>
    /// This is an interface for a class that has details and the necessary implementation 
    /// of the current security state of the system.
    ///</summary>
    public interface ISecurityController
    {
        ///<summary>
        /// Returns the current user's name.
        ///</summary>
        string CurrentUserName{ get; }
    }
}
