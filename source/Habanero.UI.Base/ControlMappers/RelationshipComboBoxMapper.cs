// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using log4net;

namespace Habanero.UI.Base
{
    /// <summary>
    /// An interface for a mapper that <br/>
    /// Wraps/Decorates a ComboBox in order to display a collection of business objects
    /// in a combobox so that the user can select a business object for the purposes 
    /// of setting a related Business Object. 
    /// </summary>
    //----------------------------------------------------------------------
    // This does not inherit from Control mapper because the the control mapper is currently specific to mapping a property
    // of the business object.
    // whereas this mapper is actually mapping to a relationship not to a property. 
    public class RelationshipComboBoxMapper : IComboBoxMapper
    {
        /// <summary>
        /// Uses for logging 
        /// </summary>
        protected static readonly ILog log = LogManager.GetLogger("Habanero.UI.Base.RelationshipComboBoxMapper");
        /// <summary>
        /// Gets the error provider for this control <see cref="IErrorProvider"/>
        /// </summary>
        public IErrorProvider ErrorProvider { get; private set; }
        
        private IClassDef _classDef;

//        private IBusinessObjectCollection _businessObjectCollection;
        private IBusinessObject _businessObject;
        /// <summary>
        /// The relationshipDef that is used for this Mapper.
        /// </summary>
        protected IRelationshipDef _relationshipDef;
        private ISingleRelationship _singleRelationship;
        private IComboBoxMapperStrategy _mapperStrategy;
        private readonly ComboBoxCollectionSelector _comboBoxCollectionSelector;
        private readonly BORelationshipMapper _boRelationshipMapper;

        /// <summary>
        /// Constructs a <see cref="RelationshipComboBoxMapper"/> with the <paramref name="comboBox"/>
        ///  <paramref name="relationshipName"/>
        /// </summary>
        /// <param name="comboBox">The combo box that is being mapped to</param>
        /// <param name="relationshipName">The name of the relation that is being mapped to</param>
        /// <param name="isReadOnly">Whether the Combo box can be used to edit from or whether it is only viewable</param>
        /// <param name="controlFactory">A control factory that is used to create control mappers etc</param>
        public RelationshipComboBoxMapper
            (IComboBox comboBox, string relationshipName, bool isReadOnly, IControlFactory controlFactory)
        {
            if (comboBox == null) throw new ArgumentNullException("comboBox");
            if (relationshipName == null) throw new ArgumentNullException("relationshipName");
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");

            IsReadOnly = isReadOnly;
            ControlFactory = controlFactory;
            Control = comboBox;
            RelationshipName = relationshipName;
            _boRelationshipMapper = new BORelationshipMapper(relationshipName);
            _boRelationshipMapper.RelationshipChanged += (sender,e) => OnMappedRelationshipChanged();
            this.IncludeBlankItem = true;
            _mapperStrategy = ControlFactory.CreateLookupComboBoxDefaultMapperStrategy();
            _mapperStrategy.AddHandlers(this);
            UpdateIsEditable();
            _comboBoxCollectionSelector = new ComboBoxCollectionSelector(comboBox, controlFactory, false);
        }

        /// <summary>
        /// The Control <see cref="IComboBox"/> that is being mapped by this Mapper.
        /// </summary>
        public IComboBox Control { get; private set; }


        /// <summary>
        /// Is this control readonly or can the value be changed via the user interface.
        /// </summary>
        public bool IsReadOnly { get; private set; }

        /// <summary>
        /// The Control factory used to create controls of the specified type.
        /// </summary>
        public IControlFactory ControlFactory { get; private set; }

        /// <summary>
        /// The name of the relationship that is being mapped by this Mapper
        /// </summary>
        public string RelationshipName { get; private set; }

        /// <summary>
        /// Get and Set whether to include a blank item in the selector or not.
        /// By default this is true.
        /// </summary>
        public bool IncludeBlankItem { get; set; }

        /// <summary>
        /// Gets or sets the SelectedIndexChanged event handler assigned to this mapper
        /// </summary>
        public EventHandler SelectedIndexChangedHandler { get; set; }

        /// <summary>
        /// Returns the name of the property being edited in the control
        /// </summary>
        public string PropertyName
        {
            get { return this.RelationshipName; }
        }

        ///<summary>
        /// Gets and Sets the Class Def of the Business object whose property
        /// this control maps.
        ///</summary>
        public IClassDef ClassDef
        {
            get { return _classDef; }
            set
            {
                _classDef = value;
                //SetUpRelationship();
            }
        }

        protected IClassDef RelatedObjectClassDef { get; set; }

        /// <summary>
        /// Gets and sets the Business Object Collection that is used to fill the combo box items.
        /// </summary>
        public IBusinessObjectCollection BusinessObjectCollection
        {
            get { return _comboBoxCollectionSelector.Collection; }
            set
            {
                CheckBusinessObjectCollectionCorrectType(value);
               _comboBoxCollectionSelector.SetCollection(value, this.IncludeBlankItem);
            }
        }

        private void CheckBusinessObjectCollectionCorrectType(IBusinessObjectCollection newBusinessObjectCollection)
        {

            if (_singleRelationship == null) return;

            RelationshipDef relationshipDef = (RelationshipDef) _singleRelationship.RelationshipDef;
            if (newBusinessObjectCollection != null && 
                relationshipDef.RelatedObjectClassType != newBusinessObjectCollection.ClassDef.ClassType)
            {
                string errorMessage = string.Format
                    ("You cannot set the Business Object Collection to the '{0}' "
                     + "since it is not of the appropriate type ('{1}') for this '{2}'",
                     newBusinessObjectCollection.ClassDef.ClassNameFull,
                     relationshipDef.RelatedObjectClassName, this.GetType().FullName);
                throw new HabaneroDeveloperException(errorMessage, errorMessage);
            }
            //if (_relationshipDef == null) return;

            //if (newBusinessObjectCollection != null && _relationshipDef.RelatedObjectClassType != newBusinessObjectCollection.ClassDef.ClassType)
            //{
            //    string errorMessage = string.Format
            //        ("You cannot set the Business Object Collection to the '{0}' "
            //         + "since it is not of the appropriate type ('{1}') for this '{2}'", newBusinessObjectCollection.ClassDef.ClassNameFull,
            //         _relationshipDef.RelatedObjectClassName, this.GetType().FullName);
            //    throw new HabaneroDeveloperException(errorMessage, errorMessage);
            //}
        }

        /// <summary>
        /// Sets the property value to that provided.  If the property value
        /// is invalid, the error provider will be given the reason why the
        /// value is invalid.
        /// </summary>
        protected virtual void SetRelatedBusinessObject(IBusinessObject value)
        {
            if (_businessObject == null) return;

            try
            {
                if (_singleRelationship != null) _singleRelationship.SetRelatedObject(value);
            }
            catch (HabaneroIncorrectTypeException)
            {
                ////TODO Brett 24 Mar 2009: Write tests and implement                this.ErrorProvider.SetError(Control, ex.Message);
                return;
            }
            UpdateErrorProviderErrorMessage();
        }

//        private void CheckRelationshipIsSingle(string relationshipName, IRelationshipDef relationshipDef)
//        {
//            if (this.ClassDef == null) return;
//
//            if (IsSingleRelationship(relationshipDef)) return;
//            string message = "The relationship '" + relationshipName + "' for the ClassDef '"
//                             + this.ClassDef.ClassNameFull + "' is not a single relationship."
//                             + " The 'RelationshipComboBoxMapper' can only be used for single relationships";
//            throw new HabaneroDeveloperException(message, message);
//        }
//
//        private static void CheckRelationshipDefined(IClassDef classDef, string relationshipName)
//        {
//            if (classDef.RelationshipDefCol.Contains(relationshipName)) return;
//            string message = "The relationship '" + relationshipName + "' does not exist on the ClassDef '"
//                             + classDef.ClassNameFull + "'";
//            throw new HabaneroDeveloperException(message, message);
//        }

//        private static bool IsSingleRelationship(IRelationshipDef relationshipDef)
//        {
//            return typeof (SingleRelationshipDef).IsInstanceOfType(relationshipDef);
//        }

        /// <summary>
        /// Returns the control being mapped
        /// </summary>
        IControlHabanero IControlMapper.Control
        {
            get { return this.Control; }
        }

        /// <summary>
        /// Gets and sets the business object that has a property
        /// being mapped by this mapper.  In other words, this property
        /// does not return the exact business object being shown in the
        /// control, but rather the business object shown in the
        /// form.  Where the business object has been amended or
        /// altered, the <see cref="UpdateControlValueFromBusinessObject"/> method is automatically called here to 
        /// implement the changes in the control itself.
        /// </summary>
        public virtual IBusinessObject BusinessObject
        {
            get { return _businessObject; }
            set
            {
                SetupRelationshipForBO(value);
                CheckBusinessObjectCorrectType(value);

                _boRelationshipMapper.BusinessObject = value;
                _businessObject = value;
                UpdateLinkedRelationshipAndControl();
                //                this.UpdateErrorProviderErrorMessage();
            }
        }

        private void UpdateLinkedRelationshipAndControl()
        {
            RemoveCurrentBOHandlers();
            SetupSingleRelationship(); //_businessObject == null ? null : GetRelationship();
            UpdateIsEditable();
            LoadCollectionForBusinessObject();
            UpdateControlValueFromBusinessObject();
            AddCurrentBOHandlers();
        }

        private void SetupSingleRelationship()
        {
            IRelationship relationship = _boRelationshipMapper.Relationship;
            if (relationship == null) _singleRelationship = null;
            else if (relationship is ISingleRelationship)
            {
                _singleRelationship = (ISingleRelationship) relationship;
                RelatedObjectClassDef = _singleRelationship.RelatedObjectClassDef;
            }
            else
            {
                string standardMessage = string.Format("The relationship '{0}' on '{1}' is not a single relationship. ",
                    RelationshipName, ClassDef.ClassNameFull);
                throw new HabaneroDeveloperException(standardMessage + "Please contact your System Administrator.",
                    standardMessage + "The 'RelationshipComboBoxMapper' can only be used for single relationships");
            }
        }

        private void OnMappedRelationshipChanged()
        {
            UpdateLinkedRelationshipAndControl();
        }

        /// <summary>
        /// Gets or sets the strategy assigned to this mapper <see cref="IComboBoxMapperStrategy"/>
        /// </summary>
        public IComboBoxMapperStrategy MapperStrategy
        {
            get { return _mapperStrategy; }
            set
            {
                _mapperStrategy = value;
                _mapperStrategy.RemoveCurrentHandlers(this);
                _mapperStrategy.AddHandlers(this);
            }
        }

        /// <summary>
        /// Provides an overrideable method for custom Loading the collection of business objects
        /// </summary>
        protected virtual void LoadCollectionForBusinessObject() { }

        private void SetupRelationshipForBO(IBusinessObject businessObject)
        {
            if (businessObject == null) return;
            if (this.ClassDef != null) return;

            this.ClassDef = businessObject.ClassDef;
            //SetUpRelationship();//This is being done by the line above
        }

//        private void SetUpRelationship()
//        {
//            CheckRelationshipDefined(this.ClassDef, this.RelationshipName);
//            _relationshipDef = this.ClassDef.RelationshipDefCol[this.RelationshipName];
//            CheckRelationshipIsSingle(this.RelationshipName, _relationshipDef);
//        }

        private void UpdateIsEditable()
        {
            if (IsReadOnly || _businessObject == null)
            {
                this.Control.Enabled = false;
                return;
            }
            this.Control.Enabled = true;
            if (IsRelationshipComposition())
            {
                if (_businessObject != null)
                {
                    this.Control.Enabled = _businessObject.Status.IsNew;
                }
            }
        }

        private bool IsRelationshipComposition()
        {
            if (_singleRelationship == null) return false;
            return _singleRelationship.RelationshipDef.RelationshipType == RelationshipType.Composition;
        }

        private void AddCurrentBOHandlers()
        {
            if (_businessObject == null) return;
            if (_singleRelationship != null)
                _singleRelationship.RelatedBusinessObjectChanged += RelatedBusinessObjectChanged_Handler;
            _businessObject.Saved += BusinessObject_OnSaved;
        }

        private void RemoveCurrentBOHandlers()
        {
            if (_businessObject == null) return;
            if (_singleRelationship != null)
                _singleRelationship.RelatedBusinessObjectChanged -= RelatedBusinessObjectChanged_Handler;
            _businessObject.Saved -= BusinessObject_OnSaved;
        }

        private void BusinessObject_OnSaved(object sender, BOEventArgs e)
        {
            UpdateIsEditable();
        }

        private void RelatedBusinessObjectChanged_Handler(object sender, EventArgs e)
        {
            UpdateControlValueFromBusinessObject();
        }

        private void CheckBusinessObjectCorrectType(IBusinessObject value)
        {
            if (value != null && !this.ClassDef.ClassType.IsInstanceOfType(value))
            {
                string errorMessage = string.Format
                    ("You cannot set the Business Object to the '{0}' identified as '{1}' "
                     + "since it is not of the appropriate type ('{2}') for this '{3}'", value.ClassDef.ClassNameFull,
                     value.ToString(), this.ClassDef.ClassNameFull, this.GetType().FullName);
                throw new HabaneroDeveloperException(errorMessage, errorMessage);
            }
        }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        public virtual void UpdateControlValueFromBusinessObject()
        {
            InternalUpdateControlValueFromBo();
            this.UpdateErrorProviderErrorMessage();
        }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        protected virtual void InternalUpdateControlValueFromBo()
        {
            IBusinessObject relatedBO = GetRelatedBusinessObject();
            if (relatedBO != null)
            {
                IComboBoxObjectCollection comboBoxObjectCollection = this.Control.Items;
                if (!comboBoxObjectCollection.Contains(relatedBO))
                {
                    comboBoxObjectCollection.Add(relatedBO);
                }
            }
            _comboBoxCollectionSelector.DeregisterForControlEvents();
            try
            {
                Control.SelectedItem = relatedBO;
            }
            finally
            {
                _comboBoxCollectionSelector.RegisterForControlEvents();
            }
        }

        /// <summary>
        /// Returns the property value of the business object being mapped
        /// </summary>
        /// <returns>Returns the property value in appropriate object form</returns>
        protected internal IBusinessObject GetRelatedBusinessObject()
        {
            return _singleRelationship == null ? null : _singleRelationship.GetRelatedObject();
        }

//        private ISingleRelationship GetRelationship()
//        {
//            IRelationship relationship = _businessObject == null
//                                             ? null
//                                             : _businessObject.Relationships[this.RelationshipName];
//            ISingleRelationship singleRelationship = null;
//            if (relationship is ISingleRelationship)
//            {
//                singleRelationship = (ISingleRelationship) relationship;
//            }
//            return singleRelationship;
//        }

        /// <summary>
        /// Sets the Error Provider Error with the appropriate value for the property e.g. if it is invalid then
        ///  sets the error provider with the invalid reason else sets the error provider with a zero length string.
        /// </summary>
        public virtual void UpdateErrorProviderErrorMessage()
        {
        }

        /// <summary>
        /// Returns the Error Provider's Error message.
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// A form field can have attributes defined in the class definition.
        /// These attributes are passed to the control mapper via a hashtable
        /// so that the control mapper can adjust its behaviour accordingly.
        /// </summary>
        /// <param name="attributes">A hashtable of attributes, which consists
        /// of name-value pairs, where name is the attribute name.  This is usually
        /// set in the XML definitions for the class's user interface.</param>
        public void SetPropertyAttributes(Hashtable attributes)
        {
            
        }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public void ApplyChangesToBusinessObject()
        {
            try
            {
                object item = this.Control.SelectedItem;
                if (item is IBusinessObject)
                {
                    RemoveCurrentBOHandlers();
                    try
                    {
                        SetRelatedBusinessObject((IBusinessObject) item);
                    }
                    finally
                    {
                        AddCurrentBOHandlers();
                    }
                }
                else
                {
                    SetRelatedBusinessObject(null);
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }
    }
}