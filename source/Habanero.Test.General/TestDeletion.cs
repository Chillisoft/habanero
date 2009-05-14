//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.General
{
	/// <summary>
	/// Class that tests different aspects of deletion from a Business object perspective
	/// </summary>
	[TestFixture]
	public class TestDeletion : TestUsingDatabase
	{
		private ContactPerson _person;
		private Address _address1;
		private Address _address2;
			
		[TestFixtureSetUp]
		public void SetupFixture()
		{
			this.SetupDBConnection();
		}

		[SetUp]
		public void SetupTest()
		{
			ClassDef.ClassDefs.Clear();
		    new Address();
            new Engine();
		    new Car();
            ContactPerson.DeleteAllContactPeople();

         
			_person = new ContactPerson();
			_person.FirstName = "Joe";
			_person.Surname = "Soap";
			_person.DateOfBirth = new DateTime(2001,01,01);
			_person.Save();
			_address1 = new Address();
			_address1.AddressLine1 = "1 Test Road";
			_address1.AddressLine2 = "Test Suburb";
			_address1.AddressLine3 = "Test City";
			_address1.AddressLine4 = "Test Country";
			_address1.Relationships.SetRelatedObject("ContactPerson", _person);
			_address1.Save();
			_address2 = new Address();
			_address2.AddressLine1 = "2 Test Road";
			_address2.AddressLine2 = "Test Suburb";
			_address2.AddressLine3 = "Test City";
			_address2.AddressLine4 = "Test Country";
			_address2.Relationships.SetRelatedObject("ContactPerson", _person);
			_address2.Save();
		}

		[TestFixtureTearDown]
		public void TearDownTest()
		{
			Address.DeleteAllAddresses();
			ContactPerson.DeleteAllContactPeople();
		}

        [Test]
        public void TestCascadeDelete()
        {
            Assert.AreEqual(2, _person.Addresses.Count);
            _person.MarkForDelete();
            _person.Save();
            Assert.IsTrue(_person.Status.IsDeleted);
            Assert.AreEqual(0, _person.Addresses.Count);
        }
	}
}
