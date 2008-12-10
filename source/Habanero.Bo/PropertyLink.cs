using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.BO
{

    /// <summary>
    /// Links two properties so that you can update one on the update of another.  This is only done if the destination
    /// property value equals the source property value (before the update).
    /// 
    /// You can supply a transform that does some operation on the source value (such as converting to lower case, etc).
    /// 
    /// Please note that this should not be used extensively in a class of which there are going to be a large number 
    /// of instantiated objects because of the large number of objects and event handlers that will be created
    /// </summary>
    public class PropertyLink
    {
        /// <summary>
        /// Takes as input a value, applies some sort of operation on this value and returns the transformed value.
        /// The simplest transform is simply to return the original value.
        /// </summary>
        /// <param name="inputValue">The source value</param>
        /// <returns>The transformed value</returns>
        public delegate object Transform(object inputValue);

        private readonly IBusinessObject _owningBO;
        private readonly string _sourcePropName;
        private readonly string _destPropName;
        private readonly Transform _transform;
        private object _previousSourceValue;

        /// <summary>
        /// Constructor for building the link between two properties.
        /// </summary>
        /// <param name="owningBO">The object that owns the properties being linked</param>
        /// <param name="sourcePropName">The source property.  This property will be watched for updates, triggering the updating of the destination property</param>
        /// <param name="destPropName">The destination property.  This will be set to the value of the source (after transformation)</param>
        /// <param name="transform">The <see cref="Transform"/> to apply</param>
        public PropertyLink(IBusinessObject owningBO, string sourcePropName, string destPropName, Transform transform)
        {
            _owningBO = owningBO;
            _sourcePropName = sourcePropName;
            _destPropName = destPropName;
            _transform = transform;
            _owningBO.Props[_sourcePropName].Updated += SourcePropUpdatedHandler;
        }

        private void SourcePropUpdatedHandler(object sender, BOPropEventArgs e)
        {
            object sourcePropValue = _owningBO.GetPropertyValue(_sourcePropName);
            IBOProp destProp = _owningBO.Props[_destPropName];
            object transformedValue = _transform(_previousSourceValue);
            if (destProp.Value == null || (destProp.Value is string && string.IsNullOrEmpty(destProp.PropertyValueString)) || destProp.Value.Equals(transformedValue))
            {
                _owningBO.SetPropertyValue(_destPropName, _transform(sourcePropValue));
            }
            _previousSourceValue = sourcePropValue;
        }
    }
}
