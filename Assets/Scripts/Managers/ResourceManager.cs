using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        private const string PrefixPrefabs = "/Prefabs";

        public GameObject Instantiate(string path, Transform parent = null)
        {
            var prefab = Resources.Load($"{PrefixPrefabs}/{path}") as GameObject;

            if (prefab == null)
            {
                Debug.LogError($"Failed to Load Prefab: {path}");
                return null;
            }

            var go = GameObject.Instantiate(prefab, parent);
            return go;
        }

        public void Destroy(GameObject go)
        {
            if (go == null)
                return;
            
            GameObject.Destroy(go);
        }
    }
}