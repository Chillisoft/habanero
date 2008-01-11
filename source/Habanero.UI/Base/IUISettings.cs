using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Assign a method to this delegate that returns a boolean
    /// to indicate whether the user has permission to right-click
    /// on the ComboBox that represents the given
    /// BusinessObject type.  This applies to all ComboBoxes in the
    /// application that are mapped using a Habanero ComboBoxMapper,
    /// but the individual XML class definition parameter settings for
    /// a field take precedence
    /// </summary>
    /// <param name="boClassType">The class type of the BusinessObject
    /// being mapped in the ComboBox</param>
    /// <param name="controlMapper">The control mapper that maps the
    /// BusinessObject to the ComboBox.  This mapper will provide
    /// information like the BusinessObject of the form.</param>
    public delegate bool PermitComboBoxRightClickDelegate(Type boClassType, IControlMapper controlMapper);

    /// <summary>
    /// An interface to model a class that stores application-wide
    /// settings for the user interface
    /// </summary>
    public interface IUISettings
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
        PermitComboBoxRightClickDelegate PermitComboBoxRightClick
        {
            get;
            set;
        }
    }
}