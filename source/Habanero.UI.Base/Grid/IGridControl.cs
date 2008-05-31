using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    public interface IGridControl : IControlChilli
    {
        /// <summary>
        /// initiliase the grid to the with the 'default' UIdef.
        /// </summary>
        void Initialise(ClassDef classDef);

        void Initialise(ClassDef classDef, string uiDefName);

        string UiDefName { get; set; }

        ClassDef ClassDef { get; set; }

        /// <summary>
        /// Returns the grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (eg. myGridWithButtons.Grid.AddBusinessObject(...)).
        /// </summary>    
        IGridBase Grid { get; }        
    }
}