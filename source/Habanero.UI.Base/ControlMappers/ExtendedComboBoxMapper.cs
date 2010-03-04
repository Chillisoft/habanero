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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Base
{
    ///<summary>
    /// The mapper for <see cref="IExtendedComboBox"/>.
    ///</summary>
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
    // ReSharper disable MemberCanBePrivate.Global
    public class ExtendedComboBoxMapper : ControlMapper //: LookupComboBoxMapper
    {
        protected IExtendedComboBox ExtendedComboBox { get; set; }
        //private IBusinessObject _relatedBusinessObject;
        protected LookupComboBoxMapper LookupComboBoxMapper { get; set; }

        ///<summary>
        /// Constructs the mapper for <see cref="IExtendedComboBox"/>.
        ///</summary>
        public ExtendedComboBoxMapper
            (IExtendedComboBox ctl, string propName, bool isReadOnly, IControlFactory controlFactory)
            : base(ctl, propName, isReadOnly, controlFactory)
        {
            ExtendedComboBox = ctl;
            LookupComboBoxMapper = new LookupComboBoxMapper
                (ExtendedComboBox.ComboBox, propName, isReadOnly, controlFactory);
            ExtendedComboBox.Button.Click += delegate
                {
                    ShowPopupForm();
                    PopupForm.Closed += HandlePopUpFormClosedEvent;
                 
                    PopupForm.ShowDialog();
                };
        }
        /// <summary>
        /// Handles the Closing of the Popup form.
        /// By default this saves the Business Object that is currently selectedin the Popup  (if there is one)
        /// Reloads the Combo Box using <see cref="ReloadLookupValues"/>.
        /// and Sets the Currently selected Business Object as the selected Item for the ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void HandlePopUpFormClosedEvent(object sender, EventArgs e)
        {
            IBOGridAndEditorControl iboGridAndEditorControl = GetIBOGridAndEditorControl();
            IBusinessObject currentBusinessObject = iboGridAndEditorControl.CurrentBusinessObject;
            if ((currentBusinessObject != null) && currentBusinessObject.IsValid())
            {
                currentBusinessObject.Save();
            }
            ReloadLookupValues();
            if (currentBusinessObject != null)
            {
                ExtendedComboBox.ComboBox.SelectedValue = currentBusinessObject.ID.GetAsValue().ToString();
            }
        }

        protected virtual IBOGridAndEditorControl GetIBOGridAndEditorControl()
        {
            return (IBOGridAndEditorControl) PopupForm.Controls[0];
        }
        /// <summary>
        /// Reloads the ComboBox.
        /// </summary>
        protected virtual void ReloadLookupValues()
        {
            Type classType;
            GetLookupTypeClassDef(out classType);
            IBusinessObjectCollection collection = GetCollection(classType);
            LookupComboBoxMapper.LookupList.Clear();
            Dictionary<string, string> lookupValues = new Dictionary<string, string>();
            foreach (IBusinessObject businessObject in collection)
            {
                string toString = businessObject.ToString();
                while (lookupValues.ContainsKey(toString))
                {
                    toString += " ";
                }
                lookupValues.Add(toString, businessObject.ID.GetAsValue().ToString());
            }
            LookupComboBoxMapper.SetupComboBoxItems();
        }

        ///<summary>
        /// Shows the popup form that is displayed when the button is clicked.
        /// This popup form is used to edit the <see cref="BusinessObject"/>s that fill the combobox.
        ///</summary>
        public virtual void ShowPopupForm()
        {
            Type classType;
            IClassDef lookupTypeClassDef = GetLookupTypeClassDef(out classType);
            PopupForm = ControlFactory.CreateForm();
            PopupForm.Height = 600;
            PopupForm.Width = 800;

            IBOGridAndEditorControl iboGridAndEditorControl = ControlFactory.CreateGridAndBOEditorControl
                (lookupTypeClassDef);
            iboGridAndEditorControl.Dock = DockStyle.Fill;
            PopupForm.Controls.Add(iboGridAndEditorControl);
            IBusinessObjectCollection col = GetCollection(classType);
            iboGridAndEditorControl.BusinessObjectCollection = col;
        }

        private IClassDef GetLookupTypeClassDef(out Type classType)
        {
            BOMapper mapper = new BOMapper(BusinessObject);
            IClassDef lookupTypeClassDef = mapper.GetLookupListClassDef(PropertyName);
            classType = lookupTypeClassDef.ClassType;
            return lookupTypeClassDef;
        }

        private static IBusinessObjectCollection GetCollection(Type classType)
        {
            Type collectionType = typeof (BusinessObjectCollection<>).MakeGenericType(classType);
            IBusinessObjectCollection col = (IBusinessObjectCollection) Activator.CreateInstance(collectionType);
            col.LoadAll();
            return col;
        }

        ///<summary>
        /// Returns the Popup Form.
        ///</summary>
        public IFormHabanero PopupForm { get; protected set; }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public override void ApplyChangesToBusinessObject()
        {
            LookupComboBoxMapper.ApplyChangesToBusinessObject();
        }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        public override void UpdateControlValueFromBusinessObject()
        {
            LookupComboBoxMapper.UpdateControlValueFromBusinessObject();
        }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        protected override void InternalUpdateControlValueFromBo()
        {
            LookupComboBoxMapper.DoUpdateControlValueFromBO();
        }

        /// <summary>
        /// Gets and sets the business object that has a property
        /// being mapped by this mapper.  In other words, this property
        /// does not return the exact business object being shown in the
        /// control, but rather the business object shown in the
        /// form.  Where the business object has been amended or
        /// altered, the <see cref="ControlMapper.UpdateControlValueFromBusinessObject"/> method is automatically called here to 
        /// implement the changes in the control itself.
        /// </summary>
        public override IBusinessObject BusinessObject
        {
            get { return base.BusinessObject; }
            set
            {
                LookupComboBoxMapper.BusinessObject = value;
                base.BusinessObject = value;
            }
        }
        /// <summary>
        /// Gets and sets the lookup list used to populate the items in the
        /// ComboBox.  This method is typically called by SetupLookupList().
        /// </summary>
        public Dictionary<string, string> LookupList
        {
            get { return LookupComboBoxMapper.LookupList; }
            set { LookupComboBoxMapper.LookupList = value; }
        }
    }
    // ReSharper restore ClassWithVirtualMembersNeverInherited.Global
    // ReSharper restore MemberCanBePrivate.Global

}