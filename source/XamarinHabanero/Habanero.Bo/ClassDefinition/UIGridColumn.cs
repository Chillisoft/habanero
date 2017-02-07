#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Collections;
using System.Reflection;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Manages property definitions for a column in a user interface grid,
    /// usually as specified in the class definitions xml file.
    /// The UIGridColumn can be for 
    /// <li>A Reflective Property e.g. 
    /// an Order BO may have a property "CustomerName" that is declared on the 
    /// Order BO class directly but is derived from the Customer class and therefore 
    /// does not have a PropDef associated with it.</li>
    /// <li>A Defined Property i.e. a normal property of the BO that is mapped to a column 
    /// in the database e.g. for Order BO an OrderNumber. This property will have a 
    /// PropDef.</li>
    /// <li>A Related Property Habanero has the ability to define a property for a grid etc.
    /// via its relationships e.g. for a grid showing order details we may want to show the 
    /// CustomerName and CustomerCode. We could define these as properties on the Order BO and
    /// use Reflective Properties (as above) or we could declare the Related Properties i.e.
    /// Customer.CustomerName and Customer.CustomerCode. Habanero will then find the PropDef for the
    /// Related Property via its defined relationships.
    /// A Related Property can be defined via any of the Business Objects single Relationships.</li>
    /// </summary>
    public class UIGridColumn : IUIGridColumn
    {
        private string _propertyName;
        private Type _gridControlType;
        /// <summary>
        /// The <see cref="IPropDef"/> this column is related to.
        /// </summary>
        protected IPropDef _propDef;
        private bool _editable;

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
        public UIGridColumn(string heading, string propertyName, String gridControlTypeName, String gridControlAssembly,
                            bool editable, int width,
                            PropAlignment alignment, Hashtable parameters)
        {
            Heading = heading;
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
            : this(
                heading, propertyName, gridControlType.Name, gridControlType.Namespace, editable, width, alignment,
                parameters)
        {
        }


        /// <summary>
        /// Returns the heading text that has been defined specifically for this
        /// UIGridColumn. If this is null then the Heading determined via
        /// <see cref="GetHeading()"/> will be used. 
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// Returns the property name
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
            set
            {
                _propertyName = value;
                //If the PropName has changed then 
                // the cached propDef may no longer be valid.
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
        /// Indicates whether the column is editable.
        /// This takes into account the following.
        /// 1) Has the GridColumn Been set to Not Editable Explicitely e.g. via ClassDef.xml.
        /// 2) Is the PropDef in a Non editable state e.g. PropDef ReadWriteStatus is ReadOnly
        /// 3) If there is no PropDef then does the reflective PropInfo have a Setter.
        /// </summary>
        public bool Editable
        {
            get { return DetermineIsEditable(); }
            set
            {
                _editable = value;
            }
        }

        /// <summary>
        /// Updates the isEditable flag and updates 
        /// the control according to the current state
        /// </summary>
        private bool DetermineIsEditable()
        {
            if (!_editable) return false;
            return IsPropertyReflective()
                       ? DoesVirtualPropertyHaveSetter()
                       : PropDefIsEditable();
        }
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

#pragma warning disable 612,618
        ///<summary>
        /// Gets the heading for this grid column.
        ///</summary>
        ///<returns> The heading for this grid column </returns>
        public string GetHeading()
        {
            return GetHeading(this.ClassDef);
        }
#pragma warning restore 612,618
        ///<summary>
        /// Gets the heading for this grid column given a classDef.
        ///</summary>
        ///<param name="classDef">The class definition that corresponds to this grid column. </param>
        ///<returns> The heading for this grid column </returns>
        [Obsolete("This is no longer necessary since the UIGridColumn can now return its associated classDef.")]
        public string GetHeading(IClassDef classDef)
        {
            //If the heading is overriden specifically for this UIGridColumn
            // i.e. it is not derived from the DisplayName of the 
            // PropDef then use it.
            if (!String.IsNullOrEmpty(Heading))
            {
                return Heading;
            }
            //Else use the PropDefs Display name
            IPropDef propDef = ClassDefHelper.GetPropDefByPropName(classDef, PropertyName);
            if (propDef != null)
            {
                return propDef.DisplayName;
            }
            //Else use a Delimited Prop Name i.e. "FirstName" -> "First Name"
            return StringUtilities.DelimitPascalCase(_propertyName, " ");
        }

        ///<summary>
        /// Gets the heading for this grid column.
        ///</summary>
        ///<returns> The heading for this grid column </returns>
        public Type GetPropertyType()
        {
            if (PropDef == null) return ClassDef.GetPropertyType(PropertyName);
            //If the Propdef has a Lookup then it is impossible for the
            // the Grid Column to know the datatype and it is not essential
            // since we will manually determine the Control type.
            // e.g. If Readonly then TextBox Column else ComboBox Column.
            if (this.PropDef.HasLookupList()) return typeof (object);
            return PropDef.PropertyType;
        }

        /// <summary>
        /// Returns the PropDef that is associated with this UIGridColumn.
        /// If one is associaciated. Returns null otherwise
        /// </summary>
        public virtual IPropDef PropDef
        {
            get
            {
                if (_propDef != null) return _propDef;
                _propDef = ClassDefHelper.GetPropDefByPropName(ClassDef, PropertyName);
                return _propDef;
            }
        }

        /// <summary>
        /// Return true if this UIGridColumn is associated with a <see cref="IPropDef"/>.
        /// This is used since a GridColumn can be associated with
        /// Reflective Property 
        /// </summary>
        public bool HasPropDef
        {
            get { return this.PropDef != null; }
        }


        private bool PropDefIsEditable()
        {
            return !PropDefIsReadOnly();
        }

        private bool PropDefIsReadOnly()
        {
            return (this.PropDef != null && this.PropDef.ReadWriteRule == PropReadWriteRule.ReadOnly);
        }

        private bool DoesVirtualPropertyHaveSetter()
        {
            if(this.ClassDef == null) return false;
            string virtualPropName = PropertyName.Trim('-');
            PropertyInfo propertyInfo =
                ReflectionUtilities.GetPropertyInfo(this.ClassDef.ClassType, virtualPropName);
            bool virtualPropertySetExists = propertyInfo != null && propertyInfo.CanWrite;
            return virtualPropertySetExists;
        }


        private bool IsPropertyReflective()
        {
            return PropertyName.IndexOf("-") != -1 || (this.PropDef == null && this.ClassDef != null);
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
                                    this.PropertyName, this.GridControlTypeName, this.GridControlAssemblyName,
                                    this.Editable, this.Width, this.Alignment, this.Parameters);
        }
    }
}