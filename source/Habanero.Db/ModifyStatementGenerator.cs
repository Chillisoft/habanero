using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Base;

namespace Habanero.DB
{
    /// <summary>
    /// Encapsulates the common logic for deciding which props are included when generating Insert and Update Statements.
    /// </summary>
    public class ModifyStatementGenerator
    {
        /// <summary>
        /// Builds a collection of properties to include in the insertion,
        /// depending on the inheritance type.
        /// </summary>
        protected virtual IBOPropCol GetPropsToInclude(IClassDef currentClassDef)
        {
            IBOPropCol propsToIncludeTemp = currentClassDef.PropDefcol.CreateBOPropertyCol(true);

            IBOPropCol propsToInclude = new BOPropCol();

            AddPersistableProps(propsToInclude, propsToIncludeTemp);
            
            while (((ClassDef)currentClassDef).IsUsingSingleTableInheritance() ||
                   ((ClassDef)currentClassDef).IsUsingConcreteTableInheritance())
            {
                var boPropertyCol = currentClassDef.SuperClassClassDef.PropDefcol.CreateBOPropertyCol(true);
                AddPersistableProps(propsToInclude, boPropertyCol);
                currentClassDef = currentClassDef.SuperClassClassDef;
            }

            return propsToInclude;
        }

        private static void AddPersistableProps(IBOPropCol propsToInclude, IBOPropCol propsToIncludeTemp)
        {
            foreach (BOProp prop in propsToIncludeTemp)
            {
                if (prop.PropDef.Persistable) propsToInclude.Add(prop);
            }
        }
    }
}