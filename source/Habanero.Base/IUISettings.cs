using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Habanero.Base
{
    /// <summary>
    /// Assign a method to this delegate that returns a boolean
    /// to indicate whether the user has permission to right-click
    /// on the given ComboBox that represents the given
    /// BusinessObject type.  This applies to all ComboBoxes in the
    /// application that are mapped using a Habanero ComboBoxMapper.
    /// </summary>
    /// <param name="boClassType">The class type of the BusinessObject
    /// being mapped in the ComboBox</param>
    /// <param name="comboBox">The ComboBox itself</param>
    public delegate bool PermitComboBoxRightClickDelegate(Type boClassType, ComboBox comboBox);

    /// <summary>
    /// An interface to model a class that stores application-wide
    /// settings for the user interface
    /// </summary>
    public interface IUISettings
    {
        /// <summary>
        /// Indicates whether the user has permission to right-click
        /// on the given ComboBox that represents the given
        /// BusinessObject type.  This applies to all ComboBoxes in the
        /// application that are mapped using a Habanero ComboBoxMapper.
        /// </summary>
        PermitComboBoxRightClickDelegate PermitComboBoxRightClick
        {
            get;
            set;
        }
    }
}
