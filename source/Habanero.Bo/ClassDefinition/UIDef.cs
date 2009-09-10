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

using System;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Defines how the properties of a class are displayed in a user
    /// interface, as specified in the class definitions xml file.
    /// This consists of definitions for a grid display and an editing form.
    /// </summary>
    public class UIDef : IEquatable<UIDef>, IUIDef
    {
        private string _name;
        private IUIForm _uiForm;
        private IUIGrid _uiGrid;
//        private ClassDef _classDef;

        /// <summary>
        /// Constructor to initialise a new definition with the name, form
        /// and grid definitions provided
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="uiForm">The form definition</param>
        /// <param name="uiGrid">The grid definition</param>
        public UIDef(string name, IUIForm uiForm, IUIGrid uiGrid)
        {
            _name = name;
            _uiForm = uiForm;
            if (_uiForm != null) _uiForm.UIDef = this;
            _uiGrid = uiGrid;
        }

        /// <summary>
        /// Returns the form definition
        /// </summary>
        public IUIForm UIForm
        {
            get { return _uiForm; }
            set { _uiForm = value; }
        }

        /// <summary>
        /// Returns the name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Returns the grid definition
        /// </summary>
        public IUIGrid UIGrid
        {
            get { return _uiGrid; }
            set { _uiGrid = value; }
        }

        ///<summary>
        /// Gets a Collection of UIDefs
        ///</summary>
        public UIDefCol UIDefCol { get; set; }

        /// <summary>
        /// The Class Definition that this UIDef belongs to.
        /// </summary>
        public IClassDef ClassDef { get; set; }

        /// <summary>
        /// Returns the form property definitions
        /// </summary>
        /// <returns>Returns a UIForm object</returns>
        [Obsolete("Please use the UIForm property instead as it returns the same UIForm. This method will be removed in later versions of Habanero")]
        public IUIForm GetUIFormProperties()
        {
            return this.UIForm;
        }

        /// <summary>
        /// Returns the grid property definitions
        /// </summary>
        /// <returns>Returns a UIGridDef object</returns>
        [Obsolete("Please use the UIGrid property instead as it returns the same UIGrid. This method will be removed in later versions of Habanero")]
        public IUIGrid GetUIGridProperties()
        {
            return this.UIGrid;
        }

        ///<summary>
        /// Returns the form field for this UIDefinition for the property specified.
        /// If the form field for the property is not defined in the uidef then null is returned.
        ///</summary>
        ///<param name="propertyName">The property name that you want the form field for</param>
        ///<returns>the form field or null</returns>
        public IUIFormField GetFormField(string propertyName)
        {
            IUIForm formProperties = this.UIForm;

            foreach (UIFormTab tab in formProperties)
            {
                foreach (UIFormColumn column in tab)
                {
                    foreach (UIFormField field in column)
                    {
                        if (field.PropertyName == propertyName)
                        {
                            return field;
                        }
                    }
                }
            }
            return null;
        }
        ///<summary>
        /// overloads the operator == 
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns></returns>
        public static bool operator ==(UIDef a, UIDef b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object) a == null) || ((object) b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        ///<summary>
        /// overloads the operator != 
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns></returns>
        public static bool operator !=(UIDef a, UIDef b)
        {
            return !(a == b);
        }

        ///<summary>
        /// Clones the collection of ui columns this performs a copy of all uicolumns but does not copy the uiFormFields.
        ///</summary>
        ///<returns>a new collection that is a shallow copy of this collection</returns>
        public IUIDef Clone()
        {
            IUIForm clonedForm = this.UIForm != null? ((UIForm) this.UIForm).Clone(): null;
            IUIGrid clonedGrid = this.UIGrid != null ? ((UIGrid)this.UIGrid).Clone() : null;
            UIDef newUIForm = new UIDef(this.Name, clonedForm, clonedGrid);
            return newUIForm;
        }

        ///<summary>
        ///Indicates whether the current object is equal to another object of the same type.
        ///</summary>
        ///
        ///<returns>
        ///true if the current object is equal to the other parameter; otherwise, false.
        ///</returns>
        ///
        ///<param name="otherUIDef">An object to compare with this object.</param>
        public bool Equals(UIDef otherUIDef)
        {
            if (otherUIDef == null) return false;
            if (this.Name != otherUIDef.Name) return false;
            if ((UIForm)this.UIForm != (UIForm)otherUIDef.UIForm) return false;
            if ((UIGrid)this.UIGrid != (UIGrid)otherUIDef.UIGrid) return false;
            return true;
        }
        ///<summary>
        ///Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        ///</returns>
        ///
        ///<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as UIDef);
        }

        ///<summary>
        ///Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the current <see cref="T:System.Object"></see>.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            int result = _name != null ? _name.GetHashCode() : 0;
            result = 29*result + (_uiForm != null ? _uiForm.GetHashCode() : 0);
            result = 29*result + (_uiGrid != null ? _uiGrid.GetHashCode() : 0);
            return result;
        }
    }
}