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

using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    public interface IPanelFactoryInfo
    {
        /// <summary>
        /// Returns the panel object
        /// </summary>
        IPanel Panel { get; }

        /// <summary>
        /// Returns the collection of control mappers
        /// </summary>
        IControlMapperCollection ControlMappers { get; }

        /// <summary>
        /// Gets and sets the preferred height setting
        /// </summary>
        int PreferredHeight { get; set; }

        /// <summary>
        /// Gets and sets the preferred width setting
        /// </summary>
        int PreferredWidth { get; set; }

        /// <summary>
        /// Returns the first control to focus on in the user interface
        /// </summary>
        IControlChilli FirstControlToFocus { get; }

        IToolTip ToolTip { get; set; }

        int MinimumPanelHeight { get; set; }

        int MinumumPanelWidth { get; set; }

        string PanelTabText { get; set; }

        UIForm UIForm { get; set; }

        UIFormTab UiFormTab { get; set; }

        string UIDefName { get; }
    }
}