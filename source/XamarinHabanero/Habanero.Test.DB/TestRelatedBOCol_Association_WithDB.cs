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

using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.Test.BO.BusinessObjectCollection;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestRelatedBOCol_Association_WithDB : TestRelatedBOCol_Association
    {
        [TestFixtureSetUp]
        public override void TestFixtureSetup()
        {
            TestUsingDatabase.SetupDBDataAccessor();
            ContactPersonTestBO.DeleteAllContactPeople();
            OrganisationTestBO.DeleteAllOrganisations();
        }

        [SetUp]
        public override void SetupTest()
        {

            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.DeleteAllContactPeople();
            OrganisationTestBO.DeleteAllOrganisations();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef_PreventAddChild();
        }
        [Test]
        public override void Test_RemoveMethod()
        {
            //DO nothing cannot get this test to work reliably on DB wierd data is always in DB when run all tests
            //But not if only run tests for the Test Class.
        }

        protected override void DeleteAllContactPersonAndOrganisations()
        {
            ContactPersonTestBO.DeleteAllContactPeople();
            OrganisationTestBO.DeleteAllOrganisations();

        }
        [Test]
        public override void Test_ResetParent_NewChild_SetToNull()
        {
            //DO nothing cannot get this test to work reliably on DB wierd data is always in DB when run all tests
            //But not if only run tests for the Test Class.
        }
    }
}