using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.BO.ClassDefinition
{
    internal class ClassDefHelper
    {
        public static PropDef GetPropDefByPropName(ClassDef classDef, string propertyName)
        {
            if (classDef == null || propertyName.IndexOf("-") != -1)
            {
                return null;
            }
            if (propertyName.IndexOf(".") != -1)
            {
                string relationshipName = propertyName.Substring(0, propertyName.IndexOf("."));
                propertyName = propertyName.Remove(0, propertyName.IndexOf(".") + 1);
                List<string> relNames = new List<string>();
                relNames.AddRange(relationshipName.Split(new string[]{"|"}, StringSplitOptions.RemoveEmptyEntries));
                RelationshipDefCol relationshipDefCol = classDef.RelationshipDefCol;
                PropDef propDef = null;
                foreach (string relName in relNames)
                {
                    if (relationshipDefCol.Contains(relName))
                    {
                        RelationshipDef relationshipDef = relationshipDefCol[relName];
                        ClassDef relatedClassDef = relationshipDef.RelatedObjectClassDef;
                        propDef = GetPropDefByPropName(relatedClassDef, propertyName);
                    }
                    if (propDef != null)
                    {
                        return propDef;
                    }
                }
                return null;
            }
            else
            {
                PropDefCol propDefCol = classDef.PropDefColIncludingInheritance;
                if (propDefCol.Contains(propertyName))
                {
                    return propDefCol[propertyName];
                } else
                {
                    return null;
                }
            }
        }
    }
}
