using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.Bo
{
    /// <summary>
    /// Supplies additional functionality for business objects
    /// </summary>
    public class BOHelper
    {
        /// <summary>
        /// Calls the BeforeApplyEdit() method which carries out additional
        /// steps before the ApplyEdit() command is run
        /// </summary>
        /// <returns>Returns the result of the call (true could indicate that
        /// the steps were carried out successfully)</returns>
        public static bool CallBeforeApplyEdit(BusinessObject bo) {
            return bo.BeforeApplyEdit();
        }
    }
}
