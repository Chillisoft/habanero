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

using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using System.Collections.Generic;

namespace Habanero.BO
{
    /// <summary>
    /// Provides utility methods that create SelectQuery objects given a set of information.
    /// </summary>
    public class QueryBuilder
    {
        /// <summary>
        /// Creates a SelectQuery using the given classdef without any Criteria. All information in the ClassDef will be taken into account
        /// (such as inheritance structures).
        /// </summary>
        /// <param name="classDef">The <see cref="ClassDef" /> to create the SelectQuery for.</param>
        /// <returns>A SelectQuery that can be used to load objects of the type the given ClassDef represents</returns>
        public static ISelectQuery CreateSelectQuery(IClassDef classDef)
        {
            return CreateSelectQuery(classDef, null);
        }

        /// <summary>
        /// Creates a SelectQuery using the given classdef with the given Criteria. All information in the ClassDef will be taken into account
        /// (such as inheritance structures).
        /// </summary>
        /// <param name="classDef">The <see cref="ClassDef" /> to create the SelectQuery for.</param>
        /// <param name="criteria">The criteria to be set on the SelectQuery</param>
        /// <returns>A SelectQuery that can be used to load objects of the type the given ClassDef represents</returns>
        public static ISelectQuery CreateSelectQuery(IClassDef classDef, Criteria criteria)
        {
            if (classDef == null) throw new ArgumentNullException("classDef");
            SelectQuery selectQuery = new SelectQuery();
            foreach (IPropDef propDef in classDef.PropDefColIncludingInheritance)
            {
                selectQuery.Fields.Add(propDef.PropertyName,
                                       new QueryField(propDef.PropertyName, propDef.DatabaseFieldName,
                                                      new Source(classDef.GetTableName(propDef))));
            }
            Criteria discriminatorCriteria = null;
            AddDiscriminatorFields(selectQuery, classDef, ref discriminatorCriteria);
            selectQuery.DiscriminatorCriteria = discriminatorCriteria;
            Source source = null;
            PrepareSource(classDef, ref source);
            selectQuery.Source = source;
            PrepareCriteria(classDef, criteria);
            PrepareDiscriminatorCriteria(classDef, discriminatorCriteria);
            selectQuery.Criteria = criteria;
            selectQuery.ClassDef = classDef;
            return selectQuery;
        }

        private static void AddDiscriminatorFields(ISelectQuery selectQuery, IClassDef classDef, ref Criteria criteria)
        {
            ClassDefCol classDefsToSearch = ((ClassDef)classDef).AllChildren;
            classDefsToSearch.Add((ClassDef) classDef);
            List<Criteria> discriminatorCriteriaList = new List<Criteria>();
            string discriminator = null;
            foreach (ClassDef thisClassDef in classDefsToSearch)
            {
                if (!thisClassDef.IsUsingSingleTableInheritance()) continue;
                SuperClassDef superClassDef = thisClassDef.SuperClassDef;
                discriminator = superClassDef.Discriminator;
                if (String.IsNullOrEmpty(discriminator)) continue;
                if (!selectQuery.Fields.ContainsKey(discriminator))
                {
                    selectQuery.Fields.Add(discriminator,
                                           new QueryField(discriminator, discriminator, new Source(classDef.GetTableName())));
                }
                discriminatorCriteriaList.Add(new Criteria(discriminator, Criteria.ComparisonOp.Equals, thisClassDef.ClassName));
            }

            if (discriminatorCriteriaList.Count > 0)
            {
                if (!((ClassDef)classDef).IsUsingSingleTableInheritance())
                    criteria = new Criteria(discriminator, Criteria.ComparisonOp.Is, "null");
                foreach (Criteria discCriteria in discriminatorCriteriaList)
                {
                    if (criteria == null) { criteria = discCriteria; continue; }
                    criteria = new Criteria(criteria, Criteria.LogicalOp.Or, discCriteria); 
                }
            }
        }

        ///<summary>
        /// Based on the class definition and the orderByString an <see cref="OrderCriteria"/> object is created.
        /// The orderCriteria object is a set of order by fields including information on their 
        /// business object properties and their dataSource. 
        ///</summary>
        ///<param name="classDef">The class definition to use for building the order criteria</param>
        ///<param name="orderByString">The orderby string to use for creating the <see cref="OrderCriteria"/>.</param>
        ///<returns>the newly created <see cref="OrderCriteria"/> object.</returns>
        public static OrderCriteria CreateOrderCriteria(IClassDef classDef, string orderByString)
        {
            OrderCriteria orderCriteria = OrderCriteria.FromString(orderByString);
            foreach (OrderCriteria.Field field in orderCriteria.Fields)
            {

                //IPropDef propDef = ((ClassDef)classDef).GetPropDef(field.Source, field.PropertyName, true);
                //field.FieldName = propDef.DatabaseFieldName;

                Source source = field.Source;
                IClassDef relatedClassDef;
                PrepareSource(classDef, ref source, out relatedClassDef);
                field.Source = source;

                IPropDef propDef = relatedClassDef.GetPropDef(field.PropertyName);
                field.FieldName = propDef.DatabaseFieldName;
                field.Source.ChildSourceLeaf.EntityName = relatedClassDef.GetTableName(propDef);
            }
            return orderCriteria;
        }

        ///<summary>
        /// Based on the class definition the given <see cref="Criteria"/> object is set up with the correct entity 
        /// names and field names, in preparation for using it as part of a <see cref="SelectQuery"/> that has been built using
        /// the <see cref="QueryBuilder"/> and the same <see cref="ClassDef"/>
        ///</summary>
        ///<param name="classDef">The class definition to use for preparing the <see cref="Criteria"/>.</param>
        ///<param name="criteria">The <see cref="Criteria"/> to prepare for use with a <see cref="SelectQuery"/>.</param>
        public static void PrepareCriteria(IClassDef classDef, Criteria criteria)
        {
            if (criteria == null) return;
            if (criteria.IsComposite())
            {
                PrepareCriteria(classDef, criteria.LeftCriteria);
                PrepareCriteria(classDef, criteria.RightCriteria);
            }
            else
            {
                QueryField field = criteria.Field;
                Source currentSource = field.Source;
                IClassDef propParentClassDef;
                PrepareSource(classDef, ref currentSource, out propParentClassDef);
                field.Source = currentSource;
                if (propParentClassDef != null)
                {
                    IPropDef propDef = propParentClassDef.GetPropDef(field.PropertyName);
                    field.FieldName = propDef.DatabaseFieldName;
                    field.Source.ChildSourceLeaf.EntityName = propParentClassDef.GetTableName(propDef);
                    //field.Source.EntityName = currentClassDef.GetTableName(propDef);
                    if (criteria.CanBeParametrised())
                    {
                        criteria.FieldValue = propDef.ConvertValueToPropertyType(criteria.FieldValue);
                    }
                }
            }
        }

        public static void PrepareSource(IClassDef classDef, ref Source source)
        {
            IClassDef relatedClassDef;
            PrepareSource(classDef, ref source, out relatedClassDef);
        }

        private static void PrepareSource(IClassDef classDef, ref Source source, out IClassDef relatedClassDef)
        {
            Source rootSource = new Source(classDef.ClassName, classDef.GetTableName());
            CreateInheritanceJoins(classDef, rootSource);
            if (source == null)
            {
                source = rootSource;
                relatedClassDef = classDef;
            }
            else if (source.Name == rootSource.Name)
            {
                //relatedClassDef = null;
                relatedClassDef = classDef;
                source.EntityName = rootSource.EntityName;
            }
            else
            {
                ClassDef currentClassDef = (ClassDef)classDef;
                Source.Join join = new Source.Join(rootSource, source);
                rootSource.Joins.Add(join);
                Source currentSource = rootSource;
                PrepareSourceTree(currentSource, ref currentClassDef);
                relatedClassDef = currentClassDef;
                source = rootSource;
            }
        }

        private static void CreateInheritanceJoins(IClassDef classDef, Source rootSource)
        {
            ClassDef currentClassDef = (ClassDef) classDef;
            while (currentClassDef.IsUsingClassTableInheritance())
            {
                ClassDef superClassDef = currentClassDef.SuperClassClassDef;
                Source baseSource = new Source(superClassDef.ClassName, superClassDef.TableName);
                Source.Join join = new Source.Join(rootSource, baseSource);
                IPropDef basePrimaryKeyPropDef = superClassDef.PrimaryKeyDef[0];
                if (currentClassDef.PrimaryKeyDef != null)
                {
                    IPropDef thisPrimaryKeyPropDef = currentClassDef.PrimaryKeyDef[0];
                    join.JoinFields.Add(new Source.Join.JoinField(
                                            new QueryField(thisPrimaryKeyPropDef.PropertyName,
                                                           thisPrimaryKeyPropDef.DatabaseFieldName,
                                                           rootSource),
                                            new QueryField(basePrimaryKeyPropDef.PropertyName,
                                                           basePrimaryKeyPropDef.DatabaseFieldName,
                                                           baseSource)));
                } else
                {
                    join.JoinFields.Add(new Source.Join.JoinField(
                                            new QueryField(basePrimaryKeyPropDef.PropertyName,
                                                           basePrimaryKeyPropDef.DatabaseFieldName,
                                                           rootSource),
                                            new QueryField(basePrimaryKeyPropDef.PropertyName,
                                                           basePrimaryKeyPropDef.DatabaseFieldName,
                                                           baseSource)));
                }
                rootSource.InheritanceJoins.Add(join);
                rootSource = baseSource;
                currentClassDef = superClassDef;
            }
        }

        private static void PrepareSourceTree(Source currentSource, ref ClassDef currentClassDef)
        {
            while (currentSource != null)
            {
                Source childSource = currentSource.ChildSource;
                currentSource.EntityName = currentClassDef.GetTableName();
                if (childSource != null)
                {
                    string relationshipName = childSource.Name;
                    RelationshipDef relationshipDef = currentClassDef.GetRelationship(relationshipName);
                    if (relationshipDef == null)
                    {
                        string message = string.Format("'{0}' does not have a relationship called '{1}'.",
                                                       currentClassDef.ClassName, relationshipName);
                        throw new RelationshipNotFoundException(message);
                    }
                    foreach (RelPropDef relPropDef in relationshipDef.RelKeyDef)
                    {
                        string ownerFieldName =
                            currentClassDef.GetPropDef(relPropDef.OwnerPropertyName).DatabaseFieldName;
                        string relatedFieldName =
                            relationshipDef.RelatedObjectClassDef.GetPropDef(relPropDef.RelatedClassPropName).DatabaseFieldName;
                        QueryField fromField = new QueryField(relPropDef.OwnerPropertyName, ownerFieldName, currentSource);
                        QueryField toField = new QueryField(relPropDef.RelatedClassPropName, relatedFieldName, childSource);
                        currentSource.Joins[0].JoinFields.Add(new Source.Join.JoinField(fromField, toField));
                    }
                    currentClassDef = relationshipDef.RelatedObjectClassDef;
                }

                currentSource = childSource;
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="classDef"></param>
        ///<param name="criteria"></param>
        public static void PrepareDiscriminatorCriteria(IClassDef classDef, Criteria criteria)
        {
            if (criteria == null) return;
            if (criteria.IsComposite())
            {
                PrepareDiscriminatorCriteria(classDef, criteria.LeftCriteria);
                PrepareDiscriminatorCriteria(classDef, criteria.RightCriteria);
            }
            else
            {
                criteria.Field.FieldName = criteria.Field.PropertyName;
                criteria.Field.Source = new Source(((ClassDef) classDef).GetBaseClassOfSingleTableHierarchy().ClassName,
                                                   classDef.GetTableName());
            }
        }
    }
}
