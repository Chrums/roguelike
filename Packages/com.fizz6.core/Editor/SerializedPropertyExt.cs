using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Fizz6.Core.Editor
{
    public static class SerializedPropertyExt
    {
        private const char SpaceDelimiter = ' ';
        private const char PathDelimiter = '.';
        private static readonly Regex ArrayRegex = new Regex(@"([^\.]*?)\.Array\.data\[(\d+)\]");
        
        public static string GetManagedReferenceFullTypeName(this SerializedProperty serializedProperty)
        {
            return serializedProperty.managedReferenceFullTypename
                .Split(SpaceDelimiter)
                .Last();
        }
        
        public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty serializedProperty)
        {
            var iterator = serializedProperty.Copy();
            var terminator = serializedProperty.GetEndProperty();
            
            if (!iterator.NextVisible(enterChildren: true)) yield break;
            
            do
            {
                if (SerializedProperty.EqualContents(iterator, terminator)) yield break;
                yield return iterator;
            }
            while (iterator.NextVisible(enterChildren: false));
        }

        public static T GetValue<T>(this SerializedProperty serializedProperty) where T : class
        {
            return GetValue(serializedProperty) as T;
        }

        public static object GetValue(this SerializedProperty serializedProperty)
        {
            return GetValue(serializedProperty.serializedObject.targetObject, serializedProperty.propertyPath);
        }
        
        private static object GetValue(object target, string path)
        {
            var value = target;
            
            var elements = ArrayRegex.Split(path);
            for (var count = 0; count < elements.Length; ++count)
            {
                var element = elements[count];
                switch (count % 3)
                {
                    case 0:
                        var names = element.Split(PathDelimiter);
                        value = names.Aggregate(value, GetValueInternal);
                        break;
                    case 1:
                        var name = element;
                        var index = System.Convert.ToInt32(elements[++count]);
                        value = GetArrayValueInternal(value, name, index);
                        break;
                    case 2:
                        Debug.LogError("Invalid");
                        break;
                }
            }

            return value;
        }

        private static object GetValueInternal(object target, string name)
        {
            if (target == null) return null;

            var type = target.GetType();
            
            while (type != null)
            {
                var fieldInfo = type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                {
                    return fieldInfo.GetValue(target);
                }

                var propertyInfo = type.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (propertyInfo != null)
                {
                    return propertyInfo.GetValue(target, null);
                }

                type = type.BaseType;
            }
            
            return null;
        }

        private static object GetArrayValueInternal(object source, string name, int index)
        {
            if (!(GetValueInternal(source, name) is IEnumerable enumerable)) return null;

            var enumerator = enumerable.GetEnumerator();

            for (var count = 0; count <= index; count++)
            {
                if (!enumerator.MoveNext()) return null;
            }

            return enumerator.Current;
        }
    }
}