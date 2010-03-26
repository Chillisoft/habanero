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
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a user interface for indicating that a control on a form has an error associated with it
    /// </summary>
    public class ErrorProviderWin : ErrorProvider, IErrorProvider
    {
        /// <summary>
        /// Returns the current error description string for the specified control.
        /// </summary>
        ///	<returns>The error description string for the specified control.</returns>
        ///	<param name="objControl">The item to get the error description string for. </param>
        ///	<exception cref="T:System.ArgumentNullException">control is null.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        public string GetError(IControlHabanero objControl)
        {
            return base.GetError((Control) objControl);
        }

        /// <summary>
        /// Gets a value indicating where the error icon should be placed in relation to the control.
        /// </summary>
        ///	<returns>One of the <see cref="T:Habanero.UI.Base.ErrorIconAlignmentHabanero"></see> values. The default icon alignment is <see cref="F:Habanero.UI.Base.ErrorIconAlignmentHabanero.MiddleRight"></see>.</returns>
        ///	<param name="objControl">The control to get the icon location for. </param>
        ///	<exception cref="T:System.ArgumentNullException">control is null.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        public ErrorIconAlignmentHabanero GetIconAlignment(IControlHabanero objControl)
        {
            return (ErrorIconAlignmentHabanero) base.GetIconAlignment((Control) objControl);
        }

        /// <summary>
        /// Returns the amount of extra space to leave next to the error icon.
        /// </summary>
        /// <returns>The number of pixels to leave between the icon and the control. </returns>
        /// <param name="objControl">The control to get the padding for. </param>
        /// <exception cref="T:System.ArgumentNullException">control is null.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        public int GetIconPadding(IControlHabanero objControl)
        {
            return base.GetIconPadding((Control) objControl);
        }

        /// <summary>
        /// Sets the error description string for the specified control.
        /// </summary>
        /// <param name="objControl">The control to set the error description string for. </param>
        /// <param name="strValue">The error description string. </param>
        /// <exception cref="T:System.ArgumentNullException">control is null.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        public void SetError(IControlHabanero objControl, string strValue)
        {
            base.SetError((Control) objControl, strValue);
        }

        /// <summary>
        /// Sets the location where the error icon should be placed in relation to the control.
        /// </summary>
        /// <param name="objControl">The control to set the icon location for. </param>
        /// <param name="enmValue">One of the <see cref="T:Habanero.UI.Base.ErrorIconAlignmentHabanero"/> values. </param>
        /// <exception cref="T:System.ArgumentNullException">control is null.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        public void SetIconAlignment(IControlHabanero objControl, ErrorIconAlignmentHabanero enmValue)
        {
            base.SetIconAlignment((Control) objControl, (ErrorIconAlignment) enmValue);
        }

        /// <summary>
        /// Sets the amount of extra space to leave between the specified control and the error icon.
        /// </summary>
        /// <param name="objControl">The control to set the padding for. </param>
        /// <param name="intPadding">The number of pixels to add between the icon and the control. </param>
        /// <exception cref="T:System.ArgumentNullException">control is null.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        public void SetIconPadding(IControlHabanero objControl, int intPadding)
        {
            base.SetIconPadding((Control) objControl, intPadding);
        }

        /// <summary>
        /// Gets or sets a value indicating when the error icon flashes.
        /// </summary>
        /// <returns>One of the <see cref="T:Habanero.UI.Base.ErrorBlinkStyleHabanero"/> values. The default is <see cref="F:Habanero.UI.Base.ErrorBlinkStyleHabanero.BlinkIfDifferentError"/>.</returns>
        /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:Habanero.UI.Base.ErrorBlinkStyleHabanero"/> values. </exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        public ErrorBlinkStyleHabanero BlinkStyleHabanero
        {
            get { return (ErrorBlinkStyleHabanero) base.BlinkStyle; }
            set { base.BlinkStyle = (ErrorBlinkStyle) value; }
        }
    }
}