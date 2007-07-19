using System;

namespace Habanero.Base
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
        /// <param name="gridControlType">The grid control type</param>
        /// <param name="editable">Whether the grid is read-only (cannot be
        /// edited directly)</param>
        /// <param name="width">The width</param>
        /// <param name="alignment">The horizontal alignment</param>
        public UIGridColumn(string heading, string propertyName, Type gridControlType, bool editable, int width,
                              PropAlignment alignment)
        {
            _heading = heading;
            _propertyName = propertyName;
            _gridControlType = gridControlType;
            _editable = editable;
            _width = width;
            this._alignment = alignment;
        }

        /// <summary>
        /// Returns the heading
        /// </summary>
        public string Heading
        {
            get { return _heading; }
			protected set { _heading = value; }
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
			protected set { _gridControlType = value; }
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
    }
}