using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Habanero.Generic;

namespace Habanero.Bo.ClassDefinition
{
	internal class DefClassFactory : IDefClassFactory
	{
		#region IDefClassFactory Members

		public BusinessObjectLookupListSource CreateBusinessObjectLookupListSource(string assemblyName, string className)
		{
			return new BusinessObjectLookupListSource(assemblyName, className);
		}

		public ClassDef CreateClassDef(string assemblyName, string className, PrimaryKeyDef primaryKeyDef,
		                               PropDefCol propDefCol, KeyDefCol keyDefCol, RelationshipDefCol relationshipDefCol,
		                               UIDefCol uiDefCol)
		{
			return new ClassDef(assemblyName, className, primaryKeyDef, propDefCol, keyDefCol, relationshipDefCol, uiDefCol);
		}

		public DatabaseLookupListSource CreateDatabaseLookupListSource(string sqlString, string assemblyName, string className)
		{
			return new DatabaseLookupListSource(sqlString, assemblyName, className);
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
		                             PropReadWriteRule readWriteRule, string databaseFieldName, string defaultValueString)
		{
			return new PropDef(propertyName, assemblyName, typeName, readWriteRule, databaseFieldName, defaultValueString);
		}

		public PropRuleDate CreatePropRuleDate(string ruleName, bool isCompulsory, DateTime? minValue, DateTime? maxValue)
		{
			return new PropRuleDate(ruleName, isCompulsory, minValue, maxValue);
		}

		public PropRuleDecimal CreatePropRuleDecimal(string ruleName, bool isCompulsory, decimal minValue, decimal maxValue)
		{
			return new PropRuleDecimal(ruleName, isCompulsory, minValue, maxValue);
		}

		public PropRuleGuid CreatePropRuleGuid(string ruleName, bool isCompulsory)
		{
			return new PropRuleGuid(ruleName, isCompulsory);
		}

		public PropRuleInteger CreatePropRuleInteger(string ruleName, bool isCompulsory, int minValue, int maxValue)
		{
			return new PropRuleInteger(ruleName, isCompulsory, minValue, maxValue);
		}

		public PropRuleString CreatePropRuleString(string ruleName, bool isCompulsory, int minLength, int maxLength,
		                                           string patternMatch, string patternMatchErrorMessage)
		{
			return new PropRuleString(ruleName, isCompulsory, minLength, maxLength, patternMatch, patternMatchErrorMessage);
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
		                                                             int minNoOfRelatedObjects, int maxNoOfRelatedObjects,
		                                                             DeleteParentAction deleteParentAction)
		{
			return
				new MultipleRelationshipDef(relationshipName, relatedAssemblyName, relatedClassName, relKeyDef,
				                            keepReferenceToRelatedObject, orderBy, minNoOfRelatedObjects, maxNoOfRelatedObjects,
				                            deleteParentAction);
		}

		public SimpleLookupListSource CreateSimpleLookupListSource(StringGuidPairCollection stringGuidPairCollection)
		{
			return new SimpleLookupListSource(stringGuidPairCollection);
		}

		public SuperClassDesc CreateSuperClassDesc(string assemblyName, string className, ORMapping orMapping)
		{
			return new SuperClassDesc(assemblyName, className, orMapping);
		}

		public UIDef CreateUIDef(string name, UIFormDef uiFormDef, UIGridDef uiGridDef)
		{
			return new UIDef(name, uiFormDef, uiGridDef);
		}

		public UIFormColumn CreateUIFormColumn()
		{
			return new UIFormColumn();
		}

		public UIFormDef CreateUIFormDef()
		{
			return new UIFormDef();
		}

		public UIFormGrid CreateUIFormGrid(string relationshipName, Type gridType, string correspondingRelationshipName)
		{
			return new UIFormGrid(relationshipName, gridType, correspondingRelationshipName);
		}

		public UIFormProperty CreateUIFormProperty(string label, string propertyName, Type controlType, string mapperTypeName,
		                                           bool isReadOnly, Hashtable propertyAttributes)
		{
			return new UIFormProperty(label, propertyName, controlType, mapperTypeName, isReadOnly, propertyAttributes);
		}

		public UIFormTab CreateUIFormTab()
		{
			return new UIFormTab();
		}

		public UIGridDef CreateUIGridDef()
		{
			return new UIGridDef();
		}

		public UIGridProperty CreateUIGridProperty(string heading, string propertyName, Type gridControlType, bool isReadOnly,
		                                           int width, UIGridProperty.PropAlignment alignment)
		{
			return new UIGridProperty(heading, propertyName, gridControlType, isReadOnly, width, alignment);
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

		#endregion
	}
}