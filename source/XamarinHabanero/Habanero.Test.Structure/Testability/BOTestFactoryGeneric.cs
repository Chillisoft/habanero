#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System.Collections.Generic;
using System.Linq;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Util;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Habanero.Testability
{
    /// <summary>
    /// The BOTestFactory is a factory used to construct a Business Object for testing.
    /// The Constructed Business object can be constructed a a valid (i.e. saveable Business object)
    /// <see cref="CreateValidBusinessObject"/> a Default Busienss object <see cref="CreateDefaultBusinessObject"/>.<br/>
    /// A Valid Property Value can also be generated for any particular Prop using one of the overloads of <see cref="GetValidPropValue{TReturn}(T, System.Linq.Expressions.Expression{System.Func{T,TReturn}})"/>,
    /// <see cref="GetValidPropValue(string)"/> or any of the methods from the base type <see cref="BOTestFactory"/>
    /// A Valid Relationship can be generated for any particular relationship using <see cref="GetValidRelationshipValue{TReturn}"/>.<br/>
    /// of <see cref="GetValidRelationshipValue(string)"/><br/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BOTestFactory<T> : BOTestFactory where T : class, IBusinessObject
    {
        //For testing I consider 3 to be many i.e. there may be special cases where the 
        // test can handle one or even two objects but most algorithms that work on 3
        // items will work on n items.
        private const int MANY = 3;

        /// <summary>
        /// The default constructor for the Factory the Busienss object can be set later
        /// by using <see cref="BusinessObject"/> or <see cref="CreateBusinessObject"/>.
        /// </summary>
        public BOTestFactory() : base(typeof(T))
        {
        }

        /// <summary>
        /// Constructs with a business objec.t
        /// </summary>
        /// <param name="bo"></param>
        public BOTestFactory(T bo) : base(typeof(T))
        {
            this.BusinessObject = bo;
        }

        private new T CreateBusinessObject()
        {
            this.BusinessObject = base.BOFactory.CreateBusinessObject<T>();
            return this.BusinessObject;
        }
        /// <summary>
        /// Creates a business object with only its default values set.
        /// </summary>
        /// <returns></returns>
        public new virtual T CreateDefaultBusinessObject()
        {
            return this.CreateBusinessObject();
        }

        /// <summary>
        /// Creates a valid value for the property identified by the lambda expression <paramref name="propExpression"/>.
        /// </summary>
        /// <param name="propExpression"></param>
        /// <returns></returns>
        public TReturn GetValidPropValue<TReturn>(Expression<Func<T, TReturn>> propExpression)
        {
            return this.GetValidPropValue(this.BusinessObject, propExpression);
        }

        /// <summary>
        /// Creates a valid property for the property identified by <paramref name="propName"/>
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public override object GetValidPropValue(string propName)
        {
            if (this.BusinessObject == null) return base.GetValidPropValue(typeof(T), propName);
            return base.GetValidPropValue(this.BusinessObject, propName);
        }

        private static PropertyInfo GetPropertyInfo<TModel, TReturn>(Expression<Func<TModel, TReturn>> expression)
        {
            return ReflectionUtilities.GetPropertyInfo(expression);
        }

        private static string GetPropertyName<TReturn>(Expression<Func<T, TReturn>> propExpression)
        {
            return GetPropertyInfo(propExpression).Name;
        }

        /// <summary>
        /// Returns a valid value for the busienss object <paramref name="bo"/>'s property
        /// identified by <pararef name="propExpression"/>.
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="propExpression"></param>
        /// <returns></returns>
        public TReturn GetValidPropValue<TReturn>(T bo, Expression<Func<T, TReturn>> propExpression)
        {
            string propName = GetPropertyName(propExpression);
            object value = (bo == null) ? this.GetValidPropValue(propName) : this.GetValidPropValue(bo, propName);
            try
            {
                return (TReturn)value;
            }
            catch (InvalidCastException ex)
            {
                string errorMessage = string.Format("The value '{0}' could not be cast to type '{1}'", value,
                                                    typeof(TReturn));
                throw new InvalidCastException(errorMessage, ex);
            }
        }

        private object GetValidPropValue(T bo, string propName)
        {
            return base.GetValidPropValue(bo, propName);
        }

        /// <summary>
        /// Returns a valid relationship for the <see cref="BusinessObject"/>'s
        /// property identified by <paramref name="propExpression"/>
        /// </summary>
        /// <param name="propExpression"></param>
        /// <returns></returns>
        public TReturn GetValidRelationshipValue<TReturn>(Expression<Func<T, TReturn>> propExpression)
        {
            string relationshipName = GetPropertyName(propExpression);
            return (TReturn)this.GetValidRelationshipValue(relationshipName);
        }

        /// <summary>
        /// Returns a valid Relationship for the <see cref="BusinessObject"/>'s
        /// relationship identified by the <paramref name="relationshipName"/>
        /// </summary>
        /// <param name="relationshipName"></param>
        /// <returns></returns>
        public IBusinessObject GetValidRelationshipValue(string relationshipName)
        {
            return base.GetValidRelationshipValue(typeof(T), relationshipName);
        }

        /// <summary>
        /// Get and set the <see cref="IBusinessObject"/> object that this generic Factory is generic
        /// factory is generating values for.
        /// This property is set directly or via the constructor or via <see cref="CreateDefaultBusinessObject"/>
        /// or via <see cref="CreateValidBusinessObject"/>
        /// </summary>
        public T BusinessObject { get; set; }

        /// <summary>
        /// Sets the <paramref name="setPropValue"/> for the method Idenfied by the <paramref name="propertyExpression"/>.
        /// This ensures that when the <see cref="CreateValidBusinessObject"/> or <see cref="BOTestFactory.UpdateCompulsoryProperties"/>
        /// or <see cref="GetValidPropValue"/> and the <see cref="GetValidRelationshipValue"/>
        /// for this Property this value is always used
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="setPropValue"></param>
        public void SetValueFor<TReturn>(Expression<Func<T, TReturn>> propertyExpression, TReturn setPropValue)
        {
            string propertyName = GetPropertyName(propertyExpression);
            _defaultValueRegistry.Register(propertyName, setPropValue);
        }
        /// <summary>
        /// Sets the builder to ensure that when it generates a BO it
        /// will generated a value for the property identified by the <paramref name="propertyExpression"/>
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public BOTestFactory<T> SetValueFor<TReturn>(Expression<Func<T, TReturn>> propertyExpression)
        {
            var propertyName = GetPropertyName(propertyExpression);
            return SetValueFor(propertyName);
        }

        /// <summary>
        /// Sets the builder to ensure that when it generates a BO it
        /// will generated a value for the property identified by the <paramref name="propertyName"/>
        /// </summary>
        public BOTestFactory<T> SetValueFor(string propertyName)
        {
            var propDef = GetPropDef(typeof(T), propertyName, false);
            if (propDef != null)
            {
                var validValueGenerator = _validValueGenRegistry.Resolve(propDef);
                _validValueGenRegistry.Register(propDef, validValueGenerator.GetType());
            }
            else
            {
                //If this is not a relationshipdef then will get Error from here.
                var validRelatedValue = GetValidRelationshipValue(propertyName);
                _defaultValueRegistry.Register(propertyName, validRelatedValue);
            }
            return this;
        }

        private static IRelationshipDef GetRelationshipDef<TReturn>(Expression<Func<T, TReturn>> propertyExpression, bool raiseErrIfNotExists)
        {
            string propertyName = GetPropertyName(propertyExpression);
            return GetRelationshipDef(typeof(T), propertyName, raiseErrIfNotExists);
        }
        private static ISingleValueDef GetPropDef<TReturn>(Expression<Func<T, TReturn>> propertyExpression, bool raiseErrIfNotExists)
        {
            string propertyName = GetPropertyName(propertyExpression);
            return GetPropDef(typeof(T), propertyName, raiseErrIfNotExists);
        }

        private static ISingleValueDef GetSingleValueDef<TReturn>(Expression<Func<T, TReturn>> propertyExpression)
        {
            var singleValueDef = GetPropDef(propertyExpression, false);
            if (singleValueDef == null) singleValueDef = GetRelationshipDef(propertyExpression, true) as ISingleValueDef;
            return singleValueDef;
        }
        /// <summary>
        /// Set the Type of valid value generator that will be used for the generating values for 
        /// a property defined by the expression.
        /// This is used primarily when generating meaningful sample data for visual testing and demos but can also be used
        /// for generating valid data for business objects for testing.
        /// An example would be where the BusinessObject has a property that has 
        /// some custom property rule <see cref="IPropRule"/> that Testability can't use to determine a valid value
        /// or where the BusinessObject has a <see cref="IBusinessObjectRule"/> that testability can't use.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="validValueGeneratorType">The type of generator which will be instantiated when a valid value is needed</param>
        /// <param name="parameter">An additional parameter to pass to the constructor of the generator</param>
        public BOTestFactory<T> SetValidValueGenerator<TReturn>(Expression<Func<T, TReturn>> propertyExpression, Type validValueGeneratorType, object parameter = null)
        {
            //TODO brett 01 Nov 2010: should do some validation that the Type inherits from abstract class ValidValueGenerator
            //TODO brett 27 Nov 2010: Need to move this all to use the SingleValueDef
            var singleValueDef = GetSingleValueDef(propertyExpression);
            _validValueGenRegistry.Register(singleValueDef, validValueGeneratorType, parameter);
            return this;
        }


        /// <summary>
        /// set up the factory to create one child business object.
        /// </summary>
        /// <typeparam name="TBusinessObject"></typeparam>
        /// <param name="relationshipExpression"></param>
        /// <returns></returns>
        public BOTestFactory<T> WithOne<TBusinessObject>(
            Expression<Func<T, BusinessObjectCollection<TBusinessObject>>> relationshipExpression)
            where TBusinessObject : class, IBusinessObject, new()
        {
            return WithMany(relationshipExpression, 1);
        }
        /// <summary>
        /// set up the factory to create two children business objects.
        /// </summary>
        /// <typeparam name="TBusinessObject"></typeparam>
        /// <param name="relationshipExpression"></param>
        /// <returns></returns>
        public BOTestFactory<T> WithTwo<TBusinessObject>(
            Expression<Func<T, BusinessObjectCollection<TBusinessObject>>> relationshipExpression)
            where TBusinessObject : class, IBusinessObject, new()
        {
            return WithMany(relationshipExpression, 2);
        }
        /// <summary>
        /// Set up the factory to create many children business objects.
        /// </summary>
        /// <typeparam name="TBusinessObject"></typeparam>
        /// <param name="relationshipExpression"></param>
        /// <returns></returns>
        public BOTestFactory<T> WithMany<TBusinessObject>(
            Expression<Func<T, BusinessObjectCollection<TBusinessObject>>> relationshipExpression)
            where TBusinessObject : class, IBusinessObject, new()
        {
            return WithMany(relationshipExpression, MANY);
        }
        /// <summary>
        /// set up the factory to create many children business objects.
        /// </summary>
        /// <typeparam name="TBusinessObject"></typeparam>
        /// <param name="relationshipExpression"></param>
        /// <param name="expectedNoOfCreatedChildObjects"></param>
        /// <returns></returns>
        public BOTestFactory<T> WithMany<TBusinessObject>(
            Expression<Func<T, BusinessObjectCollection<TBusinessObject>>> relationshipExpression,
            int expectedNoOfCreatedChildObjects)
            where TBusinessObject : class, IBusinessObject, new()
        {
            var propertyName = GetPropertyName(relationshipExpression);
            IList<TBusinessObject> bos = new List<TBusinessObject>();
            var boTBusinessObjectFactory = BOTestFactoryRegistry.Instance.Resolve<TBusinessObject>();
            for (int i = 0; i < expectedNoOfCreatedChildObjects; i++)
            {
                TBusinessObject relatedBO = boTBusinessObjectFactory.CreateValidBusinessObject();
                bos.Add(relatedBO);
            }
            _defaultValueRegistry.Register(propertyName, bos);
            return this;
        }
        /// <summary>
        /// Sets a value generator for all the PropDefs that would not necessarily have had a value set
        /// e.g. the propdef is not compulsory and does not have a default value set using
        /// WithValue and does not have a valueGenerator set using WithValueFor
        /// </summary>
        /// <returns></returns>
        public BOTestFactory<T> WithValueForAllProps()
        {
            var type = typeof(T);
            ValidateClassDef(type);
            var classDef = ClassDef.ClassDefs[type];

            foreach (var propDef in classDef.PropDefcol)
            {
                if (!MustGeneratePropValue(propDef))
                {
                    this.SetValueFor(propDef.PropertyName);
                }
            }

            return this;
        }

        private bool MustGeneratePropValue(IPropDef propDef)
        {
            var hasDefaultValue = propDef.DefaultValue != null;
            return propDef.Compulsory
                   || _defaultValueRegistry.IsRegistered(propDef.PropertyName)
                   || _validValueGenRegistry.IsRegistered(propDef)
                   || hasDefaultValue;
        }
        /// <summary>
        /// Sets the Factory so that it will not generate all the single relationships
        /// even if the single relationship is compulsory.
        /// </summary>
        /// <returns></returns>
        public BOTestFactory<T> WithOutSingleRelationships()
        {
            SetCompulsorySingleRelationships = false;
            return this;
        }
        /// <summary>
        /// Creates a valid business object based on the <see cref="ISingleValueDef"/> rules
        /// and the settings for this builder e.g. <see cref="SetValueFor"/> and <see cref="WithValueForAllProps"/>.
        /// </summary>
        /// <returns></returns>
        public new virtual T CreateValidBusinessObject()
        {
            return (this.BusinessObject = (T)base.CreateValidBusinessObject());
        }
        /// <summary>
        /// Creates a Valid Business Object based on the <see cref="ISingleValueDef"/> rules
        /// and the settings for this builder e.g. SetValueFor and WithValueForAllProps.
        /// Note_ this method could fail if you have set up this builder with 
        /// invalid settings e.g. you have <see cref="SetValueFor"/> set the value to an invalid
        /// value or you can use <see cref="WithOutSingleRelationships"/> in these cases
        /// the builder will create an invalid business object that will cause the 
        /// <see cref="IBusinessObject.Save"/> method to fail.
        /// Note_: If you are using a NonValidatingTransactionCommitter then no validation is done
        /// while committing the transaction.
        /// </summary>
        /// <returns></returns>
        public new virtual T CreateSavedBusinessObject()
        {
            T bo = this.CreateValidBusinessObject();
            bo.Save();
            return bo;
        }
        /// <summary>
        /// Create many saved business objects <see cref="CreateSavedBusinessObject"/>.
        /// the standard defintion of Many is 3.
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> CreateManySavedBusinessObject()
        {
            return CreateManySavedBusinessObject(MANY);
        }
        /// <summary>
        /// Create many saved business objects <see cref="CreateSavedBusinessObject"/>.
        /// where the number is defined.
        /// </summary>
        /// <param name="noToCreate"></param>
        /// <returns></returns>
        public virtual IList<T> CreateManySavedBusinessObject(int noToCreate)
        {
            IList<T> col = new List<T>();
            for (int i = 0; i < noToCreate; i++)
            {
                T bo = this.CreateSavedBusinessObject();
                col.Add(bo);
            }
            return col;
        }
    }
}