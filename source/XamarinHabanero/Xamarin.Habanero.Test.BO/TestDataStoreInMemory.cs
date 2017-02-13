#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.IO;
using System.Threading;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using NSubstitute;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestDataStoreInMemory
    {
        // ReSharper disable InconsistentNaming
        private string _testFolderName;

        public string GetTestPath(string folderName)
        {
            return Path.Combine(_testFolderName, folderName);
        }

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            //new Address();
            _testFolderName = Path.Combine(Environment.CurrentDirectory, "TestFolder");
            if (!Directory.Exists(_testFolderName)) Directory.CreateDirectory(_testFolderName);
        }

        [TearDown]
        public void TearDownTest()
        {
            if (Directory.Exists(_testFolderName)) Directory.Delete(_testFolderName, true);
        }

        [Test]
        public void TestDataStoreConstructor()
        {
            //---------------Set up test pack-------------------

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, dataStore.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDataStoreAdd()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            dataStore.Add(new ContactPersonTestBO());
            //---------------Test Result -----------------------
            Assert.AreEqual(1, dataStore.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDataStoreRemove()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            dataStore.Add(cp);
            //---------------Execute Test ----------------------
            dataStore.Remove(cp);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, dataStore.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_ClearAll()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            dataStore.Add(new ContactPersonTestBO());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStore.Count);

            //---------------Execute Test ----------------------
            dataStore.ClearAllBusinessObjects();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, dataStore.Count);
        }

        [Test]
        public void TestFind()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory(new DataStoreInMemory());
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(cp);
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, cp.Surname);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = dataStore.Find<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
            //---------------Tear Down -------------------------      
        }

        [Test]
        public void TestFind_Untyped()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory(new DataStoreInMemory());
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(cp);
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, cp.Surname);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = (ContactPersonTestBO) dataStore.Find(typeof(ContactPersonTestBO), criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
            //---------------Tear Down -------------------------      
        }
        [Test]
        public void TestFind_UsingGuidCriteria_Untyped()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory(new DataStoreInMemory());
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef_WOrganisationID();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO {Surname = Guid.NewGuid().ToString("N")};
            cp.OrganisationID = OrganisationTestBO.CreateSavedOrganisation().OrganisationID;
            cp.Save();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(cp);
            Criteria criteria = new Criteria("OrganisationID", Criteria.ComparisonOp.Equals, cp.OrganisationID);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = (ContactPersonTestBO) dataStore.Find(typeof(ContactPersonTestBO), criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
        }

        [Test]
        public void TestFind_UsingGuidCriteria_Typed()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory(new DataStoreInMemory());
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef_WOrganisationID();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO {Surname = Guid.NewGuid().ToString("N")};
            cp.OrganisationID = OrganisationTestBO.CreateSavedOrganisation().OrganisationID;
            cp.Save();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(cp);
            Criteria criteria = new Criteria("OrganisationID", Criteria.ComparisonOp.Equals, cp.OrganisationID);
            //---------------Assert Precondtions---------------
            Assert.IsNotNull(cp.OrganisationID);
            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = dataStore.Find<ContactPersonTestBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
        }

        [Test]
        public void TestFind_UsingGuidCriteriaString_Untyped()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory(new DataStoreInMemory());
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef_WOrganisationID();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO { Surname = Guid.NewGuid().ToString("N") };
            cp.OrganisationID = OrganisationTestBO.CreateSavedOrganisation().OrganisationID;
            cp.Save();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(cp);
            Criteria criteria = CriteriaParser.CreateCriteria("OrganisationID = " + cp.OrganisationID);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = (ContactPersonTestBO)dataStore.Find(typeof(ContactPersonTestBO), criteria);

            //---------------Test Result -----------------------
            Assert.IsNotNull(loadedCP);
            Assert.AreSame(cp.ID, loadedCP.ID);
        }

        [Test]
        public void TestFind_UsingGuidCriteriaString_Typed()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory(new DataStoreInMemory());
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef_WOrganisationID();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO { Surname = Guid.NewGuid().ToString("N") };
            cp.OrganisationID = OrganisationTestBO.CreateSavedOrganisation().OrganisationID;
            cp.Save();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(cp);
            Criteria criteria = CriteriaParser.CreateCriteria("OrganisationID = " + cp.OrganisationID);
            //---------------Assert Precondtions---------------
            Assert.IsNotNull(cp.OrganisationID);
            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = dataStore.Find<ContactPersonTestBO>(criteria);
            //---------------Test Result -----------------------
            Assert.IsNotNull(loadedCP);
            Assert.AreSame(cp.ID, loadedCP.ID);
        }

        [Test]
        public void TestFind_PrimaryKey()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(cp);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = dataStore.Find<ContactPersonTestBO>(cp.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
            //---------------Tear Down -------------------------      
        }

        [Test]
        public void TestFindAll()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory(new DataStoreInMemory());
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            OrganisationTestBO.LoadDefaultClassDef();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = TestUtil.GetRandomString();
            cp1.Save();
            dataStore.Add(cp1);
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            dataStore.Add(cp2);
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            cp2.Surname = TestUtil.GetRandomString();
            cp2.Save();
            dataStore.Add(OrganisationTestBO.CreateSavedOrganisation());
            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = dataStore.FindAll<ContactPersonTestBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
        }

        [Test]
        public void TestFindAll_Untyped()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory(new DataStoreInMemory());
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            OrganisationTestBO.LoadDefaultClassDef();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = TestUtil.GetRandomString();
            cp1.Save();
            dataStore.Add(cp1);
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            cp2.Surname = TestUtil.GetRandomString();
            cp2.Save();
            dataStore.Add(cp2);
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            dataStore.Add(OrganisationTestBO.CreateSavedOrganisation());
            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = dataStore.FindAll(typeof(ContactPersonTestBO), criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        public void TestFindAll_UsingClassDef()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory(new DataStoreInMemory());
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            OrganisationTestBO.LoadDefaultClassDef();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = TestUtil.GetRandomString();
            cp1.Save();
            dataStore.Add(cp1);
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            cp2.Surname = TestUtil.GetRandomString();
            cp2.Save();
            dataStore.Add(cp2);
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);

            dataStore.Add(OrganisationTestBO.CreateSavedOrganisation());
            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = dataStore.FindAll(ClassDef.Get<ContactPersonTestBO>(), criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
        }

        [Test]
        [TestCase("1")]
        [TestCase("2")]
        [TestCase("3")]
        [TestCase("4")]
        [TestCase("5")]
        [TestCase("6")]
        [TestCase("7")]
        [TestCase("8")]
        public void TestFindAll_UsingClassDef_BUGFIX_ShouldBeThreadsafe(string aaa)
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory(new DataStoreInMemory());
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            OrganisationTestBO.LoadDefaultClassDef();
            var dataStore = new DataStoreInMemory();
            var now = DateTime.Now;
            var cp1 = new ContactPersonTestBO {DateOfBirth = now, Surname = TestUtil.GetRandomString()};
            cp1.Save();
            dataStore.Add(cp1);
            var cp2 = new ContactPersonTestBO {DateOfBirth = now, Surname = TestUtil.GetRandomString()};
            cp2.Save();
            dataStore.Add(cp2);
            //var criteria = Substitute.For<Criteria>("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            //criteria.Stub(o => o.IsMatch(Arg<IBusinessObject>.Is.Anything)).WhenCalled(invocation =>
            //{
            //    Thread.Sleep(100);
            //    invocation.ReturnValue = true;
            //}).Return(true);
            var criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);

            for (int i1 = 0; i1 < 1000; i1++)
            {
                dataStore.Add(OrganisationTestBO.CreateSavedOrganisation());
            }

            var threads = new List<Thread>();
            threads.Add(new Thread(() =>
            {
                for (int i1 = 0; i1 < 1000; i1++)
                {
                    dataStore.Add(OrganisationTestBO.CreateSavedOrganisation());
                }
            }));

            var exceptions = new List<Exception>();
            threads.Add(new Thread(() =>
            {
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        dataStore.FindAll(ClassDef.Get<ContactPersonTestBO>(), criteria);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add( ex);
                }
            }));
            
            
            //---------------Execute Test ----------------------
            threads.AsParallel().ForAll(thread => thread.Start());
            threads.AsParallel().ForAll(thread => thread.Join());
            //Assert.DoesNotThrow(() =>
            //{
                //var col = dataStore.FindAll(ClassDef.Get<ContactPersonTestBO>(), criteria);
                //thread.Join();
            //});
            //---------------Test Result -----------------------
            if (exceptions.Count > 0)
            {
                Assert.Fail("Has an Exception: " + exceptions[0].ToString());
            }
        }

        [Test]
        public void TestFindAll_ClassDef_NullCriteria()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            OrganisationTestBO.LoadDefaultClassDef();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO {DateOfBirth = now};
            dataStore.Add(cp1);
            dataStore.Add(OrganisationTestBO.CreateSavedOrganisation());
            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = dataStore.FindAll(ClassDef.Get<ContactPersonTestBO>(), null);
            
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(cp1, col);
            Assert.IsNull(col.SelectQuery.Criteria);
        }
        [Test]
        public void TestFindAll_ClassDef_WhenIsSubType()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            CircleNoPrimaryKey.GetClassDef().SuperClassDef =
                new SuperClassDef(Shape.GetClassDef(), ORMapping.SingleTableInheritance);
            CircleNoPrimaryKey.GetClassDef().SuperClassDef.Discriminator = "ShapeType_field";
            Shape.GetClassDef().PropDefcol.Add(new PropDef("ShapeType_field", typeof(string), PropReadWriteRule.WriteOnce, "ShapeType_field", null));

            DataStoreInMemory dataStore = new DataStoreInMemory();
            CircleNoPrimaryKey circleNoPrimaryKey = new CircleNoPrimaryKey ();
            dataStore.Add(circleNoPrimaryKey);
            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = dataStore.FindAll(ClassDef.Get<Shape>(), null);
            
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(circleNoPrimaryKey, col);
            Assert.IsNull(col.SelectQuery.Criteria);
        }

        [Test]
        public void TestFindAll_NullCriteria()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            OrganisationTestBO.LoadDefaultClassDef();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            dataStore.Add(cp1);
            dataStore.Add(OrganisationTestBO.CreateSavedOrganisation());
            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = dataStore.FindAll<ContactPersonTestBO>(null);
            
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(cp1, col);
            Assert.IsNull(col.SelectQuery.Criteria);
        }

        [Ignore("TODO: fix, but this is hanging in Hudson")]
        [Test]
        public void TestCompositeKeyObject()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            //Ther are two datastores so that you can manually add an item to a datastore without
            // the save effecting the datastore you are testing.
            DataStoreInMemory dataStore = new DataStoreInMemory();
            DataStoreInMemory otherDataStore = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(otherDataStore);
            new Car();
            ContactPersonCompositeKey contactPerson = new ContactPersonCompositeKey();
            contactPerson.Save();
            //---------------Assert Precondition----------------
            Assert.IsFalse(dataStore.AllObjects.ContainsKey(contactPerson.ID.ObjectID));
            //---------------Execute Test ----------------------
            dataStore.Add(contactPerson);
            //In the save process the ID is updated to the persisted field values, so the hash of the ID changes
            // this is why the object is removed and re-added to the BusinessObjectManager (to ensure the dictionary
            // of objects is hashed on the correct, updated value.
            contactPerson.PK1Prop1 = TestUtil.GetRandomString();
            contactPerson.Save();  
            //---------------Test Result -----------------------
            Assert.IsTrue(dataStore.AllObjects.ContainsKey(contactPerson.ID.ObjectID));
        }

        [Test]
        public void Test_MutableKeyObject_TwoObjectsWithSameFieldNameAndValueAsPrimaryKey()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BOWithIntID_DifferentType.LoadClassDefWithIntID();
            BOWithIntID.LoadClassDefWithIntID();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(dataStore);
            new Car();
            BOWithIntID boWithIntID = new BOWithIntID();
            boWithIntID.IntID = TestUtil.GetRandomInt();
            boWithIntID.Save();
            BOWithIntID_DifferentType intID_DifferentType = new BOWithIntID_DifferentType();
            intID_DifferentType.IntID = TestUtil.GetRandomInt();
            intID_DifferentType.Save();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, dataStore.Count);
            //---------------Execute Test ----------------------
//            dataStore.Add(intID_DifferentType);
//            // in the save process the ID is updated to the persisted field values, so the hash of the ID changes
//            // this is why the object is removed and re-added to the BusinessObjectManager (to ensure the dictionary
//            // of objects is hashed on the correct, updated value.
//            intID_DifferentType.Save();
            IBusinessObject returnedBOWitID = dataStore.AllObjects[boWithIntID.ID.ObjectID];
            IBusinessObject returnedBOWitID_diffType = dataStore.AllObjects[intID_DifferentType.ID.ObjectID];

            //---------------Test Result -----------------------
            Assert.AreSame(boWithIntID, returnedBOWitID);
            Assert.AreSame(intID_DifferentType, returnedBOWitID_diffType);
        }

        [Test]
        public void TestMutableCompositeKeyObject_TwoObjectsWithSameFieldNameAndValueAsPrimaryKey()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BOWithIntID_DifferentType.LoadClassDefWithIntID_CompositeKey();
            BOWithIntID.LoadClassDefWithIntID_WithCompositeKey();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(dataStore);
            new Car();
            BOWithIntID boWithIntID = new BOWithIntID {IntID = TestUtil.GetRandomInt()};
            boWithIntID.Save();
            BOWithIntID_DifferentType intID_DifferentType = new BOWithIntID_DifferentType();
            intID_DifferentType.IntID = boWithIntID.IntID;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            intID_DifferentType.Save();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, dataStore.Count);

            Assert.IsTrue(dataStore.AllObjects.ContainsKey(boWithIntID.ID.ObjectID));
            Assert.IsTrue(dataStore.AllObjects.ContainsKey(intID_DifferentType.ID.ObjectID));

            IBusinessObject returnedBOWitID = dataStore.AllObjects[boWithIntID.ID.ObjectID];
            IBusinessObject returnedBOWitID_diffType = dataStore.AllObjects[intID_DifferentType.ID.ObjectID];

            Assert.AreSame(boWithIntID, returnedBOWitID);
            Assert.AreSame(intID_DifferentType, returnedBOWitID_diffType);
        }

        [Test]
        public void Test_GetNextAutoIncrementingNumber_WhenNoNumberGeneratorForClassDef_ShouldCreateNumberGenerator()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStoreInMemory = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(dataStoreInMemory);
            IClassDef classDef = Substitute.For<IClassDef>();
            IClassDef classDef2 = Substitute.For<IClassDef>();
            INumberGenerator numberGenerator = Substitute.For<INumberGenerator>();
            dataStoreInMemory.AutoIncrementNumberGenerators.Add(classDef, numberGenerator);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStoreInMemory.AutoIncrementNumberGenerators.Count);
            Assert.AreSame(numberGenerator, dataStoreInMemory.AutoIncrementNumberGenerators[classDef]);
            Assert.IsFalse(dataStoreInMemory.AutoIncrementNumberGenerators.ContainsKey(classDef2));
            //---------------Execute Test ----------------------
            long defaultNumber = dataStoreInMemory.GetNextAutoIncrementingNumber(classDef2);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, dataStoreInMemory.AutoIncrementNumberGenerators.Count);
            numberGenerator.DidNotReceive().NextNumber();
            Assert.AreEqual(1, defaultNumber);
            INumberGenerator createdNumberGenerator = dataStoreInMemory.AutoIncrementNumberGenerators[classDef2];
            Assert.IsNotNull(createdNumberGenerator);
            TestUtil.AssertIsInstanceOf<NumberGenerator>(createdNumberGenerator);
        }

        [Test]
        public void Test_GetNextAutoIncrementingNumber_ShouldCreateNumberGenerators_BUGFIX_ShouldBeThreadSafe()
        {
            //---------------Set up test pack-------------------
            var dataStoreInMemory = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(dataStoreInMemory);
            var classDef1 = Substitute.For<IClassDef>();
            var classDef2 = Substitute.For<IClassDef>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, dataStoreInMemory.AutoIncrementNumberGenerators.Count);
            Assert.IsFalse(dataStoreInMemory.AutoIncrementNumberGenerators.ContainsKey(classDef1));
            Assert.IsFalse(dataStoreInMemory.AutoIncrementNumberGenerators.ContainsKey(classDef2));
            //---------------Execute Test ----------------------
            var exceptions = new List<Exception>();
            TestUtil.ExecuteInParallelThreads(2, () =>
            {
                try
                {
                    dataStoreInMemory.GetNextAutoIncrementingNumber(classDef1);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            });
            //---------------Test Result -----------------------
            if (exceptions.Count > 0)
            {
                Assert.Fail(exceptions[0].ToString());
            }
            Assert.AreEqual(1, dataStoreInMemory.AutoIncrementNumberGenerators.Count);
        }

        [Test]
        public void Test_GetNextAutoIncrementingNumber_ShouldUseNumberGenerator()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStoreInMemory = new DataStoreInMemory();
            IClassDef classDef = Substitute.For<IClassDef>();
            INumberGenerator numberGenerator = Substitute.For<INumberGenerator>();
            long numberFromNumberGenerator = TestUtil.GetRandomInt();
            numberGenerator.NextNumber().Returns(numberFromNumberGenerator);
            dataStoreInMemory.AutoIncrementNumberGenerators.Add(classDef,numberGenerator);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStoreInMemory.AutoIncrementNumberGenerators.Count);
            Assert.AreSame(numberGenerator, dataStoreInMemory.AutoIncrementNumberGenerators[classDef]);
            //---------------Execute Test ----------------------
            long autoIncrementingNumber = dataStoreInMemory.GetNextAutoIncrementingNumber(classDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(numberFromNumberGenerator, autoIncrementingNumber);
            numberGenerator.Received(1).NextNumber();
        }        
        
        [Test]
        public void Test_GetNextAutoIncrementingNumber_ShouldUseNumberGeneratorForSpecificClassDef()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStoreInMemory = new DataStoreInMemory();
            IClassDef classDef = Substitute.For<IClassDef>();
            IClassDef classDef2 = Substitute.For<IClassDef>();
            INumberGenerator numberGenerator = Substitute.For<INumberGenerator>();
            INumberGenerator numberGenerator2 = Substitute.For<INumberGenerator>();
            dataStoreInMemory.AutoIncrementNumberGenerators.Add(classDef, numberGenerator);
            dataStoreInMemory.AutoIncrementNumberGenerators.Add(classDef2, numberGenerator2);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, dataStoreInMemory.AutoIncrementNumberGenerators.Count);
            Assert.AreSame(numberGenerator, dataStoreInMemory.AutoIncrementNumberGenerators[classDef]);
            Assert.AreSame(numberGenerator2, dataStoreInMemory.AutoIncrementNumberGenerators[classDef2]);
            //---------------Execute Test ----------------------
            long autoIncrementingNumber = dataStoreInMemory.GetNextAutoIncrementingNumber(classDef2);
            //---------------Test Result -----------------------
            numberGenerator2.Received().NextNumber();
            numberGenerator.DidNotReceive().NextNumber();
        }

        [Test]
        public void Test_AutoIncrementNumberGenerators_IsNotNull()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DataStoreInMemory dataStoreInMemory = new DataStoreInMemory();
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataStoreInMemory.AutoIncrementNumberGenerators);
        }
    }
}
