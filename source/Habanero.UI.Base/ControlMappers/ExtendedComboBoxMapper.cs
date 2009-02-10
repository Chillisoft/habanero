//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    ///<summary>
    /// The mapper for <see cref="IExtendedComboBox"/>.
    ///</summary>
    public class ExtendedComboBoxMapper : ControlMapper //: LookupComboBoxMapper
    {
        private readonly IExtendedComboBox _extendedComboBox;
        //private IBusinessObject _relatedBusinessObject;
        private readonly LookupComboBoxMapper _lookupComboBoxMapper;

        ///<summary>
        /// Constructs the mapper for <see cref="IExtendedComboBox"/>.
        ///</summary>
        public ExtendedComboBoxMapper
            (IExtendedComboBox ctl, string propName, bool isReadOnly, IControlFactory controlFactory)
            : base(ctl, propName, isReadOnly, controlFactory)
        {
            _extendedComboBox = ctl;
            _lookupComboBoxMapper = new LookupComboBoxMapper
                (_extendedComboBox.ComboBox, propName, isReadOnly, controlFactory);
            _extendedComboBox.Button.Click += delegate
                {
                    ShowPopupForm();
                    PopupForm.Closed += delegate
                        {
                            IGridAndBOEditorControl gridAndBOEditorControl =
                                (IGridAndBOEditorControl) PopupForm.Controls[0];
                            IBusinessObject currentBusinessObject = gridAndBOEditorControl.CurrentBusinessObject;
                            if ((currentBusinessObject != null) && currentBusinessObject.IsValid())
                            {
                                currentBusinessObject.Save();
                            }
                            ReloadLookupValues();
                            if (currentBusinessObject != null)
                            {
                                _extendedComboBox.ComboBox.SelectedValue = currentBusinessObject;
                            }
                        };
                    PopupForm.ShowDialog();
                };
        }

        private void ReloadLookupValues()
        {
            Type classType;
            GetLookupTypeClassDef(out classType);
            IBusinessObjectCollection collection = GetCollection(classType);
            _lookupComboBoxMapper.LookupList.Clear();
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
            _lookupComboBoxMapper.SetupComboBoxItems();
        }

        ///<summary>
        /// Shows the popup form that is displayed when the button is clicked.
        /// This popup form is used to edit the <see cref="BusinessObject"/>s that fill the combobox.
        ///</summary>
        public void ShowPopupForm()
        {
            Type classType;
            ClassDef lookupTypeClassDef = GetLookupTypeClassDef(out classType);
            PopupForm = ControlFactory.CreateForm();
            PopupForm.Height = 600;
            PopupForm.Width = 800;

            IGridAndBOEditorControl gridAndBOEditorControl = ControlFactory.CreateGridAndBOEditorControl
                (lookupTypeClassDef);
            gridAndBOEditorControl.Dock = DockStyle.Fill;
            PopupForm.Controls.Add(gridAndBOEditorControl);
            IBusinessObjectCollection col = GetCollection(classType);
            gridAndBOEditorControl.SetBusinessObjectCollection(col);
        }

        private ClassDef GetLookupTypeClassDef(out Type classType)
        {
            BOMapper mapper = new BOMapper(BusinessObject);
            ClassDef lookupTypeClassDef = mapper.GetLookupListClassDef(PropertyName);
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
        public IFormHabanero PopupForm { get; private set; }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public override void ApplyChangesToBusinessObject()
        {
            _lookupComboBoxMapper.ApplyChangesToBusinessObject();
        }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        public override void UpdateControlValueFromBusinessObject()
        {
            _lookupComboBoxMapper.UpdateControlValueFromBusinessObject();
        }

        protected override void InternalUpdateControlValueFromBo()
        {
            _lookupComboBoxMapper.DoUpdateControlValueFromBO();
        }

        public override IBusinessObject BusinessObject
        {
            get { return base.BusinessObject; }
            set
            {
                _lookupComboBoxMapper.BusinessObject = value;
                base.BusinessObject = value;
            }
        }
        /// <summary>
        /// Gets and sets the lookup list used to populate the items in the
        /// ComboBox.  This method is typically called by SetupLookupList().
        /// </summary>
        public Dictionary<string, string> LookupList
        {
            get { return _lookupComboBoxMapper.LookupList; }
        }
    }
}