using System;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Generic;

namespace Habanero.Ui.Generic
{
    /// <summary>
    /// Provides data to a facility
    /// that uses such data, such as a ReadOnlyGridWithButtons
    /// </summary>
    /// TODO ERIC - how does this differ from CollectionGridDataProvider?
    public class SimpleGridDataProvider : IGridDataProvider
    {
        private BusinessObjectCollection _collection;
        private UIGridDef _uiGridDef;

        /// <summary>
        /// Constructor to initialise a new provider
        /// </summary>
        /// <param name="collection">The business object collection to
        /// represent. This collection must have been preloaded using the 
        /// collection's Load() method.</param>
        /// <param name="uiGridDef">The UIGridDef object</param>
        public SimpleGridDataProvider(BusinessObjectCollection collection, UIGridDef uiGridDef)
        {
            _collection = collection;
            _uiGridDef = uiGridDef;
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
        /// Returns the UIGridDef object
        /// </summary>
        /// <returns>Returns the UIGridDef object</returns>
        public UIGridDef GetUIGridDef()
        {
            return _uiGridDef;
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
        /// Returns the collection's class definition
        /// </summary>
        public ClassDef ClassDef
        {
            get { return _collection.ClassDef; }
        }
    }
}