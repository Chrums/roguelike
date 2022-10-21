using UnityEngine;
using UnityEngine.SceneManagement;

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
                        hideFlags = HideFlags.NotEditable | 
                                    HideFlags.DontSave
                    };
                    
                    DontDestroyOnLoad(_singletonGameObject);
                }

                return _singletonGameObject;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            Destroy(_singletonGameObject);
        }
    }
    
    public abstract class SingletonMonoBehaviour<T> : SingletonMonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T _instance;
        public static T Instance => _instance == null
            ? _instance = SingletonGameObject.AddComponent<T>()
            : _instance;

        protected virtual void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
        
        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode) {}
        protected virtual void OnSceneUnloaded(Scene scene) {}

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            Destroy(_instance);
        }
    }
}