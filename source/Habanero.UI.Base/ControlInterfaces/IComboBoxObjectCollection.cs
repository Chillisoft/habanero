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

using System.Collections;
using Habanero.BO;

namespace Habanero.UI.Base
{
    public interface IComboBoxObjectCollection : IEnumerable
    {
        void Add(object item);

        int Count { get; }

        string Label { get; set; }

        void Remove(object item);

        void Clear();

        void SetCollection(BusinessObjectCollection<BusinessObject> collection);

        object this[int index] { get; set; }

        bool Contains(object value);

        int IndexOf(object value);
    }
}