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
using System.Collections.Generic;
using System.Text;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.General
{
    [TestFixture]
    public class TestInheritanceOverriding
    {

        [Test]
        public void TestLoadFromClasDef()
        {
            //-------------Setup Test Pack ------------------

            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------

            //-------------Test Result ----------------------
        }

        //[Test]
        //public void TestCircleOverridingProperty()
        //{
        //    //-------------Setup Test Pack ------------------
        //    PropDef propDefOverride = new PropDef("ShapeName", typeof(string), PropReadWriteRule.WriteNew, "OverriddenDefault");
        //    //-------------Test Pre-conditions --------------
        //    Assert.IsFalse(_classDefCircleNoPrimaryKey.PropDefcol.Contains("ShapeName"));
        //    //-------------Execute test ---------------------
        //    _classDefCircleNoPrimaryKey.PropDefcol.Add(propDefOverride);
        //    //-------------Test Result ----------------------
        //    Assert.IsTrue(_classDefCircleNoPrimaryKey.PropDefcol.Contains("ShapeName"));
        //}
    }
}
