using System;
using System.Collections;
using System.Data;

namespace Habanero.Generic
{
    /// <summary>
    /// Manages a collection of string-Guid pairs
    /// </summary>
    public class StringGuidPairCollection : IEnumerable
    {
        private IList _list;
        private Hashtable _hashtable;

        /// <summary>
        /// Constructor to initialise a new empty collection
        /// </summary>
        public StringGuidPairCollection()
        {
            _list = new ArrayList();
            _hashtable = new Hashtable();
        }

        /// <summary>
        /// Adds a string-Guid pair to the collection
        /// </summary>
        /// <param name="pair">The string-Guid pair</param>
        public void Add(StringGuidPair pair)
        {
            int i = _list.Count - 1;
            if (_list.Count == 0)
            {
                _list.Add(pair);
            }
            else
            {
                while (i >= 0 && String.Compare(this[i].Str, pair.Str) > 0)
                {
                    i--;
                }
                if (i == _list.Count - 1)
                {
                    _list.Add(pair);
                }
                else
                {
                    _list.Insert(i + 1, pair);
                }
//				if (i == 0) {
//					if (String.Compare(this[i].Str, pair.Str) < 0) {
//						_list.Insert(i - 1, pair);
//					} else {
//						_list.Add(pair);
//					}
//				} else {
                //_list.Insert(i+1, pair);
                //}
            }
            _hashtable.Add(pair.Id, pair);
        }

        /// <summary>
        /// Returns the collection's enumerator
        /// </summary>
        /// <returns>Returns an IEnumerator-type object</returns>
        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// Provides an indexing facility so that the contents of the
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the StringGuidPair object at the index position
        /// specified</returns>
        public StringGuidPair this[int index]
        {
            get { return (StringGuidPair) _list[index]; }
        }

        /// <summary>
        /// Finds the string-Guid pair object with the specified Guid
        /// </summary>
        /// <param name="id">The Guid to search for</param>
        /// <returns>Returns the StringGuidPair object if found, or a new
        /// empty pair if not found</returns>
        public StringGuidPair FindByGuid(Guid id)
        {
            StringGuidPair pair = (StringGuidPair) _hashtable[id];
            if (pair != null)
            {
                return pair;
            }
            else
            {
                return new StringGuidPair("", Guid.Empty);
            }
        }

        /// <summary>
        /// Loads a new collection using the database connection and
        /// sql statement provided
        /// </summary>
        /// <param name="conn">A database connection</param>
        /// <param name="statement">A sql statement object</param>
        public void Load(IDatabaseConnection conn, ISqlStatement statement)
        {
            DataTable dt = conn.LoadDataTable(statement, "", "");
            _list = new ArrayList(dt.Rows.Count);
            _hashtable = new Hashtable(dt.Rows.Count);
            foreach (DataRow row in dt.Rows)
            {
                string stringValue = DBNull.Value.Equals(row[1]) ? "" : (string) row[1];
                this.Add(new StringGuidPair(stringValue, new Guid((string) row[0])));
            }
        }

        /// <summary>
        /// Returns the number of pairs held in the collection
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Finds the string-Guid pair object with the specified string value
        /// </summary>
        /// <param name="value">The string value to search for</param>
        /// <returns>Returns the StringGuidPair object if found, or a new
        /// empty pair if not found</returns>
        public StringGuidPair FindByValue(object value)
        {
            foreach (StringGuidPair guidPair in _list)
            {
                if (guidPair.Str.Equals(value))
                {
                    return guidPair;
                }
            }
            return new StringGuidPair("", Guid.Empty);
        }
    }
}