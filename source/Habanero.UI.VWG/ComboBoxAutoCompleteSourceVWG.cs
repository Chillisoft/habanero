// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
namespace Habanero.UI.VWG
{
    class ComboBoxAutoCompleteSourceVWG
    {
        ///<summary>
        /// Gets the Habanero AutoCompleteSource equivalent to the provided Gizmox.WebGUI.Forms AutoCompleteSource
        ///</summary>
        ///<param name="autoCompleteSource">A Gizmox.WebGUI.Forms AutoCompleteSource.</param>
        ///<returns>The equivalent Habanero AutoCompleteSource.</returns>
        public static Base.AutoCompleteSource GetAutoCompleteSource(Gizmox.WebGUI.Forms.AutoCompleteSource autoCompleteSource)
        {
            switch (autoCompleteSource)
            {
                case Gizmox.WebGUI.Forms.AutoCompleteSource.AllSystemSources: return Base.AutoCompleteSource.AllSystemSources;
                case Gizmox.WebGUI.Forms.AutoCompleteSource.AllUrl: return Base.AutoCompleteSource.AllUrl;
                case Gizmox.WebGUI.Forms.AutoCompleteSource.CustomSource: return Base.AutoCompleteSource.CustomSource;
                case Gizmox.WebGUI.Forms.AutoCompleteSource.FileSystem: return Base.AutoCompleteSource.FileSystem;
                case Gizmox.WebGUI.Forms.AutoCompleteSource.FileSystemDirectories: return Base.AutoCompleteSource.FileSystemDirectories;
                case Gizmox.WebGUI.Forms.AutoCompleteSource.HistoryList: return Base.AutoCompleteSource.HistoryList;
                case Gizmox.WebGUI.Forms.AutoCompleteSource.ListItems: return Base.AutoCompleteSource.ListItems;
                case Gizmox.WebGUI.Forms.AutoCompleteSource.None: return Base.AutoCompleteSource.None;
                case Gizmox.WebGUI.Forms.AutoCompleteSource.RecentlyUsedList: return Base.AutoCompleteSource.RecentlyUsedList;
            }
            return (Base.AutoCompleteSource)autoCompleteSource;
        }

        ///<summary>
        /// Gets the Gizmox.WebGUI.Forms AutoCompleteSource equivalent to the provided Habanero AutoCompleteSource
        ///</summary>
        ///<param name="autoCompleteSource">A Habanero  AutoCompleteSource.</param>
        ///<returns>The equivalent Gizmox.WebGUI.Forms  AutoCompleteSource.</returns>
        public static Gizmox.WebGUI.Forms.AutoCompleteSource GetAutoCompleteSource(Base.AutoCompleteSource autoCompleteSource)
        {
            switch (autoCompleteSource)
            {
                case Base.AutoCompleteSource.AllSystemSources: return Gizmox.WebGUI.Forms.AutoCompleteSource.AllSystemSources;
                case Base.AutoCompleteSource.AllUrl: return Gizmox.WebGUI.Forms.AutoCompleteSource.AllUrl;
                case Base.AutoCompleteSource.CustomSource: return Gizmox.WebGUI.Forms.AutoCompleteSource.CustomSource;
                case Base.AutoCompleteSource.FileSystem: return Gizmox.WebGUI.Forms.AutoCompleteSource.FileSystem;
                case Base.AutoCompleteSource.FileSystemDirectories: return Gizmox.WebGUI.Forms.AutoCompleteSource.FileSystemDirectories;
                case Base.AutoCompleteSource.HistoryList: return Gizmox.WebGUI.Forms.AutoCompleteSource.HistoryList;
                case Base.AutoCompleteSource.ListItems: return Gizmox.WebGUI.Forms.AutoCompleteSource.ListItems;
                case Base.AutoCompleteSource.None: return Gizmox.WebGUI.Forms.AutoCompleteSource.None;
                case Base.AutoCompleteSource.RecentlyUsedList: return Gizmox.WebGUI.Forms.AutoCompleteSource.RecentlyUsedList;
            }
            return (Gizmox.WebGUI.Forms.AutoCompleteSource)autoCompleteSource;
        }
    }
}
