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
    public class TestUIDefCol
    {
        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestAddDuplicateNameException()
        {
            UIDef uiDef1 = new UIDef("defname", null, null);
            UIDef uiDef2 = new UIDef("defname", null, null);
            UIDefCol col = new UIDefCol();
            col.Add(uiDef1);
            col.Add(uiDef2);
        }

        [Test]
        public void TestContains()
        {
            UIDef uiDef = new UIDef("defname", null, null);
            UIDefCol col = new UIDefCol();

            Assert.IsFalse(col.Contains(uiDef));
            col.Add(uiDef);
            Assert.IsTrue(col.Contains(uiDef));
        }

        [Test]
        public void TestRemove()
        {
            UIDef uiDef = new UIDef("defname", null, null);
            UIDefCol col = new UIDefCol();
            col.Add(uiDef);

            Assert.IsTrue(col.Contains(uiDef));
            col.Remove(uiDef);
            Assert.IsFalse(col.Contains(uiDef));
        }

        [Test, ExpectedException(typeof(HabaneroApplicationException))]
        public void TestDefaultUIDefMissingException()
        {
            UIDefCol col = new UIDefCol();
            UIDef uiDef = col["default"];
        }

        [Test, ExpectedException(typeof(HabaneroApplicationException))]
        public void TestOtherUIDefMissingException()
        {
            UIDefCol col = new UIDefCol();
            UIDef uiDef = col["otherdef"];
        }

        [Test]
        public void TestEnumerator()
        {
            UIDef uiDef1 = new UIDef("defname1", null, null);
            UIDef uiDef2 = new UIDef("defname2", null, null);
            UIDefCol col = new UIDefCol();
            col.Add(uiDef1);
            col.Add(uiDef2);

            int count = 0;
            foreach (object def in col)
            {
                count++;
            }
            Assert.AreEqual(2, count);
        }
    }

}
