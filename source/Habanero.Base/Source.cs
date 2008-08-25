using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base.Exceptions;

namespace Habanero.Base
{
    public class Source


    {
        private string _name;
        private string _entityName;
        private JoinList _joins;
        private JoinList _inheritanceJoins;

        public Source(string name) : this(name, name)
        {
        }

        public Source(string name, string entityName)
        {
            _joins = new JoinList(this);
            _inheritanceJoins = new JoinList(this);
            _name = name;
            _entityName = entityName;
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual string EntityName
        {
            get { return _entityName; }
            set { _entityName = value; }
        }

        public virtual JoinList Joins
        {
            get { return _joins; }
        }

        public Source ChildSource
        {
            get { if (Joins.Count == 0) return null;
                return Joins[0].ToSource;
        }
        }

        /// <summary>
        /// Returns the furthermost child. 
        /// </summary>
        public Source ChildSourceLeaf
        {
            get {
                if (ChildSource != null)  return ChildSource.ChildSourceLeaf;
                return this;
            }
        }

        public virtual JoinList InheritanceJoins
        {
            get { return _inheritanceJoins; }
        }

        public override string ToString()
        {
            string toString = Name;
            if (this.Joins.Count >0)
            {
                toString += "." + this.Joins[0].ToSource.ToString();
            }
            return toString;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Source source = obj as Source;
            if (source == null) return false;
            return Object.Equals(_name, source._name);
        }


        public void JoinToSource(Source toSource)
        {
            Joins.AddNewJoinTo(toSource);
        }


        public class JoinList : List<Join>
        {
            private Source _fromSource;

            public JoinList(Source fromSource)
            {
                if (fromSource == null)
                {
                    throw new ArgumentNullException("fromSource");
                }
                _fromSource = fromSource;
            }

            public Source FromSource
            {
                get { return _fromSource; }
            }

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
                Join inheritanceJoin = null;
                Source toSource = null;
                if (this.Count > 0)
                {
                    inheritanceJoin = this[0];
                    toSource = inheritanceJoin.ToSource;
                }
                if (!Object.Equals(toSourceToMerge, toSource))
                {
                    toSource = toSourceToMerge;
                    Join newJoin = this.AddNewJoinTo(toSourceToMerge);
                    foreach (Join.JoinField joinField in inheritanceJoinToMerge.JoinFields)
                    {
                        newJoin.JoinFields.Add(joinField);
                    }
                }
                if (toSource != null)
                {
                    toSource.MergeWith(toSourceToMerge);
                }
                
            }

            public Join AddNewJoinTo(Source toSource)
            {
                if (toSource == null) return null;
                bool alreadyExists = this.Exists(delegate(Join join1)
                {
                    return join1.ToSource.Name.Equals(toSource.Name);
                });
                if (alreadyExists) return null;
                Join join = new Join(_fromSource, toSource);
                this.Add(join);
                return join;
            }
        }

        public class Join
        {
            private Source _fromSource;
            private Source _toSource;
            private List<JoinField> _joinFields = new List<JoinField>( );

            public Join(Source fromSource, Source toSource)
            {
                _fromSource = fromSource;
                _toSource = toSource;
            }

            public Source FromSource
            {
                get { return _fromSource; }
            }

            public Source ToSource
            {
                get { return _toSource; }
            }

            public List<JoinField> JoinFields
            {
                get { return _joinFields; }
            }

            public class JoinField
            {
                private QueryField _fromField;
                private QueryField _toField;

                public JoinField(QueryField fromField, QueryField toField)
                {
                    _fromField = fromField;
                    _toField = toField;
                }

                public QueryField FromField
                {
                    get { return _fromField; }
                }

                public QueryField ToField
                {
                    get { return _toField; }
                }
            }
        }

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
                currentSource.Joins.Add(new Join(currentSource, newSource));
                currentSource = newSource;
            }
            return baseSource;
        }

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
    }
}
