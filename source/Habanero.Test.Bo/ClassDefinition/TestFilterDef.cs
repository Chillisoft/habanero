//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.Util;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestFilterDef
    {
        [Test]
        public void Test_Constructor_WithNoParameters()
        {
            //---------------Set up test pack-------------------
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            FilterDef filterDef = new FilterDef();
            //---------------Test Result -----------------------
            Assert.IsNotNull(filterDef.FilterPropertyDefs);
            Assert.AreEqual(FilterModes.Filter, filterDef.FilterMode);    
        }

        [Test]
        public void Test_Constructor_WithListParameter()
        {
            //---------------Set up test pack-------------------
            IList<IFilterPropertyDef> filterPropertyDefs = MockRepository.GenerateStub < IList < IFilterPropertyDef >>();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            FilterDef filterDef = new FilterDef(filterPropertyDefs);
            //---------------Test Result -----------------------
            Assert.AreSame(filterPropertyDefs,filterDef.FilterPropertyDefs);
            Assert.AreEqual(FilterModes.Filter, filterDef.FilterMode);   
        }
    }

    [TestFixture]
    public class TestFilterPropertyDef
    {
        [Test]
        public void Test_Constructor_WithStringParameters()
        {
            //---------------Set up test pack-------------------
            string propName = TestUtil.GetRandomString();
            string label = TestUtil.GetRandomString();
            string filterType = TestUtil.GetRandomString();
            string filterTypeAssembly = TestUtil.GetRandomString();
            FilterClauseOperator filterClauseOperator = TestUtil.GetRandomEnum<FilterClauseOperator>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            FilterPropertyDef def = new FilterPropertyDef(propName, label, filterType, filterTypeAssembly, filterClauseOperator, parameters);
            //---------------Test Result -----------------------
            Assert.AreEqual(propName, def.PropertyName);
            Assert.AreEqual(label, def.Label);
            Assert.AreEqual(filterType, def.FilterType);
            Assert.AreEqual(filterTypeAssembly, def.FilterTypeAssembly);
            Assert.AreSame(parameters, def.Parameters);
            Assert.AreEqual(filterClauseOperator, def.FilterClauseOperator);
        }

        [Test]
        public void Test_Constructor_WithTypeParameters()
        {
            //---------------Set up test pack-------------------
            string propName = TestUtil.GetRandomString();
            string label = TestUtil.GetRandomString();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            FilterClauseOperator filterClauseOperator = TestUtil.GetRandomEnum<FilterClauseOperator>();
            Type filterType = typeof(MyFilterType);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            FilterPropertyDef def = new FilterPropertyDef(propName, label, filterType, filterClauseOperator, parameters);
            //---------------Test Result -----------------------
            Assert.AreEqual(propName, def.PropertyName);
            Assert.AreEqual(label, def.Label);
            string assemblyName;
            string classNameFull;
            TypeLoader.ClassTypeInfo(filterType, out assemblyName, out classNameFull);
            Assert.AreEqual(classNameFull, def.FilterType);
            Assert.AreEqual(assemblyName, def.FilterTypeAssembly);
            Assert.AreSame(parameters, def.Parameters);
            Assert.AreEqual(filterClauseOperator, def.FilterClauseOperator);
        }

        private class MyFilterType
        {
            
        }

    }

}
