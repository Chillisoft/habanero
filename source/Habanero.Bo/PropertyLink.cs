using System;
using Habanero.Base;

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

        /// <summary>
        /// Constructor for building the link between two properties.
        /// </summary>
        /// <param name="owningBO">The object that owns the properties being linked</param>
        /// <param name="sourcePropName">The name of the source property.  This property will be watched for updates, triggering the updating of the destination property</param>
        /// <param name="destPropName">The name of the destination property.  This will be set to the value of the source (after transformation)</param>
        /// <param name="transform">The transform to apply. For no transform (ie a straight copy of the source property's value) simply return the input value</param>
        public PropertyLink(IBusinessObject owningBO, string sourcePropName, string destPropName, Converter<TInput, TOutput> transform)
        {
            _owningBO = owningBO;
            _sourcePropName = sourcePropName;
            _destPropName = destPropName;
            _transform = transform;
            _previousSourceValue = GetSourcePropValue();
           Enable();
        }

        private void SourcePropUpdatedHandler(object sender, BOPropEventArgs e)
        {
            TInput sourcePropValue = GetSourcePropValue() ;
            object destPropValue = _owningBO.Props[_destPropName].Value;
            object transformedValue = _transform(_previousSourceValue);
            if (destPropValue == null || destPropValue.Equals(transformedValue))
            {
                _owningBO.SetPropertyValue(_destPropName, _transform(sourcePropValue));
            }
            _previousSourceValue = sourcePropValue;
        }

        private TInput GetSourcePropValue()
        {
            return (TInput) _owningBO.GetPropertyValue(_sourcePropName);
        }

        /// <summary>
        /// Disables the property link.  It can be enabled again with <see cref="Enable"/>, but if the source property
        /// is changed while the link is disabled the destination property will not be updated once the link is enabled
        /// again.
        /// </summary>
        public void Disable()
        {
            _owningBO.Props[_sourcePropName].Updated -= SourcePropUpdatedHandler;
        }

        /// <summary>
        /// Enables the property link.  This is the default state after construction.
        /// </summary>
        public void Enable()
        {
            _previousSourceValue = GetSourcePropValue();
            _owningBO.Props[_sourcePropName].Updated += SourcePropUpdatedHandler;
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
