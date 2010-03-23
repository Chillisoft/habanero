using System;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using Habanero.UI.Base.ControlMappers;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// A Text Box with a Search Button. This is typically used for cases where there is a large list of potential items and it is 
    /// not appropriate to use a ComboBox for selecting the items.
    /// </summary>
    public class ExtendedTextBoxVWG : UserControlVWG, IExtendedTextBox
    {
        private readonly IButton _button;
        private readonly ITextBox _textBox;
        /// <summary>
        /// Constructor with an unspecified Control Factory.
        /// </summary>
        public ExtendedTextBoxVWG(): this(GlobalUIRegistry.ControlFactory)
        {
        }

        ///<summary>
        /// Constructor with a specified Control Factory
        ///</summary>
        ///<param name="factory"></param>
        public ExtendedTextBoxVWG(IControlFactory factory)
        {
            _button = factory.CreateButton("...");
            _textBox = factory.CreateTextBox();
            _textBox.Enabled = false;
            this.Height = _textBox.Height;
            BorderLayoutManager borderLayoutManager = factory.CreateBorderLayoutManager(this);
            this.Padding = Padding.Empty;
            borderLayoutManager.AddControl(_textBox, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(_button, BorderLayoutManager.Position.East);
        }

        ///<summary>
        /// The Search Button
        ///</summary>
        public IButton Button
        {
            get { return _button; }
        }

        /// <summary>
        /// The Text box in which the result of the search are displayed.
        /// </summary>
        public ITextBox TextBox
        {
            get { return _textBox; }
        }

        ///<summary>
        /// Gets or sets the text associated with this control.           
        ///</summary>
        public override string Text
        {
            get { return this.TextBox.Text; }
            set { this.TextBox.Text = value; }
        }
    }
}