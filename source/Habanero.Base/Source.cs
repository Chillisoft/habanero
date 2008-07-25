using System;
using System.Collections.Generic;
using System.Text;

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

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string EntityName
        {
            get { return _entityName; }
            set { _entityName = value; }
        }

        public List<Join> Joins
        {
            get { return _joins; }
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
    }

   
}
