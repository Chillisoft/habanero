using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base.FilterControl;

namespace Habanero.UI.Base
{
    public enum FilterModes
    {
        Filter,
        Search
    }

    /// <summary>
    /// Manages a read-only grid with buttons (ie. a grid whose objects are
    /// edited through an editing form rather than directly on the grid).
    /// By default, an "Edit" and "Add" are added at 
    /// the bottom of the grid, which open up dialogs to edit the selected
    /// business object.<br/>
    /// To supply the business object collection to display in the grid,
    /// instantiate a new BusinessObjectCollection and load the collection
    /// from the database using the Load() command.  After instantiating this
    /// grid with the parameterless constructor, pass the collection with
    /// SetBusinessObjectCollection().<br/>
    /// To have further control of particular aspects of the buttons or
    /// grid, access the standard functionality through the Grid and
    /// Buttons properties (eg. myGridWithButtons.Buttons.AddButton(...)).
    /// You can assign a non-default object editor or creator for the buttons,
    /// using *.Buttons.BusinessObjectEditor and *.Buttons.BusinessObjectCreator.
    /// </summary>
    public interface IReadOnlyGridControl : IControlChilli
    {
        /// <summary>
        /// initiliase the grid to the with the 'default' UIdef.
        /// </summary>
        void Initialise(ClassDef classDef);

        /// <summary>
        /// Returns the grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (eg. myGridWithButtons.Grid.AddBusinessObject(...)).
        /// </summary>    
        IReadOnlyGrid Grid { get; }

        BusinessObject SelectedBusinessObject { get; set; }

        /// <summary>
        /// Returns the button control held. This property can be used
        /// to access a range of functionality for the button control
        /// (eg. myGridWithButtons.Buttons.AddButton(...)).
        /// </summary>
        IReadOnlyGridButtonsControl Buttons { get; }

        IBusinessObjectEditor BusinessObjectEditor { get; set; }

        IBusinessObjectCreator BusinessObjectCreator { get; set; }

        IBusinessObjectDeletor BusinessObjectDeletor { get; set; }

        string UiDefName { get; }

        ClassDef ClassDef { get; }

        IFilterControl FilterControl { get; }

        bool IsInitialised { get; }

        FilterModes FilterMode { get; set; }

        /// <summary>
        /// Sets the business object collection to display.  Loading of
        /// the collection needs to be done before it is assigned to the
        /// grid.  This method assumes a default ui definition is to be
        /// used, that is a 'ui' element without a 'name' attribute.
        /// </summary>
        /// <param name="boCollection">The new business object collection
        /// to be shown in the grid</param>
        void SetBusinessObjectCollection(IBusinessObjectCollection boCollection);

        void Initialise(ClassDef def, string uiDefName);
    }
}