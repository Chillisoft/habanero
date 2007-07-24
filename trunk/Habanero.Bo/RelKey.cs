using System;
using System.Collections.Generic;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.CriteriaManager;

namespace Habanero.Bo
{
    /// <summary>
    /// Holds a collection of properties on which two classes in a relationship
    /// are matching
    /// </summary>
    public class RelKey 
    {
        private RelKeyDef _relKeyDef;
        private Dictionary<string, RelProp> _relProps;

        /// <summary>
        /// Constructor to initialise a new instance
        /// </summary>
        /// <param name="lRelKeyDef">The relationship key definition</param>
        public RelKey(RelKeyDef lRelKeyDef)
        {
            _relKeyDef = lRelKeyDef;
            _relProps = new Dictionary<string, RelProp>();
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
                if (!_relProps.ContainsKey(propName))
                {
                    throw new InvalidPropertyNameException(String.Format(
                        "A related property with the name '{0}' is being " +
                        "accessed, but no property with that name exists in " +
                        "the relationship's collection.", propName));
                }
                return (_relProps[propName]);
            }
        }

        /// <summary>
        /// Adds the given RelProp to the key
        /// </summary>
        /// <param name="relProp">The RelProp object to add</param>
        internal virtual void Add(RelProp relProp)
        {
            if (_relProps.ContainsKey(relProp.OwnerPropertyName))
            {
                throw new InvalidPropertyException(String.Format(
                    "A related property with the name '{0}' is being added " +
                    "to a collection, but already exists in the collection.",
                    relProp.OwnerPropertyName));
            }
            _relProps.Add(relProp.OwnerPropertyName, relProp);
        }

        /// <summary>
        /// Indicates whether a property with the given name is part of the key
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns true if a property with this name is held</returns>
        internal bool Contains(string propName)
        {
            return (_relProps.ContainsKey(propName));
        }

        /// <summary>
        /// Indicates if there is a related object.
        /// If all relationship properties are null then it is assumed that 
        /// there is no related object.
        /// </summary>
        /// <returns>Returns true if there is a valid relationship</returns>
        internal bool HasRelatedObject()
        {
            foreach (RelProp relProp in this)
            {
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
            if (_relProps.Count >= 1)
            {
                IExpression exp = null;
                foreach (RelProp relProp in this)
                {
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

        /// <summary>
        /// Returns an enumrated for theis RelKey to iterate through its RelProps
        /// </summary>
        /// <returns></returns>
        public IEnumerator<RelProp> GetEnumerator()
        {
            return _relProps.Values.GetEnumerator();
        }
    }
}