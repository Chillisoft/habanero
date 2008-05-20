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
using System.Collections;
using Habanero.Util;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Manages property definitions for a column in a user interface grid,
    /// as specified in the class definitions xml file
    /// </summary>
    public class UIGridColumn
    {
        private string _heading;
        private string _propertyName;
        private Type _gridControlType;
        private bool _editable;
        private int _width;
        private PropAlignment _alignment;
        private readonly Hashtable _parameters;
        private string _gridControlTypeName;
        private string _gridControlAssemblyName;

        /// <summary>
        /// An enumeration to specify a horizontal alignment in a grid
        /// </summary>
        public enum PropAlignment
        {
            left,
            right,
            centre
        }

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
        /// /// <param name="parameters">The parameters for the column</param>
        public UIGridColumn(string heading, string propertyName, String gridControlTypeName, String gridControlAssembly, bool editable, int width,
                            PropAlignment alignment, Hashtable parameters)
        {
            _heading = heading;
            _propertyName = propertyName;
            _gridControlTypeName = gridControlTypeName;
            _gridControlAssemblyName = gridControlAssembly;
            _editable = editable;
            _width = width;
            _alignment = alignment;
            _parameters = parameters;
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
        /// Returns the heading
        /// </summary>
        internal string Heading
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
            protected set { _propertyName = value; }
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
                _gridControlTypeName = _gridControlType.Name;
                _gridControlAssemblyName = _gridControlType.Namespace;
            }
        }

        /// <summary>
        /// Indicates whether the column is editable
        /// </summary>
        public bool Editable
        {
            get { return _editable; }
            protected set { _editable = value; }
        }

        /// <summary>
        /// Returns the width
        /// </summary>
        public int Width
        {
            get { return _width; }
            protected set { _width = value; }
        }

        /// <summary>
        /// Returns the horizontal alignment
        /// </summary>
        public PropAlignment Alignment
        {
            get { return _alignment; }
            protected set { _alignment = value; }
        }

        /// <summary>
        /// Returns the Hashtable containing the property parameters
        /// </summary>
        public Hashtable Parameters
        {
            get { return _parameters; }
        }
        /// <summary>
        /// Returns the Name of the Grid Control Type
        /// </summary>
        public String GridControlTypeName
        {
            get { return _gridControlTypeName; }
            set { _gridControlTypeName = value; }
        }

        /// <summary>
        /// Returns the Assembly Name of the Grid Control Type
        /// </summary>
        public String GridControlAssemblyName
        {
            get { return _gridControlAssemblyName; }
            set { _gridControlAssemblyName = value; }
        }

        #region Helper Methods

        ///<summary>
        /// Gets the heading for this grid column.
        ///</summary>
        ///<returns> The heading for this grid column </returns>
        public string GetHeading()
        {
            return GetHeading(null);
        }

        ///<summary>
        /// Gets the heading for this grid column given a classDef.
        ///</summary>
        ///<param name="classDef">The class definition that corresponds to this grid column. </param>
        ///<returns> The heading for this grid column </returns>
        public string GetHeading(ClassDef classDef)
        {
            if (!String.IsNullOrEmpty(_heading))
            {
                return _heading;
            }
            string heading = null;
            PropDef propDef = ClassDefHelper.GetPropDefByPropName(classDef, PropertyName);
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
       

        #endregion //Helper Methods

        /// <summary>
        /// Returns the parameter value for the name provided
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <returns>Returns the parameter value or null if not found</returns>
        public object GetParameterValue(string parameterName)
        {
            if (_parameters.ContainsKey(parameterName))
            {
                return _parameters[parameterName];
            }
            else
            {
                return null;
            }
        }
    }
}