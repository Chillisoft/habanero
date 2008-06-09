using System.Collections.Generic;


namespace Habanero.Base
{
    public interface IKeyDef: IEnumerable<IPropDef>
    {
        /// <summary>
        /// A method used by BOKey to determine whether to check for
        /// duplicate keys.  If true, then the uniqueness check will be ignored
        /// if any of the properties making up the key are null.<br/>
        /// NOTE: If the BOKey is a primary key, then this cannot be
        /// set to true.
        /// </summary>
        bool IgnoreIfNull { get; set; }

        /// <summary>
        /// Returns the key name for this key definition - this key name is built
        /// up through a combination of the key name and the property names
        /// </summary>
        string KeyName { get;  set; }

        /// <summary>
        /// Returns just the key name as given by the user
        /// </summary>
        string KeyNameForDisplay { get;  set; }

        /// <summary>
        /// Gets and sets the message to show to the user if a key validation
        /// fails.  A default message will be provided if this is null.
        /// </summary>
        string Message { get; set; }
    }
}