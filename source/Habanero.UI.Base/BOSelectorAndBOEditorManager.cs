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
using Habanero.Base;

namespace Habanero.UI.Base
{

    /// <summary>
    /// This is a manager class that can be used to combine any <see cref="IBOSelectorControl"/>
    ///   and <see cref="IBusinessObjectControl"/>. The selector control is essentially a control 
    ///   for selecting a Business Object a list box, combo box. The <see cref="IBusinessObjectControl"/> is a control
    ///   that is used for viewing a selected <see cref="IBusinessObject"/>.<br/>
    /// The responsibilities of this class is to link these two controls together so that if a new business object is selected
    ///   in the <see cref="IBOSelectorControl"/> then its values are displayed in the <see cref="IBusinessObjectControl"/>.<br/>
    /// </summary>
    public class BOSelectorAndEditorManager
    {
        

        ///<summary>
        ///// Constructor for the <see cref="BOSelectorAndEditorManager{TBusinessObject}"/>
        ///</summary>
        ///<param name="boSelector"></param>
        ///<param name="boEditor"></param>
        public BOSelectorAndEditorManager(IBOSelectorControl boSelector, IBusinessObjectControl boEditor)
        {

            if (boSelector == null) throw new ArgumentNullException("boSelector");
            if (boEditor == null) throw new ArgumentNullException("boEditor");
            this.BOSelector = boSelector;
            this.BOEditor = boEditor;
            this.AddBoSelectedEventHandler();
        }

        private void AddBoSelectedEventHandler()
        {
            BOSelector.BusinessObjectSelected += ((sender, e) => 
                    {
                        BOEditor.BusinessObject = e.BusinessObject;
                    });
        }


        ///<summary>
        /// Returns hte <see cref="IBOSelectorControl"/> that is being managed by this <see cref="BOSelectorAndEditorManager"/>
        ///</summary>
        public IBOSelectorControl BOSelector { get; private set; }

        ///<summary>
        /// Returns hte <see cref="IBusinessObjectControl"/> that is being managed by this <see cref="BOSelectorAndEditorManager"/>
        ///</summary>
        public IBusinessObjectControl BOEditor { get; private set; }
  
    }
}
