using System;
using System.Collections.Generic;

namespace Habanero.Bo
{
    /// <summary>
    /// Provides a super-class for property rules that test the validity of
    /// a property value.  If you would like to implement your own property
    /// rule checker, inherit from this class, override the
    /// isPropValueValid method and add a constructor with the same arguments
    /// as this one and pass back these arguments to base().
    /// In the class definitions, in the 'rule'
    /// element under the relevant 'property', specify the class and assembly
    /// of your newly implemented class.
    /// </summary>
    public abstract class PropRuleBase
    {
		private string _name;
        private string _message;
    	protected Dictionary<string, object> _parameters;

		/// <summary>
		/// Constructor to initialise a new property rule
		/// </summary>
		/// <param name="name">The name of the rule</param>
		/// <param name="message">This rule's failure message</param>
		///// <param name="parameters">The parameters for this rule.</param>
		public PropRuleBase(string name, string message)
		{
			_name = name;
			_message = message;
			_parameters = FillParameters(AvailableParameters(), _parameters);
		}

        /// <summary>
        /// Indicates whether the property value is valid against the rules
        /// </summary>
        /// <param name="propName">The property name being checked</param>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        protected internal virtual bool isPropValueValid(string propName, Object propValue,
                                                         ref string errorMessage)
        {
            errorMessage = "";
            return true;
        }

    	protected internal virtual Dictionary<string, object> Parameters
    	{
			get { return _parameters; }
			set
			{
				_parameters = FillParameters(AvailableParameters(), value);
				SetupParameters();
			}
    	}

		/// <summary>
		/// Set up the rule variables from the current parameters collection.
		/// </summary>
		protected internal abstract void SetupParameters();

    	/// <summary>
    	/// This method must be implemented to return a list of the 
    	/// parameters that are available to be set for this rule. 
    	/// This is used for validation by the loader.
    	/// </summary>
    	/// <returns>A list of the parameters that this rule uses</returns>
		protected internal abstract List<string> AvailableParameters();

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
        public string Name
        {
            get { return _name; }
			protected set { _name = value; }
        }

        /// <summary>
        /// Returns the error message for if the rule fails.
        /// </summary>
        public string  Message {
			get { return _message; }
			protected set { _message = value; }
        }

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
    }
}