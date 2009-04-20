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
    /// A factory that produces instances of business object related classes.
    /// This class is used by xml loaders that read from the class definitions.
    /// </summary>
	internal class DefClassFactory : IDefClassFactory
	{
        ///<summary>
        ///</summary>
        ///<param name="assemblyName"></param>
        ///<param name="className"></param>
        ///<param name="criteria"></param>
        ///<param name="sort"></param>
        ///<returns></returns>
        public BusinessObjectLookupList CreateBusinessObjectLookupList(string assemblyName, string className, string criteria, string sort)
		{
			return new BusinessObjectLookupList(assemblyName, className, criteria, sort);
		}

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
        public ClassDef CreateClassDef(string assemblyName, string className, string displayName, PrimaryKeyDef primaryKeyDef,
		                               PropDefCol propDefCol, KeyDefCol keyDefCol, RelationshipDefCol relationshipDefCol,
		                               UIDefCol uiDefCol)
		{
			return new ClassDef(assemblyName, className, displayName, primaryKeyDef, propDefCol, keyDefCol, relationshipDefCol, uiDefCol);
		}

        ///<summary>
        ///</summary>
        ///<param name="sqlString"></param>
        ///<param name="timeout"></param>
        ///<param name="assemblyName"></param>
        ///<param name="className"></param>
        ///<param name="limitToList"></param>
        ///<returns></returns>
        public DatabaseLookupList CreateDatabaseLookupList(string sqlString, int timeout, string assemblyName, string className, bool limitToList)
		{
			return new DatabaseLookupList(sqlString, timeout, assemblyName, className, limitToList);
		}

        ///<summary>
        ///</summary>
        ///<param name="keyName"></param>
        ///<returns></returns>
        public KeyDef CreateKeyDef(string keyName)
		{
			return new KeyDef(keyName);
		}

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public PrimaryKeyDef CreatePrimaryKeyDef()
		{
			return new PrimaryKeyDef();
		}

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
        public PropDef CreatePropDef(string propertyName, string assemblyName, string typeName,
            PropReadWriteRule readWriteRule, string databaseFieldName, string defaultValueString,
            bool compulsory, bool autoIncrementing, int length, string displayName, string description, bool keepValuePrivate)
        {
            return new PropDef(propertyName, assemblyName, typeName, readWriteRule, databaseFieldName, defaultValueString, compulsory, autoIncrementing, length, displayName, description, keepValuePrivate);
        }

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        public PropRuleDate CreatePropRuleDate(string name, string message)
		{
			return new PropRuleDate(name, message, null);
		}

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        public PropRuleDecimal CreatePropRuleDecimal(string name, string message)
		{
			return new PropRuleDecimal(name, message, null);
		}

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        public PropRuleInteger CreatePropRuleInteger(string name, string message)
		{
			return new PropRuleInteger(name, message, null);
		}

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        public PropRuleString CreatePropRuleString(string name, string message)
		{
			return new PropRuleString(name, message, null);
		}
        /// <summary>
        /// Creates a <see cref="PropRuleSingle"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public PropRuleSingle CreatePropRuleSingle(string name, string message)
        {
            return new PropRuleSingle(name, message, null);
        }

        /// <summary>
        /// Creates a <see cref="PropRuleDouble"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public PropRuleDouble CreatePropRuleDouble(string name, string message)
        {
            return new PropRuleDouble(name, message, null);
        }

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
        public SingleRelationshipDef CreateSingleRelationshipDef
            (string relationshipName, string relatedAssemblyName, string relatedClassName, 
            RelKeyDef relKeyDef, bool keepReferenceToRelatedObject, 
            DeleteParentAction deleteParentAction, RelationshipType relationshipType)
		{
			return
				new SingleRelationshipDef(relationshipName, relatedAssemblyName, relatedClassName, relKeyDef,
                                          keepReferenceToRelatedObject, deleteParentAction, relationshipType);
		}

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
        public MultipleRelationshipDef CreateMultipleRelationshipDef
            (string relationshipName, string relatedAssemblyName, string relatedClassName, 
            RelKeyDef relKeyDef, bool keepReferenceToRelatedObject, string orderBy,
            DeleteParentAction deleteParentAction, RelationshipType relationshipType)
		{
			return
				new MultipleRelationshipDef(relationshipName, relatedAssemblyName, relatedClassName, relKeyDef,
				                            keepReferenceToRelatedObject, orderBy,
                                            deleteParentAction, relationshipType);
		}

        ///<summary>
        ///</summary>
        ///<param name="displayValueDictionary"></param>
        ///<returns></returns>
        public SimpleLookupList CreateSimpleLookupList(Dictionary<string, string> displayValueDictionary)
		{
			return new SimpleLookupList(displayValueDictionary);
		}

        ///<summary>
        ///</summary>
        ///<param name="assemblyName"></param>
        ///<param name="className"></param>
        ///<param name="orMapping"></param>
        ///<param name="id"></param>
        ///<param name="discriminator"></param>
        ///<returns></returns>
        public SuperClassDef CreateSuperClassDef(string assemblyName, string className, ORMapping orMapping, string id, string discriminator)
		{
			return new SuperClassDef(assemblyName, className, orMapping, id, discriminator);
		}

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="uiForm"></param>
        ///<param name="uiGrid"></param>
        ///<returns></returns>
        public UIDef CreateUIDef(string name, UIForm uiForm, UIGrid uiGrid)
		{
			return new UIDef(name, uiForm, uiGrid);
		}

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public UIFormColumn CreateUIFormColumn()
		{
			return new UIFormColumn();
		}

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public UIForm CreateUIFormDef()
		{
			return new UIForm();
		}

        ///<summary>
        ///</summary>
        ///<param name="relationshipName"></param>
        ///<param name="gridType"></param>
        ///<param name="correspondingRelationshipName"></param>
        ///<returns></returns>
        public UIFormGrid CreateUIFormGrid(string relationshipName, Type gridType, string correspondingRelationshipName)
		{
			return new UIFormGrid(relationshipName, gridType, correspondingRelationshipName);
		}

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
        public UIFormField CreateUIFormProperty(string label, string propertyName, string controlTypeName, string controlAssembly, string mapperTypeName, string mapperAssembly, bool editable, string toolTipText, Hashtable propertyAttributes, TriggerCol triggers, UIFormField.LayoutStyle layout)
		{
			return new UIFormField(label, propertyName, controlTypeName, controlAssembly,
                mapperTypeName, mapperAssembly, editable, toolTipText, propertyAttributes, triggers, layout);
		}

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public UIFormTab CreateUIFormTab()
		{
			return new UIFormTab();
		}

        ///<summary>
        ///</summary>
        ///<param name="triggeredBy"></param>
        ///<param name="target"></param>
        ///<param name="conditionValue"></param>
        ///<param name="action"></param>
        ///<param name="value"></param>
        ///<returns></returns>
        public Trigger CreateTrigger(string triggeredBy, string target, string conditionValue, string action,
                                                 string value)
        {
            return new Trigger(triggeredBy, target, conditionValue, action, value);
        }

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public UIGrid CreateUIGridDef()
		{
			return new UIGrid();
		}

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
        public UIGridColumn CreateUIGridProperty(string heading, string propertyName, String gridControlTypeName, String gridControlAssembly, bool editable,
                                                   int width, UIGridColumn.PropAlignment alignment, Hashtable propertyAttributes)
		{
            return new UIGridColumn(heading, propertyName, gridControlTypeName, gridControlAssembly, editable, width, alignment, propertyAttributes);
		}

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public PropDefCol CreatePropDefCol()
		{
			return new PropDefCol();
		}

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public KeyDefCol CreateKeyDefCol()
		{
			return new KeyDefCol();
		}

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public UIDefCol CreateUIDefCol()
		{
			return new UIDefCol();
		}

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public RelationshipDefCol CreateRelationshipDefCol()
		{
			return new RelationshipDefCol();
		}

        ///<summary>
        ///</summary>
        ///<param name="propDef"></param>
        ///<param name="relPropName"></param>
        ///<returns></returns>
        public RelPropDef CreateRelPropDef(IPropDef propDef, string relPropName)
		{
			return new RelPropDef(propDef, relPropName);
		}

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public RelKeyDef CreateRelKeyDef()
		{
			return new RelKeyDef();
		}

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public ClassDefCol CreateClassDefCol()
		{
			return new ClassDefCol();
		}

        ///<summary>
        /// Creates an individual <see cref="FilterPropertyDef"/> that will be used on the <see cref="FilterDef"/>
        ///</summary>
        ///<param name="propertyName">The Property Name that this <see cref="FilterPropertyDef"/> is mapped to</param>
        ///<param name="label">The Label Text that is displayed for this <see cref="FilterPropertyDef"/></param>
        ///<param name="filterType">The FilterType e.g. StringComboBoxFilter that is  to use for this <see cref="FilterPropertyDef"/></param>
        ///<param name="filterTypeAssembly">The FilterType Assembly that is  to use for this <see cref="FilterPropertyDef"/></param>
        ///<param name="filterClauseOperator">The <see cref="FilterClauseOperator"/> that is  to use for this <see cref="FilterPropertyDef"/></param>
        ///<param name="parameters"></param>
        ///<returns></returns>
        public FilterPropertyDef CreateFilterPropertyDef(string propertyName, string label, string filterType, 
                string filterTypeAssembly, FilterClauseOperator filterClauseOperator, 
                Dictionary<string, string> parameters)
        {
            return new FilterPropertyDef(propertyName, label, filterType, filterTypeAssembly, filterClauseOperator, parameters);
        }

        ///<summary>
        /// Creates a <see cref="FilterDef"/>
        ///</summary>
        ///<param name="filterPropertyDefs">The <see cref="FilterPropertyDef"/>s that are placed on the FilterDef</param>
        ///<returns>The newly created FilterDef</returns>
        public FilterDef CreateFilterDef(IList<FilterPropertyDef> filterPropertyDefs) { return new FilterDef(filterPropertyDefs);}
	}
}