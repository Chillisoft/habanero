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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Base.Logging;

namespace Habanero.Testability
{
    /// <summary>
    /// This is a registry of the ValidValueGenerators Registered for a specific PropertyDefinition <see cref="ISingleValueDef"/>
    /// of a particular Property for a specified Business Object type.
    /// This was creates so as to allow the Developer to override the Generic <see cref="ValidValueGenerator"/>.<br/>
    /// E.g. if the Business object Property has specific rules or things that must be set to ensure that it is a valid 
    /// saveable business object. Then you can register a specialised Valid Value Generator that will generate the appropriate
    ///  value for that Business Object Property.<br/>
    /// This is also used behind the scenes when you want to generate a business object with a specified non compulsory property
    /// in a particular case. Then a Valid Value Generator is registered for that <see cref="ISingleValueDef"/> and the non compulsory prop
    /// will have a value set. For more details see <see cref="BOTestFactory"/>
    /// </summary>
    public class BOPropValueGeneratorRegistry
    {
        private readonly Dictionary<string, ValidValueGeneratorInfo> _validValueGenTypesPerPropDef =
            new Dictionary<string, ValidValueGeneratorInfo>();

        private static BOPropValueGeneratorRegistry _boValueGeneratorRegistry;
        private readonly object _lockProp = new object();
        private readonly IHabaneroLogger _logger = GlobalRegistry.LoggerFactory.GetLogger("BOPropValueGeneratorRegistry");

        private static string GetClassType(ISingleValueDef propDef)
        {
            return propDef.ClassDef == null ? "" : propDef.ClassDef.ClassNameFull;
        }

        private void ClearPreviousInstances(ISingleValueDef propDef)
        {
            var keyName = GetKeyName(propDef);

            if (this._validValueGenTypesPerPropDef.ContainsKey(keyName))
            {
                this._validValueGenTypesPerPropDef.Remove(keyName);
            }
        }

        private static string GetKeyName(ISingleValueDef propDef)
        {
            return GetClassType(propDef) + propDef.PropertyName;
        }

        /// <summary>
        /// Register a Valid Value Generator to be used for generating values for a specified PropDef.
        /// </summary>
        /// <param name="propDef">The property definition for which this generator type is being assigned</param>
        /// <param name="validValuGenType">The type of generator which will be instantiated when a valid value is needed</param>
        /// <param name="parameter">An additional parameter to pass to the constructor of the generator</param>
        public virtual void Register(ISingleValueDef propDef, Type validValuGenType, object parameter = null)
        {
            _logger.Log("Register valid value gen for PropDef : " + propDef.PropertyName, LogCategory.Debug);
            string boTypeName = GetClassType(propDef);
            ValidateGeneratorType(validValuGenType, boTypeName);
            lock (_lockProp)
            {
                this.ClearPreviousInstances(propDef);
                _validValueGenTypesPerPropDef.Add(GetKeyName(propDef), new ValidValueGeneratorInfo(validValuGenType, parameter));
            }
        }

        /// <summary>
        /// Resolves the registered <see cref="ValidValueGenerator"/> for the PropDef if one is registered.
        /// Else tries to find a <see cref="ValidValueGenerator"/> for the specified PropDefs Property Type 
        /// using the <see cref="ValidValueGeneratorRegistry"/>
        /// </summary>
        /// <returns></returns>
        public virtual ValidValueGenerator Resolve(ISingleValueDef propDef)
        {
            if (propDef == null)
            {
                throw new ArgumentNullException("propDef");
            }
            lock (_lockProp)
            {
                var keyName = GetKeyName(propDef);
                if (_validValueGenTypesPerPropDef.ContainsKey(keyName))
                {
                    var validValueGen = this._validValueGenTypesPerPropDef[keyName];
                    var validValueGenType = validValueGen.GeneratorType;
                    var parameter = validValueGen.Parameter;
                    if (parameter != null)
                    {
                        if (!GeneratorHasConstructorWithExtraParameter(validValueGenType))
                        {
                            throw new ArgumentException(
                                "An extra parameter was provided for a valid value generator type (" +
                                validValueGenType.Name +
                                "), but no suitable constructor was found with a second parameter.");
                        }
                        return (ValidValueGenerator)Activator.CreateInstance(validValueGenType, propDef, parameter);
                    }
                    return (ValidValueGenerator)Activator.CreateInstance(validValueGenType, propDef);
                }

                return ValidValueGeneratorRegistry.Instance.Resolve(propDef);
            }
        }


        private static bool GeneratorHasConstructorWithExtraParameter(Type validValueGenType)
        {
            var constructorInfos = validValueGenType.GetConstructors();
            foreach (var constructorInfo in constructorInfos)
            {
                var parameterInfos = constructorInfo.GetParameters();
                if (parameterInfos.Length == 2 && parameterInfos[0].ParameterType == typeof(ISingleValueDef))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns True if a value Gen is registered with the singleValueDef <paramref name="propDef"/>
        /// </summary>
        /// <param name="propDef"></param>
        /// <returns></returns>
        public bool IsRegistered(ISingleValueDef propDef)
        {
            lock (_lockProp)
            {
                var keyName = GetKeyName(propDef);
                return this._validValueGenTypesPerPropDef.ContainsKey(keyName);
            }
        }

        private static void ValidateGeneratorType(Type factoryType, string typeName)
        {
            if (factoryType == null)
            {
                throw new HabaneroApplicationException(
                    string.Format(
                        "A ValidValueGenerator is being Registered for '{0}' but the ValidValueGenerator is Null",
                        typeName));
            }
            if (!typeof(ValidValueGenerator).IsAssignableFrom(factoryType))
            {
                throw new HabaneroApplicationException(
                    string.Format(
                        "A ValidValueGenerator is being Registered for '{0}' but the ValidValueGenerator is not of Type ValidValueGenerator",
                        typeName));
            }
        }

        /// <summary>
        /// Returns the Singleton Registry Instance
        /// </summary>
        public static BOPropValueGeneratorRegistry Instance
        {
            get
            {
                if (_boValueGeneratorRegistry == null)
                    return (_boValueGeneratorRegistry = new BOPropValueGeneratorRegistry());
                return _boValueGeneratorRegistry;
            }
            set { _boValueGeneratorRegistry = value; }
        }


        ///<summary>
        /// Clears all ValidValue Generators registered.
        ///</summary>
        public void ClearAll()
        {
            lock (_lockProp)
            {
                _validValueGenTypesPerPropDef.Clear();
            }
        }

        private class ValidValueGeneratorInfo
        {
            public ValidValueGeneratorInfo(Type generatorType, object parameter = null)
            {
                GeneratorType = generatorType;
                Parameter = parameter;
            }

            public Type GeneratorType { get; private set; }
            public object Parameter { get; private set; }
        }
    }
}