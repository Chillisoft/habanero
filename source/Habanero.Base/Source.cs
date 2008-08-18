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
        private List<Join> _joins;

        public Source(string name) : this(name, name)
        {
        }

        public Source(string name, string entityName)
        {
            _joins = new List<Join>();
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

        public virtual List<Join> Joins
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
            if (toSource == null) return;
            if (Joins.Exists(delegate(Join join1) { return join1.ToSource.Name.Equals(toSource.Name); })) return;
            this.Joins.Add(new Join(this, toSource));
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

            if (sourceToMerge.ChildSource == null)
                return;

            if (!sourceToMerge.ChildSource.Equals(this.ChildSource))
            {
                Join newJoin = new Join(this, sourceToMerge.ChildSource);
                foreach (Join.JoinField joinField in sourceToMerge.Joins[0].JoinFields)
                {
                    newJoin.JoinFields.Add(joinField);
                }
                this.Joins.Add(newJoin);
            }
            this.ChildSource.MergeWith(sourceToMerge.ChildSource);
        }
    }
}
