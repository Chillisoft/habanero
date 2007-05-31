using System.Collections;

namespace Chillisoft.Reporting.v2
{
    /// <summary>
    ///class to store a collection column definitions
    ///functionality needs to be added to allow this class to update the
    ///underlying XML document with the order of the columns	
    ////// </summary>
    public class ColumnCollection : CollectionBase
    {
        internal ColumnCollection() : base()
        {
        }

        public Column this[int index]
        {
            get { return ((Column) List[index]); }
            set { List[index] = value; }
        }

        public int Add(Column value)
        {
            return List.Add(value);
        }

        public int IndexOf(Column value)
        {
            return List.IndexOf(value);
        }

        public void Insert(int index, Column value)
        {
            List.Insert(index, value);
        }

        public void Remove(Column value)
        {
            List.Remove(value);
        }

        public bool Contains(Column value)
        {
            return List.Contains(value);
        }
    }
}