//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// Provides a super-class for property rules that test the validity of
    /// a property value.  If you would like to implement your own property
    /// rule checker, inherit from this class, override the
    /// IsPropValueValid method and add a constructor with the same arguments
    /// as this one and pass back these arguments to base().
    /// In the class definitions, in the 'rule'
    /// element under the relevant 'property', specify the class and assembly
    /// of your newly implemented class.
    /// </summary>
    public abstract class PropRuleBase : IPropRule
    {
        protected Dictionary<string, object> _parameters;

		/// <summary>
		/// Constructor to initialise a new property rule
		/// </summary>
		/// <param name="name">The name of the rule</param>
		/// <param name="message">This rule's failure message</param>
		///// <param name="parameters">The parameters for this rule.</param>
		protected PropRuleBase(string name, string message)
		{
			Name = name;
			Message = message;
			_parameters = FillParameters(AvailableParameters(), _parameters);
		}

        /// <summary>
        /// Indicates whether the property value is valid against the rules
        /// </summary>
        /// <param name="displayName">The property name being checked</param>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        public virtual bool IsPropValueValid(string displayName, Object propValue,
                                                         ref string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        /// <summary>
        /// Returns the list of parameters to the rule - individual pairs
        /// of rule type and rule value that make up the composite rule
        /// </summary>
    	public virtual Dictionary<string, object> Parameters
    	{
			get { return _parameters; }
			set
			{
				_parameters = FillParameters(AvailableParameters(), value);
				SetupParameters();
			}
    	}

		/// <summary>
        /// Sets up the parameters to the rule, that is the individual pairs
        /// of rule type and rule value that make up the composite rule
		/// </summary>
		protected internal abstract void SetupParameters();

    	/// <summary>
        /// Returns the list of available parameter names for the rule.
        /// This method must be implemented by creating a list of the names
        /// of each type of rule available for the class, such as "min" and
        /// "max" for integers.
    	/// </summary>
    	/// <returns>A list of the parameters that this rule uses</returns>
		protected internal abstract List<string> AvailableParameters();

        /// <summary>
        /// Returns the list of available parameter names for the rule
        /// </summary>
        /// <returns>Returns a string</returns>
		protected internal virtual string AvailableParametersString()
		{
			string list = "";
			string delimiter = "";
			foreach (string availableParameter in AvailableParameters())
			{
				list += delimiter + "'" + availableParameter + "'";
				delimiter = ", ";
			}
			list = "{" + list + "}";
			return list;
		}

        /// <summary>
        /// Returns the rule name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the error message for if the rule fails.
        /// </summary>
        public string Message { get; set; }

        private static Dictionary<string, object> FillParameters(List<string> availableParams, Dictionary<string, object> currentCollection)
		{
			Dictionary<string, object> parameters = new Dictionary<string, object>();
			if (currentCollection == null)
			{
				currentCollection = new Dictionary<string, object>();
			}
			if (availableParams == null) return new Dictionary<string, object>();
			foreach (string availableParam in availableParams)
			{
				if (currentCollection.ContainsKey(availableParam))
				{
					parameters.Add(availableParam, currentCollection[availableParam]);
				} else
				{
					parameters.Add(availableParam, null);
				}
			}
			return parameters;
		}
        /// <summary>
        /// Returns the base error message that can be used by sub classes of PropRuleBase.
        /// </summary>
        /// <param name="propValue">The value that has cuased the broken rule.</param>
        /// <param name="displayName">The display name of the property that the business rule is broken for.</param>
        /// <returns></returns>
        protected string GetBaseErrorMessage(object propValue, string displayName)
        {
            string errorMessage = String.Format("'{0}' for property '{1}' is not valid for the rule '{2}'. ",
                                                propValue, displayName, Name);
            return errorMessage;
        }
    }
}