//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using System;

namespace Habanero.Base.Logging
{
    ///<summary>
    ///</summary>
    public interface IHabaneroLoggerFactory
    {
        ///<summary>
        /// Creates the Appropriate <see cref="IHabaneroLogger"/>
        /// based on the LoggerFactory you have implemented.
        /// By Default Habanero uses the <see cref="Log4NetLoggerFactory"/>.
        /// You can change the logger factor by changing your the BootStrapper code when
        /// Your Application starts up or by creating your own <see cref="HabaneroApp"/>
        /// and overriding the <see cref="HabaneroApp.SetupLogging"/> or by calling
        /// the following code in your app startup.
        /// <code>
        /// GlobalRegistry.LoggerFactory = new Log4NetLoggerFactory();
        /// </code>
        ///</summary>
        ///<param name="contextName"></param>
        ///<returns></returns>
        IHabaneroLogger GetLogger(string contextName);

        ///<summary>
        /// Creates the Appropriate <see cref="IHabaneroLogger"/>
        /// based on the LoggerFactory you have implemented.
        /// See <see cref="GetLogger(string)"/> form more details
        ///</summary>
        ///<param name="type">The Type of BO this log is for</param>
        ///<returns></returns>
        IHabaneroLogger GetLogger(Type type);
    }
}