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
using System;
using System.Collections;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Manages property definitions for a column in a user interface grid,
    /// as specified in the class definitions xml file
    /// </summary>
    public class UIGridColumn : IUIGridColumn
    {
        private string _heading;
        private string _propertyName;
        private Type _gridControlType;
        protected IPropDef _propDef;
        /// <summary>
        /// Constructor to initialise a new definition
        /// </summary>
        /// <param name="heading">The heading</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="gridControlTypeName">The Name of the Grid Control Type</param>
        /// <param name="gridControlAssembly">The Assembly Name of the Grid Control Type</param>
        /// <param name="editable">Whether the grid is read-only (cannot be
        /// edited directly)</param>
        /// <param name="width">The width</param>
        /// <param name="alignment">The horizontal alignment</param>
        /// <param name="parameters">The parameters for the column</param>
        public UIGridColumn(string heading, string propertyName, String gridControlTypeName, String gridControlAssembly, bool editable, int width,
                            PropAlignment alignment, Hashtable parameters)
        {
            _heading = heading;
            _propertyName = propertyName;
            GridControlTypeName = gridControlTypeName;
            GridControlAssemblyName = gridControlAssembly;
            Editable = editable;
            Width = width;
            Alignment = alignment;
            Parameters = parameters ?? new Hashtable();
        }

        /// <summary>
        /// Constructor to initialise a new definition
        /// </summary>
        /// <param name="heading">The heading</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="gridControlType">The grid control type.  This cannot be null -
        /// if you need to supply null type parameters use the constructor that supplies
        /// specific type and assembly names and set these as null.</param>
        /// <param name="editable">Whether the grid is read-only (cannot be
        /// edited directly)</param>
        /// <param name="width">The width</param>
        /// <param name="alignment">The horizontal alignment</param>
        /// <param name="parameters">The parameters for the column</param>
        public UIGridColumn(string heading, string propertyName, Type gridControlType, bool editable, int width,
                            PropAlignment alignment, Hashtable parameters)
            : this(heading, propertyName, gridControlType.Name, gridControlType.Namespace, editable, width, alignment, parameters)
        {
        }


        /// <summary>
        /// Returns the heading text that will be used for this column.
        /// </summary>
        public string Heading
        {
            get { return _heading; }
            set { _heading = value; }
        }

        /// <summary>
        /// Returns the property name
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
            set
            {
                _propertyName = value;
                this._propDef = null;
            }
        }

        /// <summary>
        /// Returns the grid control type
        /// </summary>
        public Type GridControlType
        {
            get { return _gridControlType; }
            set
            {
                _gridControlType = value;
                GridControlTypeName = _gridControlType.Name;
                GridControlAssemblyName = _gridControlType.Namespace;
            }
        }

        /// <summary>
        /// Indicates whether the column is editable
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// Returns the width
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Returns the horizontal alignment
        /// </summary>
        public PropAlignment Alignment { get; set; }

        /// <summary>
        /// Returns the Hashtable containing the property parameters
        /// </summary>
        public Hashtable Parameters { get; private set; }

        /// <summary>
        /// Gets and sets the name of the grid control type
        /// </summary>
        public string GridControlTypeName { get; set; }

        /// <summary>
        /// Gets and sets the assembly name of the grid control type
        /// </summary>
        public string GridControlAssemblyName { get; set; }

        ///<summary>
        /// The <see cref="IUIGrid">Grid Definition</see> that this IUIGridColumn belongs to.
        ///</summary>
        public IUIGrid UIGrid { get; set; }

        ///<summary>
        /// The <see cref="IClassDef">ClassDefinition</see> that this IUIGridColumn belongs to
        ///</summary>
        public virtual IClassDef ClassDef
        {
            get { return this.UIGrid == null ? null : this.UIGrid.ClassDef; }
        }
        /// <summary>
        /// Returns the LookupList for the PropDef that 
        /// is associated with this PropDef.
        /// If there is no PropDef associated with this column
        /// then returns <see cref="NullLookupList"/>.
        /// </summary>
        public ILookupList LookupList
        {
            get
            {
                return this.ClassDef == null 
                    ? new NullLookupList() 
                    : this.ClassDef.GetLookupList(this.PropertyName);
            }
        }

        #region Helper Methods

        ///<summary>
        /// Gets the heading for this grid column.
        ///</summary>
        ///<returns> The heading for this grid column </returns>
        public string GetHeading()
        {
            return GetHeading(this.ClassDef);
        }

        ///<summary>
        /// Gets the heading for this grid column given a classDef.
        ///</summary>
        ///<param name="classDef">The class definition that corresponds to this grid column. </param>
        ///<returns> The heading for this grid column </returns>
        public string GetHeading(IClassDef classDef)
        {
            if (!String.IsNullOrEmpty(_heading))
            {
                return _heading;
            }
            string heading = null;
            IPropDef propDef = ClassDefHelper.GetPropDefByPropName(classDef, PropertyName);
            if (propDef != null)
            {
                heading = propDef.DisplayName;
            }
            if (String.IsNullOrEmpty(heading))
            {
                heading = StringUtilities.DelimitPascalCase(_propertyName, " ");
            }
            return heading;
        }
        ///<summary>
        /// Gets the heading for this grid column.
        ///</summary>
        ///<returns> The heading for this grid column </returns>
        public Type GetPropertyType()
        {
            if (PropDef == null) return ClassDef.GetPropertyType(PropertyName);
            if (this.PropDef.HasLookupList()) return typeof (object);
            return PropDef.PropertyType;
        }

        private IPropDef PropDef
        {
            get
            {
                if (_propDef != null) return _propDef;
                _propDef = ClassDefHelper.GetPropDefByPropName(ClassDef, PropertyName);
                return _propDef;
            }
        }

        #endregion //Helper Methods

        /// <summary>
        /// Returns the parameter value for the name provided
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <returns>Returns the parameter value or null if not found</returns>
        public object GetParameterValue(string parameterName)
        {
            return Parameters.ContainsKey(parameterName) ? Parameters[parameterName] : null;
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
            if (obj == null) return false;

            UIGridColumn otherGridColumn = obj as UIGridColumn;
            if (otherGridColumn == null) return false;
            if ((otherGridColumn.PropertyName != this.PropertyName) 
                || (otherGridColumn.Heading != this.Heading) 
                || (otherGridColumn.GridControlTypeName != this.GridControlTypeName)
                || (otherGridColumn.Editable != this.Editable)) return false;
            return true;
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (this.PropertyName + this.Heading + this.GridControlTypeName + this.Editable).GetHashCode();
        }

        ///<summary>
        /// overloads the operator == 
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns></returns>
        public static bool operator ==(UIGridColumn a, UIGridColumn b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
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
        public static bool operator !=(UIGridColumn a, UIGridColumn b)
        {
            return !(a == b);
        }

        ///<summary>
        /// Clones the collection of ui columns this performs a copy of all uicolumns but does not copy the uiFormFields.
        ///</summary>
        ///<returns>a new collection that is a shallow copy of this collection</returns>
        public IUIGridColumn Clone()
        {
            return new UIGridColumn(this.Heading,
                this.PropertyName,this.GridControlTypeName,this.GridControlAssemblyName ,
                this.Editable,this.Width,this.Alignment, this.Parameters);
        }
    }
}