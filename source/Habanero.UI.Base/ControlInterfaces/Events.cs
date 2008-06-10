using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Base.ControlInterfaces
{

    /// <summary>Represents the method that will handle the <see cref="IControlChilli.KeyPress"></see> event of a <see cref="IControlChilli"></see>.</summary>
    /// <filterpriority>2</filterpriority>
    public delegate void KeyPressEventHandler(object sender, KeyPressEventArgs e);


    /// <summary>Provides data for the <see cref="IControlChilli.KeyPress"></see> event.</summary>
    /// <filterpriority>2</filterpriority>
    //[Serializable()]
	public class KeyPressEventArgs : EventArgs
    {
        private bool handled;
        private char keyChar;

        /// <summary>Initializes a new instance of the <see cref="KeyPressEventArgs"></see> class.</summary>
        /// <param name="keyChar">The ASCII character corresponding to the key the user pressed. </param>
        public KeyPressEventArgs(char keyChar)
        {
            this.keyChar = keyChar;
        }


        /// <summary>Gets or sets a value indicating whether the <see cref="IControlChilli.KeyPress"></see> event was handled.</summary>
        /// <returns>true if the event is handled; otherwise, false.</returns>
        /// <filterpriority>1</filterpriority>
        public bool Handled
        {
            get
            {
                return this.handled;
            }
            set
            {
                this.handled = value;
            }
        }

        /// <summary>Gets or sets the character corresponding to the key pressed.</summary>
        /// <returns>The ASCII character that is composed. For example, if the user presses SHIFT + K, this property returns an uppercase K.</returns>
        /// <filterpriority>1</filterpriority>
        public char KeyChar
        {
            get
            {
                return this.keyChar;
            }
            set
            {
                this.keyChar = value;
            }
        }
    } 
}
