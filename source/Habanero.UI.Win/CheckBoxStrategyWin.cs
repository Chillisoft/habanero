using System;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    internal class CheckBoxStrategyWin : ICheckBoxMapperStrategy
    {
        public void AddClickEventHandler(CheckBoxMapper mapper)
        {
            if (mapper.Control is ICheckBox)
            {
                CheckBoxWin checkBox = (CheckBoxWin) mapper.Control;
                checkBox.Click += delegate(object sender, EventArgs e)
                {
                    mapper.ApplyChangesToBusinessObject();
                    mapper.ApplyChanges();
                };
            }
        }
    }
}