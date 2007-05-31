using System.Collections;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.CriteriaManager.v2;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Manages a relationship key
    /// </summary>
    /// TODO ERIC - what is the function of this class
    public class RelKey : DictionaryBase
    {
        private RelKeyDef mRelKeyDef;

        /// <summary>
        /// Constructor to initialise a new instance
        /// </summary>
        /// <param name="lRelKeyDef">The relationship key definition</param>
        public RelKey(RelKeyDef lRelKeyDef)
        {
            mRelKeyDef = lRelKeyDef;
        }

        /// <summary>
        /// Provides an indexing facility so that the properties can be
        /// accessed with square brackets like an array
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the RelProp object found with that name</returns>
        internal RelProp this[string propName]
        {
            get
            {
                //if (this.Contains(key)) //TODO_Err: If this does not exist
                return ((RelProp) Dictionary[propName]);
            }
        }

        /// <summary>
        /// Adds the given RelProp to the key
        /// </summary>
        /// <param name="relProp">The RelProp object to add</param>
        internal virtual void Add(RelProp relProp)
        {
            //TODO_Err: If this already exist
            Dictionary.Add(relProp.OwnerPropertyName, relProp);
        }

        /// <summary>
        /// Indicates whether a property with the given name is part of the key
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns true if a property with this name is held</returns>
        internal bool Contains(string propName)
        {
            return (Dictionary.Contains(propName));
        }

        /// <summary>
        /// Indicates if there is a related object.
        /// If all relationship properties are null then it is assumed that 
        /// there is no related object.
        /// </summary>
        /// <returns>Returns true if there is a valid relationship</returns>
        internal bool HasRelatedObject()
        {
            foreach (DictionaryEntry item in this)
            {
                RelProp relProp = (RelProp) item.Value;
                if (! (relProp.IsNull))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the relationship expression
        /// </summary>
        /// <returns>Returns an IExpression object</returns>
        internal IExpression RelationshipExpression()
        {
            if (Count >= 1)
            {
                IExpression exp = null;
                foreach (DictionaryEntry item in this)
                {
                    RelProp relProp = (RelProp) item.Value;
                    if (exp == null)
                    {
                        exp = relProp.RelatedPropExpression();
                    }
                    else
                    {
                        exp = new Expression(exp, new SqlOperator("AND"), relProp.RelatedPropExpression());
                    }
                }
                return exp;
            }
            return null;
        }
    }
}