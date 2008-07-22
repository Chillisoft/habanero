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

namespace Habanero.UI.Base
{
    public interface IDefaultBOEditorForm: IFormChilli
    {
        /// <summary>
        /// Returns the button control for the buttons in the form
        /// </summary>
        IButtonGroupControl Buttons
        {
            get;
        }

        

        /// <summary>
        /// Pops the form up in a modal dialog.  If the BO is successfully edited and saved, returns true
        /// else returns false.
        /// </summary>
        /// <returns>True if the edit was a success, false if not</returns>
        bool ShowDialog();

        void Show();
        void Dispose();

        DialogResult DialogResult { get; set;}

        IPanelFactoryInfo PanelFactoryInfo{ get;}
    }
}