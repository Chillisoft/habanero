using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;
using Habanero.Util;
using Habanero.Util.File;

namespace Habanero.Bo
{
    /// <summary>
    /// Provides a lookup-list sourced from business object collections.
    /// A lookup-list is typically used to populate features like a ComboBox,
    /// where the string would be displayed, but the Guid would be the
    /// value stored (for reasons of data integrity).
    /// The string-Guid pair collection will be created using each object's
    /// ToString() function and the object's Guid ID.<br/>
    /// Note: this class does not provide criteria, so the entire collection
    /// will be loaded.
    /// </summary>
    public class BusinessObjectLookupListSource : ILookupListSource
    {
        private Type _boType;
    	private string _assemblyName;
    	private string _className;

		#region Constructors

        /// <summary>
        /// Constructor to initialise a new lookup-list
        /// </summary>
        /// <param name="boType">The business object type</param>
        public BusinessObjectLookupListSource(Type boType)
        {
            MyBoType = boType;
		}

    	/// <summary>
		/// Constructor to initialise a new lookup-list
		/// </summary>
		/// <param name="boType">The business object type</param>
		public BusinessObjectLookupListSource(string assemblyName, string className)
		{
			_assemblyName = assemblyName;
			_className = className;
			_boType = null;
		}

		#endregion Constructors

		#region Properties

		public string AssemblyName
		{
			get { return _assemblyName; }
			protected set
			{
				if (_assemblyName != value)
				{
					_className = null;
					_boType = null;
				}
				_assemblyName = value;
			}
		}

		public string ClassName
		{
			get { return _className; }
			protected set
			{
				if (_className != value)
				{
					_boType = null;
				}
				_className = value;
			}
		}

		#endregion Properties

		#region ILookupListSource Implementation

		/// <summary>
        /// Returns a lookup-list for all the business objects stored under
        /// the class definition held in this instance
        /// </summary>
        /// <returns>Returns a collection of string-value pairs</returns>
        public Dictionary<string, object> GetLookupList()
        {
            return this.GetLookupList(null);
        }

        /// <summary>
        /// Returns a lookup-list for all the business objects stored under
        /// the class definition held in this instance, using the database
        /// connection provided
        /// </summary>
        /// <param name="connection">The database connection</param>
        /// <returns>Returns a collection of string-value pairs</returns>
        public Dictionary<string, object> GetLookupList(IDatabaseConnection connection)
        {
            BusinessObjectCollection<BusinessObject> col = new BusinessObjectCollection<BusinessObject>(ClassDef.ClassDefs[MyBoType]);
            col.Load("", "");
            return CreateDisplayValueDictionary(col);
		}

		/// <summary>
		/// Returns a collection of string Guid pairs from the business object
		/// collection provided. Each pair consists of a string version of a
		/// business object and the object's ID.
		/// </summary>
		/// <param name="col">The business object collection</param>
		/// <returns>Returns a collection of display-value pairs</returns>
        public static Dictionary<string, object> CreateDisplayValueDictionary(BusinessObjectCollection<BusinessObject> col)
		{
            Dictionary<string, object> lookupList = new Dictionary<string, object>();
			foreach (BusinessObject bo in col)
			{
				lookupList.Add(bo.ToString(), bo);
			}
			return lookupList;
		}

		#endregion ILookupListSource Implementation

		#region Type Initialisation

		private Type MyBoType
		{
			get
			{
				TypeLoader.LoadClassType(ref _boType, _assemblyName, _className,
					"property", "property definition");
				return _boType;
			}
			set
			{
				_boType = value;
				TypeLoader.ClassTypeInfo(_boType, out _assemblyName, out _className);
			}
		}

		#endregion Type Initialisation

		/// <summary>
        /// Returns a collection of all the business objects stored under
        /// the class definition held in this instance.  The collection contains
        /// a string version of each of the business objects.
        /// </summary>
        /// <returns>Returns an ICollection object</returns>
        public ICollection GetValueCollection()
        {
            BusinessObjectCollection<BusinessObject> col = new BusinessObjectCollection<BusinessObject>(ClassDef.ClassDefs[MyBoType]);
            col.Load("", "");
            return CreateValueList(col);
        }

        /// <summary>
        /// Populates a collection with a string version of each business
        /// object in the collection provided
        /// </summary>
        /// <param name="col">The business object collection</param>
        /// <returns>Returns an ICollection object</returns>
        private static ICollection CreateValueList(BusinessObjectCollection<BusinessObject> col)
        {
            SortedStringCollection valueList = new SortedStringCollection();
            foreach (BusinessObject bo in col)
            {
                valueList.Add(bo.ToString());
            }
            return valueList;
		}

	}
}