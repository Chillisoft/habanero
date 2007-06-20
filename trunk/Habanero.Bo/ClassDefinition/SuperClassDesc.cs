namespace Habanero.Bo.ClassDefinition
{
	/// <summary>
	/// Manages a super-class in the case where inheritance is being used.
	/// </summary>
	/// TODO ERIC - what is desc? description?  maybe this could be renamed
	public class SuperClassDesc
	{
		private ORMapping _orMapping;
		private ClassDef _superClassDef;
		private string _className;
		private string _assemblyName;

		#region Constructors

		/// <summary>
		/// Constructor to create a new super-class
		/// </summary>
		/// <param name="superClassDef">The class definition</param>
		/// <param name="mapping">The type of OR-Mapping to use. See
		/// the ORMapping enumeration for more detail.</param>
		public SuperClassDesc(ClassDef superClassDef, ORMapping mapping)
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
		public SuperClassDesc(string assemblyName, string className, ORMapping mapping)
		{
			_orMapping = mapping;
			_superClassDef = null;
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
					_superClassDef = null;
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
					_superClassDef = null;
				_className = value;
			}
		}

		/// <summary>
		/// Returns the class definition for this super-class
		/// </summary>
		public ClassDef SuperClassDef
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
				if (_superClassDef == null && _assemblyName != null && _className != null)
				{
					//TODO error: What happens if the ClassDef does not exist?
					_superClassDef = ClassDef.GetClassDefCol[_assemblyName, _className];
				}
				return _superClassDef;
			}
			set
			{
				//TODO error: What happens if it is null?
				_superClassDef = value;
				if (_superClassDef != null)
				{
					_assemblyName = _superClassDef.AssemblyName;
					_className = _superClassDef.ClassNameFull;
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