using System.Collections.Generic;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Represents the result of an Xml validation check, containing a boolean (<see cref="IsValid"/>) flag 
    /// indicating validity and a list of <see cref="Messages"/> containing any validation error messages.
    /// </summary>
    
    public class XmlValidationResult
    {
        private readonly List<string> _messages;

        /// <summary>
        /// Constructs a validation result.
        /// </summary>
        /// <param name="isValid">Whether the validation was successful</param>
        /// <param name="messages">Any messages that have arisen in the validation.</param>
        public XmlValidationResult(bool isValid, List<string> messages)
        {
            _messages = messages;
            IsValid = isValid;
        }

        /// <summary>
        /// Whether the validation was successful
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Any messages that have arisen in the validation.
        /// </summary>
        public List<string> Messages { get { return _messages; } }
    }
}