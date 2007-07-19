using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.Bo.ClassDefinition
{
    /// <summary>
    /// An interface to model a factory that produces business object related
    /// classes
    /// </summary>
	public interface IDefClassFactory
	{
		BusinessObjectLookupList CreateBusinessObjectLookupList(string assemblyName, string className, string criteria);

		ClassDef CreateClassDef(string assemblyName, string className, PrimaryKeyDef primaryKeyDef, PropDefCol propDefCol, KeyDefCol keyDefCol, RelationshipDefCol relationshipDefCol, UIDefCol uiDefCol);

        DatabaseLookupList CreateDatabaseLookupList(string sqlString, int timeout, string assemblyName, string className);
		
		KeyDef CreateKeyDef(string keyName);
		
		PrimaryKeyDef CreatePrimaryKeyDef();

		PropDef CreatePropDef(string propertyName, string assemblyName, string typeName, PropReadWriteRule readWriteRule, string databaseFieldName, string defaultValueString, bool compulsory);

		PropRuleDate CreatePropRuleDate(string name, string message);

		PropRuleDecimal CreatePropRuleDecimal(string name, string message);

		PropRuleInteger CreatePropRuleInteger(string name, string message);

		PropRuleString CreatePropRuleString(string name, string message);

		SingleRelationshipDef CreateSingleRelationshipDef(string relationshipName, string relatedAssemblyName, string relatedClassName, RelKeyDef relKeyDef, bool keepReferenceToRelatedObject);

		MultipleRelationshipDef CreateMultipleRelationshipDef(string relationshipName, string relatedAssemblyName, string relatedClassName, RelKeyDef relKeyDef, bool keepReferenceToRelatedObject, string orderBy, DeleteParentAction deleteParentAction);

		SimpleLookupList CreateSimpleLookupList(Dictionary<string, object> displayValueDictionary);

		SuperClassDef CreateSuperClassDef(string assemblyName, string className, ORMapping orMapping);

		UIDef CreateUIDef(string name, UIFormDef uiFormDef, UIGridDef uiGridDef);

		UIFormColumn CreateUIFormColumn();

		UIFormDef CreateUIFormDef();

		UIFormGrid CreateUIFormGrid(string relationshipName, Type gridType, string correspondingRelationshipName);

		UIFormProperty CreateUIFormProperty(string label, string propertyName, Type controlType, string mapperTypeName, string mapperAssembly, bool editable, System.Collections.Hashtable propertyAttributes);

		UIFormTab CreateUIFormTab();

		UIGridDef CreateUIGridDef();

		UIGridProperty CreateUIGridProperty(string heading, string propertyName, Type gridControlType, bool editable, int width, UIGridProperty.PropAlignment alignment);

		PropDefCol CreatePropDefCol();

		KeyDefCol CreateKeyDefCol();

		UIDefCol CreateUIDefCol();

		RelationshipDefCol CreateRelationshipDefCol();

		RelPropDef CreateRelPropDef(PropDef propDef, string relPropName);

		RelKeyDef CreateRelKeyDef();

		ClassDefCol CreateClassDefCol();
	}
}
