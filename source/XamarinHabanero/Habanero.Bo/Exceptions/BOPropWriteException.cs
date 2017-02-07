using System;
using System.Runtime.Serialization;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Exceptions
{
    /// <summary>
    /// Provides an exception to throw when a there is an issue writing to a property on 
    /// the businessobject due to the ReadWriteRule that has been set up for the property.
    /// </summary>
    [Serializable]
    public class BOPropWriteException : BusinessObjectException
    {
        private readonly IPropDef _propDef;

        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        /// <param name="propDef">The property definition for the property that had the ReadWriteRule which threw the error.</param>
        public BOPropWriteException(PropDef propDef): this(propDef, ConstructMessage(propDef))
        {
            _propDef = propDef;
        }

        private static string ConstructMessage(PropDef propDef)
        {
            if (propDef == null) return "";
            string displayName = String.IsNullOrEmpty(propDef.DisplayName) ? propDef.PropertyName : propDef.DisplayName;
            return String.Format("Error writing to property '{0}' because it is configured as a '{1}' property.",
                displayName, propDef.ReadWriteRule);
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="propDef">The property definition for the property that had the ReadWriteRule which threw the error.</param>
        /// <param name="message">The error message</param>
        public BOPropWriteException(IPropDef propDef, string message) : base(message)
        {
            _propDef = propDef;
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="propDef">The property definition for the property that had the ReadWriteRule which threw the error.</param>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public BOPropWriteException(PropDef propDef, string message, Exception inner) : base(message, inner)
        {
            _propDef = propDef;
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>

        protected BOPropWriteException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        
        ///<summary>
        /// The property definition for the property that had the ReadWriteRule which threw the error.
        ///</summary>
        public IPropDef PropDef
        {
            get { return _propDef; }
        }
    }
}