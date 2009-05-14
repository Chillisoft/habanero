using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.DB4O;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.DB4O
{
    [TestFixture]
    public class TestBusinessObjectDTO
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear(); ;
            BOBroker.LoadClassDefs();
        }

        [Test]
        public void TestConstructDTO_ClassName()
        {
            //---------------Set up test pack-------------------
           
            Person person = new Person {FirstName = TestUtil.GetRandomString(), LastName = TestUtil.GetRandomString()};

            //---------------Execute Test ----------------------
            BusinessObjectDTO dto = new BusinessObjectDTO(person);

            //---------------Test Result -----------------------
            Assert.AreEqual(person.ClassDef.ClassName, dto.ClassDefName);
            Assert.AreEqual(typeof(Person).Name, dto.ClassName);
            Assert.AreEqual(typeof(Person).Assembly.GetName().Name, dto.AssemblyName);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestConstructDTO_Props()
        {
            //---------------Set up test pack-------------------
            Person person = new Person { FirstName = TestUtil.GetRandomString(), LastName = TestUtil.GetRandomString() };

            //---------------Execute Test ----------------------
            BusinessObjectDTO dto = new BusinessObjectDTO(person);

            //---------------Test Result -----------------------
            Assert.AreEqual(person.Props.Count, dto.Props.Count);
            Assert.IsTrue(dto.Props.ContainsKey("LASTNAME"));
            Assert.AreEqual(person.LastName, dto.Props["LASTNAME"]);
            Assert.IsTrue(dto.Props.ContainsKey("FIRSTNAME"));
            Assert.AreEqual(person.FirstName, dto.Props["FIRSTNAME"]);
            Assert.IsTrue(dto.Props.ContainsKey("PERSONID"));
            Assert.AreEqual(person.PersonID, dto.Props["PERSONID"]);

            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestConstructDTO_ID()
        {
            //---------------Set up test pack-------------------
            Person person = new Person { FirstName = TestUtil.GetRandomString(), LastName = TestUtil.GetRandomString() };

            //---------------Execute Test ----------------------
            BusinessObjectDTO dto = new BusinessObjectDTO(person);

            //---------------Test Result -----------------------
            Assert.AreEqual(person.ID.ToString(), dto.ID);

            //---------------Tear Down -------------------------          
        }


    }
}