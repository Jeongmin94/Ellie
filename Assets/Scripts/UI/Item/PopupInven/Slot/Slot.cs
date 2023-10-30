using Assets.Scripts.UI.Framework;
using UnityEngine;

namespace Assets.Scripts.UI.Item.PopupInven
{
    public class Slot : UIBase
    {
        private enum GameObjects
        {
            ItemPosition
        }

        public Transform ItemPosition => itemPosition.transform;
        private GameObject itemPosition;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            // !TODO

            Bind<GameObject>(typeof(GameObjects));

            itemPosition = GetGameObject((int)GameObjects.ItemPosition);
        }
    }
}