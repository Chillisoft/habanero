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
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestPropertyDef.
    /// </summary>
    [TestFixture]
    public class TestPropDef
    {
        [
            Test,
                ExpectedException(typeof (ArgumentException),
                    "A property name cannot contain any of the following characters: [.-|]  Invalid property name This.That"
                    )]
        public void TestDotIsNotAllowedInName()
        {
            PropDef def = new PropDef("This.That", typeof (string), PropReadWriteRule.ReadWrite, "");
        }


        [
            Test,
                ExpectedException(typeof (ArgumentException),
                    "A property name cannot contain any of the following characters: [.-|]  Invalid property name This-That"
                    )]
        public void TestDashIsNotAllowedInName()
        {
            PropDef def = new PropDef("This-That", typeof (string), PropReadWriteRule.ReadWrite, "");
        }

        [
            Test,
                ExpectedException(typeof (ArgumentException),
                    "A property name cannot contain any of the following characters: [.-|]  Invalid property name This|That"
                    )]
        public void TestPipeIsNotAllowedInName()
        {
            PropDef def = new PropDef("This|That", typeof (string), PropReadWriteRule.ReadWrite, "");

           
        }
    }
}