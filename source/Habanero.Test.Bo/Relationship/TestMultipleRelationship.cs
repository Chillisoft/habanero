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

using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestMultipleRelationship : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            base.SetupDBConnection();
        }

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [Test]
        public void TestTypeOfMultipleCollection()
        {
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            new AddressTestBO();

            ContactPersonTestBO cp = new ContactPersonTestBO();

            Assert.AreSame(typeof(RelatedBusinessObjectCollection<AddressTestBO>), cp.Addresses.GetType());
        }

        [Test]
        public void TestReloadingRelationship()
        {
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            new AddressTestBO();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            IBusinessObjectCollection addresses = cp.Addresses;
            Assert.AreSame(addresses, cp.Addresses);
        }

        [Test]
        public void TestColIsInstantiatedButNotLoaded()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef();
            ClassDef contactPersonClassDef = ContactPersonTestBO.LoadClassDefOrganisationRelationship();
            RelKeyDef keyDef = new RelKeyDef();
            keyDef.Add(new RelPropDef(contactPersonClassDef.PropDefcol["OrganisationID"], "OrganisationID"));
            MultipleRelationshipDef def = new MultipleRelationshipDef
                (TestUtil.CreateRandomString(), typeof(ContactPersonTestBO),
                 keyDef, false, "", DeleteParentAction.DoNothing);

            OrganisationTestBO organisation = new OrganisationTestBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IMultipleRelationship rel = (IMultipleRelationship) def.CreateRelationship(organisation, organisation.Props);

            //---------------Test Result -----------------------

            IBusinessObjectCollection collection = rel.BusinessObjectCollection;
            Assert.IsNotNull(collection);
            Assert.AreEqual(0, collection.Count);
            Assert.AreSame(contactPersonClassDef, collection.ClassDef);
            Assert.IsInstanceOfType(typeof(ContactPersonTestBO), collection.CreateBusinessObject());
        }
    }
}
