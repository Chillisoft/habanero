using System.Data;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// An interface to model data-set providers
    /// </summary>
    public interface IDataSetProvider
    {
        /// <summary>
        /// Returns a data table with the UIGridDef provided
        /// </summary>
        /// <param name="uiGridDef">The UIGridDef</param>
        /// <returns>Returns a DataTable object</returns>
        DataTable GetDataTable(UIGridDef uiGridDef);
    }
}