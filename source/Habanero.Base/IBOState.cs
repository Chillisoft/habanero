namespace Habanero.Base
{
    ///<summary>
    /// The Current State of a Business Object.
    ///</summary>
    public interface IBOState
    {
        /// <summary>
        /// Indicates if the business object is new
        /// </summary>
        bool IsNew { get;  }

        /// <summary>
        /// Indicates if the business object has been deleted
        /// </summary>
        bool IsDeleted { get;  }

        /// <summary>
        /// Gets and sets the flag which indicates if the business object
        /// is currently being edited
        /// </summary>
        bool IsEditing { get;  }

        /// <summary>
        /// Indicates whether the business object has been amended since it
        /// was last persisted to the database
        /// </summary>
        bool IsDirty { get;  }

        /// <summary>
        /// Indicates whether all of the property values of the object are valid
        /// </summary>
        /// <param name="message">If the object is not valid then this returns the reason for it being invalid/param>
        /// <returns>Returns true if all are valid </returns>
        bool IsValid(out string message);

        /// <summary>
        /// Indicates whether all of the property values of the object are valid
        /// </summary>
        /// <returns>Returns true if all are valid</returns>
        bool IsValid();

        ///<summary>
        /// Returns an invalid message if the object is valid <see IsValid()>
        ///</summary>
        string IsValidMessage { get; }
    }
}