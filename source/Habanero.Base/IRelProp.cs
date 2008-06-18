namespace Habanero.Base
{
    public interface IRelProp
    {
        /// <summary>
        /// Returns the property name of the relationship owner
        /// </summary>
        string OwnerPropertyName
        {
            get;
        }

        /// <summary>
        /// Returns the property name of the related object
        /// </summary>
        string RelatedClassPropName
        {
            get;
        }

        /// <summary>
        /// The BoProp this RelProp requires to generate its search expression
        /// </summary>
        IBOProp BOProp
        {
            get;
        }
    }
}