using Assets.Scripts.UI.Framework.Billboard;
using UnityEngine;

namespace Assets.Scripts.UI.Monster
{
    public class UIMonsterBillboard : UIMonsterCanvas, IBillboard
    {
        public float scaleFactor = 0.001f;

        private Transform target;
        private Camera mainCamera;

        private void Awake()
        {
            Init();

            mainCamera = Camera.main;
        }

        public void InitBillboard(Transform parent)
        {
            target = parent;
        }

        public void UpdateBillboard()
        {
            monsterPanelRect.position = mainCamera.WorldToScreenPoint(target.position);
        }

        private void Update()
        {
            UpdateBillboard();
        }
    }
}