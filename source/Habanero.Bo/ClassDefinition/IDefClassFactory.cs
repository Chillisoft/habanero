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
        ///<returns></returns>
        BusinessObjectLookupList CreateBusinessObjectLookupList(string assemblyName, string className, string criteria, string sort);

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
        ClassDef CreateClassDef(string assemblyName, string className, string displayName, PrimaryKeyDef primaryKeyDef, PropDefCol propDefCol, KeyDefCol keyDefCol, RelationshipDefCol relationshipDefCol, UIDefCol uiDefCol);

        ///<summary>
        ///</summary>
        ///<param name="displayValueDictionary"></param>
        ///<returns></returns>
        SimpleLookupList CreateSimpleLookupList(Dictionary<string, string> displayValueDictionary);
        
        ///<summary>
        ///</summary>
        ///<param name="sqlString"></param>
        ///<param name="timeout"></param>
        ///<param name="assemblyName"></param>
        ///<param name="className"></param>
        ///<param name="limitToList"></param>
        ///<returns></returns>
        DatabaseLookupList CreateDatabaseLookupList(string sqlString, int timeout, string assemblyName, string className, bool limitToList);

        ///<summary>
        ///</summary>
        ///<param name="keyName"></param>
        ///<returns></returns>
        KeyDef CreateKeyDef(string keyName);

        ///<summary>
        ///</summary>
        ///<returns></returns>
        PrimaryKeyDef CreatePrimaryKeyDef();

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
        PropDef CreatePropDef(string propertyName, string assemblyName, string typeName, PropReadWriteRule readWriteRule, string databaseFieldName, string defaultValueString, bool compulsory, bool autoIncrementing, int length, string displayName, string description, bool keepValuePrivate);

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        PropRuleDate CreatePropRuleDate(string name, string message);

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        PropRuleDecimal CreatePropRuleDecimal(string name, string message);

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        PropRuleInteger CreatePropRuleInteger(string name, string message);

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        PropRuleString CreatePropRuleString(string name, string message);

        ///<summary>
        ///</summary>
        ///<param name="relationshipName"></param>
        ///<param name="relatedAssemblyName"></param>
        ///<param name="relatedClassName"></param>
        ///<param name="relKeyDef"></param>
        ///<param name="keepReferenceToRelatedObject"></param>
        ///<param name="deleteParentAction"></param>
        ///<param name="relationshipType"></param>
        ///<returns></returns>
        SingleRelationshipDef CreateSingleRelationshipDef(string relationshipName, string relatedAssemblyName, string relatedClassName, RelKeyDef relKeyDef, bool keepReferenceToRelatedObject, DeleteParentAction deleteParentAction, RelationshipType relationshipType);

        ///<summary>
        ///</summary>
        ///<param name="relationshipName"></param>
        ///<param name="relatedAssemblyName"></param>
        ///<param name="relatedClassName"></param>
        ///<param name="relKeyDef"></param>
        ///<param name="keepReferenceToRelatedObject"></param>
        ///<param name="orderBy"></param>
        ///<param name="deleteParentAction"></param>
        ///<param name="relationshipType"></param>
        ///<returns></returns>
        MultipleRelationshipDef CreateMultipleRelationshipDef(string relationshipName, string relatedAssemblyName, string relatedClassName, RelKeyDef relKeyDef, bool keepReferenceToRelatedObject, string orderBy, DeleteParentAction deleteParentAction, RelationshipType relationshipType);

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
        UIDef CreateUIDef(string name, UIForm uiForm, UIGrid uiGrid);

        ///<summary>
        ///</summary>
        ///<returns></returns>
        UIFormColumn CreateUIFormColumn();

        ///<summary>
        ///</summary>
        ///<returns></returns>
        UIForm CreateUIFormDef();

        ///<summary>
        ///</summary>
        ///<param name="relationshipName"></param>
        ///<param name="gridType"></param>
        ///<param name="correspondingRelationshipName"></param>
        ///<returns></returns>
        UIFormGrid CreateUIFormGrid(string relationshipName, Type gridType, string correspondingRelationshipName);

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
        ///<param name="triggers"></param>
        ///<param name="layout"></param>
        ///<returns></returns>
        UIFormField CreateUIFormProperty(string label, string propertyName, string controlTypeName, string controlAssembly, string mapperTypeName, string mapperAssembly, bool editable, string toolTipText, Hashtable propertyAttributes, TriggerCol triggers, UIFormField.LayoutStyle layout);

        ///<summary>
        ///</summary>
        ///<returns></returns>
        UIFormTab CreateUIFormTab();

        ///<summary>
        ///</summary>
        ///<param name="triggeredBy"></param>
        ///<param name="target"></param>
        ///<param name="conditionValue"></param>
        ///<param name="action"></param>
        ///<param name="value"></param>
        ///<returns></returns>
        Trigger CreateTrigger(string triggeredBy, string target, string conditionValue, string action, string value);

        ///<summary>
        ///</summary>
        ///<returns></returns>
        UIGrid CreateUIGridDef();

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
        UIGridColumn CreateUIGridProperty(string heading, string propertyName, String gridControlTypeName, String gridControlAssembly, bool editable, int width, UIGridColumn.PropAlignment alignment, Hashtable propertyAttributes);

        ///<summary>
        ///</summary>
        ///<returns></returns>
        PropDefCol CreatePropDefCol();


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
        RelationshipDefCol CreateRelationshipDefCol();

        ///<summary>
        ///</summary>
        ///<param name="propDef"></param>
        ///<param name="relPropName"></param>
        ///<returns></returns>
        RelPropDef CreateRelPropDef(IPropDef propDef, string relPropName);

        ///<summary>
        ///</summary>
        ///<returns></returns>
        RelKeyDef CreateRelKeyDef();

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
        FilterPropertyDef CreateFilterPropertyDef(string propertyName, string label, string filterType, string filterTypeAssembly, FilterClauseOperator filterClauseOperator, Dictionary<string, string> parameters);

        ///<summary>
        ///</summary>
        ///<param name="filterPropertyDefs"></param>
        ///<returns></returns>
        FilterDef CreateFilterDef(IList<FilterPropertyDef> filterPropertyDefs);
	}
}
