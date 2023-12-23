using Assets.Scripts.UI.Framework.Billboard;
using UnityEngine;

namespace Assets.Scripts.UI.Monster
{
    public class UIMonsterBillboard : UIMonsterCanvas, IBillboard
    {
        public float scaleFactor = 0.001f;
        private Camera mainCamera;

        private Transform target;

        private void Awake()
        {
            Init();

            mainCamera = Camera.main;
        }

        private void Update()
        {
            UpdateBillboard();
        }

        public void InitBillboard(Transform parent)
        {
            target = parent;
        }

        public void UpdateBillboard()
        {
            monsterPanelRect.position = mainCamera.WorldToScreenPoint(target.position);
        }
    }
}