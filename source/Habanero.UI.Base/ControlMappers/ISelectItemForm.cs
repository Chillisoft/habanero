using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.UI.Base.ControlMappers
{
    /// <summary>
    /// A Text Box with a Search Button. This is typically used for cases where there is a large list of potential items and it is 
    /// not appropriate to use a ComboBox for selecting the items.
    /// </summary>
    public interface IExtendedTextBox: IControlHabanero
    {
        ///<summary>
        /// The Search Button
        ///</summary>
        IButton Button { get; }
        /// <summary>
        /// The Text box in which the result of the search are displayed.
        /// </summary>
        ITextBox TextBox { get; }
    }

    /// <summary>
    /// The select item form is a control that is used to select a related item.
    /// E.g. If a property of an object is related to another object e.g. Employee object's organisationID 
    /// is related to the organisation object but there are potionally lots of organisation in the available items.
    /// A combo box to select the organisation for the employee is therefore not valid.
    /// In these cases a searchTextBoxControl can be used. When the button is clickeed a form implementing
    /// ISelectItemForm will be opened and the user can search for and select the organisation to be linked to this
    /// employee.
    ///  <see cref="IExtendedTextBox"/>
    /// </summary>
    public interface ISelectItemForm : IFormHabanero
    {
        /// <summary>
        /// Opens the Search Form for a particular BOProp and allows the user to pass in a delegate that
        /// is called when the search is complete is complete.
        /// </summary>
        /// <param name="boProp">The BOProp that the select item form is being opened for.</param>
        /// <param name="completionDelegate">The delegate that will be called when the select item form is complete.</param>
        void ShowEditor(IBOProp boProp, NotifySelectItemCompleteDelegate completionDelegate);

    }
    /// <summary>
    /// The delegate that is called with the Select Item form is closed.
    /// </summary>
    /// <param name="busObject"></param>
    public delegate void NotifySelectItemCompleteDelegate(IBusinessObject busObject);
}
