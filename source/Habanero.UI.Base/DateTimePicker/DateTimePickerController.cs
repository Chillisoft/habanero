using System;
using System.Reflection;
using Habanero.UI;

namespace Habanero.UI.Base
{
    public abstract class DateTimePickerController
    {
        //private readonly IControlFactory _controlFactory;

        private IControlFactory _controlFactory;
        //private IChilliControl _nullValueCover;

        
        public DateTimePickerController(IControlFactory controlFactory)
        {

        }


        ///<summary>
        /// The DateTimePicker control being controlled
        ///</summary>
        public abstract IDateTimePicker DateTimePicker { get;}

        public abstract DateTime? Value { get; set;}
    }
}