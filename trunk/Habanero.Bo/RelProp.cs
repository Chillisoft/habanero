using Habanero.Bo.ClassDefinition;
using Habanero.Bo.CriteriaManager;
using NUnit.Framework;

namespace Habanero.Bo
{
    /// <summary>
    /// Represents the property on which two objects match up in a relationship
    /// </summary>
    public class RelProp
    {
        private BOProp _boProp;
        private RelPropDef _relPropDef;

        /// <summary>
        /// Constructor to initialise a new property
        /// </summary>
        /// <param name="mRelPropDef">The relationship property definition</param>
        /// <param name="lBoProp">The property</param>
        internal RelProp(RelPropDef mRelPropDef, BOProp lBoProp)
        {
            this._relPropDef = mRelPropDef;
            _boProp = lBoProp;
        }

        /// <summary>
        /// Returns the property name of the relationship owner
        /// </summary>
        internal string OwnerPropertyName
        {
            get { return _relPropDef.OwnerPropertyName; }
        }

        /// <summary>
        /// Returns the property name of the related object
        /// </summary>
        internal string RelatedClassPropName
        {
            get { return _relPropDef.RelatedClassPropName; }
        }

        /// <summary>
        /// Indicates if the property is null
        /// </summary>
        internal bool IsNull
        {
            get { return _boProp == null || _boProp.PropertyValue == null; }
        }

        /// <summary>
        /// Returns the related property's expression
        /// </summary>
        /// <returns>Returns an IExpression object</returns>
        internal IExpression RelatedPropExpression()
        {
            if (_boProp.PropertyValue == null)
            {
                return new Parameter(_relPropDef.RelatedClassPropName, "IS", "NULL");
            }
            return new Parameter(_relPropDef.RelatedClassPropName, "=", _boProp.PropertyValueString);
        }
    }

}