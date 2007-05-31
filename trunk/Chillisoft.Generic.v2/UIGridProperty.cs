using System;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Manages property definitions for a column in a user interface grid,
    /// as specified in the class definitions xml file
    /// </summary>
    public class UIGridProperty
    {
        private string itsHeading;
        private string itsPropertyName;
        private Type itsGridControlType;
        private bool itsIsReadOnly;
        private int itsWidth;
        private readonly PropAlignment itsAlignment;

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
        /// <param name="isReadOnly">Whether the grid is read-only (cannot be
        /// edited directly)</param>
        /// <param name="width">The width</param>
        /// <param name="alignment">The horizontal alignment</param>
        public UIGridProperty(string heading, string propertyName, Type gridControlType, bool isReadOnly, int width,
                              PropAlignment alignment)
        {
            itsHeading = heading;
            itsPropertyName = propertyName;
            itsGridControlType = gridControlType;
            itsIsReadOnly = isReadOnly;
            itsWidth = width;
            this.itsAlignment = alignment;
        }

        /// <summary>
        /// Returns the heading
        /// </summary>
        public string Heading
        {
            get { return itsHeading; }
        }

        /// <summary>
        /// Returns the property name
        /// </summary>
        public string PropertyName
        {
            get { return itsPropertyName; }
        }

        /// <summary>
        /// Returns the grid control type
        /// </summary>
        public Type GridControlType
        {
            get { return itsGridControlType; }
        }

        /// <summary>
        /// Indicates whether the grid is read-only (cannot be edited directly)
        /// </summary>
        public bool IsReadOnly
        {
            get { return itsIsReadOnly; }
        }

        /// <summary>
        /// Returns the width
        /// </summary>
        public int Width
        {
            get { return itsWidth; }
        }

        /// <summary>
        /// Returns the horizontal alignment
        /// </summary>
        public PropAlignment Alignment
        {
            get { return itsAlignment; }
        }
    }
}