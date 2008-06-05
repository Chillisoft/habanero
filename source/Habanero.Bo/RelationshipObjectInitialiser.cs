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

using System.Data;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using log4net;

namespace Habanero.BO
{
    /// <summary>
    /// Initialises a relationship object
    /// </summary>
    public class RelationshipObjectInitialiser : IBusinessObjectInitialiser
    {
        private readonly string _correspondingRelationshipName;
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.RelationshipObjectInitialiser");
        private readonly RelationshipDef _relationship;
        private readonly IBusinessObject _parentObject;

        /// <summary>
        /// Constructor for a new initialiser
        /// </summary>
        /// <param name="parentObject">The parent for the relationship</param>
        /// <param name="relationship">The relationship object</param>
        /// <param name="correspondingRelationshipName">The corresponding
        /// relationship name</param>
        public RelationshipObjectInitialiser(IBusinessObject parentObject, RelationshipDef relationship,
                                             string correspondingRelationshipName)
        {
            _parentObject = parentObject;
            _relationship = relationship;
            _correspondingRelationshipName = correspondingRelationshipName;
        }

        /// <summary>
        /// Initialises the given object
        /// </summary>
        /// <param name="objToInitialise">The object to initialise</param>
        public void InitialiseObject(IBusinessObject objToInitialise)
        {
            //log.Debug("Entered initialiseobject.") ;
            //log.Debug(objToInitialise.GetType().Name);
            BusinessObject newBo = (BusinessObject) objToInitialise;
            //log.Debug(_relationship);
            //log.Debug(_relationship.RelKeyDef.Count + " props in relkeydef. ");

            // TODO - this code should go in the SetRelatedBusinessObject method.
			//foreach (DictionaryEntry relKeyDef in _relationship.RelKeyDef)
			foreach (RelPropDef propDef in _relationship.RelKeyDef)
			{
                //RelPropDef propDef = (RelPropDef) relKeyDef.Value;
                //log.Debug(propDef.OwnerPropertyName);
                //log.Debug(propDef.RelatedClassPropName);
                newBo.SetPropertyValue(propDef.OwnerPropertyName,
                                       _parentObject.GetPropertyValue(propDef.RelatedClassPropName));
            }
            newBo.Relationships.SetRelatedObject(_correspondingRelationshipName, _parentObject);
        }

        /// <summary>
        /// Initialises a DataRow object
        /// </summary>
        /// <param name="row">The DataRow object to initialise</param>
        public void InitialiseDataRow(DataRow row) {
        }
    }
}