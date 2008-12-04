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

namespace Habanero.UI.Win
{
    class ComboBoxAutoCompleteSourceWin
    {
        ///<summary>
        /// Gets the Habanero AutoCompleteSource equivalent to the provided System.Windows.AutoCompleteSource
        ///</summary>
        ///<param name="autoCompleteSource">A System.Windows.Forms AutoCompleteSource.</param>
        ///<returns>The equivalent Habanero AutoCompleteSource.</returns>
        public static Base.AutoCompleteSource GetAutoCompleteSource(System.Windows.Forms.AutoCompleteSource autoCompleteSource)
        {
            switch (autoCompleteSource)
            {
                case System.Windows.Forms.AutoCompleteSource.AllSystemSources: return Base.AutoCompleteSource.AllSystemSources;
                case System.Windows.Forms.AutoCompleteSource.AllUrl: return Base.AutoCompleteSource.AllUrl;
                case System.Windows.Forms.AutoCompleteSource.CustomSource: return Base.AutoCompleteSource.CustomSource;
                case System.Windows.Forms.AutoCompleteSource.FileSystem: return Base.AutoCompleteSource.FileSystem;
                case System.Windows.Forms.AutoCompleteSource.FileSystemDirectories: return Base.AutoCompleteSource.FileSystemDirectories;
                case System.Windows.Forms.AutoCompleteSource.HistoryList: return Base.AutoCompleteSource.HistoryList;
                case System.Windows.Forms.AutoCompleteSource.ListItems: return Base.AutoCompleteSource.ListItems;
                case System.Windows.Forms.AutoCompleteSource.None: return Base.AutoCompleteSource.None;
                case System.Windows.Forms.AutoCompleteSource.RecentlyUsedList: return Base.AutoCompleteSource.RecentlyUsedList;
            }
            return (Base.AutoCompleteSource)autoCompleteSource;
        }

        ///<summary>
        /// Gets the System.Windows.Forms AutoCompleteSource equivalent to the provided Habanero AutoCompleteSource
        ///</summary>
        ///<param name="autoCompleteSource">A Habanero  AutoCompleteSource.</param>
        ///<returns>The equivalent System.Windows.Forms  AutoCompleteSource.</returns>
        public static System.Windows.Forms.AutoCompleteSource GetAutoCompleteSource(Base.AutoCompleteSource autoCompleteSource)
        {
            switch (autoCompleteSource)
            {
                case Base.AutoCompleteSource.AllSystemSources: return System.Windows.Forms.AutoCompleteSource.AllSystemSources;
                case Base.AutoCompleteSource.AllUrl: return System.Windows.Forms.AutoCompleteSource.AllUrl;
                case Base.AutoCompleteSource.CustomSource: return System.Windows.Forms.AutoCompleteSource.CustomSource;
                case Base.AutoCompleteSource.FileSystem: return System.Windows.Forms.AutoCompleteSource.FileSystem;
                case Base.AutoCompleteSource.FileSystemDirectories: return System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
                case Base.AutoCompleteSource.HistoryList: return System.Windows.Forms.AutoCompleteSource.HistoryList;
                case Base.AutoCompleteSource.ListItems: return System.Windows.Forms.AutoCompleteSource.ListItems;
                case Base.AutoCompleteSource.None: return System.Windows.Forms.AutoCompleteSource.None;
                case Base.AutoCompleteSource.RecentlyUsedList: return System.Windows.Forms.AutoCompleteSource.RecentlyUsedList;
            }
            return (System.Windows.Forms.AutoCompleteSource)autoCompleteSource;
        }
    }
}
