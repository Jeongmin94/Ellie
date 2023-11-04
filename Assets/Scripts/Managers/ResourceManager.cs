using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        private const string PrefixExtern = "Extern/";

        private const string PrefixPrefabs = "Prefabs/";

        public GameObject Instantiate(string path, Transform parent = null)
        {
            var prefab = Resources.Load($"{PrefixPrefabs}{path}") as GameObject;

            if (prefab == null)
            {
                Debug.LogError($"Failed to Load Prefab: {path}");
                return null;
            }

            var go = GameObject.Instantiate(prefab, parent);
            go.name = prefab.name;
            return go;
        }

        public T LoadExternResource<T>(string path) where T : Object
        {
            T resource = Resources.Load<T>($"{PrefixExtern}{path}");
            
            return resource;
        }

        public void Destroy(GameObject go)
        {
            if (go == null)
                return;

            GameObject.Destroy(go);
        }
    }
}