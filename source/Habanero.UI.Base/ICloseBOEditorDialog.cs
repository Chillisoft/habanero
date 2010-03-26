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
    /// The Result returned from the <see cref="CloseBOEditorDialogWin"/>
    /// </summary>
    public enum CloseBOEditorDialogResult
    {
        ///<summary>
        /// Cancel Closing of the Origional form.
        ///</summary>
        CancelClose,
        /// <summary>
        /// Save the BusinessObject(s) and then Close the Form.
        /// </summary>
        SaveAndClose,
        /// <summary>
        /// Close the form and lose all changes to the Busienss Object(s).
        /// </summary>
        CloseWithoutSaving
    }
    /// <summary>
    /// This the interface for a Dialog Box that is specialiased for dealing with the
    /// Closing of any form or application that is editing Business Objects.
    /// The dialogue box will display a sensible message to the user to determine
    /// whether they want to Close the Origional form without saving, Save the BO and then
    /// Close or Cancel the Closing of the origional form.
    /// </summary>
    public interface ICloseBOEditorDialog:IFormHabanero
    {
        /// <summary>
        /// The CancelClose Button.
        /// </summary>
        IButton CancelCloseBtn { get; }

        /// <summary>
        /// The Save and Close Button.
        /// </summary>
        IButton SaveAndCloseBtn { get; }

        /// <summary>
        /// The Close without saving Button.
        /// </summary>
        IButton CloseWithoutSavingBtn { get; }

        /// <summary>
        /// The Result from this Form.
        /// </summary>
        CloseBOEditorDialogResult BOEditorDialogResult { get; }
    }
}