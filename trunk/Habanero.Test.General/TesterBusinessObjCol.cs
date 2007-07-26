using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for BusinessObjColTester.
    /// </summary>
    [TestFixture]
    public class TesterBusinessObjCol : TestUsingDatabase
    {
        public TesterBusinessObjCol()
        {
        }

        /// <summary>
        /// Used by Gui to step through the application. If the reason for failing a test is 
        /// not obvious.
        /// </summary>
        public static void RunTest()
        {
            TesterBusinessObjCol test = new TesterBusinessObjCol();
            //test.CreateTestPack();
            //			test.TestLoadBusinessObjects();
            //			test.TestLoadBusinessObjectsFromObjectManager();
            //			test.TestLoadBusinessObjectsSearchCriteria();
            //			test.TestLoadBusinessObjects();
        }

        [Test]
        public void TestLoadBusinessObjects()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<BusinessObject> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);
            ContactPerson p = new ContactPerson();
            p.FirstName = "a";
            p.Surname = "bb";
            p.Save();
            ContactPerson.ClearContactPersonCol();
            myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(1, myCol.Count);
        }

        [Test]
        public void TestLoadBusinessObjectsFromObjectManager()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<BusinessObject> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);
            ContactPerson p = new ContactPerson();
            p.FirstName = "a";
            p.Surname = "bb";
            p.Save();
            BOPrimaryKey pKey = p.ID;
            ContactPerson.ClearContactPersonCol();
            p = ContactPerson.GetContactPerson(pKey);
            myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(1, myCol.Count);
            Assert.AreSame(p, myCol[0]);
        }

        [Test]
        public void TestLoadBusinessObjectsFromObjectManagerAndFresh()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<BusinessObject> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);
            ContactPerson p = new ContactPerson();
            p.FirstName = "a";
            p.Surname = "bb";
            p.Save();
            BOPrimaryKey pKey = p.ID;
            p = new ContactPerson();
            p.FirstName = "aa";
            p.Surname = "abc";
            p.Save();
            ContactPerson.ClearContactPersonCol();
            p = ContactPerson.GetContactPerson(pKey);
            myCol = ContactPerson.LoadBusinessObjCol("", "Surname ASC");
            Assert.AreEqual(2, myCol.Count);
            Assert.AreSame(p, myCol[1]);
        }

        [Test]
        public void TestLoadBusinessObjectsSortOrder()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<BusinessObject> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);
            ContactPerson p = new ContactPerson();
            p.FirstName = "a";
            p.Surname = "bb";
            p.Save();
            BOPrimaryKey pKey = p.ID;
            p = new ContactPerson();
            p.FirstName = "aa";
            p.Surname = "abc";
            p.Save();
            ContactPerson.ClearContactPersonCol();
            p = ContactPerson.GetContactPerson(pKey);
            myCol = ContactPerson.LoadBusinessObjCol("", "Surname Desc");
            Assert.AreEqual(2, myCol.Count);
            Assert.AreSame(p, myCol[0]);
        }

        [Test]
        public void TestLoadBusinessObjectsSearchCriteria()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<BusinessObject> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);
            ContactPerson p = new ContactPerson();
            p.FirstName = "a";
            p.Surname = "bb";
            p.Save();
            BOPrimaryKey pKey = p.ID;
            p = new ContactPerson();
            p.FirstName = "aa";
            p.Surname = "abc";
            p.Save();
            ContactPerson.ClearContactPersonCol();
            p = ContactPerson.GetContactPerson(pKey);
            myCol = ContactPerson.LoadBusinessObjCol("Surname = 'bb'", "Surname");
            Assert.AreEqual(1, myCol.Count);
            Assert.AreSame(p, myCol[0]);
        }

        [Test]
        public void TestLoadBusinessObjectsSearchCriteriaWithOR()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<BusinessObject> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);
            ContactPerson p = new ContactPerson();
            p.FirstName = "a";
            p.Surname = "bb";
            p.Save();
            BOPrimaryKey pKey = p.ID;
            p = new ContactPerson();
            p.FirstName = "aa";
            p.Surname = "abc";
            p.Save();

            p = new ContactPerson();
            p.FirstName = "aa";
            p.Surname = "abcd";
            p.Save();

            ContactPerson.ClearContactPersonCol();

            //TODO:			IExpression exp1 = new Parameter(
            myCol = ContactPerson.LoadBusinessObjCol("Surname = 'bb' or Surname = 'abc'", "Surname");
            Assert.AreEqual(2, myCol.Count);
            myCol = ContactPerson.LoadBusinessObjCol("Surname = 'bb' or Surname like 'abc%'", "Surname");
            Assert.AreEqual(3, myCol.Count);
        }

        [Test]
        public void TestRefreshBOCol()
        {
            ContactPerson.DeleteAllContactPeople();
            BusinessObjectCollection<BusinessObject> myCol = ContactPerson.LoadBusinessObjCol();
            Assert.AreEqual(myCol.Count, 0);
            ContactPerson p = new ContactPerson();
            p.FirstName = "a";
            p.Surname = "bb";
            p.Save();
            BOPrimaryKey pKey = p.ID;
            p = new ContactPerson();
            p.FirstName = "aa";
            p.Surname = "abc";
            p.Save();

            p = new ContactPerson();
            p.FirstName = "aa";
            p.Surname = "abcd";
            p.Save();

            ContactPerson.ClearContactPersonCol();

            //TODO:			IExpression exp1 = new Parameter(
            myCol = ContactPerson.LoadBusinessObjCol("Surname = 'bb' or Surname = 'abc'", "Surname");
            Assert.AreEqual(2, myCol.Count);
            //ensure that a new object is created to edit. to simulate multi user editing of objects.
            ContactPerson.ClearContactPersonCol();
            p = ContactPerson.GetContactPerson(pKey);

            p.Surname = "zzz";
            p.Save();

            Assert.AreEqual(2, myCol.Count,
                            "The object collection should not have changed since the physical object edited is different.");

            myCol.Refresh();
            Assert.AreEqual(1, myCol.Count,
                            "The object collection should now have fewer object since it has been reloaded from the database.");

            p = (ContactPerson) myCol[0];
            Assert.AreEqual("abc", p.Surname);
        }
    }
}