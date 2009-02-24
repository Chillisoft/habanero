using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestMessageBoxConfirmer
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }

        [Test]
        public void Test_Constructor()
        {
            //---------------Set up test pack-------------------
            IControlFactory controlFactory = new ControlFactoryWin();
            string title = TestUtil.GetRandomString();
            MessageBoxIcon messageBoxIcon = MessageBoxIcon.None;
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            MessageBoxConfirmer messageBoxConfirmer = new MessageBoxConfirmer(controlFactory, title, messageBoxIcon);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(IConfirmer), messageBoxConfirmer);
            Assert.AreSame(controlFactory, messageBoxConfirmer.ControlFactory);
            Assert.AreEqual(title, messageBoxConfirmer.Title);
            Assert.AreEqual(messageBoxIcon, messageBoxConfirmer.MessageBoxIcon);
        }

        public class MessageBoxConfirmer: IConfirmer
        {
            public IControlFactory ControlFactory { get; private set; }
            public string Title { get; private set; }
            public MessageBoxIcon MessageBoxIcon { get; private set; }

            public MessageBoxConfirmer(IControlFactory controlFactory, string title, MessageBoxIcon messageBoxIcon)
            {
                ControlFactory = controlFactory;
                Title = title;
                MessageBoxIcon = messageBoxIcon;
            }


            /// <summary>
            /// Gets confirmation from the user after providing them with an option
            /// </summary>
            /// <param name="message">The message to display</param>
            /// <returns>Returns true if the user confirms the choice and false
            /// if they decline the offer</returns>
            public bool Confirm(string message)
            {
                throw new System.NotImplementedException();
            }

            ///<summary>
            /// Gets confirmation from the user after providing them with an option
            /// and executes the provided delegate once the user has responded.
            ///</summary>
            ///<param name="message">The message to display</param>
            ///<param name="confirmationDelegate">The delegate to execute once the user has responded.</param>
            ///<returns>Returns true if the user confirms the choice and false
            /// if they decline the offer</returns>
            public bool Confirm(string message, ConfirmationDelegate confirmationDelegate)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}