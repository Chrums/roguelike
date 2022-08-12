using System;
using System.Collections.Generic;
using System.Linq;

namespace Fizz6.Core
{
    public static class TypeExt
    {
        private static readonly Dictionary<string, Type> TypeCache = new Dictionary<string, Type>();
        
        public static Type GetType(string typeName)
        {
            if (TypeCache.TryGetValue(typeName, out var type)) return type;
            
            type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(_ => _.Name == typeName || _.FullName == typeName);
            TypeCache[typeName] = type;

            return type;
        }

        private static readonly Dictionary<Type, IReadOnlyList<Type>> AssignableTypesCache = new Dictionary<Type, IReadOnlyList<Type>>();
        
        public static IReadOnlyList<Type> GetAssignableTypes(this Type type)
        {
            if (AssignableTypesCache.TryGetValue(type, out var types)) return types;
            
            types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(_ => type.IsAssignableFrom(_) && !_.IsAbstract && !_.IsInterface)
                .ToList();
            AssignableTypesCache[type] = types;

            return types;
        }
    }
}