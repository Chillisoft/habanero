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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides an interface for a control to edit an individual Business object,. 
    /// The control that is using this control can obtain and display any errors that have occured for the Business object associated with this control.
    /// The list of errors can be obtained via the Errors method.
    /// </summary>
    public interface IBOEditorControl : IBusinessObjectControl
    {
        /// <summary>
        /// Applies any changes that have occured in any of the Controls on this control's to their related
        /// Properties on the Business Object.
        /// </summary>
        void ApplyChangesToBusinessObject();
        /// <summary>
        /// Does the business object controlled by this control or any of its Aggregate or Composite children have and Errors.
        /// </summary>
        bool HasErrors { get; }
        /// <summary>
        /// Does the Business Object controlled by this control or any of its Aggregate or Composite children have and warnings.
        /// </summary>
        bool HasWarning { get; }
        /// <summary>
        ///  Returns a list of all warnings for the business object controlled by this control or any of its children.
        /// </summary>
        ErrorList Errors { get; }
        /// <summary>
        /// Does the business object being managed by this control have any edits that have not been persisted.
        /// </summary>
        /// <returns></returns>
        bool IsDirty { get; }
        /// <summary>
        /// Returns a list of all warnings for the business object controlled by this control or any of its children.
        /// </summary>
        /// <returns></returns>
        ErrorList Warnings { get; }
//
//        /// <summary>
//        /// Hides all the error providers.  Typically used where a new object has just
//        /// been added and the interface is being cleaned up.
//        /// </summary>
//        void ClearErrors();
    }
    /// <summary>
    /// This is an interface for the Windows and VWG controls that implement the 
    /// ability to edit a Business Object where the business object is being edited via
    ///  a Panel with the associated <see cref="PanelInfo"/>s and <see cref="PanelInfo.FieldInfo"/>s.<br/>
    /// The <see cref="IPanelInfo"/> is built from the <see cref="UIDef"/> that is part of the <see cref="IClassDef"/><br/>
    /// This interface therefore implements both the <see cref="IBOEditorControl"/> and the <see cref="IBusinessObjectPanel"/>
    /// </summary>
    public interface IBOPanelEditorControl : IBOEditorControl, IBusinessObjectPanel
    {

    }

    /// <summary>
    /// A ReadOnly collection of Errors Or Warnings for a Business Object and its children Business objects.
    /// </summary>
    public class ErrorList:ReadOnlyCollectionBase
    {
    }
}