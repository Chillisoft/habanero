using System;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a set of strategies that can be applied to a control
    /// </summary>
    public class ControlMapperStrategyWin : IControlMapperStrategy
    {
        private Control _control;


        public void AddCurrentBOPropHandlers(ControlMapper mapper, IBOProp boProp)
        {
            if (boProp != null)
            {
//                Add needed handlers
                boProp.Updated += mapper.BOPropValueUpdatedHandler;
            }
        }

        public void RemoveCurrentBOPropHandlers(ControlMapper mapper, IBOProp boProp)
        {
            if(boProp!=null)
            {
                boProp.Updated -= mapper.BOPropValueUpdatedHandler;
            }
        }

        /// <summary>
        /// Provides an interface for handling the default key press behaviours on a control.
        /// This is typically used to change the handling of the enter key. I.e. A common behavioural
        /// requirement is to have the enter key move to the next control.
        /// </summary>
        /// <param name="control">The control whose events will be handled</param>
        public void AddKeyPressEventHandler(IControlChilli control)
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
            if (e.KeyChar == 0x013)//Should be a Keys.Enter
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
        private static Control GetNextControlInTabOrder(Control parentControl, Control control)
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
        private static Control GetFirstControl(Control parentControl, Control control)
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