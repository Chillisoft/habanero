// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestParameterNameGenerator : TestUsingDatabase
    {
        //	private ParameterNameGenerator gen;

        [Test]
        public void TestGetNextParameterName_FirstParamName()
        {
            //---------------Set up test pack-------------------
            ParameterNameGenerator gen = new ParameterNameGenerator("@");
        
            //---------------Execute Test ----------------------
            string paramName = gen.GetNextParameterName();

            //---------------Test Result -----------------------
            Assert.AreEqual("@Param0", paramName);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetNextParameterName_SecondParamName()
        {
            //---------------Set up test pack-------------------
            ParameterNameGenerator gen = new ParameterNameGenerator("@");
            gen.GetNextParameterName();
      
            //---------------Execute Test ----------------------
            string paramName1 = gen.GetNextParameterName();
            string paramName2 = gen.GetNextParameterName();

            //---------------Test Result -----------------------
            Assert.AreEqual("@Param1", paramName1);
            Assert.AreEqual("@Param2", paramName2);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestReset()
        {
            //---------------Set up test pack-------------------
            ParameterNameGenerator gen = new ParameterNameGenerator("@");
            gen.GetNextParameterName();
            gen.GetNextParameterName();
            string paramName = gen.GetNextParameterName();
            
            //---------------Assert PreConditions---------------       
            Assert.AreEqual("@Param2", paramName);

            //---------------Execute Test ----------------------
            gen.Reset();
            string paramNameAfterReset = gen.GetNextParameterName();

            //---------------Test Result -----------------------
            Assert.AreEqual("@Param0", paramNameAfterReset);
            //---------------Tear Down -------------------------          
        }

    }
}