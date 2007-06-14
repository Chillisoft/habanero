using System;
using Chillisoft.Bo.v2;
using Chillisoft.Db.v2;
using Chillisoft.Test;
using Chillisoft.Test.General.v2;
using Chillisoft.Util.v2;
using NUnit.Framework;

namespace Chillisoft.Bo.ClassDefinition.v2
{
    /// <summary>
    /// An enumeration that gives some instructions or limitations in the
    /// case where a parent is to be deleted.
    /// </summary>
    public enum DeleteParentAction
    {
        /// <summary>Delete all related objects when the parent is deleted</summary>
        DeleteRelatedObjects = 1,
        /// <summary>Dereference all related objects when the parent is deleted</summary>
        DereferenceRelatedObjects = 2,
        /// <summary>Prevent deletion of parent if it has related objects</summary>
        PreventDeleteParent = 3,
    }

    /// <summary>
    /// Defines a relationship where the owner may relate to more than one object.
    /// </summary>
    public class MultipleRelationshipDef : RelationshipDef
    {
        protected string _orderBy;
        protected int _minNoOfRelatedObjects;
        protected int _maxNoOfRelatedObjects;
        protected DeleteParentAction _deleteParentAction;

		#region Constructors

		/// <summary>
        /// Constructor to create a new Multiple Relationship Definition
        /// </summary>
        /// <param name="relationshipName">A name for the relationship</param>
        /// <param name="relatedObjectClassType">The class type of the related objects</param>
        /// <param name="relKeyDef">The related key definition</param>
        /// <param name="keepReferenceToRelatedObject">Whether to keep a
        /// reference to the related object</param>
        /// <param name="orderBy">The sql order-by clause</param>
        /// <param name="minNoOfRelatedObjects">Minimum required number of objects
        /// to relate to</param>
        /// <param name="maxNoOfRelatedObjects">Maximum possible number of objects
        /// to relate to</param>
        /// <param name="deleteParentAction">Provides specific instructions 
        /// with regards to deleting a parent object.  See the DeleteParentAction 
        /// enumeration for more detail.</param>
        /// TODO ERIC - review keepref param
        public MultipleRelationshipDef(string relationshipName,
                                       Type relatedObjectClassType,
                                       RelKeyDef relKeyDef,
                                       bool keepReferenceToRelatedObject,
                                       //				IExpression searchCriteria, 
                                       string orderBy,
                                       int minNoOfRelatedObjects,
                                       int maxNoOfRelatedObjects,
                                       DeleteParentAction deleteParentAction) : base(relationshipName,
                                                                                     relatedObjectClassType, relKeyDef,
                                                                                     keepReferenceToRelatedObject)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(orderBy, "orderBy");

            _orderBy = orderBy;
            _minNoOfRelatedObjects = minNoOfRelatedObjects;
            _maxNoOfRelatedObjects = maxNoOfRelatedObjects;
            _deleteParentAction = deleteParentAction;
		}

    	/// <summary>
    	/// Constructor to create a new single relationship definition
    	/// </summary>
    	/// <param name="relationshipName">A name for the relationship</param>
    	/// <param name="relatedObjectAssemblyName">The assembly name of the related object</param>
    	/// <param name="relatedObjectClassName">The class name of the related object</param>
    	/// <param name="relKeyDef">The related key definition</param>
    	/// <param name="keepReferenceToRelatedObject">Whether to keep a
    	/// reference to the related object</param>
    	/// <param name="orderBy">The sql order-by clause</param>
    	/// <param name="minNoOfRelatedObjects">Minimum required number of objects
    	/// to relate to</param>
    	/// <param name="maxNoOfRelatedObjects">Maximum possible number of objects
    	/// to relate to</param>
    	/// <param name="deleteParentAction">Provides specific instructions 
    	/// with regards to deleting a parent object.  See the DeleteParentAction 
    	/// enumeration for more detail.</param>
    	/// TODO ERIC - review keepref param
    	public MultipleRelationshipDef(string relationshipName, string relatedObjectAssemblyName,
    	                               string relatedObjectClassName, RelKeyDef relKeyDef,
    	                               bool keepReferenceToRelatedObject, string orderBy,
    	                               int minNoOfRelatedObjects,
    	                               int maxNoOfRelatedObjects,
    	                               DeleteParentAction deleteParentAction)
    		: base(relationshipName, relatedObjectAssemblyName, relatedObjectClassName, relKeyDef, keepReferenceToRelatedObject)
		{
			ArgumentValidationHelper.CheckArgumentNotNull(orderBy, "orderBy");
			
			_orderBy = orderBy;
			_minNoOfRelatedObjects = minNoOfRelatedObjects;
			_maxNoOfRelatedObjects = maxNoOfRelatedObjects;
			_deleteParentAction = deleteParentAction;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
        /// Returns the sql order-by clause
        /// </summary>
        public string OrderBy
        {
            get { return _orderBy; }
			protected set { _orderBy = value; }
        }

        /// <summary>
        /// Indicates the minimum number of related objects that the owner
        /// object must have to be valid (e.g. A car must have at least 4 wheels.)
        /// </summary>
        public int MinNoOfRelatedObjects
        {
			get { return _minNoOfRelatedObjects; }
			protected set { _minNoOfRelatedObjects = value; }
        }

        /// <summary>
        /// Indicates the maximum number of related objects that the owner 
        /// object can have and still be valid (e.g. A person cannot have 
        /// more than two biological parents.)
        /// </summary>
        public int MaxNoOfRelatedObjects
        {
			get { return _maxNoOfRelatedObjects; }
			protected set { _maxNoOfRelatedObjects = value; }
        }

        /// <summary>
        /// Provides specific instructions with regards to deleting a parent
        /// object.  See the DeleteParentAction enumeration for more detail.
        /// </summary>
        public DeleteParentAction DeleteParentAction
        {
			get { return _deleteParentAction; }
			protected set { _deleteParentAction = value; }
		}

		#endregion Properties

		/// <summary>
		/// Overrides abstract method of parent to facilitate creation of 
		/// a new Multiple Relationship
		/// </summary>
		/// <param name="owningBo">The business object that will manage
		/// this relationship</param>
		/// <param name="lBOPropCol">The collection of properties</param>
		/// <returns>Returns the new relationship that has been created</returns>
		internal override Relationship CreateRelationship(BusinessObject owningBo, BOPropCol lBOPropCol)
		{
			return new MultipleRelationship(owningBo, this, lBOPropCol);
		}
    }

    #region testing

    [TestFixture]
    public class UnrefRelationshipDefTester : TestUsingDatabase
    {
        private RelationshipDef mRelationshipDef;
        private RelKeyDef mRelKeyDef;
        private PropDefCol mPropDefCol;
        private MockBO mMockBo;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.SetupDBConnection();
        }

        [SetUp]
        public void init()
        {
            mMockBo = new MockBO();
            mPropDefCol = mMockBo.PropDefCol;

            mRelKeyDef = new RelKeyDef();
            PropDef propDef = mPropDefCol["MockBOID"];

            RelPropDef lRelPropDef = new RelPropDef(propDef, "MockBOProp1");
            mRelKeyDef.Add(lRelPropDef);

            mRelationshipDef = new MultipleRelationshipDef("Relation1", typeof (MockBO),
                                                           mRelKeyDef, false, "",
                                                           -1, -1, DeleteParentAction.DeleteRelatedObjects);
            DatabaseConnection.CurrentConnection.ConnectionString = MyDBConnection.GetConnectionString();
        }

        [Test]
        public void TestCreateRelationshipDef()
        {
            Assert.AreEqual("Relation1", mRelationshipDef.RelationshipName);
            Assert.AreEqual(typeof (MockBO), mRelationshipDef.RelatedObjectClassType);
            Assert.AreEqual(mRelKeyDef, mRelationshipDef.RelKeyDef);
        }

        [Test]
        [ExpectedException(typeof (HabaneroArgumentException))]
        public void TestCreateRelationshipWithNonBOType()
        {
            RelationshipDef relDef = new MultipleRelationshipDef("Relation1", typeof (String), mRelKeyDef, false, "",
                                                                 -1, -1, DeleteParentAction.DeleteRelatedObjects);
        }

        [Test]
        public void TestCreateRelationship()
        {
            MultipleRelationship rel =
                (MultipleRelationship) mRelationshipDef.CreateRelationship(mMockBo, mMockBo.PropCol);
            Assert.AreEqual(mRelationshipDef.RelationshipName, rel.RelationshipName);

            Assert.IsTrue(mMockBo.GetPropertyValue("MockBOProp1") == null);

            Assert.IsTrue(rel.GetRelatedBusinessObjectCol().Count == 0);

            //			mMockBo.SetPropertyValue("MockBOProp1",mMockBo.GetPropertyValue("MockBOID"));
            //			mMockBo.ApplyEdit();
            //			Assert.IsTrue (rel.HasRelationship(), "Should be true since prop MockBOProp1 has been set");
            //
            //			Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1") ,mMockBo.GetPropertyValue("MockBOID"));
            //TODO:
            //			MockBO ltempBO = (MockBO) rel.GetRelatedObject();
            //			Assert.IsFalse(ltempBO == null);
            //			Assert.AreEqual(mMockBo.GetPropertyValue("MockBOID") ,ltempBO.GetPropertyValue("MockBOID"), "The object returned should be the one with the ID = MockBOID");
            //			Assert.AreEqual(mMockBo.GetPropertyValueString("MockBOProp1") ,ltempBO.GetPropertyValueString("MockBOID"), "The object returned should be the one with the ID = MockBOID");
            //			Assert.AreEqual(mMockBo.GetPropertyValue("MockBOProp1") ,ltempBO.GetPropertyValue("MockBOID"), "The object returned should be the one with the ID = MockBOID");
        }
    }

    #endregion //Testing
}