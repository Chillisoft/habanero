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
    public class TestPrimaryKeyDef
    {
        [Test, ExpectedException(typeof(InvalidPropertyException))]
        public void TestMultiplePropertiesForIDException()
        {
            PropDef propDef1 = new PropDef("prop1", typeof(String), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("prop2", typeof(String), PropReadWriteRule.ReadWrite, null);
            PrimaryKeyDef pkDef = new PrimaryKeyDef();
            pkDef.Add(propDef1);
            pkDef.Add(propDef2);
        }

        [Test]
        public void TestIgnoreIfNullReturnsFalse()
        {
            PrimaryKeyDef pkDef = new PrimaryKeyDef();
            Assert.IsFalse(pkDef.IgnoreIfNull);
            pkDef.IgnoreIfNull = false;
            Assert.IsFalse(pkDef.IgnoreIfNull);
        }

        [Test, ExpectedException(typeof(InvalidKeyException))]
        public void TestSettingIgnoreIfNullTrueException()
        {
            PrimaryKeyDef pkDef = new PrimaryKeyDef();
            pkDef.IgnoreIfNull = true;
        }
    }
}
