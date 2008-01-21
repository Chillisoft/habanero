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
    public class TestUIDef
    {
        [Test]
        public void TestProtectedSets()
        {
            UIDefInheritor uiDef = new UIDefInheritor();

            Assert.AreEqual("uidef", uiDef.Name);
            uiDef.SetName("newuidef");
            Assert.AreEqual("newuidef", uiDef.Name);

            UIForm uiForm = new UIForm();
            Assert.IsNull(uiDef.UIForm);
            uiDef.SetUIForm(uiForm);
            Assert.AreEqual(uiForm, uiDef.UIForm);

            UIGrid uiGrid = new UIGrid();
            Assert.IsNull(uiDef.UIGrid);
            uiDef.SetUIGrid(uiGrid);
            Assert.AreEqual(uiGrid, uiDef.UIGrid);
        }

        // Grants access to protected methods
        private class UIDefInheritor : UIDef
        {
            public UIDefInheritor() : base("uidef", null, null)
            {}

            public void SetName(string name)
            {
                Name = name;
            }

            public void SetUIForm(UIForm uiForm)
            {
                UIForm = uiForm;
            }

            public void SetUIGrid(UIGrid uiGrid)
            {
                UIGrid = uiGrid;
            }
        }
    }

}
