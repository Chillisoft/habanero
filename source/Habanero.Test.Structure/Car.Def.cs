// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class Car : Vehicle
    {
        
        #region Properties
        public virtual Guid? CarID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("CarID")));
            }
            set
            {
                base.SetPropertyValue("CarID", value);
            }
        }
        
        public virtual String RegistrationNo
        {
            get
            {
                return ((String)(base.GetPropertyValue("RegistrationNo")));
            }
            set
            {
                base.SetPropertyValue("RegistrationNo", value);
            }
        }
        
        public virtual Double? Length
        {
            get
            {
                return ((Double?)(base.GetPropertyValue("Length")));
            }
            set
            {
                base.SetPropertyValue("Length", value);
            }
        }
        
        public virtual Boolean? IsConvertible
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("IsConvertible")));
            }
            set
            {
                base.SetPropertyValue("IsConvertible", value);
            }
        }
        
        public virtual Guid? DriverID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("DriverID")));
            }
            set
            {
                base.SetPropertyValue("DriverID", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual Person Driver
        {
            get
            {
                return Relationships.GetRelatedObject<Person>("Driver");
            }
            set
            {
                Relationships.SetRelatedObject("Driver", value);
            }
        }
        #endregion
    }
}
