//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Habanero.Util
{
    /// <summary>
    /// These utilities provide quick methods for retrieving information from a user dialog.
    /// For example: an open file name or select folder name dialog.
    /// </summary>
    public static class DialogUtilities
    {
        #region Select Folder

        /// <summary>
        /// This method shows a dialog for the user to choose a folder
        /// </summary>
        /// <param name="selectedPath">The folder that was selected by the user</param>
        /// <returns>Returns a value representing whether the folder was chosen or not</returns>
        public static bool SelectFolderDialog(out string selectedPath)
        {
            return SelectFolderDialog(out selectedPath, null);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a folder
        /// </summary>
        /// <param name="selectedPath">The folder that was selected by the user</param>
        /// <param name="currentFolder">The folder to start navigating from</param>
        /// <returns>Returns a value representing whether the folder was chosen or not</returns>
        public static bool SelectFolderDialog(out string selectedPath, string currentFolder)
        {
            return SelectFolderDialog(out selectedPath, null, currentFolder);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a folder
        /// </summary>
        /// <param name="selectedPath">The folder that was selected by the user</param>
        /// <param name="currentFolder">The folder to start navigating from</param>
        /// <param name="description">The title of the dialog</param>
        /// <returns>Returns a value representing whether the folder was chosen or not</returns>
        public static bool SelectFolderDialog(out string selectedPath, string currentFolder, string description)
        {
            return SelectFolderDialog(null, out selectedPath, currentFolder, description);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a folder
        /// </summary>
        /// <param name="owner">The owner window for the dialog</param>
        /// <param name="selectedPath">The folder that was selected by the user</param>
        /// <returns>Returns a value representing whether the folder was chosen or not</returns>
        public static bool SelectFolderDialog(IWin32Window owner, out string selectedPath)
        {
            return SelectFolderDialog(owner, out selectedPath, null);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a folder
        /// </summary>
        /// <param name="owner">The owner window for the dialog</param>
        /// <param name="selectedPath">The folder that was selected by the user</param>
        /// <param name="currentFolder">The folder to start navigating from</param>
        /// <returns>Returns a value representing whether the folder was chosen or not</returns>
        public static bool SelectFolderDialog(IWin32Window owner, out string selectedPath, string currentFolder)
        {
            return SelectFolderDialog(owner, out selectedPath, null, currentFolder);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a folder
        /// </summary>
        /// <param name="owner">The owner window for the dialog</param>
        /// <param name="selectedPath">The folder that was selected by the user</param>
        /// <param name="currentFolder">The folder to start navigating from</param>
        /// <param name="description">The title of the dialog</param>
        /// <returns>Returns a value representing whether the folder was chosen or not</returns>
        public static bool SelectFolderDialog(IWin32Window owner, out string selectedPath, string currentFolder, string description)
        {
            FolderBrowserDialog browserDialog = new FolderBrowserDialog();
            string currentPath = currentFolder;
            if (!String.IsNullOrEmpty(currentPath))
            {
                browserDialog.SelectedPath = currentPath;
            }
            if (!String.IsNullOrEmpty(description))
            {
                browserDialog.Description = description;
            }
            DialogResult result = browserDialog.ShowDialog(owner);
            selectedPath = browserDialog.SelectedPath;
            return result == DialogResult.OK;
        }

        #endregion //Select Folder

        #region Open File Dialog

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetOpenFileName(out string fileName)
        {
            return GetOpenFileName(out fileName, null);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetOpenFileName(out string fileName, string defaultFileName)
        {
            return GetOpenFileName(out fileName, null, null, defaultFileName);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetOpenFileName(out string fileName, string extension, string extensionDescription)
        {
            return GetOpenFileName(out fileName, extension, extensionDescription, null);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetOpenFileName(out string fileName, string extension, string extensionDescription, string defaultFileName)
        {
            return GetOpenFileName(null, out fileName, extension, extensionDescription, defaultFileName);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <param name="title">The title that will be displayed on the dialog</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetOpenFileName(out string fileName, string extension, string extensionDescription,
            string defaultFileName, string title)
        {
            return GetOpenFileName(null, out fileName, extension, extensionDescription, defaultFileName, title);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="owner">The owner window for the dialog</param>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetOpenFileName(IWin32Window owner, out string fileName)
        {
            return GetOpenFileName(owner, out fileName, null);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="owner">The owner window for the dialog</param>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetOpenFileName(IWin32Window owner, out string fileName, string defaultFileName)
        {
            return GetOpenFileName(owner, out fileName, null, null, defaultFileName);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="owner">The owner window for the dialog</param>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetOpenFileName(IWin32Window owner, out string fileName, string extension, string extensionDescription)
        {
            return GetOpenFileName(owner, out fileName, extension, extensionDescription, null);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="owner">The owner window for the dialog</param>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetOpenFileName(IWin32Window owner, out string fileName, string extension, string extensionDescription, 
            string defaultFileName)
        {
            return GetOpenFileName(owner, out fileName, extension, extensionDescription, defaultFileName, null);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="owner">The owner window for the dialog</param>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <param name="title">The title that will be displayed on the dialog</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetOpenFileName(IWin32Window owner, out string fileName, string extension, string extensionDescription, 
            string defaultFileName, string title)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.AddExtension = true;
            openFileDialog.Multiselect = false;
            if (!String.IsNullOrEmpty(title))
            {
                openFileDialog.Title = title;
            }
            return ShowFileDialog(openFileDialog, owner, out fileName, extension, extensionDescription, defaultFileName);
        }

        #endregion //Open File Dialog

        #region Save File Dialog

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetSaveFileName(out string fileName)
        {
            return GetSaveFileName(out fileName, null);
        }


        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetSaveFileName(out string fileName, string defaultFileName)
        {
            return GetSaveFileName(out fileName, null, null, defaultFileName);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetSaveFileName(out string fileName, string extension, string extensionDescription)
        {
            return GetSaveFileName(out fileName, extension, extensionDescription, null);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetSaveFileName(out string fileName, string extension, string extensionDescription,
            string defaultFileName)
        {
            return GetSaveFileName(out fileName, extension, extensionDescription, defaultFileName, null);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <param name="title">The title that will be displayed on the dialog</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetSaveFileName(out string fileName, string extension, string extensionDescription,
            string defaultFileName, string title)
        {
            return GetSaveFileName(out fileName, extension, extensionDescription, defaultFileName,
                title, true);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <param name="title">The title that will be displayed on the dialog</param>
        /// <param name="overwritePrompt">Should the dialog prompt the user when the file will overwrite another file?</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetSaveFileName(out string fileName, string extension, string extensionDescription,
            string defaultFileName, string title, bool overwritePrompt)
        {
            return GetSaveFileName(null, out fileName, extension, extensionDescription, defaultFileName,
                title, overwritePrompt, false);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="owner">The owner window for the dialog</param>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetSaveFileName(IWin32Window owner, out string fileName, string defaultFileName)
        {
            return GetSaveFileName(owner, out fileName, null, null, defaultFileName);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="owner">The owner window for the dialog</param>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetSaveFileName(IWin32Window owner, out string fileName, string extension, string extensionDescription)
        {
            return GetSaveFileName(owner, out fileName, extension, extensionDescription, null);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="owner">The owner window for the dialog</param>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetSaveFileName(IWin32Window owner, out string fileName, string extension, string extensionDescription,
            string defaultFileName)
        {
            return GetSaveFileName(owner, out fileName, extension, extensionDescription, defaultFileName, null);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="owner">The owner window for the dialog</param>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <param name="title">The title that will be displayed on the dialog</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetSaveFileName(IWin32Window owner, out string fileName, string extension, string extensionDescription,
            string defaultFileName, string title)
        {
            return GetSaveFileName(owner, out fileName, extension, extensionDescription, defaultFileName,
                title, true);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="owner">The owner window for the dialog</param>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <param name="title">The title that will be displayed on the dialog</param>
        /// <param name="overwritePrompt">Should the dialog prompt the user when the file will overwrite another file?</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetSaveFileName(IWin32Window owner, out string fileName, string extension, string extensionDescription,
            string defaultFileName, string title, bool overwritePrompt)
        {
            return GetSaveFileName(owner, out fileName, extension, extensionDescription, defaultFileName,
                title, overwritePrompt, false);
        }

        /// <summary>
        /// This method shows a dialog for the user to choose a file to open
        /// </summary>
        /// <param name="owner">The owner window for the dialog</param>
        /// <param name="fileName">The file that was selected by the user</param>
        /// <param name="extension">The default file extension</param>
        /// <param name="extensionDescription">A description of the default file extension</param>
        /// <param name="defaultFileName">The default file to open</param>
        /// <param name="title">The title that will be displayed on the dialog</param>
        /// <param name="overwritePrompt">Should the dialog prompt the user when the file will overwrite another file?</param>
        /// <param name="createPrompt">Should the dialog prompt the user when the file will be created?</param>
        /// <returns>Returns a value representing whether a file was chosen or not</returns>
        public static bool GetSaveFileName(IWin32Window owner, out string fileName, string extension, string extensionDescription, 
            string defaultFileName, string title, bool overwritePrompt, bool createPrompt)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.CreatePrompt = createPrompt;
            saveFileDialog.OverwritePrompt = overwritePrompt;
            if (!String.IsNullOrEmpty(title))
            {
                saveFileDialog.Title = title;
            }
            return ShowFileDialog(saveFileDialog, owner, out fileName, extension, extensionDescription, defaultFileName);
            //string allFilesFilter = "All files (*.*)|*.*";
            //if (!String.IsNullOrEmpty(extension))
            //{
            //    saveFileDialog.Filter = String.Format("{0} (*.{1})|*.{1}|" + allFilesFilter, extensionDescription, extension);
            //    saveFileDialog.DefaultExt = extension;
            //}
            //else
            //{
            //    saveFileDialog.Filter = allFilesFilter;
            //}
            //saveFileDialog.FileName = defaultFileName;
            //DialogResult result = saveFileDialog.ShowDialog(this);
            //if (result == DialogResult.OK)
            //{
            //    fileName = saveFileDialog.FileName;
            //    return true;
            //}
            //else
            //{
            //    fileName = "";
            //    return false;
            //}
        }

        #endregion //Save File Dialog

        #region Show File Dialog

        private static bool ShowFileDialog(FileDialog openFileDialog, IWin32Window owner, out string fileName, string extension, string extensionDescription, string defaultFileName)
        {
            string allFilesFilter = "All files (*.*)|*.*";
            if (!String.IsNullOrEmpty(extension))
            {
                openFileDialog.Filter = String.Format("{0} (*.{1})|*.{1}|" + allFilesFilter, extensionDescription, extension);
                openFileDialog.DefaultExt = extension;
            }
            else
            {
                openFileDialog.Filter = allFilesFilter;
            }
            if (!String.IsNullOrEmpty(defaultFileName))
            {
                openFileDialog.FileName = defaultFileName;
            }
            DialogResult result;
            if (owner != null)
            {
                result = openFileDialog.ShowDialog(owner);
            }
            else
            {
                result = openFileDialog.ShowDialog();
            }
            if (result == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
                return true;
            }
            else
            {
                fileName = "";
                return false;
            }
        }

        #endregion //Show File Dialog

    }
}
