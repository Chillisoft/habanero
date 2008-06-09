using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;
using KeyPressEventHandler=Habanero.UI.Base.ControlInterfaces.KeyPressEventHandler;

namespace Habanero.UI.Win
{
    public class TextBoxWin : TextBox, ITextBox
    {
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        public TextBoxWin()
        {
            //_textBox.KeyPress += delegate(object sender, KeyPressEventArgs e)
            //             {
            //                 if (IsIntegerType())
            //                 {
            //                     if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != 8 && e.KeyChar != '-')
            //                     {
            //                         e.Handled = true;
            //                     }
            //                     if (e.KeyChar == '-' && _textBox.SelectionStart != 0)
            //                     {
            //                         e.Handled = true;
            //                     }
            //                 }
            //                 if (IsDecimalType())
            //                 {
            //                     if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '.' && e.KeyChar != 8 && e.KeyChar != '-')
            //                     {
            //                         e.Handled = true;
            //                     }

            //                     if (e.KeyChar == '.' && _textBox.Text.Contains("."))
            //                     {
            //                         e.Handled = true;
            //                     }
            //                     if (e.KeyChar == '.' && _textBox.SelectionStart == 0)
            //                     {
            //                         _textBox.Text = "0." + _textBox.Text;
            //                         e.Handled = true;
            //                         _textBox.SelectionStart = 2;
            //                         _textBox.SelectionLength = 0;
            //                     }
            //                     if (e.KeyChar == '-' && _textBox.SelectionStart != 0)
            //                     {
            //                         e.Handled = true;
            //                     }
            //                 }
            //             };
        }
    }
}