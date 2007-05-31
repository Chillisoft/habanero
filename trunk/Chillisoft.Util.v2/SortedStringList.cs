using System;
using System.Collections;

namespace Chillisoft.Util.v2
{
    /// <summary>
    /// Manages a collection of strings that is always alphabetically sorted
    /// </summary>
    public class SortedStringCollection : ICollection
    {
        private IList itsList;

        /// <summary>
        /// Constructor to initialise a new empty collection
        /// </summary>
        public SortedStringCollection()
        {
            itsList = new ArrayList();
        }

        /// <summary>
        /// Not yet implemented
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// TODO ERIC - implement
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the number of items held in the collection
        /// </summary>
        public int Count
        {
            get { return itsList.Count; }
        }

        /// <summary>
        /// Returns the synchronisation root
        /// </summary>
        public object SyncRoot
        {
            get { return itsList.SyncRoot; }
        }

        /// <summary>
        /// Indicates whether the collection is synchronised
        /// </summary>
        public bool IsSynchronized
        {
            get { return itsList.IsSynchronized; }
        }

        /// <summary>
        /// Returns the collection's enumerator
        /// </summary>
        /// <returns>Returns an IEnumerator-type object</returns>
        public IEnumerator GetEnumerator()
        {
            return itsList.GetEnumerator();
        }

        /// <summary>
        /// Provides an indexing facility so that the contents of the
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to search at</param>
        /// <returns>Returns the string found at the specified position</returns>
        public string this[int index]
        {
            get { return (string) itsList[index]; }
        }

        /// <summary>
        /// Adds a string to the collection, inserting it at the appropriate
        /// sorted position
        /// </summary>
        /// <param name="s">The string to add</param>
        public void Add(string s)
        {
            if (itsList.Count == 0)
            {
                itsList.Add(s);
            }
            else
            {
                int i = 0;
                while (i < itsList.Count && String.Compare(this[i], s) < 0)
                {
                    i++;
                }
                if (i == itsList.Count)
                {
                    if (String.Compare(this[i - 1], s) > 0)
                    {
                        itsList.Insert(i - 1, s);
                    }
                    else
                    {
                        itsList.Add(s);
                    }
                }
                else
                {
                    itsList.Insert(i, s);
                }
            }
        }
    }
}