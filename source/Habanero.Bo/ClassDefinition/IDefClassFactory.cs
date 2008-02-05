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
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// An interface to model a factory that produces business object related
    /// classes
    /// </summary>
	public interface IDefClassFactory
	{
		BusinessObjectLookupList CreateBusinessObjectLookupList(string assemblyName, string className, string criteria, string sort);

		ClassDef CreateClassDef(string assemblyName, string className, PrimaryKeyDef primaryKeyDef, PropDefCol propDefCol, KeyDefCol keyDefCol, RelationshipDefCol relationshipDefCol, UIDefCol uiDefCol);

        DatabaseLookupList CreateDatabaseLookupList(string sqlString, int timeout, string assemblyName, string className);
		
		KeyDef CreateKeyDef(string keyName);
		
		PrimaryKeyDef CreatePrimaryKeyDef();

		PropDef CreatePropDef(string propertyName, string assemblyName, string typeName, PropReadWriteRule readWriteRule, string databaseFieldName, string defaultValueString, bool compulsory, bool autoIncrementing, int length);

		PropRuleDate CreatePropRuleDate(string name, string message);

		PropRuleDecimal CreatePropRuleDecimal(string name, string message);

		PropRuleInteger CreatePropRuleInteger(string name, string message);

		PropRuleString CreatePropRuleString(string name, string message);

		SingleRelationshipDef CreateSingleRelationshipDef(string relationshipName, string relatedAssemblyName, string relatedClassName, RelKeyDef relKeyDef, bool keepReferenceToRelatedObject);

		MultipleRelationshipDef CreateMultipleRelationshipDef(string relationshipName, string relatedAssemblyName, string relatedClassName, RelKeyDef relKeyDef, bool keepReferenceToRelatedObject, string orderBy, DeleteParentAction deleteParentAction);

		SimpleLookupList CreateSimpleLookupList(Dictionary<string, object> displayValueDictionary);

		SuperClassDef CreateSuperClassDef(string assemblyName, string className, ORMapping orMapping, string id, string discriminator);

		UIDef CreateUIDef(string name, UIForm uiForm, UIGrid uiGrid);

		UIFormColumn CreateUIFormColumn();

		UIForm CreateUIFormDef();

		UIFormGrid CreateUIFormGrid(string relationshipName, Type gridType, string correspondingRelationshipName);

		UIFormField CreateUIFormProperty(string label, string propertyName, string controlTypeName, string controlAssembly, string mapperTypeName, string mapperAssembly, bool editable, System.Collections.Hashtable propertyAttributes);
//		UIFormField CreateUIFormProperty(string label, string propertyName, Type controlType, string mapperTypeName, string mapperAssembly, bool editable, System.Collections.Hashtable propertyAttributes);

		UIFormTab CreateUIFormTab();

		UIGrid CreateUIGridDef();

        UIGridColumn CreateUIGridProperty(string heading, string propertyName, Type gridControlType, bool editable, int width, UIGridColumn.PropAlignment alignment, System.Collections.Hashtable propertyAttributes);

		PropDefCol CreatePropDefCol();

		KeyDefCol CreateKeyDefCol();

		UIDefCol CreateUIDefCol();

		RelationshipDefCol CreateRelationshipDefCol();

		RelPropDef CreateRelPropDef(PropDef propDef, string relPropName);

		RelKeyDef CreateRelKeyDef();

		ClassDefCol CreateClassDefCol();
	}
}
