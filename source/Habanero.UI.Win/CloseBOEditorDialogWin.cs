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
using System;
using System.Drawing;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// This is a Dialog Box that is specialiased for dealing with the
    /// Closing of any form or application that is editing Business Objects.
    /// The dialogue box will display a sensible message to the user to determine
    /// whether they want to Close the Origional form without saving, Save the BO and then
    /// Close or Cancel the Closing of the origional form.
    /// </summary>
    public class CloseBOEditorDialogWin : FormWin, ICloseBOEditorDialog
    {
        /// <summary>
        /// The CancelClose Button.
        /// </summary>
        public IButton CancelCloseBtn { get; private set; }
        /// <summary>
        /// The Save and Close Button.
        /// </summary>
        public IButton SaveAndCloseBtn { get; private set; }
        /// <summary>
        /// The Close without saving Button.
        /// </summary>
        public IButton CloseWithoutSavingBtn { get; private set; }
        private IBusinessObject BusinessObject { get; set; }
        /// <summary>
        /// The Result from this Form.
        /// </summary>
        public CloseBOEditorDialogResult BOEditorDialogResult { get; private set; }
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="controlFactory">The control Factory used to construct buttons, labels etc by ths control</param>
        /// <param name="businessObject">The business Object whose Dirty state is being checked.</param>
        public CloseBOEditorDialogWin(IControlFactory controlFactory, IBusinessObject businessObject)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (businessObject == null) throw new ArgumentNullException("businessObject");
            BusinessObject = businessObject;
            var fullDisplayName = businessObject.ClassDef.DisplayName
                    + " '" + businessObject.ToString() + "'";
            ConstructControl(controlFactory, fullDisplayName, this.BusinessObject.Status.IsValid(), this.BusinessObject.Status.IsDirty);
            this.MinimumSize = new Size(400, 200);
            this.Size = this.MinimumSize;
        }
        ///<summary>
        /// Construct the Dialog form for any situation e.g. where the Form being closed has 
        /// Mutliple Business Objects is a wizard etc.
        ///</summary>
        /// <param name="controlFactory">The control Factory used to construct buttons, labels etc by ths control</param>
        ///<param name="fullDisplayName">Full display name for the BusienssObject(s)</param>
        ///<param name="isInValidState">Are the BusinessObject(s) in a valid state</param>
        ///<exception cref="ArgumentNullException">control Factory must not be null</exception>
        public CloseBOEditorDialogWin(IControlFactory controlFactory, string fullDisplayName, bool isInValidState, bool isDirty)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            ConstructControl(controlFactory, fullDisplayName, isInValidState, isDirty);
            this.MinimumSize = new Size(400, 200);
            this.Size = this.MinimumSize;
        }
        private void ConstructControl(IControlFactory controlFactory, string fullDisplayName, bool isInValidState, bool isDirty)
        {
            IButtonGroupControl buttonGroupControl = controlFactory.CreateButtonGroupControl();
            CancelCloseBtn = buttonGroupControl.AddButton("CancelClose", "Cancel Close", ButtonClick);
            CloseWithoutSavingBtn = buttonGroupControl.AddButton("CloseWithoutSaving", "&Close without saving", ButtonClick);
            SaveAndCloseBtn = buttonGroupControl.AddButton("SaveAndClose","&Save & Close", ButtonClick);
            SaveAndCloseBtn.Enabled = isInValidState;
            BOEditorDialogResult = CloseBOEditorDialogResult.CancelClose;
            if (!isDirty)
            {
                BOEditorDialogResult = CloseBOEditorDialogResult.CloseWithoutSaving;
                this.Close();               
            }
            var isValidString = "";
            if (isInValidState)
            {
                isValidString = " and is in a valid state to be saved";                 
            }
            ILabel label = controlFactory.CreateLabel("The " + fullDisplayName + " is has been edited" + isValidString + ". Please choose the appropriate action");
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(label, BorderLayoutManager.Position.Centre);
            layoutManager.AddControl(buttonGroupControl, BorderLayoutManager.Position.South);
        }

        private void ButtonClick(object sender, EventArgs eventArgs)
        {
            var button = sender as IButton;
            if (button == null) return;
            BOEditorDialogResult = (CloseBOEditorDialogResult)Enum.Parse(typeof(CloseBOEditorDialogResult), button.Name);
            this.Close();
        }
    }
}