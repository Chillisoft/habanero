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

namespace Habanero.UI.VWG
{
    class ComboBoxAutoCompleteModeVWG
    {
        ///<summary>
        /// Gets the Habanero AutoCompleteMode equivalent to the provided Gizmox.WebGUI.Forms AutoCompleteMode
        ///</summary>
        ///<param name="autoCompleteMode">A Gizmox.WebGUI.Forms AutoCompleteMode.</param>
        ///<returns>The equivalent Habanero AutoCompleteMode.</returns>
        public static Base.AutoCompleteMode GetAutoCompleteMode(Gizmox.WebGUI.Forms.AutoCompleteMode autoCompleteMode)
        {
            switch (autoCompleteMode)
            {
                case Gizmox.WebGUI.Forms.AutoCompleteMode.None: return Base.AutoCompleteMode.None;
                case Gizmox.WebGUI.Forms.AutoCompleteMode.Append: return Base.AutoCompleteMode.Append;
                case Gizmox.WebGUI.Forms.AutoCompleteMode.Suggest: return Base.AutoCompleteMode.Suggest;
                case Gizmox.WebGUI.Forms.AutoCompleteMode.SuggestAppend: return Base.AutoCompleteMode.SuggestAppend;
            }
            return (Base.AutoCompleteMode)autoCompleteMode;
        }

        ///<summary>
        /// Gets the Gizmox.WebGUI.Forms AutoCompleteMode equivalent to the provided Habanero AutoCompleteMode
        ///</summary>
        ///<param name="autoCompleteMode">A Habanero  AutoCompleteMode.</param>
        ///<returns>The equivalent Gizmox.WebGUI.Forms  AutoCompleteMode.</returns>
        public static Gizmox.WebGUI.Forms.AutoCompleteMode GetAutoCompleteMode(Base.AutoCompleteMode autoCompleteMode)
        {
            switch (autoCompleteMode)
            {
                case Base.AutoCompleteMode.None: return Gizmox.WebGUI.Forms.AutoCompleteMode.None;
                case Base.AutoCompleteMode.Append: return Gizmox.WebGUI.Forms.AutoCompleteMode.Append;
                case Base.AutoCompleteMode.Suggest: return Gizmox.WebGUI.Forms.AutoCompleteMode.Suggest;
                case Base.AutoCompleteMode.SuggestAppend: return Gizmox.WebGUI.Forms.AutoCompleteMode.SuggestAppend;
            }
            return (Gizmox.WebGUI.Forms.AutoCompleteMode)autoCompleteMode;
        }
    }
}
