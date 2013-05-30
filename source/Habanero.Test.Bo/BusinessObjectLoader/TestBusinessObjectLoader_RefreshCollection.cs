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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Test.BO.BusinessObjectLoader
{
	[TestFixture]
	public class TestBusinessObjectLoader_RefreshCollection 
	{
		#region Setup/Teardown
		[TestFixtureSetUp]
        public virtual void TestFixtureSetup()
		{
            FixtureEnvironment.SetupNewIsolatedBusinessObjectManager();
		}

		[TearDown]
		public virtual void TearDownTest()
		{
		}

		[SetUp]
		public virtual void SetupTest()
		{
			ClassDef.ClassDefs.Clear();
			SetupDataAccessor();
			
			TestUtil.WaitForGC();
		}

		#endregion

		private DataStoreInMemory _dataStore;
	    private string _contactPersonTableName;

	    protected virtual void SetupDataAccessor()
		{
			_dataStore = new DataStoreInMemory();
			BORegistry.DataAccessor = new DataAccessorInMemory(_dataStore);
		}

	    protected virtual void CreateContactPersonTable()
	    {
            _contactPersonTableName = "contact_person_" + TestUtil.GetRandomString();
	    }

        public string GetContactPersonTableName()
        {
            if (string.IsNullOrEmpty(_contactPersonTableName))
            {
                CreateContactPersonTable();
            }
            return _contactPersonTableName;
        }

	    protected virtual IClassDef SetupDefaultContactPersonBO()
        {
            CreateContactPersonTable();
            var cpClassDef = ContactPersonTestBO.LoadDefaultClassDef();
            //cpClassDef.TableName = "ContactPersonTable with a randomlygenerated guid";

            cpClassDef.TableName = GetContactPersonTableName();
            return cpClassDef;
        }
		
		#region Refresh

		//Refreshing a business object collection
		//A business object collection can be refreshed from the database. When refreshing a collection the collection will be reloaded. 
		//Any dirty business objects will not be refreshed. Any Business Objects in the Removed or deleted list will not be shown in the
		//current list. Any items in the Created List will be shown in the main list. 
		[Test]
		public void Test_Refresh_NoNewItems_Typed()
		{
            //---------------Set up test pack-------------------
            SetupDefaultContactPersonBO();
			ContactPersonTestBO.CreateSavedContactPerson();
			ContactPersonTestBO.CreateSavedContactPerson();
			var col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.LoadAll();

			//---------------Assert Precondition----------------
			Assert.AreEqual(2, col.Count);
			Assert.AreEqual(2, col.PersistedBusinessObjects.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(2, col.Count);
			Assert.AreEqual(2, col.PersistedBusinessObjects.Count);
		}
		[Test]
		public void Test_Refresh_NoNewItems_UnTyped()
		{
            //---------------Set up test pack-------------------
            SetupDefaultContactPersonBO();
			ContactPersonTestBO.CreateSavedContactPerson();
			ContactPersonTestBO.CreateSavedContactPerson();
			IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.LoadAll();

			//---------------Assert Precondition----------------
			Assert.AreEqual(2, col.Count);
			Assert.AreEqual(2, col.PersistedBusinessObjects.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(2, col.Count);
			Assert.AreEqual(2, col.PersistedBusinessObjects.Count);
		}

		[Test]
		public void Test_Refresh_Typed()
		{
            //---------------Set up test pack-------------------
            SetupDefaultContactPersonBO();
			var now = DateTime.Now;
			var cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			var cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-1));
			var criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
			var col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.Load(criteria, "Surname");
			var cp3 = ContactPersonTestBO.CreateSavedContactPerson(now);

			//---------------Assert Precondition ---------------
			Assert.AreEqual(2, col.Count);
			Assert.AreEqual(2, col.PersistedBusinessObjects.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(3, col.Count);
			Assert.AreEqual(3, col.PersistedBusinessObjects.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
		}


		[Test]
		public void Test_Refresh_UnTyped()
		{
            //---------------Set up test pack-------------------
            SetupDefaultContactPersonBO();
			var now = DateTime.Now;
			var cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			var cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-1));
			var criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
			var col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.Load(criteria, "Surname");
			var cp3 = ContactPersonTestBO.CreateSavedContactPerson(now);

			//---------------Assert Precondition ---------------
			Assert.AreEqual(2, col.Count);
			Assert.AreEqual(2, col.PersistedBusinessObjects.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(3, col.Count);
			Assert.AreEqual(3, col.PersistedBusinessObjects.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
		}

		[Test]
		public virtual void Test_Refresh_W_ParametrizedClassDef_Typed()
		{
			//---------------Set up test pack-------------------
            SetupDataAccessor();
            SetupDefaultContactPersonBO();
			var parametrizedClassDef =
				new XmlClassLoader(new DtdLoader(), new DefClassFactory()).LoadClass(
					@"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"" typeParameter=""SuperHero"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
					<property name=""FirstName"" databaseField=""FirstName_field"" />
					<property name=""DateOfBirth"" type=""DateTime"" />
					<property name=""SuperPowerDescription"" /> 
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
				</class>
			");
			ClassDef.ClassDefs.Add(parametrizedClassDef);

			var now = DateTime.Now;
			var cp1 = new ContactPersonTestBO((ClassDef) parametrizedClassDef)
			              {
			                  Surname = TestUtil.GetRandomString(),
			                  FirstName = TestUtil.GetRandomString(),
			                  DateOfBirth = now.AddDays(-1)
			              };
		    cp1.Save();
			var cp2 = new ContactPersonTestBO((ClassDef) parametrizedClassDef)
			              {
			                  Surname = TestUtil.GetRandomString(),
			                  FirstName = TestUtil.GetRandomString(),
			                  DateOfBirth = now.AddDays(-1)
			              };
		    cp2.Save();
			ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(1));
			
			FixtureEnvironment.ClearBusinessObjectManager();
			var col = new BusinessObjectCollection<ContactPersonTestBO>(parametrizedClassDef);
			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(2, col.Count);
			Assert.AreEqual(2, col.PersistedBusinessObjects.Count);
			Assert.IsTrue(col[0].Props.Contains("SuperPowerDescription"));
			Assert.IsTrue(col[1].Props.Contains("SuperPowerDescription"));
		}



		[Test]
		public virtual void Test_Refresh_W_ParametrizedClassDef_Untyped()
		{
            //---------------Set up test pack-------------------
            SetupDefaultContactPersonBO();
			var parametrizedClassDef =
				new XmlClassLoader(new DtdLoader(), new DefClassFactory()).LoadClass(
					@"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"" typeParameter=""SuperHero"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
					<property name=""FirstName"" databaseField=""FirstName_field"" />
					<property name=""DateOfBirth"" type=""DateTime"" />
					<property name=""SuperPowerDescription"" /> 
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
				</class>
			");
			ClassDef.ClassDefs.Add(parametrizedClassDef);

			var now = DateTime.Now;
			var cp1 = new ContactPersonTestBO((ClassDef) parametrizedClassDef)
			              {
			                  Surname = TestUtil.GetRandomString(),
			                  FirstName = TestUtil.GetRandomString(),
			                  DateOfBirth = now.AddDays(-1)
			              };
		    cp1.Save();
			var cp2 = new ContactPersonTestBO((ClassDef) parametrizedClassDef)
			              {
			                  Surname = TestUtil.GetRandomString(),
			                  FirstName = TestUtil.GetRandomString(),
			                  DateOfBirth = now.AddDays(-1)
			              };
		    cp2.Save();
			ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(1));

			FixtureEnvironment.ClearBusinessObjectManager();
			IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>(parametrizedClassDef);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(2, col.Count);
			Assert.AreEqual(2, col.PersistedBusinessObjects.Count);
			Assert.IsTrue(col[0].Props.Contains("SuperPowerDescription"));
			Assert.IsTrue(col[1].Props.Contains("SuperPowerDescription"));
		}


		[Test]
		public void Test_Refresh_W_RemovedBOs_Typed()
		{
            //---------------Set up test pack-------------------
            SetupDefaultContactPersonBO();

			ContactPersonTestBO cp;
			var cpCol = CreateCol_OneCP(out cp);
			cpCol.Remove(cp);

			//---------------Assert Precondition----------------
			Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
			Assert.AreEqual(0, cpCol.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpCol);

			//---------------Test Result -----------------------
			Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
			Assert.AreEqual(0, cpCol.Count);
		}




		[Test]
		public void Test_Refresh_W_RemovedBOs_UnTyped()
		{
            //---------------Set up test pack-------------------
		    ClassDef.ClassDefs.Clear();
            SetupDefaultContactPersonBO();
			ContactPersonTestBO cp;
			IBusinessObjectCollection cpCol = CreateCol_OneCP(out cp);
			cpCol.Remove(cp);

			//---------------Assert Precondition----------------
			Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
			Assert.AreEqual(0, cpCol.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpCol);

			//---------------Test Result -----------------------
			Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
			Assert.AreEqual(0, cpCol.Count);
		}

		[Test]
		public void Test_Refresh_W_RemovedBOs_UnTyped_StillRespondingToEvents()
		{
            //---------------Set up test pack-------------------
            SetupDefaultContactPersonBO();
			ContactPersonTestBO cp;
			IBusinessObjectCollection cpCol = CreateCol_OneCP(out cp);
			cpCol.Remove(cp);
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpCol);

			//---------------Assert Precondition----------------
			Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
			Assert.AreEqual(0, cpCol.Count);

			//---------------Execute Test ----------------------
			cpCol.CancelEdits();

			cp.MarkForDelete();
			//---------------Test Result -----------------------
			Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
			Assert.AreEqual(0, cpCol.Count);
			Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
		}

		[Test]
		public void Test_Refresh_W_CreatedBOs_CreatedObjectsStillRespondToEvents_Typed()
		{
            //---------------Set up test pack-------------------
            SetupDefaultContactPersonBO();
			var cpCol = CreateCollectionWith_OneBO();

			var createdCp = cpCol.CreateBusinessObject();
			createdCp.Surname = BOTestUtils.RandomString;

			//---------------Assert Precondition----------------
			Assert.AreEqual(2, cpCol.Count);
			Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
			Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
			Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpCol);
			createdCp.Save();

			//---------------Test Result -----------------------
			Assert.AreEqual(2, cpCol.Count);
			Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
			Assert.AreEqual(2, cpCol.PersistedBusinessObjects.Count);
			Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
		}

		[Test]
		public void Test_Refresh_W_CreatedBOs_CreatedObjectsStillRespondToEvents_UnTyped()
		{
            //---------------Set up test pack-------------------
            SetupDefaultContactPersonBO();
			var cpCol = CreateCollectionWith_OneBO();

			var createdCp = cpCol.CreateBusinessObject();
			createdCp.Surname = BOTestUtils.RandomString;

			//---------------Assert Precondition----------------
			Assert.AreEqual(2, cpCol.Count);
			Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
			Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpCol);
			createdCp.Save();

			//---------------Test Result -----------------------
			Assert.AreEqual(2, cpCol.Count);
			Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
			Assert.AreEqual(2, cpCol.PersistedBusinessObjects.Count);
		}

		[Test]
		public void Test_Refresh_W_CreatedBOs_Typed()
		{
            //---------------Set up test pack-------------------
            SetupDefaultContactPersonBO();
			var cpCol = CreateCollectionWith_OneBO();
			Assert.AreEqual(1, cpCol.Count);
			var createdCp = cpCol.CreateBusinessObject();
			createdCp.Surname = BOTestUtils.RandomString;

			//---------------Assert Precondition----------------
			Assert.AreEqual(2, cpCol.Count);
			Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
			Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpCol);

			//---------------Test Result -----------------------
			Assert.AreEqual(2, cpCol.Count);
			Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
			Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
		}

		[Test]
		public void Test_Refresh_W_CreatedBOs_UnTyped()
		{
            //---------------Set up test pack-------------------
            SetupDefaultContactPersonBO();
			var cpCol = CreateCollectionWith_OneBO();

			var createdCp = cpCol.CreateBusinessObject();
			createdCp.Surname = BOTestUtils.RandomString;

			//---------------Assert Precondition----------------
			Assert.AreEqual(2, cpCol.Count);
			Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
			Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpCol);

			//---------------Test Result -----------------------
			Assert.AreEqual(2, cpCol.Count);
			Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
			Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);

		}

		[Test]
		public void Test_Refresh_W_MarkedForDelete_Typed()
		{
			//---------------Set up test pack-------------------
            SetupDefaultContactPersonBO();

			ContactPersonTestBO deletedCP = ContactPersonTestBO.CreateSavedContactPerson();
			BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
			deletedCP.MarkForDelete();

			//---------------Assert Precondition----------------
			Assert.AreEqual(1, cpCol.Count);
			Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
			Assert.AreEqual(2, cpCol.PersistedBusinessObjects.Count);
			Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpCol);

			//---------------Test Result -----------------------
			Assert.AreEqual(1, cpCol.Count);
			Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
			Assert.AreEqual(2, cpCol.PersistedBusinessObjects.Count);
			Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
		}

		[Test]
		public void Test_Refresh_W_MarkedForDelete_UnTyped()
		{
			//---------------Set up test pack-------------------
			SetupDefaultContactPersonBO();
			ContactPersonTestBO deletedCP = ContactPersonTestBO.CreateSavedContactPerson();
			IBusinessObjectCollection cpCol = CreateCollectionWith_OneBO();

			deletedCP.MarkForDelete();

			//---------------Assert Precondition----------------
			Assert.AreEqual(1, cpCol.Count);
			Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
			Assert.AreEqual(2, cpCol.PersistedBusinessObjects.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpCol);

			//---------------Test Result -----------------------
			Assert.AreEqual(1, cpCol.Count);
			Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
			Assert.AreEqual(2, cpCol.PersistedBusinessObjects.Count);
			Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
		}

		[Test]
		public void Test_RefreshLoadedCollection_Typed_LTEQCriteriaString_IntegerProperty()
		{
			//---------------Set up test pack-------------------
		    LoadContactPersonClassDefWithIntProp();
		    DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			cp1.SetPropertyValue("IntegerProperty", 0);
			cp1.Save();
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			cp2.SetPropertyValue("IntegerProperty", 1);
			cp2.Save();
			ContactPersonTestBO cpExcluded = ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(+3));
			cpExcluded.SetPropertyValue("IntegerProperty", 3);
			cpExcluded.Save();
			ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(+1));
			cpEqual.SetPropertyValue("IntegerProperty", 2);
			cpEqual.Save();

			string criteria = "IntegerProperty <= " + cpEqual.GetPropertyValue("IntegerProperty");
			var col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.Load(criteria, "");
			var cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));
			cp3.SetPropertyValue("IntegerProperty", 1);
			cp3.Save();
			var cpExcludeNew = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(+1));
			cpExcludeNew.SetPropertyValue("IntegerProperty", 5);
			cpExcludeNew.Save();
			var cpEqualNew = ContactPersonTestBO.CreateSavedContactPerson(cpEqual.DateOfBirth);
			cpEqualNew.SetPropertyValue("IntegerProperty", 2);
			cpEqualNew.Save();

			//---------------Assert Precondition ---------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cpEqual, col);
			Assert.IsFalse(col.Contains(cpExcluded));

			//---------------Execute Test ----------------------
			col.Refresh();

			//---------------Test Result -----------------------
			Assert.AreEqual(5, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
			Assert.Contains(cpEqual, col);
			Assert.Contains(cpEqualNew, col);
			Assert.IsFalse(col.Contains(cpExcludeNew));
			Assert.IsFalse(col.Contains(cpExcluded));
		}

	    private IClassDef LoadContactPersonClassDefWithIntProp()
	    {
	        var classDef = ContactPersonTestBO.LoadDefaultClassDef_W_IntegerProperty();
	        CreateContactPersonTable();
	        classDef.TableName = _contactPersonTableName;
	        return classDef;
	    }

	    [Test]
		public void Test_RefreshLoadedCollection_Typed_NotEQ_CriteriaString()
		{
			//---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            LoadContactPersonClassDefWithIntProp();
			ContactPersonTestBO cp1 = CreateSavedContactPerson(TestUtil.GetRandomString(), 2);
			ContactPersonTestBO cp2 = CreateSavedContactPerson(TestUtil.GetRandomString(), 4);
			CreateSavedContactPerson(TestUtil.GetRandomString(), 2);
			ContactPersonTestBO cpEqual = CreateSavedContactPerson(TestUtil.GetRandomString(), 3);

			string criteria = "IntegerProperty <> " + 3;
			BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.Load(criteria, "");

			//---------------Assert Precondition ---------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.IsFalse(col.Contains(cpEqual));

			//---------------Execute Test ----------------------
			ContactPersonTestBO cp3 = CreateSavedContactPerson(TestUtil.GetRandomString(), 2);
			ContactPersonTestBO cpNotEqual = CreateSavedContactPerson(TestUtil.GetRandomString(), 4);
			ContactPersonTestBO cpEqualNew = CreateSavedContactPerson(TestUtil.GetRandomString(), 3);
			col.Refresh();

			//---------------Test Result -----------------------
			Assert.AreEqual(5, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
			Assert.Contains(cpNotEqual, col);
			Assert.IsFalse(col.Contains(cpEqualNew));
			Assert.IsFalse(col.Contains(cpEqual));
		}

		[Test]
		public void Test_RefreshLoadedCollection_Typed_NotEQ_CriteriaString_Null()
		{
			//---------------Set up test pack-------------------
			SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(+3));
			ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(+1));

			const string criteria = "DateOfBirth <> " + null;
			BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.Load(criteria, "");
			ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));
			ContactPersonTestBO cpNotEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(+1));
			ContactPersonTestBO cpEqualNew = ContactPersonTestBO.CreateSavedContactPerson(cpEqual.DateOfBirth);

			//---------------Assert Precondition ---------------
			Assert.AreEqual(4, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.IsTrue(col.Contains(cpEqual));

			//---------------Execute Test ----------------------
			col.Refresh();

			//---------------Test Result -----------------------
			Assert.AreEqual(7, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
			Assert.Contains(cpNotEqual, col);
			Assert.IsTrue(col.Contains(cpEqualNew));
			Assert.IsTrue(col.Contains(cpEqual));
		}

		[Test]
		public void Test_RefreshLoadedCollection_Typed_NotEQ_CriteriaString_Null_2()
		{
			//---------------Set up test pack-------------------
			SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(+3));
			ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(null, "sn", "fn");

			const string criteria = "DateOfBirth <> " + null;
			BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.Load(criteria, "");
			ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));
			ContactPersonTestBO cpNotEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(+1));
			ContactPersonTestBO cpEqualNew = ContactPersonTestBO.CreateSavedContactPerson(null, "sn2", "fn2");

			//---------------Assert Precondition ---------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.IsFalse(col.Contains(cpEqual));

			//---------------Execute Test ----------------------
			col.Refresh();

			//---------------Test Result -----------------------
			Assert.AreEqual(5, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
			Assert.Contains(cpNotEqual, col);
			Assert.IsFalse(col.Contains(cpEqualNew));
			Assert.IsFalse(col.Contains(cpEqual));
		}

		[Test]
		public void Test_RefreshLoadedCollection_ISNull_CriteriaString()
		{
			//---------------Set up test pack-------------------
			SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(+3));
			ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(null, "sn", "fn");

			const string criteria = "DateOfBirth IS NULL";
			BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.Load(criteria, "");
			ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));
			ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(+1));
			ContactPersonTestBO cpEqualNew = ContactPersonTestBO.CreateSavedContactPerson(null, "sn2", "fn2");

			//---------------Assert Precondition ---------------
			Assert.AreEqual(1, col.Count);
			Assert.IsTrue(col.Contains(cpEqual));

			//---------------Execute Test ----------------------
			col.Refresh();

			//---------------Test Result -----------------------
			Assert.AreEqual(2, col.Count);
			Assert.IsTrue(col.Contains(cpEqualNew));
			Assert.IsTrue(col.Contains(cpEqual));
		}

		[Test]
		public void Test_RefreshLoadedCollection_IsNotNull_CriteriaString()
		{
			//---------------Set up test pack-------------------
			SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(+3));
			ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(null, "sn", "fn");

			const string criteria = "DateOfBirth IS NOT NULL";
			BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.Load(criteria, "");
			ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));
			ContactPersonTestBO cpNotEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(+1));
			ContactPersonTestBO cpEqualNew = ContactPersonTestBO.CreateSavedContactPerson(null, "sn2", "fn2");

			//---------------Assert Precondition ---------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.IsFalse(col.Contains(cpEqual));

			//---------------Execute Test ----------------------
			//col.Refresh();
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
			//---------------Test Result -----------------------
			Assert.AreEqual(5, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
			Assert.Contains(cpNotEqual, col);
			Assert.IsFalse(col.Contains(cpEqualNew));
			Assert.IsFalse(col.Contains(cpEqual));
		}

		[Test]
		public void Test_RefreshLoadedCollection_Untyped_GTCriteriaObject_LoadsNewObject()
		{
			//---------------Set up test pack-------------------
			SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));

			Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-2));
			BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.Load(criteria, "");
			ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(-1));

			//---------------Assert Precondition ---------------
			Assert.AreEqual(2, col.Count);

			//---------------Execute Test ----------------------
			col.Refresh();

			//---------------Test Result -----------------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
		}

		[Test]
		public void
			Test_RefreshLoadedCollection_Untyped_GTCriteriaObject_OrderClause_DoesNotLoadNewObject()
		{
			//---------------Set up test pack-------------------
			SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now, "aaa");
			ContactPersonTestBO cpLast = ContactPersonTestBO.CreateSavedContactPerson(now, "zzz");
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now, "hhh");
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));

			Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-1));
			BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.Load(criteria, "Surname");
			ContactPersonTestBO cpnew = ContactPersonTestBO.CreateSavedContactPerson(now, "bbb");

			//---------------Assert Precondition ---------------
			Assert.AreEqual(3, col.Count);

			//---------------Execute Test ----------------------
			//            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
			col.Refresh();

			//---------------Test Result -----------------------
			Assert.AreEqual(4, col.Count);
			Assert.AreSame(cp1, col[0]);
			Assert.AreSame(cpnew, col[1]);
			Assert.AreSame(cp2, col[2]);
			Assert.AreSame(cpLast, col[3]);
		}

		[Test]
		public void Test_RefreshLoadedCollection_UsingLike()
		{
			//---------------Set up test pack-------------------
			ContactPerson.DeleteAllContactPeople();
			SetupDefaultContactPersonBO();
			//Create data in the database with the 5 contact people two with Search in surname
			CreateContactPersonInDB();
			CreateContactPersonInDB();
			CreateContactPersonInDB();
			CreateContactPersonInDB_With_SSSSS_InSurname();
			CreateContactPersonInDB_With_SSSSS_InSurname();
			BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.Load("Surname like %SSS%", "Surname");
			//---------------Assert Precondition----------------
			Assert.AreEqual(2, col.Count);
			//---------------Execute Test ----------------------
			CreateContactPersonInDB();
			CreateContactPersonInDB();
			ContactPersonTestBO cpNewLikeMatch = CreateContactPersonInDB_With_SSSSS_InSurname();
			col.Refresh();

			//---------------Test Result -----------------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cpNewLikeMatch, col);
		}

		[Test]
		public void Test_RefreshLoadedCollection_UsingNotLike()
		{
			//---------------Set up test pack-------------------
			ContactPerson.DeleteAllContactPeople();
			SetupDefaultContactPersonBO();
			//Create data in the database with the 5 contact people two with Search in surname
			CreateContactPersonInDB();
			CreateContactPersonInDB();
			CreateContactPersonInDB();
			CreateContactPersonInDB_With_SSSSS_InSurname();
			CreateContactPersonInDB_With_SSSSS_InSurname();
			var col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.Load("Surname not like %SSS%", "Surname");

			//---------------Assert Precondition----------------
			Assert.AreEqual(3, col.Count);
			//---------------Execute Test ----------------------
			CreateContactPersonInDB();
			CreateContactPersonInDB();
			ContactPersonTestBO cpNewLikeMatch = CreateContactPersonInDB_With_SSSSS_InSurname();
			col.Refresh();

			//---------------Test Result -----------------------
			Assert.AreEqual(5, col.Count);
			Assert.IsFalse(col.Contains(cpNewLikeMatch));
		}

		[Test]
		public void Test_Refresh_DoesNotRefreshDirtyOjects()
		{
			//---------------Set up test pack-------------------
			ContactPersonTestBO.DeleteAllContactPeople();
			FixtureEnvironment.ClearBusinessObjectManager();

			SetupDefaultContactPersonBO();
			var col = new BusinessObjectCollection<ContactPersonTestBO>();

			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson();
			ContactPersonTestBO.CreateSavedContactPerson();
			ContactPersonTestBO.CreateSavedContactPerson();
			col.LoadAll();
			string newSurname = Guid.NewGuid().ToString();

			//--------------------Assert Preconditions----------
			Assert.AreEqual(3, col.Count);

			//---------------Execute Test ----------------------
			cp1.Surname = newSurname;
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(3, col.Count);
			Assert.AreEqual(newSurname, cp1.Surname);
			Assert.IsTrue(cp1.Status.IsDirty);
		}

		[Test]
		public void Test_RefreshLoadedCollection_Typed_GTEQCriteriaString()
		{
			//---------------Set up test pack-------------------
			SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));
			ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(-1));

			string criteria = "DateOfBirth >= " + cpEqual.DateOfBirth;
			IBusinessObjectCollection col =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);
			ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(1));
			ContactPersonTestBO cpExclude = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));

			//---------------Assert Precondition ---------------
			Assert.AreEqual(3, col.Count);
			BusinessObjectCollection<ContactPersonTestBO> typedCol = (BusinessObjectCollection<ContactPersonTestBO>)col;
			Assert.AreEqual(3, typedCol.PersistedBusinessObjects.Count);
			Assert.Contains(cpEqual, col);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(4, col.Count);
			Assert.AreEqual(4, typedCol.PersistedBusinessObjects.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
			Assert.Contains(cpEqual, col);
			Assert.IsFalse(col.Contains(cpExclude));
		}

		[Test]
		public void Test_Refresh_LTEQCriteriaString_UnTyped()
		{
			//---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            LoadContactPersonClassDefWithIntProp();
		    ContactPersonTestBO cp1 = CreateSavedContactPerson(TestUtil.GetRandomString(), 1);
			ContactPersonTestBO cp2 = CreateSavedContactPerson(TestUtil.GetRandomString(), 1);
			CreateSavedContactPerson(TestUtil.GetRandomString(), 4);
			ContactPersonTestBO cpEqual = CreateSavedContactPerson(TestUtil.GetRandomString(), 2);

			string criteria = "IntegerProperty <= " + 2;
			IBusinessObjectCollection col =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>(criteria);
			ContactPersonTestBO cp3 = CreateSavedContactPerson(TestUtil.GetRandomString(), 1);
			ContactPersonTestBO cpExclude = CreateSavedContactPerson(TestUtil.GetRandomString(), 4);
			ContactPersonTestBO cpEqualNew = CreateSavedContactPerson(TestUtil.GetRandomString(), 2);

			//---------------Assert Precondition ---------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cpEqual, col);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(5, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
			Assert.Contains(cpEqual, col);
			Assert.Contains(cpEqualNew, col);
			Assert.IsFalse(col.Contains(cpExclude));
		}

		[Test]
		public void Test_RefreshLoadedCollection_Untyped()
		{
			//---------------Set up test pack-------------------
			SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-1));

			Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
			BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.Load(criteria, "Surname");
			ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now);
			//---------------Assert Precondition ---------------
			Assert.AreEqual(2, col.Count);

			//---------------Execute Test ----------------------
			//            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
			col.Refresh();

			//---------------Test Result -----------------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
		}
		[Test]
		public void Test_RefreshLoadedCollection_Untyped_DataAccessor()
		{
			//---------------Set up test pack-------------------
			IClassDef classDef = SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-1));

			Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
			IBusinessObjectCollection col =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
			ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now);
			//---------------Assert Precondition ---------------
			Assert.AreEqual(2, col.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
		}

		[Test]
		public void Test_RefreshLoadedCollection_TypedAsBusinessObject()
		{
			//---------------Set up test pack-------------------
			IClassDef classDef = SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-1));

			Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
			BusinessObjectCollection<BusinessObject> col = new BusinessObjectCollection<BusinessObject>(classDef);
			col.SelectQuery.Criteria = criteria;
			col.Add(cp1, cp2);
			ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now);
			//---------------Assert Precondition ---------------
			Assert.AreEqual(2, col.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(3, col.Count);
			Assert.AreEqual(3, col.PersistedBusinessObjects.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
		}


		[Test]
		public void Test_RefreshLoadedCollection_Untyped_GTCriteriaObject_DoesNotLoadNewObject()
		{
			//---------------Set up test pack-------------------
			SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));

			Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-1));
			BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
			col.Load(criteria, "");
			ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));

			//---------------Assert Precondition ---------------
			Assert.AreEqual(2, col.Count);

			//---------------Execute Test ----------------------
			//            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
			col.Refresh();

			//---------------Test Result -----------------------
			Assert.AreEqual(2, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
		}
		[Test]
		public void Test_RefreshLoadedCollection_Untyped_GTCriteriaObject_DoesNotLoadNewObject_BOLoader()
		{
			//---------------Set up test pack-------------------
			IClassDef classDef = SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));
			ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(-1));

			Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, cpEqual.DateOfBirth);
			IBusinessObjectCollection col =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
			ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));

			//---------------Assert Precondition ---------------
			Assert.AreEqual(2, col.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(2, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.IsFalse(col.Contains(cpEqual));
		}

		[Test]
		public void Test_RefreshLoadedCollection_Untyped_GTCriteriaObject_LoadsNewObject_BoLoader()
		{
			//---------------Set up test pack-------------------
			IClassDef classDef = SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));

			Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.GreaterThan, now.AddHours(-2));
			IBusinessObjectCollection col =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
			ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(-1));

			//---------------Assert Precondition ---------------
			Assert.AreEqual(2, col.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
		}

		[Test]
		public void Test_RefreshLoadedCollection_Untyped_GTCriteriaString()
		{
			//---------------Set up test pack-------------------
		    var classDef = LoadContactPersonClassDefWithIntProp();
		    ContactPersonTestBO cp1 = CreateSavedContactPerson(TestUtil.GetRandomString(), 4);
			ContactPersonTestBO cp2 = CreateSavedContactPerson(TestUtil.GetRandomString(), 4);
			CreateSavedContactPerson(TestUtil.GetRandomString(), 1);
			ContactPersonTestBO cpEqual = CreateSavedContactPerson(TestUtil.GetRandomString(), 2);

			string criteria = "IntegerProperty > " + 2;
			IBusinessObjectCollection col =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
			ContactPersonTestBO cp3 = CreateSavedContactPerson(TestUtil.GetRandomString(), 4);
			CreateSavedContactPerson(TestUtil.GetRandomString(), 2);
			//---------------Assert Precondition ---------------
			Assert.AreEqual(2, col.Count);
			Assert.IsFalse(col.Contains(cpEqual));

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
			Assert.IsFalse(col.Contains(cpEqual));
		}

		[Test]
		public void Test_RefreshLoadedCollection_Untyped_GTEQCriteriaObject()
		{
			//---------------Set up test pack-------------------
			IClassDef classDef = SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));
			ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(-1));

			Criteria criteria = new Criteria("DateofBirth", Criteria.ComparisonOp.GreaterThanEqual, cpEqual.DateOfBirth);
			IBusinessObjectCollection col =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
			ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(1));
			ContactPersonTestBO cpExclude = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));

			//---------------Assert Precondition ---------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cpEqual, col);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(4, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
			Assert.Contains(cpEqual, col);
			Assert.IsFalse(col.Contains(cpExclude));
		}

		[Test]
		public void Test_RefreshLoadedCollection_Untyped_GTEQCriteriaString()
		{
			//---------------Set up test pack-------------------
			IClassDef classDef = SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(-3));
			ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(-1));

			string criteria = "DateOfBirth >= " + cpEqual.DateOfBirth;
			IBusinessObjectCollection col =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
			ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(1));
			ContactPersonTestBO cpExclude = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));

			//---------------Assert Precondition ---------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cpEqual, col);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(4, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
			Assert.Contains(cpEqual, col);
			Assert.IsFalse(col.Contains(cpExclude));
		}


		[Test]
		public void Test_RefreshLoadedCollection_Untyped_LTEQCriteriaObject()
		{
			//---------------Set up test pack-------------------
			IClassDef classDef = SetupDefaultContactPersonBO();
			DateTime now = DateTime.Now;
			ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson(now);
			ContactPersonTestBO.CreateSavedContactPerson(DateTime.Now.AddDays(+3));
			ContactPersonTestBO cpEqual = ContactPersonTestBO.CreateSavedContactPerson(now.AddHours(+1));

			Criteria criteria = new Criteria("DateofBirth", Criteria.ComparisonOp.LessThanEqual, cpEqual.DateOfBirth);
			IBusinessObjectCollection col =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
			ContactPersonTestBO cp3 = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(-1));
			ContactPersonTestBO cpExclude = ContactPersonTestBO.CreateSavedContactPerson(now.AddDays(+1));

			//---------------Assert Precondition ---------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cpEqual, col);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(4, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
			Assert.Contains(cpEqual, col);
			Assert.IsFalse(col.Contains(cpExclude));
		}

		[Test]
		public void Test_RefreshLoadedCollection_Untyped_LTEQCriteriaString()
		{
			//---------------Set up test pack-------------------
            IClassDef classDef = LoadContactPersonClassDefWithIntProp();
			ContactPersonTestBO cp1 = CreateSavedContactPerson(TestUtil.GetRandomString(), 2);
			ContactPersonTestBO cp2 = CreateSavedContactPerson(TestUtil.GetRandomString(), 2);
			CreateSavedContactPerson(TestUtil.GetRandomString(), 4);
			ContactPersonTestBO cpEqual = CreateSavedContactPerson(TestUtil.GetRandomString(), 3);

			string criteria = "IntegerProperty <= " + 3;
			IBusinessObjectCollection col =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
			ContactPersonTestBO cp3 = CreateSavedContactPerson(TestUtil.GetRandomString(), 2);
			ContactPersonTestBO cpEqualNew = CreateSavedContactPerson(TestUtil.GetRandomString(), 3);
			ContactPersonTestBO cpExclude = CreateSavedContactPerson(TestUtil.GetRandomString(), 4);

			//---------------Assert Precondition ---------------
			Assert.AreEqual(3, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cpEqual, col);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);

			//---------------Test Result -----------------------
			Assert.AreEqual(5, col.Count);
			Assert.Contains(cp1, col);
			Assert.Contains(cp2, col);
			Assert.Contains(cp3, col);
			Assert.Contains(cpEqual, col);
			Assert.Contains(cpEqualNew, col);
			Assert.IsFalse(col.Contains(cpExclude));
		}

		[Test]
		public void Test_Refresh_WithRemovedBOs_Typed()
		{
			//---------------Set up test pack-------------------
			SetupDefaultContactPersonBO();
			ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
			BusinessObjectCollection<ContactPersonTestBO> cpCol =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
			cpCol.Remove(cp);

			//---------------Assert Precondition----------------
			Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
			Assert.AreEqual(0, cpCol.Count);

			//---------------Execute Test ----------------------
			BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpCol);

			//---------------Test Result -----------------------
			Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
			Assert.AreEqual(0, cpCol.Count);
		}

		[Test]
		public void Test_Refresh_WithSavedBOAlreadyInCollection()
		{
			//---------------Set up test pack-------------------
			SetupDefaultContactPersonBO();
			ContactPersonTestBO cp = ContactPersonTestBO.CreateUnsavedContactPerson();
			BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
			cpCol.Add(cp);
			cp.Save();
			//---------------Assert preconditions---------------
			Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
			//---------------Execute Test ----------------------
			cpCol.Refresh();
			//---------------Test Result -----------------------
			Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
		}

		#endregion
			
		private static BusinessObjectCollection<ContactPersonTestBO> CreateCol_OneCP(out ContactPersonTestBO cp)
		{
			cp = ContactPersonTestBO.CreateSavedContactPerson();
			return BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
		}
		private static BusinessObjectCollection<ContactPersonTestBO> CreateCollectionWith_OneBO()
		{
			ContactPersonTestBO.CreateSavedContactPerson();
			return BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
		}
					
		
		private static ContactPersonTestBO CreateSavedContactPerson(string surnameValue, int integerPropertyValue)
		{
			ContactPersonTestBO cp = new ContactPersonTestBO();
			cp.Surname = surnameValue;
			cp.SetPropertyValue("IntegerProperty", integerPropertyValue);
			cp.Save();
			return cp;
		}

		private static ContactPersonTestBO CreateContactPersonInDB_With_SSSSS_InSurname()
		{
			ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
			contactPersonTestBO.Surname = Guid.NewGuid().ToString("N") + "SSSSS";
			contactPersonTestBO.Save();
			return contactPersonTestBO;
		}

		private static void CreateContactPersonInDB()
		{
			ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
			contactPersonTestBO.Surname = Guid.NewGuid().ToString("N");
			contactPersonTestBO.Save();
			return;
		}
		

  }      
}
