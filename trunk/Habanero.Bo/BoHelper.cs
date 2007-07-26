using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// Supplies additional functionality for business objects
    /// </summary>
    public class BOHelper
    {
        /// <summary>
        /// Calls the BeforeSave() method which carries out additional
        /// steps before the Save() command is run
        /// </summary>
        /// <returns>Returns the result of the call (true could indicate that
        /// the steps were carried out successfully)</returns>
        public static bool CallBeforeApplyEdit(BusinessObject bo) {
            return bo.BeforeSave();
        }

    }
}
