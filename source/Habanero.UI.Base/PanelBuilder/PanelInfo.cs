using System.Collections;
using System.Collections.Generic;
using Habanero.Base.Exceptions;
using Habanero.BO;

namespace Habanero.UI.Base
{
    public class PanelInfo : IPanelInfo
    {
        private IPanel _panel;
        private GridLayoutManager _layoutManager;
        private readonly FieldInfoCollection _fieldInfos;
        private BusinessObject _businessObject;
        private IList<IPanelInfo> _panelInfos;

        public PanelInfo()
        {
            _fieldInfos = new FieldInfoCollection();
            _panelInfos = new List<IPanelInfo>();
        }

        public IPanel Panel
        {
            get { return _panel; }
            set { _panel = value; }
        }

        public GridLayoutManager LayoutManager
        {
            get { return _layoutManager; }
            set { _layoutManager = value; }
        }

        public FieldInfoCollection FieldInfos
        {
            get { return _fieldInfos; }
        }

        public BusinessObject BusinessObject
        {
            get { return _businessObject; }
            set
            {
                _businessObject = value;
                for (int fieldInfoNum = 0; fieldInfoNum < _fieldInfos.Count; fieldInfoNum++)
                {
                    _fieldInfos[fieldInfoNum].ControlMapper.BusinessObject = value;
                }
            }
        }

        public bool ControlsEnabled
        {
            set
            {
                foreach (FieldInfo fieldInfo in _fieldInfos)
                {
                    fieldInfo.ControlMapper.Control.Enabled = value;
                }
            }
        }

        public IList<IPanelInfo> PanelInfos
        {
            get { return _panelInfos; }
        }

        public void ApplyChangesToBusinessObject()
        {
            for (int fieldInfoNum = 0; fieldInfoNum < _fieldInfos.Count; fieldInfoNum++)
            {
                _fieldInfos[fieldInfoNum].ControlMapper.ApplyChangesToBusinessObject();
            }
        }

        public void ClearErrorProviders()
        {
            foreach (FieldInfo fieldInfo in FieldInfos)
            {
                fieldInfo.ControlMapper.ErrorProvider.SetError(fieldInfo.InputControl, "");
            }
        }

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

        public class FieldInfo
        {
            private readonly ILabel _label;
            private string _propertyName;
            private IControlMapper _controlMapper;

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