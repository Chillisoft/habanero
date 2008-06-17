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

        string UiDefName { get; set; }

        IClassDef ClassDef { get; set; }

        /// <summary>
        /// Returns the grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (eg. myGridWithButtons.Grid.AddBusinessObject(...)).
        /// </summary>    
        IGridBase Grid { get; }        
    }
}