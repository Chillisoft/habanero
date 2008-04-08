//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Collections.Generic;
using System.Text;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestUIForm
    {
        [Test]
        public void TestRemove()
        {
            UIFormTab tab = new UIFormTab();
            UIForm uiForm = new UIForm();
            uiForm.Add(tab);

            Assert.IsTrue(uiForm.Contains(tab));
            uiForm.Remove(tab);
            Assert.IsFalse(uiForm.Contains(tab));
        }

        [Test]
        public void TestCopyTo()
        {
            UIFormTab tab1 = new UIFormTab();
            UIFormTab tab2 = new UIFormTab();
            UIForm uiForm = new UIForm();
            uiForm.Add(tab1);
            uiForm.Add(tab2);

            UIFormTab[] target = new UIFormTab[2];
            uiForm.CopyTo(target, 0);
            Assert.AreEqual(tab1, target[0]);
            Assert.AreEqual(tab2, target[1]);
        }

        // Just gets test coverage up
        [Test]
        public void TestSync()
        {
            UIForm uiForm = new UIForm();
            Assert.AreEqual(typeof(object), uiForm.SyncRoot.GetType());
            Assert.IsFalse(uiForm.IsSynchronized);
        }
    }

}
