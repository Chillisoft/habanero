//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base.Exceptions;
using Habanero.BO;
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

            Assert.AreEqual("acolumn", relDef.OrderBy);
            relDef.SetOrderBy("somecolumn");
            Assert.AreEqual("somecolumn", relDef.OrderBy);

            Assert.AreEqual(DeleteParentAction.Prevent, relDef.DeleteParentAction);
            relDef.SetDeleteParentAction(DeleteParentAction.DeleteRelated);
            Assert.AreEqual(DeleteParentAction.DeleteRelated, relDef.DeleteParentAction);
        }

        private class MultipleRelationshipDefInheritor : MultipleRelationshipDef
        {
            public MultipleRelationshipDefInheritor() : base("relName", typeof(MyRelatedBo),
                new RelKeyDef(), true, "acolumn", Habanero.BO.ClassDefinition.DeleteParentAction.Prevent) {}

            public void SetOrderBy(string orderBy)
            {
                OrderBy = orderBy;
            }

            public void SetDeleteParentAction(DeleteParentAction deleteParentAction)
            {
                DeleteParentAction = deleteParentAction;
            }
        }
    }
}
