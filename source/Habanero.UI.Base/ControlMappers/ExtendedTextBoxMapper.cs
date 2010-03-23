using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base.ControlMappers;

namespace Habanero.UI.Base
{
    ///<summary>
    /// The mapper for <see cref="IExtendedTextBox"/>.
    ///</summary>
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
    // ReSharper disable MemberCanBePrivate.Global
    public class ExtendedTextBoxMapper : ControlMapper //: LookupComboBoxMapper
    {
        protected IExtendedTextBox ExtendedTextBox { get; set; }
        protected LookupComboBoxMapper LookupComboBoxMapper { get; set; }

        ///<summary>
        /// Constructs the mapper for <see cref="IExtendedComboBox"/>.
        ///</summary>
        public ExtendedTextBoxMapper
            (IExtendedTextBox ctl, string propName, bool isReadOnly, IControlFactory controlFactory)
            : base(ctl, propName, isReadOnly, controlFactory)
        {
            ExtendedTextBox = ctl;
            //The use of a LookupComboBoxMapper may seem strange but it is being used to simplify the 
            // code for ApplyChangesToBusinessObject etc.
            LookupComboBoxMapper = new LookupComboBoxMapper
                (controlFactory.CreateComboBox(), propName, isReadOnly, controlFactory);
            ExtendedTextBox.Button.Click += delegate
                     {
                         ShowPopupForm();
                         PopupForm.Closed += HandlePopUpFormClosedEvent;
                         //TODO brett 23 Mar 2010: This will not work in VWG.
                         PopupForm.ShowDialog();
                     };
        }
        /// <summary>
        /// Handles the Closing of the Popup form.
        /// By default this saves the Business Object that is currently selectedin the Popup  (if there is one)
        /// and Sets the Currently selected Business Object.ToString as the text of the TextBox
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
            if (currentBusinessObject != null)
            {
                ExtendedTextBox.TextBox.Text = currentBusinessObject.ToString();
            }
        }

        protected virtual IBOGridAndEditorControl GetIBOGridAndEditorControl()
        {
            return (IBOGridAndEditorControl) PopupForm.Controls[0];
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
            this.LookupComboBoxMapper.ApplyChangesToBusinessObject();
        }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        public override void UpdateControlValueFromBusinessObject()
        {
            this.LookupComboBoxMapper.UpdateControlValueFromBusinessObject();
        }

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="IControlMapper.BusinessObject"/>
        /// </summary>
        protected override void InternalUpdateControlValueFromBo()
        {
            this.LookupComboBoxMapper.DoUpdateControlValueFromBO();
        }
        /// <summary>
        /// Gets and sets the lookup list used to populate the items in the
        /// ComboBox.  This method is typically called by SetupLookupList().
        /// </summary>
        public Dictionary<string, string> LookupList
        {
            get { return this.LookupComboBoxMapper.LookupList; }
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
                this.LookupComboBoxMapper.BusinessObject = value;
                base.BusinessObject = value;
            }
        }
    }
}