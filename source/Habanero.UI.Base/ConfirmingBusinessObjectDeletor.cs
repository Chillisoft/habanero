// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.UI.Base
{
    ///<summary>
    /// This is an <see cref="IBusinessObjectDeletor"/> that uses the specified <see cref="IConfirmer"/> to prompt the user
    /// for confirmation of the deletion. If the user confirms, the BusinessObject will be deleted. 
    /// If the user does not confirm then the BusinessObject will not be deleted.
    ///</summary>
    public class ConfirmingBusinessObjectDeletor : IBusinessObjectDeletor
    {
        ///<summary>
        /// The <see cref="IConfirmer"/> to use when prompting the user for confirmation.
        ///</summary>
        public IConfirmer Confirmer { get; private set; }
        
        ///<summary>
        /// The delegate that has been provided for constructing the confirmation message that will be 
        /// displayed to the user for a particular <see cref="IBusinessObject"/>.
        ///</summary>
        public Function<IBusinessObject, string> CustomConfirmationMessageDelegate { get; private set; }

        ///<summary>
        /// Constructs a new <see cref="ConfirmingBusinessObjectDeletor"/> with the specified <see cref="IConfirmer"/>.
        ///</summary>
        ///<param name="confirmer">The <see cref="IConfirmer"/> to use to prompt the user for confirmation of the deletion.</param>
        public ConfirmingBusinessObjectDeletor(IConfirmer confirmer)
        {
            Confirmer = confirmer;
        }

        ///<summary>
        /// Constructs a new <see cref="ConfirmingBusinessObjectDeletor"/> with the specified <see cref="IConfirmer"/>
        /// and a delegate for constructing the confirmation message.
        ///</summary>
        ///<param name="confirmer">The <see cref="IConfirmer"/> to use to prompt the user for confirmation of the deletion.</param>
        ///<param name="customConfirmationMessageDelegate">The delegate to use for constructing the confirmation message that will be 
        /// displayed to the user for a particular <see cref="IBusinessObject"/>.</param>
        public ConfirmingBusinessObjectDeletor(IConfirmer confirmer,
                                               Function<IBusinessObject, string> customConfirmationMessageDelegate)
            : this(confirmer)
        {
            CustomConfirmationMessageDelegate = customConfirmationMessageDelegate;
        }

        ///<summary>
        /// Deletes the given business object
        ///</summary>
        ///<param name="businessObject">The business object to delete</param>
        public void DeleteBusinessObject(IBusinessObject businessObject)
        {
            string message;
            if (CustomConfirmationMessageDelegate != null)
            {
                message = CustomConfirmationMessageDelegate(businessObject);
            }
            else
            {
                message = string.Format("Are you certain you want to delete the object '{0}'", businessObject);
            }
            Confirmer.Confirm(message, delegate(bool confirmed)
            {
                if (!confirmed) return;
                DefaultBODeletor defaultBODeletor = new DefaultBODeletor();
                try
                {
                    defaultBODeletor.DeleteBusinessObject(businessObject);
                }
                catch (Exception ex)
                {
                    GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error Deleting");
                }
            });
        }
    }
}