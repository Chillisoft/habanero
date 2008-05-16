using System;
using System.Drawing;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;

namespace Habanero.UI.Win
{
    public class ControlFactoryWin : IControlFactory
    {
        public IFilterControl CreateFilterControl()
        {
            return new FilterControlWin(this);
        }

        public ITextBox CreateTextBox()
        {
            return new TextBoxWin();
        }

        /// <summary>
        /// Creates a new empty TreeView
        /// </summary>
        /// <param name="name">The name of the view</param>
        /// <returns>Returns a new TreeView object</returns>
        public ITreeView CreateTreeView(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new control of the type specified.
        /// </summary>
        /// <param name="controlType">The control type, which must be a
        /// sub-type of Control</param>
        /// <returns>Returns a new object of the type requested</returns>
        public IControlChilli CreateControl(Type controlType)
        {
            return (IControlChilli) Activator.CreateInstance(controlType);
        }

        /// <summary>
        /// Creates a new DateTimePicker with a specified date
        /// </summary>
        /// <param name="defaultDate">The initial date value</param>
        /// <returns>Returns a new DateTimePicker object</returns>
        public IDateTimePicker CreateDateTimePicker(DateTime defaultDate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new DateTimePicker that is formatted to handle months
        /// and years
        /// </summary>
        /// <returns>Returns a new DateTimePicker object</returns>
        public IDateTimePicker CreateMonthPicker()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// two decimal places for monetary use
        /// </summary>
        /// <returns>Returns a new NumericUpDown object</returns>
        public INumericUpDown CreateNumericUpDownMoney()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new numeric up-down control that is formatted with
        /// zero decimal places for integer use
        /// </summary>
        /// <returns>Returns a new NumericUpDown object</returns>
        public INumericUpDown CreateNumericUpDownInteger()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new CheckBox with a specified initial checked state
        /// </summary>
        /// <param name="defaultValue">Whether the initial box is ticked</param>
        /// <returns>Returns a new CheckBox object</returns>
        public ICheckBox CreateCheckBox(bool defaultValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new progress bar
        /// </summary>
        /// <returns>Returns a new ProgressBar object</returns>
        public IProgressBar CreateProgressBar()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new splitter which enables the user to resize 
        /// docked controls
        /// </summary>
        /// <returns>Returns a new Splitter object</returns>
        public ISplitter CreateSplitter()
        {
            return new SplitterWin();
        }

        /// <summary>
        /// Creates a new tab page
        /// </summary>
        /// <param name="title">The page title to appear in the tab</param>
        /// <returns>Returns a new TabPage object</returns>
        public ITabPage CreateTabPage(string title)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new radio button
        /// </summary>
        /// <param name="text">The text to appear next to the radio button</param>
        /// <returns>Returns a new RadioButton object</returns>
        public IRadioButton CreateRadioButton(string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new GroupBox
        /// </summary>
        /// <returns>Returns the new GroupBox</returns>
        public IGroupBox CreateGroupBox()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a form in which a business object can be edited
        /// </summary>
        /// <param name="bo">The business object to edit</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returns a DefaultBOEditorForm object</returns>
        public IDefaultBOEditorForm CreateBOEditorForm(BusinessObject bo, string uiDefName)
        {
            throw new NotImplementedException();
        }

        public ITabControl CreateTabControl()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a multi line textbox, setting the scrollbars to vertical
        /// </summary>
        /// <param name="numLines"></param>
        public ITextBox CreateTextBoxMultiLine(int numLines)
        {
            TextBoxWin tb = (TextBoxWin)CreateTextBox();
            tb.Multiline = true;
            tb.AcceptsReturn = true;
            tb.Height = tb.Height * numLines;
            tb.ScrollBars = ScrollBars.Vertical;
            return tb;
        }

        public IDataGridViewColumn CreateDataGridViewColumn()
        {
            throw new NotImplementedException();
        }

        public IComboBox CreateComboBox()
        {
            return new ComboBoxWin();
        }

        public IListBox CreateListBox()
        {
            return new ListBoxWin();
        }

        public IMultiSelector<T> CreateMultiSelector<T>()
        {
            return new MultiSelectorWin<T>();
        }

        public IButton CreateButton()
        {
            return new ButtonWin();
        }

        /// <summary>
        /// Creates a new button
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        /// <returns>Returns the new Button object</returns>
        public IButton CreateButton(string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new button with an attached event handler to carry out
        /// further instructions if the button is pressed
        /// </summary>
        /// <param name="text">The text to appear on the button</param>
        /// <param name="clickHandler">The method that handles the event</param>
        /// <returns>Returns the new Button object</returns>
        public IButton CreateButton(string text, EventHandler clickHandler)
        {
            throw new NotImplementedException();
        }

        public ICheckBox CreateCheckBox()
        {
            return new CheckBoxWin();
        }

        public ILabel CreateLabel()
        {
            ILabel label = new LabelWin();
            label.TabStop = false;
            return label;
        }

        public ILabel CreateLabel(string labelText)
        {
            ILabel label = CreateLabel();
            label.Text = labelText;
            return label;
        }
        public ILabel CreateLabel(string labelText, bool isBold)
        {
            LabelWin label = (LabelWin) CreateLabel();
            label.Text = labelText;
            label.FlatStyle = FlatStyle.System;
            if (isBold)
            {
                label.Font = new Font(label.Font, FontStyle.Bold);
            }
            label.Width = label.PreferredWidth;
            if (isBold)
            {
                label.Width += 10;
            }
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.TabStop = false;
            return label;
        }

        public IDateTimePicker CreateDateTimePicker()
        {
            return new  DateTimePickerWin();
        }

        public BorderLayoutManager CreateBorderLayoutManager(IControlChilli control)
        {
            return new BorderLayoutManagerWin(control, this);
        }

        public IPanel CreatePanel()
        {
            return new PanelWin();
        }

        public IReadOnlyGrid CreateReadOnlyGrid()
        {
            return new ReadOnlyGridWin();
        }

        public IReadOnlyGridWithButtons CreateReadOnlyGridWithButtons(IControlFactory controlfactory)
        {
            return  new ReadOnlyGridWithButtonsWin();
        }

        public IButtonGroupControl CreateButtonGroupControl()
        {
            return new ButtonGroupControlWin(this);
        }

        public IReadOnlyGridButtonsControl CreateReadOnlyGridButtonsControl()
        {
            throw new System.NotImplementedException();
        }

        public IPanel CreatePanel(IControlFactory controlFactory)
        {
            return new PanelWin();
        }

        /// <summary>
        /// Creates a new panel
        /// </summary>
        /// <param name="name">The name of the panel</param>
        /// <returns>Returns a new Panel object</returns>
        public IPanel CreatePanel(string name, IControlFactory controlFactory)
        {
            throw new NotImplementedException();
        }

        public ITabPage createTabPage(string name)
        {
            throw new System.NotImplementedException();
        }

       

        public ITextBox CreatePasswordTextBox()
        {
            throw new System.NotImplementedException();
        }

        public IToolTip CreateToolTip()
        {
            return new ToolTipWin();
        }

        public IControlChilli CreateControl()
        {
            return new ControlWin();
        }
    }

    internal class SplitterWin : Splitter, ISplitter
    {
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(this.Controls); }
        }
    }
}
