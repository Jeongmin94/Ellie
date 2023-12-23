using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Static;
using UnityEngine;

namespace Assets.Scripts.UI.Status
{
    public class UIStatusBar : UIStatic
    {
        private const string SubItemStatus = "Status";

        private GameObject statusPanel;
        private readonly Stack<UIStatus> statusStack = new();

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(GameObjects));

            statusPanel = GetGameObject((int)GameObjects.StatusPanel);
        }

        // !TODO: status 전달 받아서 Sprite 이미지 찾은 다음에 Status 추가해주기 
        public void AddStatus()
        {
            if (statusStack.Count > 6)
            {
                return;
            }

            var status = UIManager.Instance.MakeSubItem<UIStatus>(statusPanel.transform, SubItemStatus);

            statusStack.Push(status);
        }

        public void RemoveStatus()
        {
            if (!statusStack.Any())
            {
                return;
            }

            var status = statusStack.Pop();
            ResourceManager.Instance.Destroy(status.gameObject);
        }

        private enum GameObjects
        {
            StatusPanel
        }
    }
}