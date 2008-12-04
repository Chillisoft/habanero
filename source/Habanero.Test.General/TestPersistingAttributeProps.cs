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
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.General
{
    [TestFixture]
    public class TestTestPersistingAttributeProps : TestUsingDatabase
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            base.SetupDBConnection();
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }
        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
           
        }

        [Test]
        public void TestNonPersistablePropDef()
        {
            //Create Contact person
            ContactPerson person = new ContactPerson();
            ClassDef contactPersonClassdef = person.ClassDef;
            ClassDef clonedClassDef = contactPersonClassdef.Clone();

            //add non persistable attribute.
            PropDef propDef = new PropDef("NewName", typeof(string), PropReadWriteRule.ReadWrite, "");
            propDef.Persistable = false;
            clonedClassDef.PropDefcol.Add(propDef);

            ContactPerson savePerson = new ContactPerson(clonedClassDef);
            savePerson.Surname = Guid.NewGuid().ToString();
            savePerson.FirstName = Guid.NewGuid().ToString();

            //Get sql or save or whatever
            savePerson.Save();
            //check that non persistable not included in SQL.
            Assert.IsFalse(savePerson.Status.IsNew);
        }

        //[Test]
        //public void TestLoadAttribute()
        //{
        //    //---------------Set up test pack-------------------
        //    //Create Contact person
        //    ContactPerson person = new ContactPerson();
        //    ClassDef contactPersonClassdef = person.ClassDef;
        //    ClassDef clonedClassDef = contactPersonClassdef.Clone();

        //    //add non persistable attribute.
        //    PropDef propDef = new PropDef("NewName", typeof(string), PropReadWriteRule.ReadWrite, "");
        //    propDef.Persistable = false;
        //    clonedClassDef.PropDefcol.Add(propDef);

        //    ContactPerson savePerson = new ContactPerson(clonedClassDef);
        //    savePerson.Surname = Guid.NewGuid().ToString();
        //    savePerson.FirstName = Guid.NewGuid().ToString();

        //    //Get sql or save or whatever
        //    savePerson.Save();
        //    //---------------Execute Test ----------------------
        //    //---------------Test Result -----------------------
        //}

    }
}
