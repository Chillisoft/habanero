//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// A factory that produces instances of business object related classes.
    /// This class is used by xml loaders that read from the class definitions.
    /// </summary>
	internal class DefClassFactory : IDefClassFactory
	{
		public BusinessObjectLookupList CreateBusinessObjectLookupList(string assemblyName, string className, string criteria, string sort)
		{
			return new BusinessObjectLookupList(assemblyName, className, criteria, sort);
		}

		public ClassDef CreateClassDef(string assemblyName, string className, PrimaryKeyDef primaryKeyDef,
		                               PropDefCol propDefCol, KeyDefCol keyDefCol, RelationshipDefCol relationshipDefCol,
		                               UIDefCol uiDefCol)
		{
			return new ClassDef(assemblyName, className, primaryKeyDef, propDefCol, keyDefCol, relationshipDefCol, uiDefCol);
		}

		public DatabaseLookupList CreateDatabaseLookupList(string sqlString, int timeout, string assemblyName, string className)
		{
			return new DatabaseLookupList(sqlString, timeout, assemblyName, className);
		}

		public KeyDef CreateKeyDef(string keyName)
		{
			return new KeyDef(keyName);
		}

		public PrimaryKeyDef CreatePrimaryKeyDef()
		{
			return new PrimaryKeyDef();
		}

        public PropDef CreatePropDef(string propertyName, string assemblyName, string typeName,
            PropReadWriteRule readWriteRule, string databaseFieldName, string defaultValueString,
            bool compulsory, bool autoIncrementing, int length, string displayName, string description, bool keepValuePrivate)
        {
            return new PropDef(propertyName, assemblyName, typeName, readWriteRule, databaseFieldName, defaultValueString, compulsory, autoIncrementing, length, displayName, description, keepValuePrivate);
        }

		public PropRuleDate CreatePropRuleDate(string name, string message)
		{
			return new PropRuleDate(name, message, null);
		}

		public PropRuleDecimal CreatePropRuleDecimal(string name, string message)
		{
			return new PropRuleDecimal(name, message, null);
		}

		public PropRuleInteger CreatePropRuleInteger(string name, string message)
		{
			return new PropRuleInteger(name, message, null);
		}

		public PropRuleString CreatePropRuleString(string name, string message)
		{
			return new PropRuleString(name, message, null);
		}

		public SingleRelationshipDef CreateSingleRelationshipDef(string relationshipName, string relatedAssemblyName,
		                                                         string relatedClassName, RelKeyDef relKeyDef,
		                                                         bool keepReferenceToRelatedObject)
		{
			return
				new SingleRelationshipDef(relationshipName, relatedAssemblyName, relatedClassName, relKeyDef,
				                          keepReferenceToRelatedObject);
		}

		public MultipleRelationshipDef CreateMultipleRelationshipDef(string relationshipName, string relatedAssemblyName,
		                                                             string relatedClassName, RelKeyDef relKeyDef,
		                                                             bool keepReferenceToRelatedObject, string orderBy,
		                                                             DeleteParentAction deleteParentAction)
		{
			return
				new MultipleRelationshipDef(relationshipName, relatedAssemblyName, relatedClassName, relKeyDef,
				                            keepReferenceToRelatedObject, orderBy, 
				                            deleteParentAction);
		}

        public SimpleLookupList CreateSimpleLookupList(Dictionary<string, object> displayValueDictionary)
		{
			return new SimpleLookupList(displayValueDictionary);
		}

        public SuperClassDef CreateSuperClassDef(string assemblyName, string className, ORMapping orMapping, string id, string discriminator)
		{
			return new SuperClassDef(assemblyName, className, orMapping, id, discriminator);
		}

		public UIDef CreateUIDef(string name, UIForm uiForm, UIGrid uiGrid)
		{
			return new UIDef(name, uiForm, uiGrid);
		}

		public UIFormColumn CreateUIFormColumn()
		{
			return new UIFormColumn();
		}

		public UIForm CreateUIFormDef()
		{
			return new UIForm();
		}

		public UIFormGrid CreateUIFormGrid(string relationshipName, Type gridType, string correspondingRelationshipName)
		{
			return new UIFormGrid(relationshipName, gridType, correspondingRelationshipName);
		}

		public UIFormField CreateUIFormProperty(string label, string propertyName, string controlTypeName, string controlAssembly, string mapperTypeName, string mapperAssembly,
                                                   bool editable, string toolTipText, Hashtable propertyAttributes, TriggerCol triggers)
		{
			return new UIFormField(label, propertyName, controlTypeName, controlAssembly,
                mapperTypeName, mapperAssembly, editable, toolTipText, propertyAttributes, triggers);
		}

		public UIFormTab CreateUIFormTab()
		{
			return new UIFormTab();
		}

        public Trigger CreateTrigger(string triggeredBy, string target, string conditionValue, string action,
                                                 string value)
        {
            return new Trigger(triggeredBy, target, conditionValue, action, value);
        }

        public UIGrid CreateUIGridDef()
		{
			return new UIGrid();
		}

		public UIGridColumn CreateUIGridProperty(string heading, string propertyName, Type gridControlType, bool editable,
                                                   int width, UIGridColumn.PropAlignment alignment, Hashtable propertyAttributes)
		{
			return new UIGridColumn(heading, propertyName, gridControlType, editable, width, alignment, propertyAttributes);
		}

		public PropDefCol CreatePropDefCol()
		{
			return new PropDefCol();
		}

		public KeyDefCol CreateKeyDefCol()
		{
			return new KeyDefCol();
		}

		public UIDefCol CreateUIDefCol()
		{
			return new UIDefCol();
		}

		public RelationshipDefCol CreateRelationshipDefCol()
		{
			return new RelationshipDefCol();
		}

		public RelPropDef CreateRelPropDef(PropDef propDef, string relPropName)
		{
			return new RelPropDef(propDef, relPropName);
		}

		public RelKeyDef CreateRelKeyDef()
		{
			return new RelKeyDef();
		}

		public ClassDefCol CreateClassDefCol()
		{
			return new ClassDefCol();
		}
	}
}