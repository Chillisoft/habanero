#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.Loaders;

namespace Habanero.BO.ClassDefinition
{
	/// <summary>
	/// A factory that produces instances of business object related classes.
	/// This class is used by <see cref="IClassDefsLoader"/> that read from the class definitions.
	/// </summary>
	public class DefClassFactory : IDefClassFactory
	{
		///<summary>
		///</summary>
		///<param name="assemblyName"></param>
		///<param name="className"></param>
		///<param name="criteria"></param>
		///<param name="sort"></param>
		///<param name="timeout">the timeout period in milliseconds. This is the period that the lookup list will cached (i.e will not be reloaded from the database between successive calls)</param>
		///<returns></returns>
		public ILookupList CreateBusinessObjectLookupList(string assemblyName, string className, string criteria, string sort, int timeout)
		{
			return new BusinessObjectLookupList(assemblyName, className, criteria, sort, timeout);
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
		public IClassDef CreateClassDef(string assemblyName, string className, string displayName, IPrimaryKeyDef primaryKeyDef,
									   IPropDefCol propDefCol, KeyDefCol keyDefCol, IRelationshipDefCol relationshipDefCol,
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
		public ILookupList CreateDatabaseLookupList(string sqlString, int timeout, string assemblyName, string className, bool limitToList)
		{
			Type databaseLookupListType = Util.TypeLoader.LoadType("Habanero.DB", "DatabaseLookupList");
			return
				(ILookupList)
				Activator.CreateInstance(databaseLookupListType,
										 new object[] {sqlString, timeout, assemblyName, className, limitToList});
		}

		///<summary>
		///</summary>
		///<param name="keyName"></param>
		///<returns></returns>
		public IKeyDef CreateKeyDef(string keyName)
		{
			return new KeyDef(keyName);
		}

		///<summary>
		///</summary>
		///<returns></returns>
		public IPrimaryKeyDef CreatePrimaryKeyDef()
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
		public IPropDef CreatePropDef(string propertyName, string assemblyName, string typeName,
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
		public IPropRule CreatePropRuleDate(string name, string message)
		{
			return new PropRuleDate(name, message);
		}

		///<summary>
		///</summary>
		///<param name="name"></param>
		///<param name="message"></param>
		///<returns></returns>
		public IPropRule CreatePropRuleDecimal(string name, string message)
		{
			return new PropRuleDecimal(name, message);
		}

        ///<summary>
        /// Creates an IPropRule of Type Int
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        public IPropRule CreatePropRuleInteger(string name, string message)
        {
            return new PropRuleInteger(name, message);
        }
        ///<summary>
        /// Creates an IPropRule of Type Short
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        public IPropRule CreatePropRuleShort(string name, string message)
        {
            return new PropRuleShort(name, message);
        }
        ///<summary>
        /// Constructs a <see cref="PropRuleLong"/>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="message"></param>
        ///<returns></returns>
        public IPropRule CreatePropRuleLong(string name, string message)
        {
            return new PropRuleLong(name, message);
        }
		///<summary>
		///</summary>
		///<param name="name"></param>
		///<param name="message"></param>
		///<returns></returns>
		public IPropRule CreatePropRuleString(string name, string message)
		{
			return new PropRuleString(name, message);
		}
		/// <summary>
		/// Creates a <see cref="PropRuleSingle"/>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public IPropRule CreatePropRuleSingle(string name, string message)
		{
			return new PropRuleSingle(name, message);
		}

		/// <summary>
		/// Creates a <see cref="PropRuleDouble"/>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public IPropRule CreatePropRuleDouble(string name, string message)
		{
			return new PropRuleDouble(name, message);
		}

		///<summary>
		///</summary>
		///<param name="relationshipName"></param>
		///<param name="relatedAssemblyName"></param>
		///<param name="relatedClassName"></param>
		///<param name="relKeyDef"></param>
		///<param name="keepReferenceToRelatedObject"></param>
		///<param name="deleteParentAction"></param>
		///<param name="insertParentAction"><see cref="InsertParentAction"/></param>
		///<param name="relationshipType"></param>
		///<returns></returns>
		public IRelationshipDef CreateSingleRelationshipDef
			(string relationshipName, string relatedAssemblyName, string relatedClassName, IRelKeyDef relKeyDef, bool keepReferenceToRelatedObject, DeleteParentAction deleteParentAction, InsertParentAction insertParentAction, RelationshipType relationshipType)
		{
			return
				new SingleRelationshipDef(relationshipName, relatedAssemblyName, relatedClassName, relKeyDef,
										  keepReferenceToRelatedObject, deleteParentAction, insertParentAction, relationshipType);
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
		///<param name="insertParentAction"><see cref="InsertParentAction"/></param>
		///<param name="relationshipType"></param>
		///<param name="timeout">The timout in milliseconds. The collection will not be automatically refreshed from the DB if the timeout has not expired</param>
		///<returns></returns>
		public IRelationshipDef CreateMultipleRelationshipDef
			(string relationshipName, string relatedAssemblyName, string relatedClassName, IRelKeyDef relKeyDef, bool keepReferenceToRelatedObject, string orderBy, DeleteParentAction deleteParentAction, InsertParentAction insertParentAction, RelationshipType relationshipType, int timeout)
		{
			return
				new MultipleRelationshipDef(relationshipName, relatedAssemblyName, relatedClassName, relKeyDef,
											keepReferenceToRelatedObject, orderBy,
											deleteParentAction, insertParentAction, relationshipType, timeout);
		}

		///<summary>
		///</summary>
		///<param name="displayValueDictionary"></param>
		///<returns></returns>
		public ILookupList CreateSimpleLookupList(Dictionary<string, string> displayValueDictionary)
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
		public ISuperClassDef CreateSuperClassDef(string assemblyName, string className, ORMapping orMapping, string id, string discriminator)
		{
			return new SuperClassDef(assemblyName, className, orMapping, id, discriminator);
		}

		///<summary>
		///</summary>
		///<param name="name"></param>
		///<param name="uiForm"></param>
		///<param name="uiGrid"></param>
		///<returns></returns>
		public IUIDef CreateUIDef(string name, IUIForm uiForm, IUIGrid uiGrid)
		{
			return new UIDef(name, uiForm, uiGrid);
		}

		///<summary>
		///</summary>
		///<returns></returns>
		public IUIFormColumn CreateUIFormColumn()
		{
			return new UIFormColumn();
		}

		///<summary>
		///</summary>
		///<returns></returns>
		public IUIForm CreateUIFormDef()
		{
			return new UIForm();
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
		///<param name="showAsCompulsory"></param>
		///<param name="toolTipText"></param>
		///<param name="propertyAttributes"></param>
		///<param name="layout"></param>
		///<returns></returns>
		public IUIFormField CreateUIFormProperty(string label, string propertyName, string controlTypeName, string controlAssembly, string mapperTypeName, string mapperAssembly, bool editable, bool? showAsCompulsory, string toolTipText, Hashtable propertyAttributes, LayoutStyle layout)
		{
			return new UIFormField(label, propertyName, controlTypeName, controlAssembly,
				mapperTypeName, mapperAssembly, editable, showAsCompulsory, toolTipText, propertyAttributes, layout);
		}

		///<summary>
		///</summary>
		///<returns></returns>
		public IUIFormTab CreateUIFormTab()
		{
			return new UIFormTab();
		}

		///<summary>
		///</summary>
		///<returns></returns>
		public IUIGrid CreateUIGridDef()
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
		public IUIGridColumn CreateUIGridProperty(string heading, string propertyName, string gridControlTypeName, string gridControlAssembly, bool editable, int width, PropAlignment alignment, Hashtable propertyAttributes)
		{
			return new UIGridColumn(heading, propertyName, gridControlTypeName, gridControlAssembly, editable, width, alignment, propertyAttributes);
		}

		///<summary>
		///</summary>
		///<returns></returns>
		public IPropDefCol CreatePropDefCol()
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
		public IRelationshipDefCol CreateRelationshipDefCol()
		{
			return new RelationshipDefCol();
		}

		///<summary>
		///</summary>
		///<param name="propName"></param>
		///<param name="relPropName"></param>
		///<returns></returns>
		public IRelPropDef CreateRelPropDef(string propName, string relPropName)
		{
			return new RelPropDef(propName, relPropName);
		}

		///<summary>
		///</summary>
		///<returns></returns>
		public IRelKeyDef CreateRelKeyDef()
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
		public IFilterPropertyDef CreateFilterPropertyDef(string propertyName, string label, string filterType, string filterTypeAssembly, FilterClauseOperator filterClauseOperator, Dictionary<string, string> parameters)
		{
			return new FilterPropertyDef(propertyName, label, filterType, filterTypeAssembly, filterClauseOperator, parameters);
		}

		///<summary>
		/// Creates a <see cref="FilterDef"/>
		///</summary>
		///<param name="filterPropertyDefs">The <see cref="FilterPropertyDef"/>s that are placed on the FilterDef</param>
		///<returns>The newly created FilterDef</returns>
		public IFilterDef CreateFilterDef(IList<IFilterPropertyDef> filterPropertyDefs) { return new FilterDef(filterPropertyDefs);}

	}
}