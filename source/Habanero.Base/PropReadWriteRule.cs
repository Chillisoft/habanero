using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.Base
{
    /// <summary>
    /// An enumeration used to specify different file access modes.
    /// </summary>
    public enum PropReadWriteRule
    {
        /// <summary>Full access</summary>
        ReadWrite,
        /// <summary>Read but not write/edit</summary>
        ReadOnly,
        /// <summary>Can only be edited it if was never edited before 
        /// (regardless of whether the object is new or not)</summary>
        WriteOnce,
        /// <summary>Can only be edited if the object is not new. 
        /// I.e. the property can only be updated but never created in a new object that is being inserted</summary>
        WriteNotNew,
        /// <summary>Can only be edited if the object is new. 
        /// I.e. the property can only be inserted and can never be updated after that</summary>
        WriteNew
    }
}
