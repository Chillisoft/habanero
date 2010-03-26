// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
namespace Habanero.UI.Base
{
    /// <summary>
    /// This is a Main Editor Panel that consists of a Header control that can be styled and takes an Icon and a Title.
    /// </summary>
    public interface IMainEditorPanel: IPanel
    {
        /// <summary>
        /// The Control that is positioned at the top of this panel that can be used to set an icon and title for the
        ///  information being displayed on the MainEditorPanelVWG
        /// </summary>
        IMainTitleIconControl MainTitleIconControl { get; }

        /// <summary>
        /// The Panel in which the control being set is placed in.
        /// </summary>
        IPanel EditorPanel { get; }
    }
}