using System;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.DB;
using Habanero.Base;
using Habanero.Test;
using NMock;
using NUnit.Framework;
using BusinessObject = Habanero.BO.BusinessObject;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestBusinessObjectCollection.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectCollection : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            base.SetupDBConnection();
            ClassDef.ClassDefs.Clear();
            ClassDef myboClassDef = MyBO.LoadDefaultClassDef();
        }

        [Test]
        public void TestInstantiate()
        {
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            Assert.AreSame(ClassDef.ClassDefs[typeof(MyBO)], col.ClassDef);
        }

        [Test]
        public void TestFindByGuid()
        {
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO bo1 = new MyBO();
            col.Add(bo1);
            col.Add(new MyBO());
            Assert.AreSame(bo1, col.FindByGuid(bo1.MyBoID));
        }

    }
}