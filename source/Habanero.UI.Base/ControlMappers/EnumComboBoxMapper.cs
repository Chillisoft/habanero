using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.UI.Base
{
    /// <summary>
    /// TODO: comment 
    /// </summary>
    public class EnumComboBoxMapper : ComboBoxMapper
    {
        public EnumComboBoxMapper(IComboBox comboBox, string propName, bool isReadOnly, IControlFactory factory) : base(comboBox, propName, isReadOnly, factory)
        {
        }

        public override void ApplyChangesToBusinessObject()
        {
            throw new System.NotImplementedException();
        }

        protected override void InternalUpdateControlValueFromBo()
        {
            //
        }

        protected internal override void SetupComboBoxItems()
        {
            IPropDef propDef = _businessObject.ClassDef.PropDefcol[_propertyName];
            if (!propDef.PropertyType.IsEnum)
            {
                throw new InvalidPropertyException("EnumComboBoxMapper can only be used for an enum property type");
            }

            string[] names = Enum.GetNames(propDef.PropertyType);
            foreach (string name in names)
            {
                _comboBox.Items.Add(name);
            }
        }
    }
}
