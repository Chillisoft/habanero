// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    ///<summary>
    /// A panel info is a class that wraps the panel and provides functionality 
    ///  for linking a business object a layout manager and a panel.
    ///</summary>
    public class PanelInfo : IPanelInfo
    {
        private IBusinessObject _businessObject;

        ///<summary>
        /// Constructs a panel info with a list of fields and a list of children panel info.
        ///</summary>
        public PanelInfo()
        {
            FieldInfos = new FieldInfoCollection();
            PanelInfos = new List<IPanelInfo>();
        }

        ///<summary>
        /// The panel that this panel info is controlling
        ///</summary>
        public IPanel Panel { get; set; }

        ///<summary>
        /// Gets and sets the layout manager used for this Panel
        ///</summary>
        public GridLayoutManager LayoutManager { get; set; }

        ///<summary>
        /// Returns a list of Field infos (info on the fields controlled by this panel.
        ///</summary>
        public FieldInfoCollection FieldInfos { get; private set; }

        ///<summary>
        /// Sets the business object for this panel.
        ///</summary>
        public IBusinessObject BusinessObject
        {
            get { return _businessObject; }
            set
            {
                _businessObject = value;
                if (this.BusinessObject != null)
                    this.BusinessObject.IsValid(); //This causes the BO to run all its validation rules.
                for (int fieldInfoNum = 0; fieldInfoNum < FieldInfos.Count; fieldInfoNum++)
                {
                    FieldInfos[fieldInfoNum].ControlMapper.BusinessObject = value;
                }
            }
        }

        ///<summary>
        /// Sets whether the controls on this panel are enabled or not
        ///</summary>
        public bool ControlsEnabled
        {
            set
            {
                foreach (FieldInfo fieldInfo in FieldInfos)
                {
                    fieldInfo.ControlMapper.Control.Enabled = value;
                }
            }
        }

        ///<summary>
        /// A list of all panel infos containd in this panel info.
        ///</summary>
        public IList<IPanelInfo> PanelInfos { get; private set; }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public void ApplyChangesToBusinessObject()
        {
            try
            {
                for (int fieldInfoNum = 0; fieldInfoNum < FieldInfos.Count; fieldInfoNum++)
                {
                    FieldInfos[fieldInfoNum].ControlMapper.ApplyChangesToBusinessObject();
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        /// <summary>
        /// Sets all the error providers to have no errors.
        /// </summary>
        public void ClearErrorProviders()
        {
            foreach (FieldInfo fieldInfo in FieldInfos)
            {
                fieldInfo.ControlMapper.ErrorProvider.SetError(fieldInfo.InputControl, "");
            }
        }

        /// <summary>
        /// Gets the UIFormTab definition used to construct the panel
        /// for a single tab in the form.  By default, there is one
        /// tab for a form, even if it has not been explicitly defined.
        /// </summary>
        public IUIFormTab UIFormTab { get; internal set; }

        /// <summary>
        /// Gets and sets the minimum height for the panel
        /// </summary>
        public int MinimumPanelHeight { get; internal set; }

        /// <summary>
        /// Gets and sets the UIForm definition used to construct the
        /// panel - this is taken from the class definitions for the
        /// business object
        /// </summary>
        public IUIForm UIForm { get; internal set; }


        /// <summary>
        /// Gets the text for the panel tab (UIFormTab.Name)
        /// </summary>
        public string PanelTabText
        {
            get { return this.UIFormTab == null ? "" : this.UIFormTab.Name; }
        }

        /// <summary>
        /// Sets the Error providers Error message with the appropriate message from the businessObject for each
        /// Control mapped on this panel.
        /// </summary>
        public void UpdateErrorProvidersErrorMessages()
        {
            for (int fieldInfoNum = 0; fieldInfoNum < FieldInfos.Count; fieldInfoNum++)
            {
                FieldInfos[fieldInfoNum].ControlMapper.UpdateErrorProviderErrorMessage();
            }
        }

        ///<summary>
        /// An enumeration of all 
        ///</summary>
        public class FieldInfoCollection : IEnumerable<FieldInfo>
        {
            private readonly IList<FieldInfo> _fieldInfos = new List<FieldInfo>();

            ///<summary>
            /// Returns the Field Info identified by the PropertyName.
            ///</summary>
            ///<param name="propertyName"></param>
            ///<exception cref="InvalidPropertyNameException"></exception>
            public FieldInfo this[string propertyName]
            {
                get
                {
                    foreach (FieldInfo fieldInfo in _fieldInfos)
                    {
                        if (fieldInfo.PropertyName == propertyName)
                        {
                            return fieldInfo;
                        }
                    }
                    throw new InvalidPropertyNameException
                        (string.Format("A label for the property {0} was not found.", propertyName));
                }
            }

            ///<summary>
            /// Returns the field info at index
            ///</summary>
            ///<param name="index"></param>
            public FieldInfo this[int index]
            {
                get { return _fieldInfos[index]; }
            }

            ///<summary>
            /// Adds a new Field Info
            ///</summary>
            ///<param name="fieldInfo"></param>
            public void Add(FieldInfo fieldInfo)
            {
                _fieldInfos.Add(fieldInfo);
            }

            ///<summary>
            /// The number of fields
            ///</summary>
            public int Count
            {
                get { return _fieldInfos.Count; }
            }

            ///<summary>
            ///Returns an enumerator that iterates through the collection.
            ///</summary>
            ///
            ///<returns>
            ///A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>1</filterpriority>
            IEnumerator<FieldInfo> IEnumerable<FieldInfo>.GetEnumerator()
            {
                return _fieldInfos.GetEnumerator();
            }

            ///<summary>
            ///Returns an enumerator that iterates through a collection.
            ///</summary>
            ///
            ///<returns>
            ///An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>2</filterpriority>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return _fieldInfos.GetEnumerator();
            }
        }

        ///<summary>
        /// This class provides information on the control that is visible on a user form.
        /// It contains a reference to the label and the control mapper for a paticular control.
        ///</summary>
        public class FieldInfo
        {
            private readonly IControlHabanero _labelControl;
            private readonly string _propertyName;
            private readonly IControlMapper _controlMapper;

            ///<summary>
            /// Constructs a Field info.
            ///</summary>
            ///<param name="propertyName">The property that this field info is for</param>
            ///<param name="labelControl">The label that this field info is wrapping</param>
            ///<param name="controlMapper">The control mapper that this field info is mapping</param>
            public FieldInfo(string propertyName, IControlHabanero labelControl, IControlMapper controlMapper)
            {
                _propertyName = propertyName;
                _labelControl = labelControl;
                _controlMapper = controlMapper;
            }

            ///<summary>
            /// Returns the PropertyName of this <see cref="FieldInfo"/>
            ///</summary>
            public string PropertyName
            {
                get { return _propertyName; }
            }

            ///<summary>
            /// Returns the Label Control for this <see cref="FieldInfo"/>
            ///</summary>
            public IControlHabanero LabelControl
            {
                get { return _labelControl; }
            }

            ///<summary>
            /// Returns the Label Control for this <see cref="FieldInfo"/>
            ///</summary>
            [Obsolete(
                "Please use LabelControl as UIFormFields can be configured to return a GroupBox or other types of label controls"
                )]
            public ILabel Label
            {
                get { return (ILabel) _labelControl; }
            }

            ///<summary>
            /// Returns the Input control <see cref="FieldInfo"/>
            ///</summary>
            public IControlHabanero InputControl
            {
                get { return _controlMapper.Control; }
            }

            ///<summary>
            /// Returns the ControlMapper <see cref="FieldInfo"/>
            ///</summary>
            public IControlMapper ControlMapper
            {
                get { return _controlMapper; }
            }
        }
    }
}