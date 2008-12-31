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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestDeleteHelper
    {
        private ClassDef _baseClassDef;

        [SetUp]
        public void SetupTest()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            _baseClassDef = CreateClassDef(1, false, true, false);
            ClassDef.ClassDefs.Add(_baseClassDef);
            ClassDef.ClassDefs.Add(CreateClassDef(2, true, true, true));
            ClassDef.ClassDefs.Add(CreateClassDef(3, true, true, false));
            ClassDef.ClassDefs.Add(CreateClassDef(4, false, true, true));
            ClassDef.ClassDefs.Add(CreateClassDef(5, true, true, false));
            ClassDef.ClassDefs.Add(CreateClassDef(6, true, true, true));
            ClassDef.ClassDefs.Add(CreateClassDef(7, true, false, false));
        }

        [Test]
        public void TestFindPreventDeleteRelationships()
        {
            //List<List<string>> listOfPaths;
            MatchList listOfPaths = DeleteHelper.FindPreventDeleteRelationships(_baseClassDef.RelationshipDefCol);
            //Assert.AreEqual(3, listOfPaths.Count, "There should be 3 prevent delete relationships found");
            string relationshipPath = listOfPaths.ToString(".");
            Assert.AreEqual("MyBO2.{MyBO3.MyBO4.{MyBO5.MyBO6.MyPreventBO7,MyPreventBO5},MyPreventBO3}", relationshipPath);
        }

        [Test]
        public void CheckCanDeleteWithSomePrevents()
        {
            TestBO1 testBO1 = PopulateObjectWithSomePrevents();
            string result;
            bool canDelete = DeleteHelper.CheckCanDelete(testBO1, out result);
            Assert.IsFalse(canDelete, "Should prevent delete.");
            Assert.AreEqual("Cannot delete this 'TestBO1' for the following reasons:" + Environment.NewLine +
                    "There are 7 objects related through the 'MyBO2.MyPreventBO3' relationship that need to be deleted first.",
                    result);
        }


        [Test]
        public void CheckCanDeleteWithNoPrevents()
        {
            TestBO1 testBO1 = PopulateObjectWithNoPrevents();
            string result;
            bool canDelete = DeleteHelper.CheckCanDelete(testBO1, out result);
            Assert.IsTrue(canDelete, "Should prevent delete.");
            Assert.AreEqual("", result);
        }

        [Test]
        public void CheckCanDeleteWithTieredPrevents()
        {
            TestBO1 testBO1 = PopulateObjectWithTieredPrevents();
            string result;
            bool canDelete = DeleteHelper.CheckCanDelete(testBO1, out result);
            Assert.IsFalse(canDelete, "Should prevent delete.");
            Assert.AreEqual("Cannot delete this 'TestBO1' for the following reasons:" + Environment.NewLine +
                    "There are 9 objects related through the 'MyBO2.MyBO3.MyBO4.MyPreventBO5' relationship that need to be deleted first." + Environment.NewLine +
                    "There are 7 objects related through the 'MyBO2.MyPreventBO3' relationship that need to be deleted first." + Environment.NewLine +
                    "There are 1 objects related through the 'MyBO2.MyBO3.MyBO4.MyBO5.MyBO6.MyPreventBO7' relationship that need to be deleted first.",
                    result);
        }

        #region Object Structure

        private static TestBO1 PopulateObjectWithSomePrevents()
        {
            TestBO1 testBO1 = new TestBO1();
            testBO1.MyBoID = "1";
            testBO1.SetStatus(BOStatus.Statuses.isNew, false);
            IBusinessObjectCollection children = AddRelatedObjects<TestBO2>(testBO1, "MyBO2", 3);
            AddRelatedObjects<TestBO3>((TestBO)children[0], "MyBO3", 2);
            AddRelatedObjects<TestBO3>((TestBO)children[0], "MyPreventBO3", 1);
            AddRelatedObjects<TestBO3>((TestBO)children[1], "MyBO3", 2);
            AddRelatedObjects<TestBO3>((TestBO)children[1], "MyPreventBO3", 4);
            AddRelatedObjects<TestBO3>((TestBO)children[2], "MyPreventBO3", 2);
            return testBO1;
        }

        private static TestBO1 PopulateObjectWithNoPrevents()
        {
            TestBO1 testBO1 = new TestBO1();
            testBO1.MyBoID = "1";
            IBusinessObjectCollection children = AddRelatedObjects<TestBO2>(testBO1, "MyBO2", 3);
            AddRelatedObjects<TestBO3>((TestBO)children[0], "MyBO3", 2);
            children = AddRelatedObjects<TestBO3>((TestBO)children[1], "MyBO3", 2);
            AddRelatedObjects<TestBO4>((TestBO)children[0], "MyBO4", 2);
            AddRelatedObjects<TestBO4>((TestBO)children[1], "MyBO4", 2);
            return testBO1;
        }

        private static TestBO1 PopulateObjectWithTieredPrevents()
        {
            TestBO1 testBO1 = new TestBO1();
            testBO1.MyBoID = "1";
            IBusinessObjectCollection children2 = AddRelatedObjects<TestBO2>(testBO1, "MyBO2", 3);
            IBusinessObjectCollection children3 = AddRelatedObjects<TestBO3>((TestBO)children2[0], "MyBO3", 2);
            IBusinessObjectCollection children4 = AddRelatedObjects<TestBO4>((TestBO)children3[0], "MyBO4", 1);
            AddRelatedObjects<TestBO5>((TestBO)children4[0], "MyPreventBO5", 1);
            AddRelatedObjects<TestBO3>((TestBO)children2[0], "MyPreventBO3", 1);
            AddRelatedObjects<TestBO3>((TestBO)children2[1], "MyBO3", 2);
            AddRelatedObjects<TestBO3>((TestBO)children2[1], "MyPreventBO3", 4);
            AddRelatedObjects<TestBO3>((TestBO)children2[2], "MyPreventBO3", 2);
            children3 = AddRelatedObjects<TestBO3>((TestBO)children2[2], "MyBO3", 3);
            children4 = AddRelatedObjects<TestBO4>((TestBO)children3[0], "MyBO4", 2);
            AddRelatedObjects<TestBO5>((TestBO)children4[0], "MyPreventBO5", 1);
            children4 = AddRelatedObjects<TestBO4>((TestBO)children3[1], "MyBO4", 2);
            IBusinessObjectCollection children5 = AddRelatedObjects<TestBO5>((TestBO)children4[1], "MyBO5", 2);
            IBusinessObjectCollection children6 = AddRelatedObjects<TestBO6>((TestBO)children5[1], "MyBO6", 2);
            AddRelatedObjects<TestBO7>((TestBO)children6[1], "MyBO7", 2);
            AddRelatedObjects<TestBO7>((TestBO)children6[1], "MyPreventBO7", 1);
            AddRelatedObjects<TestBO5>((TestBO)children4[1], "MyPreventBO5", 3);
            children4 = AddRelatedObjects<TestBO4>((TestBO)children3[2], "MyBO4", 4);
            AddRelatedObjects<TestBO5>((TestBO)children4[0], "MyBO5", 2);
            AddRelatedObjects<TestBO5>((TestBO)children4[0], "MyPreventBO5", 4);

            return testBO1;
        }

        private static IBusinessObjectCollection AddRelatedObjects<T>(TestBO testBO,
            string relationshipName, int numberOfBos)
            where T : TestBO, new()
        {
            return AddRelatedObjects<T>(testBO, relationshipName, numberOfBos, false);
        }

        private static IBusinessObjectCollection AddRelatedObjects<T>(TestBO testBO,
            string relationshipName, int numberOfBos, bool isNew)
            where T : TestBO, new()
        {
            IBusinessObjectCollection children = testBO.Relationships.GetRelatedCollection(relationshipName);
            for (int count = 1; count <= numberOfBos; count++)
            {
                T testBO2 = new T();
                testBO2.SetStatus(BOStatus.Statuses.isNew, isNew);
                testBO2.MyBoID = "2." + count;
                testBO2.MyParentBoID = testBO.MyParentBoID;
                children.Add(testBO2);
            }
            return children;
        }

        #endregion //Object Structure

        #region Class Defs

        private static ClassDef CreateClassDef(long number, bool hasSingleRelationship,
            bool hasMultipleRelationship, bool hasMultipleRelationshipWithPreventDelete)
        {
            const string assemblyName = "Habanero.Test.BO";
            const string className = "TestBO";
            const string idPropName = "MyBoID";
            const string fkPropertyName = "MyParentBoID";
            string suffix = number.ToString();
            PropDefCol propDefCol = new PropDefCol();
            PrimaryKeyDef primaryKeyDef = new PrimaryKeyDef();
            primaryKeyDef.IsGuidObjectID = false;
            RelationshipDefCol relationshipDefCol = new RelationshipDefCol();
            PropDef idPropDef = new PropDef(idPropName, typeof(string), PropReadWriteRule.ReadWrite, "");
            propDefCol.Add(idPropDef);
            primaryKeyDef.Add(idPropDef);
            //propDef = new PropDef("MyProp", typeof(string), PropReadWriteRule.ReadWrite, "" );
            //propDefCol.Add(propDef);
            PropDef propDef = new PropDef(fkPropertyName, typeof(string), PropReadWriteRule.ReadWrite, "");
            propDefCol.Add(propDef);
            if (hasSingleRelationship)
            {
                string relatedClassSuffix = (number - 1).ToString();
                RelKeyDef relKeyDef = new RelKeyDef();
                RelPropDef relPropDef = new RelPropDef(propDef, idPropName);
                relKeyDef.Add(relPropDef);
                SingleRelationshipDef singleRelationshipDef = new SingleRelationshipDef(
                    "MyParent", assemblyName,
                    className + relatedClassSuffix, relKeyDef, false, DeleteParentAction.Prevent);
                relationshipDefCol.Add(singleRelationshipDef);
            }
            if (hasMultipleRelationship)
            {
                string relatedClassSuffix = (number + 1).ToString();
                RelKeyDef relKeyDef = new RelKeyDef();
                RelPropDef relPropDef = new RelPropDef(idPropDef, fkPropertyName);
                relKeyDef.Add(relPropDef);
                MultipleRelationshipDef multipleRelationshipDef =
                    new MultipleRelationshipDef("MyBO" + relatedClassSuffix,
                    assemblyName, className + relatedClassSuffix, relKeyDef, false,
                    "", DeleteParentAction.DeleteRelated);
                relationshipDefCol.Add(multipleRelationshipDef);
            }
            if (hasMultipleRelationshipWithPreventDelete)
            {
                string relatedClassSuffix = (number + 1).ToString();
                RelKeyDef relKeyDef = new RelKeyDef();
                RelPropDef relPropDef = new RelPropDef(idPropDef, fkPropertyName);
                relKeyDef.Add(relPropDef);
                MultipleRelationshipDef multipleRelationshipDef =
                    new MultipleRelationshipDef("MyPreventBO" + relatedClassSuffix,
                    assemblyName, className + relatedClassSuffix, relKeyDef, false,
                    "", DeleteParentAction.Prevent);
                relationshipDefCol.Add(multipleRelationshipDef);
            }
            ClassDef classDef = new ClassDef(assemblyName, className + suffix,
                primaryKeyDef, propDefCol, new KeyDefCol(), relationshipDefCol, new UIDefCol());
            return classDef;
        }

        #endregion //Class Defs
    }

    #region Test BOs

    public abstract class TestBO : BusinessObject
    {
        private readonly MockRepository _mock;

        protected TestBO()
        {
            _mock = new MockRepository();
            RelationshipCol relationshipCol = new RelationshipCol(this);
            this.Relationships = relationshipCol;
            int childSuffix = TypeNameNumber + 1;
            AddChildIfNeeded("MyBO" + childSuffix, relationshipCol);
            AddChildIfNeeded("MyPreventBO" + childSuffix, relationshipCol);
            _mock.ReplayAll();
        }

        private void AddChildIfNeeded(string childName, RelationshipCol relationshipCol)
        {
            RelationshipDefCol relationshipDefCol = ClassDef.RelationshipDefCol;
            if (relationshipDefCol.Contains(childName))
            {
                RelationshipDef relationshipDef = relationshipDefCol[childName];
                ClassDef classDef = relationshipDef.RelatedObjectClassDef;
                IMultipleRelationship relationship = (IMultipleRelationship) relationshipDef.CreateRelationship(this, this._boPropCol);
                //_mock.DynamicMock<IMultipleRelationship>();
                //(this, relationshipDef, this._boPropCol);
                //BusinessObjectCollection<BusinessObject> businessObjectCollection = new BusinessObjectCollection<BusinessObject>(classDef);
                //SetupResult.For(relationship.BusinessObjectCollection)
                //    .Return(businessObjectCollection);
                //SetupResult.For(relationship.RelationshipName)
                //    .Return(businessObjectCollection);

                //SetupResult.For(relationship.GetRelatedBusinessObjectCol<BusinessObject>())
                //    .Return(businessObjectCollection);
                relationshipCol.Add(relationship);
            }
        }

        private int TypeNameNumber
        {
            get
            {
                string typeName = GetType().Name.Replace("TestBO", "");
                int val;
                if (int.TryParse(typeName, out val))
                {
                    return val;
                }
                return 0;
            }
        }

        public string MyBoID
        {
            get { return (string)GetPropertyValue("MyBoID"); }
            set { SetPropertyValue("MyBoID", value); }
        }

        public string MyParentBoID
        {
            get { return (string)GetPropertyValue("MyParentBoID"); }
            set { SetPropertyValue("MyParentBoID", value); }
        }
    }

    public class TestBO1 : TestBO { }
    public class TestBO2 : TestBO { }
    public class TestBO3 : TestBO { }
    public class TestBO4 : TestBO { }
    public class TestBO5 : TestBO { }
    public class TestBO6 : TestBO { }
    public class TestBO7 : TestBO { }

    #endregion //Test BOs


}
