using System;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Base.Grid;

namespace Habanero.UI.WebGUI
{
    public class EditableGridControlGiz : UserControlGiz, IEditableGridControl
    {
        private readonly IControlFactory _controlFactory;
        private readonly IEditableGrid _grid;
        private readonly EditableGridControlManager _editableGridManager;

        public EditableGridControlGiz(IControlFactory controlFactory)
        {
            if (controlFactory == null) throw new HabaneroArgumentException("controlFactory", 
                    "Cannot create an editable grid control if the control factory is null");
            _controlFactory = controlFactory;
            _editableGridManager = new EditableGridControlManager(this, controlFactory);
            _grid = _controlFactory.CreateEditableGrid();
            BorderLayoutManager manager = controlFactory.CreateBorderLayoutManager(this);
            manager.AddControl(_grid, BorderLayoutManager.Position.Centre);
        }

        public IGridBase Grid
        {
            get { return _grid; }
        }

        public void Initialise(IClassDef classDef)
        {
            _editableGridManager.Initialise(classDef);
        }


        public void Initialise(IClassDef classDef, string uiDefName)
        {
            _editableGridManager.Initialise(classDef, uiDefName);
        }


        public string UiDefName
        {
            get { return _editableGridManager.UiDefName;  }
            set { _editableGridManager.UiDefName = value; }
        }

        public IClassDef ClassDef
        {
            get { return _editableGridManager.ClassDef; }
            set { _editableGridManager.ClassDef = (ClassDef) value; }
        }

        /// <summary>
        /// Sets the business object collection to display.  Loading of
        /// the collection needs to be done before it is assigned to the
        /// grid.  This method assumes a default ui definition is to be
        /// used, that is a 'ui' element without a 'name' attribute.
        /// </summary>
        /// <param name="boCollection">The new business object collection
        /// to be shown in the grid</param>
        public void SetBusinessObjectCollection(IBusinessObjectCollection boCollection)
        {
            if (boCollection == null)
            {
                _grid.SetBusinessObjectCollection(null);
                return;
            }
            //if (this.ClassDef == null)
            //{
            //    Initialise(boCollection.ClassDef);
            //}
            //else
            //{
            //    if (this.ClassDef != boCollection.ClassDef)
            //    {
            //        throw new ArgumentException(
            //            "You cannot call set collection for a collection that has a different class def than is initialised");
            //    }
            //}
            //if (this.BusinessObjectCreator is DefaultBOCreator)
            //{
            //    this.BusinessObjectCreator = new DefaultBOCreator(boCollection);
            //}
            //if (this.BusinessObjectCreator == null) this.BusinessObjectCreator = new DefaultBOCreator(boCollection);
            //if (this.BusinessObjectEditor == null) this.BusinessObjectEditor = new DefaultBOEditor(_controlFactory);
            //if (this.BusinessObjectDeletor == null) this.BusinessObjectDeletor = new DefaultBODeletor();

            _grid.SetBusinessObjectCollection(boCollection);

            //this.Buttons.Enabled = true;
            //this.FilterControl.Enabled = true;
        }
    }
}