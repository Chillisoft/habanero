using System;
using System.Collections.Generic;
using System.Text;
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
            FilterClauseOperator opEquals = FilterClauseOperator.OpEquals;
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

            IList<FilterPropertyDef> defs = new List<FilterPropertyDef>() {def};

            
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
