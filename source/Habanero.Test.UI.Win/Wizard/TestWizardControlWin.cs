using Habanero.Test.UI.Base.Wizard;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Wizard
{
    [TestFixture]
    public class TestWizardControlWin : TestWizardControl
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override IWizardControllerStub CreateWizardControllerStub()
        {
            return new WizardControllerStub<WizardStepStubWin>();
        }

        protected override IWizardStepStub CreateWizardStepStub()
        {
            return new WizardStepStubWin();
        }

        internal class WizardStepStubWin : ControlWin, IWizardStepStub
        {
            private bool _allowMoveOn = true;

            private string _headerText;
            private bool _allowMoveBack = true;
            private bool _isInitialised;

            public bool UndoMoveOnWasCalled { get; set; }
            public bool MoveOnWasCalled { get; set; }
            public WizardStepStubWin()
                : this("")
            {
            }

            public WizardStepStubWin(string headerText)
            {
                _headerText = headerText;
                UndoMoveOnWasCalled = false;
                MoveOnWasCalled = false;
            }

            public void UndoMoveOn()
            {
                UndoMoveOnWasCalled = true;
            }

            public string HeaderText
            {
                get { return _headerText; }
                set { _headerText = value; }
            }

            /// <summary>
            /// Provides an interface for the developer to implement functionality to cancel any edits made as part of this
            /// wizard step. The default wizard controller functionality is to call all wizard steps cancelStep methods when
            /// its Cancel method is called.
            /// </summary>
            public void CancelStep()
            {

            }


            public bool AllowMoveBack
            {
                get { return _allowMoveBack; }
                set { _allowMoveBack = value; }
            }

            #region IWizardStep Members

            public void InitialiseStep()
            {
                _isInitialised = true;
            }

            public bool CanMoveOn(out string message)
            {
                message = "";
                if (!AllowMoveOn) message = "Sorry, can't move on";
                return AllowMoveOn;
            }

            /// <summary>
            /// Verifies whether the user can move back from this step.
            /// </summary>
            /// <returns></returns>
            public bool CanMoveBack()
            {
                return AllowMoveBack;
            }

            public void MoveOn()
            {
                MoveOnWasCalled = true;
            }

            #endregion

            public bool AllowMoveOn
            {
                get { return _allowMoveOn; }
                set { _allowMoveOn = value; }
            }

            IControlCollection IControlHabanero.Controls
            {
                get
                {
                    return null;

                }
            }

            public bool IsInitialised
            {
                get { return _isInitialised; }
            }

            ///<summary>
            ///Returns a <see cref="T:System.String"></see> containing the name of the <see cref="T:System.ComponentModel.Component"></see>, if any. This method should not be overridden.
            ///</summary>
            ///
            ///<returns>
            ///A <see cref="T:System.String"></see> containing the name of the <see cref="T:System.ComponentModel.Component"></see>, if any, or null if the <see cref="T:System.ComponentModel.Component"></see> is unnamed.
            ///</returns>
            ///
            public override string ToString()
            {
                return Name;
            }
        }
    }
}