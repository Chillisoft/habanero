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

using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestUIFormColumn
    {
        [Test]
        public void TestRemove()
        {
            UIFormField field = new UIFormField("label", "prop", "control", null, null, null, true, null, null, null);
            UIFormColumn uiFormColumn = new UIFormColumn();
            uiFormColumn.Add(field);

            Assert.IsTrue(uiFormColumn.Contains(field));
            uiFormColumn.Remove(field);
            Assert.IsFalse(uiFormColumn.Contains(field));
        }

        [Test]
        public void TestCopyTo()
        {
            UIFormField field1 = new UIFormField("label", "prop", "control", null, null, null, true, null, null, null);
            UIFormField field2 = new UIFormField("label", "prop", "control", null, null, null, true, null, null, null);
            UIFormColumn uiFormColumn = new UIFormColumn();
            uiFormColumn.Add(field1);
            uiFormColumn.Add(field2);

            UIFormField[] target = new UIFormField[2];
            uiFormColumn.CopyTo(target, 0);
            Assert.AreEqual(field1, target[0]);
            Assert.AreEqual(field2, target[1]);
        }

        // Just gets test coverage up
        [Test]
        public void TestSync()
        {
            UIFormColumn uiFormColumn = new UIFormColumn();
            Assert.AreEqual(typeof(object), uiFormColumn.SyncRoot.GetType());
            Assert.IsFalse(uiFormColumn.IsSynchronized);
        }
    }

}
