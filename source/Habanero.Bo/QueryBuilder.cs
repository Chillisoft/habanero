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
            ISelectQuery selectQuery = new SelectQuery();
            foreach (IPropDef propDef in classDef.PropDefColIncludingInheritance)
            {
                selectQuery.Fields.Add(propDef.PropertyName,
                                       new QueryField(propDef.PropertyName, propDef.DatabaseFieldName,
                                                      new Source(classDef.GetTableName(propDef))));
            }
            AddDiscriminatorFields(selectQuery, classDef);
            selectQuery.Source = new Source(classDef.ClassName, classDef.GetTableName());
            selectQuery.Criteria = criteria;
            selectQuery.ClassDef = classDef;
            return selectQuery;
        }

        private static void AddDiscriminatorFields(ISelectQuery selectQuery, IClassDef classDef)
        {
            foreach (ClassDef thisClassDef in ((ClassDef) classDef).ImmediateChildren)
            {
                if (thisClassDef.IsUsingSingleTableInheritance())
                {
                    SuperClassDef superClassDef = thisClassDef.SuperClassDef;
                    string discriminator = superClassDef.Discriminator;
                    if (!String.IsNullOrEmpty(discriminator))
                    {
                        if (!selectQuery.Fields.ContainsKey(discriminator))
                        {
                            selectQuery.Fields.Add(discriminator,
                                                   new QueryField(discriminator, discriminator, new Source(classDef.GetTableName())));
                        }
                    }
                }
            }
        }

        ///<summary>
        /// Based on the class definition and the orderByString an OrderCriteria object is created.
        /// The orderCriteria object is a set of order by fields including information on their 
        /// business object properties and their dataSource. <see cref="OrderCriteria"/>
        ///</summary>
        ///<param name="classDef">The class definition to use for building the order criteria</param>
        ///<param name="orderByString">The orderby string to use for creating the OrderCriteria.</param>
        ///<returns>the newly created OrderCriteria object.</returns>
        public static OrderCriteria CreateOrderCriteria(ClassDef classDef, string orderByString)
        {
            OrderCriteria orderCriteria = OrderCriteria.FromString(orderByString);
            foreach (OrderCriteria.Field field in orderCriteria.Fields)
            {

                IPropDef propDef = classDef.GetPropDef(field.Source, field.PropertyName, true);
                //if (field.Source == null)
                //{
                //    field.Source = new Source(classDef.ClassName, classDef.GetTableName());
                //}
                field.FieldName = propDef.DatabaseFieldName;
         
                Source currentSource = field.Source;
                field.Source = new Source(classDef.ClassName, classDef.GetTableName());
                if (currentSource == null) continue;
                field.Source.Joins.Add(new Source.Join(field.Source, currentSource));
                currentSource = field.Source;
                ClassDef currentClassDef = classDef;
                while (currentSource != null) 
                {
                    Source childSource = currentSource.ChildSource;
                    currentSource.EntityName = currentClassDef.GetTableName();
                    if (childSource != null)
                    {
                        RelationshipDef relationshipDef = currentClassDef.GetRelationship(childSource.Name);
                        foreach (RelPropDef relPropDef in relationshipDef.RelKeyDef)
                        {
                            string ownerFieldName =
                                currentClassDef.GetPropDef(relPropDef.OwnerPropertyName).DatabaseFieldName;
                            string relatedFieldName =
                                relationshipDef.RelatedObjectClassDef.GetPropDef(relPropDef.RelatedClassPropName).DatabaseFieldName;
                            QueryField fromField = new QueryField(relPropDef.OwnerPropertyName, ownerFieldName, currentSource);
                            QueryField toField = new QueryField(relPropDef.RelatedClassPropName, relatedFieldName, childSource);
                            currentSource.Joins[0].JoinFields.Add(new Source.Join.JoinField(fromField, toField ));
                        }
                        currentClassDef = relationshipDef.RelatedObjectClassDef;
                    }

                    currentSource = childSource;
                }
            }
            return orderCriteria;
        }
    }
}