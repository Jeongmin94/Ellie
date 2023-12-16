using Sirenix.OdinInspector;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class Singleton<T> : SerializedMonoBehaviour where T : Component
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject();
                        go.name = typeof(T).Name;
                        instance = go.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(instance);
                Debug.Log($"{instance} µî·Ï", instance);
            }
            else if (instance != this)
            {
                Debug.Log($"{gameObject} ÆÄ±«", gameObject);
                Debug.Log($"{instance.gameObject} À¯Áö", instance.gameObject);

                Destroy(gameObject);
            }
            else
            {
                Debug.Log($"{instance}½Ì±ÛÅæ ÆÄ±« ¿À·ù", Instance);
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }

        public virtual void ClearAction()
        {
        }
    }
}