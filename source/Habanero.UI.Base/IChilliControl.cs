using System;
using System.Collections;
using System.Collections.Generic;

namespace Habanero.UI.Base
{
    public interface IChilliControl
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
    }
}