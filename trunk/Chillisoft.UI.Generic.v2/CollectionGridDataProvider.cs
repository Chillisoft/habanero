using System;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Provides data from a business object collection to a facility
    /// that uses such data, such as a ReadOnlyGridWithButtons.<br/>
    /// An example usage would be:<br/>
    /// <code>grid = new ReadOnlyGridWithButtons(new CollectionGridDataProvider(boCollection));</code>
    /// </summary>
    public class CollectionGridDataProvider : IGridDataProvider
    {
        private BusinessObjectCollection _collection;
        private string _uiDefName;

        /// <summary>
        /// Constructor to initialise a new provider
        /// </summary>
        /// <param name="collection">The business objects to represent.
        /// This collection must have been preloaded using the collection's
        /// Load() method.</param>
        public CollectionGridDataProvider(BusinessObjectCollection collection) : this(collection, "")
        {
        }

        /// <summary>
        /// A constructor as before, but specifies a loader that can retrieve
        /// the business objects to represent
        /// </summary>
        /// <param name="loader">The business object loader</param>
        public CollectionGridDataProvider(BusinessObjectCollectionLoader loader) : this(loader, "")
        {
        }

        /// <summary>
        /// A constructor as before, but allows a loader and a UIDefName 
        /// to be specified
        /// </summary>
        public CollectionGridDataProvider(BusinessObjectCollectionLoader loader, string uiDefName)
        {
            _collection = loader.Load();
            _uiDefName = uiDefName;
        }

        /// <summary>
        /// A constructor as before, but allows a UIDefName to be specified
        /// </summary>
        public CollectionGridDataProvider(BusinessObjectCollection collection, string uiDefName)
        {
            _collection = collection;
            _uiDefName = uiDefName;
        }

        /// <summary>
        /// Returns the business object collection being represented
        /// </summary>
        /// <returns>Returns the collection</returns>
        public BusinessObjectCollection GetCollection()
        {
            return _collection;
        }

        /// <summary>
        /// Returns the user interface definition
        /// </summary>
        /// <returns>Returns the definition</returns>
        public UIGridDef GetUIGridDef()
        {
            IUserInterfaceMapper uiDef;
            if (_uiDefName != null && _uiDefName.Length > 0)
            {
                uiDef = _collection.SampleBo.GetUserInterfaceMapper(_uiDefName);
            }
            else
            {
                uiDef = _collection.SampleBo.GetUserInterfaceMapper();
            }
            return uiDef.GetUIGridProperties();
        }

        /// <summary>
        /// This method has not yet been implemented
        /// </summary>
        /// <param name="parentObject"></param>
        /// TODO ERIC - implement
        public void SetParentObject(object parentObject)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the class definition of the business object collection
        /// </summary>
        public ClassDef ClassDef
        {
            get { return _collection.ClassDef; }
        }
    }
}