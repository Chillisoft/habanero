using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Base.Wizard
{
    ///<summary>
    /// This is a very simple wizard step that has a lable
    /// and allows you to set the text for this Label via the 
    /// Set Message.
    ///</summary>
    public interface IMessageWizardStep:IWizardStep
    {
        ///<summary>
        /// The message that will be shown on the Label
        ///</summary>
        ///<param name="message"></param>
        void SetMessage(string message);
    }
}
