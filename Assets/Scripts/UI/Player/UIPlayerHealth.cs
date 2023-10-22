using System;
using Assets.Scripts.Data.ActionData.Player;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Static;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Player
{
    public class UIPlayerHealth : UIStatic
    {
        [SerializeField] private PlayerHealthData healthData;

        private enum Images
        {
            HealthImage
        }

        private enum GameObjects
        {
            HealthPanel
        }

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind<Image>(typeof(Images));
            Bind<GameObject>(typeof(GameObjects));

            var panel = GetGameObject((int)GameObjects.HealthPanel);
            foreach (Transform child in panel.transform)
            {
                ResourceManager.Instance.Destroy(child.gameObject);
            }
        }
    }
}