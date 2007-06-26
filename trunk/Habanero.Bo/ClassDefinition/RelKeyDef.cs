using System.Collections;
using Habanero.Base.Exceptions;
using Habanero.Bo;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Bo.ClassDefinition
{
    /// <summary>
    /// The key definition that is being related to in a 
    /// relationship between objects
    /// </summary>
    /// TODO ERIC - review
    public class RelKeyDef : DictionaryBase
    {
        /// <summary>
        /// Constructor to create a new RelKeyDef object
        /// </summary>
        public RelKeyDef() : base()
        {
        }

        /// <summary>
        /// Provides an indexing facility for the property definitions
        /// in this key definition so that they can be 
        /// accessed like an array (e.g. collection["surname"])
        /// </summary>
        /// <param name="propName">The name of the property</param>
        /// <returns>Returns the corresponding RelPropDef object</returns>
        public RelPropDef this[string propName]
        {
            get
            {
                //if (this.Contains(key)) //TODO_Err: If this does not exist
                return ((RelPropDef) Dictionary[propName]);
            }
        }

        /// <summary>
        /// Adds the related property definition to this key, as long as
        /// a property by that name has not already been added.
        /// </summary>
        /// <param name="lRelPropDef">The RelPropDef object to be added.</param>
        /// <exception cref="HabaneroArgumentException">Thrown if the
        /// argument passed is null</exception>
        public virtual void Add(RelPropDef lRelPropDef)
        {
            if (lRelPropDef == null)
            {
                throw new HabaneroArgumentException("lPropDef",
                                                   "ClassDef-Add. You cannot add a null prop def to a classdef");
            }
            if (!Dictionary.Contains(lRelPropDef.OwnerPropertyName))
            {
                Dictionary.Add(lRelPropDef.OwnerPropertyName, lRelPropDef);
            }
        }

		/// <summary>
		/// Removes a Related Property definition from the key
		/// </summary>
		/// <param name="relPropDef">The Related Property Definition to remove</param>
		protected void Remove(RelPropDef relPropDef)
		{
			if (Dictionary.Contains(relPropDef.OwnerPropertyName))
			{
				base.Dictionary.Remove(relPropDef.OwnerPropertyName);
			}
		}

        /// <summary>
        /// Returns true if a property with this name is part of this key.
        /// </summary>
        /// <param name="propName">The property name to search by</param>
        /// <returns>Returns true if found, false if not</returns>
        internal bool Contains(string propName)
        {
            return (Dictionary.Contains(propName));
        }

        /// <summary>
        /// Create a relationship key based on this key definition and
        /// its associated property definitions
        /// </summary>
        /// <param name="lBoPropCol">The collection of properties</param>
        /// <returns>Returns the new RelKey object</returns>
        public RelKey CreateRelKey(BOPropCol lBoPropCol)
        {
            RelKey lRelKey = new RelKey(this);
            foreach (DictionaryEntry item in this)
            {
                RelPropDef relPropDef = (RelPropDef) item.Value;

                lRelKey.Add(relPropDef.CreateRelProp(lBoPropCol));
            }
            return lRelKey;
        }
    }


    #region Tests

    [TestFixture]
    public class RelKeyDefTester
    {
        private RelKeyDef mRelKeyDef;
        private PropDefCol mPropDefCol;

        [SetUp]
        public void init()
        {
            mRelKeyDef = new RelKeyDef();
            mPropDefCol = new PropDefCol();

            PropDef propDef = new PropDef("Prop", typeof (string), PropReadWriteRule.ReadWrite, "1");

            mPropDefCol.Add(propDef);
            RelPropDef lRelPropDef = new RelPropDef(propDef, "PropName");
            mRelKeyDef.Add(lRelPropDef);

            propDef = new PropDef("Prop2", typeof (string), PropReadWriteRule.ReadWrite, "2");

            mPropDefCol.Add(propDef);
            lRelPropDef = new RelPropDef(propDef, "PropName2");
            mRelKeyDef.Add(lRelPropDef);
        }

        [Test]
        public void TestAddPropDef()
        {
            Assert.AreEqual(2, mRelKeyDef.Count);
        }

        [Test]
        public void TestContainsPropDef()
        {
            Assert.IsTrue(mRelKeyDef.Contains("Prop"));
            RelPropDef lPropDef = mRelKeyDef["Prop"];
            Assert.AreEqual("Prop", lPropDef.OwnerPropertyName);
        }

        [Test]
        public void TestCreateRelKey()
        {
            BOPropCol propCol = mPropDefCol.CreateBOPropertyCol(true);
            RelKey relKey = mRelKeyDef.CreateRelKey(propCol);
            Assert.IsTrue(relKey.Contains("Prop"));
            Assert.IsTrue(relKey.Contains("Prop2"));
            RelProp relProp = relKey["Prop"];
            Assert.AreEqual("Prop", relProp.OwnerPropertyName);
            Assert.AreEqual("PropName", relProp.RelatedClassPropName);
            relProp = relKey["Prop2"];
            Assert.AreEqual("Prop2", relProp.OwnerPropertyName);
            Assert.AreEqual("PropName2", relProp.RelatedClassPropName);

            Assert.IsTrue(relKey.HasRelatedObject(),
                          "This is true since the values for the properties should have defaulted to 1 each");

            Assert.AreEqual("(PropName = '1' AND PropName2 = '2')", relKey.RelationshipExpression().ExpressionString());
        }
    }

    #endregion //Tests

}