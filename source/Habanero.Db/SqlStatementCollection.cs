using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.DB
{
    /// <summary>
    /// Manages a collection of sql statements
    /// </summary>
    public class SqlStatementCollection : ISqlStatementCollection
    {
        private List<ISqlStatement> _list;

        /// <summary>
        /// Constructor to initialise a new empty collection
        /// </summary>
        public SqlStatementCollection()
        {
            _list = new List<ISqlStatement>();
        }

        /// <summary>
        /// Constructor to initialise a new collection containing one
        /// sql statement object
        /// </summary>
        /// <param name="statement">The sql statement object</param>
        public SqlStatementCollection(ISqlStatement statement) : this()
        {
            this.Add(statement);
        }

        /// <summary>
        /// Adds a sql statement object to the collection
        /// </summary>
        /// <param name="statement">The sql statement object</param>
        public void Add(ISqlStatement statement)
        {
            if (statement == null) throw new ArgumentNullException("statement");
            _list.Add(statement);
        }

        /// <summary>
        /// Adds the contents of another sql statement collection into this
        /// collection
        /// </summary>
        /// <param name="statementCollection">The other collection</param>
        public void Add(ISqlStatementCollection statementCollection)
        {
            if (statementCollection == null) throw new ArgumentNullException("statementCollection");
            foreach (ISqlStatement statement in statementCollection)
            {
                _list.Add(statement);
            }
        }

        /// <summary>
        /// Returns the count of statements in this collection
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }


        /// <summary>
        /// Provides an indexing facility so that the collection can
        /// be accessed like an array with square brackets
        /// </summary>
        /// <param name="index">The position in the collection</param>
        /// <returns>Returns the sql statement object at that position</returns>
        public ISqlStatement this[int index]
        {
            get { return _list[index]; }
        }

        /// <summary>
        /// Inserts a sql statement object at the position specified
        /// </summary>
        /// <param name="index">The position to insert at</param>
        /// <param name="sql">The sql statement object to add</param>
        public void Insert(int index, ISqlStatement sql)
        {
            _list.Insert(index, sql);
        }

        IEnumerator<ISqlStatement> IEnumerable<ISqlStatement>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

    	#region Standard object Overrides

		/// <summary>
		/// Indicates whether a specified collection is equal in content
		/// and order to this one
		/// </summary>
		/// <param name="obj">A SqlStatementCollection object</param>
		/// <returns>Returns true if equal</returns>
		public override bool Equals(object obj)
		{
			if (obj is SqlStatementCollection)
			{
				SqlStatementCollection col = (SqlStatementCollection)obj;
				if (col.Count != this.Count) return false;
				for (int i = 0; i < col.Count; i++)
				{
					if (!col[i].Equals(this[i])) return false;
				}
				return true;
			}
			return false;
		}


    	///<summary>
    	///Serves as a hash function for a particular type. 
    	/// <see cref="M:System.Object.GetHashCode"></see> is suitable for use in 
    	/// hashing algorithms and data structures like a hash table.
    	///</summary>
    	///<returns>
    	///A hash code for the current <see cref="T:System.Object"></see>.
    	///</returns>
    	///<filterpriority>2</filterpriority>
    	public override int GetHashCode()
    	{
           return _list.GetHashCode();
    	}

    	/// <summary>
		/// Returns a string containing all the collection's sql statements
		/// </summary>
		/// <returns>Returns a string</returns>
		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			foreach (SqlStatement statement in this)
			{
				str.Append(statement.ToString());
				str.Append(Environment.NewLine);
			}
			return str.ToString();
		}

		#endregion //Standard object Overrides

	}
}