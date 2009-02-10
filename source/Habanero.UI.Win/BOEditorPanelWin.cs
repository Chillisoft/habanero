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

using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{

    public class BOEditorPanelWin : UserControlWin, IBusinessObjectControlWithErrorDisplay, IBOEditorPanel
    {
        private readonly IPanelInfo _panelInfo;

        public BOEditorPanelWin(ClassDef classDef, string uiDefName, IControlFactory controlFactory)
        {
            PanelBuilder panelBuilder = new PanelBuilder(controlFactory);
            UIDef uiDef = classDef.GetUIDef(uiDefName);
            if (uiDef == null) { throw new HabaneroDeveloperException(string.Format("No UI Definition is defined for '{0}' called '{1}'.", classDef.ClassName, uiDefName), "Please check the Class definitions."); }
            UIForm uiForm = uiDef.UIForm;
            _panelInfo = panelBuilder.BuildPanelForForm(uiForm);
            IPanel panel = _panelInfo.Panel;
            BorderLayoutManager borderLayoutManager = controlFactory.CreateBorderLayoutManager(this);
            borderLayoutManager.AddControl(panel, BorderLayoutManager.Position.Centre);
        }

        public IBusinessObject BusinessObject
        {
            get { return _panelInfo.BusinessObject; }
            set { _panelInfo.BusinessObject = value; }
        }

        public void DisplayErrors()
        {
            _panelInfo.ApplyChangesToBusinessObject();
        }

        public void ClearErrors()
        {
            _panelInfo.ClearErrorProviders();
        }
    }
}