using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.Base
{
    public class Source


    {
        private string _name;
        private string _entityName;

        public Source(string name) : this(name, name)
        {
        }

        public Source(string name, string entityName)
        {
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

        public override string ToString()
        {
            return Name;
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
    }
}
