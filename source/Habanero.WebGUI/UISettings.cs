using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.WebGUI
{
    public class UISettings : IUISettings
    {
        /// <summary>
        /// Assign a method to this delegate that returns a boolean
        /// to indicate whether the user has permission to right-click
        /// on the ComboBox that represents the given
        /// BusinessObject type.  This applies to all ComboBoxes in the
        /// application that are mapped using a Habanero ComboBoxMapper,
        /// but the individual XML class definition parameter settings for
        /// a field take precedence.
        /// </summary>
        public PermitComboBoxRightClickDelegate PermitComboBoxRightClick
        {
            get { return null; }
            set {  }
        }
    }
}
