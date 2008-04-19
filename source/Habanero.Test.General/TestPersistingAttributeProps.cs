using System;
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
            Assert.IsFalse(savePerson.State.IsNew);
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
