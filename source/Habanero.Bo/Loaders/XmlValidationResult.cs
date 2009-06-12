using System.Collections.Generic;

namespace Habanero.BO.Loaders
{
    public class XmlValidationResult
    {
        private readonly List<string> _messages;

        public XmlValidationResult(bool isValid, List<string> messages)
        {
            _messages = messages;
            IsValid = isValid;
        }

        public bool IsValid { get; set; }

        public List<string> Messages { get { return _messages; } }
    }
}