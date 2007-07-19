using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.Bo.ClassDefinition
{
    /// <summary>
    /// A factory that produces instances of business object related classes.
    /// This class is used by xml loaders that read from the class definitions.
    /// </summary>
	internal class DefClassFactory : IDefClassFactory
	{
		public BusinessObjectLookupList CreateBusinessObjectLookupList(string assemblyName, string className, string criteria)
		{
			return new BusinessObjectLookupList(assemblyName, className, criteria);
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
		                             PropReadWriteRule readWriteRule, string databaseFieldName, string defaultValueString, bool compulsory)
		{
			return new PropDef(propertyName, assemblyName, typeName, readWriteRule, databaseFieldName, defaultValueString, compulsory);
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

		public SuperClassDef CreateSuperClassDef(string assemblyName, string className, ORMapping orMapping)
		{
			return new SuperClassDef(assemblyName, className, orMapping);
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

		public UIFormField CreateUIFormProperty(string label, string propertyName, Type controlType, string mapperTypeName, string mapperAssembly,
		                                           bool editable, Hashtable propertyAttributes)
		{
			return new UIFormField(label, propertyName, controlType, mapperTypeName, mapperAssembly, editable, propertyAttributes);
		}

		public UIFormTab CreateUIFormTab()
		{
			return new UIFormTab();
		}

		public UIGrid CreateUIGridDef()
		{
			return new UIGrid();
		}

		public UIGridColumn CreateUIGridProperty(string heading, string propertyName, Type gridControlType, bool editable,
		                                           int width, UIGridColumn.PropAlignment alignment)
		{
			return new UIGridColumn(heading, propertyName, gridControlType, editable, width, alignment);
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