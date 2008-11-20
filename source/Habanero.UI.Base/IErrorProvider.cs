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
    /// <summary>
    /// Specifies constants indicating when the error icon, supplied by an ErrorProvider,
    /// should blink to alert the user that an error has occurred
    /// </summary>
    public enum ErrorBlinkStyleHabanero
    {
        /// <summary>
        /// Always blink when the error icon is first displayed, or when a error description 
        /// string is set for the control and the error icon is already displayed.
        /// </summary>
        AlwaysBlink = 1,
        /// <summary>
        /// Blinks when the icon is already displayed and a new error string is set for the control.
        /// </summary>
        BlinkIfDifferentError = 0,
        /// <summary>
        /// Never blink the error icon.
        /// </summary>
        NeverBlink = 2
    }


    /// <summary>
    /// Specifies constants indicating the locations that an error icon can appear in relation to the control with an error.
    /// </summary>
    //[Serializable()]
    public enum ErrorIconAlignmentHabanero
    {
        /// <summary>
        /// The icon appears aligned with the bottom of the control and the left of the control.
        /// </summary>
        BottomLeft = 4,

        /// <summary>
        /// The icon appears aligned with the bottom of the control and the right of the control.
        /// </summary>
        BottomRight = 5,

        /// <summary>
        /// The icon appears aligned with the middle of the control and the left of the control.
        /// </summary>
        MiddleLeft = 2,

        /// <summary>
        /// The icon appears aligned with the middle of the control and the right of the control.
        /// </summary>
        MiddleRight = 3,

        /// <summary>
        /// The icon appears aligned with the top of the control and to the left of the control.
        /// </summary>
        TopLeft = 0,

        /// <summary>
        /// The icon appears aligned with the top of the control and to the right of the control.
        /// </summary>
        TopRight = 1
    }

    /// <summary>
    /// Provides a user interface for indicating that a control on a form has an error associated with it
    /// </summary>
    public interface IErrorProvider 
    {
        /// <summary>
        /// Returns the current error description string for the specified control.
        /// </summary>
        ///	<returns>The error description string for the specified control.</returns>
        ///	<param name="objControl">The item to get the error description string for. </param>
        ///	<exception cref="T:System.ArgumentNullException">control is null.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        string GetError(IControlHabanero objControl);

        /// <summary>
        /// Gets a value indicating where the error icon should be placed in relation to the control.
        /// </summary>
        ///	<returns>One of the <see cref="T:Habanero.UI.Base.ErrorIconAlignmentHabanero"></see> values. The default icon alignment is <see cref="F:Habanero.UI.Base.ErrorIconAlignmentHabanero.MiddleRight"></see>.</returns>
        ///	<param name="objControl">The control to get the icon location for. </param>
        ///	<exception cref="T:System.ArgumentNullException">control is null.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        ErrorIconAlignmentHabanero GetIconAlignment(IControlHabanero objControl);

        /// <summary>
        /// Returns the amount of extra space to leave next to the error icon.
        /// </summary>
        /// <returns>The number of pixels to leave between the icon and the control. </returns>
        /// <param name="objControl">The control to get the padding for. </param>
        /// <exception cref="T:System.ArgumentNullException">control is null.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        int GetIconPadding(IControlHabanero objControl);

        /// <summary>
        /// Sets the error description string for the specified control.
        /// </summary>
        /// <param name="objControl">The control to set the error description string for. </param>
        /// <param name="strValue">The error description string. </param>
        /// <exception cref="T:System.ArgumentNullException">control is null.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        void SetError(IControlHabanero objControl, string strValue);

        /// <summary>
        /// Sets the location where the error icon should be placed in relation to the control.
        /// </summary>
        /// <param name="objControl">The control to set the icon location for. </param>
        /// <param name="enmValue">One of the <see cref="T:Habanero.UI.Base.ErrorIconAlignmentHabanero"/> values. </param>
        /// <exception cref="T:System.ArgumentNullException">control is null.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        void SetIconAlignment(IControlHabanero objControl, ErrorIconAlignmentHabanero enmValue);

        /// <summary>
        /// Sets the amount of extra space to leave between the specified control and the error icon.
        /// </summary>
        /// <param name="objControl">The control to set the padding for. </param>
        /// <param name="intPadding">The number of pixels to add between the icon and the control. </param>
        /// <exception cref="T:System.ArgumentNullException">control is null.</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        void SetIconPadding(IControlHabanero objControl, int intPadding);

        /// <summary>
        /// Provides a method to update the bindings of the <see cref="P:Habanero.UI.Base.IErrorProvider.DataSource"/>, <see cref="P:Habanero.UI.Base.IErrorProvider.DataMember"/>, and the error text.
        /// </summary>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        void UpdateBinding();

        /// <summary>
        /// Gets a value indicating whether a control can be extended.
        /// </summary>
        /// <returns>true if the control can be extended; otherwise, false.This property will be true if the object is a <see cref="T:Habanero.UI.Base.IControlHabanero"/>.</returns>
        /// <param name="objExtendee">The control to be extended. </param>
        bool CanExtend(object objExtendee);

        /// <summary>
        /// Gets or sets the rate at which the error icon flashes.
        /// </summary>
        /// <returns>The rate, in milliseconds, at which the error icon should flash. The default is 250 milliseconds.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than zero. </exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        int BlinkRate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating when the error icon flashes.
        /// </summary>
        /// <returns>One of the <see cref="T:Habanero.UI.Base.ErrorBlinkStyleHabanero"/> values. The default is <see cref="F:Habanero.UI.Base.ErrorBlinkStyleHabanero.BlinkIfDifferentError"/>.</returns>
        /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:Habanero.UI.Base.ErrorBlinkStyleHabanero"/> values. </exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        ErrorBlinkStyleHabanero BlinkStyleHabanero { get; set; }

        /// <summary>
        /// Gets or sets the list within a data source to monitor.
        /// </summary>
        /// <returns>The string that represents a list within the data source specified by the <see cref="P:Habanero.UI.Base.IErrorProvider.DataSource"/> to be monitored. Typically, this will be a <see cref="T:System.Data.DataTable"/>.</returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        string DataMember { get; set; }

        /// <summary>
        /// Gets or sets the data source that the <see cref="T:Habanero.UI.Base.IErrorProvider"/> monitors.
        /// </summary>
        /// <returns>A data source based on the <see cref="T:System.Collections.IList"/> interface to be monitored for errors. Typically, this is a <see cref="T:System.Data.DataSet"/> to be monitored for errors.</returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        object DataSource { get; set; }
    }
}