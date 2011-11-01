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

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// The interface for a a Trigger which is a trigger set up to two properties so that when one changes
    /// the other can be notified.
    /// </summary>
    [Obsolete("This is no longer used")]
    public interface ITrigger
    {
        /// <summary>
        /// Gets the property name of the field, if any, which must have a value change
        /// in order for this trigger to be fired.  This property is used when the
        /// trigger has been created on the target - alternatively place the trigger on
        /// the source and indicate the Target.
        /// </summary>
        string TriggeredBy { get;  }

        /// <summary>
        /// Gets the property name of the field, if any, which will be affected by the
        /// designated action when the source field has had a value change.  This property
        /// is used when the trigger has been created on the source field with the value
        /// change - alternatively place the trigger on the Target and indicate
        /// the source using TriggeredBy.
        /// </summary>
        string Target { get;  }

        /// <summary>
        /// Gets and sets a condition value that the source field must
        /// hold in order to cause the trigger to fire.  In most cases you can
        /// simply specify the value as a string literal, but in lookup lists you
        /// can specify the string value as shown in the drop-down.
        /// </summary>
        string ConditionValue { get; set; }

        /// <summary>
        /// Gets and sets the name of the action to take when the trigger is
        /// ready to fire.  Possible options include:
        /// <ul>
        /// <li>assignLiteral - assigns the literal value as specified between the quotes
        /// in the value</li>
        /// <li>assignProperty - assigns the current value of the property given in the value</li>
        /// <li>filter - filters a lookup list with an exact match on the
        /// trigger property value (assumes that the target has a field with the same name
        /// as the trigger source)</li>
        /// <li>execute - calls a given method as provided in the value</li>
        /// <li>enable - enables/disables the control of the targer property, provide
        /// "true" or "false" as the value</li>
        /// </ul>
        /// </summary>
        string Action { get; set; }

        /// <summary>
        /// Gets and sets the relevant value to use for the action given.  See Action
        /// for further clarification of the options available.
        /// </summary>
        string Value { get; set; }
    }
}