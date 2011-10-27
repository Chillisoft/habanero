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
    
    
    public partial class Engine : Part
    {
        
        #region Properties
        public virtual Guid? EngineID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("EngineID")));
            }
            set
            {
                base.SetPropertyValue("EngineID", value);
            }
        }
        
        public virtual String EngineNo
        {
            get
            {
                return ((String)(base.GetPropertyValue("EngineNo")));
            }
            set
            {
                base.SetPropertyValue("EngineNo", value);
            }
        }
        
        public virtual DateTime? DateManufactured
        {
            get
            {
                return ((DateTime?)(base.GetPropertyValue("DateManufactured")));
            }
            set
            {
                base.SetPropertyValue("DateManufactured", value);
            }
        }
        
        public virtual Int32? HorsePower
        {
            get
            {
                return ((Int32?)(base.GetPropertyValue("HorsePower")));
            }
            set
            {
                base.SetPropertyValue("HorsePower", value);
            }
        }
        
        public virtual Boolean? FuelInjected
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("FuelInjected")));
            }
            set
            {
                base.SetPropertyValue("FuelInjected", value);
            }
        }
        
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
        #endregion
        
        #region Relationships
        public virtual Car Car
        {
            get
            {
                return Relationships.GetRelatedObject<Car>("Car");
            }
            set
            {
                Relationships.SetRelatedObject("Car", value);
            }
        }
        #endregion
    }
}
