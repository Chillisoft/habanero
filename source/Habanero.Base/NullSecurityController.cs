using System;

namespace Habanero.Base
{
    ///<summary>
    /// An <see cref="ISecurityController"/> that always returns the logged in Windows user name.
    ///</summary>
    public class NullSecurityController : ISecurityController
    {
        public string CurrentUserName
        {
            get { return ""; }
        }
    }
}