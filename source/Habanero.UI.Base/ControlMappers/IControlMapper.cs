using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// An interface to model a mapper that wraps a control in
    /// order to display information related to a business object 
    /// </summary>
    public interface IControlMapper
    {
        /// <summary>
        /// Returns the control being mapped
        /// </summary>
        IControlChilli Control { get; }

        /// <summary>
        /// Returns the name of the property being edited in the control
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Controls access to the business object being represented
        /// by the control.  Where the business object has been amended or
        /// altered, the UpdateControlValueFromBo() method is automatically called here to 
        /// implement the changes in the control itself.
        /// </summary>
        BusinessObject BusinessObject { get; set; }
        void ApplyChangesToBusinessObject();
        void UpdateControlValueFromBusinessObject();
    }
}