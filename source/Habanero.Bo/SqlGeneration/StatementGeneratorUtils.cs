using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.SqlGeneration
{
    internal class StatementGeneratorUtils
    {
        public static string GetTableName(BusinessObject bo)
        {
            ClassDef classDefToUseForPrimaryKey = bo.GetClassDefToUseForPrimaryKey();
            return (classDefToUseForPrimaryKey.IsUsingSingleTableInheritance())
                       ? classDefToUseForPrimaryKey.SuperClassClassDef.TableName
                       : classDefToUseForPrimaryKey.TableName;
        }


    }
}
