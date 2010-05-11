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
using System;
using System.Collections.Generic;
using Habanero.Base.Exceptions;

namespace Habanero.Base
{
    /// <summary>
    /// Represents a source from which data is retrieved
    /// </summary>
    public class Source
    {
        private string _name;
        private string _entityName;
        private readonly JoinList _joins;
        private readonly JoinList _inheritanceJoins;

        ///<summary>
        /// Constructs a <see cref="Source"/> with the name of the source.
        ///</summary>
        ///<param name="name">The name of the source.</param>
        public Source(string name) : this(name, name)
        {
        }

        ///<summary>
        /// Creates a source 
        ///</summary>
        ///<param name="name"></param>
        ///<param name="entityName"></param>
        public Source(string name, string entityName)
        {
            _joins = new JoinList(this);
            _inheritanceJoins = new JoinList(this);
            _name = name;
            _entityName = entityName;
        }

        /// <summary>
        /// Gets and sets the name of the source
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets and sets the entity name of the source
        /// </summary>
        public virtual string EntityName
        {
            get { return _entityName; }
            set { _entityName = value; }
        }

        /// <summary>
        /// Gets the list of joins that make up the source
        /// </summary>
        public virtual JoinList Joins
        {
            get { return _joins; }
        }

        /// <summary>
        /// Gets the source which is a child of this one, which can
        /// occur where one source inherits from another
        /// </summary>
        public Source ChildSource
        {
            get
            {
                if (Joins.Count == 0) return null;
                return Joins[0].ToSource;
            }
        }

        /// <summary>
        /// Gets the furthermost child
        /// </summary>
        public Source ChildSourceLeaf
        {
            get {
                if (ChildSource != null)  return ChildSource.ChildSourceLeaf;
                return this;
            }
        }

        /// <summary>
        /// Gets the list of joins that create one source through inheritance
        /// </summary>
        public virtual JoinList InheritanceJoins
        {
            get { return _inheritanceJoins; }
        }

        /// <summary>
        /// Gets and sets the value that indicates whether the source has been prepared
        /// </summary>
        public bool IsPrepared { get; set; }

        /// <summary>
        /// Returns this source in a string form
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string toString = Name;
            if (this.Joins.Count >0)
            {
                toString += "." + this.Joins[0].ToSource;
            }
            return toString;
        }

        /// <summary>
        /// Returns this source in hash code form
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the given source is equal to this one
        /// </summary>
        /// <param name="obj">The source to compare with this one</param>
        /// <returns>Returns true if equal, false if not</returns>
        public override bool Equals(object obj)
        {
            Source source = obj as Source;
            if (source == null) return false;
            return Equals(_name, source._name);
        }

        /// <summary>
        /// Joins a given source onto this one using an inner join
        /// </summary>
        /// <param name="toSource">The source to join onto this one</param>
        public void JoinToSource(Source toSource)
        {
            Joins.AddNewJoinTo(toSource, JoinType.InnerJoin);
        }

        /// <summary>
        /// Manages a list of joins that make up a <see cref="Source"/>
        /// </summary>
        public class JoinList : List<Join>
        {
            private readonly Source _fromSource;

            ///<summary>
            /// Creates a Join List
            ///</summary>
            ///<param name="fromSource"></param>
            ///<exception cref="ArgumentNullException"></exception>
            public JoinList(Source fromSource)
            {
                if (fromSource == null)
                {
                    throw new ArgumentNullException("fromSource");
                }
                _fromSource = fromSource;
            }

            /// <summary>
            /// Gets the source containing this join list
            /// </summary>
            public Source FromSource
            {
                get { return _fromSource; }
            }

            /// <summary>
            /// Merges a given list of joins into this one
            /// </summary>
            /// <param name="joinListToMerge">The list of joins to add to this one</param>
            public void MergeWith(JoinList joinListToMerge)
            {
                Source fromSourceToMerge = joinListToMerge.FromSource;
                if (!_fromSource.Equals(fromSourceToMerge))
                    throw new HabaneroDeveloperException(
                        "A source's joins cannot merge with another source's joins " + 
                        "if they do not have the same base source.",
                        "Please check your Source structures. Base Source:" + this +
                        ", Source to merge:" + fromSourceToMerge);
                if (joinListToMerge.Count == 0) return;
                Join inheritanceJoinToMerge = joinListToMerge[0];
                Source toSourceToMerge = inheritanceJoinToMerge.ToSource;
                Join inheritanceJoin;
                Source toSource = null;
                if (this.Count > 0)
                {
                    inheritanceJoin = this[0];
                    toSource = inheritanceJoin.ToSource;
                }
                if (!Equals(toSourceToMerge, toSource))
                {
                    toSource = toSourceToMerge;
                    Join newJoin = this.AddNewJoinTo(toSourceToMerge, inheritanceJoinToMerge.JoinType);
                    if (newJoin != null)
                    {
                        foreach (Join.JoinField joinField in inheritanceJoinToMerge.JoinFields)
                        {
                            newJoin.JoinFields.Add(joinField);
                        }
                    }
                }
                if (toSource != null)
                {
                    toSource.MergeWith(toSourceToMerge);
                }
                
            }

            /// <summary>
            /// Adds a new join from the source containing this join list to the
            /// specified source, using the specified join type
            /// </summary>
            /// <param name="toSource">The source to connect the current source to</param>
            /// <param name="joinType">The type of join to use</param>
            /// <returns>Returns the newly created join</returns>
            public Join AddNewJoinTo(Source toSource, JoinType joinType)
            {
                if (toSource == null) return null;
                bool alreadyExists = this.Exists(join1 => join1.ToSource.Name.Equals(toSource.Name));
                if (alreadyExists) return null;
                Join join = new Join(_fromSource, toSource.Clone(), joinType);
                this.Add(join);
                return join;
            }
        }

        /// <summary>
        /// Provides a list of join types used to connect sources
        /// </summary>
        public enum JoinType
        {
            /// <summary>
            /// Merges two sources only on the records with they match on some given criteria
            /// </summary>
            InnerJoin,
            /// <summary>
            /// Merges two sources by including all records in the primary (left) source
            /// and only rows in the secondary (right) source that match on some given criteria
            /// </summary>
            LeftJoin
        }

        /// <summary>
        /// Represents a join between sources that allows the multiple sources to
        /// be regarded as one
        /// </summary>
        public class Join
        {
            private readonly Source _fromSource;
            private readonly Source _toSource;
            private readonly List<JoinField> _joinFields = new List<JoinField>( );

            ///<summary>
            /// Constructor for Join
            ///</summary>
            ///<param name="fromSource"></param>
            ///<param name="toSource"></param>
            public Join(Source fromSource, Source toSource) : this(fromSource, toSource, JoinType.InnerJoin)
            {
            }

            ///<summary>
            /// Constructor for Join
            ///</summary>
            ///<param name="fromSource"></param>
            ///<param name="toSource"></param>
            ///<param name="joinType"></param>
            public Join(Source fromSource, Source toSource, JoinType joinType)
            {
                _fromSource = fromSource;
                _toSource = toSource;
                JoinType = joinType;
            }

            /// <summary>
            /// Gets the primary source from which the join originates
            /// </summary>
            public Source FromSource
            {
                get { return _fromSource; }
            }

            /// <summary>
            /// Gets the source to which this join connects
            /// </summary>
            public Source ToSource
            {
                get { return _toSource; }
            }

            /// <summary>
            /// Gets a list of fields on which the two sources must match
            /// </summary>
            public List<JoinField> JoinFields
            {
                get { return _joinFields; }
            }

            /// <summary>
            /// Gets and sets the type of join used to connect the sources
            /// </summary>
            public JoinType JoinType { get; set; }

            /// <summary>
            /// Represents a field on which two sources must match
            /// </summary>
            public class JoinField
            {
                private readonly QueryField _fromField;
                private readonly QueryField _toField;

                ///<summary>
                /// Constructor for a JoinField
                ///</summary>
                ///<param name="fromField"></param>
                ///<param name="toField"></param>
                public JoinField(QueryField fromField, QueryField toField)
                {
                    _fromField = fromField;
                    _toField = toField;
                }

                /// <summary>
                /// Gets the field in the primary source
                /// </summary>
                public QueryField FromField
                {
                    get { return _fromField; }
                }

                /// <summary>
                /// Gets the field in the secondary source
                /// </summary>
                public QueryField ToField
                {
                    get { return _toField; }
                }
            }

            /// <summary>
            /// Gets the clause used to indicate the type of join
            /// </summary>
            /// <returns></returns>
            public string GetJoinClause()
            {
                switch (JoinType)
                {
                    case JoinType.InnerJoin:
                        return "JOIN";
                    case JoinType.LeftJoin:
                        return "LEFT JOIN";
                }
                return "";
            }

            /// <summary>
            /// Does a shallow clone of this join (i.e. doesn't clone the FromSource and ToSource)
            /// </summary>
            /// <returns></returns>
            public Join Clone()
            {
                Join clone = new Join(this.FromSource, this.ToSource, this.JoinType);
                foreach (JoinField joinField in JoinFields)
                {
                    clone.JoinFields.Add(joinField);
                }
                return clone;
            }
        }

        /// <summary>
        /// Gets the string clause that represents the source from which a join originates
        /// </summary>
        /// <param name="sourcename">The name of the source</param>
        /// <returns></returns>
        public static Source FromString(string sourcename)
        {
            if (String.IsNullOrEmpty(sourcename)) return null;
            string[] sourceParts = sourcename.Split('.');
            if (sourceParts.Length == 1) return new Source(sourcename);
            Source baseSource = new Source(sourceParts[0]);
            Source currentSource = baseSource;
            for (int i = 1; i < sourceParts.Length; i++)
            {
                Source newSource = new Source(sourceParts[i]);
                currentSource.Joins.Add(new Join(currentSource, newSource, JoinType.LeftJoin));
                currentSource = newSource;
            }
            return baseSource;
        }

        /// <summary>
        /// Merges the current source with another specified source
        /// </summary>
        /// <param name="sourceToMerge">The source to merge this one with</param>
        public void MergeWith(Source sourceToMerge)
        {
            if (sourceToMerge == null) return;
            if (String.IsNullOrEmpty(sourceToMerge.Name)) return;
            if (!this.Equals(sourceToMerge))
                throw new HabaneroDeveloperException("A source cannot merge with another source if they do not have the same base source.", 
                        "Please check your Source structures. Base Source:" + this + " source to merge " + sourceToMerge);

            this.InheritanceJoins.MergeWith(sourceToMerge.InheritanceJoins);
            this.Joins.MergeWith(sourceToMerge.Joins);
        }

        /// <summary>
        /// Does a shallow clone of this Source. That is, it copies this Source object and its lists of joins, but the 
        /// sources linked to in the joins are not copied.
        /// </summary>
        /// <returns></returns>
        public Source Clone()
        {
            Source clonedSource = new Source(this.Name, this.EntityName);
            foreach (Join join in Joins)
            {
                clonedSource.Joins.Add(join.Clone());
            }
            foreach (Join join in InheritanceJoins)
            {
                clonedSource.InheritanceJoins.Add(join.Clone());
            }
            return clonedSource;
        }
    }
}
