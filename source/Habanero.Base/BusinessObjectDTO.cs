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
using System.Collections.Generic;

namespace Habanero.Base
{
    ///<summary>
    /// A Business Object Data Transfer Object. used for transferring objects accross a network e.g when loading from a
    /// remote database.
    ///</summary>
    public class BusinessObjectDTO
    {
        /// <summary>
        /// the name of the <see cref="IClassDef"/> that this DTO is for.
        /// </summary>
        public string ClassDefName { get; private set; }
        /// <summary>
        /// the name of the <see cref="IBusinessObject"/> that this DTO is for
        /// </summary>
        public string ClassName { get; private set; }
        /// <summary>
        /// The Assembly name that this DTO is for.
        /// </summary>
        public string AssemblyName { get; private set; }
        /// <summary>
        /// The Unique identifier of this DTO.
        /// </summary>
        public string ID { get; private set; }
        private readonly Dictionary<string, object> _props = new Dictionary<string, object>();
        /// <summary>
        /// Constructs a DTO for a Business Object.
        /// </summary>
        /// <param name="businessObject"></param>
        public BusinessObjectDTO(IBusinessObject businessObject) {
            ClassDefName = businessObject.ClassDef.ClassName;
            ClassName = businessObject.ClassDef.ClassNameExcludingTypeParameter;
            AssemblyName = businessObject.ClassDef.AssemblyName;
            foreach (IBOProp boProp in businessObject.Props)
            {
                Props[boProp.PropertyName.ToUpper()] = boProp.Value;
            }
            ID = businessObject.ID.ToString();
        }
        /// <summary>
        /// A dictionalry of Properties keyed by the Property name for this DTO.
        /// </summary>
        public Dictionary<string, object> Props { get { return _props; } }
    }
}
