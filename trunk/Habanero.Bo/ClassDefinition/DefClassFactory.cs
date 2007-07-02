using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

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
		                             PropReadWriteRule readWriteRule, string databaseFieldName, string defaultValueString, bool compulsory)
		{
			return new PropDef(propertyName, assemblyName, typeName, readWriteRule, databaseFieldName, defaultValueString, compulsory);
		}

		public PropRuleDate CreatePropRuleDate(string name, string message, Dictionary<string, object> parameters)
		{
			return new PropRuleDate(name, message, parameters);
		}

		public PropRuleDecimal CreatePropRuleDecimal(string name, string message, Dictionary<string, object> parameters)
		{
			return new PropRuleDecimal(name, message, parameters);
		}

		public PropRuleInteger CreatePropRuleInteger(string name, string message, Dictionary<string, object> parameters)
		{
			return new PropRuleInteger(name, message, parameters);
		}

		public PropRuleString CreatePropRuleString(string name, string message, Dictionary<string, object> parameters)
		{
			return new PropRuleString(name, message, parameters);
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

        public SimpleLookupListSource CreateSimpleLookupListSource(Dictionary<string, object> displayValueDictionary)
		{
			return new SimpleLookupListSource(displayValueDictionary);
		}

		public SuperClassDef CreateSuperClassDef(string assemblyName, string className, ORMapping orMapping)
		{
			return new SuperClassDef(assemblyName, className, orMapping);
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
		                                           bool editable, Hashtable propertyAttributes)
		{
			return new UIFormProperty(label, propertyName, controlType, mapperTypeName, editable, propertyAttributes);
		}

		public UIFormTab CreateUIFormTab()
		{
			return new UIFormTab();
		}

		public UIGridDef CreateUIGridDef()
		{
			return new UIGridDef();
		}

		public UIGridProperty CreateUIGridProperty(string heading, string propertyName, Type gridControlType, bool editable,
		                                           int width, UIGridProperty.PropAlignment alignment)
		{
			return new UIGridProperty(heading, propertyName, gridControlType, editable, width, alignment);
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

		#endregion
	}
}