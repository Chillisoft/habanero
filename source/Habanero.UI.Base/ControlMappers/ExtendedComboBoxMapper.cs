using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    public class ExtendedComboBoxMapper:LookupComboBoxMapper //<TBusinessObject> : LookupComboBoxMapper where TBusinessObject : class, IBusinessObject
    {
        private readonly IControlFactory _controlFactory;
        private LookupComboBoxMapper _lookupComboBoxMapper;
        private IExtendedComboBox _extendedComboBox;
        private IFormHabanero _popupForm;
        private IBusinessObject _relatedBusinessObject;

        public ExtendedComboBoxMapper(IExtendedComboBox ctl, string propName, bool isReadOnly, IControlFactory controlFactory)
            : base(ctl.ComboBox, propName, isReadOnly, controlFactory)
        {
            _controlFactory = controlFactory;
            _extendedComboBox = ctl;
          //  _lookupComboBoxMapper = base.;
            _extendedComboBox.Button.Click += delegate
            {
                ShowPopupForm();
                _popupForm.Closed += delegate
                                         {
                                             IGridAndBOEditorControl gridAndBOEditorControl = (IGridAndBOEditorControl)_popupForm.Controls[0];
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
        public IBusinessObject RelatedBusinessObject
        {
            get { return _relatedBusinessObject; }
            set { _relatedBusinessObject = value; }
        }

        private void ReloadLookupValues()
        {
            Type classType;
            GetLookupTypeClassDef(out classType);
            IBusinessObjectCollection collection = GetCollection(classType);
            base.LookupList.Clear();
            //base.SetupComboBoxItems();
           // Dictionary<string, object> lookupValues = new Dictionary<string, object>();
            Dictionary<string, object> displayValueDictionary = BusinessObjectLookupList.CreateDisplayValueDictionary(collection, true);
            //foreach (IBusinessObject businessObject in collection)
            //{
            //    string toString = businessObject.ToString();
            //    while (lookupValues.ContainsKey(toString))
            //    {
            //        toString += " ";
            //    }
            //    lookupValues.Add(toString, businessObject);
            //}
           // base.SetupComboBoxItems();

            base.LookupList = displayValueDictionary;
        }

        public void ShowPopupForm()
        {
            //IDefaultBOEditorForm defaultBOEditorForm = new DefaultBOEditorStub((BusinessObject) this.BusinessObject,"default",controlFactory);
            Type classType;
            ClassDef lookupTypeClassDef = GetLookupTypeClassDef(out classType);
            _popupForm = ControlFactory.CreateForm();
            _popupForm.Height = 600;
            _popupForm.Width = 800;
            
            Type classSpecificControlType = typeof(IGridAndBOEditorControl);
            IBusinessObjectControlWithErrorDisplay boEditorPanel = _controlFactory.CreateBOEditorForm(lookupTypeClassDef, "default", ControlFactory);
            //if (RelatedBusinessObject == null) throw new NullReferenceException("RealtedBusinessObject is null");
            IGridAndBOEditorControl gridAndBOEditorControl = ControlFactory.CreateGridAndBOEditorControl(lookupTypeClassDef); //(IGridAndBOEditorControl) Activator.CreateInstance(classSpecificControlType, ControlFactory, boEditorPanel); //ControlFactory.CreateGridAndBOEditorControl(boEditorPanel);
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

        private IBusinessObjectCollection GetCollection(Type classType)
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

        public IControlHabanero Control
        {
            get { return _extendedComboBox; }
        }

        //public string PropertyName
        //{
        //    get { return _lookupComboBoxMapper.PropertyName; }
        //}

        //public IBusinessObject BusinessObject
        //{
        //    get { return _lookupComboBoxMapper.BusinessObject; }
        //    set { _lookupComboBoxMapper.BusinessObject = value; }
        //}

        //public IErrorProvider ErrorProvider
        //{
        //    get { return _lookupComboBoxMapper.ErrorProvider; }
        //}

        //public bool IsReadOnly
        //{
        //    get { return _lookupComboBoxMapper.IsReadOnly; }
        //}

        //public IControlFactory ControlFactory
        //{
        //    get { return _lookupComboBoxMapper.ControlFactory; }
        //}

        public IFormHabanero PopupForm
        {
            get { return _popupForm; }
        }



        //public void UpdateControlValueFromBusinessObject()
        //{
        //    _lookupComboBoxMapper.UpdateControlValueFromBusinessObject();
        //}
    }
}
