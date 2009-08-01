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
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// An interface to model a factory that produces business object related
    /// classes
    /// </summary>
	public interface IDefClassFactory
	{
        ///<summary>
        ///</summary>
        ///<param name="assemblyName"></param>
        ///<param name="className"></param>
        ///<param name="criteria"></param>
        ///<param name="sort"></param>
        ///<param name="timeout">the timeout period in milliseconds. This is the period that the lookup list will cached (i.e will not be reloaded from the database between successive calls)</param>
        ///<returns></returns>
        ILookupList CreateBusinessObjectLookupList(string assemblyName, string className, string criteria, string sort, int timeout);

        ///<summary>
        ///</summary>
        ///<param name="assemblyName"></param>
        ///<param name="className"></param>
        ///<param name="displayName"></param>
        ///<param name="primaryKeyDef"></param>
        ///<param name="propDefCol"></param>
        ///<param name="keyDefCol"></param>
        ///<param name="relationshipDefCol"></param>
        ///<param name="uiDefCol"></param>
        ///<returns></returns>
        IClassDef CreateClassDef(string assemblyName, string className, string displayName, IPrimaryKeyDef primaryKeyDef, IPropDefCol propDefCol, KeyDefCol keyDefCol, IRelationshipDefCol relationshipDefCol, UIDefCol uiDefCol);

        ///<summary>
        ///</summary>
        ///<param name="displayValueDictionary"></param>
        ///<returns></returns>
        ILookupList CreateSimpleLookupList(Dictionary<string, string> displayValueDictionary);
        
        ///<summary>
        ///</summary>
        ///<param name="sqlString"></param>
        ///<param name="timeout"></param>
        ///<param name="assemblyName"></param>
        ///<param name="className"></param>
        ///<param name="limitToList"></param>
        ///<returns></returns>
        ILookupList CreateDatabaseLookupList(string sqlString, int timeout, string assemblyName, string className, bool limitToList);

        ///<summary>
        ///</summary>
        ///<param name="keyName"></param>
        ///<returns></returns>
        IKeyDef CreateKeyDef(string keyName);

        ///<summary>
        ///</summary>
        ///<returns></returns>
        IPrimaryKeyDef CreatePrimaryKeyDef();

        ///<summary>
        ///</summary>
        ///<param name="propertyName"></param>
        ///<param name="assemblyName"></param>
        ///<param name="typeName"></param>
        ///<param name="readWriteRule"></param>
        ///<param name="databaseFieldName"></param>
        ///<param name="defaultValueString"></param>
        ///<param name="compulsory"></param>
        ///<param name="autoIncrementing"></param>
        ///<param name="length"></param>
        ///<param name="displayName"></param>
        ///<param name="description"></param>
        ///<param name="keepValuePrivate"></param>
        ///<returns></returns>
        IPropDef CreatePropDef(string propertyName, string assemblyName, string typeName, PropReadWriteRule readWriteRule, string databaseFieldName, string defaultValueString, bool compulsory, bool autoIncrementing, int length, string displayName, string description, bool keepValuePrivate);

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        IPropRule CreatePropRuleDate(string name, string message);

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        IPropRule CreatePropRuleDecimal(string name, string message);

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        IPropRule CreatePropRuleInteger(string name, string message);

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        IPropRule CreatePropRuleString(string name, string message);

        ///<summary>
        /// Creates a Single Relationship Definitions <see cref="SingleRelationshipDef"/>
        ///</summary>
        ///<param name="relationshipName"></param>
        ///<param name="relatedAssemblyName"></param>
        ///<param name="relatedClassName"></param>
        ///<param name="relKeyDef"></param>
        ///<param name="keepReferenceToRelatedObject"></param>
        ///<param name="deleteParentAction"><see cref="DeleteParentAction"/></param>
        ///<param name="insertParentAction"><see cref="InsertParentAction"/></param>
        ///<param name="relationshipType"><see cref="RelationshipType"/></param>
        ///<returns></returns>
        IRelationshipDef CreateSingleRelationshipDef(string relationshipName, string relatedAssemblyName, string relatedClassName, IRelKeyDef relKeyDef, bool keepReferenceToRelatedObject, DeleteParentAction deleteParentAction, InsertParentAction insertParentAction, RelationshipType relationshipType);

        ///<summary>
        ///</summary>
        ///<param name="relationshipName"></param>
        ///<param name="relatedAssemblyName"></param>
        ///<param name="relatedClassName"></param>
        ///<param name="relKeyDef"></param>
        ///<param name="keepReferenceToRelatedObject"></param>
        ///<param name="orderBy"></param>
        ///<param name="deleteParentAction"></param>
        ///<param name="insertParentAction"><see cref="InsertParentAction"/></param>
        ///<param name="relationshipType"></param>
        ///<param name="timeout">The timout in milliseconds. The collection will not be automatically refreshed from the DB if the timeout has not expired</param>
        ///<returns></returns>
        IRelationshipDef CreateMultipleRelationshipDef(string relationshipName, string relatedAssemblyName, string relatedClassName, IRelKeyDef relKeyDef, bool keepReferenceToRelatedObject, string orderBy, DeleteParentAction deleteParentAction, InsertParentAction insertParentAction, RelationshipType relationshipType, int timeout);

        ///<summary>
        ///</summary>
        ///<param name="assemblyName"></param>
        ///<param name="className"></param>
        ///<param name="orMapping"></param>
        ///<param name="id"></param>
        ///<param name="discriminator"></param>
        ///<returns></returns>
        SuperClassDef CreateSuperClassDef(string assemblyName, string className, ORMapping orMapping, string id, string discriminator);

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="uiForm"></param>
        ///<param name="uiGrid"></param>
        ///<returns></returns>
        IUIDef CreateUIDef(string name, IUIForm uiForm, IUIGrid uiGrid);

        ///<summary>
        ///</summary>
        ///<returns></returns>
        IUIFormColumn CreateUIFormColumn();

        ///<summary>
        ///</summary>
        ///<returns></returns>
        IUIForm CreateUIFormDef();

        ///<summary>
        ///</summary>
        ///<param name="label"></param>
        ///<param name="propertyName"></param>
        ///<param name="controlTypeName"></param>
        ///<param name="controlAssembly"></param>
        ///<param name="mapperTypeName"></param>
        ///<param name="mapperAssembly"></param>
        ///<param name="editable"></param>
        ///<param name="toolTipText"></param>
        ///<param name="propertyAttributes"></param>
        ///<param name="layout"></param>
        ///<returns></returns>
        IUIFormField CreateUIFormProperty(string label, string propertyName, string controlTypeName, string controlAssembly, string mapperTypeName, string mapperAssembly, bool editable, string toolTipText, Hashtable propertyAttributes, LayoutStyle layout);

        ///<summary>
        ///</summary>
        ///<returns></returns>
        IUIFormTab CreateUIFormTab();

        ///<summary>
        ///</summary>
        ///<returns></returns>
        IUIGrid CreateUIGridDef();

        ///<summary>
        ///</summary>
        ///<param name="heading"></param>
        ///<param name="propertyName"></param>
        ///<param name="gridControlTypeName"></param>
        ///<param name="gridControlAssembly"></param>
        ///<param name="editable"></param>
        ///<param name="width"></param>
        ///<param name="alignment"></param>
        ///<param name="propertyAttributes"></param>
        ///<returns></returns>
        IUIGridColumn CreateUIGridProperty(string heading, string propertyName, string gridControlTypeName, string gridControlAssembly, bool editable, int width, PropAlignment alignment, Hashtable propertyAttributes);

        ///<summary>
        ///</summary>
        ///<returns></returns>
        IPropDefCol CreatePropDefCol();


        ///<summary>
        ///</summary>
        ///<returns></returns>
        KeyDefCol CreateKeyDefCol();

        ///<summary>
        ///</summary>
        ///<returns></returns>
        UIDefCol CreateUIDefCol();

        ///<summary>
        ///</summary>
        ///<returns></returns>
        IRelationshipDefCol CreateRelationshipDefCol();

        ///<summary>
        ///</summary>
        ///<param name="propDef"></param>
        ///<param name="relPropName"></param>
        ///<returns></returns>
        IRelPropDef CreateRelPropDef(IPropDef propDef, string relPropName);

        ///<summary>
        ///</summary>
        ///<returns></returns>
        IRelKeyDef CreateRelKeyDef();

        ///<summary>
        ///</summary>
        ///<returns></returns>
        ClassDefCol CreateClassDefCol();

        ///<summary>
        ///</summary>
        ///<param name="propertyName"></param>
        ///<param name="label"></param>
        ///<param name="filterType"></param>
        ///<param name="filterTypeAssembly"></param>
        ///<param name="filterClauseOperator"></param>
        ///<param name="parameters"></param>
        ///<returns></returns>
        IFilterPropertyDef CreateFilterPropertyDef(string propertyName, string label, string filterType, string filterTypeAssembly, FilterClauseOperator filterClauseOperator, Dictionary<string, string> parameters);

        ///<summary>
        ///</summary>
        ///<param name="filterPropertyDefs"></param>
        ///<returns></returns>
        IFilterDef CreateFilterDef(IList<IFilterPropertyDef> filterPropertyDefs);

        /// <summary>
        /// Creates a <see cref="PropRuleSingle"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        IPropRule CreatePropRuleSingle(string name, string message);

        /// <summary>
        /// Creates a <see cref="PropRuleDouble"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        IPropRule CreatePropRuleDouble(string name, string message);
	}
}
