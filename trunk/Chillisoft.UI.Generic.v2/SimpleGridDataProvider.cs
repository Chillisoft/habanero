using System;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Provides data to a facility
    /// that uses such data, such as a ReadOnlyGridWithButtons
    /// </summary>
    /// TODO ERIC - how does this differ from CollectionGridDataProvider?
    public class SimpleGridDataProvider : IGridDataProvider
    {
        private BusinessObjectBaseCollection itsCollection;
        private UIGridDef itsUIGridDef;

        /// <summary>
        /// Constructor to initialise a new provider
        /// </summary>
        /// <param name="collection">The business object collection to
        /// represent. This collection must have been preloaded using the 
        /// collection's Load() method.</param>
        /// <param name="uiGridDef">The UIGridDef object</param>
        public SimpleGridDataProvider(BusinessObjectBaseCollection collection, UIGridDef uiGridDef)
        {
            itsCollection = collection;
            itsUIGridDef = uiGridDef;
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
        /// Returns the UIGridDef object
        /// </summary>
        /// <returns>Returns the UIGridDef object</returns>
        public UIGridDef GetUIGridDef()
        {
            return itsUIGridDef;
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
            get { return itsCollection.ClassDef; }
        }
    }
}