using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;

namespace Habanero.UI.Win
{
    public class ReadOnlyGridControlWin : ControlWin, IReadOnlyGridControl
    {
        private IBusinessObjectEditor _BusinessObjectEditor;
        private IBusinessObjectCreator _BusinessObjectCreator;
        private IBusinessObjectDeletor _businessObjectDeletor;
        private ClassDef _classDef;
        private IFilterControl _filterControl;

        /// <summary>
        /// initiliase the grid to the with the 'default' UIdef.
        /// </summary>
        public void Initialise(ClassDef classDef)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyGrid Grid
        {
            get { return null; }
        }

        public BusinessObject SelectedBusinessObject
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Returns the button control held. This property can be used
        /// to access a range of functionality for the button control
        /// (eg. myGridWithButtons.Buttons.AddButton(...)).
        /// </summary>
        public IReadOnlyGridButtonsControl Buttons
        {
            get { throw new System.NotImplementedException(); }
        }

        public IBusinessObjectEditor BusinessObjectEditor
        {
            get { return _BusinessObjectEditor; }
            set { _BusinessObjectEditor = value; }
        }

        public IBusinessObjectCreator BusinessObjectCreator
        {
            get { return _BusinessObjectCreator; }
            set { _BusinessObjectCreator = value; }
        }

        public IBusinessObjectDeletor BusinessObjectDeletor
        {
            get { return _businessObjectDeletor; }
            set { _businessObjectDeletor = value; }
        }

        public string UiDefName
        {
            get { return ""; }
        }

        public ClassDef ClassDef
        {
            get { return _classDef; }
        }

        public IFilterControl FilterControl
        {
            get { return _filterControl; }
        }


        public void SetBusinessObjectCollection(IBusinessObjectCollection boCollection)
        {
            throw new System.NotImplementedException();
        }

        public void Initialise(ClassDef def, string uiDefName)
        {
            throw new System.NotImplementedException();
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }
    }
}