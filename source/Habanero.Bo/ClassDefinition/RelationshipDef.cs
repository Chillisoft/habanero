//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.Util;
using Habanero.Util.File;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Describes the relationship between an object and some other object(s)
    /// </summary>
    public abstract class RelationshipDef
    {
		private Type _relatedObjectClassType;
		private string _relatedObjectAssemblyName;
		private string _relatedObjectClassName;
		private RelKeyDef _relKeyDef;
		private string _relationshipName;
		private bool _keepReferenceToRelatedObject;

		#region Constructors

        /// <summary>
        /// Constructor to create a new relationship definition
        /// </summary>
        /// <param name="relationshipName">A name for the relationship</param>
        /// <param name="relatedObjectClassType">The class type of the related object</param>
        /// <param name="relKeyDef">The related key definition</param>
        /// <param name="keepReferenceToRelatedObject">Whether to keep a
        /// reference to the related object.  Could be false for memory-
        /// intensive applications.</param>
		public RelationshipDef(string relationshipName,
							   Type relatedObjectClassType,
							   RelKeyDef relKeyDef,
							   bool keepReferenceToRelatedObject)
			:this(relationshipName, relatedObjectClassType, null,null,relKeyDef, keepReferenceToRelatedObject)
		{}

		public RelationshipDef(string relationshipName,
								string relatedObjectAssemblyName,
								string relatedObjectClassName,
								RelKeyDef relKeyDef,
								bool keepReferenceToRelatedObject)
			:this(relationshipName, null, relatedObjectAssemblyName,relatedObjectClassName, relKeyDef, keepReferenceToRelatedObject)
		{}

    	private RelationshipDef(string relationshipName,
								Type relatedObjectClassType,
								string relatedObjectAssemblyName,
								string relatedObjectClassName,
								RelKeyDef relKeyDef,
								bool keepReferenceToRelatedObject)
		{
            //ArgumentValidationHelper.CheckArgumentNotNull(relatedObjectClassType, "relatedObjectClassType");
            ArgumentValidationHelper.CheckArgumentNotNull(relKeyDef, "relKeyDef");
            ArgumentValidationHelper.CheckStringArgumentNotEmpty(relationshipName, "relationshipName");
			//ArgumentValidationHelper.CheckArgumentIsSubType(relatedObjectClassType, "relatedObjectClassType",
			//                                                typeof (BusinessObject));

			if (relatedObjectClassType != null) 
				MyRelatedObjectClassType = relatedObjectClassType;
			else
			{
				_relatedObjectAssemblyName = relatedObjectAssemblyName;
				_relatedObjectClassName = relatedObjectClassName;
				_relatedObjectClassType = null;
			}
			_relKeyDef = relKeyDef;
            _relationshipName = relationshipName;
            _keepReferenceToRelatedObject = keepReferenceToRelatedObject;
        }

		#endregion Constructors

		#region Properties

		/// <summary>
		/// A name for the relationship
		/// </summary>
		public string RelationshipName
		{
			get { return _relationshipName; }
			protected set { _relationshipName = value; }
		}

		/// <summary>
		/// The assembly name of the related object type
		/// </summary>
		public string RelatedObjectAssemblyName
		{
			get { return _relatedObjectAssemblyName; }
			protected set { _relatedObjectAssemblyName = value; }
		}

		/// <summary>
		/// The class name of the related object type
		/// </summary>
		public string RelatedObjectClassName
		{
			get { return _relatedObjectClassName; }
			protected set { _relatedObjectClassName = value; }
		}

        /// <summary>
        /// The class type of the related object
        /// </summary>
        public Type RelatedObjectClassType
        {
            get { return MyRelatedObjectClassType; }
			protected set { MyRelatedObjectClassType = value;}
        }

        /// <summary>
        /// The related key definition
        /// </summary>
        public RelKeyDef RelKeyDef
        {
			get { return _relKeyDef; }
			protected set { _relKeyDef = value; }
        }

        /// <summary>
        /// Whether to keep a reference to the related object.  Could be false 
        /// for memory-intensive applications.
        /// </summary>
        public bool KeepReferenceToRelatedObject
        {
			get { return _keepReferenceToRelatedObject; }
			protected set { _keepReferenceToRelatedObject = value; }
        }

    	internal ClassDef RelatedObjectClassDef
    	{
    		get
    		{
				ClassDef classDef = null;
				if (ClassDef.ClassDefs.Contains(RelatedObjectAssemblyName, RelatedObjectClassName))
				{
					classDef = ClassDef.ClassDefs[RelatedObjectAssemblyName, RelatedObjectClassName];
				}
    			return classDef;
    		}
    	}

		#endregion Properties

		#region Type Initialisation

		private Type MyRelatedObjectClassType
    	{
			get
			{
				TypeLoader.LoadClassType(ref _relatedObjectClassType, _relatedObjectAssemblyName, _relatedObjectClassName,
					"related object", "relationship definition");
				return _relatedObjectClassType;
			}
			set
			{
				_relatedObjectClassType = value;
				if (_relatedObjectClassType != null)
					ArgumentValidationHelper.CheckArgumentIsSubType(_relatedObjectClassType, "relatedObjectClassType", typeof(BusinessObject));
				TypeLoader.ClassTypeInfo(_relatedObjectClassType, out _relatedObjectAssemblyName, out _relatedObjectClassName);
			}
    	}

		#endregion Type Initialisation

        /// <summary>
        /// Create and return a new Relationship
        /// </summary>
        /// <param name="owningBo">The business object that will
        /// manage this relationship</param>
        /// <param name="lBOPropCol">The collection of properties</param>
        /// <returns>The new relationship object created</returns>
        public abstract Relationship CreateRelationship(BusinessObject owningBo, BOPropCol lBOPropCol);
    }
}