//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{
    public class DataStoreInMemory
    {
        private Dictionary<IPrimaryKey, IBusinessObject> _objects = new Dictionary<IPrimaryKey, IBusinessObject>();

        public int Count
        {
            get { return _objects.Count; }
        }

        public void Add(IBusinessObject businessObject)
        {
            _objects.Add(businessObject.ID, businessObject);
        }

        public Dictionary<IPrimaryKey, IBusinessObject> AllObjects
        {
            get { return _objects; }
        }

        public T Find<T>(Criteria criteria) where T : class, IBusinessObject
        {
            foreach (IBusinessObject bo in _objects.Values)
            {
                T boAsT = bo as T;
                if (boAsT == null) continue; ;
                if (criteria.IsMatch(boAsT)) return boAsT;
            }
            return null;
        }

        public IBusinessObject Find(Type BOType, Criteria criteria)
        {
            foreach (IBusinessObject bo in _objects.Values)
            {
                if (BOType.IsInstanceOfType(bo) && criteria.IsMatch(bo)) return bo;
            }
            return null;
        }

        public T Find<T>(IPrimaryKey primaryKey) where T : class, IBusinessObject
        {
            foreach (IBusinessObject bo in _objects.Values)
            {
                if (bo.ID.Equals(primaryKey)) return bo as T;
            }
            return null;
        }

        public void Remove(IBusinessObject businessObject)
        {
            _objects.Remove(businessObject.ID);
        }

        public BusinessObjectCollection<T> FindAll<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            foreach (IBusinessObject bo in _objects.Values)
            {
                T boAsT = bo as T;
                if (boAsT == null) continue; ;
                if (criteria == null || criteria.IsMatch(boAsT)) col.Add(boAsT);
            }
            col.SelectQuery.Criteria = criteria;
            return col;
        }

        public IBusinessObjectCollection FindAll(Type BOType, Criteria criteria) 
        {
            Type boColType = typeof (BusinessObjectCollection<>).MakeGenericType(BOType);
            IBusinessObjectCollection col = (IBusinessObjectCollection) Activator.CreateInstance(boColType);
            foreach (IBusinessObject bo in _objects.Values)
            {

                if (BOType.IsInstanceOfType(bo))
                {
                    if (criteria == null || criteria.IsMatch(bo)) col.Add(bo);
                }
            }
            col.SelectQuery.Criteria = criteria;
            return col;
        }
    }
}