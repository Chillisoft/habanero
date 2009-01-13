using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;

namespace Habanero.UI.Base
{
    ///<summary>
    /// A panel info is a class that wraps the panel and provides functionality 
    ///  for linking a business object a layout manager and a panel.
    ///</summary>
    public class PanelInfo : IPanelInfo
    {
        private IBusinessObject _businessObject;

        ///<summary>
        /// Constructs a panel info with a list of fields and a list of children panel info.
        ///</summary>
        public PanelInfo()
        {
            FieldInfos = new FieldInfoCollection();
            PanelInfos = new List<IPanelInfo>();
        }

        ///<summary>
        /// The panel that this panel info is controlling
        ///</summary>
        public IPanel Panel { get; set; }

        ///<summary>
        /// Gets and sets the layout manager used for this Panel
        ///</summary>
        public GridLayoutManager LayoutManager { get; set; }

        ///<summary>
        /// Returns a list of Field infos (info on the fields controlled by this panel.
        ///</summary>
        public FieldInfoCollection FieldInfos { get; private set; }

        ///<summary>
        /// Sets the business object for this panel.
        ///</summary>
        public IBusinessObject BusinessObject
        {
            get { return _businessObject; }
            set
            {
                _businessObject = value;
                if (this.BusinessObject != null) this.BusinessObject.IsValid();//This causes the BO to run all its validation rules.
                for (int fieldInfoNum = 0; fieldInfoNum < FieldInfos.Count; fieldInfoNum++)
                {
                    FieldInfos[fieldInfoNum].ControlMapper.BusinessObject = value;
                }
            }
        }

        ///<summary>
        /// Sets whether the controls on this panel are enabled or not
        ///</summary>
        public bool ControlsEnabled
        {
            set
            {
                foreach (FieldInfo fieldInfo in FieldInfos)
                {
                    fieldInfo.ControlMapper.Control.Enabled = value;
                }
            }
        }

        ///<summary>
        /// A list of all panel infos containd in this panel info.
        ///</summary>
        public IList<IPanelInfo> PanelInfos { get; private set; }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public void ApplyChangesToBusinessObject()
        {
            for (int fieldInfoNum = 0; fieldInfoNum < FieldInfos.Count; fieldInfoNum++)
            {
                FieldInfos[fieldInfoNum].ControlMapper.ApplyChangesToBusinessObject();
            }

        }
        /// <summary>
        /// Sets all the error providers to have no errors.
        /// </summary>
        public void ClearErrorProviders()
        {
            foreach (FieldInfo fieldInfo in FieldInfos)
            {
                fieldInfo.ControlMapper.ErrorProvider.SetError(fieldInfo.InputControl, "");
            }
        }

        ///<summary>
        /// An enumeration of all 
        ///</summary>
        public class FieldInfoCollection : IEnumerable<FieldInfo>
        {
            private readonly IList<FieldInfo> _fieldInfos = new List<FieldInfo>();

            public FieldInfo this[string propertyName]
            {
                get
                {
                    foreach (FieldInfo fieldInfo in _fieldInfos)
                    {
                        if (fieldInfo.PropertyName == propertyName)
                        {
                            return fieldInfo;
                        }
                    }
                    throw new InvalidPropertyNameException(
                        string.Format("A label for the property {0} was not found.", propertyName));
                }
            }

            public FieldInfo this[int index]
            {
                get { return _fieldInfos[index]; }
            }

            public void Add(FieldInfo fieldInfo)
            {
                _fieldInfos.Add(fieldInfo);
            }

            public int Count
            {
                get { return _fieldInfos.Count; }
            }

            IEnumerator<FieldInfo> IEnumerable<FieldInfo>.GetEnumerator()
            {
                return _fieldInfos.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _fieldInfos.GetEnumerator();
            }
        }

        ///<summary>
        /// This class provides information on the control that is visible on a user form.
        /// It contains a reference to the label and the control mapper for a paticular control.
        ///</summary>
        public class FieldInfo
        {
            private readonly ILabel _label;
            private readonly string _propertyName;
            private readonly IControlMapper _controlMapper;

            ///<summary>
            /// Constructs a Field info.
            ///</summary>
            ///<param name="propertyName">The property that this field info is for</param>
            ///<param name="label">The label that this field info is wrapping</param>
            ///<param name="controlMapper">The control mapper that this field info is mapping</param>
            public FieldInfo(string propertyName, ILabel label, IControlMapper controlMapper)
            {
                _propertyName = propertyName;
                _label = label;
                _controlMapper = controlMapper;
            }

            public string PropertyName
            {
                get { return _propertyName; }
            }

            public ILabel Label
            {
                get { return _label; }
            }

            public IControlHabanero InputControl
            {
                get { return _controlMapper.Control; }
            }

            public IControlMapper ControlMapper
            {
                get { return _controlMapper; }
            }
        }
    }
}