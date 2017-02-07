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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
	/// <summary>
	/// A helper class that encapsulates the deleting checking for a specified business object.
	/// This class finds all the relationships that are marked as prevent delete for this business object
	/// and any of its children business object. It then checks if there are any children that are marked as 
	/// prevent delete. If there are any business objects in these relationships that are not marked as deleted
	/// then the <see cref="CheckCanDelete"/> method will return false.
	/// </summary>
	internal class DeleteHelper
	{
        /// <summary>
        /// Checks if the business object <see cref="IBusinessObject"/> can be deleted.
        /// If the object can be deleted then returns true else returns false with 
        /// a list of reasons that the object cannot be deleted.
        /// </summary>
        /// <param name="bo">The Business object to check for deletion</param>
        /// <param name="reason">The reasons that the object cannot be deleted</param>
        /// <returns></returns>
		public static bool CheckCanDelete(IBusinessObject bo, out string reason)
		{
		    if (bo == null) throw new ArgumentNullException("bo");
		    reason = "";

		    var classDef = (ClassDef) bo.ClassDef;
		    var relationshipDefCol = classDef.RelationshipDefCol;
			var listOfPaths = FindPreventDeleteRelationships(relationshipDefCol);
			var results = new Dictionary<string, int>();
			CheckCanDeleteSafe(bo, new List<IBusinessObject>(), listOfPaths, "", ref results);
			foreach (KeyValuePair<string, int> pair in results)
			{
				reason += Environment.NewLine + String.Format(
					"There are {0} objects related through the '{1}' relationship that need to be deleted first.",
					pair.Value, pair.Key);
			}
			if (reason.Length > 0)
			{
				reason = String.Format("Cannot delete this '{0}' identified as '{1}' for the following reasons:",
                    classDef.ClassName, bo) + reason;
			}
			/*
			 * Message format:-
			 * Cannot delete this '<ClassName>' for the following reasons:
			 * There are <NUM> '<RelatedClassName>' objects related through the '<RelationshipPath>' relationship that need to be deleted first.
			 */

			return String.IsNullOrEmpty(reason);
		}

		private static void CheckCanDeleteSafe(IBusinessObject bo, List<IBusinessObject> alreadyChecked, 
			MatchList matchList, string currentRelationshipPath, ref Dictionary<string, int> results)
		{
			if (alreadyChecked.Contains(bo)) return;
			alreadyChecked.Add(bo);
			if (currentRelationshipPath.Length >0)
			{
				currentRelationshipPath += ".";
			}
			var listOfPaths = matchList;
			foreach (var pair in listOfPaths)
			{
				var relationshipName = pair.Key;
				if (!bo.Relationships.Contains(relationshipName)) return;
				var thisRelationshipPath = currentRelationshipPath + relationshipName;
				var relationship = bo.Relationships[relationshipName] as IMultipleRelationship;
				if (relationship == null) continue;
				if (pair.Value == null)
				{
				    var businessObjects = relationship.BusinessObjectCollection;
				    if (businessObjects.Count > 0)
					{
						if (!results.ContainsKey(thisRelationshipPath))
						{
							results.Add(thisRelationshipPath, businessObjects.Count);
						} else
						{
							results[thisRelationshipPath] += businessObjects.Count;
						}
					}
				}
				else if (pair.Value.Count > 0)
				{
				    var boCol = relationship.BusinessObjectCollection;
				    foreach (BusinessObject businessObject in boCol)
					{
						CheckCanDeleteSafe(businessObject, alreadyChecked, pair.Value, thisRelationshipPath, ref results);
					}
				}
			}
		}

		/// <summary>
		/// Returns a list of all relationships that are marked as prevent deletion.
		/// </summary>
		/// <param name="relationshipDefCol"></param>
		/// <returns></returns>
		public static MatchList FindPreventDeleteRelationships(IRelationshipDefCol relationshipDefCol)
		{
			return FindRelationships<MultipleRelationshipDef>(relationshipDefCol, PreventDeleteRelationshipCondition);
		}

		public delegate bool MatchesConditionDelegate<in TRelationshipDef>(TRelationshipDef relationshipDef)
			where TRelationshipDef : RelationshipDef;

        /// <summary>
        /// Returns a list of all relationships that match the Delegate relationship.
        /// </summary>
        /// <typeparam name="TRelationshipDef"></typeparam>
        /// <param name="relationshipDefCol"></param>
        /// <param name="matchesConditionDelegate"></param>
        /// <returns></returns>
		public static MatchList FindRelationships<TRelationshipDef>(IRelationshipDefCol relationshipDefCol,
			MatchesConditionDelegate<TRelationshipDef> matchesConditionDelegate)
			where TRelationshipDef : RelationshipDef
		{
			return FindRelationshipsSafe(relationshipDefCol, matchesConditionDelegate, new List<IRelationshipDefCol>());
		}


		private static MatchList FindRelationshipsSafe<TRelationshipDef>(IRelationshipDefCol relationshipDefCol,
			MatchesConditionDelegate<TRelationshipDef> matchesConditionDelegate, List<IRelationshipDefCol> alreadyChecked)
			where TRelationshipDef : RelationshipDef
		{
			var listOfPaths = new MatchList();
			if (matchesConditionDelegate == null) return listOfPaths;
            if (relationshipDefCol == null) return listOfPaths;
			if (alreadyChecked.Contains(relationshipDefCol)) return listOfPaths;
			alreadyChecked.Add(relationshipDefCol);
			foreach (var relationshipDef in relationshipDefCol)
			{
				var relationshipName = relationshipDef.RelationshipName;
				var castedRelationshipDef = relationshipDef as TRelationshipDef;
			    if (castedRelationshipDef == null) continue;
			    var matchesCondition = matchesConditionDelegate(castedRelationshipDef);
			    if (matchesCondition)
			    {
			        listOfPaths.Add(relationshipName, null);
			    } else
			    {
			        var classDef = (ClassDef) relationshipDef.RelatedObjectClassDef;
			        if (classDef != null)
			        {
			            var results = FindRelationshipsSafe(classDef.RelationshipDefCol, matchesConditionDelegate, alreadyChecked);
			            if (results.Count > 0)
			            {
			                listOfPaths.Add(relationshipName, results);
			            }
			        }
			    }
			}
			return listOfPaths;
		}

		private static bool PreventDeleteRelationshipCondition(MultipleRelationshipDef multipleRelationshipDef)
		{
			return multipleRelationshipDef.DeleteParentAction == DeleteParentAction.Prevent;
		}
	}

    /// <summary>
    /// A Dictionary of all the items that match the list
    /// </summary>
	internal class MatchList : Dictionary<string, MatchList>
	{
		///<summary>
		///Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		///</summary>
		///<returns>
		///A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		///</returns>
		///<filterpriority>2</filterpriority>
		public override string ToString()
		{
			return ToString(".");
		}

		public string ToString(string delimiter)
		{
			var keysString = "";
			foreach (var pair in this)
			{
				if (keysString.Length > 0)
				{
					keysString += ",";
				}
				keysString += pair.Key;
				if (pair.Value != null)
				{
					keysString += delimiter + pair.Value.ToString(delimiter);
				}
			}
			if (Count > 1)
			{
				keysString = "{" + keysString + "}";
			}
			return keysString;
			//i.e.  MyBO.{MyBO2.{MyBO21,MyBO22},MyBO3}
		}

	}
}
