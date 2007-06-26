using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.Bo.ClassDefinition
{
	public interface IDefClassFactory
	{
		BusinessObjectLookupListSource CreateBusinessObjectLookupListSource(string assemblyName, string className);

		ClassDef CreateClassDef(string assemblyName, string className, PrimaryKeyDef primaryKeyDef, PropDefCol propDefCol, KeyDefCol keyDefCol, RelationshipDefCol relationshipDefCol, UIDefCol uiDefCol);

		DatabaseLookupListSource CreateDatabaseLookupListSource(string sqlString, string assemblyName, string className);
		
		KeyDef CreateKeyDef(string keyName);
		
		PrimaryKeyDef CreatePrimaryKeyDef();

		PropDef CreatePropDef(string propertyName, string assemblyName, string typeName, PropReadWriteRule readWriteRule, string databaseFieldName, string defaultValueString, bool compulsory);

		PropRuleDate CreatePropRuleDate(string name, string message, Dictionary<string, object> parameters);

		PropRuleDecimal CreatePropRuleDecimal(string name, string message, Dictionary<string, object> parameters);

		PropRuleInteger CreatePropRuleInteger(string name, string message, Dictionary<string, object> parameters);

		PropRuleString CreatePropRuleString(string name, string message, Dictionary<string, object> parameters);

		SingleRelationshipDef CreateSingleRelationshipDef(string relationshipName, string relatedAssemblyName, string relatedClassName, RelKeyDef relKeyDef, bool keepReferenceToRelatedObject);

		MultipleRelationshipDef CreateMultipleRelationshipDef(string relationshipName, string relatedAssemblyName, string relatedClassName, RelKeyDef relKeyDef, bool keepReferenceToRelatedObject, string orderBy, int minNoOfRelatedObjects, int maxNoOfRelatedObjects, DeleteParentAction deleteParentAction);

		SimpleLookupListSource CreateSimpleLookupListSource(StringGuidPairCollection stringGuidPairCollection);

		SuperClassDef CreateSuperClassDesc(string assemblyName, string className, ORMapping orMapping);

		UIDef CreateUIDef(string name, UIFormDef uiFormDef, UIGridDef uiGridDef);

		UIFormColumn CreateUIFormColumn();

		UIFormDef CreateUIFormDef();

		UIFormGrid CreateUIFormGrid(string relationshipName, Type gridType, string correspondingRelationshipName);

		UIFormProperty CreateUIFormProperty(string label, string propertyName, Type controlType, string mapperTypeName, bool isReadOnly, System.Collections.Hashtable propertyAttributes);

		UIFormTab CreateUIFormTab();

		UIGridDef CreateUIGridDef();

		UIGridProperty CreateUIGridProperty(string heading, string propertyName, Type gridControlType, bool isReadOnly, int width, UIGridProperty.PropAlignment alignment);

		PropDefCol CreatePropDefCol();

		KeyDefCol CreateKeyDefCol();

		UIDefCol CreateUIDefCol();

		RelationshipDefCol CreateRelationshipDefCol();

		RelPropDef CreateRelPropDef(PropDef propDef, string relPropName);

		RelKeyDef CreateRelKeyDef();

		ClassDefCol CreateClassDefCol();
	}
}
