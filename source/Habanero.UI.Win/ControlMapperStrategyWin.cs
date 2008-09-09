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

using System;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a control
    /// depending on the environment
    /// </summary>
    public class ControlMapperStrategyWin : IControlMapperStrategy
    {
        private Control _control;

        /// <summary>
        /// Adds handlers to events of a current business object property.
        /// </summary>
        /// <param name="mapper">The control mapper that maps the business object property to the control</param>
        /// <param name="boProp">The business object property being mapped to the control</param>
        public void AddCurrentBOPropHandlers(ControlMapper mapper, IBOProp boProp)
        {
            if (boProp != null)
            {
                // Add needed handlers
                boProp.Updated += mapper.BOPropValueUpdatedHandler;
            }
        }

        /// <summary>
        /// Removes handlers to events of a current business object property.
        /// It is essential that if the AddCurrentBoPropHandlers is implemented then this 
        /// is implemented such that editing a business object that is no longer being shown on the control does not
        /// does not update the value in the control.
        /// </summary>
        /// <param name="mapper">The control mapper that maps the business object property to the control</param>
        /// <param name="boProp">The business object property being mapped to the control</param>
        public void RemoveCurrentBOPropHandlers(ControlMapper mapper, IBOProp boProp)
        {
            if(boProp!=null)
            {
                boProp.Updated -= mapper.BOPropValueUpdatedHandler;
            }
        }

        /// <summary>
        /// Handles the default key press behaviours on a control.
        /// This is typically used to change the handling of the enter key (such as having
        /// the enter key cause focus to move to the next control).
        /// </summary>
        /// <param name="control">The control whose events will be handled</param>
        public void AddKeyPressEventHandler(IControlHabanero control)
        {
            if (control == null) throw new ArgumentNullException("control");
            _control = (Control) control;
            _control.KeyUp += CtlKeyUpHandler;
            _control.KeyDown += CtlKeyDownHandler;
            _control.KeyPress += CtlKeyPressHandler;
        }
        /// <summary>
        /// A handler to deal with the case where a key has been pressed.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CtlKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x013)//Should be a Keys.Enter?
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// A handler to deal with the case where a key is down.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CtlKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// A handler to deal with the case where a key has been released.
        /// If the key is an Enter key, focus moves to the next item in the
        /// tab order.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CtlKeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                //TODO: Port this 
                //if (_control is TextBoxWin && ((TextBoxWin)_control).Multiline)
                //{
                //    return;
                //}

                Control nextControl = GetNextControlInTabOrder(_control.Parent, _control);

                if (nextControl != null)
                {
                    nextControl.Focus();
                }
            }
        }

        /// <summary>
        /// Provides the next item in the tab order on a control
        /// </summary>
        /// <param name="parentControl">The parent of the controls in question</param>
        /// <param name="control">The current control</param>
        /// <returns>Returns the next control in the tab order</returns>
        internal static Control GetNextControlInTabOrder(Control parentControl, Control control)
        {
            Control nextControl = parentControl.GetNextControl(control, true);
            if (nextControl == null)
            {
                return GetFirstControl(parentControl, control);
            }
            if (!nextControl.TabStop)
            {
                return GetNextControlInTabOrder(parentControl, nextControl);
            }
            return nextControl;
        }

        /// <summary>
        /// Provides the first control in the tab order on a control
        /// </summary>
        /// <param name="parentControl">The parent of the controls in question</param>
        /// <param name="control">The current control</param>
        /// <returns>Returns the first control in the tab order</returns>
        internal static Control GetFirstControl(Control parentControl, Control control)
        {
            Control lastTabStopControl = control;
            Control currentControl = control;
            do
            {
                Control prevControl = parentControl.GetNextControl(currentControl, false);
                if (prevControl == null)
                {
                    return lastTabStopControl;
                }
                if (prevControl.TabStop)
                {
                    lastTabStopControl = prevControl;
                }
                currentControl = prevControl;
            } while (true);
        }
    }
}