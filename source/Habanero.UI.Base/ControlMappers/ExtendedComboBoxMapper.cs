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
    public class ExtendedComboBoxMapper : LookupComboBoxMapper
    {
        private readonly IExtendedComboBox _extendedComboBox;
        private IFormHabanero _popupForm;
        //private IBusinessObject _relatedBusinessObject;

        ///<summary>
        /// Constructs the mapper for <see cref="IExtendedComboBox"/>.
        ///</summary>
        public ExtendedComboBoxMapper(IExtendedComboBox ctl, string propName, bool isReadOnly, IControlFactory controlFactory)
            : base(ctl.ComboBox, propName, isReadOnly, controlFactory)
        {
            _extendedComboBox = ctl;
            _extendedComboBox.Button.Click += delegate
            {
                ShowPopupForm();
                _popupForm.Closed += delegate
                    {
                        IGridAndBOEditorControl gridAndBOEditorControl =
                            (IGridAndBOEditorControl) _popupForm.Controls[0];
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
                _popupForm.ShowDialog();

            };
        }

        //public IBusinessObject RelatedBusinessObject
        //{
        //    get { return _relatedBusinessObject; }
        //    set { _relatedBusinessObject = value; }
        //}

        private void ReloadLookupValues()
        {
            Type classType;
            GetLookupTypeClassDef(out classType);
            IBusinessObjectCollection collection = GetCollection(classType);
            base.LookupList.Clear();
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
           base.SetupComboBoxItems();
        }

        ///<summary>
        /// Shows the popup form that is displayed when the button is clicked.
        /// This popup form is used to edit the <see cref="BusinessObject"/>s that fill the combobox.
        ///</summary>
        public void ShowPopupForm()
        {
            Type classType;
            ClassDef lookupTypeClassDef = GetLookupTypeClassDef(out classType);
            _popupForm = ControlFactory.CreateForm();
            _popupForm.Height = 600;
            _popupForm.Width = 800;
            
            IGridAndBOEditorControl gridAndBOEditorControl = ControlFactory.CreateGridAndBOEditorControl(lookupTypeClassDef); 
            gridAndBOEditorControl.Dock = DockStyle.Fill;
            _popupForm.Controls.Add(gridAndBOEditorControl);
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
            Type collectionType = typeof(BusinessObjectCollection<>).MakeGenericType(classType);
            IBusinessObjectCollection col = (IBusinessObjectCollection)Activator.CreateInstance(collectionType);
            col.LoadAll();
            return col;
        }

        //public LookupComboBoxMapper LookupComboBoxMapper
        //{
        //    get { return _lookupComboBoxMapper; }
        //    //internal set { _lookupComboBoxMapper = value; }
        //    set { _lookupComboBoxMapper = value; }
        //}

        ///<summary>
        /// Returns the <see cref="IExtendedComboBox"/> that is managed by this mapper.
        ///</summary>
        public new IControlHabanero Control
        {
            get { return _extendedComboBox; }
        }

        ///<summary>
        /// Returns the Popup Form.
        ///</summary>
        public IFormHabanero PopupForm
        {
            get { return _popupForm; }
        }
    }
}
