using UnityEngine;

namespace Assets.Scripts.Monsters.Utility
{
    public class Functions : MonoBehaviour
    {
        public static GameObject FindHighestParent(GameObject gameObject)
        {
            var parent = gameObject.transform;
            while (parent.parent != null)
            {
                parent = parent.parent;
            }

            return parent.gameObject;
        }

        public static GameObject FindParentByName(GameObject gameObject, string parentName)
        {
            var parent = gameObject.transform;
            while (parent.parent != null)
            {
                parent = parent.parent;
                Debug.Log(parent.name);
                if (parent.name == parentName)
                {
                    return parent.gameObject;
                }
            }

            return null;
        }

        public static GameObject FindChildByName(GameObject gameObject, string childName)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.name == childName)
                {
                    return child.gameObject;
                }

                var result = FindChildByName(child.gameObject, childName);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}