using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides a store of application-wide user interface settings
    /// </summary>
    public class UISettings : IUISettings
    {
        private PermitComboBoxRightClickDelegate _permitComboBoxRightClick;

        /// <summary>
        /// Indicates whether the user has permission to right-click
        /// on the given ComboBox that represents the given
        /// BusinessObject type.  This applies to all ComboBoxes in the
        /// application that are mapped using a Habanero ComboBoxMapper.
        /// </summary>
        public PermitComboBoxRightClickDelegate PermitComboBoxRightClick
        {
            get { return _permitComboBoxRightClick; }
            set { _permitComboBoxRightClick = value; }
        }
    }
}
