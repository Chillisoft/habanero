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

using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestSingleRelationship.
    /// </summary>
    [TestFixture]
    public class TestSingleRelationship
    {
        private ClassDef itsClassDef;
        private ClassDef itsRelatedClassDef;

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            ClassDef.ClassDefs.Clear();
            itsClassDef = MyBO.LoadClassDefWithRelationship();
            itsRelatedClassDef = MyRelatedBo.LoadClassDef();
        }

        [Test]
        public void TestSetRelatedObject()
        {
            MyBO bo1 = (MyBO) itsClassDef.CreateNewBusinessObject();
            MyRelatedBo relatedBo1 = (MyRelatedBo) itsRelatedClassDef.CreateNewBusinessObject();
            ((SingleRelationship) bo1.Relationships["MyRelationship"]).SetRelatedObject(relatedBo1);
            Assert.AreSame(relatedBo1, bo1.Relationships.GetRelatedObject<MyRelatedBo>("MyRelationship"));
            Assert.AreSame(bo1.GetPropertyValue("RelatedID"), relatedBo1.GetPropertyValue("MyRelatedBoID"));
        }
    }
}