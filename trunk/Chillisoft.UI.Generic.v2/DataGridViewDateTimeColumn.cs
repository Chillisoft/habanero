using System;
using System.Windows.Forms;
namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Represents a column in data grid view
    /// </summary>
    public class DataGridViewDateTimeColumn : DataGridViewColumn
    {
        /// <summary>
        /// Constructor to initialise a new column
        /// </summary>
        public DataGridViewDateTimeColumn()
            : base(new CalendarCell())
        {
        }

        /// <summary>
        /// Gets and sets the cell template
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(CalendarCell)))
                {
                    throw new InvalidCastException("Must be a CalendarCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}
