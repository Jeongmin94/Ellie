using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts.UI.Framework
{
    public abstract class UIBase : MonoBehaviour
    {
        // Key: Enum
        // Value: UI Object List
        private readonly Dictionary<Type, List<Object>> objects = new Dictionary<Type, List<Object>>();

        protected abstract void Init();

        /// <summary>
        /// T에 해당하는 Object 들을 objects에 바인딩
        /// type은 사용자가 정의해서 사용할 enum을 의미함
        /// </summary>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        protected void Bind<T>(Type type) where T : Object
        {
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{type} is not an Enum type");
            }

            string[] names = Enum.GetNames(type);
            List<Object> objectList = new List<Object>();

            foreach (var enumName in names)
            {
                Object childComponent = null;
                if (typeof(T) == typeof(GameObject))
                {
                    childComponent = gameObject.FindChild(enumName, true);
                }
                else
                {
                    childComponent = gameObject.FindChild<T>(enumName, true);
                }

                if (childComponent == null)
                    Debug.Log($"{enumName} is missing");
                else
                    objectList.Add(childComponent);
            }

            if (objectList.Any())
                objects[typeof(T)] = objectList;
        }

        protected T Get<T>(int idx) where T : Object
        {
            if (objects.TryGetValue(typeof(T), out var list))
            {
                return list[idx] as T;
            }
            else
            {
                return null;
            }
        }

        protected GameObject GetGameObject(int idx)
        {
            return Get<GameObject>(idx);
        }

        protected Button GetButton(int idx)
        {
            return Get<Button>(idx);
        }

        protected Image GetImage(int idx)
        {
            return Get<Image>(idx);
        }

        protected TextMeshProUGUI GetText(int idx)
        {
            return Get<TextMeshProUGUI>(idx);
        }

        public static void SetSprite(Image image, Sprite sprite)
        {
            image.sprite = sprite;
        }

        public static void SetText(TextMeshProUGUI textMeshPro, string text)
        {
            textMeshPro.text = text;
        }
    }
}