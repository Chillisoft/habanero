using System;
using System.Collections.Generic;
using System.Text;
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
            string propName = TestUtil.CreateRandomString();
            string label = TestUtil.CreateRandomString();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            FilterPropertyDef def = new FilterPropertyDef(propName, label, "BoolCheckBoxFilter", "Habanero.Test.UI.Base", parameters);
            //---------------Test Result -----------------------
            Assert.AreEqual(propName, def.PropertyName);
            Assert.AreEqual(label, def.Label);
            Assert.AreSame(parameters, def.Parameters);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            string propName = TestUtil.CreateRandomString();
            string label = TestUtil.CreateRandomString();
            FilterPropertyDef def = new FilterPropertyDef(propName, label, "BoolCheckBoxFilter", "Habanero.Test.UI.Base", null);

            IList<FilterPropertyDef> defs = new List<FilterPropertyDef>() {def};

            
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            FilterDef filterDef = new FilterDef(defs);
            //---------------Test Result -----------------------
            Assert.AreSame(defs,filterDef.FilterPropertyDefs);
            //---------------Tear Down -------------------------          
        }
    }

}
