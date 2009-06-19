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

using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestFilterDef
    {

        [Test]
        public void TestFilterPropertyDef_Constructor()
        {
            //---------------Set up test pack-------------------
            string propName = TestUtil.GetRandomString();
            string label = TestUtil.GetRandomString();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            const FilterClauseOperator opEquals = FilterClauseOperator.OpEquals;
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            FilterPropertyDef def = 
                new FilterPropertyDef(propName, label, "BoolCheckBoxFilter", "Habanero.Test.UI.Base", opEquals, parameters);
            //---------------Test Result -----------------------
            Assert.AreEqual(propName, def.PropertyName);
            Assert.AreEqual(label, def.Label);
            Assert.AreSame(parameters, def.Parameters);
            Assert.AreEqual(opEquals, def.FilterClauseOperator);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            string propName = TestUtil.GetRandomString();
            string label = TestUtil.GetRandomString();
            FilterPropertyDef def = 
                new FilterPropertyDef(propName, label, "BoolCheckBoxFilter", "Habanero.Test.UI.Base", FilterClauseOperator.OpEquals, null);

            IList<IFilterPropertyDef> defs = new List<IFilterPropertyDef> {def};

            
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            FilterDef filterDef = new FilterDef(defs);
            //---------------Test Result -----------------------
            Assert.AreSame(defs,filterDef.FilterPropertyDefs);
            Assert.AreEqual(FilterModes.Filter, filterDef.FilterMode);
            //---------------Tear Down -------------------------          
        }
    }

}
