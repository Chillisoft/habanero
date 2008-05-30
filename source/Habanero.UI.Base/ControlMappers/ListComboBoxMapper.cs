using Habanero.UI.Base;

namespace Habanero.UI.Base
{
    public class ListComboBoxMapper : ControlMapper
    {
        private IComboBox _comboBox;
        public ListComboBoxMapper(IControlChilli ctl, string propName, bool isReadOnly, IControlFactory factory)
            : base(ctl, propName, isReadOnly, factory)
        {
            _comboBox = (IComboBox)ctl;
        }

        public override void ApplyChangesToBusinessObject()
        {
            SetPropertyValue(_comboBox.SelectedItem);
        }

        /// <summary>
        /// Updates the value in the control from its business object.
        /// </summary>
        protected  override void InternalUpdateControlValueFromBo()
        {
            _comboBox.SelectedItem = GetPropertyValue();
        }

        public void SetList(string list)
        {
            foreach (string item in list.Split('|'))
            {
                _comboBox.Items.Add(item);
            }
        }
    }
}