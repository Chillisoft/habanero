using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    public interface IGridInitialiser
    {
        /// <summary>
        /// Initialises the grid based with no classDef. This is used where the columns are set up manually.
        /// A typical case of when you would want to set the columns manually would be when the grid
        ///  requires alternate columns e.g. images to indicate the state of the object or buttons/links.
        /// The grid must already have at least one column added. At least one column must be a column with the name
        /// "ID" This column is used to synchronise the grid with the business objects.
        /// </summary>
        /// <exception cref="GridBaseInitialiseException"> in the case where the columns have not already been defined for the grid</exception>
        void InitialiseGrid();
        /// <summary>
        /// initialises the grid set in the objects constructor with the default ui def for the class
        /// identified by classDef <see cref="InitialiseGrid(ClassDef, string)"/>
        /// </summary>
        /// <param name="classDef">The classdef to initialise the grid to</param>
        void InitialiseGrid(ClassDef classDef);
        /// <summary>
        /// initialises the grid set in the objects constructor with an alternate ui def for the class
        /// identified by classDef. The initialisation involves setting up the appropriate columns 
        ///  as specified in the grid def for the uiDef.
        /// </summary>
        /// <param name="classDef">The classdef to initialise the grid to</param>
        /// <param name="uiDefName">initialiss the grid with an alternate ui definition</param>
        void InitialiseGrid(ClassDef classDef, string uiDefName);

        bool IsInitialised { get; }

        /// <summary>
        /// returns the grid that this initialiser is initialising
        /// </summary>
        IGridControl Grid { get; }
    }
}