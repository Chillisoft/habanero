using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Base
{
    #region Enums

    /// <summary>
    /// Specifies constants defining which buttons to display on a MessageBox.
    /// </summary>
    //[Serializable()]
    public enum MessageBoxButtons
    {
        /// <summary>
        /// The message box contains Abort, Retry, and Ignore buttons.
        /// </summary>
        AbortRetryIgnore = 2,
        /// <summary>
        /// The message box contains an OK button.
        /// </summary>
        OK = 0,
        /// <summary>
        /// The message box contains OK and Cancel buttons.
        /// </summary>
        OKCancel = 1,
        /// <summary>
        /// The message box contains Retry and Cancel buttons.
        /// </summary>
        RetryCancel = 5,
        /// <summary>
        /// The message box contains Yes and No buttons.
        /// </summary>
        YesNo = 4,
        /// <summary>
        /// The message box contains Yes, No, and Cancel buttons.
        /// </summary>
        YesNoCancel = 3
    }

    /// <summary>
    /// Specifies constants defining the default button on a MessageBox.
    /// </summary>
    //[Serializable()]
    public enum MessageBoxDefaultButton
    {
        /// <summary>
        /// The first button on the message box is the default button.
        /// </summary>
        Button1 = 0,
        /// <summary>
        /// The second button on the message box is the default button.
        /// </summary>
        Button2 = 0x100,
        /// <summary>
        /// The third button on the message box is the default button.
        /// </summary>
        Button3 = 0x200
    }

    /// <summary>
    /// Specifies constants defining which information to display.
    /// </summary>
    //[Serializable()]
    public enum MessageBoxIcon
    {
        /// <summary>
        /// The message box contains a symbol consisting of a lowercase letter i in a circle.
        /// </summary>
        Asterisk = 0x40,
        /// <summary>
        /// The message box contains a symbol consisting of white X in a circle with a red background.
        /// </summary>
        Error = 0x10,
        /// <summary>
        /// The message box contains a symbol consisting of an exclamation point in a triangle with a yellow background.
        /// </summary>
        Exclamation = 0x30,
        /// <summary>
        /// The message box contains a symbol consisting of a white X in a circle with a red background.
        /// </summary>
        Hand = 0x10,
        /// <summary>
        /// The message box contains a symbol consisting of a lowercase letter i in a circle.
        /// </summary>
        Information = 0x40,
        /// <summary>
        /// The message box contain no symbols.
        /// </summary>
        None = 0,
        /// <summary>
        /// The message box contains a symbol consisting of a question mark in a circle.
        /// </summary>
        Question = 0x20,
        /// <summary>
        /// The message box contains a symbol consisting of white X in a circle with a red background.
        /// </summary>
        Stop = 0x10,
        /// <summary>
        /// The message box contains a symbol consisting of an exclamation point in a triangle with a yellow background.
        /// </summary>
        Warning = 0x30
    }

    /// <summary>
    /// Specifies options on a MessageBox.
    /// </summary>
    [Flags]
    //[Serializable()]
    public enum MessageBoxOptions
    {
        /// <summary>
        /// The message box is displayed on the active desktop.
        /// </summary>
        DefaultDesktopOnly = 0x20000,
        /// <summary>
        /// The message box text is right-aligned.
        /// </summary>
        RightAlign = 0x80000,
        /// <summary>
        /// Specifies that the message box text is displayed with right to left reading order.
        /// </summary>
        RtlReading = 0x100000,
        /// <summary>
        /// The message box is displayed on the active desktop.
        /// </summary>
        ServiceNotification = 0x200000
    }


    #endregion Enums
}
