using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;

namespace Habanero.Base
{
    public class BusinessObjectDTO
    {
        public string ClassDefName { get; private set; }
        public string ClassName { get; private set; }
        public string AssemblyName { get; private set; }
        public string ID { get; private set; }
        private readonly Dictionary<string, object> _props = new Dictionary<string, object>();

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

        public Dictionary<string, object> Props { get { return _props; } }
    }
}
