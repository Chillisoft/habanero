using System.Collections.Generic;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.PanelBuilder
{
    [TestFixture]
    public class TestPanelInfo 
    {
        private IControlFactory _controlFactory = new ControlFactoryWin();
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [Test]
        public void TestPanel()
        {
            //---------------Set up test pack-------------------
            IPanelInfo panelInfo = new PanelInfo();
            IPanel panel = _controlFactory.CreatePanel();
            //---------------Assert Precondition----------------
            Assert.IsNull(panelInfo.Panel);
            //---------------Execute Test ----------------------
            panelInfo.Panel = panel;
            //---------------Test Result -----------------------
            Assert.AreSame(panel, panelInfo.Panel);

        }

        [Test]
        public void TestControlMappers()
        {
            //---------------Set up test pack-------------------
           
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = new PanelInfo();
            //---------------Test Result -----------------------
            Assert.IsNotNull(panelInfo.ControlMappers);
            Assert.AreEqual(0, panelInfo.ControlMappers.Count);
        }

        [Test]
        public void TestFieldInfos()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = new PanelInfo();
            //---------------Test Result -----------------------
            Assert.IsNotNull(panelInfo.FieldInfos);
            Assert.AreEqual(0, panelInfo.FieldInfos.Count);
        }

        [Test, ExpectedException(typeof(InvalidPropertyNameException))]
        public void TestFieldInfos_WrongPropertyNameGivesUsefulError()
        {
            //---------------Set up test pack-------------------
            IPanelInfo panelInfo = new PanelInfo();

            //---------------Execute Test ----------------------
            PanelInfo.FieldInfo fieldInfo = panelInfo.FieldInfos["invalidPropName"];
        }

        [Test]
        public void TestFieldInfo_Constructor()
        {
            //---------------Set up test pack-------------------
            ILabel label = _controlFactory.CreateLabel();
            string propertyName = TestUtil.CreateRandomString();
            ITextBox tb = _controlFactory.CreateTextBox();
            IControlMapper controlMapper = new TextBoxMapper(tb, propertyName, false, _controlFactory);
            IErrorProvider errorProvider = _controlFactory.CreateErrorProvider();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            PanelInfo.FieldInfo fieldInfo = new PanelInfo.FieldInfo(propertyName, label, controlMapper, errorProvider);

            //---------------Test Result -----------------------

            Assert.AreEqual(propertyName, fieldInfo.PropertyName);
            Assert.AreSame(label, fieldInfo.Label);
            Assert.AreSame(controlMapper, fieldInfo.ControlMapper);
            Assert.AreSame(tb, fieldInfo.InputControl);
            Assert.AreSame(errorProvider, fieldInfo.ErrorProvider);
        }

        [Test]
        public void TestSetBusinessObjectUpdatesControlMappers()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IPanelInfo panelInfo = new PanelInfo();
            panelInfo.FieldInfos.Add(CreateFieldInfo("SampleText"));
            panelInfo.FieldInfos.Add(CreateFieldInfo("SampleInt"));
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Sample sampleBO = new Sample();
            panelInfo.BusinessObject = sampleBO;
            //---------------Test Result -----------------------
            Assert.AreSame(sampleBO, panelInfo.BusinessObject);
            Assert.AreSame(sampleBO, panelInfo.FieldInfos[0].ControlMapper.BusinessObject);
            Assert.AreSame(sampleBO, panelInfo.FieldInfos[1].ControlMapper.BusinessObject);
        }

        [Test, Ignore("Eric: not sure how the underlying architecture works here")]
        public void TestApplyChangesToBusinessObject()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            Sample sampleBO = new Sample();
            const string startText = "startText";
            const string endText = "endText";
            sampleBO.SampleText = startText;
            sampleBO.SampleInt = 1;

            IPanelInfo panelInfo = new PanelInfo();
            PanelInfo.FieldInfo sampleTextFieldInfo = CreateFieldInfo("SampleText");
            PanelInfo.FieldInfo sampleIntFieldInfo = CreateFieldInfo("SampleInt");
            panelInfo.FieldInfos.Add(sampleTextFieldInfo);
            panelInfo.FieldInfos.Add(sampleIntFieldInfo);
            panelInfo.BusinessObject = sampleBO;

            sampleTextFieldInfo.InputControl.Text = endText;
            //---------------Assert Precondition----------------
            Assert.AreEqual(startText, sampleBO.SampleText);
            //---------------Execute Test ----------------------
            panelInfo.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual(endText, sampleBO.SampleText);
            Assert.AreEqual(1, sampleBO.SampleInt);
        }

        private PanelInfo.FieldInfo CreateFieldInfo(string propertyName)
        {
            ILabel label = _controlFactory.CreateLabel();
            ITextBox tb = _controlFactory.CreateTextBox();
            IControlMapper controlMapper = new TextBoxMapper(tb, propertyName, false, _controlFactory);
            IErrorProvider errorProvider = _controlFactory.CreateErrorProvider();
            return  new PanelInfo.FieldInfo(propertyName, label, controlMapper, errorProvider);
        }

        private PanelInfo.FieldInfo CreateFieldInfo()
        {
            return CreateFieldInfo(TestUtil.CreateRandomString());
        }
    }
    
    public class PanelInfo : IPanelInfo
    {
        private IPanel _panel;
        private readonly IControlMapperCollection _controlMappers;
        private GridLayoutManager _layoutManager;
        private readonly FieldInfoCollection _fieldInfos;
        private BusinessObject _businessObject;

        public PanelInfo()
        {
            _controlMappers = new ControlMapperCollection();
            _fieldInfos = new FieldInfoCollection();
        }

        public IPanel Panel
        {
            get { return _panel; }
            set { _panel = value; }
        }

        public IControlMapperCollection ControlMappers
        {
            get { return _controlMappers; }
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

        public void ApplyChangesToBusinessObject()
        {
            for (int fieldInfoNum = 0; fieldInfoNum < _fieldInfos.Count; fieldInfoNum++)
            {
                _fieldInfos[fieldInfoNum].ControlMapper.ApplyChangesToBusinessObject();
            }
        }

        public class FieldInfoCollection// : IEnumerable<FieldInfo>
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

            //public IEnumerator<FieldInfo> GetEnumerator()
            //{
            //    throw new System.NotImplementedException();
            //}
        }

        public class FieldInfo
        {
            private readonly ILabel _label;
            private string _propertyName;
            private IErrorProvider _errorProvider;
            private IControlMapper _controlMapper;

            public FieldInfo(string propertyName, ILabel label, IControlMapper controlMapper, IErrorProvider errorProvider)
            {
                _propertyName = propertyName;
                _label = label;
                _controlMapper = controlMapper;
                _errorProvider = errorProvider;
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

            public IErrorProvider ErrorProvider
            {
                get { return _errorProvider; }
            }

            public IControlMapper ControlMapper
            {
                get { return _controlMapper; }
            }
        }
    }

    public interface IPanelInfo
    {
        IPanel Panel { get; set; }
        IControlMapperCollection ControlMappers { get; }
        GridLayoutManager LayoutManager { get; set; }
        PanelInfo.FieldInfoCollection FieldInfos { get; }
        BusinessObject BusinessObject { get; set; }
        void ApplyChangesToBusinessObject();
    }
}