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
#region Copyright � 2005 Noogen Technologies Inc.
// Author:
//	Tommy Noogen (tom@noogen.net)
//
// (C) 2005 Noogen Technologies Inc. (http://www.noogen.net)
// 
// MIT X.11 LICENSE
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
#endregion

using System;

namespace Habanero.UI.Base
{
	/// <summary>
	/// Delegate for custom validation methods.
	/// </summary>
	public delegate	void CustomValidationEventHandler(object sender, CustomValidationEventArgs e);

	/// <summary>
	/// Provides arguments for a validation event
	/// </summary>
	public class CustomValidationEventArgs : EventArgs
	{
		private readonly object _value;
		private readonly ValidationRule _validationRule;

		///<summary>
		/// Constructs the <see cref="CustomValidationEventArgs"/>
		///</summary>
		///<param name="Value"></param>
		///<param name="vr"></param>
		public CustomValidationEventArgs(object Value, ValidationRule vr)
		{
			this._value = Value;
			this._validationRule = vr;
		}

		/// <summary>
		/// Gets the value to validate
		/// </summary>
		public object Value
		{
			get { return _value; }
		}

		/// <summary>
		/// Gets or sets the validity of the validation rule
		/// </summary>
		public bool IsValid
		{
			get { return this._validationRule.IsValid; }
			set { this._validationRule.IsValid = value; }
		}

		/// <summary>
		/// Gets or sets the error message to display when validation fails
		/// </summary>
		public string ErrorMessage
		{
			get { return this._validationRule.ErrorMessage; }
			set { this._validationRule.ErrorMessage = value; }
		}

		/// <summary>
		/// Gets the validation rule
		/// </summary>
		public ValidationRule ValidationRule
		{
			get { return this._validationRule;}
		}
	}
}
