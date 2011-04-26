using Habanero.Base;

namespace Habanero.Test
{
    public class EmailAddressAsCustomProperty : CustomProperty
    {
        public EmailAddressAsCustomProperty(string emailAddress, bool isLoading)
            : base(emailAddress, isLoading)
        {
            EmailAddress = emailAddress;
        }

        public override string ToString()
        {
            return EmailAddress;
        }

        public override object GetPersistValue()
        {
            return EmailAddress;
        }

        public string EmailAddress { get; private set; }
    }
}