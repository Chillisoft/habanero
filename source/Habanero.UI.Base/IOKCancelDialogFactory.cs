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
    /// Creates OK/Cancel dialogs which contain OK and Cancel buttons, as well
    /// as control placed above the buttons, which the developer must provide.
    /// </summary>
    public interface IOKCancelDialogFactory
    {
        /// <summary>
        /// Creates a panel containing OK and Cancel buttons
        /// </summary>
        /// <param name="nestedControl">The control to place above the buttons</param>
        /// <returns>Returns the created panel</returns>
        IOKCancelPanel CreateOKCancelPanel(IControlHabanero nestedControl);

        /// <summary>
        /// Creates a form containing OK and Cancel buttons
        /// </summary>
        /// <param name="nestedControl">The control to place above the buttons</param>
        /// <param name="formTitle">The title shown on the form</param>
        /// <returns>Returns the created form</returns>
        IFormHabanero CreateOKCancelForm(IControlHabanero nestedControl, string formTitle);
    }   

    /// <summary>
    /// Represents a panel that contains an OK and Cancel button
    /// </summary>
    public interface IOKCancelPanel: IPanel
    {
        /// <summary>
        /// Gets the OK button
        /// </summary>
        IButton OKButton { get; }

        /// <summary>
        /// Gets the Cancel button
        /// </summary>
        IButton CancelButton { get; }
    }
}