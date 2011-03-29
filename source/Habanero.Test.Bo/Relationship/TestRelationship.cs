using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO.ClassDefinition;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO.Relationship
{
    [TestFixture]
    public class TestRelationship
    {
        #region Setup/Teardown

        [SetUp]
        public void Init()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            new Address();
        }

        #endregion

        [TestFixtureSetUp]
        public void SetupFixture()
        {
        }

        private static MockBO GetMockBO(out RelationshipDef mRelationshipDef, out RelKeyDef mRelKeyDef)
        {
            MockBO _mMockBO= new MockBO();
            IPropDefCol mPropDefCol = _mMockBO.PropDefCol;
            mRelKeyDef = new RelKeyDef();
            IPropDef propDef = mPropDefCol["MockBOProp1"];
            RelPropDef lRelPropDef = new RelPropDef(propDef, "MockBOID");
            mRelKeyDef.Add(lRelPropDef);
            mRelationshipDef = new SingleRelationshipDef("Relation1", typeof(MockBO), mRelKeyDef, false,
                                                         DeleteParentAction.Prevent);
            return _mMockBO;
        }

        [Test]
        public void Test_IsDeletable_WhenDeleteAction_EQ_DeleteRelated_WhenHasRelatedObject_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO) classDef.CreateNewBusinessObject();
            bo.Save();
            IRelationship relationship = bo.Relationships["MyMultipleRelationship"];
            SetDeleteRelatedAction(relationship, DeleteParentAction.DeleteRelated);
            bo.MyMultipleRelationship.CreateBusinessObject();
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.AreEqual(1, bo.MyMultipleRelationship.Count);
            Assert.AreEqual(DeleteParentAction.DeleteRelated, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            string message;
            bool isDeletable = relationship.IsDeletable(out message);
            //---------------Test Result -----------------------
            Assert.IsTrue(isDeletable);
        }

        private void SetDeleteRelatedAction(IRelationship relationship, DeleteParentAction deleteParentAction)
        {
            ((RelationshipDef)relationship.RelationshipDef).DeleteParentAction = deleteParentAction;
        }

        [Test]
        public void Test_IsDeletable_WhenDeleteAction_EQ_DeleteRelated_WhenNotHasRelated_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.Save();
            IRelationship relationship = bo.Relationships["MyMultipleRelationship"];
            SetDeleteRelatedAction(relationship, DeleteParentAction.DeleteRelated);
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.AreEqual(0, bo.MyMultipleRelationship.Count);
            Assert.AreEqual(DeleteParentAction.DeleteRelated, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            string message;
            bool isDeletable = relationship.IsDeletable(out message);
            //---------------Test Result -----------------------
            Assert.IsTrue(isDeletable);
        }

        [Test]
        public void Test_IsDeletable_WhenDeleteAction_EQ_DereferenceRelated_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO) classDef.CreateNewBusinessObject();
            bo.Save();
            IRelationship relationship = bo.Relationships["MyMultipleRelationship"];
            SetDeleteRelatedAction(relationship, DeleteParentAction.DereferenceRelated);
            bo.MyMultipleRelationship.CreateBusinessObject();
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.AreEqual(1, bo.MyMultipleRelationship.Count);
            Assert.AreEqual(DeleteParentAction.DereferenceRelated, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            string message;
            bool isDeletable = relationship.IsDeletable(out message);
            //---------------Test Result -----------------------
            Assert.IsTrue(isDeletable);
        }

        [Test]
        public void Test_IsDeletable_WhenDeleteAction_EQ_PreventDelete_WhenHasRelated_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO) classDef.CreateNewBusinessObject();
            bo.Save();
            IRelationship relationship = bo.Relationships["MyMultipleRelationship"];
            SetDeleteRelatedAction(relationship, DeleteParentAction.Prevent);
            bo.MyMultipleRelationship.CreateBusinessObject();
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.AreEqual(1, bo.MyMultipleRelationship.Count);
            Assert.AreEqual(DeleteParentAction.Prevent, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            string message;
            bool isDeletable = relationship.IsDeletable(out message);
            //---------------Test Result -----------------------
            Assert.IsFalse(isDeletable);
        }

        [Test]
        public void Test_IsDeletable_WhenDeleteAction_EQ_PreventDelete_WhenNotHasRelated_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO) classDef.CreateNewBusinessObject();
            bo.Save();
            IRelationship relationship = bo.Relationships["MyMultipleRelationship"];
            SetDeleteRelatedAction(relationship, DeleteParentAction.DeleteRelated);
            bo.MyMultipleRelationship.CreateBusinessObject();
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.AreEqual(1, bo.MyMultipleRelationship.Count);
            Assert.AreEqual(DeleteParentAction.DeleteRelated, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            string message;
            bool isDeletable = relationship.IsDeletable(out message);
            //---------------Test Result -----------------------
            Assert.IsTrue(isDeletable);
        }

        [Test]
        public void Test_IsDeletable_WhenDeleteActionEqDoNothing_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO) classDef.CreateNewBusinessObject();
            bo.Save();
            IRelationship relationship = bo.Relationships["MyMultipleRelationship"];
            SetDeleteRelatedAction(relationship, DeleteParentAction.DoNothing);
            bo.MyMultipleRelationship.CreateBusinessObject();
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.AreEqual(1, bo.MyMultipleRelationship.Count);
            Assert.AreEqual(DeleteParentAction.DoNothing, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            string message;
            bool isDeletable = relationship.IsDeletable(out message);
            //---------------Test Result -----------------------
            Assert.IsTrue(isDeletable);
        }


   [Test]
        public void Test_IsDeletable_WhenSingle_WhenDeleteAction_EQ_PreventDelete_WhenHasRelated_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO) classDef.CreateNewBusinessObject();
            bo.Save();
            SingleRelationship<MyRelatedBo> relationship = (SingleRelationship<MyRelatedBo>)bo.Relationships["MyRelationship"];
            relationship.SetRelatedObject(new MyRelatedBo());
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.IsNotNull(relationship.GetRelatedObject());
            Assert.AreEqual(DeleteParentAction.Prevent, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            string message;
            bool isDeletable = relationship.IsDeletable(out message);
            //---------------Test Result -----------------------
            Assert.IsFalse(isDeletable);
        }

        [Test]
        public void Test_IsDeletable_WhenSingle_WhenDeleteAction_EQ_PreventDelete_WhenNotHasRelated_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO) classDef.CreateNewBusinessObject();
            bo.Save();
            SingleRelationship<MyRelatedBo> relationship = (SingleRelationship<MyRelatedBo>)bo.Relationships["MyRelationship"];

            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.Status.IsDeleted);
            Assert.IsNull(relationship.GetRelatedObject());
            Assert.AreEqual(DeleteParentAction.Prevent, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            string message;
            bool isDeletable = relationship.IsDeletable(out message);
            //---------------Test Result -----------------------
            Assert.IsTrue(isDeletable);
        }

        [Test]
        public void TestChangedEnginesForeignKey_Dereference_Single()
        {
            //---------------Set up test pack-------------------
            Car car = new Car();
            Car car2 = new Car();
            Engine engine = new Engine {CarID = car.CarID};
            car.Save();
            car2.Save();
            engine.Save();

            //---------------Assert Precondition----------------
            Assert.AreSame(engine, car.GetEngine());
            Assert.AreSame(car, engine.GetCar());

            //---------------Execute Test ----------------------
            engine.CarID = car2.CarID;
            Engine loadedEngine = car.GetEngine();

            //---------------Test Result -----------------------
            Assert.IsNull(loadedEngine);
        }

        [Test]
        public void TestChangedEnginesForeignKey_Dereference_Single_Saved()
        {
            //---------------Set up test pack-------------------
            Car car = new Car();
            Car car2 = new Car();
            Engine engine = new Engine();
            engine.CarID = car.CarID;
            car.Save();
            car2.Save();
            engine.Save();

            //---------------Assert Precondition----------------
            Assert.AreSame(engine, car.GetEngine());
            Assert.AreSame(car, engine.GetCar());

            //---------------Execute Test ----------------------
            engine.CarID = car2.CarID;
            engine.Save();
            Engine loadedEngine = car.GetEngine();

            //---------------Test Result -----------------------
            Assert.IsNull(loadedEngine);
        }

        [Test]
        public void TestChangedEnginesForeignKey_SetIDToNull()
        {
            //---------------Set up test pack-------------------
            Car car = new Car();
            Engine engine = new Engine {CarID = car.CarID};
            car.Save();
            engine.Save();

            //---------------Assert Precondition----------------
            Assert.AreSame(engine, car.GetEngine());
            Assert.AreSame(car, engine.GetCar());

            //---------------Execute Test ----------------------
            engine.SetPropertyValue("CarID", null);
            Engine loadedEngine = car.GetEngine();

            //---------------Test Result -----------------------
            Assert.IsNull(engine.GetPropertyValue("CarID"));
            //Assert.IsNull(car.GetPropertyValue("EngineID"));
            Assert.IsNull(loadedEngine);
        }

        [Test]
        public void Test_MarkForDelete_WhenMultiple_WhenDeleteRelated__WhenHasRelatedBO_ShouldMarkForDeleteRelated()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.Save();
            ReflectionUtilities.SetPropertyValue(bo.Status, "IsDeleted", true);
            MyRelatedBo myRelatedBO = bo.MyMultipleRelationship.CreateBusinessObject();
            IRelationship relationship = bo.Relationships["MyMultipleRelationship"];
            ((RelationshipDef)relationship.RelationshipDef).DeleteParentAction = DeleteParentAction.DeleteRelated;

            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsFalse(myRelatedBO.Status.IsDeleted);
            Assert.AreEqual(1, bo.MyMultipleRelationship.Count);
            Assert.AreEqual(DeleteParentAction.DeleteRelated, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            relationship.MarkForDelete();
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsTrue(myRelatedBO.Status.IsDeleted);
        }

        [Test]
        public void Test_MarkForDelete_WhenMultiple_WhenDeleteRelated_WhenNotHasRelatedBO_ShouldDoNothing()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.Save();
            ReflectionUtilities.SetPropertyValue(bo.Status, "IsDeleted", true);
            IRelationship relationship = bo.Relationships["MyMultipleRelationship"];
            ((RelationshipDef)relationship.RelationshipDef).DeleteParentAction = DeleteParentAction.DeleteRelated;

            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.AreEqual(0, bo.MyMultipleRelationship.Count);
            Assert.AreEqual(DeleteParentAction.DeleteRelated, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            relationship.MarkForDelete();
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsDeleted);
        }

        [Test]
        public void Test_MarkForDelete_WhenMultiple_WhenDerefenceRelated_WhenHasRelatedBO_ShouldDoNothing()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.Save();
            ReflectionUtilities.SetPropertyValue(bo.Status,"IsDeleted", true);
            MyRelatedBo myRelatedBO = bo.MyMultipleRelationship.CreateBusinessObject();
            IRelationship relationship = bo.Relationships["MyMultipleRelationship"];
            ((RelationshipDef)relationship.RelationshipDef).DeleteParentAction = DeleteParentAction.DereferenceRelated;
     
            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsFalse(myRelatedBO.Status.IsDeleted);
            Assert.AreEqual(1, bo.MyMultipleRelationship.Count);
            Assert.AreEqual(DeleteParentAction.DereferenceRelated, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            relationship.MarkForDelete();
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsFalse(myRelatedBO.Status.IsDeleted);
        }

        [Test]
        public void Test_MarkForDelete_WhenMultiple_WhenDerefenceRelated_WhenNotHasRelatedBO_ShouldDoNothing()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.Save();
            ReflectionUtilities.SetPropertyValue(bo.Status,"IsDeleted", true);
            IRelationship relationship = bo.Relationships["MyMultipleRelationship"];
            ((RelationshipDef)relationship.RelationshipDef).DeleteParentAction = DeleteParentAction.DereferenceRelated;
            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.AreEqual(0, bo.MyMultipleRelationship.Count);
            Assert.AreEqual(DeleteParentAction.DereferenceRelated, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            relationship.MarkForDelete();
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsDeleted);
        }

        [Test]
        public void Test_MarkForDelete_WhenSingle_WhenDerefenceRelated_WhenHasRelatedBO_ShouldDoNothing()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.Save();
            ReflectionUtilities.SetPropertyValue(bo.Status, "IsDeleted", true);
            ISingleRelationship relationship = (ISingleRelationship)bo.Relationships["MyRelationship"];
            MyRelatedBo myRelatedBO = new MyRelatedBo();
            relationship.SetRelatedObject(myRelatedBO);
            SetDeleteRelatedAction(relationship, DeleteParentAction.DereferenceRelated);
            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsFalse(myRelatedBO.Status.IsDeleted);
            Assert.IsNotNull(relationship.GetRelatedObject());
            Assert.AreEqual(DeleteParentAction.DereferenceRelated, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            relationship.MarkForDelete();
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsFalse(myRelatedBO.Status.IsDeleted);
        }

        [Test]
        public void Test_MarkForDelete_WhenSingle_WhenDerefenceRelated_WhenNotHasRelatedBO_ShouldDoNothing()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.Save();
            ReflectionUtilities.SetPropertyValue(bo.Status, "IsDeleted", true);
            ISingleRelationship relationship = (ISingleRelationship)bo.Relationships["MyRelationship"];
            SetDeleteRelatedAction(relationship, DeleteParentAction.DereferenceRelated);
            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsNull(relationship.GetRelatedObject());
            Assert.AreEqual(DeleteParentAction.DereferenceRelated, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            relationship.MarkForDelete();
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsDeleted);
        }

        [Test]
        public void Test_MarkForDelete_WhenSingle_WhenDeleteRelated_WhenHasRelatedBO_ShouldDoNothing()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.Save();
            ReflectionUtilities.SetPropertyValue(bo.Status, "IsDeleted", true);
            ISingleRelationship relationship = (ISingleRelationship)bo.Relationships["MyRelationship"];
            MyRelatedBo myRelatedBO = new MyRelatedBo();
            relationship.SetRelatedObject(myRelatedBO);
            SetDeleteRelatedAction(relationship, DeleteParentAction.DeleteRelated);


            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsFalse(myRelatedBO.Status.IsDeleted);
            Assert.IsNotNull(relationship.GetRelatedObject());
            Assert.AreEqual(DeleteParentAction.DeleteRelated, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            relationship.MarkForDelete();
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsTrue(myRelatedBO.Status.IsDeleted);
        }

        [Test]
        public void Test_MarkForDelete_WhenSingle_WhenDeleteRelated_WhenNotHasRelatedBO_ShouldDoNothing()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = (MyBO)classDef.CreateNewBusinessObject();
            bo.Save();
            ReflectionUtilities.SetPropertyValue(bo.Status, "IsDeleted", true);
            ISingleRelationship relationship = (ISingleRelationship)bo.Relationships["MyRelationship"];
            SetDeleteRelatedAction(relationship, DeleteParentAction.DeleteRelated);
            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsDeleted);
            Assert.IsNull(relationship.GetRelatedObject());
            Assert.AreEqual(DeleteParentAction.DeleteRelated, relationship.DeleteParentAction);
            //---------------Execute Test ----------------------
            relationship.MarkForDelete();
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsDeleted);
        }

        [Test]
        public void TestCreateRelationship()
        {
            RelationshipDef mRelationshipDef;
            RelKeyDef mRelKeyDef;
            MockBO _mMockBO= GetMockBO(out mRelationshipDef, out mRelKeyDef);

            ISingleRelationship rel =
                (ISingleRelationship) mRelationshipDef.CreateRelationship(_mMockBO, _mMockBO.PropCol);
            Assert.AreEqual(mRelationshipDef.RelationshipName, rel.RelationshipName);
            Assert.IsTrue(_mMockBO.GetPropertyValue("MockBOProp1") == null);
            Assert.IsFalse(rel.HasRelatedObject(), "Should be false since props are not defaulted in Mock bo");
            _mMockBO.SetPropertyValue("MockBOProp1", _mMockBO.GetPropertyValue("MockBOID"));
            _mMockBO.Save();
            Assert.IsTrue(rel.HasRelatedObject(), "Should be true since prop MockBOProp1 has been set");

            Assert.AreEqual(_mMockBO.GetPropertyValue("MockBOProp1"), _mMockBO.GetPropertyValue("MockBOID"));
            MockBO ltempBO = (MockBO) rel.GetRelatedObject();
            Assert.IsFalse(ltempBO == null);
            Assert.AreEqual(_mMockBO.GetPropertyValue("MockBOID"), ltempBO.GetPropertyValue("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual(_mMockBO.GetPropertyValueString("MockBOProp1"), ltempBO.GetPropertyValueString("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual(_mMockBO.GetPropertyValue("MockBOProp1"), ltempBO.GetPropertyValue("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");

            Assert.AreSame(ltempBO, rel.GetRelatedObject());
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            Assert.AreSame(ltempBO, rel.GetRelatedObject());
            _mMockBO.MarkForDelete();
            _mMockBO.Save();
        }

        [Test]
        public void TestCreateRelationshipHoldRelRef()
        {
            RelationshipDef mRelationshipDef;
            RelKeyDef mRelKeyDef;
            MockBO _mMockBO= GetMockBO(out mRelationshipDef, out mRelKeyDef);
            RelationshipDef lRelationshipDef = new SingleRelationshipDef("Relation1", typeof (MockBO), mRelKeyDef, true,
                                                                         DeleteParentAction.Prevent);
            ISingleRelationship rel =
                (ISingleRelationship) lRelationshipDef.CreateRelationship(_mMockBO, _mMockBO.PropCol);
            Assert.AreEqual(lRelationshipDef.RelationshipName, rel.RelationshipName);
            Assert.IsTrue(_mMockBO.GetPropertyValue("MockBOProp1") == null);
            Assert.IsFalse(rel.HasRelatedObject(), "Should be false since props are not defaulted in Mock bo");
            _mMockBO.SetPropertyValue("MockBOProp1", _mMockBO.GetPropertyValue("MockBOID"));
            _mMockBO.Save();
            Assert.IsTrue(rel.HasRelatedObject(), "Should be true since prop MockBOProp1 has been set");

            Assert.AreEqual(_mMockBO.GetPropertyValue("MockBOProp1"), _mMockBO.GetPropertyValue("MockBOID"));
            MockBO ltempBO = (MockBO) rel.GetRelatedObject();
            Assert.IsFalse(ltempBO == null);
            Assert.AreEqual(_mMockBO.GetPropertyValue("MockBOID"), ltempBO.GetPropertyValue("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual(_mMockBO.GetPropertyValueString("MockBOProp1"), ltempBO.GetPropertyValueString("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");
            Assert.AreEqual(_mMockBO.GetPropertyValue("MockBOProp1"), ltempBO.GetPropertyValue("MockBOID"),
                            "The object returned should be the one with the ID = MockBOID");

            Assert.IsTrue(ReferenceEquals(ltempBO, rel.GetRelatedObject()));
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            Assert.IsTrue(ReferenceEquals(ltempBO, rel.GetRelatedObject()));
            _mMockBO.MarkForDelete();
            _mMockBO.Save();
        }

        [Test]
        public void TestGetRelatedBusinessObjectCollection_SortOrder()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_SortOrder_AddressLine1();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            AddressTestBO address1 = new AddressTestBO();
            address1.ContactPersonID = cp.ContactPersonID;
            address1.AddressLine1 = "ffff";
            address1.Save();
            AddressTestBO address2 = new AddressTestBO();
            address2.ContactPersonID = cp.ContactPersonID;
            address2.AddressLine1 = "bbbb";
            address2.Save();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------

            RelatedBusinessObjectCollection<AddressTestBO> addresses = cp.Addresses;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, addresses.Count);
            Assert.AreSame(address1, addresses[1]);
            Assert.AreSame(address2, addresses[0]);
            //---------------Tear Down -------------------------     
        }


        [Test]
        public void TestGetRelatedBusinessObjectCollection_SortOrder_ChangeOrder()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_SortOrder_AddressLine1();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            AddressTestBO address1 = new AddressTestBO();
            address1.ContactPersonID = cp.ContactPersonID;
            address1.AddressLine1 = "ffff";
            address1.Save();
            AddressTestBO address2 = new AddressTestBO();
            address2.ContactPersonID = cp.ContactPersonID;
            address2.AddressLine1 = "bbbb";
            address2.Save();

            //---------------Assert PreConditions---------------     
            RelatedBusinessObjectCollection<AddressTestBO> addresses = cp.Addresses;
            Assert.AreEqual(2, addresses.Count);
            Assert.AreSame(address1, addresses[1]);
            Assert.AreSame(address2, addresses[0]);

            //---------------Execute Test ----------------------
            address2.AddressLine1 = "zzzzz";
            address2.Save();
            RelatedBusinessObjectCollection<AddressTestBO> addressesAfterChangeOrder = cp.Addresses;

            //---------------Test Result -----------------------

            Assert.AreSame(address1, addressesAfterChangeOrder[0]);
            Assert.AreSame(address2, addressesAfterChangeOrder[1]);
        }

        [Test]
        public void TestGetRelatedObject()
        {
            RelationshipDef mRelationshipDef;
            RelKeyDef mRelKeyDef;
            MockBO _mMockBO= GetMockBO(out mRelationshipDef, out mRelKeyDef);
            RelationshipDef lRelationshipDef = new SingleRelationshipDef("Relation1", typeof (MockBO), mRelKeyDef, true,
                                                                         DeleteParentAction.Prevent);
            ISingleRelationship rel =
                (ISingleRelationship) lRelationshipDef.CreateRelationship(_mMockBO, _mMockBO.PropCol);
            Assert.AreEqual(lRelationshipDef.RelationshipName, rel.RelationshipName);
            Assert.IsTrue(_mMockBO.GetPropertyValue("MockBOProp1") == null);
            Assert.IsFalse(rel.HasRelatedObject(), "Should be false since props are not defaulted in Mock bo");
            //Set a related object
            _mMockBO.SetPropertyValue("MockBOProp1", _mMockBO.GetPropertyValue("MockBOID"));
            //Save the object, so that the relationship can retrieve the object from the database
            _mMockBO.Save();
            Assert.IsTrue(rel.HasRelatedObject(), "Should have a related object since the relating props have values");
            MockBO ltempBO = (MockBO) rel.GetRelatedObject();
            Assert.IsNotNull(ltempBO, "The related object should exist");
            //Clear the related object
            _mMockBO.SetPropertyValue("MockBOProp1", null);
            Assert.IsFalse(rel.HasRelatedObject(),
                           "Should not have a related object since the relating props have been set to null");
            ltempBO = (MockBO) rel.GetRelatedObject();
            Assert.IsNull(ltempBO, "The related object should now be null");
            //Set a related object again
            _mMockBO.SetPropertyValue("MockBOProp1", _mMockBO.GetPropertyValue("MockBOID"));
            Assert.IsTrue(rel.HasRelatedObject(),
                          "Should have a related object since the relating props have values again");
            ltempBO = (MockBO) rel.GetRelatedObject();
            Assert.IsNotNull(ltempBO, "The related object should exist again");
            _mMockBO.MarkForDelete();
            _mMockBO.Save();
        }

        [Test]
        public void TestGetReverseRelationship()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_WithTwoRelationshipsToContactPerson();
            IClassDef cpClassDef = ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();

            cpClassDef.RelationshipDefCol["Organisation"].ReverseRelationshipName = "OtherContactPeople";

            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            SingleRelationship<OrganisationTestBO> organisationRel =
                contactPerson.Relationships.GetSingle<OrganisationTestBO>("Organisation");
            MultipleRelationship<ContactPersonTestBO> contactPeopleRel =
                organisation.Relationships.GetMultiple<ContactPersonTestBO>("OtherContactPeople");

            //---------------Execute Test ----------------------
            IRelationship reverseRelationship = organisationRel.GetReverseRelationship(organisation);

            //---------------Test Result -----------------------
            Assert.AreSame(contactPeopleRel, reverseRelationship);
        }

        [Test]
        public void TestGetReverseRelationship_ReverseRelationshipSpecifiedButNotFound()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef();
            IClassDef cpClassDef = ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();

            string reverseRelationshipName = TestUtil.GetRandomString();
            cpClassDef.RelationshipDefCol["Organisation"].ReverseRelationshipName = reverseRelationshipName;

            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            SingleRelationship<OrganisationTestBO> organisationRel =
                contactPerson.Relationships.GetSingle<OrganisationTestBO>("Organisation");

            //---------------Execute Test ----------------------
            try
            {
                organisationRel.GetReverseRelationship(organisation);
                Assert.Fail("Should have failed since a reverse relationship was specified that didn't exist.");
            }
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains(
                    string.Format(
                        "The relationship 'Organisation' on class 'ContactPersonTestBO' has a reverse relationship defined ('{0}')",
                        reverseRelationshipName), ex.Message);
            }
        }

        [Test]
        public void TestRefreshWithRemovedChild()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            AddressTestBO address;
            ContactPersonTestBO cp = ContactPersonTestBO.CreateContactPersonWithOneAddress_DeleteDoNothing(out address);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cp.Addresses.Count);

            //---------------Execute Test ----------------------
            address.MarkForDelete();
            address.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cp.Addresses.Count);
        }

        [Test]
        public void TestRefreshWithRemovedChild_DereferenceChild()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            AddressTestBO address;
            ContactPersonTestBO cp = ContactPersonTestBO.CreateContactPersonWithOneAddress_DeleteDoNothing(out address);
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.Surname = Guid.NewGuid().ToString("N");
            cp2.FirstName = Guid.NewGuid().ToString("N");
            cp2.Save();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cp.Addresses.Count);

            //---------------Execute Test ----------------------
            address.ContactPersonID = cp2.ContactPersonID;
            address.Save();
            address.SetDeletable(false);
            RelatedBusinessObjectCollection<AddressTestBO> addresses = cp.Addresses;

            //---------------Test Result -----------------------
            Assert.AreEqual(0, addresses.Count);
        }

        [Test]
        public void RelationshipType_ShouldBeSameAsRelationshipDefRelationshipType()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            var bo = new MyBO();
            var relationship = bo.Relationships["MyRelationship"];
            var expectedRelationshipType = relationship.RelationshipDef.RelationshipType;
            //---------------Execute Test ----------------------
            RelationshipType relationshipType = relationship.RelationshipType;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedRelationshipType, relationshipType);
        }
       
    }
}