using System.Data;
using Habanero.BO.ClassDefinition;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model data-set providers
    /// </summary>
    public interface IDataSetProvider
    {
        /// <summary>
        /// Returns a data table with the UIGridDef provided
        /// </summary>
        /// <param name="uiGrid">The UIGridDef</param>
        /// <returns>Returns a DataTable object</returns>
        DataTable GetDataTable(UIGrid uiGrid);
    }
}