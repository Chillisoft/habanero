using System;
using System.ComponentModel;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Base
{
//    /// <summary>
//    /// The delegate used for custom loading of the grid
//    /// </summary>
//    /// <param name="grid">The grid to be loaded</param>
//    /// <param name="col">The collection to load into the grid</param>
//    public delegate void SelectorLoaderDelegate(IGridBase grid, IBusinessObjectCollection col);
//
    /// <summary>
    /// Handles the event of a user double-clicking on a row in the <see cref="IBOColSelectorControl"/>
    /// </summary>
    /// <param name="sender">The object that notified of the event</param>
    /// <param name="e">Attached arguments regarding the event</param>
    public delegate void RowDoubleClickedHandler(Object sender, BOEventArgs e);
    /// <summary>
    /// Provides a common interface that is specialised for showing a collection of 
    /// Business Objects and allowing the user to select one (Or in some cases more than one)
    /// The common controls used for selecting business objects are ComboBox, ListBox, ListView, Grid,
    ///  <see cref="ICollapsiblePanelGroupControl"/>, <see cref="IBOColTabControl"/>, a <see cref="IMultiSelector{T}"/>
    ///  or an <see cref="ITreeView"/>
    /// </summary>
    public interface IBOColSelectorControl:IControlHabanero
    {
        /// <summary>
        /// Gets and Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command or from the
        /// <see cref="IBusinessObjectLoader"/>.
        /// The default UI definition will be used, that is a 'ui' element 
        /// without a 'name' attribute.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        IBusinessObjectCollection BusinessObjectCollection { get; set; }

        /// <summary>
        /// Gets and sets the currently selected business object in the grid
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        IBusinessObject SelectedBusinessObject { get; set; }
//
//        /// <summary>
//        /// Gets a List of currently selected business objects (In Controls that do not allow the selection 
//        /// of multiple items this will be a
//        /// </summary>
//        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        ReadOnlyCollection<BusinessObject> SelectedBusinessObjects { get; }

        /// <summary>
        /// Event Occurs when a business object is selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        event EventHandler<BOEventArgs> BusinessObjectSelected;

//        /// <summary>
//        /// Occurs when the current selection in the grid is changed
//        /// </summary>
//        event EventHandler SelectionChanged;

//        /// <summary>
//        /// Occurs when the collection in the grid is changed
//        /// </summary>
//        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        event EventHandler CollectionChanged;

        /// <summary>
        /// Clears the business object collection and the rows in the data table
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        void Clear();

        /// <summary>Gets the number of rows displayed in the <see cref="IBOColSelectorControl"></see>.</summary>
        /// <returns>The number of rows in the <see cref="IBOColSelectorControl"></see>.</returns>
        [DefaultValue(0), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
         EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        int NoOfItems { get; }


        /// <summary>
        /// Returns the business object at the specified row number
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        IBusinessObject GetBusinessObjectAtRow(int row);
    }
    /// <summary>
    /// Provides a common interface that is specialised for showing a collection of 
    /// Business Objects and allowing the user to select and edit one.
    /// The common controls used for selecting business objects are ComboBox, ListBox, ListView, Grid,
    ///  <see cref="ICollapsiblePanelGroupControl"/>, <see cref="IBOColTabControl"/>, a <see cref="IMultiSelector{T}"/>
    ///  or an <see cref="ITreeView"/>.
    /// For an <see cref="IEditableGridControl"/> the business objects can be edited directly in the grid. In the case of 
    ///  a <see cref="IReadOnlyGridControl"/> the business objects are edited via pop up forms that open when appropriate.
    /// The <see cref="IBOGridAndEditorControl"/> has a grid selector with the functionality to edit the business obects to the right.
    /// This interface inherits from the <see cref="IBOColSelectorControl"/> and adds three additional methods.
    /// <li><see cref="AllowUsersToAddBO"/></li>"
    /// <li><see cref="AllowUsersToDeleteBO"/></li>"
    /// <li><see cref="AllowUsersToEditBO"/></li>"
    /// </summary>
    /// <remarks>
    /// Should possibly allow the user to set an adding, editing or deleting delegate
    /// </remarks>
    public interface IBOSelectorAndEditor : IBOColSelectorControl
    {
        ///<summary>
        /// Gets and sets whether the user can add Business objects via this control
        ///</summary>
        bool AllowUsersToAddBO { get; set; }
        /// <summary>
        /// Gets and sets whether the user can Delete (<see cref="IBusinessObject.MarkForDelete"/>) <see cref="IBusinessObject"/>s via this control
        /// </summary>
        bool AllowUsersToDeleteBO { get; set; }
        /// <summary>
        /// Gets and sets whether the user can edit <see cref="IBusinessObject"/>s via this control
        /// </summary>
        bool AllowUsersToEditBO { get; set; }
        /// <summary>
        /// Gets or sets a boolean value that determines whether to confirm
        /// deletion with the user when they have chosen to delete a row
        /// </summary>
        bool ConfirmDeletion { get; set; }
        /// <summary>
        /// Gets or sets the delegate that checks whether the user wants to delete selected rows.
        /// If <see cref="ConfirmDeletion"/> is true and no specific <see cref="CheckUserConfirmsDeletionDelegate"/> is set then
        /// a default <see cref="CheckUserConfirmsDeletion"/> is used.
        /// </summary>
        CheckUserConfirmsDeletion CheckUserConfirmsDeletionDelegate { get; set; }
    }
}