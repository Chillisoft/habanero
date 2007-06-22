using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Generic;

namespace Habanero.Bo.ClassDefinition
{
	public interface IDefClassFactory
	{
		BusinessObjectLookupListSource CreateBusinessObjectLookupListSource(string assemblyName, string className);

		ClassDef CreateClassDef(string assemblyName, string className, PrimaryKeyDef primaryKeyDef, PropDefCol propDefCol, KeyDefCol keyDefCol, RelationshipDefCol relationshipDefCol, UIDefCol uiDefCol);
		
		DatabaseLookupListSource CreateDatabaseLookupListSource(string sqlString, string assemblyName, string className);
		
		KeyDef CreateKeyDef(string keyName);
		
		PrimaryKeyDef CreatePrimaryKeyDef();

		PropDef CreatePropDef(string propertyName, string assemblyName, string typeName, PropReadWriteRule readWriteRule, string databaseFieldName, string defaultValueString);

		PropRuleDate CreatePropRuleDate(string ruleName, bool isCompulsory, DateTime? minValue, DateTime? maxValue);

		PropRuleDecimal CreatePropRuleDecimal(string ruleName, bool isCompulsory, decimal minValue, decimal maxValue);

		PropRuleGuid CreatePropRuleGuid(string ruleName, bool isCompulsory);

		PropRuleInteger CreatePropRuleInteger(string ruleName, bool isCompulsory, int minValue, int maxValue);

		PropRuleString CreatePropRuleString(string ruleName, bool isCompulsory, int minLength, int maxLength, string patternMatch, string patternMatchErrorMessage);

		SingleRelationshipDef CreateSingleRelationshipDef(string relationshipName, string relatedAssemblyName, string relatedClassName, RelKeyDef relKeyDef, bool keepReferenceToRelatedObject);

		MultipleRelationshipDef CreateMultipleRelationshipDef(string relationshipName, string relatedAssemblyName, string relatedClassName, RelKeyDef relKeyDef, bool keepReferenceToRelatedObject, string orderBy, int minNoOfRelatedObjects, int maxNoOfRelatedObjects, DeleteParentAction deleteParentAction);

		SimpleLookupListSource CreateSimpleLookupListSource(StringGuidPairCollection stringGuidPairCollection);

		SuperClassDesc CreateSuperClassDesc(string assemblyName, string className, ORMapping orMapping);

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
	}
}
