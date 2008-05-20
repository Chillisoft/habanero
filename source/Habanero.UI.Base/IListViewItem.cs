using System.ComponentModel;

namespace Habanero.UI.Base
{
    public interface IListViewItem
    {
        /// <summary>
        ///
        /// </summary>
        void Remove();

        /// <summary>Gets the zero-based index of the item within the <see cref="ListView"></see> control.</summary>
        /// <returns>The zero-based index of the item within the <see cref=""></see> of the <see cref="ListView"></see> control, or -1 if the item is not associated with a <see cref="ListView"></see> control.</returns>
        [Browsable(false)]
        int Index { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ListViewItem"/> is selected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if selected; otherwise, <c>false</c>.
        /// </value>
        [System.ComponentModel.DefaultValue(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        bool Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ListViewItem"/> is checked.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if checked; otherwise, <c>false</c>.
        /// </value>
        [System.ComponentModel.DefaultValue(false)]
        bool Checked { get; set; }

        /// <summary>
        /// Gets the list view.
        /// </summary>
        /// <value></value>
        [System.ComponentModel.Browsable(false)]
        IListView ListView { get; }

        /// <summary>
        /// Gets or sets the list view item text.
        /// </summary>
        /// <value></value>
        [System.ComponentModel.DefaultValue("")]
        string Text { get; set; }

        object Tag { get; set; }
    }
}