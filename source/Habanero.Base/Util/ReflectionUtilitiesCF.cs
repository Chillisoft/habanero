using System;

namespace Habanero.Base.Util
{
    public static class ReflectionUtilitiesCF
    {
        public static object GetInstanceWithConstructorParameters(Type type, params object[] pars)
        {

            Type[] paramTypes = new Type[pars.Length];

            for (int i = 0; i<pars.Length;i++)
            {
                paramTypes[i] = pars[i].GetType();
            }

            var c = type.GetConstructor(paramTypes); 
            if (c == null) return null; 
            
            return c.Invoke(pars);
        }
    }
}
