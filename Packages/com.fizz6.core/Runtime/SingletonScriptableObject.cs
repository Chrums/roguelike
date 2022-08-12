using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fizz6.Core
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
    {
        private static T _instance;
        public static T Instance => _instance == null
            ? _instance = Resources.Load(typeof(T).Name, typeof(T)) as T
            : _instance;
    }
}