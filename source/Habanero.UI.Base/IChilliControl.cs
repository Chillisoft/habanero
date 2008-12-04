//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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