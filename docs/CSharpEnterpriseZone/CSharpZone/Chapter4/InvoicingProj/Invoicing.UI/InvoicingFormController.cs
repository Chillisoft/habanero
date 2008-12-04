using System.Windows.Forms;
using Habanero.UI.Base;
using Habanero.UI.Forms;

namespace Invoicing.UI
{
    public class InvoicingFormController : FormController
    {
        //public const string OBJECT_NAME = "Object Name";

        public InvoicingFormController(Form parentForm) : base(parentForm) { }

        protected override IFormControl GetFormControl(string heading)
        {
            IFormControl formCtl = null;
            switch (heading)
            {
                //case OBJECT_NAME:
                //    formCtl = new IFormControl();
                //    break;
            }
            return formCtl;
        }
    }
}