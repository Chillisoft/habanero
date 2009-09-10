// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// A model of a Select Query that can be used to load data from a data store.  This includes the Fields to load, the source to load from
    /// (such as the database table name), the OrderCriteria to use (what fields must be sorted on), the Criteria to use (only objects that
    /// match the given criteria will be loaded), and the number of objects to load (defined by the Limit).
    /// </summary>
    public class SelectQuery : ISelectQuery
    {
        private readonly Dictionary<string, QueryField> _fields = new Dictionary<string, QueryField>(5);
        private Criteria _criteria;
        private OrderCriteria _orderCriteria = new OrderCriteria();

        ///<summary>
        /// Creates a SelectQuery with no Criteria and no fields.  In order to use the SelectQuery at least on field must be added
        /// to the <see cref="Fields"/>, and a <see cref="Source"/> must be specified.
        ///</summary>
        public SelectQuery()
        {
            Limit = -1;
            FirstRecordToLoad = 0;
        }


        ///<summary>
        /// Creates a SelectQuery with a Critiria and no fields.  In order to use the SelectQuery at least on field must be added
        /// to the <see cref="Fields"/>, and a <see cref="Source"/> must be specified.
        ///</summary>
        /// <param name="criteria">The Criteria to initialise this SelectQuery with.</param>
        public SelectQuery(Criteria criteria) : this()
        {
            _criteria = criteria;
        }

        /// <summary>
        /// The Criteria to use when loading. Only objects that match these criteria will be loaded.
        /// </summary>
        public Criteria Criteria
        {
            get { return _criteria; }
            set
            {
                _criteria = value;
                if (Source == null)
                    throw new HabaneroApplicationException
                        ("You cannot set a Criteria for a SelectQuery if no Source has been set");
                if (_criteria == null) return;
                MergeCriteriaSource(_criteria);
            }
        }

        private void MergeCriteriaSource(Criteria criteria)
        {
            if (criteria == null) return;
            if (criteria.IsComposite())
            {
                MergeCriteriaSource(criteria.LeftCriteria);
                MergeCriteriaSource(criteria.RightCriteria);
            }
            else
            {
                this.Source.MergeWith(criteria.Field.Source);
            }
        }

        /// <summary>
        /// The fields to load from the data store.
        /// </summary>
        public Dictionary<string, QueryField> Fields
        {
            get { return _fields; }
        }

        /// <summary>
        /// The source of the data. In a database query this would be the first table listed in the FROM clause.
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        /// The fields to use to order a collection of objects when loading them.
        /// </summary>
        public OrderCriteria OrderCriteria
        {
            get { return _orderCriteria; }
            set
            {
                _orderCriteria = value;
                if (Source == null)
                    throw new HabaneroApplicationException
                        ("You cannot set an OrderCriteria for a SelectQuery if no Source has been set");
                if (_orderCriteria == null) return;
                foreach (OrderCriteria.Field field in _orderCriteria.Fields) this.Source.MergeWith(field.Source);
            }
        }

        /// <summary>
        /// The number of objects to load
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// The classdef this select query corresponds to. This can be null if the select query is being used
        /// without classdefs, but if it is built using the QueryBuilder 
        /// </summary>
        public IClassDef ClassDef { get; set; }

        /// <summary>
        /// Gets or sets criteria for the discriminator that is used in single table
        /// inheritance
        /// </summary>
        public Criteria DiscriminatorCriteria { get; set; }

        ///<summary>
        /// Gets and sets the first record to be loaded by the select query.
        ///</summary>
        public int FirstRecordToLoad { get; set; }
    }
}