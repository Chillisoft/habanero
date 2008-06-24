//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

namespace Habanero.Base
{
    /// <summary>
    /// An interface to implement a transaction log. There are many 
    /// strategies for implementing transaction logs, such as recording 
    /// to a database table, recording to a text file, etc.<br/><br/>
    /// This interface fulfils the roll of the Strategy Object in the 
    /// GOF Strategy pattern.<br/><br/>
    /// The combination of properties passed to the class in its 
    /// constructor, methods or properties should be able to provide 
    /// all required functionality to implement the strategy chosen.
    /// </summary>
    public interface ITransactionLog: ITransactional
    {
        //    /// <summary>
        //    /// Record a transaction log for the business object
        //    /// </summary>
        //    /// <param name="busObj">The business object the transaction log is 
        //    /// being recorded for</param>
        //    /// <param name="logonUserName">The user who made the changes to the business object that is being logged</param>
        //    void RecordTransactionLog(BusinessObject busObj, string logonUserName);
    }
}