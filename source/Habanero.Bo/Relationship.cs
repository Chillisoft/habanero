//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.BO
{
	internal interface IRelationshipForLoading: IRelationship
	{
		void Initialise();
	}
	internal static class RelationshipUtils
	{
		/// <summary>
		/// Creates a <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> with boType as its type parameter, using the Activator.
		/// </summary>
		/// <param name="boType">The type parameter to be used</param>
		/// <param name="relationship">The relationship that this <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> is the collection for</param>
		/// <returns>The instantiated <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/></returns>
		public static IBusinessObjectCollection CreateRelatedBusinessObjectCollection(Type boType, IMultipleRelationship relationship)
		{
			IBusinessObjectCollection collection = CreateNewRelatedBusinessObjectCollection(boType, relationship);
			SetupCriteriaForRelationship(relationship, collection);
			return collection;
		}

		/// <summary>
		/// Creates a <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> with boType as its type parameter, using the Activator.
		/// </summary>
		/// <param name="boClassName"></param>
		/// <param name="relationship">The relationship that this <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> is the collection for</param>
		/// <param name="boAssemblyName"></param>
		/// <returns>The instantiated <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/></returns>
		public static IBusinessObjectCollection CreateRelatedBusinessObjectCollection(string boAssemblyName, string boClassName, IMultipleRelationship relationship)
		{
			var collection = CreateNewRelatedBusinessObjectCollection(boAssemblyName, boClassName, relationship);
			SetupCriteriaForRelationship(relationship, collection);
			return collection;
		}

		/// <summary>
		/// Creates a <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> with boType as its type parameter, using the Activator.
		/// </summary>
		/// <param name="boClassName"></param>
		/// <param name="relationship">The relationship that this <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> is the collection for</param>
		/// <param name="boAssemblyName"></param>
		/// <returns>The instantiated <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/></returns>
		private static IBusinessObjectCollection CreateNewRelatedBusinessObjectCollection(string boAssemblyName, string boClassName, IRelationship relationship)
		{
			Type classType = null;
			TypeLoader.LoadClassType(ref classType, boAssemblyName, boClassName,
									 "related object", "relationship definition");
			return CreateNewRelatedBusinessObjectCollection(classType, relationship);
		}
		/// <summary>
		/// Creates a <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> with boType as its type parameter, using the Activator.
		/// </summary>
		/// <param name="boType">The type parameter to be used</param>
		/// <param name="relationship">The relationship that this <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> is the collection for</param>
		/// <returns>The instantiated <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/></returns>
		private static IBusinessObjectCollection CreateNewRelatedBusinessObjectCollection(Type boType, IRelationship relationship)
		{
//            Utilities.CheckTypeCanBeCreated(boType);
			Type relatedCollectionType = typeof(RelatedBusinessObjectCollection<>);
			relatedCollectionType = relatedCollectionType.MakeGenericType(boType);
			IBusinessObjectCollection collection = (IBusinessObjectCollection)Activator.CreateInstance(relatedCollectionType, relationship);
			return collection;
		}

		internal static void SetupCriteriaForRelationship(IMultipleRelationship relationship, IBusinessObjectCollection collection)
		{
			Criteria relationshipCriteria = Criteria.FromRelationship(relationship);
			IOrderCriteria preparedOrderCriteria;
			var orderCriteriaString = relationship.OrderCriteria.ToString();
			try
			{
				preparedOrderCriteria =
					QueryBuilder.CreateOrderCriteria(relationship.RelatedObjectClassDef, orderCriteriaString);
			}
			catch (InvalidOrderCriteriaException)
			{
				throw new InvalidOrderCriteriaException("The Relationship '" + relationship.RelationshipName 
						+ "' on the ClassDef '" + relationship.OwningBO.ClassDef.ClassNameFull 
						+ "' has an Invalid OrderCriteria '" + orderCriteriaString);
			}

			//QueryBuilder.PrepareCriteria(relationship.RelatedObjectClassDef, relationshipCriteria);
			collection.SelectQuery.Criteria = relationshipCriteria;
			collection.SelectQuery.OrderCriteria = preparedOrderCriteria;
		}

		internal static void CheckCorrespondingSingleRelationshipsAreValid(SingleRelationshipBase singleRelationship, SingleRelationshipBase singleReverseRelationship)
		{
			if (singleRelationship.OwningBOHasForeignKey && singleReverseRelationship.OwningBOHasForeignKey)
			{
				string message = String.Format("The corresponding single (one to one) relationships " +
											   "{0} (on {1}) and {2} (on {3}) cannot both be configured as having the foreign key " 
											   + "(OwningBOHasForeignKey). Please set this property to false on one of these relationships " 
											   + "(In the ClassDefs.XML or in Firestarter.",
											   singleRelationship.RelationshipName, singleRelationship.OwningBO.GetType().Name,
											   singleReverseRelationship.RelationshipName,
											   singleRelationship.RelatedObjectClassDef.ClassName);
				throw new HabaneroDeveloperException(message, "");
			}
		}
	}

	///<summary>
	/// This provides a base class from which all relationship classes inherit.
	///</summary>
	public abstract class RelationshipBase : IRelationship
	{

		///<summary>
		/// The key that identifies this relationship i.e. the properties in the 
		/// source object and how they are related to properties in the related object.
		///</summary>
		public abstract IRelKey RelKey { get; }

		/// <summary>
		/// The class Definition for the related object.
		/// </summary>
		public abstract IClassDef RelatedObjectClassDef { get; }

		///<summary>
		/// Returns whether the relationship is dirty or not.
		/// A relationship is always dirty if it has Added, created, removed or deleted Related business objects.
		/// If the relationship is of type composition or aggregation then it is dirty if it has any 
		///  related (children) business objects that are dirty.
		///</summary>
		public abstract bool IsDirty { get; }

		/// <summary>
		/// Returns the relationship definition
		/// </summary>
		public abstract IRelationshipDef RelationshipDef { get; }
		/// <summary>
		/// Returns the relationship name
		/// </summary>
		public abstract string RelationshipName { get; }
		///<summary>
		/// Is the relationship initialised.
		///</summary>
		public abstract bool Initialised { get; }
		///<summary>
		/// Returns the appropriate delete action when the parent is deleted.
		/// i.e. delete related objects, dereference related objects, prevent deletion.
		///</summary>
		public abstract DeleteParentAction DeleteParentAction { get; }

		///<summary>
		/// Returns the business object that owns this relationship e.g. Invoice has many lines
		/// the owning BO would be invoice.
		///</summary>
		public abstract IBusinessObject OwningBO { get; }

		/// <summary>
		/// Returns the <see cref="IRelationship.RelationshipType"/> of this relationship. 
		/// This comes from the <see cref="IRelationship.RelationshipDef"/>
		/// </summary>
		public RelationshipType RelationshipType
		{
			get { return this.RelationshipDef.RelationshipType; }
		}

		/// <summary>
		/// Is there anything in this relationship to prevent the business object from being deleted.
		/// e.g. if there are related business objects that are not marked as mark for delete.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public abstract bool IsDeletable(out string message);

		/// <summary>
		/// If the relationship is <see cref="IRelationship.DeleteParentAction"/>.DeleteRelated then
		/// all the related objects and their relevant children will be marked for Delete.
		/// See <see cref="IBusinessObject.MarkForDelete"/>
		/// </summary>
		public abstract void MarkForDelete();

		/// <summary>
		/// Cancels Edits on this relationship and all its children.
		/// </summary>
		internal abstract void CancelEdits();
		/// <summary>
		/// Adds all dirty children to the <see cref="TransactionCommitter"/>
		/// </summary>
		/// <param name="committer"></param>
		internal abstract void AddDirtyChildrenToTransactionCommitter(TransactionCommitter committer);

		/// <summary>
		/// Dereferences the Children if required and adds the relevant Deref Block to the transaction committer.
		/// </summary>
		/// <param name="committer"></param>
		internal abstract void DereferenceChildren(TransactionCommitter committer);

		/// <summary>
		/// Deletes the Children if required and adds the relevant children to the transaction committer.
		/// </summary>
		/// <param name="committer"></param>
		internal abstract void DeleteChildren(TransactionCommitter committer);

		/// <summary>
		/// Dereferences the Removed Children and adds the dereference transactionItem to the transaction committer
		/// </summary>
		/// <param name="committer"></param>
		internal abstract void DereferenceRemovedChildren(TransactionCommitter committer);

		/// <summary>
		/// Adds all the marked for deleted children to the transaction committer and commits it.
		/// </summary>
		/// <param name="committer"></param>
		internal abstract void DeleteMarkedForDeleteChildren(TransactionCommitter committer);

		/// <summary>
		/// Returns the reverse relationship for this relationship i.e. If invoice has invoice lines and you 
		/// can navigate from invoice lines to invoices then the invoicelines to invoice relationship is the
		/// reverse relationship of the invoice to invoicelines relationship and vica versa.
		/// </summary>
		/// <param name="bo">The related Business object (in the example the invoice lines)</param>
		/// <returns>The reverse relationship or null if no reverse relationship is set up.</returns>
		public IRelationship GetReverseRelationship(IBusinessObject bo)
		{
			if (bo == null || bo.Relationships == null) return null;
			if (HasReverseRelationshipDefined(this))
			{
			    var relationships = bo.Relationships.Where(relationship => relationship.RelationshipName == this.RelationshipDef.ReverseRelationshipName);
			    foreach (var relationship in relationships)
			    {
			        return relationship;
			    }
			    throw new HabaneroDeveloperException(
					String.Format(
						"The relationship '{0}' on class '{1}' has a reverse relationship defined ('{2}'), but this " +
						"relationship was not found on the related object. " +
						"Please check that the reverse relationship is defined correctly " +
						"and that it exists on the related class '{3}'",
						this.RelationshipName, this.OwningBO.ClassDef.ClassName,
						this.RelationshipDef.ReverseRelationshipName,
						this.RelatedObjectClassDef.ClassName), "");
			}
		    return null;
		}
		/// <summary>
		/// Returns true if htis relationship has a reverse relationship defined. False otherwise.
		/// </summary>
		/// <param name="relationship"></param>
		/// <returns></returns>
		protected static bool HasReverseRelationshipDefined(IRelationship relationship)
		{
			return !String.IsNullOrEmpty(relationship.RelationshipDef.ReverseRelationshipName);
		}
	}
	
	/// <summary>
	/// Provides a super-class for relationships between business objects
	/// </summary>
	public abstract class Relationship : RelationshipBase, IRelationshipForLoading
	{
		/// <summary> The Definition that defines this relationship. </summary>
		protected IRelationshipDef _relDef;
		/// <summary> The Busienss Object that owns this relationship </summary>
		protected readonly IBusinessObject _owningBo;
		/// <summary>
		/// The Key that defines this relationship I.e. OwningProps and their related props.
		/// </summary>
		private readonly Lazy<IRelKey> _relKey;

		private bool _initialised;

		/// <summary>
		/// Constructor to initialise a new relationship
		/// </summary>
		/// <param name="owningBo">The business object from where the 
		/// relationship originates</param>
		/// <param name="lRelDef">The relationship definition</param>
		/// <param name="lBOPropCol">The set of properties used to
		/// initialise the RelKey object</param>
		protected Relationship(IBusinessObject owningBo, IRelationshipDef lRelDef, IBOPropCol lBOPropCol)
		{
			if (owningBo == null) throw new ArgumentNullException("owningBo");
			if (lRelDef == null) throw new ArgumentNullException("lRelDef");
			if (lBOPropCol == null) throw new ArgumentNullException("lBOPropCol");
			_relDef = lRelDef;
			_owningBo = owningBo;
			_relKey = new Lazy<IRelKey>(() => _relDef.RelKeyDef.CreateRelKey(lBOPropCol));
		}

		/// <summary>
		/// Returns the relationship name
		/// </summary>
		public override string RelationshipName
		{
			get { return _relDef.RelationshipName; }
		}

		/// <summary>
		/// Returns the relationship definition
		/// </summary>
		public override IRelationshipDef RelationshipDef
		{
			get { return _relDef; }
		}

		///<summary>
		/// Returns the appropriate delete action when the parent is deleted.
		/// i.e. delete related objects, dereference related objects, prevent deletion.
		///</summary>
		public override DeleteParentAction DeleteParentAction
		{
			get { return _relDef.DeleteParentAction; }
		}

		///<summary>
		/// Returns the business object that owns this relationship e.g. Invoice has many lines
		/// the owning BO would be invoice.
		///</summary>
		public override IBusinessObject OwningBO
		{
			get { return _owningBo; }
		}

		///<summary>
		/// The key that identifies this relationship i.e. the properties in the 
		/// source object and how they are related to properties in the related object.
		///</summary>
		public override IRelKey RelKey
		{
			get { return _relKey.Value; }
		}

		/// <summary>
		/// The class Definition for the related object.
		/// </summary>
		public override IClassDef RelatedObjectClassDef
		{
			get { return _relDef.RelatedObjectClassDef; }
		}

		void IRelationshipForLoading.Initialise()
		{
			if (_initialised) return;
			DoInitialisation();
			_initialised = true;
		}
		/// <summary>
		/// Do the initialisation of this relationship.
		/// </summary>
		protected abstract void DoInitialisation();

		///<summary>
		/// Is the relationship initialised or not.
		///</summary>
		public override bool Initialised { get { return _initialised; } }

		/// <summary>
		/// DereferenceThe Child <see cref="IBusinessObject"/> identified by <paramref name="bo"/>
		/// </summary>
		/// <param name="committer">The transaction commtter responsible for persisting this dereference.</param>
		/// <param name="bo">The Business Object being dereferenced.</param>
		protected void DereferenceChild(TransactionCommitter committer, IBusinessObject bo)
		{
			foreach (RelPropDef relPropDef in RelationshipDef.RelKeyDef)
			{
				bo.SetPropertyValue(relPropDef.RelatedClassPropName, null);
			}
			if (bo.Status.IsNew) return;
			committer.ExecuteTransactionToDataSource(committer.CreateTransactionalBusinessObject(bo));
		}
		/// <summary>
		/// Deletes the Child Bo identified by <paramref name="bo"/>
		/// </summary>
		/// <param name="committer"></param>
		/// <param name="bo"></param>
		protected virtual void DeleteChild(TransactionCommitter committer, IBusinessObject bo)
		{
			if (bo == null) return;
			if (!bo.Status.IsDeleted)
			{
				bo.MarkForDelete();
			}
//            if (bo.Status.IsNew) return;
			committer.ExecuteTransactionToDataSource(committer.CreateTransactionalBusinessObject(bo));
		}

		internal abstract void UpdateRelationshipAsPersisted();
	}

	
}