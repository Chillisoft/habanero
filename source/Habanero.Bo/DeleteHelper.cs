using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
	
	internal class DeleteHelper
	{

		public static bool CheckCanDelete(BusinessObject bo, out string reason)
		{
			ClassDef classDef = bo.ClassDef;
			RelationshipDefCol relationshipDefCol;
			relationshipDefCol = classDef.RelationshipDefCol;
			MatchList listOfPaths = FindPreventDeleteRelationships(relationshipDefCol);
			Dictionary<string, int> results = new Dictionary<string, int>();
			CheckCanDeleteSafe(bo, new List<BusinessObject>(), listOfPaths, "", ref results);
			reason = "";
			foreach (KeyValuePair<string, int> pair in results)
			{
				reason += Environment.NewLine + String.Format(
					"There are {0} objects related through the '{1}' relationship that need to be deleted first.",
					pair.Value, pair.Key);
			}
			if (reason.Length > 0)
			{
				reason = String.Format("Cannot delete this '{0}' for the following reasons:",
					classDef.ClassName) + reason;
			}
			/*
			 * Message format:-
			 * Cannot delete this '<ClassName>' for the following reasons:
			 * There are <NUM> '<RelatedClassName>' objects related through the '<RelationshipPath>' relationship that need to be deleted first.
			 */

			return String.IsNullOrEmpty(reason);
		}

		private static void CheckCanDeleteSafe(BusinessObject bo, List<BusinessObject> alreadyChecked, 
			MatchList matchList, string currentRelationshipPath, ref Dictionary<string, int> results)
		{
			if (alreadyChecked.Contains(bo)) return;
			alreadyChecked.Add(bo);
			if (currentRelationshipPath.Length >0)
			{
				currentRelationshipPath += ".";
			}
			MatchList listOfPaths = matchList;
			foreach (KeyValuePair<string, MatchList> pair in listOfPaths)
			{
				string relationshipName = pair.Key;
				if (!bo.Relationships.Contains(relationshipName)) return;
				string thisRelationshipPath = currentRelationshipPath + relationshipName;
				MultipleRelationship relationship = bo.Relationships[relationshipName] as MultipleRelationship;
				if (relationship == null) continue;
				if (pair.Value == null)
				{
					BusinessObjectCollection<BusinessObject> boCol;
					boCol = relationship.GetRelatedBusinessObjectCol();
					if (boCol.Count > 0)
					{
						if (!results.ContainsKey(thisRelationshipPath))
						{
							results.Add(thisRelationshipPath, boCol.Count);
						} else
						{
							results[thisRelationshipPath] += boCol.Count;
						}
					}
				}
				else if (pair.Value.Count > 0)
				{
					BusinessObjectCollection<BusinessObject> boCol;
					boCol = relationship.GetRelatedBusinessObjectCol();
					foreach (BusinessObject businessObject in boCol)
					{
						CheckCanDeleteSafe(businessObject, alreadyChecked, pair.Value, thisRelationshipPath, ref results);
					}
				}
			}

			//foreach (Relationship relationship in bo.Relationships)
			//{
			//    MultipleRelationship multipleRelationship = relationship as MultipleRelationship;
			//    if (multipleRelationship != null)
			//    {
			//        MultipleRelationshipDef multipleRelationshipDef =
			//            multipleRelationship.RelationshipDef as MultipleRelationshipDef;
			//        if (multipleRelationshipDef != null)
			//        {
			//            if (multipleRelationshipDef.DeleteParentAction == DeleteParentAction.Prevent)
			//            {
			//                BusinessObjectCollection<BusinessObject> col;
			//                col = multipleRelationship.GetRelatedBusinessObjectCol();
			//                int count = col.Count;
			//                if (count > 0)
			//                {
			//                    reason += "";
			//                }
			//            }
			//        }
			//    }
			//}

		}

		
		public static MatchList FindPreventDeleteRelationships(RelationshipDefCol relationshipDefCol)
		{
			return FindRelationships<MultipleRelationshipDef>(relationshipDefCol, PreventDeleteRelationshipCondition);
		}

		public delegate bool MatchesConditionDelegate<TRelationshipDef>(TRelationshipDef relationshipDef)
			where TRelationshipDef : RelationshipDef;

		public static MatchList FindRelationships<TRelationshipDef>(RelationshipDefCol relationshipDefCol,
			MatchesConditionDelegate<TRelationshipDef> matchesConditionDelegate)
			where TRelationshipDef : RelationshipDef
		{
			return FindRelationshipsSafe(relationshipDefCol, matchesConditionDelegate, new List<RelationshipDefCol>());
		}

		private static MatchList FindRelationshipsSafe<TRelationshipDef>(RelationshipDefCol relationshipDefCol,
			MatchesConditionDelegate<TRelationshipDef> matchesConditionDelegate, List<RelationshipDefCol> alreadyChecked)
			where TRelationshipDef : RelationshipDef
		{
			MatchList listOfPaths = new MatchList();
			if (matchesConditionDelegate == null) return listOfPaths;
			if (alreadyChecked.Contains(relationshipDefCol)) return listOfPaths;
			alreadyChecked.Add(relationshipDefCol);
			foreach (RelationshipDef relationshipDef in relationshipDefCol)
			{
				string relationshipName = relationshipDef.RelationshipName;
				TRelationshipDef castedRelationshipDef =
					relationshipDef as TRelationshipDef;
				if (castedRelationshipDef != null)
				{
					bool matchesCondition;
					matchesCondition = matchesConditionDelegate(castedRelationshipDef);
					if (matchesCondition)
					{
						listOfPaths.Add(relationshipName, null);
					} else
					{
						ClassDef classDef = relationshipDef.RelatedObjectClassDef;
						if (classDef != null)
						{
							MatchList results;
							results = FindRelationshipsSafe(classDef.RelationshipDefCol, matchesConditionDelegate, alreadyChecked);
							if (results.Count > 0)
							{
								listOfPaths.Add(relationshipName, results);
							}
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
			string keysString = "";
			foreach (KeyValuePair<string, MatchList> pair in this)
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
