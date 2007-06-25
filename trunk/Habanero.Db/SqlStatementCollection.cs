using System;
using System.Collections;
using System.Text;
using Habanero.Base;

namespace Habanero.Db
{
    /// <summary>
    /// Manages a collection of sql statements
    /// </summary>
    public class SqlStatementCollection : ISqlStatementCollection
    {
        private IList _list;

        /// <summary>
        /// Constructor to initialise a new empty collection
        /// </summary>
        public SqlStatementCollection()
        {
            _list = new ArrayList();
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
            _list.Add(statement);
        }

        /// <summary>
        /// Adds the contents of another sql statement collection into this
        /// collection
        /// </summary>
        /// <param name="statementCollection">The other collection</param>
        public void Add(ISqlStatementCollection statementCollection)
        {
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
        /// Returns the collection's enumerator
        /// </summary>
        /// <returns>Returns the enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
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

        /// <summary>
        /// Provides an indexing facility so that the collection can
        /// be accessed like an array with square brackets
        /// </summary>
        /// <param name="pos">The position in the collection</param>
        /// <returns>Returns the sql statement object at that position</returns>
        public ISqlStatement this[int pos]
        {
            get { return (ISqlStatement) _list[pos]; }
        }

        /// <summary>
        /// Inserts a sql statement object at the position specified
        /// </summary>
        /// <param name="pos">The position to insert at</param>
        /// <param name="sql">The sql statement object to add</param>
        public void Insert(int pos, ISqlStatement sql)
        {
            _list.Insert(pos, sql);
        }

        /// <summary>
        /// Indicates whether a specified collection is equal in content
        /// and order to this one
        /// </summary>
        /// <param name="o">A SqlStatementCollection object</param>
        /// <returns>Returns true if equal</returns>
        public override bool Equals(object o)
        {
            if (o is SqlStatementCollection )
            {
                SqlStatementCollection col = (SqlStatementCollection) o;
                if (col.Count != this.Count ) return false;
                for (int i = 0; i < col.Count; i++) {
                    if (!col[i].Equals(this[i])) return false;
                }
                return true;
            }
            return false;
        }
    }
}