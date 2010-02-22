// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// Links two properties so that you can update one on the update of another.  This is only done if the destination
    /// property value equals the source property value (before the update).
    /// 
    /// You can supply a transform that does some operation on the source value (such as converting to lower case, etc).
    /// 
    /// Please note_ that this should not be used extensively in a class of which there are going to be a large number 
    /// of instantiated objects because of the large number of objects and event handlers that will be created
    /// </summary>
    public class PropertyLink<TInput, TOutput>
    {
        private readonly IBusinessObject _owningBO;
        private readonly string _sourcePropName;
        private readonly string _destPropName;
        private readonly Converter<TInput, TOutput> _transform;
        private TInput _previousSourceValue;
        private readonly IBOProp _sourceProp;
        private readonly IBOProp _destinationProp;

        /// <summary>
        /// Constructor for building the link between two properties.
        /// </summary>
        /// <param name="owningBO">The object that owns the properties being linked</param>
        /// <param name="sourcePropName">The name of the source property.  This property will be watched for updates, triggering the updating of the destination property</param>
        /// <param name="destPropName">The name of the destination property.  This will be set to the value of the source (after transformation)</param>
        /// <param name="transform">The transform to apply. For no transform (ie a straight copy of the source property's value) simply return the input value</param>
        public PropertyLink(IBusinessObject owningBO, string sourcePropName, string destPropName,
                            Converter<TInput, TOutput> transform)
        {
            _owningBO = owningBO;
            _sourcePropName = sourcePropName;
            _sourceProp = _owningBO.Props[_sourcePropName];
            _destPropName = destPropName;
            _destinationProp = _owningBO.Props[_destPropName];
            _transform = transform;
            _previousSourceValue = GetSourcePropValue();
            Enable();
        }

        private void SourcePropUpdatedHandler(object sender, BOPropEventArgs e)
        {
            TInput sourcePropValue;
            try
            {
                sourcePropValue = GetSourcePropValue();
            }
            catch (InvalidCastException ex)
            {
                string message = "An error occured in the Updating a property via the property Link " + ex.Message;
                throw new HabaneroDeveloperException(message, message, ex);
            }
            object destPropValue = _destinationProp.Value;
            object transformedValue = _transform(_previousSourceValue);
            if (destPropValue == null || destPropValue.Equals(transformedValue))
            {
                _owningBO.SetPropertyValue(_destPropName, _transform(sourcePropValue));
            }
            _previousSourceValue = sourcePropValue;
        }

        private TInput GetSourcePropValue()
        {
            return (TInput) _sourceProp.Value;
        }

        /// <summary>
        /// Disables the property link.  It can be enabled again with <see cref="Enable"/>, but if the source property
        /// is changed while the link is disabled the destination property will not be updated once the link is enabled
        /// again.
        /// </summary>
        public void Disable()
        {
            if (_sourceProp == null) return;
            _sourceProp.Updated -= SourcePropUpdatedHandler;
        }

        /// <summary>
        /// Enables the property link.  This is the default state after construction.
        /// </summary>
        public void Enable()
        {
            _previousSourceValue = GetSourcePropValue();
            _sourceProp.Updated += SourcePropUpdatedHandler;
        }

        /// <summary>
        /// Destructor - disables the property link to ensure no references are kept.
        /// </summary>
        ~PropertyLink()
        {
            Disable();
        }
    }
}