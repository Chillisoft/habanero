using Habanero.Base.Exceptions;
using Habanero.Bo;
using Habanero.Util;

namespace Habanero.Bo.ClassDefinition
{
    /// <summary>
    /// Lists a property on which two classes in a relationship are
    /// being matched
    /// </summary>
    public class RelPropDef
    {
        private PropDef _ownerPropDef;
		private string _relatedClassPropName;

		/// <summary>
        /// Constructor to create new RelPropDef object
        /// </summary>
        /// <param name="ownerClassPropDef">The property definition of the 
        /// owner object</param>
        /// <param name="relatedObjectPropName">The property name of the 
        /// related object</param>
        public RelPropDef(PropDef ownerClassPropDef,
                          string relatedObjectPropName)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(ownerClassPropDef, "ownerClassPropDef");
            _ownerPropDef = ownerClassPropDef;
            _relatedClassPropName = relatedObjectPropName;
		}

    	///<summary>
    	/// Gets or sets the property definition for the relationship owner
    	///</summary>
    	protected PropDef OwnerProperty
    	{
			get { return _ownerPropDef; }
			set
			{
				ArgumentValidationHelper.CheckArgumentNotNull(value, "value");
				_ownerPropDef = value;
			}
    	}

		/// <summary>
        /// Returns the property name for the relationship owner
        /// </summary>
        public string OwnerPropertyName
        {
            get { return _ownerPropDef.PropertyName; }
        }

        /// <summary>
        /// The property name to be matched to in the related class
        /// </summary>
        public string RelatedClassPropName
        {
			get { return _relatedClassPropName; }
			protected set { _relatedClassPropName = value; }
		}

		/// <summary>
        /// Creates a new RelProp object based on this property definition
        /// </summary>
        /// <param name="lBoPropCol">The collection of properties</param>
        /// <returns>The newly created RelProp object</returns>
        protected internal RelProp CreateRelProp(BOPropCol lBoPropCol)
        {
            BOProp lBoProp = lBoPropCol[OwnerPropertyName];
            return new RelProp(this, lBoProp);
        }
    }
}