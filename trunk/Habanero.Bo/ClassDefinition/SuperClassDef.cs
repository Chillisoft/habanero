using System;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.Bo.ClassDefinition
{
	/// <summary>
	/// Manages a super-class in the case where inheritance is being used.
	/// </summary>
	public class SuperClassDef
	{
		private ORMapping _orMapping;
		private ClassDef _superClassClassDef;
		private string _className;
		private string _assemblyName;

		#region Constructors

		/// <summary>
		/// Constructor to create a new super-class
		/// </summary>
		/// <param name="superClassDef">The class definition</param>
		/// <param name="mapping">The type of OR-Mapping to use. See
		/// the ORMapping enumeration for more detail.</param>
		public SuperClassDef(ClassDef superClassDef, ORMapping mapping)
		{
			_orMapping = mapping;
			MySuperClassDef = superClassDef;
		}

		/// <summary>
		/// Constructor to create a new super-class
		/// </summary>
		/// <param name="assemblyName">The assembly name of the superClass</param>
		/// <param name="className">The class name of the superClass</param>
		/// <param name="mapping">The type of OR-Mapping to use. See
		/// the ORMapping enumeration for more detail.</param>
		public SuperClassDef(string assemblyName, string className, ORMapping mapping)
		{
			_orMapping = mapping;
			_superClassClassDef = null;
			_assemblyName = assemblyName;
			_className = className;
		}

		#endregion Constructors

		#region properties

		/// <summary>
		/// Returns the type of ORMapping used.  See the ORMapping
		/// enumeration for more detail.
		/// </summary>
		public ORMapping ORMapping
		{
			get { return _orMapping; }
			protected set { _orMapping = value; }
		}


		///<summary>
		/// The assembly name of the SuperClass
		///</summary>
		public string AssemblyName
		{
			get { return _assemblyName; }
			protected set
			{
				if (_assemblyName != value)
				{
					_superClassClassDef = null;
					_className = null;
				}
				_assemblyName = value;
			}
		}

		///<summary>
		/// The class name of the SuperClass
		///</summary>
		public string ClassName
		{
			get { return _className; }
			protected set
			{
				if (_className != value)
					_superClassClassDef = null;
				_className = value;
			}
		}

		/// <summary>
		/// Returns the class definition for this super-class
		/// </summary>
		public ClassDef SuperClassClassDef
		{
			get { return MySuperClassDef; }
			protected set { MySuperClassDef = value; }
		}
		
		#endregion properties

		#region SuperClassDef Methods

		private ClassDef MySuperClassDef
		{
			get
			{
				if (_superClassClassDef == null && _assemblyName != null && _className != null)
				{
					_superClassClassDef = ClassDef.ClassDefs[_assemblyName, _className];
                    if (_superClassClassDef == null)
                    {
                        throw new InvalidXmlDefinitionException(String.Format(
                            "The class definition for the super class with the type " +
                            "'{0}' was not found.  Check that the class definition " +
                            "exists or that spelling and capitalisation are correct."
                            ,_assemblyName + "." + _className));
                    }
				}
				return _superClassClassDef;
			}
			set
			{
				//TODO error: What happens if it is null?
				_superClassClassDef = value;
				if (_superClassClassDef != null)
				{
					_assemblyName = _superClassClassDef.AssemblyName;
					_className = _superClassClassDef.ClassNameFull;
				}
				else
				{
					_assemblyName = null;
					_className = null;
				}
			}
		}

		#endregion SuperClassDef Methods

	}
}