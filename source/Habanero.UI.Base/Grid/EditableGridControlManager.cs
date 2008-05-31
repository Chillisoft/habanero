using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base.Grid
{
    public class EditableGridControlManager
    {
        private readonly IEditableGridControl _gridControl;
        private string _uiDefName;
        private ClassDef _classDef;
        private GridInitialiser _gridInitialiser;


        public EditableGridControlManager(IEditableGridControl gridControl)
        {
            _gridControl = gridControl;
            _gridInitialiser = new GridInitialiser(gridControl);
        }

        public string UiDefName
        {
            get { return _uiDefName; }
            set { _uiDefName = value; }
        }

        public ClassDef ClassDef
        {
            get { return _classDef; }
            set { _classDef = value; }
        }

        public void Initialise(ClassDef classDef)
        {
            _gridInitialiser.InitialiseGrid(classDef);
        }

        public void Initialise(ClassDef classDef, string uiDefName)
        {
            _gridInitialiser.InitialiseGrid(classDef, uiDefName);
        }
    }
}
