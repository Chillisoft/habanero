// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Base.Logging;
using Habanero.BO.ClassDefinition;
using Habanero.Util;
using log4net;

namespace Habanero.BO
{
	///<summary>
	/// An Enum used for Constructing fake Business objects.
	/// This is used for Testing using a Mocking Framework such as 
	/// Rhino Mocks. In this scenario no ClassDefs are created for the BO.
	/// This enum is created so as to make the Constructor more self documenting when called.
	///</summary>
	public enum ConstructForFakes
	{
		///<summary>
		/// This object must be constructed as a Fake BO i.e. no ClassDefs
		///</summary>
		True
	}
	/// <summary>
	/// Provides a super-class for business objects. This class contains all
	/// the common functionality used by business objects.
	/// This Class implements the Layer SuperType - Fowler (xxx)
	/// </summary>
	public class BusinessObject : IBusinessObject, ISerializable
	{
		protected static readonly IHabaneroLogger _logger = GlobalRegistry.LoggerFactory.GetLogger(typeof(BusinessObject));

		#region IBusinessObject Members

		/// <summary>
		/// Event fired when this <see cref="IBusinessObject"/> is updated.
		/// </summary>
		public event EventHandler<BOEventArgs> Updated;

		/// <summary>
		/// Event that is fired when this <see cref="IBusinessObject"/> is Saved.
		/// </summary>
		public event EventHandler<BOEventArgs> Saved;

		/// <summary>
		/// Event that is fired when this <see cref="IBusinessObject"/> is deleted.
		/// </summary>
		public event EventHandler<BOEventArgs> Deleted;

		/// <summary>
		/// Event that is fired when this <see cref="IBusinessObject"/> is Restored.
		/// </summary>
		public event EventHandler<BOEventArgs> Restored;

		/// <summary>
		/// Event that is fired when this <see cref="IBusinessObject"/> is MarkedForDeletion.
		/// </summary>
		public event EventHandler<BOEventArgs> MarkedForDeletion;

		/// <summary>
		/// Event that is fired when this <see cref="IBusinessObject"/> is PropertyUpdated.
		/// </summary>
		public event EventHandler<BOPropUpdatedEventArgs> PropertyUpdated;

		/// <summary>
		/// Event that is fired when this <see cref="IBusinessObject"/> is IDUpdated.
		/// </summary>
		public event EventHandler<BOEventArgs> IDUpdated;

		#endregion

		#region Fields

		private IBusinessObjectAuthorisation AuthorisationRules { get; set; }

		/// <summary>
		/// The Collection of Business Object Properties for this Business Object.
		/// </summary>
		protected IBOPropCol _boPropCol;

		private IList<IBusinessObjectRule> _boRules;

		private BOStatus _boStatus;

		/// <summary>
		/// The Update Log being used for this Business Object.
		/// </summary>
		private IBusinessObjectUpdateLog _businessObjectUpdateLog;

		/// <summary>
		/// The Class Definition <see cref="IClassDef"/> for this business object.
		/// </summary>
		protected IClassDef _classDef;

		/// <summary> The Concurrency Control mechanism used by this Business object </summary>
		protected IConcurrencyControl _concurrencyControl;

		/// <summary>
		/// the Collection of alternate keys used by this <see cref="IBusinessObject"/>
		/// </summary>
		protected BOKeyCol _keysCol;

		/// <summary>
		/// The Primary key for this <see cref="IBusinessObject"/>
		/// </summary>
		protected IPrimaryKey _primaryKey;

		/// <summary>
		/// The Relationships owned by this <see cref="IBusinessObject"/>.
		/// This is a lazy initialised part of the <see cref="IBusinessObject"/>
		/// </summary>
		private IRelationshipCol _relationshipCol;

		#endregion //Fields

		#region Constructors

		/// <summary>
		/// Constructor to initialise a new business object
		/// </summary>
		public BusinessObject() : this(null)
		{
		}
		// ReSharper disable DoNotCallOverridableMethodsInConstructor
		/// <summary>
		/// Constructor that specifies a class definition
		/// </summary>
		/// <param name="def">The class definition</param>
		protected internal BusinessObject(IClassDef def)
		{
			ConstructEntireClassFromClassDefs(def);
		}
		// ReSharper disable DoNotCallOverridableMethodsInConstructor
		/// <summary>
		/// Constructor that specifies a class definition
		/// </summary>
		/// <param name="constructForFakes"></param>
		// ReSharper disable UnusedParameter.Local
		protected BusinessObject(ConstructForFakes constructForFakes)
		{
			_boStatus = new BOStatus(this) { IsDeleted = false, IsEditing = false, IsNew = true };
			_boPropCol = new BOPropCol();
		}

		// ReSharper restore UnusedParameter.Local
		// ReSharper restore DoNotCallOverridableMethodsInConstructor

		protected virtual void ConstructEntireClassFromClassDefs(IClassDef def)
		{
			Initialise(def);
			SetupBOPropsWithThisBo();
			AddToObjectManager();
			RegisterForPropertyEvents();
		}

		#region Serialisation of BusinessObject

		// TODO_ - Mark 03 Feb 2009 : The only detail that is recorded off of a BOProp is the current value. Is this correct?
		//      I noticed that the prop values that have come out of a seriaizable context are all going to be the persisted values as well.
		/// <summary>
		/// Constructs an <see cref="IBusinessObject"/> from a serialised source.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected BusinessObject(SerializationInfo info, StreamingContext context)
		{
			Initialise(ClassDef.ClassDefs[GetType()]);
			foreach (BOProp prop in _boPropCol)
			{
				try
				{
					prop.InitialiseProp(info.GetValue(prop.PropertyName, prop.PropertyType));
					prop._isDirty = (bool)info.GetValue(prop.PropertyName + "_IsDirty", typeof(Boolean));
				}
				catch (SerializationException ex)
				{
					if (ex.Message.Contains("Member") && ex.Message.Contains("was not found"))
					{
						continue;
					}
					throw;
				}
				catch (Exception ex)
				{
					string message = "The Business Object " + ClassDef.ClassName
									 + " could not be deserialised because the property " + prop.PropertyName
									 + " raised an exception";
					throw new HabaneroDeveloperException(message, message, ex);
				}
			}
			_boStatus = (BOStatus) info.GetValue("Status", typeof (BOStatus));
			_boStatus.BusinessObject = this;
			SetupBOPropsWithThisBo();
			ReplaceInObjectManager();
			RegisterForPropertyEvents();
		}

		/// <summary>
		/// Gets the Objects data for the purposes of serialisation.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			foreach (IBOProp prop in _boPropCol)
			{
				info.AddValue(prop.PropertyName, prop.Value);
				info.AddValue(prop.PropertyName + "_IsDirty", prop.IsDirty);
			}
			info.AddValue("Status", Status);
		}

		#endregion // Serialisation of BusinessObject

		private void AddToObjectManager()
		{
			BORegistry.BusinessObjectManager.Add(this);
		}

		private void ReplaceInObjectManager()
		{
			BORegistry.BusinessObjectManager.AddWithReplace(this);
		}

		private void SetupBOPropsWithThisBo()
		{
			foreach (IBOProp prop in _boPropCol)
			{
				((BOProp)prop).BusinessObject = this;
			}
		}

		private void FinaliseBusinessObjectTearDown()
		{
			RemoveFromObjectManager();
			UnregisterForPropertyEvents();
		}

		private void RemoveFromObjectManager()
		{
			BORegistry.BusinessObjectManager.Remove(this);
		}

		private void RegisterForPropertyEvents()
		{
			ID.Updated += (sender, e) => FireIDUpdatedEvent();
			foreach (IBOProp boProp in Props)
			{
				boProp.Updated += BOProp_OnUpdated;
			}
		}

		private void UnregisterForPropertyEvents()
		{
			ID.Updated -= ((sender, e) => FireIDUpdatedEvent());
			foreach (IBOProp boProp in Props)
			{
				boProp.Updated -= BOProp_OnUpdated;
			}
		}

		private void BOProp_OnUpdated(object sender, BOPropEventArgs e)
		{
			FirePropertyUpdatedEvent(e.Prop);
		}

		private void InitialisePrimaryKeyPropertiesBasedOnParentClass(Guid myID)
		{
			IClassDef currentClassDef = _classDef;
			if (currentClassDef == null) return;
			while (currentClassDef.IsUsingClassTableInheritance())
			{
				while (currentClassDef.SuperClassClassDef != null
					   && currentClassDef.SuperClassClassDef.PrimaryKeyDef == null)
				{
					currentClassDef = currentClassDef.SuperClassClassDef;
				}

				if (currentClassDef.SuperClassClassDef == null) continue;
				if (currentClassDef.SuperClassClassDef.PrimaryKeyDef != null)
				{
					InitialisePropertyValue(currentClassDef.SuperClassClassDef.PrimaryKeyDef.KeyName, myID);
				}
				currentClassDef = currentClassDef.SuperClassClassDef;
			}
		}

		///<summary>
		///Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
		///</summary>
		///
		///<returns>
		///A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
		///</returns>
		///<filterpriority>2</filterpriority>
		public override string ToString()
		{
			if (HasTypeParameter() && HasAlternateKeys())
				foreach (BOKey boKeyCol in _keysCol)
				{
					return boKeyCol.ToString();
				}
			return ID.GetAsValue() == null ? this.ID.ToString() : ID.GetAsValue().ToString();
		}

		private bool HasAlternateKeys()
		{
			return _keysCol.Count > 0;
		}

		private bool HasTypeParameter()
		{
			return !String.IsNullOrEmpty(ClassDef.TypeParameter);
		}

		/// <summary>
		/// A destructor for the object
		/// </summary>
		~BusinessObject()
		{
			try
			{
				if (ClassDef == null) return;
				if (ID != null)
				{
					//BusinessObjectManager.Instance.Remove(this);
					FinaliseBusinessObjectTearDown();
				}
			}
			catch (Exception ex)
			{
				_logger.Log("Error disposing BusinessObject.", ex);
				//_log.Error("Error disposing BusinessObject.", ex);
			}
			finally
			{
				try
				{
					ReleaseWriteLocks();
				}
				catch (Exception e)
				{
					_logger.Log("it is potentially dangerous to access ADO connections in an object's destructor. But the release locks is really a but if when maybe type situation so we can live with this.", e, LogCategory.Debug);
				}            
			}
		}

		private void Initialise(IClassDef classDef)
		{
			_boStatus = new BOStatus(this) {IsDeleted = false, IsEditing = false, IsNew = true};
			if (classDef == null)
			{
				try { _classDef = ClassDef.ClassDefs[GetType()]; }
				catch (Exception) {
					_classDef = null;  }
			}
			else _classDef = classDef;
			ConstructFromClassDef(true);
			Guid myID = Guid.NewGuid();
			if (_primaryKey != null)
			{
				_primaryKey.SetObjectGuidID(myID);
			}
			InitialisePrimaryKeyPropertiesBasedOnParentClass(myID);

			if (_classDef == null)
			{
				throw new HabaneroDeveloperException
					("There is an error constructing a business object. Please refer to the system administrator"
					+ "The Class could not be constructed since no classdef could be loaded",
					 "The Class could not be constructed since no classdef could be loaded");
			}
			if (ID == null)
			{
				throw new HabaneroDeveloperException
					("There is an error constructing a business object. Please refer to the system administrator" 
					+ "The Class could not be constructed since no _primaryKey has been created",
					 "The Class could not be constructed since no _primaryKey has been created");
			}
			BackupObjectIdPropValues();
		}

		private void BackupObjectIdPropValues()
		{
			foreach (BOProp prop in ID)
			{
				prop.BackupPropValue();
				//This next line sets the prop to be for a new object again, because 
				// the Backup would have set it to be not a new object.
				prop.IsObjectNew = true;
			}
		}

		/// <summary>
		/// Constructs the class
		/// </summary>
		/// <param name="newObject">Whether the object is new or not</param>
		protected virtual void ConstructFromClassDef(bool newObject)
		{
			if (_classDef == null) _classDef = ConstructClassDef();
			ClassDef classDef = (ClassDef) _classDef;
			CheckClassDefNotNull();

			_boPropCol = classDef.CreateBOPropertyCol(newObject);
			_keysCol = classDef.CreateBOKeyCol(_boPropCol);

			SetPrimaryKeyForInheritedClass();
			if (classDef.IsUsingSingleTableInheritance())
			{
				try
				{
					this.SetPropertyValue(classDef.SuperClassDef.Discriminator, this.GetType().Name);
				} catch (InvalidPropertyNameException ex)
				{
					throw new HabaneroDeveloperException(ex.Message + " Your discriminator field is not included in the properties of the class and you are using Single Table Inheritance. Please include the discriminator field as a property.");
				}
			}
			// NOTE: This has been moved to the Relationships getter so that it does lazy loading.
			//_relationshipCol = classDef.CreateRelationshipCol(_boPropCol, this);
		}

		private void SetPrimaryKeyForInheritedClass()
		{
			IClassDef classDefToUseForPrimaryKey = GetClassDefToUseForPrimaryKey();
			PrimaryKeyDef primaryKeyDef = (PrimaryKeyDef) classDefToUseForPrimaryKey.PrimaryKeyDef;
			if ((classDefToUseForPrimaryKey.SuperClassDef == null)
				|| (((ClassDef)classDefToUseForPrimaryKey).IsUsingConcreteTableInheritance())
				|| (_classDef.IsUsingClassTableInheritance()))
			{
				if (primaryKeyDef != null)
				{
					_primaryKey = (BOPrimaryKey) primaryKeyDef.CreateBOKey(_boPropCol);
				}
			}
			else
			{
				if (primaryKeyDef != null)
				{
					PrimaryKeyDef def = (PrimaryKeyDef) classDefToUseForPrimaryKey.SuperClassClassDef.PrimaryKeyDef;
					_primaryKey =(BOPrimaryKey) def.CreateBOKey(_boPropCol);
				}
			}
			if (_primaryKey == null)
			{
				SetupPrimaryKey();
			}
			if (_primaryKey != null) _primaryKey.BusinessObject = this;
		}

		internal IClassDef GetClassDefToUseForPrimaryKey()
		{
			ClassDef classDefToUseForPrimaryKey = (ClassDef) _classDef;
			while (classDefToUseForPrimaryKey.IsUsingSingleTableInheritance() || classDefToUseForPrimaryKey.PrimaryKeyDef == null)
			{
				classDefToUseForPrimaryKey = (ClassDef) classDefToUseForPrimaryKey.SuperClassClassDef;
			}
			return classDefToUseForPrimaryKey;
		}

		/// <summary>
		/// Constructs a class definition
		/// </summary>
		/// <returns>Returns a class definition</returns>
		protected virtual IClassDef ConstructClassDef()
		{
			return ClassDef.ClassDefs.Contains(GetType()) ? ClassDef.ClassDefs[GetType()] : null;
		}

		#endregion //Constructors

		#region Properties

		/// <summary>
		/// Returns an XML string that contains the changes in the object
		/// since the last persistance to the database
		/// </summary>
		[TypeDescriptorIgnore]
		public string DirtyXML
		{
			get
			{
				return "<" + ClassDef.ClassName + " ID='" + ID + "'>" + _boPropCol.DirtyXml + "</"
					   + ClassDef.ClassName + ">";
			}
		}

		/// <summary>
		/// Gets and sets the collection of relationships
		/// </summary>
		[TypeDescriptorIgnore]
		public RelationshipCol Relationships
		{
			get
			{
				return (RelationshipCol) (_relationshipCol 
					   ?? (_relationshipCol = ((ClassDef) _classDef).CreateRelationshipCol(_boPropCol, this)));
			}
			set { _relationshipCol = value; }
		}


		/// <summary>
		/// Returns or sets the class definition. Setting the classdef is not recommended
		/// </summary>

		[TypeDescriptorIgnore]
		public ClassDef ClassDef
		{
			get { return (ClassDef) _classDef; }
			set { _classDef = value; }
		}
//
		/// <summary>
		/// A property to store the business object's full classdef name.  this is used when persisting to an object database
		/// so that the object can be queried based on its classdef (without the necessity of persisting the entire classdef
		/// for each object).
		/// </summary>
//        public string ClassDefName { get; set; }

		/// <summary>
		/// Gets and sets the collection of relationships
		/// </summary>

		[TypeDescriptorIgnore]
		IRelationshipCol IBusinessObject.Relationships
		{
			get { return this.Relationships; }
			set { _relationshipCol = value; }
		}

		/// <summary>
		/// Returns or sets the class definition. Setting the classdef is not recommended
		/// </summary>

		[TypeDescriptorIgnore]
		IClassDef IBusinessObject.ClassDef
		{
			get { return _classDef; }
			set { _classDef = value; }
		}


		/// <summary>
		/// Returns the primary key ID of this object.  If there is no primary key on this
		/// class, the primary key of the nearest suitable parent is found and populated
		/// with the values held for that key in this object.  This is a possible situation
		/// in some forms of inheritance.
		/// </summary>
		[TypeDescriptorIgnoreAttribute]
		public IPrimaryKey ID
		{
			get
			{
				//                if (_primaryKey == null)
				//                {
				//                    CheckClassDefNotNull();
				//                    SetupPrimaryKey();
				//                }
				return _primaryKey;
			}
		}

		private void CheckClassDefNotNull()
		{
			if (_classDef == null)
			{
				throw new NullReferenceException
					(String.Format
						 ("An error occurred while loading the class definitions (usually ClassDef.xml) for "
						  + "'{0}'. Check that the class exists in that "
						  + "namespace and assembly and that there are corresponding "
						  + "class definitions for this class.\n"
						  + "Please check that the ClassDef.xml file is either an embedded resource "
						  + "or is copied to the output directory via the appropriate postbuild command ", GetType()));
			}
		}

		private void SetupPrimaryKey()
		{
			PrimaryKeyDef primaryKeyDef = (PrimaryKeyDef)ClassDef.PrimaryKeyDef;
			if (primaryKeyDef == null) return;
			_primaryKey = (BOPrimaryKey) primaryKeyDef.CreateBOKey(Props);
		}

		/// <summary>
		/// Sets the concurrency control object
		/// </summary>
		/// <param name="concurrencyControl">The concurrency control</param>
		protected void SetConcurrencyControl(IConcurrencyControl concurrencyControl)
		{
			_concurrencyControl = concurrencyControl;
		}

		/// <summary>
		/// Sets the transaction log to that specified
		/// </summary>
		/// <param name="transactionLog">A transaction log</param>
		protected void SetTransactionLog(ITransactionLog transactionLog)
		{
			TransactionLog = transactionLog;
		}

		/// <summary>
		/// Sets the IBusinessObjectAuthorisation to that specified
		/// </summary>
		/// <param name="authorisationRules">The authorisation Rules</param>
		protected internal void SetAuthorisationRules(IBusinessObjectAuthorisation authorisationRules)
		{
			AuthorisationRules = authorisationRules;
		}

		/// <summary>
		/// Sets the business object update log to the one specified
		/// </summary>
		/// <param name="businessObjectUpdateLog">A businessObject update log object</param>
		protected void SetBusinessObjectUpdateLog(IBusinessObjectUpdateLog businessObjectUpdateLog)
		{
			BusinessObjectUpdateLog = businessObjectUpdateLog;
		}

		/// <summary>
		/// Returns the collection of BOKeys
		/// </summary>
		/// <returns>Returns a BOKeyCol object</returns>
		public BOKeyCol GetBOKeyCol()
		{
			return _keysCol;
		}

		/// <summary>
		/// Returns a string useful for debugging output
		/// </summary>
		/// <returns>Returns an output string</returns>
		internal string GetDebugOutput()
		{
			string output = "";
			output += "Type: " + GetType().Name + Environment.NewLine;
			return _boPropCol.Aggregate(output, (current, entry) => current + (entry.PropertyName + " - " + entry.PropertyValueString + Environment.NewLine));
		}

		#endregion //Properties

		#region Editing Property Values

		
		/// <summary>
		/// The primary key for this business object 
		/// </summary>
		[Obsolete ("V2.5 Please use ID")]
		[TypeDescriptorIgnore]
		protected IPrimaryKey PrimaryKey
		{
			get { return _primaryKey; }
		}

		///<summary>
		/// This method can be overridden by a class that inherits from Business object.
		/// The method allows the Business object developer to add customised rules that determine.
		/// The Creatable rules of a business object.
		/// E.g. Certain users may not be allowed to create certain Business Objects.
		/// </summary>
		public virtual bool IsCreatable(out string message)
		{
			message = "";
			if (AuthorisationRules == null) return true;
			if (AuthorisationRules.IsAuthorised(this, BusinessObjectActions.CanCreate)) return true;
			message = string.Format
				("The logged on user {0} is not authorised to create a {1}", Thread.CurrentPrincipal.Identity.Name,
				 ClassDef.ClassName);
			return false;
		}


		///<summary>
		/// This method can be overridden by a class that inherits from Business object.
		/// The method allows the Business object developer to add customised rules that determine.
		/// The editable state of a business object.
		/// E.g. Once an invoice is paid it is no longer editable. Or when a course is old it is no
		/// longer editable. This allows a UI developer to standise Code for enabling and disabling controls.
		/// These rules are applied to new object as well so if you want a new object 
		/// to be editable then you must include this.Status.IsNew in evaluating IsEditable.
		/// It also allows the Application developer to implement security controlling the 
		///   Editability of a particular Business Object.
		/// </summary>
		public virtual bool IsEditable(out string message)
		{
			message = "";
			if (AuthorisationRules == null) return true;
			if (AuthorisationRules.IsAuthorised(this, BusinessObjectActions.CanUpdate)) return true;
			message = string.Format
				("The logged on user {0} is not authorised to update {1} Identified By {2}",
				 Thread.CurrentPrincipal.Identity.Name, ClassDef.ClassName, ID.AsString_CurrentValue());
			return false;
		}

		///<summary>
		/// This method can be overridden by a class that inherits from BusinessObject.
		/// The method allows the Business object developer to add customised rules that determine
		/// the Deletable state of a business object, e.g. Invoices can never be deleted once created. 
		/// Objects cannot be deleted once they have reached certain stages e.g. a customer order after it is accepted.
		/// These rules are applied to new objects too, so if you want a new object 
		/// to be deletable then you must include this.Status.IsNew in evaluating IsDeletable.
		/// It also allows the application developer to implement security that controls the 
		///  deletion of a particular Business Object.
		///</summary>
		public virtual bool IsDeletable(out string message)
		{
			message = "";
			if (AuthorisationRules != null && !AuthorisationRules.IsAuthorised(this, BusinessObjectActions.CanDelete))
			{
				message = string.Format
					("The logged on user {0} is not authorised to delete {1} Identified By {2}",
					 Thread.CurrentPrincipal.Identity.Name, ClassDef.ClassName, ID.AsString_CurrentValue());
				return false;
			}
			if (Relationships == null) return true;
			foreach (IRelationship relationship in Relationships)
			{
				bool isDeletable = relationship.IsDeletable(out message);
				if (!isDeletable)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Returns the value under the property name specified
		/// </summary>
		/// <param name="propName">The property name</param>
		/// <returns>Returns the value if found</returns>
		public object GetPropertyValue(string propName)
		{
			if (propName == null) throw new ArgumentNullException("propName");
			string message;
			if (!IsReadable(out message)) throw new BusObjReadException(message);

			return GetProperty(propName).Value;
		}

		/// <summary>
		/// Returns the value under the property name specified, accessing it through the 'source'
		/// </summary>
		/// <param name="source">The source of the property ie - the relationship or C# property this property is on</param>
		/// <param name="propName">The property name</param>
		/// <returns>Returns the value if found</returns>
		public object GetPropertyValue(Source source, string propName)
		{
			if (source == null || String.IsNullOrEmpty(source.Name)) return GetPropertyValue(propName);
			IBusinessObject businessObject = Relationships.GetRelatedObject(source.Name);
			if (businessObject == null) return null;
			if (source.Joins.Count > 0)
			{
				return businessObject.GetPropertyValue(source.Joins[0].ToSource, propName);
			}
			return businessObject.GetPropertyValue(propName);
		}

		/// <summary>
		/// Returns the value stored in the DataStore for the property name specified, accessing it through the 'source'
		/// </summary>
		/// <param name="source">The source of the property ie - the relationship or C# property this property is on</param>
		/// <param name="propName">The property name</param>
		/// <returns>Returns the value if found</returns>
		public object GetPersistedPropertyValue(Source source, string propName)
		{
			if (String.IsNullOrEmpty(propName)) throw new ArgumentNullException("propName");
			IBOProp prop;
			if (source == null || String.IsNullOrEmpty(source.Name))
			{
				prop = GetProperty(propName);
				return prop.PersistedPropertyValue;
			}

			BusinessObject businessObject = (BusinessObject) Relationships.GetRelatedObject(source.Name);
			if (businessObject == null) return null;
			if (source.Joins.Count > 0)
			{
				return businessObject.GetPersistedPropertyValue(source.Joins[0].ToSource, propName);
			}
			return businessObject.GetPersistedPropertyValue(null, propName);
		}


		/// <summary>
		/// Sets a property value to a new value
		/// </summary>
		/// <param name="propName">The property name</param>
		/// <param name="newPropValue">The new value to set to</param>
		public void SetPropertyValue(string propName, object newPropValue)
		{
			var prop = GetProperty(propName);
#if DEBUG
			if (_logger.IsLogging(LogCategory.Debug))
			{
				//_logger.Log("SetPropertyValue BO (" + this + ") prop (" + propName + ") previousValue (" + prop.Value + ") new value (" + newPropValue + ")", LogCategory.Debug);
				_logger.Log(propName + " is being set to '" + newPropValue + "'", LogCategory.Debug);
				_logger.Log(GetStackTrace(), LogCategory.Debug);
			}
#endif
			prop.Value = newPropValue;
		}

		private static string GetStackTrace()
		{
			var stack = new StackTrace();
			return stack.ToString();
			// var frame = stack.GetFrame(1);
		}
		/// <summary>
		/// The BOProps in this business object
		/// </summary>
		[TypeDescriptorIgnore]
		public IBOPropCol Props
		{
			get { return _boPropCol; }
		}

		/// <summary>
		/// Indicates whether all of the property values are valid
		/// </summary>
		/// <param name="invalidReason">A string to modify with a reason
		/// for any invalid values</param>
		/// <returns>Returns true if all are valid</returns>
		[Obsolete("Please use IsValid on the Status property of the BusinessObject: eg. myBO.Status.IsValid()")]
		public bool IsValid(out string invalidReason)
		{
			return _boStatus.IsValid(out invalidReason);
		}

		/// <summary>
		/// Indicates whether all of the property values are valid
		/// </summary>
		/// <returns>Returns true if all are valid</returns>
		[Obsolete("Please use IsValid on the Status property of the BusinessObject: eg. myBO.Status.IsValid()")]
		public bool IsValid()
		{
			return Status.IsValid();
		}

		/// <summary>
		/// The IBOState <see cref="IBOStatus"/> object for this BusinessObject, which records the status information of the object
		/// </summary>
		[TypeDescriptorIgnore]
		public virtual IBOStatus Status
		{
			get { return _boStatus; }
		}

		/// <summary>
		/// Returns the value under the property name specified. 
		/// But returns the value as a string regardless of the
		/// true underlying value type.
		/// </summary>
		/// <param name="propName">The property name</param>
		/// <returns>Returns a string</returns>
		public string GetPropertyValueString(string propName)
		{
			return GetProperty(propName).PropertyValueString;
		}

		///<summary>
		/// This method can be overridden by a class that inherits from Business object.
		/// The method allows the Business object developer to add customised rules that determine.
		/// The Readable rules of a business object.
		/// E.g. Certain users may not be allowed to view certain objects.
		/// </summary>
		public virtual bool IsReadable(out string message)
		{
			message = "";
			if (AuthorisationRules == null) return true;
			if (AuthorisationRules.IsAuthorised(this, BusinessObjectActions.CanRead)) return true;
			message = string.Format
				("The logged on user {0} is not authorised to read a {1}", Thread.CurrentPrincipal.Identity.Name,
				 ClassDef.ClassName);
			return false;
		}

		///<summary>
		/// Returns the value under the property name specified
		/// as a specific type <paramref name="{T}"/>
		///</summary>
		///<param name="propName">The property name</param>
		///<typeparam name="T">The type to cast the retrieved property value to.</typeparam>
		///<returns>Returns the value if found</returns>
		public T GetPropertyValue<T>(string propName)
		{
			return (T) GetPropertyValue(propName);
		}
		/// <summary>
		/// Returns the <see cref="IBOProp"/> that for this property.
		/// Raises <see cref="InvalidPropertyNameException"/> if prop not found.
		/// </summary>
		/// <param name="propName"></param>
		/// <exception cref="InvalidPropertyNameException">Raised if BOProp with propname does not exit in
		/// Props the collection of BOPRops</exception>
		/// <returns></returns>
		internal IBOProp GetProperty(string propName)
		{
			try
			{
				
				return Props[propName];
			}
			catch (InvalidPropertyNameException)
			{
				string errMessage = String.Format
					("The given property name '{0}' does not exist in the "
					 + "collection of properties for the class '{1}'.", propName, GetType().Name);
				throw new InvalidPropertyNameException(errMessage);
			}
		}


		/// <summary>
		/// Sets the object's state into editing mode.  The original state can
		/// be restored with Restore() and changes can be committed to the
		/// datastore by calling Save().
		/// </summary>
		internal void BeginEdit()
		{
			BeginEdit(false);
		}

		/// <summary>
		/// Sets the object's state into editing mode.  The original state can
		/// be restored with Restore() and changes can be committed to the
		/// database by calling Save().
		/// </summary>
		private bool _beginEditRunning;

		private void BeginEdit(bool delete)
		{
			if (_beginEditRunning) return;
			try
			{
				_beginEditRunning = true;
				string message;
				if (!IsEditable(out message) && !delete)
				{
					throw new BusObjEditableException(this, message);
				}
				CheckNotEditing();
				CheckConcurrencyBeforeBeginEditing();
				_boStatus.IsEditing = true;
			}
			finally
			{
				_beginEditRunning = false;
			}
		}

		/// <summary>
		/// Checks whether editing is already taking place, in which case
		/// an exception is thrown
		/// </summary>
		/// <exception cref="EditingException">Thrown if editing is taking
		/// place</exception>
		private void CheckNotEditing()
		{
			if (Status.IsEditing)
			{
				throw new EditingException(ClassDef.ClassName, ID.ToString(), this);
			}
		}

		/// <summary>
		/// Returns the named property value that should be displayed
		///   on a user interface e.g. a textbox.
		/// This is used primarily for Lookup lists where
		///    the value stored for the object may be a guid but the value
		///    to display may be a string.
		/// </summary>
		/// <param name="propName">The property name</param>
		/// <returns>Returns the property value</returns>
		internal object GetPropertyValueToDisplay(string propName)
		{
			IBOProp prop = GetProperty(propName);
			return prop.PropertyValueToDisplay;
		}

		/// <summary>
		/// Returns the property value as in <see cref="GetPropertyValueToDisplay"/> , but
		/// returns the value as a string.
		/// </summary>
		/// <param name="propName">The property name</param>
		/// <returns>Returns a string</returns>
		internal string GetPropertyStringValueToDisplay(string propName)
		{
			object val = GetPropertyValueToDisplay(propName);
			return val != null ? val.ToString() : "";
		}
		[Obsolete("2010/10/11 V2.5 This internal method no longer appears to be used anywhere")]
		internal static bool PropValueHasChanged(object currentPropValue, object newPropValue)
		{
			if (currentPropValue == newPropValue) return false;
			if (currentPropValue != null) return !currentPropValue.Equals(newPropValue);
			return (newPropValue != null && !string.IsNullOrEmpty(Convert.ToString(newPropValue)));
		}

		/// <summary>
		/// Sets an initial property value
		/// </summary>
		/// <param name="propName">The property name</param>
		/// <param name="propValue">The value to initialise to</param>
		private void InitialisePropertyValue(string propName, object propValue)
		{
			IBOProp prop = GetProperty(propName);
			prop.Value = propValue;

		}

		#endregion //Editing Property Values

		#region Persistance

		[Obsolete("V 2.5 Use Props.HasAutoIncrementingField")]
		internal bool HasAutoIncrementingField
		{
			get { return _boPropCol.HasAutoIncrementingField; }
		}

		/// <summary>
		/// This returns the Transaction Log object set up for this BusinessObject.
		/// </summary>
		[TypeDescriptorIgnoreAttribute]
		public ITransactionLog TransactionLog { get; private set; }

		/// <summary>
		/// The Update Log being used for this Business Object.
		/// </summary>
		protected virtual internal IBusinessObjectUpdateLog BusinessObjectUpdateLog
		{
			get { return _businessObjectUpdateLog; }
			set { _businessObjectUpdateLog = value; }
		}

		internal IList<IBusinessObjectRule> GetBusinessObjectRules()
		{
			//Lazy initialisation so as to prevent unneccessary objects being created during construction.
			if (_boRules == null)
			{
				_boRules = new List<IBusinessObjectRule>();
				LoadBusinessObjectRules(_boRules);
			}
			return _boRules;
		}

		///// <summary>
		///// Commits to the database any changes made to the object
		///// </summary>
		//public virtual IBusinessObject Save()
		//{
		//    ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
		//    committer.AddBusinessObject(this);
		//    committer.CommitTransaction();
		//    return this;
		//}

		/// <summary>
		/// Cancel all edits made to the object since it was loaded from the 
		/// database or last saved to the database
		/// </summary>
		[Obsolete("This is replaced by CancelEdits().")]
		public void Restore()
		{
			CancelEdits();
		}

		/// <summary>
		/// Cancel all edits made to the object since it was loaded from the 
		/// database or last saved to the database
		/// </summary>
		public void CancelEdits()
		{
			_boPropCol.RestorePropertyValues();
			_boStatus.IsDeleted = false;
			_boStatus.IsEditing = false;
			Relationships.CancelEdits();
			ReleaseWriteLocks();
			FireUpdatedEvent();
			FireRestoredEvent();
			_boPropCol.RestorePropertyValues();
		}

		/// <summary>
		/// Marks the business object for deleting.  Calling Save() or saving the transaction will
		/// then carry out the deletion from the database.
		/// </summary>
		public void MarkForDelete()
		{
			CheckIsDeletable();
			if (!Status.IsEditing)
			{
				BeginEdit(true);
			}
			_boStatus.IsDeleted = true;
			MarkChildrenForDelete();
			FireMarkForDeleteEvent();
		}

		private void MarkChildrenForDelete()
		{
			foreach (IRelationship relationship in Relationships)
			{
				if (relationship.DeleteParentAction == DeleteParentAction.DeleteRelated)
				{
					relationship.MarkForDelete();
				}
			}
		}


		/// <summary>
		/// Marks the business object for deleting.  Calling Save() will
		/// then carry out the deletion from the database.
		/// </summary>
		[Obsolete(
			"V2.1 This method has been replaced with MarkForDelete() since it is far more explicit that this does not instantly delete the business object."
			)]
		public void Delete()
		{
			MarkForDelete();
		}

		/// <summary>
		/// Extra preparation or steps to take out after loading the business. Override this if you need to update a calculated property, for example.
		/// This method will be called after an object is loaded for the first time, and after it is refreshed, but only if the refreshing causes
		/// a property to be updated.
		/// object
		/// </summary>
		protected internal virtual void AfterLoad()
		{

		}

		/// <summary>
		/// Carries out updates to the object after changes have been
		/// committed to the database
		/// </summary>
		protected internal void UpdateStateAsPersisted()
		{
			if (Status.IsDeleted)
			{
				CleanUpAllRelationshipCollections();
				SetStateAsPermanentlyDeleted();
				BORegistry.BusinessObjectManager.Remove(this);
				FireDeletedEvent();
			}
			else
			{
				BORegistry.BusinessObjectManager.Remove(this);
				StorePersistedPropertyValues();
				SetStateAsUpdated();
				var boManager = BORegistry.BusinessObjectManager;
				if (!boManager.Contains(this))
				{
					if (!boManager.Contains(ID.ObjectID))
					{
						boManager.Add(this);
					}
				}
				FireSavedEvent();
			}
			AfterSave();
			ReleaseWriteLocks();
		}

		private void CleanUpAllRelationshipCollections()
		{
			if (!Status.IsDeleted) return;
			foreach (IRelationship relationship in Relationships)
			{
				if (!(relationship is IMultipleRelationship)) continue;
				IMultipleRelationship multipleRelationship = (IMultipleRelationship) relationship;

				IList createdBos = multipleRelationship.CurrentBusinessObjectCollection.CreatedBusinessObjects;
				while (createdBos.Count > 0)
				{
					IBusinessObject businessObject = (IBusinessObject) createdBos[createdBos.Count - 1];
					createdBos.Remove(businessObject);
					if (relationship.DeleteParentAction == DeleteParentAction.DereferenceRelated) continue;
					((BOStatus) businessObject.Status).IsDeleted = true;
				}
				multipleRelationship.CurrentBusinessObjectCollection.RemovedBusinessObjects.Clear();
			}
		}

		private void StorePersistedPropertyValues()
		{
			_boPropCol.BackupPropertyValues();
		}


		private void SetStateAsUpdated()
		{
			_boStatus.IsNew = false;
			_boStatus.IsDeleted = false;
			//_boStatus.IsDirty = false;
			_boStatus.IsEditing = false;
		}

		internal void SetStateAsPermanentlyDeleted()
		{
			_boStatus.IsNew = true;
			_boStatus.IsDeleted = true;
			//_boStatus.IsDirty = false;
			_boStatus.IsEditing = false;
		}

		///<summary>
		/// Executes any custom code required by the business object before it is persisted to the database.
		/// This has the additionl capability of creating or updating other business objects and adding these
		/// to the transaction committer.
		/// <remarks> Recursive call to UpdateObjectBeforePersisting will not be done i.e. it is the bo developers responsibility to implement</remarks>
		///</summary>
		///<param name="transactionCommitter">the transaction committer that is executing the transaction</param>
		protected internal virtual void UpdateObjectBeforePersisting(ITransactionCommitter transactionCommitter)
		{
			//TODO: solidify method for using the transactionlog (either in constructor or in updateobjectbeforepersisting)
			//if (_transactionLog != null)
			//{
			//    transactionCommitter.AddTransaction(_transactionLog);
			//}
			Relationships.AddDirtyChildrenToTransactionCommitter((TransactionCommitter) transactionCommitter);

			if (BusinessObjectUpdateLog != null && (Status.IsNew || (Status.IsDirty && !Status.IsDeleted)))
			{
				BusinessObjectUpdateLog.Update();
			}
		}

		private void CheckIsDeletable()
		{
			string errMsg;
			if (!IsDeletable(out errMsg))
			{
				throw new BusObjDeleteException(this, errMsg);
			}
		}

		/// <summary>
		/// Fires Updates Event for <see cref="IBusinessObject"/>
		/// </summary>
		protected void FireMarkForDeleteEvent()
		{
			if (MarkedForDeletion != null)
			{
				MarkedForDeletion(this, new BOEventArgs(this));
			}
		}

		/// <summary>
		/// Fires Updates Event for <see cref="IBusinessObject"/>
		/// </summary>
		internal protected void FireUpdatedEvent()
		{
			if (Updated != null)
			{
				Updated(this, new BOEventArgs(this));
			}
		}

		/// <summary>
		/// Fires Updated Event for <see cref="IBusinessObject"/>
		/// </summary>
		/// <param name="prop"></param>
		protected void FirePropertyUpdatedEvent(IBOProp prop)
		{
			if (PropertyUpdated != null)
			{
				PropertyUpdated(this, new BOPropUpdatedEventArgs(this, prop));
			}
		}

		/// <summary>
		/// Fires IDUpdated Event for <see cref="IBusinessObject"/>
		/// </summary>
		protected void FireIDUpdatedEvent()
		{
			if (IDUpdated != null)
			{
				IDUpdated(this, new BOEventArgs(this));
			}
		}

		private void FireRestoredEvent()
		{
			if (Restored != null)
			{
				Restored(this, new BOEventArgs(this));
			}
		}

		private void FireSavedEvent()
		{
			if (Saved != null)
			{
				Saved(this, new BOEventArgs(this));
			}
		}

		private void FireDeletedEvent()
		{
			if (Deleted != null)
			{
				Deleted(this, new BOEventArgs(this));
			}
		}

		/// <summary>
		/// Override this method in subclasses of BusinessObject to check custom rules for that
		/// class.  The default implementation returns true and sets customRuleErrors to the empty string.
		/// </summary>
		/// <param name="customRuleErrors">The error string to display</param>
		/// <returns>true if no custom rule errors are encountered.</returns>
		protected virtual bool AreCustomRulesValid(out string customRuleErrors)
		{
			customRuleErrors = "";
			return true;
		}

		/// <summary>
		/// Override this method in subclasses of BusinessObject to check custom rules for that
		/// class.  The default implementation returns true and sets customRuleErrors to the empty string.
		/// </summary>
		/// <param name="errors">The errors</param>
		/// <returns>true if no custom rule errors are encountered.</returns>
		protected virtual bool AreCustomRulesValid(ref IList<IBOError> errors)
		{
			if (errors ==null) errors = new List<IBOError>();

			return errors.Count == 0;
		}

		/// <summary>
		/// Load the <see cref="IBusinessObjectRule"/>s for this BusinessObject.
		/// </summary>
		/// <param name="boRules"></param>
		protected virtual void LoadBusinessObjectRules(IList<IBusinessObjectRule> boRules)
		{
			//This must be overridden in the inherited class with any custom rules.
		}

		/// <summary>
		/// Calls through to <see cref="AreCustomRulesValid(out string)"/>
		/// </summary>
		/// <param name="customRuleErrors">The error string to display</param>
		/// <returns>true if no custom rule errors are encountered.</returns>
		internal bool AreCustomRulesValidInternal(out string customRuleErrors)
		{
			return AreCustomRulesValid(out customRuleErrors);
		}

		/// <summary>
		/// Checks the <see cref="GetBusinessObjectRules"/>. Calls through to 
		/// <see cref="AreCustomRulesValid(ref System.Collections.Generic.IList{Habanero.Base.IBOError})"/>
		/// </summary>
		/// <param name="errors">The errors</param>
		/// <returns>true if no custom rule errors are encountered.</returns>
		protected internal bool AreCustomRulesValidInternal(out IList<IBOError> errors)
		{
			IList<IBOError> customErrors = new List<IBOError>();
			AreCustomRulesValid(ref customErrors);
			errors = new List<IBOError>();
			HasErrors(ref errors);
			if (customErrors != null)
			{
				foreach (IBOError e in customErrors)
				{
					errors.Add(e);
				}
			}
			return errors == null || errors.Count == 0;
		}

		private bool HasErrors(ref IList<IBOError> errors)
		{
			if (errors == null) errors = new List<IBOError>();
			var rules = GetBusinessObjectRules()
					.Where(rule 
					  => (rule != null && ErrorLevelIsError(rule)) 
					  && !rule.IsValid(this));
			foreach (IBusinessObjectRule rule in rules)
			{
				CreateBOError(rule, errors);
			}
			return errors.Count != 0;
		}

		/// <summary>
		/// Checks the <see cref="GetBusinessObjectRules"/> for any rules that have warnings or suggestions.
		/// </summary>
		/// <param name="errors">The warnings and suggestions</param>
		/// <returns>true if no <see cref="GetBusinessObjectRules"/> errors are encountered.</returns>
		internal bool HasWarnings(out IList<IBOError> errors)
		{
			errors = new List<IBOError>();
			foreach (IBusinessObjectRule rule in GetBusinessObjectRules())
			{
				if (rule == null || ErrorLevelIsError(rule) || rule.IsValid(this)) continue;
				CreateBOError(rule, errors);
			}
			return errors.Count != 0;
		}

		private static bool ErrorLevelIsError(IBusinessObjectRule rule)
		{
			return rule.ErrorLevel == ErrorLevel.Error;
		}

		private void CreateBOError(IBusinessObjectRule rule, ICollection<IBOError> errors)
		{
			string message = rule.Message;
			BOError error = new BOError(message, rule.ErrorLevel) {BusinessObject = this};
			errors.Add(error);
		}

		#endregion //Persistance

		#region XMLSerialization

		/// <summary>
		/// Method implemented for legacy purposes only. Returns null.
		/// </summary>
		/// <returns></returns>
		[Obsolete("v2.6.0: Rather use classes like DataStoreInMemoryXmlReader/DataStoreInMemoryXmlWriter")]
		public XmlSchema GetSchema()
		{
			return null;
		}

		/// <summary>
		/// Defines how to read Business Objects from serialized xml
		/// </summary>
		/// <param name="reader">The XmlReader</param>
		[Obsolete("v2.6.0: Rather use classes like DataStoreInMemoryXmlReader/DataStoreInMemoryXmlWriter")]
		public void ReadXml(XmlReader reader)
		{
			while (reader.MoveToNextAttribute())
			{
				string propertyName = reader.Name;
				string propertyValue = reader.Value;
				SetPropertyValue(propertyName, propertyValue);
			}

			reader.MoveToContent();
			reader.Read();
			if (!string.IsNullOrEmpty(reader.Name))
			{
				string relationshipName = reader.Name;
				if (Relationships.Contains(relationshipName))
				{
					IRelationship relationship = Relationships[relationshipName];
					RelationshipDef relationshipDef = (RelationshipDef)relationship.RelationshipDef;
					Type relatedObjectType = relationshipDef.RelatedObjectClassType;
					reader.MoveToContent();
					reader.Read();

					if (relationship is ISingleRelationship)
					{
						IBusinessObject relatedObject = (IBusinessObject)Activator.CreateInstance(relatedObjectType);
						relatedObject.ReadXml(reader);
						((ISingleRelationship)relationship).SetRelatedObject(relatedObject);
					}
					else if (relationship is IMultipleRelationship)
					{
						ReadRelatedObject(reader, relationship, relatedObjectType);
					}
				}
				else
				{
					string className = ClassDef.ClassName;
					if (relationshipName != className && relationshipName != "ArrayOf" + className
						&& reader.NodeType != XmlNodeType.EndElement)
					{
						throw new InvalidRelationshipNameException
							(string.Format
								 ("The relationship '{0}' does not exist on the class '{1}'.", relationshipName,
								  className));
					}
				}
			}
		}

		/// <summary>
		/// Defines how to write Business Objects to serialized xml 
		/// </summary>
		/// <param name="writer">The XmlWriter</param>
		[Obsolete("v2.6.0: Rather use classes like DataStoreInMemoryXmlReader/DataStoreInMemoryXmlWriter")]
		public void WriteXml(XmlWriter writer)
		{
			foreach (IBOProp prop in _boPropCol)
			{
				writer.WriteAttributeString(prop.PropertyName, Convert.ToString(prop.Value));
			}
			foreach (IRelationship relationship in Relationships)
			{
				//what type of relationship? composition,aggregation...
				//if (relationship.RelationshipDef.RelationshipType != RelationshipType.Association)

				RelationshipType relationshipType = relationship.RelationshipDef.RelationshipType;
				if (relationshipType == RelationshipType.Composition || relationshipType == RelationshipType.Aggregation)
				{
					WriteXmlNestedRelationship(writer, relationship);
				}
			}
		}

		[Obsolete("v2.6.0: Rather use classes like DataStoreInMemoryXmlReader/DataStoreInMemoryXmlWriter")]
		private static void ReadRelatedObject(XmlReader reader, IRelationship relationship, Type relatedObjectType)
		{
			IBusinessObject relatedObject = (IBusinessObject)Activator.CreateInstance(relatedObjectType);
			relatedObject.ReadXml(reader);
			((IMultipleRelationship)relationship).BusinessObjectCollection.Add(relatedObject);

			string elementName = reader.Name;
			if (elementName == relatedObjectType.Name)
			{
				ReadRelatedObject(reader, relationship, relatedObjectType);
			}
		}

		/// <summary>
		/// Writes related objects that are in composite and aggregate relationships
		/// as nested xml.
		/// </summary>
		[Obsolete("v2.6.0: Rather use classes like DataStoreInMemoryXmlReader/DataStoreInMemoryXmlWriter")]
		private static void WriteXmlNestedRelationship(XmlWriter writer, IRelationship relationship)
		{
			if (relationship is ISingleRelationship)
			{
				ISingleRelationship singleRelationship = (ISingleRelationship)relationship;
				IBusinessObject relatedObject = singleRelationship.GetRelatedObject();
				if (relatedObject != null)
				{
					writer.WriteStartElement(relationship.RelationshipName);
					writer.WriteStartElement(relationship.RelationshipDef.RelatedObjectClassName);
					relatedObject.WriteXml(writer);
					writer.WriteEndElement();
					writer.WriteEndElement();
				}
			}
			else if (relationship is IMultipleRelationship)
			{
				IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
				IBusinessObjectCollection relatedObjects = multipleRelationship.BusinessObjectCollection;
				if (relatedObjects.Count != 0)
				{
					writer.WriteStartElement(relationship.RelationshipName);
					foreach (IBusinessObject relatedObject in relatedObjects)
					{
						writer.WriteStartElement(relationship.RelationshipDef.RelatedObjectClassName);
						relatedObject.WriteXml(writer);
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
				}
			}
		}

		#endregion

		#region Concurrency

		/// <summary>
		/// Checks concurrency before persisting to the database
		/// </summary>
		protected internal virtual void CheckConcurrencyBeforePersisting()
		{
			if (_concurrencyControl != null)
			{
				_concurrencyControl.CheckConcurrencyBeforePersisting();
			}
			UpdatedConcurrencyControlPropertiesBeforePersisting();
		}

		/// <summary>
		/// Checks concurrency before beginning to edit an object's values
		/// </summary>
		protected virtual void CheckConcurrencyBeforeBeginEditing()
		{
			if (_concurrencyControl != null)
			{
				_concurrencyControl.CheckConcurrencyBeforeBeginEditing();
			}
		}

		/// <summary>
		/// Updates the concurrency control properties
		/// </summary>
		protected virtual void UpdatedConcurrencyControlPropertiesBeforePersisting()
		{
			if (_concurrencyControl != null)
			{
				_concurrencyControl.UpdatePropertiesWithLatestConcurrencyInfoBeforePersisting();
			}
		}

		/// <summary>
		/// Releases write locks from the database
		/// </summary>
		protected virtual void ReleaseWriteLocks()
		{
			if (_concurrencyControl != null)
			{
				_concurrencyControl.ReleaseWriteLocks();
			}
		}

		//        /// <summary>
		//        /// Releases read locks from the database
		//        /// </summary>
		//        protected virtual void ReleaseReadLocks()
		//        {
		//            if (!(_concurrencyControl == null))
		//            {
		//                _concurrencyControl.ReleaseReadLocks();
		//            }
		//        }

		#endregion //Concurrency

		///<summary>
		/// Called by the transaction committer in the case where the transaction failed
		/// and was rolled back.
		///</summary>
		protected internal virtual void UpdateAsTransactionRolledBack()
		{
			if ((_concurrencyControl == null)) return;
			_concurrencyControl.UpdateAsTransactionRolledBack();
		}

		/// <summary>
		/// Called by the business object when the transaction has been successfully committed
		/// to the database. Called in cases of insert, delete and update.
		/// </summary>
		protected internal virtual void AfterSave()
		{
			FireUpdatedEvent();
		}

		/// <summary>
		/// Sets the status of the business object to the status true or false.
		/// </summary>
		/// <param name="status"></param>
		/// <param name="value"></param>
		internal void SetStatus(BOStatus.Statuses status, bool value)
		{
			_boStatus.SetBOFlagValue(status, value);
		}

		/// <summary>
		/// Checks if the object can be persisted. This
		/// Checks the basic rules e.g. If you are deleting then
		///   IsDeletable if creating a new object then IsCreatable
		///   if Updating then IsEditable.
		/// </summary>
		/// <param name="errMsg">The appropriate error message if the 
		///  Business Object cannot be persisted</param>
		/// <returns></returns>
		protected internal virtual bool CanBePersisted(out string errMsg)
		{
			errMsg = "";
			if (Status.IsDeleted && Status.IsNew)
			{
				errMsg = "The object has already been deleted from the dataBase and cannot be persisted again";
				return false;
			}
			if (Status.IsDeleted && !Status.IsNew)
			{
				return IsDeletable(out errMsg);
			}

			if (Status.IsNew)
			{
				return IsCreatable(out errMsg);
			}

			return !Status.IsDirty || IsEditable(out errMsg);
		}
/*
		internal void UpdateDirtyStatusFromProperties()
		{
			bool hasDirtyProps = false;
			foreach (BOProp prop in _boPropCol)
			{
				if (prop.IsDirty) hasDirtyProps = true;
			}

		  //  _boStatus.SetBOFlagValue(BOStatus.Statuses.isDirty, hasDirtyProps);
		}*/
/*
		
		internal void SetDirty(bool dirty)
		{
			_boStatus.IsDirty = dirty;
		}
*/

		/// <summary>
		/// Is the <see cref="IBusinessObject"/> archived or not. This can be overriden by a
		/// specific business object to implement required behaviour.
		/// </summary>
		/// <returns></returns>
		[Obsolete("V 2.5.0 This is no longer used")]
		protected internal virtual bool IsArchived()
		{
			return false;
		}
		/// <summary>
		/// Commits to the database any changes made to the object
		/// </summary>
		public IBusinessObject Save()
		{
			var committer = BORegistry.DataAccessor.CreateTransactionCommitter();
			committer.AddBusinessObject(this);
			committer.CommitTransaction();
			return this;
		}
	}
	/// <summary>
	/// This is an attribute that is placed on properties such as <see cref="Relationship"/>
	/// which should not be part of the TypeDescriptor.GetProperties when binding.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]  
	public class TypeDescriptorIgnoreAttribute : Attribute
	{
	}

	/// <summary>
	/// A Generic version of BusinessObject.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class BusinessObject<T> : BusinessObject
	{
		public BusinessObject() : base() { }
		protected internal BusinessObject(IClassDef def) : base(def) { }
		protected BusinessObject(ConstructForFakes constructForFakes) : base(constructForFakes)
		{
		}

		///// <summary>
		///// Sets a property value to a new value
		///// </summary>
		///// <param name="propNameExpression">The property name expression (eg. p => p.Name)</param>
		///// <param name="newPropValue">The new value to set to</param>
		///// <remarks>This runs about 5 times slower than the normal <see cref="BusinessObject.SetPropertyValue"/> method but has the advantage
		///// of being type safe.  Unless you are experiencing performance problems using this method, it is the recommended way of setting a property value.
		///// </remarks>
		//public void SetPropertyValue(Expression<Func<T, object>> propNameExpression, object newPropValue)
		//{
		//    var memberExpression = propNameExpression.Body as MemberExpression;
		//    if (memberExpression == null)
		//    {
		//        throw new ArgumentException(propNameExpression + " is not a valid property on " + this.GetType().Name);
		//    }
		//    SetPropertyValue(memberExpression.Member.Name, newPropValue);
		//}
		/// <summary>
		/// Sets a property value to a new value
		/// </summary>
		/// <param name="propNameExpression">The property name expression (eg. p => p.Name)</param>
		/// <param name="newPropValue">The new value to set to</param>
		/// <remarks>This runs about 5 times slower than the normal <see cref="BusinessObject.SetPropertyValue"/> method but has the advantage
		/// of being type safe.  Unless you are experiencing performance problems using this method, it is the recommended way of setting a property value.
		/// </remarks>
		public void SetPropertyValue<TOut>(Expression<Func<T, TOut>> propNameExpression, object newPropValue)
		{
			MemberExpression memberExpression;
			try
			{
				memberExpression = (MemberExpression)propNameExpression.Body;  //this is done instead of "as" for a small performance gain
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(propNameExpression + " is not a valid property on " + this.GetType().Name);
			}
			SetPropertyValue(memberExpression.Member.Name, newPropValue);
		}

		/// <summary>
		/// Returns the value under the property name specified
		/// </summary>
		/// <param name="propNameExpression">The property name expression (eg. p => p.Name)</param>
		/// <returns>Returns the value if found</returns>
		/// <remarks>This runs about 5 times slower than the normal <see cref="BusinessObject.GetPropertyValue(string)"/> method but has the 
		/// advantage of being type safe.  Unless you are experiencing performance problems using this method, 
		/// it is the recommended way of setting a property value.
		/// </remarks>
		public TOut GetPropertyValue<TOut>(Expression<Func<T, TOut>> propNameExpression)
		{
			MemberExpression memberExpression;
			try
			{
				memberExpression = (MemberExpression)propNameExpression.Body;  //this is done instead of "as" for a small performance gain
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(propNameExpression + " is not a valid property on " + this.GetType().Name);
			}
			return (TOut) GetPropertyValue(memberExpression.Member.Name);
		}
		/// <summary>
		/// Commits to the database any changes made to the object
		/// </summary>
		public new BusinessObject<T> Save()
		{
			var committer = BORegistry.DataAccessor.CreateTransactionCommitter();
			committer.AddBusinessObject(this);
			committer.CommitTransaction();
			return this;
		}
	}
/*    /// <summary>
	/// Adds convenience extension methods to the <see cref="IBusinessObject"/>
	/// e.g. the <see cref="Save{T}"/> method
	/// </summary>
	public static class BusinessObjectExtensions
	{
		/// <summary>
		/// Commits to the database any changes made to the object
		/// </summary>
		public static T Save<T>(this T businessObject) where T: IBusinessObject
		{
			if(businessObject.IsNull()) throw new HabaneroApplicationException("There is an application error a business object that is null was saved via the Save{T} extension Method"); 
			var committer = BORegistry.DataAccessor.CreateTransactionCommitter();
			committer.AddBusinessObject(businessObject);
			committer.CommitTransaction();
			return businessObject;
		}
	}*/
}
