//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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


using System.Windows.Forms;
using Habanero.UI.Forms;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Maintains an application-wide store of UI-related
    /// settings
    /// </summary>
    public class GlobalUIRegistry
    {
        private static IUISettings _uiSettings;
        private static DateDisplaySettings _dateDisplaySettings;

        /// <summary>
        /// Gets and sets the store of general user interface settings
        /// </summary>
        public static IUISettings UISettings
        {
            get { return _uiSettings; }
            set { _uiSettings = value; }
        }

        /// <summary>
        /// Gets and sets the store of date display settings
        /// </summary>
        public static DateDisplaySettings DateDisplaySettings
        {
            get { return _dateDisplaySettings; }
            set { _dateDisplaySettings = value; }
        }

        public delegate void ApplyStyleDelegate(Control control);

        private static ApplyStyleDelegate _applyStyleMethod;

        ///<summary>
        /// Set or Get the Delegate used for Applying Style changes to a control.
        ///</summary>
        public static ApplyStyleDelegate ApplyStyleMethod
        {
            get { return _applyStyleMethod; }
            set { _applyStyleMethod = value; }
        }

        ///<summary>
        /// Applies the style to the control as implemented by the ApplyStyleMethod.
        ///</summary>
        ///<param name="control">The control to apply the style to.</param>
        public static void ApplyStyle(Control control)
        {
            if (_applyStyleMethod != null)
            {
                _applyStyleMethod(control);
            }
        }
    }
}
