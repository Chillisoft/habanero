//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

#region Copyright � 2005 VisualWebGUI Technologies Inc.
// Author:
//	Tommy VisualWebGUI (tom@VisualWebGUI.net)
//
// (C) 2005 VisualWebGUI Technologies Inc. (http://www.VisualWebGUI.net)
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Habanero.UI.Base;


namespace Habanero.UI.Base
{
	/// <summary>
	/// Provides validation properties to controls that can be validated
	/// </summary>
	public class ValidationProvider : IExtenderProvider
	{
        private readonly Dictionary<IControlHabanero, List<ValidationRule>> _ValidationRules = new Dictionary<IControlHabanero, List<ValidationRule>>();
		private readonly ValidationRule				_DefaultValidationRule	= new ValidationRule();
	    private readonly IErrorProvider _ErrorProvider;

	    public ValidationProvider(IErrorProvider errorProvider)
	    {
	        _ErrorProvider = errorProvider;
	    }

	    #region "public Validation Methods"

		/// <summary>
		/// Perform validation on all controls.
		/// </summary>
		/// <returns>False if any control contains invalid data.</returns>
		public bool Validate()
		{
			bool bIsValid = true;
			List<ValidationRule> vr = null;
            foreach (IControlHabanero ctrl in _ValidationRules.Keys)
			{
				this.Validate(ctrl);
                
				vr = this.GetValidationRules(ctrl);
                vr.ForEach(delegate(ValidationRule obj)
                {
                    if (vr != null && obj.IsValid == false) bIsValid = false;
                });
				
			}
			return bIsValid;
		}

        /// <summary>
        /// Perform validation on a specific control
        /// </summary>
        /// <param name="ctrl">The control to validate</param>
        /// <returns>Returns true if valid, false if not</returns>
        public bool ValidateControl(IControlHabanero ctrl)
        {
            bool bIsValid = true;
            List<ValidationRule> vr;
            
                this.Validate(ctrl);

                vr = this.GetValidationRules(ctrl);
                vr.ForEach(delegate(ValidationRule obj)
                {
                    if (obj != null && obj.IsValid == false) bIsValid = false;
                });
                
            
            return bIsValid;
        }

		/// <summary>
		/// Get validation error messages.
		/// </summary>
		public string ValidationMessages(bool showErrorIcon)
		{
			StringBuilder sb = new StringBuilder();
			List<ValidationRule> vr;
            foreach (IControlHabanero ctrl in _ValidationRules.Keys)
			{
				vr = this.GetValidationRules(ctrl);
				if (vr != null) 
				{
                    vr.ForEach(delegate(ValidationRule obj)
                    {
                        if (obj.IsValid == false && ctrl.Visible)
                        {
                            obj.ResultErrorMessage += obj.ErrorMessage.Replace("%ControlName%", ctrl.Name);
                            sb.Append(obj.ResultErrorMessage);
                            sb.Append(Environment.NewLine);
                            if (showErrorIcon)
                                this._ErrorProvider.SetError(ctrl, obj.ResultErrorMessage);
                            else
                                this._ErrorProvider.SetError(ctrl, null);
                        }
                        else
                            this._ErrorProvider.SetError(ctrl, "");
                    });
					
				}
			}
			return sb.ToString();
		}

        /// <summary>
        /// Gets validation error messages for a specific control
        /// </summary>
        public string ValidationMessagesControl(IControlHabanero ctrl, bool showErrorIcon)
        {
            StringBuilder sb = new StringBuilder();
            List<ValidationRule> vr;
            
                vr = this.GetValidationRules(ctrl);
                if (vr != null)
                {
                    vr.ForEach(delegate(ValidationRule obj)
                    {
                        if (obj.IsValid == false && ctrl.Visible)
                        {
                            obj.ResultErrorMessage += obj.ErrorMessage.Replace("%ControlName%", ctrl.Name);
                            sb.Append(obj.ResultErrorMessage);
                            sb.Append(Environment.NewLine);
                        }
                        if (showErrorIcon)
                            this._ErrorProvider.SetError(ctrl, obj.ResultErrorMessage);
                        else
                            this._ErrorProvider.SetError(ctrl, null);
                        
                    });
                    
                }
            return sb.ToString();
        }
		#endregion

		#region "private helper methods"

		/// <summary>
		/// Perform validation on specific control.
		/// </summary>
        private bool Validate(IControlHabanero ctrl)
		{
			List<ValidationRule> vr = this.GetValidationRules(ctrl);
		    bool valid=false;
            vr.ForEach(delegate(ValidationRule obj)
            {
                if (obj != null)
                {
                    obj.ResultErrorMessage = string.Empty;
                    obj.IsValid = true;
                }

                if (obj == null || obj.IsValid)
                    obj = this.DataTypeValidate(ctrl);

                if (obj == null || obj.IsValid)
                    obj = this.CompareValidate(ctrl);

                if (obj == null || obj.IsValid)
                    obj = this.CustomValidate(ctrl);

                if (obj == null || obj.IsValid)
                    obj = this.RangeValidate(ctrl);

                if (obj == null || obj.IsValid)
                    obj = this.RegularExpressionValidate(ctrl);

                if (obj == null || obj.IsValid)
                    obj = this.RequiredFieldValidate(ctrl);
                
                valid=(obj == null) ? true : obj.IsValid;
            });
		    return valid;
		}

		/// <summary>
		/// Validate Data Type.
		/// </summary>
        private ValidationRule DataTypeValidate(IControlHabanero ctrl)
		{
            ValidationRule returnRule = new ValidationRule();
            List<ValidationRule> vr = this._ValidationRules[ctrl];
            vr.ForEach(delegate(ValidationRule obj)
            {
                if (obj != null && obj.Operator.Equals(ValidationCompareOperator.DataTypeCheck))
                {
                    if (obj.DataType.Equals(this._DefaultValidationRule.DataType))
                    {
                        returnRule = obj;
                    }
                    else
                    {
                        System.Web.UI.WebControls.ValidationDataType vdt =
                            (System.Web.UI.WebControls.ValidationDataType) Enum.Parse(typeof (System.Web.UI.WebControls.
                                                                               ValidationDataType),obj.DataType.ToString());

                        obj.IsValid = ValidationUtil.CanConvert(ctrl.Text, vdt);
                        returnRule = obj;
                    }
                }
            });
		    return returnRule;
		}

		/// <summary>
		/// Perform CompareValidate on a specific control.
		/// </summary>
		/// <returns>true if control has no validation rule.</returns>
        private ValidationRule CompareValidate(IControlHabanero ctrl)
		{
            ValidationRule returnRule = new ValidationRule();
            List<ValidationRule> vr = _ValidationRules[ctrl];
            vr.ForEach(delegate(ValidationRule obj)
            {
                if (obj != null)
                {
                    if (this._DefaultValidationRule.ValueToCompare.Equals(obj.ValueToCompare)
                        && this._DefaultValidationRule.Operator.Equals(obj.Operator))
                    {
                        returnRule = obj;
                    }
                    else
                    {
                        obj.IsValid = ValidationRule.Compare(ctrl.Text, obj.ValueToCompare, obj.Operator, obj);
                    }
                }
            });


            return returnRule;
		}

		/// <summary>
		/// Perform Custom Validation on specific control.
		/// </summary>
        private ValidationRule CustomValidate(IControlHabanero ctrl)
		{
            ValidationRule returnRule = new ValidationRule();
            List<ValidationRule> vr = _ValidationRules[ctrl];
            vr.ForEach(delegate(ValidationRule obj)
            {
                if (obj != null)
                {
                    CustomValidationEventArgs e = new CustomValidationEventArgs(ctrl.Text, obj);
                    obj.OnCustomValidationMethod(e);
                }
                returnRule = obj;
            });
		    return returnRule;
		}


		/// <summary>
		/// Perform Range Validation on a specific control.
		/// </summary>
        private ValidationRule RangeValidate(IControlHabanero ctrl)
		{
            List<ValidationRule> vr = _ValidationRules[ctrl];
            ValidationRule returnRule = new ValidationRule();
            if (vr != null)
            {
                vr.ForEach(delegate(ValidationRule obj)
                {
                    if (this.IsDefaultRange(obj))
                    {
                        returnRule = obj;
                    }
                    else
                    {
                        obj.IsValid =ValidationRule.Compare(ctrl.Text, obj.MinimumValue,ValidationCompareOperator.GreaterThanEqual, obj);

                        if (obj.IsValid)
                            obj.IsValid = ValidationRule.Compare(ctrl.Text, obj.MaximumValue,ValidationCompareOperator.LessThanEqual, obj);
                        returnRule = obj;
                    }
                });
                
            }
            return returnRule;
		}

		/// <summary>
		/// Check if validation rule range is default.
		/// </summary>
		/// <param name="vr"></param>
		/// <returns></returns>
		private bool IsDefaultRange(ValidationRule vr)
		{
			return (this._DefaultValidationRule.MinimumValue.Equals(vr.MinimumValue)
				&& this._DefaultValidationRule.MaximumValue.Equals(vr.MaximumValue));
			
		}

		/// <summary>
		/// Perform Regular Expression Validation on a specific control.
		/// </summary>
        private ValidationRule RegularExpressionValidate(IControlHabanero ctrl)
		{
            List<ValidationRule> vr = _ValidationRules[ctrl];
            ValidationRule returnRule = new ValidationRule();

            if (vr != null)
            {
                vr.ForEach(delegate(ValidationRule obj)
                {
                    try
                    {
                        if (this._DefaultValidationRule.RegExPattern.Equals(obj.RegExPattern)) returnRule= obj;

                        obj.IsValid = ValidationUtil.ValidateRegEx(ctrl.Text, obj.RegExPattern);
                        returnRule = obj;
                    }
                    catch (Exception ex)
                    {
                        obj.ResultErrorMessage = "RegEx Validation Exception: " + ex.Message + Environment.NewLine;
                        obj.IsValid = false;
                    }

                });
                
            }
            return returnRule;
		}

		/// <summary>
		/// Perform RequiredField Validation on a specific control.
		/// </summary>
        private ValidationRule RequiredFieldValidate(IControlHabanero ctrl)
		{
            ValidationRule returnRule = new ValidationRule();
            List<ValidationRule> vr = _ValidationRules[ctrl];
            vr.ForEach(delegate(ValidationRule obj)
            {
                if (obj != null && obj.IsRequired)
                {
                    obj.IsValid = obj.IsValid && !ValidationRule.Compare(ctrl.Text, obj.InitialValue, ValidationCompareOperator.Equal, obj);
                }
                returnRule = obj;
            });


            return returnRule;
		}
		#endregion

		#region "Properties"

		/// <summary>
		/// Set validation rule.
		/// </summary>
        public void SetValidationRule(IControlHabanero inputComponent, ValidationRule vr)
		{
			if (inputComponent != null)
			{
				// only throw error in DesignMode
				
					if (!this.CanExtend(inputComponent))
						throw new InvalidOperationException(inputComponent.GetType() 
							+ " is not supported by the validation provider.");

					if (!this.IsDefaultRange(vr) 
						&& ValidationRule.Compare(vr.MinimumValue, vr.MaximumValue, ValidationCompareOperator.GreaterThanEqual, vr))
						throw new ArgumentException("MinimumValue must not be greater than or equal to MaximumValue.");
				

				//ValidationRule vrOld = this._ValidationRules[inputComponent] as ValidationRule;

				// if new rule is valid and in not DesignMode, clone rule
				if ((vr != null) )
				{
					vr = vr.Clone() as ValidationRule;
				}
 			    if(!this._ValidationRules.ContainsKey(inputComponent))
 			    {
 			        List<ValidationRule> vrList = new List<ValidationRule>();
                    vrList.Add(vr);
                    this._ValidationRules.Add(inputComponent,vrList);
 			    }
				else if ((vr != null) && !this._ValidationRules[inputComponent].Contains(vr))
				{
                    
					this._ValidationRules[inputComponent].Add(vr);
				}

			}
		}

		/// <summary>
		/// Gets validation rules for a control.
		/// </summary>
		[DefaultValue(null), Category("Data")]
        public List<ValidationRule> GetValidationRules(IControlHabanero inputComponent)
		{
            return this._ValidationRules[inputComponent];
		}

		#endregion

		#region "ErrorProvider properties delegation"
		/// <summary>
		/// Icon display when validation failed.
		/// </summary>
        //[Category("Appearance"), Description("Icon display when validation failed."), Localizable(true)]
        //public Icon Icon
        //{
        //    get { return this._ErrorProvider.Icon; }
        //    set { this._ErrorProvider.Icon = value; }
        //}

		/// <summary>
		/// BlinkRate of ErrorIcon.
		/// </summary>
		[RefreshProperties(RefreshProperties.Repaint), Description("BlinkRate of ErrorIcon."), Category("Behavior"), DefaultValue(250)]
		public int BlinkRate
		{
			get { return this._ErrorProvider.BlinkRate;}
			set { this._ErrorProvider.BlinkRate = value; }
		}

		/// <summary>
		/// Get or set Blink Behavior.
		/// </summary>
        public ErrorBlinkStyleHabanero BlinkStyle
		{
			get { return this._ErrorProvider.BlinkStyleHabanero; }
			set { this._ErrorProvider.BlinkStyleHabanero = value; }
		}
 
		/// <summary>
		/// Get Error Icon alignment.
		/// </summary>
		/// <param name="control"></param>
		/// <returns></returns>
        public ErrorIconAlignmentHabanero GetIconAlignment(IControlHabanero control)
		{
			return this._ErrorProvider.GetIconAlignment(control);
		}
 
		/// <summary>
		/// Get Error Icon padding.
		/// </summary>
		/// <param name="control"></param>
		/// <returns></returns>
        public int GetIconPadding(IControlHabanero control)
		{
			return this._ErrorProvider.GetIconPadding(control);
		}
 
		/// <summary>
		/// Set Error Icon alignment.
		/// </summary>
		/// <param name="control"></param>
		/// <param name="value"></param>
        public void SetIconAlignment(IControlHabanero control, ErrorIconAlignmentHabanero value)
		{
			this._ErrorProvider.SetIconAlignment(control, value);
		}
 
		/// <summary>
		/// Set Error Icon padding.
		/// </summary>
		/// <param name="control"></param>
		/// <param name="padding"></param>
        public void SetIconPadding(IControlHabanero control, int padding)
		{
			this._ErrorProvider.SetIconPadding(control, padding);
		}
		#endregion

		#region IExtenderProvider Members

		/// <summary>
		/// Determine if ValidationProvider supports a component.
		/// </summary>
		/// <param name="extendee"></param>
		/// <returns></returns>
        public bool CanExtend(IControlHabanero extendee)
		{
            if ((extendee is ITextBox) || (extendee is IComboBox)) return true;

			return false;
		}

		#endregion

        /// <summary>
        /// Determine if ValidationProvider supports the given component
        /// </summary>
        /// <param name="extendee"></param>
        /// <returns></returns>
	    public bool CanExtend(object extendee)
	    {
	        return CanExtend(extendee as IControlHabanero);
	    }
	}
}
