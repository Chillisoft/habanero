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
namespace Habanero.Util
{
    ///<summary>
    /// A delegate for a function with the specified return type and one parameter of the specified parameter type.
    /// This is the equivalent of the System.Func&lt;TReturn&gt; in .Net 3.
    ///</summary>
    ///<typeparam name="TReturn">The return type of the function.</typeparam>
    public delegate TReturn Function<TReturn>();

    ///<summary>
    /// A delegate for a function with the specified return type and one parameter of the specified parameter type.
    /// This is the equivalent of the System.Func&lt;T,TReturn&gt; in .Net 3.
    ///</summary>
    ///<param name="arg0">The first argument of the function.</param>
    ///<typeparam name="TArg0">The type of the first argument of the function.</typeparam>
    ///<typeparam name="TReturn">The return type of the function.</typeparam>
    public delegate TReturn Function<TArg0, TReturn>(TArg0 arg0);
}