using System;
using System.Collections;
using System.Drawing;

namespace Habanero.UI.Base
{
    public interface IControlChilli
    {
        event EventHandler Resize;
        event EventHandler VisibleChanged;

        int Width { get; set; }

        IList Controls { get; }

        bool Visible { get; set; }

        int Left { get; set; }

        int TabIndex { get; set; }

        int Height { get; set; }

        int Top { get; set; }

        int Right { get; }

        string Text { get; set; }
        string Name { get; set; }
        bool Enabled { get; set; }

        Color ForeColor { get; set; }

        Color BackColor { get; set; }
        bool TabStop { get; set; }

    }
}