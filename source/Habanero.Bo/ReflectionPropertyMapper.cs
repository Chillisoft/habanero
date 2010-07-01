using System;
using System.Reflection;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Util;

namespace Habanero.BO
{
    /// <summary>
    /// This is a mapper class that handles the mapping of a property name 
    /// to a specific property for a specified <see cref="IBusinessObject"/>.
    /// The property name can be specified as a path through single relationships on the <see cref="IBusinessObject"/>
    /// and its' relationship tree.
    /// <remarks>For Example:<br/>
    /// For the ContactPerson BusinessObject when the propertyName is "FirstName",
    ///  the returned <see cref="IBOProp"/> will be the "FirstName" property on ContactPerson.<br/>
    /// If the propertyName was "Organisation.Name" then the Organisation relationship on the contact person will 
    /// be traversed and monitored and return the corresponding "Name" <see cref="IBOProp"/> for the ContactPerson's current Organisation.</remarks>
    /// </summary>
    public class ReflectionPropertyMapper : IBOPropertyMapper
    {
        private IBusinessObject _businessObject;
        protected PropertyInfo _propertyInfo;
        protected string _invalidMessage = "";

        ///<summary>
        /// Creates a BOPropertyMapper for the specified property name/path.
        ///</summary>
        ///<param name="propertyName">The name of the property to be mapped (this could also be in the form of a path through single relationships on the BO).</param>
        ///<exception cref="ArgumentNullException">This is thrown if <paramref name="propertyName"/> is null or empty.</exception>
        public ReflectionPropertyMapper(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("propertyName");
            PropertyName = propertyName.Trim('-');
        }
/*
        /// <summary>
        /// This event is fired when the current <see cref="Property"/> object changes, either through 
        /// the current <see cref="BusinessObject"/> being changed, or one of the related BusinessObjects in the 
        /// mapped Property path has been changed.
        /// </summary>
        public event EventHandler PropertyChanged;*/

        ///<summary>
        /// The name of the property to be mapped. 
        ///</summary>
        public string PropertyName { get; private set; }

        ///<summary>
        /// The BusinessObject for which the Property is being mapped.
        /// Once this property has been set, the <see cref="ReflectionPropertyMapper"/>'s PropertyInfo will be set.
        ///</summary>
        ///<exception cref="InvalidPropertyException">This is thrown if the specified property does not exist on the <see cref="IBusinessObject"/> being set or if one of the Relationships within the Property Path is not a single relationship.</exception>
        public IBusinessObject BusinessObject
        {
            get { return _businessObject; }
            set
            {
                IBusinessObject businessObject = value;
                if (businessObject != null)
                {
                    _propertyInfo = ReflectionUtilities.GetPropertyInfo(businessObject.GetType(), this.PropertyName);
                    if (_propertyInfo == null)
                    {
                        ThrowPropertyNotFoundException(businessObject);
                    }
                }
                _invalidMessage = "";
                _businessObject = businessObject;
            }
        }

        private void ThrowPropertyNotFoundException(IBusinessObject businessObject)
        {
            IClassDef classDef = businessObject.ClassDef;
            throw new InvalidPropertyException("The property '" + PropertyName + "' on '"
                                               + classDef.ClassName + "' cannot be found. Please contact your system administrator.");
        }
        /// <summary>
        /// Sets the BOProp that this mapper is mapped to the associated propValue
        /// </summary>
        /// <param name="propValue"></param>
        public void SetPropertyValue(object propValue)
        {
            CheckBusinessObjectSet("Set Property Value");
            try
            {
                ReflectionUtilities.SetPropValue(this.BusinessObject, this._propertyInfo, propValue);
                _invalidMessage = "";
            }
            catch (Exception e)
            {
                _invalidMessage = e.Message;
            }
        }
        private void CheckBusinessObjectSet(string methodName)
        {
            if (this.BusinessObject == null)
            {
                throw new HabaneroApplicationException(string.Format("Tried to {1} the ReflectionPropertyMapper for Property '{0}' when the BusinessObject is not set ", this.PropertyName, methodName));
            }
        }
        /// <summary>
        /// Return the Property Value for the Property being mapped.
        /// </summary>
        /// <returns></returns>
        public object GetPropertyValue()
        {
            CheckBusinessObjectSet("GetPropertyValue");
            return ReflectionUtilities.GetPropertyValue(this.BusinessObject, this.PropertyName);
        }
        /// <summary>
        /// If the Property is invalid then returns the Invalid reason.
        /// </summary>
        public string InvalidReason
        {
            get
            {
                var boProp = this._propertyInfo;
                if (boProp == null)
                {
                    return string.Format("The Property '{0}' is not available"
                                         , PropertyName);
                }
                return _invalidMessage;
            }
        }
    }
}