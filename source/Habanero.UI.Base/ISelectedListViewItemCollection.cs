using System.Collections;

namespace Habanero.UI.Base
{
    public interface ISelectedListViewItemCollection
    {
        /// <summary>
        ///
        /// </summary>
        int Count { get; }

        /// <summary>
        ///
        /// </summary>
        IEnumerator GetEnumerator();

        /// <summary>
        ///
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        ///
        /// </summary>
        IListViewItem this[int index] { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objListViewItem"></param>
        bool Contains(IListViewItem objListViewItem);

        /// <summary>
        ///
        /// </summary>
        /// <param name="objListViewItem"></param>
        int IndexOf(IListViewItem objListViewItem);
    }
}