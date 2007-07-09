using System;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.DB;
using Habanero.Base;
using Habanero.Test;
using NMock;
using NUnit.Framework;
using BusinessObject = Habanero.Bo.BusinessObject;

namespace Habanero.Test.Bo
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
            ClassDef myboClassDef = MyBo.LoadDefaultClassDef();
        }

        [Test]
        public void TestInstantiate()
        {
            BusinessObjectCollection<MyBo> col = new BusinessObjectCollection<MyBo>();
            Assert.AreSame(ClassDef.ClassDefs[typeof(MyBo)], col.ClassDef);
        }

        [Test]
        public void TestFindByGuid()
        {
            BusinessObjectCollection<MyBo> col = new BusinessObjectCollection<MyBo>();
            MyBo bo1 = new MyBo();
            col.Add(bo1);
            col.Add(new MyBo());
            Assert.AreSame(bo1, col.FindByGuid(bo1.MyBoID));
        }

    }
}