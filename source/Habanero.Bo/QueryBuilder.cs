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
using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

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
                if (propDef.Persistable)
                {
                    selectQuery.Fields.Add(propDef.PropertyName,
                                           new QueryField(propDef.PropertyName, propDef.DatabaseFieldName,
                                                          new Source(classDef.GetTableName(propDef))));
                }
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
                ISuperClassDef superClassDef = thisClassDef.SuperClassDef;
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
        public static IOrderCriteria CreateOrderCriteria(IClassDef classDef, string orderByString)
        {
            if (classDef == null) throw new ArgumentNullException("classDef");
            IOrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria = orderCriteria.FromString(orderByString);
            try
            {
                //TODO Mark 20 Mar 2009: Souldn't the following code be stripped out into a PrepareOrderBy method that is called before loading? (Similar to PrepareCriteria)
                foreach (OrderCriteriaField field in orderCriteria.Fields)
                {
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
            catch (InvalidPropertyNameException)
            {
                throw new InvalidOrderCriteriaException("The orderByString '" + orderByString 
                        + "' is not valid for the classDef '" + classDef.ClassNameFull);
            }
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
            if (classDef == null) throw new ArgumentNullException("classDef");
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
                    if (criteria.CanBeParametrised() && (criteria.ComparisonOperator != Criteria.ComparisonOp.In && criteria.ComparisonOperator != Criteria.ComparisonOp.NotIn))
                    {
                        object returnedValue;
                        propDef.TryParsePropValue(criteria.FieldValue, out returnedValue );
                        criteria.FieldValue = returnedValue;
                    }
                }
            }
        }

        ///<summary>
        /// Uses the Class Definition to add the correct table name to the Source.
        ///</summary>
        ///<param name="classDef"></param>
        ///<param name="source"></param>
        public static void PrepareSource(IClassDef classDef, ref Source source)
        {
            IClassDef relatedClassDef;
            PrepareSource(classDef, ref source, out relatedClassDef);
        }

        private static void PrepareSource(IClassDef classDef, ref Source source, out IClassDef relatedClassDef)
        {
            if (classDef == null) throw new ArgumentNullException("classDef");
            relatedClassDef = null;
            if (source != null && source.IsPrepared) return;
            Source rootSource = new Source(classDef.ClassNameExcludingTypeParameter, classDef.GetTableName());
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
                Source.Join join = new Source.Join(rootSource, source, Source.JoinType.LeftJoin);
                rootSource.Joins.Add(join);
                Source currentSource = rootSource;
                PrepareSourceTree(currentSource, ref currentClassDef);
                relatedClassDef = currentClassDef;
                source = rootSource;
            }
            source.IsPrepared = true;
        }

        private static void CreateInheritanceJoins(IClassDef classDef, Source rootSource)
        {
            ClassDef currentClassDef = (ClassDef) classDef;
            while (currentClassDef.IsUsingClassTableInheritance())
            {
                ClassDef superClassDef = currentClassDef.SuperClassClassDef;
                Source baseSource = new Source(superClassDef.ClassNameExcludingTypeParameter, superClassDef.TableName);
                Source.Join join = new Source.Join(rootSource, baseSource);
                PrimaryKeyDef superClassPrimaryKeyDef = (PrimaryKeyDef) superClassDef.PrimaryKeyDef;
                IPropDef basePrimaryKeyPropDef = superClassPrimaryKeyDef[0];
                PrimaryKeyDef currentPrimaryKeyDef = (PrimaryKeyDef) currentClassDef.PrimaryKeyDef;
                if (currentPrimaryKeyDef != null)
                {
                    IPropDef thisPrimaryKeyPropDef = currentPrimaryKeyDef[0];
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
                    IRelationshipDef relationshipDef = currentClassDef.GetRelationship(relationshipName);
                    if (relationshipDef == null)
                    {
                        string message = string.Format("'{0}' does not have a relationship called '{1}'.",
                                                       currentClassDef.ClassName, relationshipName);
                        throw new RelationshipNotFoundException(message);
                    }
                    foreach (RelPropDef relPropDef in relationshipDef.RelKeyDef)
                    {
                        string ownerFieldName = currentClassDef.GetPropDef(relPropDef.OwnerPropertyName).DatabaseFieldName;
                        string relatedFieldName = 
                            relationshipDef.RelatedObjectClassDef.GetPropDef(relPropDef.RelatedClassPropName).DatabaseFieldName;
                        QueryField fromField = new QueryField(relPropDef.OwnerPropertyName, ownerFieldName, currentSource);
                        QueryField toField = new QueryField(relPropDef.RelatedClassPropName, relatedFieldName, childSource);
                        currentSource.Joins[0].JoinFields.Add(new Source.Join.JoinField(fromField, toField));
                    }
                    currentClassDef = (ClassDef) relationshipDef.RelatedObjectClassDef;
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
                criteria.Field.Source = new Source(((ClassDef)classDef).GetBaseClassOfSingleTableHierarchy().ClassNameExcludingTypeParameter,
                                                   classDef.GetTableName());
            }
        }

        ///<summary>
        /// Creates a select query to return the count of objects in a table for that classdef
        ///</summary>
        ///<param name="classDef"></param>
        ///<returns></returns>
        public static ISelectQuery CreateSelectCountQuery(IClassDef classDef)
        {
            return CreateSelectCountQuery(classDef, null);

        }
        ///<summary>
        /// Creates a select query to return the count of objects in a table for that classdef with the criteria
        ///</summary>
        ///<param name="classDef">The class def for the class that the count is being returned</param>
        ///<param name="criteria">The Criteria for the class that the count is being returned</param>
        ///<returns></returns>
        ///<exception cref="ArgumentNullException"></exception>
        public static ISelectQuery CreateSelectCountQuery(IClassDef classDef, Criteria criteria)
        {
            if (classDef == null) throw new ArgumentNullException("classDef");
            ISelectQuery selectQuery = CreateSelectQuery(classDef,criteria);
            selectQuery.Fields.Clear();

            selectQuery.Fields.Add("count", QueryField.FromString("Count(*)"));
            return selectQuery;
        }
    }
}
