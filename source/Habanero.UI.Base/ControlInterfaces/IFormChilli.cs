using System;

namespace Habanero.UI.Base
{
    
    public interface IFormChilli : IControlChilli
    {
        void Show();
        void Refresh();
        bool Focus();
        void PerformLayout();

        IFormChilli MdiParent { get; set; }
        event EventHandler Closed;
    }
}