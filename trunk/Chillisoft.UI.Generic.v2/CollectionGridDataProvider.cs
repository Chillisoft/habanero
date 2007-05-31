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
        private BusinessObjectBaseCollection itsCollection;
        private string itsUiDefName;

        /// <summary>
        /// Constructor to initialise a new provider
        /// </summary>
        /// <param name="collection">The business objects to represent.
        /// This collection must have been preloaded using the collection's
        /// Load() method.</param>
        public CollectionGridDataProvider(BusinessObjectBaseCollection collection) : this(collection, "")
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
            itsCollection = loader.Load();
            itsUiDefName = uiDefName;
        }

        /// <summary>
        /// A constructor as before, but allows a UIDefName to be specified
        /// </summary>
        public CollectionGridDataProvider(BusinessObjectBaseCollection collection, string uiDefName)
        {
            itsCollection = collection;
            itsUiDefName = uiDefName;
        }

        /// <summary>
        /// Returns the business object collection being represented
        /// </summary>
        /// <returns>Returns the collection</returns>
        public BusinessObjectBaseCollection GetCollection()
        {
            return itsCollection;
        }

        /// <summary>
        /// Returns the user interface definition
        /// </summary>
        /// <returns>Returns the definition</returns>
        public UIGridDef GetUIGridDef()
        {
            IUserInterfaceMapper uiDef;
            if (itsUiDefName != null && itsUiDefName.Length > 0)
            {
                uiDef = itsCollection.SampleBo.GetUserInterfaceMapper(itsUiDefName);
            }
            else
            {
                uiDef = itsCollection.SampleBo.GetUserInterfaceMapper();
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
            get { return itsCollection.ClassDef; }
        }
    }
}