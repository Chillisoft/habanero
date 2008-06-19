using System.ComponentModel;

namespace Habanero.UI.Base
{

    public interface IDataGridViewTextBoxColumn
    {
        /// <summary>Gets or sets the maximum number of characters that can be entered into the text box.</summary>
        /// <returns>The maximum number of characters that can be entered into the text box; the default value is 32767.</returns>
        /// <exception cref="T:System.InvalidOperationException">The value of the CellTemplate property is null.</exception>
        [DefaultValue(0x7fff)]
        int MaxInputLength { get; set; }
    }
}