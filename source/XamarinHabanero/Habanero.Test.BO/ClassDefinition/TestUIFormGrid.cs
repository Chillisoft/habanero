#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Windows.Forms;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestUIFormGrid
    {
        [Test]
        public void TestSetsAndGets()
        {
            UIFormGridInheritor formGrid = new UIFormGridInheritor();

            Assert.AreEqual("rel", formGrid.RelationshipName);
            formGrid.SetRelationshipName("newrel");
            Assert.AreEqual("newrel", formGrid.RelationshipName);

            Assert.AreEqual("correl", formGrid.CorrespondingRelationshipName);
            formGrid.SetCorrespondingRelationshipName("newcorrel");
            Assert.AreEqual("newcorrel", formGrid.CorrespondingRelationshipName);

            Assert.AreEqual(typeof(DataGridView), formGrid.GridType);
            formGrid.SetGridType(typeof(ComboBox));
            Assert.AreEqual(typeof(ComboBox), formGrid.GridType);
        }

        // Grants access to protected methods
        private class UIFormGridInheritor : UIFormGrid
        {
            public UIFormGridInheritor() : base("rel", typeof(DataGridView), "correl")
            {}

            public void SetRelationshipName(string name)
            {
                RelationshipName = name;
            }

            public void SetGridType(Type type)
            {
                GridType = type;
            }

            public void SetCorrespondingRelationshipName(string name)
            {
                CorrespondingRelationshipName = name;
            }
        }
    }
}