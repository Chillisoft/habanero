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
using Habanero.Util;

namespace Habanero.Testability
{
    /// <summary>
    /// This is a registry of the BOTestFactories for various Business Object types.
    /// This was creates so as to allow the Developer to override the Generic <see cref="BOTestFactory{TBO}"/>.<br/>
    /// E.g. if the Business object has specific rules or things that must be set to ensure that it is a valid 
    /// saveable business object. This specialised <see cref="BOTestFactory{TBO}"/> must then be 
    /// registered with this Registery.<br/>
    /// When any other TestBusiness Object has a compulsory relationship involving the Business Object with the Specialised
    /// BOTestFactory the specialised factory will be used instead of the generalised Test factory.
    /// This allows a situation such as Creating a valid Asset wich has a compulsory relationship to AssetType.
    /// For a valid Asset to be created a valid Asset Type is required. If the Asset type has a specialised
    /// BOTestFactoryAssetType then the asset must use this specialised factory else it will not be constructed in a valid.
    /// manner.
    /// </summary>
    public class BOTestFactoryRegistry
    {
        private readonly Dictionary<Type, Type> _boTestFactoryTypes = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, BOTestFactory> _boTestFactoryInstances = new Dictionary<Type, BOTestFactory>();
        private static BOTestFactoryRegistry _boTestFactoryRegistry;
        private readonly object _lockProp = new object(); //Used for locking the Resolve.

        private static readonly object _staticLockProp = new object(); //Used for Locking the SingleTon
        private IHabaneroLogger _logger = GlobalRegistry.LoggerFactory.GetLogger("BOTestFactoryRegistry");

        private void ClearPreviousInstances(Type boType)
        {
            if (this._boTestFactoryTypes.ContainsKey(boType))
            {
                this._boTestFactoryTypes.Remove(boType);
            }

            if (this._boTestFactoryInstances.ContainsKey(boType))
            {
                this._boTestFactoryInstances.Remove(boType);
            }
        }

        /// <summary>
        /// Registeres a specific type <typeparamref name="TBOTestFactory"/> of <see cref="BOTestFactory"/> for the specified 
        /// <see cref="IBusinessObject"/> <typeparamref name="TBO"/>
        /// </summary>
        /// <typeparam name="TBO"></typeparam>
        /// <typeparam name="TBOTestFactory"></typeparam>
        public virtual void Register<TBO, TBOTestFactory>() where TBO : class, IBusinessObject
            where TBOTestFactory : BOTestFactory<TBO>
        {
            this.Register(typeof(TBO), typeof(TBOTestFactory));
        }

        /// <summary>
        /// Registers an instance of <see cref="BOTestFactory"/>
        /// </summary>
        /// <typeparam name="TBO"></typeparam>
        /// <param name="boTestFactory"></param>
        public virtual void Register<TBO>(BOTestFactory boTestFactory)
        {
            lock (_lockProp)
            {
                var boType = typeof(TBO);
                this.ClearPreviousInstances(boType);
                this._boTestFactoryInstances.Add(boType, boTestFactory);
            }
        }

        /// <summary>
        /// Registeres a specific type of <see cref="BOTestFactory"/>
        /// </summary>
        /// <typeparam name="TBO"></typeparam>
        ///
        public virtual void Register<TBO>(Type type) where TBO : class, IBusinessObject
        {
            this.Register(typeof(TBO), type);
        }

        private void Register(Type boType, Type factoryType)
        {
            lock (_lockProp)
            {
                ValidateFactoryType(boType, factoryType);
                this.ClearPreviousInstances(boType);
                this._boTestFactoryTypes.Add(boType, factoryType);
            }
        }

        /// <summary>
        /// Resolves the registered <see cref="BOTestFactory"/> if one is registered else resolves the 
        /// Generid <see cref="BOTestFactory{TBO}"/>
        /// </summary>
        /// <typeparam name="TBO"></typeparam>
        /// <returns></returns>
        public virtual BOTestFactory<TBO> Resolve<TBO>() where TBO : class, IBusinessObject
        {
            var typeOfBO = typeof(TBO);
            return (BOTestFactory<TBO>)this.Resolve(typeOfBO);
        }

        /// <summary>
        /// Resolves the registered <see cref="BOTestFactory"/> if one is registered.
        /// Else tries to find a Sub Class of the Generic <see cref="BOTestFactory{TBO}"/> 
        ///   in the App Domain and returns an instance of it.
        /// else constructs the Generic <see cref="BOTestFactory{TBO}"/>
        /// </summary>
        /// <returns></returns>
        public virtual BOTestFactory Resolve(Type typeOfBO)
        {
            Type boTestFactoryType;
            if (typeOfBO == null)
            {
                throw new ArgumentNullException("typeOfBO");
            }
            _logger.Log("Resolve TestFactory For '" + typeOfBO.Name + "'", LogCategory.Debug);
            lock (_lockProp)
            {
                if (this._boTestFactoryInstances.ContainsKey(typeOfBO))
                {
                    var boTestFactoryInstance = this._boTestFactoryInstances[typeOfBO];
                    if (boTestFactoryInstance != null)
                    {
                        return boTestFactoryInstance;
                    }
                }
            }
            lock (_lockProp)
            {
                if (this._boTestFactoryTypes.ContainsKey(typeOfBO))
                {
                    boTestFactoryType = this._boTestFactoryTypes[typeOfBO];
                    return (BOTestFactory)Activator.CreateInstance(boTestFactoryType);
                }
            }
            boTestFactoryType = typeof(BOTestFactory<>).MakeGenericType(new[] { typeOfBO });
            var factoryType = boTestFactoryType;
            //Excludes Types created via RhinoMocks.
            var typeSource = new AppDomainTypeSource(type => !type.Name.Contains("Proxy"));
            Type firstSubType = null;
            try
            {
                firstSubType = typeSource.GetTypes()
                    .Where(factoryType.IsAssignableFrom)
                    .FirstOrDefault();
            }
            catch (Exception)
            {
                //Ignore any error that occured while trying to create the factory based on it type
                // these will be fixed below by creating a factory of this specific instance
            }
            if (firstSubType != null)
            {
                boTestFactoryType = firstSubType;
            }
            return (BOTestFactory)Activator.CreateInstance(boTestFactoryType);
        }

        private static void ValidateFactoryType(Type boType, Type factoryType)
        {
            if (factoryType == null)
            {
                throw new HabaneroApplicationException(
                    string.Format("A BOTestFactory is being Registered for '{0}' but the BOTestFactory is Null",
                                  boType.Name));
            }
            if (!typeof(BOTestFactory).IsAssignableFrom(factoryType))
            {
                throw new HabaneroApplicationException(
                    string.Format(
                        "A BOTestFactory is being Registered for '{0}' but the BOTestFactory is not of Type BOTestFactory",
                        boType.Name));
            }
        }

        /// <summary>
        /// Returns the Singleton Registry Instance
        /// </summary>
        public static BOTestFactoryRegistry Instance
        {
            get
            {
                lock (_staticLockProp)
                {
                    if (_boTestFactoryRegistry == null) return (_boTestFactoryRegistry = new BOTestFactoryRegistry());
                    return _boTestFactoryRegistry;
                }
            }
            set { _boTestFactoryRegistry = value; }
        }

        /// <summary>
        /// Clears all registered test factories
        /// </summary>
        public virtual void ClearAll()
        {
            lock (_lockProp)
            {
                this._boTestFactoryInstances.Clear();
                this._boTestFactoryTypes.Clear();
            }
        }
    }

    /// <summary>
    /// Gets the Types for the Currently Loaded App Domain
    /// </summary>
    internal class AppDomainTypeSource
    {
        private static IHabaneroLogger _logger = GlobalRegistry.LoggerFactory.GetLogger("AppDomainTypeSource");

        public AppDomainTypeSource(Func<Type, bool> where)
        {
            this.Where = where;
        }

        public IEnumerable<Type> GetTypes()
        {
            return ((this.Where == null) ? TypesImplementing() : TypesImplementing().Where(this.Where));
        }

        private static IEnumerable<Type> TypesImplementing()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(GetAssemblyTypes)
                .Where(type1 => !type1.IsInterface && !type1.IsAbstract);
        }

        private static IEnumerable<Type> GetAssemblyTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                var message = e.Message;
                var loaderExceptions = e.LoaderExceptions;
                foreach (var exception in loaderExceptions)
                {
                    message = StringUtilities.AppendMessage(message, exception.Message, Environment.NewLine);
                }
                _logger.Log(message, LogCategory.Exception);
                throw new HabaneroApplicationException(message, e);
            }
        }

        private Func<Type, bool> Where { get; set; }
    }
}