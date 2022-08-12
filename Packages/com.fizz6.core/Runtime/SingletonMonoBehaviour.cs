using UnityEngine;

namespace Fizz6
{
    public abstract class SingletonMonoBehaviour : MonoBehaviour
    {
        private const string SingletonGameObjectName = "[SingletonMonoBehaviours]";
        
        private static GameObject _singletonGameObject;

        protected static GameObject SingletonGameObject
        {
            get
            {
                if (_singletonGameObject == null)
                {
                    _singletonGameObject = new GameObject(SingletonGameObjectName)
                    {
                        hideFlags = HideFlags.DontSave
                    };
                }

                return _singletonGameObject;
            }
        }
    }
    
    public abstract class SingletonMonoBehaviour<T> : SingletonMonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T _instance;
        public static T Instance => _instance == null
            ? _instance = SingletonGameObject.AddComponent<T>()
            : _instance;

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}