using System;

namespace Habanero.Base
{
    public interface IOrderCriteriaField {
        /// <summary>
        /// The name of the property (as defined in the ClassDef) that this QueryField is for
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// The name of the field in the data source that this QueryField is for
        /// </summary>
        string FieldName { get; set; }

        /// <summary>
        /// The name of the source (such as a table name) that this QueryField is from.
        /// </summary>
        Source Source { get; set; }

        /// <summary>
        /// The SortDirection option to use when sorting
        /// </summary>
        SortDirection SortDirection { get; }

        /// <summary>
        /// Returns the full name of the order criteria - ie "Source.Name"
        /// </summary>
        string FullName { get; }
    }

    /// <summary>
    /// Field represents one field in an OrderCriteria object.  Each OrderCriteriaField has a name and SortDirection.
    /// </summary>
    public class OrderCriteriaField : IOrderCriteriaField
    {

        private readonly string _propertyName;
        private string _fieldName;
        private Source _source;
        private readonly SortDirection _sortDirection;
        private IClassDef _classDef;
        /// <summary>
        /// This is used as a type object because IPropertyComparer inherits from the base generic type 
        /// IComparer but you cannot set the use T at a field level.
        /// We wanted to however cache this since it is taking a significant amount of time
        /// in the loading.
        /// </summary>
        private object _comparer;

        private string _fullNameExcludingPrimarySource;

        /// <summary>
        /// Creates a Field with the given name and SortDirection
        /// </summary>
        /// <param name="propertyName">The name of the property to sort on</param>
        /// <param name="source">The source for the field i.e. the primary object that the field is defined from.</param>
        /// <param name="sortDirection">The SortDirection option to use when sorting</param>
        /// <param name="fieldName">The name of the field.</param>
        public OrderCriteriaField(string propertyName, string fieldName, Source source, SortDirection sortDirection)
        {
            _sortDirection = sortDirection;
            _propertyName = propertyName;
            _source = source;
            _fieldName = fieldName;
        }

        /// <summary>
        /// Creates a Field with the given name and SortDirection
        /// </summary>
        /// <param name="propertyName">The name of the property to sort on</param>
        /// <param name="sortDirection">The SortDirection option to use when sorting</param>
        public OrderCriteriaField(string propertyName, SortDirection sortDirection)
            : this(propertyName, propertyName, null, sortDirection)
        { 
        }

        /// <summary>
        /// The name of the property (as defined in the ClassDef) that this QueryField is for
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }

        /// <summary>
        /// The name of the field in the data source that this QueryField is for
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        /// <summary>
        /// The name of the source (such as a table name) that this QueryField is from.
        /// </summary>
        public Source Source
        {
            get { return _source; }
            set { _source = value; }
        }

        /// <summary>
        /// The SortDirection option to use when sorting
        /// </summary>
        public SortDirection SortDirection
        {
            get { return _sortDirection; }
        }

        /// <summary>
        /// Returns the full name of the order criteria - ie "Source.Name"
        /// </summary>
        public string FullName
        {
            get
            {
                return this.Source == null || String.IsNullOrEmpty(this.Source.ToString()) ? this.PropertyName : this.Source + "." + this.PropertyName;
            }
        }

        private string FullNameExcludingPrimarySource
        {
            get
            {
                if (this.Source == null || this.Source.ChildSource == null) return this.PropertyName;
                return this.Source.ChildSource + "." + this.PropertyName;
            }
        }


        ///<summary>
        ///Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        ///</returns>
        ///
        ///<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (!(obj is OrderCriteriaField)) return false;
            OrderCriteriaField objOrderCriteriaField = obj as OrderCriteriaField;
            return (this.PropertyName.Equals(objOrderCriteriaField.PropertyName) && _sortDirection.Equals(objOrderCriteriaField.SortDirection));
        }


        ///<summary>
        ///Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override string ToString()
        {
            string fieldAsString = "";
            if (this.Source != null) fieldAsString += Source + ".";
            return fieldAsString + String.Format("{0} {1}", PropertyName, (_sortDirection == SortDirection.Ascending ? "ASC" : "DESC"));
        }

        ///<summary>
        ///Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the current <see cref="T:System.Object"></see>.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Creates a Field object by parsing a string in the correct format.
        /// The format is:
        /// <para>&lt;field&gt; => &lt;fieldName&gt; [ ASC | DESC ] </para>
        /// For example: <code>Age DESC</code> or <code>Surname DESC</code>
        /// </summary>
        /// <param name="fieldString">The string in the correct format (see above)</param>
        /// <returns>A Field created from the string</returns>
        public new static OrderCriteriaField FromString(string fieldString)
        {
            string[] parts = fieldString.Trim().Split(' ');
            if (parts.Length > 1)
            {
                SortDirection sortDirection;
                if (parts[1].ToUpper().Equals("ASC")) sortDirection = SortDirection.Ascending;
                else if (parts[1].ToUpper().Equals("DESC")) sortDirection = SortDirection.Descending;
                else throw new ArgumentException(String.Format("'{0}' is an invalid sort order. Valid options are ASC and DESC", parts[1]));
                return CreateField(parts[0], sortDirection);
            }
            return CreateField(parts[0], SortDirection.Ascending);
        }

        /// <summary>
        /// Compares two BusinessObjects using this field.
        /// </summary>
        /// <typeparam name="T">The Type of objects being compared. This must be a class that implements IBusinessObject</typeparam>
        /// <param name="bo1">The first object to compare</param>
        /// <param name="bo2">The object to compare the first with</param>
        /// <returns>a value less than 0 if bo1 is less than bo2, 0 if bo1 = bo2 and greater than 0 if b01 is greater than bo2
        /// The value returned is negated if the SortDirection is Descending</returns>
        public int Compare<T>(T bo1, T bo2) where T : IBusinessObject
        {
            if (_comparer == null)
            {
                _classDef = bo1.ClassDef;
                _fullNameExcludingPrimarySource = this.FullNameExcludingPrimarySource;
                _comparer = _classDef.CreatePropertyComparer<IBusinessObject>(_fullNameExcludingPrimarySource);
            }
            IPropertyComparer<IBusinessObject> comparer = (IPropertyComparer<IBusinessObject>)_comparer;
            comparer.PropertyName = PropertyName;
            comparer.Source = Source != null && Source.ChildSource != null ? Source.ChildSource : null;
            int compareResult = comparer.Compare(bo1, bo2);
            if (compareResult != 0)
                return SortDirection == SortDirection.Ascending ? compareResult : -compareResult;
            return 0;
        }

        private static OrderCriteriaField CreateField(string sourceAndFieldName, SortDirection sortDirection)
        {
            //string source = null;
            string[] parts = sourceAndFieldName.Trim().Split('.');
            string fieldName = parts[parts.Length - 1];
            Source source = Source.FromString(String.Join(".", parts, 0, parts.Length - 1));
            OrderCriteriaField orderCriteriaField = new OrderCriteriaField(fieldName, fieldName, source, sortDirection);
            return orderCriteriaField;
        }

    }
}