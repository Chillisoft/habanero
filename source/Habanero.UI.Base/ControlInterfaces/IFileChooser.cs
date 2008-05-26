using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Base.ControlInterfaces
{
    public interface IFileChooser : IControlChilli
    {
        string SelectedFilePath { get; set;}
    }
}
