using System;
using System.Reflection;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using log4net;

namespace Habanero.UI.Win
{
    public class DefaultBOEditorFormWin : FormWin, IDefaultBOEditorForm
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.UI.Win.DefaultBOEditorFormWin");
        private BusinessObject _bo;
        private IControlFactory _controlFactory;
        private string _uiDefName;
        private IPanelFactoryInfo _panelFactoryInfo;
        private IPanel _boPanel;
        private IButtonGroupControl _buttons;
        private PostObjectPersistingDelegate _action;


        public DefaultBOEditorFormWin(BusinessObject bo, string name, IControlFactory controlFactory, PostObjectPersistingDelegate action)
            : this(bo, name, controlFactory)
        {
            _action = action;
            
        }

        public DefaultBOEditorFormWin(BusinessObject bo, string uiDefName, IControlFactory controlFactory)
        {
            _bo = bo;
            _controlFactory = controlFactory;
            _uiDefName = uiDefName;

            BOMapper mapper = new BOMapper(bo);

            UIForm def;
            if (_uiDefName.Length > 0)
            {
                UIDef uiMapper = mapper.GetUIDef(_uiDefName);
                if (uiMapper == null)
                {
                    throw new NullReferenceException("An error occurred while " +
                                                     "attempting to load an object editing form.  A possible " +
                                                     "cause is that the class definitions do not have a " +
                                                     "'form' section for the class, under the 'ui' " +
                                                     "with the name '" + _uiDefName + "'.");
                }
                def = uiMapper.GetUIFormProperties();
            }
            else
            {
                UIDef uiMapper = mapper.GetUIDef();
                if (uiMapper == null)
                {
                    throw new NullReferenceException("An error occurred while " +
                                                     "attempting to load an object editing form.  A possible " +
                                                     "cause is that the class definitions do not have a " +
                                                     "'form' section for the class.");
                }
                def = uiMapper.GetUIFormProperties();
            }
            if (def == null)
            {
                throw new NullReferenceException("An error occurred while " +
                                                 "attempting to load an object editing form.  A possible " +
                                                 "cause is that the class definitions do not have a " +
                                                 "'form' section for the class.");
            }

            IPanelFactory factory = new PanelFactory(_bo, def, _controlFactory);
            _panelFactoryInfo = factory.CreatePanel();
            _boPanel = _panelFactoryInfo.Panel;
            _buttons = _controlFactory.CreateButtonGroupControl();
            _buttons.AddButton("&Cancel", CancelButtonHandler);
            IButton okbutton = _buttons.AddButton("&OK", OKButtonHandler);
            okbutton.NotifyDefault(true);
            this.AcceptButton = (ButtonWin)okbutton;
            this.Load += delegate { FocusOnFirstControl(); };

            Text = def.Title;
            SetupFormSize(def);
            MinimizeBox = false;
            MaximizeBox = false;
            //this.ControlBox = false;

            CreateLayout();
            OnResize(new EventArgs());
        }

        protected IPanel BoPanel
        {
            get { return _boPanel; }
        }

        private void CreateLayout()
        {
            BorderLayoutManager borderLayoutManager = new BorderLayoutManagerWin(this, _controlFactory);
            borderLayoutManager.AddControl(BoPanel, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(Buttons, BorderLayoutManager.Position.South);
        }

        private void SetupFormSize(UIForm def)
        {
            int width = def.Width;
            int minWidth = _boPanel.Width +
                           Margin.Left + Margin.Right;
            if (width < minWidth)
            {
                width = minWidth;
            }
            int height = def.Height;
            int minHeight = _boPanel.Height + _buttons.Height +
                            Margin.Top + Margin.Bottom;
            if (height < minHeight)
            {
                height = minHeight;
            }
            Height = height;
            Width = width;
        }

        private void FocusOnFirstControl()
        {
            IControlChilli controlToFocus = _panelFactoryInfo.FirstControlToFocus;
            MethodInfo focusMethod = controlToFocus.GetType().
                GetMethod("Focus", BindingFlags.Instance | BindingFlags.Public);
            if (focusMethod != null)
            {
                focusMethod.Invoke(controlToFocus, new object[] { });
            }
        }

        private void OKButtonHandler(object sender, EventArgs e)
        {
            try
            {
                _panelFactoryInfo.ControlMappers.ApplyChangesToBusinessObject();
                TransactionCommitter committer = CreateSaveTransaction();
                committer.CommitTransaction();
                DialogResult = DialogResult.OK;
                Close();
                if (_action != null)
                {
                    _action(this._bo);
                }
                _panelFactoryInfo.ControlMappers.BusinessObject = null;
            }
            catch (Exception ex)
            {
                log.Error(ExceptionUtilities.GetExceptionString(ex, 0, true));
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "There was a problem saving for the following reason(s):",
                    "Saving Problem");
            }
        }

        protected virtual TransactionCommitter CreateSaveTransaction()
        {
            TransactionCommitterDB committer = new TransactionCommitterDB();
            committer.AddBusinessObject(_bo);
            return committer;
        }

        private void CancelButtonHandler(object sender, EventArgs e)
        {
            _panelFactoryInfo.ControlMappers.BusinessObject = null;
            _bo.Restore();
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public IButtonGroupControl Buttons
        {
            get { throw new NotImplementedException(); }
        }

        bool IDefaultBOEditorForm.ShowDialog()
        {
            {
                if (this.ShowDialog() == DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}