using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.BO.Comparer
{
	/// <summary>
    /// Compares two business objects on the Double property specified 
    /// in the constructor
    /// </summary>
	public class DoubleComparer<T> : PropertyComparer<T, Double>
		where T : BusinessObject
	{

		/// <summary>
		/// Constructor to initialise a comparer, specifying the property
		/// on which two business objects will be compared using the Compare()
		/// method for the specified type
		/// </summary>
		/// <param name="propName">The property name on which two
		/// business objects will be compared</param>
		public DoubleComparer(string propName)
			: base(propName)
		{
		}
	}
}
