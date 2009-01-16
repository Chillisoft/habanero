using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    public interface IExtendedComboBox : IControlHabanero
    {
        IComboBox ComboBox { get; }
        IButton Button { get; }
    }
}