﻿using System.Windows.Forms;

namespace Habanero.UI.Win.Grid
{
    ///<summary>
    /// Class that provides utility methods for the <see cref="DataGridViewSelectionMode"/> class
    ///</summary>
    public static class DataGridViewSelectionModeWin
    {
        ///<summary>
        /// Gets the Habanero <see cref="Base.DataGridViewSelectionMode"/> equivalent to the provided System.Windows.Forms <see cref="DataGridViewSelectionMode"/>.
        ///</summary>
        ///<param name="DataGridViewSelectionMode">A System.Windows.Forms <see cref="DataGridViewSelectionMode"/>.</param>
        ///<returns>The equivalent Habanero <see cref="Base.DataGridViewSelectionMode"/>.</returns>
        public static Base.DataGridViewSelectionMode GetDataGridViewSelectionMode(DataGridViewSelectionMode DataGridViewSelectionMode)
        {
            switch (DataGridViewSelectionMode)
            {
                case DataGridViewSelectionMode.CellSelect: return Base.DataGridViewSelectionMode.CellSelect;
                case DataGridViewSelectionMode.FullRowSelect: return Base.DataGridViewSelectionMode.FullRowSelect;
                case DataGridViewSelectionMode.FullColumnSelect: return Base.DataGridViewSelectionMode.FullColumnSelect;
                case DataGridViewSelectionMode.RowHeaderSelect: return Base.DataGridViewSelectionMode.RowHeaderSelect;
                case DataGridViewSelectionMode.ColumnHeaderSelect: return Base.DataGridViewSelectionMode.ColumnHeaderSelect;
            }
            return (Base.DataGridViewSelectionMode)DataGridViewSelectionMode;
        }

        ///<summary>
        /// Gets the System.Windows.Forms <see cref="DataGridViewSelectionMode"/> equivalent to the provided Habanero <see cref="Base.DataGridViewSelectionMode"/>.
        ///</summary>
        ///<param name="DataGridViewSelectionMode">A Habanero <see cref="Base.DataGridViewSelectionMode"/>.</param>
        ///<returns>The equivalent System.Windows.Forms <see cref="DataGridViewSelectionMode"/>.</returns>
        public static DataGridViewSelectionMode GetDataGridViewSelectionMode(Base.DataGridViewSelectionMode DataGridViewSelectionMode)
        {
            switch (DataGridViewSelectionMode)
            {
                case Base.DataGridViewSelectionMode.CellSelect: return System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
                case Base.DataGridViewSelectionMode.FullRowSelect: return System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
                case Base.DataGridViewSelectionMode.FullColumnSelect: return System.Windows.Forms.DataGridViewSelectionMode.FullColumnSelect;
                case Base.DataGridViewSelectionMode.RowHeaderSelect: return System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
                case Base.DataGridViewSelectionMode.ColumnHeaderSelect: return System.Windows.Forms.DataGridViewSelectionMode.ColumnHeaderSelect;
            }
            return (DataGridViewSelectionMode)DataGridViewSelectionMode;
        }
    }
}
