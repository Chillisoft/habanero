using System.Collections;
using Habanero.Bo.ClassDefinition;

namespace Habanero.Bo.Loaders
{
	/// <summary>
	/// An interface to model classes that load class definitions from
	/// xml data
	/// </summary>
	public interface IClassDefsLoader
	{
		/// <summary>
		/// Loads class definitions from loader source data
		/// </summary>
		/// <returns>Returns an ClassDefCol containing the definitions</returns>
		ClassDefCol LoadClassDefs();

		///// <summary>
		///// Loads class definitions from pre-specified xml data
		///// </summary>
		///// <returns>Returns an IList object containing the definitions</returns>
		//IList LoadClassDefs();	
	}
}