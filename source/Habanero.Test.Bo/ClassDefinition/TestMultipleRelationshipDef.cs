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
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestMultipleRelationshipDef
    {
        [Test]
        public void TestProtectedSets()
        {
            MultipleRelationshipDefInheritor relDef = new MultipleRelationshipDefInheritor();

            Assert.AreEqual("acolumn ASC", relDef.OrderCriteria.ToString());
            relDef.SetOrderBy(new OrderCriteria().Add("somecolumn"));
            Assert.AreEqual("somecolumn ASC", relDef.OrderCriteria.ToString());

            Assert.AreEqual(DeleteParentAction.Prevent, relDef.DeleteParentAction);
            relDef.SetDeleteParentAction(DeleteParentAction.DeleteRelated);
            Assert.AreEqual(DeleteParentAction.DeleteRelated, relDef.DeleteParentAction);
        }

        private class MultipleRelationshipDefInheritor : MultipleRelationshipDef
        {
            public MultipleRelationshipDefInheritor() : base("relName", typeof(MyRelatedBo),
                new RelKeyDef(), true, "acolumn", DeleteParentAction.Prevent) {}

            public void SetOrderBy(OrderCriteria orderCriteria)
            {
                this.OrderCriteria = orderCriteria;
            }

            public void SetDeleteParentAction(DeleteParentAction deleteParentAction)
            {
                DeleteParentAction = deleteParentAction;
            }
        }
    }
}
