using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    public interface IGridControl : IControlChilli
    {
        /// <summary>
        /// initiliase the grid to the with the 'default' UIdef.
        /// </summary>
        void Initialise(IClassDef classDef);

        void Initialise(IClassDef classDef, string uiDefName);
        /// <summary>
        /// gets and sets the user interface defiition that will load the grid and will be used by the grid add and edit buttons.
        /// </summary>
        string UiDefName { get; set; }
        /// <summary>
        /// gets and sets the class definition that will load the grid and will be used by the grid add and edit buttons.
        /// </summary>
        IClassDef ClassDef { get; set;}
        /// <summary>
        /// Returns the grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (eg. myGridWithButtons.Grid.AddBusinessObject(...)).
        /// </summary>    
        IGridBase Grid { get; }        
    }
}