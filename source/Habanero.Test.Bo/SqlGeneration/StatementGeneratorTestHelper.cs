using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO.ClassDefinition;

namespace Habanero.Test.BO.SqlGeneration
{
    class StatementGeneratorTestHelper
    {
        internal static MockBO CreateMockBOWithExtraNonPersistableProp(string newPropName)
        {
            ClassDef newClassDef = new MockBO().ClassDef.Clone();
            PropDef def = new PropDef(newPropName, typeof(string), PropReadWriteRule.ReadWrite, "");
            def.Persistable = false;
            newClassDef.PropDefcol.Add(def);
            return new MockBO(newClassDef);
        }
    }
}
