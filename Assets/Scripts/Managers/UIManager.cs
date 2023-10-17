using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class UIManager: Singleton<UIManager>
    {
        private readonly string NAME_UI_ROOT = "@UI_Root";
        private int sortOrder = 10;
        
        // !TODO: Popup UI 게임 오브젝트 추가(GameObject => PopupUI)
        private Stack<GameObject> popupUIStack = new Stack<GameObject>();

        public GameObject Root
        {
            get
            {
                GameObject root = GameObject.Find(NAME_UI_ROOT);
                if (root == null)
                    root = new GameObject(NAME_UI_ROOT);

                return root;
            }
        }
    }
}