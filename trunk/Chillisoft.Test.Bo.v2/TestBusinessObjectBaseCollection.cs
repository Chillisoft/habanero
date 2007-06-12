using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Test.Setup.v2;
using NUnit.Framework;
using System;

namespace Chillisoft.Test.Bo.v2
{
    /// <summary>
    /// Summary description for TestBusinessObjectBaseCollection.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectBaseCollection : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            this.SetupDBConnection();
        }

        [Test]
        public void TestIntersection()
        {
            ClassDef.GetClassDefCol.Clear();
            ClassDef itsClassDef = MyBo.LoadDefaultClassDef();
            BusinessObjectBase bo1 = itsClassDef.CreateNewBusinessObject();
            BusinessObjectBase bo2 = itsClassDef.CreateNewBusinessObject();
            BusinessObjectBaseCollection col1 = new BusinessObjectBaseCollection(itsClassDef);

            col1.Add(itsClassDef.CreateNewBusinessObject());
            col1.Add(bo2);
            col1.Add(itsClassDef.CreateNewBusinessObject());
            col1.Add(bo1);

            BusinessObjectBaseCollection col2 = new BusinessObjectBaseCollection(itsClassDef);
            col2.Add(itsClassDef.CreateNewBusinessObject());
            col2.Add(bo1);
            col2.Add(bo2);
            col2.Add(itsClassDef.CreateNewBusinessObject());

            BusinessObjectBaseCollection intersectionCol = col1.Intersection(col2);
            Assert.AreEqual(2, intersectionCol.Count);
        }

        [Test]
        public void TestUnion()
        {
            ClassDef.GetClassDefCol.Clear();
            ClassDef itsClassDef = MyBo.LoadDefaultClassDef();
            BusinessObjectBase bo1 = itsClassDef.CreateNewBusinessObject();
            BusinessObjectBase bo2 = itsClassDef.CreateNewBusinessObject();
            BusinessObjectBaseCollection col1 = new BusinessObjectBaseCollection(itsClassDef);

            col1.Add(itsClassDef.CreateNewBusinessObject());
            col1.Add(bo2);
            col1.Add(itsClassDef.CreateNewBusinessObject());
            col1.Add(bo1);

            BusinessObjectBaseCollection col2 = new BusinessObjectBaseCollection(itsClassDef);
            col2.Add(itsClassDef.CreateNewBusinessObject());
            col2.Add(bo1);
            col2.Add(bo2);
            col2.Add(itsClassDef.CreateNewBusinessObject());

            BusinessObjectBaseCollection unionCol = col1.Union(col2);
            Assert.AreEqual(6, unionCol.Count);
        }

        [Test]
        public void TestDeletingRemovesObjectFromCollection()
        {
            ClassDef.GetClassDefCol.Clear();
            ClassDef itsClassDef = MyBo.LoadDefaultClassDef();
            BusinessObjectBase bo1 = itsClassDef.CreateNewBusinessObject();
            BusinessObjectBaseCollection col1 = new BusinessObjectBaseCollection(itsClassDef);
            col1.Add(bo1);
            col1.Add(itsClassDef.CreateNewBusinessObject());

            Assert.AreEqual(2, col1.Count);

            bo1.Delete();
            bo1.ApplyEdit();
            Assert.AreEqual(1, col1.Count);
        }

        [Test]
        public void TestFindByGuid()
        {
            ClassDef.GetClassDefCol.Clear();
            ClassDef itsClassDef = MyBo.LoadDefaultClassDef();
            BusinessObjectBase bo1 = itsClassDef.CreateNewBusinessObject();
            BusinessObjectBase bo2 = itsClassDef.CreateNewBusinessObject();
            BusinessObjectBaseCollection col1 = new BusinessObjectBaseCollection(itsClassDef);
            col1.Add(bo1);
            col1.Add(itsClassDef.CreateNewBusinessObject());

            Assert.AreEqual(bo1, col1.FindByGuid(bo1.ID.GetGuid()));
            Assert.AreEqual(null, col1.FindByGuid(bo2.ID.GetGuid()));
        }
    }
}