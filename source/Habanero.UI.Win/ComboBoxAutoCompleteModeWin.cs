//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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


namespace Habanero.UI.Win
{
    class ComboBoxAutoCompleteModeWin
    {

        ///<summary>
        /// Gets the Habanero AutoCompleteMode equivalent to the provided System.Windows.AutoCompleteMode
        ///</summary>
        ///<param name="autoCompleteMode">A System.Windows.Forms AutoCompleteMode.</param>
        ///<returns>The equivalent Habanero AutoCompleteMode.</returns>
        public static Base.AutoCompleteMode GetAutoCompleteMode(System.Windows.Forms.AutoCompleteMode autoCompleteMode)
        {
            switch (autoCompleteMode)
            {
                case System.Windows.Forms.AutoCompleteMode.None: return Base.AutoCompleteMode.None;
                case System.Windows.Forms.AutoCompleteMode.Append: return Base.AutoCompleteMode.Append;
                case System.Windows.Forms.AutoCompleteMode.Suggest: return Base.AutoCompleteMode.Suggest;
                case System.Windows.Forms.AutoCompleteMode.SuggestAppend: return Base.AutoCompleteMode.SuggestAppend;
            }
            return (Base.AutoCompleteMode)autoCompleteMode;
        }

        ///<summary>
        /// Gets the System.Windows.Forms AutoCompleteMode equivalent to the provided Habanero AutoCompleteMode
        ///</summary>
        ///<param name="autoCompleteMode">A Habanero  AutoCompleteMode.</param>
        ///<returns>The equivalent System.Windows.Forms  AutoCompleteMode.</returns>
        public static System.Windows.Forms.AutoCompleteMode GetAutoCompleteMode(Base.AutoCompleteMode autoCompleteMode)
        {
            switch (autoCompleteMode)
            {
                case Base.AutoCompleteMode.None: return System.Windows.Forms.AutoCompleteMode.None;
                case Base.AutoCompleteMode.Append: return System.Windows.Forms.AutoCompleteMode.Append;
                case Base.AutoCompleteMode.Suggest: return System.Windows.Forms.AutoCompleteMode.Suggest;
                case Base.AutoCompleteMode.SuggestAppend: return System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            }
            return (System.Windows.Forms.AutoCompleteMode)autoCompleteMode;
        }
    }
}
