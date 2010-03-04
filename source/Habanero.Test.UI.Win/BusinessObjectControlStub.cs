using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;

namespace Habanero.Test.UI.Win
{
    internal class BusinessObjectControlStub : UserControlWin, IBOEditorControl
    {
        public bool DisplayErrorsCalled { get; private set; }

        public bool ClearErrorsCalled { get; private set; }

        public IBusinessObject BusinessObject { get; set; }

        public void DisplayErrors()
        {
            DisplayErrorsCalled = true;
        }

        public void ClearErrors()
        {
            ClearErrorsCalled = true;
        }

        #region Implementation of IBOEditorControl

        /// <summary>
        /// Applies any changes that have occured in any of the Controls on this control's to their related
        /// Properties on the Business Object.
        /// </summary>
        public void ApplyChangesToBusinessObject()
        {

        }

        /// <summary>
        /// Does the business object controlled by this control or any of its Aggregate or Composite children have and Errors.
        /// </summary>
        public bool HasErrors
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Does the Business Object controlled by this control or any of its Aggregate or Composite children have and warnings.
        /// </summary>
        public bool HasWarning
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        ///  Returns a list of all warnings for the business object controlled by this control or any of its children.
        /// </summary>
        public ErrorList Errors
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Does the business object being managed by this control have any edits that have not been persisted.
        /// </summary>
        /// <returns></returns>
        public bool IsDirty
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Returns a list of all warnings for the business object controlled by this control or any of its children.
        /// </summary>
        /// <returns></returns>
        public ErrorList Warnings
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region Implementation of IBusinessObjectPanel

        /// <summary>
        /// Gets and sets the PanelInfo object created by the control
        /// </summary>
        public IPanelInfo PanelInfo
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}